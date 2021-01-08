using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Models
{
    public class Parroquia
    {
        [Key]
        public int Cod_Parroquia { get; set; }
        public int Cod_Canton { get; set; }
        public string Nom_Parroquia { get; set; }
    }
}
