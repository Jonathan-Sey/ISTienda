using Microsoft.EntityFrameworkCore;
using Tienda.Models;

namespace Tienda
{
    //Heredamos la clase  DbContext de EntityFrameworkCore esto para evitar errores
    public class ApplicationDbContext:DbContext
    {
        // Cracion de un constructor, esto para resivir la conexion de la base de datos
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options)
        :base(options)
        {
            
        }
        // CRUD
        // Creacion de dbSets para cada modelo
        public DbSet<Usuario> Usuarios{get;set;} = null!;
        public DbSet<Rol> Roles{get;set;} = null!;
        public DbSet<Producto> Productos{get;set;} = null!;
        public DbSet<Pedido> Pedidos{get;set;} = null!;
        public DbSet<Direccion> Direcciones{get;set;} = null!;
        public DbSet<Detalle_Pedido> DetallePedidos{get;set;} = null!;
        public DbSet<Categoria> Categorias{get;set;} = null!;

        // Configuracion de relaciones y restricciones de nuestras tablas 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Relacion Pedidos => usuarios
            modelBuilder.Entity<Usuario>()
            .HasMany(u=>u.Pedidos)
            .WithOne(p=>p.Usuario)
            .HasForeignKey(p=>p.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
            
            // Relacion Detalle pedidos => Producto
            modelBuilder.Entity<Producto>()
            .HasMany(p=>p.DetallesPedido)
            .WithOne(p=>p.Producto)
            .HasForeignKey(dp=>dp.ProductoId)
            .OnDelete(DeleteBehavior.Cascade);

            // Relacion Pedido => detalle pedido 
              modelBuilder.Entity<Pedido>()
            .HasMany(p=>p.DetallesPedido)
            .WithOne(dp=>dp.Pedido)
            .HasForeignKey(dp=>dp.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pedido>()
            .Ignore(p=>p.Direccion);

            // Relacion Categoria => productos
            modelBuilder.Entity<Categoria>()
            .HasMany(c=>c.Productos)
            .WithOne(p=>p.Categoria)
            .HasForeignKey(p=>p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}