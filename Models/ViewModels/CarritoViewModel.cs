namespace Tienda.Models.ViewModels
{
    public class CarritoViewModel
    {
        // creacion de lista, esto para almacenar los productos y el total a pagar
        public List<CarritoItemViewModel> Items {get; set;} = new List<CarritoItemViewModel>();
        public decimal Total { get; set; }

    }
}