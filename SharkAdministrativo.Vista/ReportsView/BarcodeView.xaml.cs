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
    /// Lógica de interacción para BarcodeView.xaml
    /// </summary>
    public partial class BarcodeView : Window
    {
        public BarcodeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Carga el reporte de código de barra de entradas.
        /// </summary>
        /// <param name="id"></param>
        public void loadReport(int id)
        {
            DataReports.Barcode report = new DataReports.Barcode();
            report.loadParameters(id);
            barcodeViewer.DocumentSource = report;
        
           
            report.CreateDocument();
        }
    }
}
