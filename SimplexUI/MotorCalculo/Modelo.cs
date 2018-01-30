using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MotorCalculo
{
   public class Modelo : IModelo
   {
      public Modelo(string nombre, string objetivo = "Max")
      {
         Nombre = nombre;
         Objetivo = objetivo;
         FuncionAOptimizar = new Ecuacion("ZOp", 0, -2);
         FuncionOriginal = new Ecuacion("FO", 0, -1);
         Ecuaciones = new LEcuaciones();
      }
      [Key]
      public Guid Id { get; set; }
      public LEcuaciones Ecuaciones { get; set; }
      public Ecuacion FuncionOriginal { get ; set; }
      public Ecuacion FuncionAOptimizar { get ; set; }
      private int _i = 1;
      private string nuevoZ;
      private LTerminos listTempTerminos = new LTerminos();
      public string Objetivo { get; set; }
      public string Nombre { get; set; }
      
      public void Add(Ecuacion ecuacion)
      {
         Ecuaciones.Add(ecuacion);
      }
      public void AddFuncion(Ecuacion funcion)
      {
         FuncionOriginal = funcion;
      }
      public string MostrarModelo()
      {
         string outPut="";
         outPut += "Modelo :" + Nombre + "\rObjetivo :" + Objetivo + "\r"+"********************\r";
         foreach (var item in Ecuaciones)
         {        
            foreach (var ter in item.MostrarEcuacion())
            {
               if (ter.Valor < 0)
               {
                  outPut += ter.Valor + ter.Variable;
               } else
               {
                  outPut += "+" + ter.Valor + ter.Variable;
               }
            }
            outPut += " = " + item.ValorDerecho + "\r";
         };
         outPut += "\r Función a Optimizar Z \r";
         foreach (var zitem in FuncionAOptimizar.ListoVariables())
         {
            if (FuncionAOptimizar.Get(zitem).Valor < 0)
            {
               outPut += FuncionAOptimizar.Get(zitem).Valor + FuncionAOptimizar.Get(zitem).Variable;
            }
            else
            {
               outPut += "+" + FuncionAOptimizar.Get(zitem).Valor + FuncionAOptimizar.Get(zitem).Variable;
            }
         }
         outPut += "\r\r Resultado de variables: \r\r";
         foreach (var item in Ecuaciones)
         {
            outPut += "VarBasica :" + item.VariableBasica + " : " + item.ValorDerecho + "\r";
         }
         outPut += "\r\rResultado Optimizado(Z):"+ FuncionAOptimizar.ValorDerecho + "\r";
         outPut += "********************";
         return outPut;
      }
      public void Preparo()
      {
         //Le agrego la función identidad y conformo los diferentes 
         //vectores para completar el modelo
         //Creo todos los Y
         var listvari = ListoVariables();
         for (var i = 1; i <= Ecuaciones.Count; i++)
         {
            nuevoZ = "Y" + i.ToString();
            listTempTerminos.Add(new Termino(nuevoZ, 0));
         }
         // Agrego los Y a las ecuaciones, pero dejando en 1 el valor de la ecuación actual
         foreach (var ec in Ecuaciones)
         {
            // Agrego los X que falten
            foreach (var t in listvari)
            {
               if (!ec.IsByVariable(t))
               {
                  ec.Add(new Termino(t, 0));
               }
            }
            // Agrego matrix identidad
            foreach (var nz in listTempTerminos)
            {
               if (ec.NroEcu == _i)
               {
                  ec.Add(new Termino(nz.Variable, 1));
                  ec.VariableBasica = nz.Variable;
               }
               else
               {
                  ec.Add(nz);
               }
               _i++;
            }
            // Agrego los Z
            ec.Add(new Termino("Z", 0));
            _i = 1;
            // Marco la ecuación como preparada
            ec.Preparada = true;
         }
         //Preparo la función a Optimizar
         FuncionAOptimizar.Add(new Termino("Z", 1));
         FuncionAOptimizar.VariableBasica = "Z";
         foreach (var z in FuncionOriginal.ListoVariables())
         {
            FuncionAOptimizar.Add(new Termino(z,(FuncionOriginal.Get(z).Valor*-1)));
         }
         // Agrego las variables de la matriz identidad en la función a optimizar.
         foreach (var iltt in listTempTerminos)
         {
            FuncionAOptimizar.Add(new Termino(iltt.Variable, iltt.Valor * -1));
         }
      }
      private IEnumerable<string> ListoVariables()
      {
         var listoVar = new List<string>();
         foreach (var ecus in Ecuaciones)
         {
            listoVar.AddRange(ecus.ListoVariables());
         }
         var result = (from r in listoVar
                       select r).Distinct().OrderBy(r=>r);
         return result;
      }
      public void Proceso()
      {
         string colpiv;
         int ecupiv;
         while (Itero())
         {
            colpiv = FuncionAOptimizar.GetMenor().Variable;
            ecupiv = BuscoEcuacionPivote(colpiv);
            ProcesoModelo(ecupiv, colpiv);
            ReemplazoSalientePorEntrante(ecupiv, colpiv);
         }
      }
      private void ReemplazoSalientePorEntrante(int ecuacionSaliente, string variableEntrante)
      {
         var ecua = (from e in Ecuaciones
                     where e.NroEcu == ecuacionSaliente
                     select e).SingleOrDefault();
         ecua.VariableBasica = variableEntrante;
      }
      private void ProcesoModelo(int ecupiv, string colpiv)
      {
         IEcuacion ecuPivoteTmp = null;
         // Proceso las ecuaciones
         ecuPivoteTmp = ProcesoPivote(ecupiv, colpiv, ecuPivoteTmp);
         ProcesoEcuacionesNoPivote(ecupiv, colpiv, ecuPivoteTmp);
         // Proceso Z
         FuncionAOptimizar.ValorDerecho = FuncionAOptimizar.ValorDerecho - FuncionAOptimizar.Get(colpiv).Valor * ecuPivoteTmp.ValorDerecho;
         FuncionAOptimizar.ProcesoNoPivote(FuncionAOptimizar.Get(colpiv).Valor, ecuPivoteTmp);
      }
      private void ProcesoEcuacionesNoPivote(int ecupiv, string colpiv, IEcuacion ecuPivoteTmp)
      {
         foreach (var ecuresto in Ecuaciones)
         {
            if (ecuresto.NroEcu != ecupiv)
            {
               ecuresto.ValorDerecho = ecuresto.ValorDerecho - ecuresto.Get(colpiv).Valor * ecuPivoteTmp.ValorDerecho;
               ecuresto.ProcesoNoPivote(ecuresto.Get(colpiv).Valor, ecuPivoteTmp);
            }
         }
      }
      private IEcuacion ProcesoPivote(int ecupiv, string colpiv, IEcuacion ecuPivoteTmp)
      {
         foreach (var ecua in Ecuaciones)
         {
            if (ecua.NroEcu == ecupiv)
            {
               var valPivote = ecua.Get(colpiv);
               ecua.ValorDerecho = ecua.ValorDerecho / valPivote.Valor;
               ecua.ProcesoPivote(valPivote.Valor);
               ecuPivoteTmp = ecua;
            }
         }
         return ecuPivoteTmp;
      }
      private int BuscoEcuacionPivote(string cp)
      {
         int point = 1;
         int ecu = 0;
         decimal min = Decimal.MaxValue;
         foreach (var bcp in Ecuaciones)
         {
            if (bcp.Get(cp).Valor != 0 && (min >= (bcp.ValorDerecho / bcp.Get(cp).Valor)))
            {
              min = (bcp.ValorDerecho / bcp.Get(cp).Valor);
              ecu = point;
            }
            point++;
         }
         return ecu;
      }
      private bool Itero()
      {
         // Verifico si existe algún número negativo en Z
         return (FuncionAOptimizar.GetMenor().Valor < 0);
      }
   }
}
