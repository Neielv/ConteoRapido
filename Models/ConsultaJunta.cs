using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Models
{
    public class ConsultaJunta
    {
        public int COD_PROVINCIA { get; set; }
        public int COD_CANTON { get; set; }
        public int COD_PARROQUIA { get; set; }
        public int COD_ZONA { get; set; }
        public char SEXO { get; set; }
        public int JUNTA { get; set; }
        public int COD_JUNTA { get; set; }
    }
}
