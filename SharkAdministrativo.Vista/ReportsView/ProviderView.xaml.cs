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

namespace SharkAdministrativo.Vista.ReportsView
{
    /// <summary>
    /// Lógica de interacción para ProviderView.xaml
    /// </summary>
    public partial class ProviderView : Window
    {
        public ProviderView()
        {
            InitializeComponent();
            loadReport();
        }
        /// <summary>
        /// Carga el reporte de proveedores.
        /// </summary>
        public void loadReport()
        {
            DataReports.ProviderData report = new DataReports.ProviderData();
            providerViewer.DocumentSource = report;
            report.CreateDocument();
        }
    }
}
