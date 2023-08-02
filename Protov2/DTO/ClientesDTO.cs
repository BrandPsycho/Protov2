using System.ComponentModel.DataAnnotations;

namespace Protov2.DTO
{
    public class ClientesDTO
    {
        [Key]
        public int id_cliente { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string? nombre_cliente { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string? apellido_cliente { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string? telefono_cliente { get; set; }
    }
}
