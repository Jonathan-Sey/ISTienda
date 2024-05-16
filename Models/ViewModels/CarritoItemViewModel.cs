namespace Tienda.Models.ViewModels
{
    public class CarritoItemViewModel
    {
        // identificador de productos 

        public int ProductoId { get; set; }

        public Producto Producto { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        public decimal Subtotal => Precio * Cantidad;

    }
}