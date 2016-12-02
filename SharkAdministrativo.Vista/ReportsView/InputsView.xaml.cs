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
using SharkAdministrativo.Modelo;

namespace SharkAdministrativo.Vista.ReportsView
{
    /// <summary>
    /// Lógica de interacción para InputsView.xaml
    /// </summary>
    public partial class InputsView : Window
    {
        public InputsView()
        {
            InitializeComponent();
            loadReport();
        }
        /// <summary>
        /// Carga el reporte de entradas de almacén.
        /// </summary>
        public void loadReport()
        {
            DataReports.InputData report = new DataReports.InputData();
            inputsViewer.DocumentSource = report;
            report.CreateDocument();
        }
    }
}
