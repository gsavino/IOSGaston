using MotorCalculo;
using SimplexDataModel;
using SimplexCSVDataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimplexUI
{
   public partial class Form1 : Form
   {
      public Form1()
      {
         InitializeComponent();
      }

      private void Btn_Procesar_Click(object sender, EventArgs e)
      {
         //Creo dos terminos
         Termino ter = new Termino("X1", 2);
         Termino ter2 = new Termino("X2", 3);
         Termino ter3 = new Termino("X1", 2);
         Termino ter4 = new Termino("X2", 1);
         Termino ter5 = new Termino("X2", 4);
         Termino terz1 = new Termino("X1", 8);
         Termino terz2 = new Termino("X2", 10);
         var Lter = new List<Termino>
         {
            ter,
            ter2
         };
         //Creo la ecuación
         Ecuacion ecu = new Ecuacion(">=", 600, 1);
         Ecuacion ecu2 = new Ecuacion(">=", 500, 2);
         Ecuacion ecu3 = new Ecuacion(">=", 600, 3);

         //Agrego los terminos a la ecuación
         ecu.Add(ter);
         ecu.Add(ter2);
         ecu2.Add(ter3);
         ecu2.Add(ter4);
         ecu3.Add(ter5);
         //Creo un modelo
         Modelo primerModelo = new Modelo("Primer Modelo Simplex");
         //Agrego la ecuacion al modelo
         primerModelo.Add(ecu);
         primerModelo.Add(ecu2);
         primerModelo.Add(ecu3);
         primerModelo.FuncionOriginal.Operador = "FOrig";
         primerModelo.FuncionOriginal.ValorDerecho = 0;
         primerModelo.FuncionOriginal.NroEcu=-1;
         primerModelo.FuncionOriginal.Terminos.Add(terz1);
         primerModelo.FuncionOriginal.Terminos.Add(terz2);
         primerModelo.MostrarModelo();
         primerModelo.Preparo();
         //
         //traigo un modelo desde SQL
         //
         //IModeloRepository CSVRepository = new CSVSimplexRepository();
         //ShowRepositoryType(CSVRepository);
         //CSVRepository.Add(primerModelo);
         //primerModelo.Nombre = "Segundo Modelo Simplex";
         //CSVRepository.Add(primerModelo);
         //primerModelo.Nombre = "Tercer Modelo Simplex";
         //CSVRepository.Add(primerModelo);
         //var output = CSVRepository.Get("Segundo Modelo Simplex");
         //CSVRepository.Remove("Segundo Modelo Simplex");
         IModeloRepository SQLRepository = new SQLSimplexRepository();
         ShowRepositoryType(SQLRepository);
         primerModelo.Nombre = "Modelo Simplex Preparado";
         SQLRepository.Add(primerModelo);
         //var x = SQLRepository.Get("ModeloModificado00");
         //SQLRepository.Remove(x.Id);
         //SQLRepository.DeleteModelo(mdel.Id);
         //primerModelo.Nombre = "Segundo Modelo Simplex";
         //SQLRepository.Add(primerModelo);
         //primerModelo.Nombre = "Tercer Modelo Simplex";
         //SQLRepository.Add(primerModelo);
         primerModelo.MostrarModelo();
         primerModelo.Proceso();
         //SQLRepository.SaveModelo(primerModelo);
         primerModelo.MostrarModelo();
      }
      private void ShowRepositoryType(IModeloRepository repository)
      {
         MessageBox.Show(string.Format("Repository Type:\n{0}", repository.GetType().ToString()));
      }

      private void button1_Click(object sender, EventArgs e)
      {
         IModeloRepository del = new SQLSimplexRepository();
         del.Remove(new Guid("FAB61EEA-11BE-4ABC-9DEC-8FB8079B3EAE"));
         del.Remove(new Guid("22C9813A-B179-4500-AC9D-9A7788808830"));
         del.Remove(new Guid("4E61D414-4146-4E80-BD33-E4EB60DD3AFE"));


      }
   }
}
