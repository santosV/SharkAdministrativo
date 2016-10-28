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
using SharkAdministrativo.SDKCONTPAQi;

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
            SDKCONTPAQi.
     
           
        }
    }
}
