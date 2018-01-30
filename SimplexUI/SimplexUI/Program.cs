using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using SimplexDataModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimplexUI
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         //Database inicializacion...paso null para no inicializar la base creando una nueva.
         Database.SetInitializer(new NullDatabaseInitializer<SimplexContext>());
         Application.Run(new Form1());
      }
   }
}
