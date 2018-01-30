using System;
using System.Data.Entity;
using System.Data.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MotorCalculo;


namespace SimplexDataModel
{
   public class SQLSimplexRepository : IModeloRepository
   {
      public IModelo Get(string nombre)
      {
         var model = new Modelo("tmp");
         using (var dbmod = new SimplexContext())
         {
            var q = dbmod.Modelos.Include(n => n.FuncionAOptimizar)
                                 .Include(n => n.FuncionOriginal)
                                 .Include(n => n.Ecuaciones)
                                 .Include(n => n.Ecuaciones.Select(x => x.Terminos))
                                 .Where(n => n.Nombre == nombre).FirstOrDefault();
            dbmod.Entry(q.FuncionAOptimizar).Collection(n => n.Terminos).Load();
            dbmod.Entry(q.FuncionOriginal).Collection(n => n.Terminos).Load();
            CopiarAModelo(q, model);
         }         
         return model;
      }
      public IModelo Get(Guid id)
      {
         var model = new Modelo("tmp");
         using (var dbmod = new SimplexContext())
         {
            var q = dbmod.Modelos.Include(n => n.FuncionAOptimizar)
                               .Include(n => n.FuncionOriginal)
                               .Include(n => n.Ecuaciones)
                               .Include(n => n.Ecuaciones.Select(x => x.Terminos))
                               .Where(n => n.Id == id).FirstOrDefault();
            dbmod.Entry(q.FuncionAOptimizar).Collection(n => n.Terminos).Load();
            dbmod.Entry(q.FuncionOriginal).Collection(n => n.Terminos).Load();
            CopiarAModelo(q, model);
         }
         return model;
      }
      public void Add(IModelodata modelo)
      {
         DBModelo modeloTmp = new DBModelo();
         CopiarADBModelo(modelo, modeloTmp);
         using (var DBCtxt = new SimplexContext())
         {
            DBCtxt.Modelos.Add(modeloTmp);
            DBCtxt.SaveChanges();
         }
      }
      public void Modify(IModelodata modelo)
      {
         Remove(modelo.Id);
         Add(modelo);
         //DBModelo m = new DBModelo();
         //CopiarADBModelo(modelo, m);
         //using (var DBCtxt = new SimplexContext())
         //{
         //   DBCtxt.Modelos.Attach(m);
         //   DBCtxt.Entry(m).State = StateHelper.ConvertState(m.State);
         //   DBCtxt.Entry(m.FuncionAOptimizar).State = StateHelper.ConvertState(m.FuncionAOptimizar.State); 
         //   DBCtxt.Entry(m.FuncionOriginal).State = StateHelper.ConvertState(m.FuncionOriginal.State);
         //   foreach (var t in m.FuncionAOptimizar.Terminos)
         //   {
         //      DBCtxt.Entry(t).State = EntityState.Modified;
         //   }
         //   foreach (var t in m.FuncionOriginal.Terminos)
         //   {
         //      DBCtxt.Entry(t).State = EntityState.Modified;
         //   }
         //   foreach (var e in m.Ecuaciones)
         //   {
         //      foreach (var t in e.Terminos)
         //      {
         //         DBCtxt.Entry(t).State = EntityState.Modified;
         //      }
         //      DBCtxt.Entry(e).State = EntityState.Modified;
         //   }

         //   DBCtxt.SaveChanges();
         //}
      }
      public void Remove(Guid id)
      {
         using (var DBCtxt = new SimplexContext())
         {
            var modelo = Get(id);
            DBModelo q = new DBModelo();
            CopiarADBModelo((IModelodata)modelo, q);
            DBCtxt.Modelos.Attach(q);
            DBCtxt.Ecuaciones.Attach(q.FuncionAOptimizar);
            DBCtxt.Terminos.RemoveRange(q.FuncionAOptimizar.Terminos);
            DBCtxt.Ecuaciones.Remove(q.FuncionAOptimizar);
            DBCtxt.Terminos.RemoveRange(q.FuncionOriginal.Terminos); 
            DBCtxt.Ecuaciones.Remove(q.FuncionOriginal);
            foreach (var e in q.Ecuaciones)
            {
               DBCtxt.Terminos.RemoveRange(e.Terminos);
            }
            DBCtxt.Ecuaciones.RemoveRange(q.Ecuaciones);
            DBCtxt.Modelos.Remove(q);
            DBCtxt.SaveChanges();
         }
      }
      public void Remove(string nombre)
      {
         using (var DBCtxt = new SimplexContext())
         {
            var modelo = Get(nombre);
            DBModelo q = new DBModelo();
            CopiarADBModelo((IModelodata)modelo, q);
            DBCtxt.Modelos.Attach(q);
            DBCtxt.Ecuaciones.Attach(q.FuncionAOptimizar);
            DBCtxt.Terminos.RemoveRange(q.FuncionAOptimizar.Terminos);
            DBCtxt.Ecuaciones.Remove(q.FuncionAOptimizar);
            DBCtxt.Terminos.RemoveRange(q.FuncionOriginal.Terminos);
            DBCtxt.Ecuaciones.Remove(q.FuncionOriginal);
            foreach (var e in q.Ecuaciones)
            {
               DBCtxt.Terminos.RemoveRange(e.Terminos);
            }
            DBCtxt.Ecuaciones.RemoveRange(q.Ecuaciones);
            DBCtxt.Modelos.Remove(q);
            DBCtxt.SaveChanges();
         }
      }

      private static void CopiarADBModelo(IModelodata modelo, DBModelo dbm)
      {
         DBTermino terminoTmp = null;
         DBEcuacion ecuacionTmp = null;
         bool tieneId = modelo.Id != Guid.Empty;
         if (tieneId)
         {
            dbm.Id = modelo.Id;
         }
         else { dbm.Id = Guid.NewGuid(); };
         dbm.Nombre = modelo.Nombre;
         dbm.Objetivo = modelo.Objetivo;
         foreach (var ecu in modelo.Ecuaciones)
         {
            ecuacionTmp = new DBEcuacion();
            foreach (var term in ecu.Terminos)
            {
               terminoTmp = new DBTermino();
               if (tieneId) {
                  terminoTmp.Id = term.Id;
               }
               else { terminoTmp.Id = Guid.NewGuid(); };
               terminoTmp.Valor = term.Valor;
               terminoTmp.Variable = term.Variable;
               ecuacionTmp.Terminos.Add(terminoTmp);
            }
            if (tieneId)
            {
               ecuacionTmp.Id = ecu.Id;
            }
            else { ecuacionTmp.Id = Guid.NewGuid(); };
            ecuacionTmp.NroEcu = ecu.NroEcu;
            ecuacionTmp.Operador = ecu.Operador;
            ecuacionTmp.Preparada = ecu.Preparada;
            ecuacionTmp.ValorDerecho = ecu.ValorDerecho;
            ecuacionTmp.VariableBasica = ecu.VariableBasica;
            dbm.Ecuaciones.Add(ecuacionTmp);
            //ecuacionTmp = new DBEcuacion();
         }
         if (tieneId)
         {
            dbm.FuncionAOptimizar.Id = modelo.FuncionAOptimizar.Id;
         }
         else { dbm.FuncionAOptimizar.Id = Guid.NewGuid(); };
         dbm.FuncionAOptimizar.NroEcu = modelo.FuncionAOptimizar.NroEcu;
         dbm.FuncionAOptimizar.Operador = modelo.FuncionAOptimizar.Operador;
         dbm.FuncionAOptimizar.Preparada = modelo.FuncionAOptimizar.Preparada;
         dbm.FuncionAOptimizar.ValorDerecho = modelo.FuncionAOptimizar.ValorDerecho;
         dbm.FuncionAOptimizar.VariableBasica = modelo.FuncionAOptimizar.VariableBasica;
         foreach (var fAOp in modelo.FuncionAOptimizar.Terminos)
         {
            terminoTmp = new DBTermino();
            if (tieneId)
            {
               terminoTmp.Id = fAOp.Id;
            }
            else { terminoTmp.Id = Guid.NewGuid(); };

            terminoTmp.Valor = fAOp.Valor;
            terminoTmp.Variable = fAOp.Variable;
            dbm.FuncionAOptimizar.Terminos.Add(terminoTmp);
         }
         if (tieneId)
         {
            dbm.FuncionOriginal.Id = modelo.FuncionOriginal.Id;
         }
         else { dbm.FuncionOriginal.Id = Guid.NewGuid(); };

         dbm.FuncionOriginal.NroEcu = modelo.FuncionOriginal.NroEcu;
         dbm.FuncionOriginal.Operador = modelo.FuncionOriginal.Operador;
         dbm.FuncionOriginal.Preparada = modelo.FuncionOriginal.Preparada;
         dbm.FuncionOriginal.ValorDerecho = modelo.FuncionOriginal.ValorDerecho;
         dbm.FuncionOriginal.VariableBasica = modelo.FuncionOriginal.VariableBasica;
         foreach (var fO in modelo.FuncionOriginal.Terminos)
         {
            terminoTmp = new DBTermino();
            if (tieneId)
            {
               terminoTmp.Id = fO.Id;
            }
            else { terminoTmp.Id = Guid.NewGuid(); };

            terminoTmp.Valor = fO.Valor;
            terminoTmp.Variable = fO.Variable;
            dbm.FuncionOriginal.Terminos.Add(terminoTmp);
         }
      }
      private void CopiarAModelo(DBModelo q, Modelo m)
      {

         Ecuacion e = null;
         Termino t = null;
         //Copio Ecuaciones
         foreach (var ec in q.Ecuaciones)
         {
            e = new Ecuacion("x", 0);
            foreach (var ti in ec.Terminos)
            {
               t = new Termino
               {
                  Id = ti.Id,
                  Valor = ti.Valor,
                  Variable = ti.Variable
               };
               e.Terminos.Add(t);
            }
            e.Id = ec.Id;
            e.NroEcu = ec.NroEcu;
            e.Operador = ec.Operador;
            e.Preparada = ec.Preparada;
            e.ValorDerecho = ec.ValorDerecho;
            e.VariableBasica = ec.VariableBasica;
            m.Ecuaciones.Add(e);
            e = new Ecuacion("x", 0);
         }
         m.Id = q.Id;
         m.Nombre = q.Nombre;
         m.Objetivo = q.Objetivo;
         m.FuncionAOptimizar = new Ecuacion("tmp", 0, 0)
         {
            Id = q.FuncionAOptimizar.Id,
            NroEcu = q.FuncionAOptimizar.NroEcu,
            Operador = q.FuncionAOptimizar.Operador,
            Preparada = q.FuncionAOptimizar.Preparada,
            ValorDerecho = q.FuncionAOptimizar.ValorDerecho,
            VariableBasica = q.FuncionAOptimizar.VariableBasica
         };

         foreach (var fAOp in q.FuncionAOptimizar.Terminos)
         {
            t = new Termino
            {
               Id = fAOp.Id,
               Valor = fAOp.Valor,
               Variable = fAOp.Variable
            };
            m.FuncionAOptimizar.Terminos.Add(t);
         }
         //m.funcionOriginal.Id = model.funcionOriginal.Id;
         m.FuncionOriginal = new Ecuacion("tmp", 0, 0)
         {
            Id = q.FuncionOriginal.Id,
            NroEcu = q.FuncionOriginal.NroEcu,
            Operador = q.FuncionOriginal.Operador,
            Preparada = q.FuncionOriginal.Preparada,
            ValorDerecho = q.FuncionOriginal.ValorDerecho,
            VariableBasica = q.FuncionOriginal.VariableBasica
         };
         foreach (var fO in q.FuncionOriginal.Terminos)
         {
            t = new Termino
            {
               Id = fO.Id,
               Valor = fO.Valor,
               Variable = fO.Variable
            };
            m.FuncionOriginal.Terminos.Add(t);
         }
      }
   }
}
