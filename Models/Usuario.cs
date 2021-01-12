using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Models
{
    public class Usuario
    {
        [DisplayName("CODIGO")]
        public int COD_USUARIO { get; set; }
        public int COD_PROVINCIA { get; set; }
        public string PROVINCIA { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El campo cédula debe tener mínimo 10 dígitos")]
        [Required(ErrorMessage = "La cédula debe ser ingresada")]
        public string CEDULA { get; set; }
        public string DIGITO { get; set; }
        public string LOGEO { get; set; }
        [StringLength(20, MinimumLength = 10, ErrorMessage = "El campo clave debe tener mínimo 10 dígitos")]
        public string CLAVE { get; set; }
        public string NOMBRE { get; set; }
        public int COD_ROL { get; set; }
        public string ROL { get; set; }
        [DisplayName("HABILITADO")]
        public bool ESTADO { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "El email debe ser ingresado")]
        public string MAIL { get; set; }
        [Required(ErrorMessage = "El teléfono debe ser ingresado")]
        public string TELEFONO { get; set; }
    }
}
