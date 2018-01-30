using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorCalculo.Enums;

namespace MotorCalculo
{
    public class Ecuacion : IEcuacion
   {
      public Ecuacion(string operador, decimal valorDerecho, int nroecu = 0)
      {
         NroEcu = nroecu;
         Operador = operador;
         ValorDerecho = valorDerecho;
         Preparada = false;
         Terminos = new LTerminos();
      }
      [Key]
      public Guid Id { get; set; }
      public int Cant { get; set; }
      public int NroEcu { get; set; }
      public bool Preparada { get; set; }
      public string Operador { get; set; }
      public decimal ValorDerecho { get; set; }
      public string VariableBasica { get; set; }
      public LTerminos Terminos { get; set; }

      public void Add(ITermino termino)
      {
         Terminos.Add(termino);
         Cant++;
      }
      public bool IsByVariable(string var)
      {
         var result = (from r in Terminos
                       where r.Variable == var
                       select new { r.Variable }).SingleOrDefault();
         return (result != null);
      }
      public ITermino Get(string var)
      {
         var result = (from r in Terminos
                       where r.Variable == var
                       select r).SingleOrDefault();
         return result;
      }
      public LTerminos MostrarEcuacion()
      {
         return Terminos;
      }
      public List<string> ListoVariables()
      {
         var salida = (from n in Terminos
                      select  n.Variable).Distinct().ToList();
         return salida;
      }
      public ITermino GetMenor()
      {
         var result = (from res in Terminos
                       where res.Valor == (from men in Terminos
                                           select men.Valor).Min()
                       select res).FirstOrDefault();
         return result;
      }
      public void ProcesoPivote(decimal valorPivote)
      {
         foreach (var term in Terminos)
         {
            term.Valor = term.Valor / valorPivote;
         }
      }
      public void ProcesoNoPivote(decimal Coef, IEcuacion ecuPivoteTmp)
      {
         foreach (var term in Terminos)
         {
            var terPivote = ecuPivoteTmp.Get(term.Variable);
            term.Valor = term.Valor - Coef * terPivote.Valor;
         }
      }

   }
}
