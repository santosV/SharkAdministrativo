using SharkAdministrativo.SDKCONTPAQi;
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
using System.Windows.Shapes;


namespace SharkAdministrativo.Vista.View.Contpaqi
{
    /// <summary>
    /// Lógica de interacción para StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            txtRutaEmpresa.Text = @"C:\Compac\Empresas";
        }

        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog carpeta = new System.Windows.Forms.FolderBrowserDialog();
            carpeta.Description = "Seleccione la Empresa";
            carpeta.SelectedPath = @"C:\Compac\Empresas\";
            carpeta.ShowDialog();
            txtRutaEmpresa.Text = carpeta.SelectedPath;

        }

        /// <summary>
        /// Accede o niega el acceso al sistema, probando la conexión con contpaqi comercial.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
             if (txtRutaEmpresa.Text != @"C:\Compac\Empresas")
            {
                
                SDK.companyRoute = txtRutaEmpresa.Text; 
                SDK.SetCurrentDirectory(SDK.systemRoute);

                int lResult = 0;// SDK.fSetNombrePAQ(SDK.systemName);
                if (lResult != 0)
                {

                    StringBuilder sMensaje = new StringBuilder(512);
                    SDK.rError(lResult);
                    Console.WriteLine(sMensaje.ToString());

                }
                else {
                    //SDK.fTerminaSDK();
                    /*
                    int error = SDK.fAbreEmpresa(txtRutaEmpresa.Text);
                    if (error != 0)
                    {
                        SDK.rError(error);
                    }
                    else
                    {
                        SDK.fCierraEmpresa();
                        SDK.fTerminaSDK();
                       
                       
                    }
                    */
                    MainWindow view = new MainWindow();
                    SDK.companyName = txtRutaEmpresa.Text.Remove(0, 19);
                    view.lblEmpresa.Text = "@" + SDK.companyName;
                    view.Show();

                    this.Close();
                    
                }
                
            }          
        }

      

       

    }
}
