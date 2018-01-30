using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexWPF
{
   class SQLRepository : ISimplexRepository
   {
      public void AddEcuacion(Ecuacion nuevaEcuacion)
      {
         var context = new SimplexEntities();
         nuevaEcuacion.Id = BuscoMax();
         context.Ecuacions.Add(nuevaEcuacion);
         context.SaveChanges();
      }

      public void DeleteTermino(int ID)
      {
         var context = new SimplexEntities();
         var borrarTermino = context.Ecuacions.Find(ID);
         
         context.Ecuacions.Remove(borrarTermino);

         context.SaveChanges();
      }

      public IEnumerable<Ecuacion> GetEcuacion()
      {
         var ecu = new SimplexEntities();
         var qry = from r in ecu.Ecuacions
                   select r;
         return qry;
      }

      public Ecuacion GetTermino()
      {
         throw new NotImplementedException();
      }
      private int BuscoMax()
      {
         var context = new SimplexEntities();
         var max = (from m in context.Ecuacions
                    select m.Id).Max();
         return max + 1;
      }
   }
}
