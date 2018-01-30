using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexWPF
{
   public static class RepositoryFactor
   {
      public static ISimplexRepository GetRepository(string repositoryType)
      {
         ISimplexRepository repo = null;
         switch (repositoryType)
         {
            case "SQL": repo = new SQLRepository();
               break;
            case "CSV": repo = new CVSRepository();
               break;
            default: throw new ArgumentException("Tipo de repositorio invalido!");
               break;
         }
         return repo;
      }
   }
}
