using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexWPF
{
   public static class DynamicRepositoryFactor
   {
      public static ISimplexRepository GetRepository()
      {
         string typeName = ConfigurationManager.AppSettings["RepositoryType"];
      }
   }
}
