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
        [Required]
        public string CEDULA { get; set; }
        public string DIGITO { get; set; }
        public string LOGEO { get; set; }
        public string CLAVE { get; set; }
        public string NOMBRE { get; set; }
        public int COD_ROL { get; set; }
        public string ROL { get; set; }
        [DisplayName("HABILITADO")]
        public bool ESTADO { get; set; }
        [EmailAddress]
        public string MAIL { get; set; }
    }
}
