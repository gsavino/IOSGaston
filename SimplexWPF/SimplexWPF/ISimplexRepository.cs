using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexWPF
{
   public interface ISimplexRepository
   {
      void AddEcuacion(Ecuacion nuevaEcuacion);
      IEnumerable<Ecuacion> GetEcuacion();
      Ecuacion GetTermino();
      void DeleteTermino(int ID);
   }
}
