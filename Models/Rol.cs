using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Models
{
    public class Rol
    {
        [Key]
        public int COD_ROL { get; set; }
        public string DES_ROL { get; set; }
    }
}
