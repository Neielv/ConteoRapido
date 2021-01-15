using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.ViewModels.Reportes
{
    public class AOperadoresCanton
    {
        public int COD_CANTON { get; set; }
        public string CANTON { get; set; }
        public AOperadoresProvincia operadoresProvincia { get; set; }
    }
}
