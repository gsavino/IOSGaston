using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorCalculo;

namespace SimplexCSVDataModel
{
   public class CSVSimplexRepository : IModeloRepository
   {
      string path;
      string _tmpfile; 

      public CSVSimplexRepository()
      {
         var filename = ConfigurationManager.AppSettings["CSVFileName"];
         path = AppDomain.CurrentDomain.BaseDirectory + filename;
         _tmpfile = AppDomain.CurrentDomain.BaseDirectory + "TMPModelo.txt";

      }
      public void Add(IModelodata modelo)
      {
         using (StreamWriter file = new StreamWriter(path, true))
         {
            file.WriteLine("Modelo");
            file.WriteLine(modelo.Id.ToString());
            file.WriteLine(modelo.Nombre);
            file.WriteLine(modelo.Objetivo);
            file.WriteLine("FuncionOriginal");
            file.WriteLine(modelo.FuncionOriginal.Id);
            file.WriteLine(modelo.FuncionOriginal.NroEcu);
            file.WriteLine(modelo.FuncionOriginal.Operador);
            file.WriteLine(modelo.FuncionOriginal.ValorDerecho);
            file.WriteLine(modelo.FuncionOriginal.VariableBasica);
            foreach (var t in modelo.FuncionOriginal.Terminos)
            {
               file.WriteLine("FuncionOriginal.Terminos");
               file.WriteLine(t.Id);
               file.WriteLine(t.Valor);
               file.WriteLine(t.Variable);
            }
            file.WriteLine("FuncionAOptimizar");
            file.WriteLine(modelo.FuncionAOptimizar.Id);
            file.WriteLine(modelo.FuncionAOptimizar.NroEcu);
            file.WriteLine(modelo.FuncionAOptimizar.Operador);
            file.WriteLine(modelo.FuncionAOptimizar.ValorDerecho);
            file.WriteLine(modelo.FuncionAOptimizar.VariableBasica);
            foreach (var t in modelo.FuncionAOptimizar.Terminos)
            {
               file.WriteLine("FuncionAOptimizar.Terminos");
               file.WriteLine(t.Id);
               file.WriteLine(t.Valor);
               file.WriteLine(t.Variable);
            }
            foreach (var e in modelo.Ecuaciones)
            {
               file.WriteLine("Ecuacion");
               file.WriteLine(e.Id);
               file.WriteLine(e.NroEcu);
               file.WriteLine(e.Operador);
               file.WriteLine(e.ValorDerecho);
               file.WriteLine(e.VariableBasica);
               file.WriteLine(e.Preparada);
               foreach (var t in e.Terminos)
               {
                  file.WriteLine("Ecuacion.Terminos");
                  file.WriteLine(t.Id);
                  file.WriteLine(t.Valor);
                  file.WriteLine(t.Variable);
               }
            }
         }
      }
      public IModelo Get(string nombre)
      {
         IModelo result = new Modelo(nombre);
         using(StreamReader file = new StreamReader(path, true))
         {
            bool encontrado = false;
            var d = file.ReadLine();
            while (!file.EndOfStream && !encontrado)
            {
               if (d == "Modelo")
               {
                  result.Id = Guid.Parse(file.ReadLine());
                  result.Nombre = file.ReadLine();
                  if (result.Nombre == nombre)
                  {
                     encontrado = true;
                     result.Objetivo = file.ReadLine();
                     var next = file.ReadLine();
                     if (next == "FuncionOriginal")
                     {
                        result.FuncionOriginal.Id = Guid.Parse(file.ReadLine());
                        result.FuncionOriginal.NroEcu = int.Parse(file.ReadLine());
                        result.FuncionOriginal.Operador = file.ReadLine();
                        result.FuncionOriginal.ValorDerecho = decimal.Parse(file.ReadLine());
                        result.FuncionOriginal.VariableBasica = file.ReadLine();
                        next = file.ReadLine();
                        while (next == "FuncionOriginal.Terminos")
                        {
                           var ter = new Termino();
                           ter.Id = Guid.Parse(file.ReadLine());
                           ter.Valor = decimal.Parse(file.ReadLine());
                           ter.Variable = file.ReadLine();
                           result.FuncionOriginal.Terminos.Add(ter);
                           next = file.ReadLine();
                        }
                     }
                     if (next == "FuncionAOptimizar")
                     {
                        result.FuncionAOptimizar.Id = Guid.Parse(file.ReadLine());
                        result.FuncionAOptimizar.NroEcu = int.Parse(file.ReadLine());
                        result.FuncionAOptimizar.Operador = file.ReadLine();
                        result.FuncionAOptimizar.ValorDerecho = decimal.Parse(file.ReadLine());
                        result.FuncionAOptimizar.VariableBasica = file.ReadLine();
                        next = file.ReadLine();
                        while (next == "FuncionAOptimizar.Terminos")
                        {
                           var ter = new Termino();
                           ter.Id = Guid.Parse(file.ReadLine());
                           ter.Valor = decimal.Parse(file.ReadLine());
                           ter.Variable = file.ReadLine();
                           result.FuncionAOptimizar.Terminos.Add(ter);
                           next = file.ReadLine();
                        }
                     }
                     if (next == "Ecuacion")
                     {
                        while (next == "Ecuacion")
                        {
                           var ec = new Ecuacion("T", 0, 0);
                           ec.Id = Guid.Parse(file.ReadLine());
                           ec.NroEcu = int.Parse(file.ReadLine());
                           ec.Operador = file.ReadLine();
                           ec.ValorDerecho = decimal.Parse(file.ReadLine());
                           ec.VariableBasica = file.ReadLine();
                           ec.Preparada = bool.Parse(file.ReadLine());
                           next = file.ReadLine();
                           while (next == "Ecuacion.Terminos")
                           {
                              var t = new Termino();
                              t.Id = Guid.Parse(file.ReadLine());
                              t.Valor = decimal.Parse(file.ReadLine());
                              t.Variable = file.ReadLine();
                              ec.Add(t);
                              next = file.ReadLine();
                           }
                           result.Ecuaciones.Add(ec);
                        }
                     }
                  } else
                  {
                     d = file.ReadLine();
                  }
               }
               else
               {
                  d = file.ReadLine();
               }
            }
            return result;
         }
      }
      public IModelo Get(Guid id)
      {
         IModelo result = new Modelo("tmp");
         using (StreamReader file = new StreamReader(path, true))
         {
            bool encontrado = false;
            var d = file.ReadLine();
            while (!file.EndOfStream && !encontrado)
            {
               if (d == "Modelo")
               {
                  result.Id = Guid.Parse(file.ReadLine());
                  result.Nombre = file.ReadLine();
                  if (result.Id == id)
                  {
                     encontrado = true;
                     result.Objetivo = file.ReadLine();
                     var next = file.ReadLine();
                     if (next == "FuncionOriginal")
                     {
                        result.FuncionOriginal.Id = Guid.Parse(file.ReadLine());
                        result.FuncionOriginal.NroEcu = int.Parse(file.ReadLine());
                        result.FuncionOriginal.Operador = file.ReadLine();
                        result.FuncionOriginal.ValorDerecho = decimal.Parse(file.ReadLine());
                        result.FuncionOriginal.VariableBasica = file.ReadLine();
                        next = file.ReadLine();
                        while (next == "FuncionOriginal.Terminos")
                        {
                           var ter = new Termino();
                           ter.Id = Guid.Parse(file.ReadLine());
                           ter.Valor = decimal.Parse(file.ReadLine());
                           ter.Variable = file.ReadLine();
                           result.FuncionOriginal.Terminos.Add(ter);
                           next = file.ReadLine();
                        }
                     }
                     if (next == "FuncionAOptimizar")
                     {
                        result.FuncionAOptimizar.Id = Guid.Parse(file.ReadLine());
                        result.FuncionAOptimizar.NroEcu = int.Parse(file.ReadLine());
                        result.FuncionAOptimizar.Operador = file.ReadLine();
                        result.FuncionAOptimizar.ValorDerecho = decimal.Parse(file.ReadLine());
                        result.FuncionAOptimizar.VariableBasica = file.ReadLine();
                        next = file.ReadLine();
                        while (next == "FuncionAOptimizar.Terminos")
                        {
                           var ter = new Termino();
                           ter.Id = Guid.Parse(file.ReadLine());
                           ter.Valor = decimal.Parse(file.ReadLine());
                           ter.Variable = file.ReadLine();
                           result.FuncionAOptimizar.Terminos.Add(ter);
                           next = file.ReadLine();
                        }
                     }
                     if (next == "Ecuacion")
                     {
                        while (next == "Ecuacion")
                        {
                           var ec = new Ecuacion("T", 0, 0);
                           ec.Id = Guid.Parse(file.ReadLine());
                           ec.NroEcu = int.Parse(file.ReadLine());
                           ec.Operador = file.ReadLine();
                           ec.ValorDerecho = decimal.Parse(file.ReadLine());
                           ec.VariableBasica = file.ReadLine();
                           ec.Preparada = bool.Parse(file.ReadLine());
                           next = file.ReadLine();
                           while (next == "Ecuacion.Terminos")
                           {
                              var t = new Termino();
                              t.Id = Guid.Parse(file.ReadLine());
                              t.Valor = decimal.Parse(file.ReadLine());
                              t.Variable = file.ReadLine();
                              ec.Add(t);
                              next = file.ReadLine();
                           }
                           result.Ecuaciones.Add(ec);
                        }
                     }
                  }
                  else
                  {
                     d = file.ReadLine();
                  }
               }
               else
               {
                  d = file.ReadLine();
               }
            }
            return result;
         }
      }
      public void Modify(IModelodata modelo)
      {
         this.Remove(modelo.Nombre);
         this.Add(modelo);
      }
      public void Remove(string nombre)
      {
         using (StreamWriter sw = new StreamWriter(_tmpfile, true))
         using(StreamReader sr = new StreamReader(path, true))
         {
            bool copio = true;
            string name = "";
            string id = "";
            var linea = sr.ReadLine();
            while (!sr.EndOfStream)
            {
               if (linea == "Modelo")
               {
                  id = sr.ReadLine();
                  name = sr.ReadLine();
                  if (name == nombre)
                  {
                     copio = false;
                  } else
                  {
                     copio = true;
                     sw.WriteLine(linea);
                     sw.WriteLine(id);
                     sw.WriteLine(name);
                  } 
               } else
               {
                  if (copio) sw.WriteLine(linea);             
               }
               linea = sr.ReadLine();
            }
         }
         File.Delete(path);
         File.Move(_tmpfile, path);
      }
      public void Remove(Guid id)
      {
         using (StreamWriter sw = new StreamWriter(_tmpfile, true))
         using (StreamReader sr = new StreamReader(path, true))
         {
            bool copio = true;
            string name = "";
            string ide = "";
            var linea = sr.ReadLine();
            while (!sr.EndOfStream)
            {
               if (linea == "Modelo")
               {
                  ide = sr.ReadLine();
                  name = sr.ReadLine();
                  if (ide == id.ToString())
                  {
                     copio = false;
                  }
                  else
                  {
                     copio = true;
                     sw.WriteLine(linea);
                     sw.WriteLine(id);
                     sw.WriteLine(name);
                  }
               }
               else
               {
                  if (copio) sw.WriteLine(linea);
               }
               linea = sr.ReadLine();
            }
         }
         File.Delete(path);
         File.Move(_tmpfile, path);
      }
   }
}
