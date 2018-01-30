using MotorCalculo;
using System;
using System.Windows.Forms;
using RepositoryFactory;

namespace SimplexUIDynamics
{
   public partial class Form1 : Form
   {
      public Form1()
      {
         InitializeComponent();
         //////var builder = new ContainerBuilder();
      }

      private void Btn_ProcesoModelo_Click(object sender, EventArgs e)
      {
         IModeloRepository repository = RepositoryFactor.GetRepository();
         var modelo = repository.Get("Modelo Simplex Preparado");
         modelo.Proceso();
         ShowModelo(modelo.MostrarModelo());
         ShowRepositoryType(repository);
      }

      private void ShowModelo(string str)
      {
         MessageBox.Show(str);
      }

      private void ShowRepositoryType(IModeloRepository repository)
      {
         MessageBox.Show(string.Format("Repository Type:\n{0}", repository.GetType()));
      }
   }
}
