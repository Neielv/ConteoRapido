using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Models
{
    public class Zona
    {
        [Key]
        public int Cod_Zona { get; set; }
        public int Cod_Parroquia { get; set; }
        public string Nom_Zona { get; set; }
    }
}
