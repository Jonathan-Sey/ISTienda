using Tienda.Models;

namespace Tienda.Services
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> GetCategorias();

    }
}