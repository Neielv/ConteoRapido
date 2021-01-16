using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.ViewModels.Reportes
{
    public class AOperadoresParroquia
    {
        [DisplayName("CODIGO DE PARROQUIA")]
        public int PCODIGO { get; set; }
        public string PARROQUIA { get; set; }
        public AOperadoresCanton operadoresCanton { get; set; }
    }
}
