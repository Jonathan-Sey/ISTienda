using System.Data.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PayPalCheckoutSdk.Orders;
using Tienda.Models;
using Tienda.Models.ViewModels;

namespace Tienda.Controllers
{
    public class BaseController : Controller
    {   
        // Propiedad de lectura
        public readonly ApplicationDbContext _context;
        // definicion de constructor
        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public override ViewResult View(string? viewName, object? model)
        {
            ViewBag.NumeroProductos=GetCarritoCount();
            return base.View(viewName, model);
        }

        protected int GetCarritoCount()
        {
            int count=0;
            string? carritoJson = Request.Cookies["carrito"];
            if (!string.IsNullOrEmpty(carritoJson))
            {
                var carrito = JsonConvert.DeserializeObject<List<ProductoIdAndCantidad>>(carritoJson);
                if (carrito!=null)
                {
                    count = carrito.Count;
                }
            }

            return count;
        }

        // meotod agregar producto al carrito
        public async Task<CarritoViewModel> AgregarProductoAlCarrito(int productoId, int cantidad)
        {
            var producto = await _context.Productos.FindAsync(productoId);
            if(producto!=null)
            {
                var carritoViewModel= await GetCarritoViewModelAsync();
                var carritoItem= carritoViewModel.Items.FirstOrDefault(
                    item=>item.ProductoId==productoId
                );

                // validar si esta vacio, caso contraro sumar 
                if(carritoItem != null)
                carritoItem.Cantidad += cantidad;
                else{
                    carritoViewModel.Items.Add(
                        new CarritoItemViewModel{
                            ProductoId=producto.ProductoId,
                            Nombre=producto.Nombre,
                            Precio=producto.Precio,
                            Cantidad=cantidad
                        }
                    );
                }

                carritoViewModel.Total = carritoViewModel.Items.Sum(
                    item=>item.Cantidad*item.Precio
                );
                await UpdateCarritoViewModelAsync(carritoViewModel);
                return carritoViewModel;
            }
            return new CarritoViewModel();
        }

        public async Task UpdateCarritoViewModelAsync(CarritoViewModel carritoViewModel)
        {
            var ProductoIds = carritoViewModel.Items.Select(
                item=>new ProductoIdAndCantidad
                {
                    ProductoId=item.ProductoId,
                    Cantidad = item.Cantidad
                }
            )
            .ToList();
            // definicion del limite de expiracion
            var carritoJson= await Task.Run(()=>JsonConvert.SerializeObject(ProductoIds));
            Response.Cookies.Append(
                "carrito",
                carritoJson,
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7)}
            );
        }

        // metodo para obtener los datos actuales

        public async Task<CarritoViewModel> GetCarritoViewModelAsync()
        {
            // variable para almacenar la info
            var carritoJson=Request.Cookies["carrito"];
            // validamos si el Json tiene valores
            if(string.IsNullOrEmpty(carritoJson))
            return new CarritoViewModel();

            // variable
            var productoIdsAndCantidades = JsonConvert.DeserializeObject<List<ProductoIdAndCantidad>>(carritoJson);

            var carritoViewModel= new CarritoViewModel();
            // validar que nuestro producto no sea null
            if (productoIdsAndCantidades !=null){
                foreach (var item in productoIdsAndCantidades)
                {
                    var producto = await _context.Productos.FindAsync(item.ProductoId);
                    if(producto!=null)
                    {
                            carritoViewModel.Items.Add(
                                new CarritoItemViewModel
                                {
                                    ProductoId = producto.ProductoId,
                                    Nombre=producto.Nombre,
                                    Precio=producto.Precio,
                                    Cantidad=item.Cantidad

                                }
                            );
                        }
                    }
                }

                carritoViewModel.Total=carritoViewModel.Items.Sum(item => item.Subtotal);
                return carritoViewModel;
        }

        protected IActionResult HandleError(Exception e)
        {
            return View(
                "Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }

        protected IActionResult HandleDbError(DbException dbException)
        {
            var ViewModel= new DbErrorViewModel{
                ErrorMessage = "Error de base de datos",
                Details = dbException.Message
            };
            return View("DbError" , ViewModel);
        }

        protected IActionResult HandleDbUpdateError(DbUpdateException dbUpdateException)
        {
            var ViewModel= new DbErrorViewModel{
                ErrorMessage = "Error de actualizacion de base de datos",
                Details = dbUpdateException.Message
            };
            return View("DbError" , ViewModel);
        }

    }
}