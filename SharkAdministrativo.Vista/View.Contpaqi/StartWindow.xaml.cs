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

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
             if (txtRutaEmpresa.Text != @"C:\Compac\Empresas")
            {
                SDK.companyRoute = txtRutaEmpresa.Text;
                StringBuilder sMensaje = new StringBuilder(512);
                //Indica el directorio activo.

                //Indica el directorio activo.
                SDK.SetCurrentDirectory(SDK.systemRoute);

                int lResult = SDK.fSetNombrePAQ(SDK.systemName);
                if (lResult != 0)
                {
                    SDK.rError(lResult);
                    Console.WriteLine(sMensaje.ToString());

                }
                else {
                    MessageBox.Show("Entramos al directorio");
                    openCompany(txtRutaEmpresa.Text);
                    closeCompany();
                    closeSession();
                    MessageBox.Show("Se inicio todo bien");
                }
            }          
        }

        public void openCompany(string directory)
        {
            //función que abre la empresa.
            int error = SDK.fAbreEmpresa(directory);
            if (error != 0)
            {
                SDK.rError(error);
            }
            else
            {
                Console.WriteLine("Se abrió la empresa ubicada en: {0}", directory);
            }
        }

        public void closeCompany()
        {
            //Función que cierra la empresa.
            SDK.fCierraEmpresa();
            Console.WriteLine("Se cerró la empresa");
        }

        /// <summary>
        /// Cierra la sesión del SDK.
        /// </summary>
        /// <param name="system"></param>

        public void closeSession()
        {
            //Termina Cierra el SDK.
            SDK.fTerminaSDK();
        }

    }
}
