using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.ViewModels.Reportes
{
    public class AOperadoresCanton
    {
        [DisplayName("CODIGO DE CANTON")]
        public int COD_CANTON { get; set; }
        public string CANTON { get; set; }
        public AOperadoresProvincia operadoresProvincia { get; set; }
    }
}
