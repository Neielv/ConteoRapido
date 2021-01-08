using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Models
{
    public class UsuarioResponse : Usuario
    {
        public int CODIGO_PROVINCIA { get; set; }
        public int CODIGO_ROL { get; set; }
    }
}
