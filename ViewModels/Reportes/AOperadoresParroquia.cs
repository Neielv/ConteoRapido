using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.ViewModels.Reportes
{
    public class AOperadoresParroquia
    {
        public string PARROQUIA { get; set; }
        public AOperadoresCanton operadoresCanton { get; set; }
    }
}
