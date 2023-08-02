using System.ComponentModel.DataAnnotations;

namespace Protov2.DTO
{
    public class UsuariosDTO
    {
        [Key]
        public int id_usuario { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string? correo_elec { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string? contrasena { get; set; }

 
        public string? confirmar_contrasena { get; set; }



    }
}
