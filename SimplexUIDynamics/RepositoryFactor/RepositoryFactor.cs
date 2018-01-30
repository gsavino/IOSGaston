using System;
using System.Configuration;
using MotorCalculo;

namespace RepositoryFactory
{
   public static class RepositoryFactor
    {
      public static IModeloRepository GetRepository()
      {
         //Obtengo el tipo de repositorio del appSettings, donde está la referencia
         //al proyecto que contiene el repositorio para obtener la información
         //necesaria par acargar el assembly
           ////////test de Carga
             //////var DLL = Assembly.LoadFile(@"C:\Users\gsavino\source\repos\SimplexUIDynamics\SimplexUIDynamics\bin\Debug\SimplexCSVDataModel.dll");
             //////foreach (Type type in DLL.GetExportedTypes())
             //////{
             //////   var c = Activator.CreateInstance(type);
             //////   type.InvokeMember("Output", BindingFlags.InvokeMethod, null, c, new object[] { @"Hello" });
             //////}
         string typeName = ConfigurationManager.AppSettings["RepositoryType"];
         ////convertimos el string en un Type.
         Type repoType = Type.GetType(typeName);
         // Creo una instancia de ese tipo, usando reflexion
         object repoInstance = Activator.CreateInstance(repoType);
         // Hago un cast del objeto a la Interfaz IModeloRepository, si el 
         //objeto no cumple con IModeloRepository, devuelve null
         IModeloRepository repo = repoInstance as IModeloRepository;
         return repo;
      }
    }
}
