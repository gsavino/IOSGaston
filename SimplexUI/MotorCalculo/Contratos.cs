using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MotorCalculo.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorCalculo
{
   public interface IModeloRepository
   {
      IModelo Get(string nombre);
      IModelo Get(Guid id);
      void Add(IModelodata modelo);
      void Modify(IModelodata modelo);
      void Remove(string nombre);
      void Remove(Guid id);
   }
   public interface IModelo : IModelodata
   {

      void MostrarModelo();
      void Add(Ecuacion ecuacion);
      void AddFuncion(Ecuacion funcion);
      void Preparo();
      void Proceso();
   }
   public interface IModelodata 
   {
      [Key]
      Guid Id { get; set; }
      string Objetivo { get; set; }
      string Nombre { get; set; }
      LEcuaciones Ecuaciones { get; set; }
      Ecuacion FuncionOriginal { get; set; }
      Ecuacion FuncionAOptimizar { get; set; }
   }
   public interface IEcuacion : IEcuacionData 
   {
      
      void Add(ITermino termino);
      LTerminos MostrarEcuacion();
      List<string> ListoVariables();
      ITermino Get(string var);
      bool IsByVariable(string var);
      ITermino GetMenor();
      void ProcesoPivote(decimal valorPivote);
      void ProcesoNoPivote(decimal Coef, IEcuacion ecuPivoteTmp);
   }
   public interface IEcuacionData 
   {
      [Key]
      Guid Id { get; set; }
      int NroEcu { get; set; }
      bool Preparada { get; set; }
      string Operador { get; set; }
      decimal ValorDerecho { get; set; }
      string VariableBasica { get; set; }
      LTerminos Terminos { get; set; }
   }
   public interface ITermino 
   {
      [Key]
      Guid Id { get; set; }
      decimal Valor { get; set; }
      string Variable { get; set; }
   }
}
