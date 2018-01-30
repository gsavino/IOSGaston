using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotorCalculo;

namespace SimplexTesting
{
   [TestClass]
   public class TesteoTerminos
   {
      [TestMethod]
      public void TestTermino()
      {
         //Creo dos terminos
         Termino ter = new Termino("X1",5);
         Termino ter2 = new Termino("X2", 3);
         //Creo la ecuación
         Ecuacion ecu = new Ecuacion(">=", 120);
         //Agrego los terminos a la ecuación
         ecu.Add(ter);
         ecu.Add(ter2);
         //Creo un modelo
         Modelo primerModelo = new Modelo("Primer Modelo Simplex");
         //Agrego la ecuacion al modelo
         primerModelo.Add(ecu);
         ecu.MostrarEcuacion();
         Assert.AreEqual("Max", primerModelo.Objetivo);
      }
   }
}
