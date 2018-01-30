using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexWPF
{
   class CVSRepository : ISimplexRepository
   {
      public CVSRepository()
      {
         var filename = "ModeloExcel.csv";
          path = AppDomain.CurrentDomain.BaseDirectory + filename;

      }

      public string path { get; private set; }

      public void AddEcuacion(Ecuacion nuevaEcuacion)
      {
         throw new NotImplementedException();
      }

      public void DeleteTermino(int ID)
      {
         throw new NotImplementedException();
      }

      public IEnumerable<Ecuacion> GetEcuacion()
      {
         var ecuacion = new List<Ecuacion>();
         if (File.Exists(path))
         {
            var sr = new StreamReader(path);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
               var elems = line.Split(',');
               var ecua = new Ecuacion()
               {
                  Id = Int32.Parse(elems[0]),
                  Nro_Ecu = Int32.Parse(elems[1]),
                  Valor = Int32.Parse(elems[2]),
                  Variable = elems[3]
               };
               ecuacion.Add(ecua);
            }
         }
         return ecuacion;
      }

      public Ecuacion GetTermino()
      {
         throw new NotImplementedException();
      }
   }
}
