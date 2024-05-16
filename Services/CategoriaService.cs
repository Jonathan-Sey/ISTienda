using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using Tienda.Models;

namespace Tienda.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ApplicationDbContext _context;
        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Categoria>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }
    }
}