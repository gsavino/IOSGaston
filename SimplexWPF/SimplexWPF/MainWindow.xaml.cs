using SimplexWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimplexWPF
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public MainWindow()
      {
         InitializeComponent();
      }

      private void txt_output_TextChanged(object sender, TextChangedEventArgs e)
      {

      }

      private void btn_leerModelo_Click(object sender, RoutedEventArgs e)
      {

         FetchData("SQL");

      }

      private void FetchData(string repositoryType)
      {
         ISimplexRepository qry = RepositoryFactor.GetRepository(repositoryType);
         var output = qry.GetEcuacion();

         txt_output.Text = "";
         var oldE = 1;
         foreach (var q in output)
         {
            if (oldE == q.Nro_Ecu)
            {
               txt_output.Text += " + ";
            }
            else
            {

               txt_output.Text += "\r";
               oldE = (int)q.Nro_Ecu;
            }
            txt_output.Text += q.Valor + q.Variable;
         }
      }

      private void Button_Click(object sender, RoutedEventArgs e)
      {
         ISimplexRepository repositorio = new SQLRepository();
         var addEcu = new Ecuacion
         {
            Id = 0,
            Nro_Ecu = 1,
            Variable = "X2",
            Valor = 3
         };
         repositorio.AddEcuacion(addEcu);
      }

      private void DeleteEcu_Click(object sender, RoutedEventArgs e)
      {
         ISimplexRepository repositorio = new SQLRepository();
         repositorio.DeleteTermino(4);
      }

      private void Button_Click_1(object sender, RoutedEventArgs e)
      {
         FetchData("CSV");
      }
   }
}

