// libreria para el proceso de migracion de tablas como tambien el uso de Data Annotations para realizar validaciones
using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class Rol
    {
        [Key]
       public int RolId {get; set;} 
       // Aplicando reglas de validacion
       [Required(ErrorMessage = "El campo nombre es obligatorio")]
       [StringLength(50)]
       public string Nombre {get; set;} = null!;
    }
}

