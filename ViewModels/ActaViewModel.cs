using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.ViewModels
{
    public class ActaViewModel
    {
        public int CodigoJunta { get; set; }
        public List<SelectListItem> Juntas { get; set; }
        public int CodigoUsuario { get; set; }
        public List<SelectListItem> Usuarios { get; set; }
    }
}
