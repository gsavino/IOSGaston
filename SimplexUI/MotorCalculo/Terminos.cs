using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MotorCalculo.Enums;

namespace MotorCalculo
{
   public class Termino : ITermino 
   {
      [Key]
      public Guid Id { get; set; }
      public Termino(string variable, decimal valor = 0)
      {
         Variable = variable;
         Valor = valor;
      }
      public Termino(decimal valor = 0m) => this.Valor = valor;
      public decimal Valor { get; set; }
      public string Variable { get; set; }
   }
}
