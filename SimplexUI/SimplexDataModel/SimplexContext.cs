using MotorCalculo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using MotorCalculo.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplexDataModel
{
    public class SimplexContext : DbContext
   {
      public DbSet<DBEcuacion> Ecuaciones { get; set; }
      public DbSet<DBModelo> Modelos { get; set; }
      public DbSet<DBTermino> Terminos { get; set; }
    }
   public class DBModelo 
   {
      public DBModelo() 
      {
         Ecuaciones = new List<DBEcuacion>();
         FuncionAOptimizar = new DBEcuacion();
         FuncionOriginal = new DBEcuacion();
      }
      [Key]
      public Guid Id { get; set; }
      public string Objetivo { get; set; }
      public string Nombre { get; set; }
      public List<DBEcuacion> Ecuaciones { get; set; }
      public DBEcuacion FuncionOriginal { get; set; }
      public DBEcuacion FuncionAOptimizar { get; set; }
   }
   public class DBEcuacion 
   {
      public DBEcuacion()
      {
         Terminos = new List<DBTermino>();
      }
      [Key]
      public Guid Id { get; set; }
      public int NroEcu { get; set ; }
      public bool Preparada { get ; set; }
      public string Operador { get ; set; }
      public decimal ValorDerecho { get; set; }
      public string VariableBasica { get; set; }
      public List<DBTermino> Terminos { get; set; }
   }

   public class DBTermino
   {
      public DBTermino()
      {
      }
      [Key]
      public Guid Id { get; set; }
      public decimal Valor { get; set; }
      public string Variable { get; set; }
   }

}
