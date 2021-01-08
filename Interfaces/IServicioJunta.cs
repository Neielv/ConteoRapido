using CoreCRUDwithORACLE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Interfaces
{
    public interface IServicioJunta
    {
        IEnumerable<Junta> ObtenerJuntas();
        Junta GetActa(int iUsuario, int iEstado);
    }
}
