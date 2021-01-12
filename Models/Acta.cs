using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Models
{
    public class Acta
    {
        [DisplayName("CODIGO DE JUNTA")]
        public int COD_JUNTA { get; set; }
        [DisplayName("CODIGO DE USUARIO")]
        public int COD_USUARIO { get; set; }
        public int VOT_JUNTA { get; set; }
        public int BLA_JUNTA { get; set; }
        public int NUL_JUNTA { get; set; }
    }
}
