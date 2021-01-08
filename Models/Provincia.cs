using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Models
{
    public class Provincia
    {
        [Key]
        public int COD_PROVINCIA { get; set; }
        public string NOM_PROVINCIA { get; set; }
    }
}
