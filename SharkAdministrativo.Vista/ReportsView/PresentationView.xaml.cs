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
    /// Lógica de interacción para PresentationView.xaml
    /// </summary>
    public partial class PresentationView : Window
    {
        public PresentationView()
        {
            InitializeComponent();
            loadReport();
        }
        /// <summary>
        /// Carga el reporte de Presentación.
        /// </summary>
        public void loadReport()
        {
            DataReports.PresaentationData report = new DataReports.PresaentationData();
            presentationViewer.DocumentSource = report;
            report.CreateDocument();
        }
    }
}
