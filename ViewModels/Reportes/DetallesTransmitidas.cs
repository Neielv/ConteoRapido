using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.ViewModels.Reportes
{
    public class DetallesTransmitidas
    {
        public int CCANTON { get; set; }
        public int CODIGO { get; set; }
        public string PROVINCIA { get; set; }
        public string CANTON { get; set; }
        public string PARROQUIA { get; set; }
        public int COD_JUNTA { get; set; }
        public int JUNTA { get; set; }
        public string SEXO { get; set; }
        public string USUARIO { get; set; }
        public string TELEFONO { get; set; }
        public bool TRANSMITIDAS { get; set; }
        
    }
}
