using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{
    public class Usuario
    {

        // Definicion de constructor Usuario, esto para crear una lista vacia de pedidos 
        public Usuario()
        {
            // Definicion de lista de objetos del tipo pedido
            Pedidos = new List<Pedido>();
        }
      [Key]
      public int UsuarioId { get; set; } 
      [Required] 
      [StringLength(50)]
      public string Nombre { get; set; } = null!;
      [Required] 
      [StringLength(15)]
      public string Telefono { get; set; } = null!;
      [Required] 
      [StringLength(50)]
      public string NombreUsuario { get; set; } = null!;
      [Required] 
      [StringLength(50)]
      public string Contrasenia { get; set; } = null!;
      [Required] 
      [StringLength(50)]
      public string Correo { get; set; } = null!;
      [Required] 
      [StringLength(255)]
      public string Direccion { get; set; } = null!;
      [Required] 
      [StringLength(50)]
      public string Ciudad { get; set; } = null!;
      [Required] 
      [StringLength(20)]
      public string Estado { get; set; } = null!;
      [Required] 
      [StringLength(20)]
      public string CodigoPostal { get; set; } = null!;
      [Required] 
      // Este campo sera nuestra llave foranea 
       public int RolId { get; set; } 
             
       // Para ello realizamos lo siguiente 
       [ForeignKey("RolId")]
       public Rol Rol { get; set;} = null!; 

        // Definimos una coleccion de elementos en este caso de tipo pedidos 
       public ICollection<Pedido> Pedidos {get; set;}
       // Propiedad para relacionar bidireccionalmente la entidad usuario y Direcciones
       // esto a razon de poder mapear mejor la informacion entre estas dos tablas 
       [InverseProperty("Usuario")]
       public ICollection<Direccion> Direcciones {get; set;} = null!;


      
    }
}