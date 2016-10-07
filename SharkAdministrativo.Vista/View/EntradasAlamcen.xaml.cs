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

namespace SharkAdministrativo.Vista.View
{
    /// <summary>
    /// Lógica de interacción para EntradasAlamcen.xaml
    /// </summary>
    public partial class EntradasAlamcen : Window
    {
        Presentacion presentacion = new Presentacion();
        Almacen almacen = new Almacen();
        public EntradasAlamcen()
        {
            InitializeComponent();
            loadComboBoxes();
        }

        private void loadComboBoxes() {
            List<Almacen> almacenes = almacen.obtenerTodos();
            foreach (var storage in almacenes)
            {
                cbxAlmacenes.Items.Add(storage.nombre);
            }
            List<Presentacion> presentaciones = presentacion.getAll();
            foreach (var presentation in presentaciones)
            {
                cbxPresentaciones.Items.Add(presentation.descripcion );
            }
        }

        private void save() {
            if (cbxPresentaciones.SelectedItem!=null && cbxAlmacenes.SelectedItem!=null && !String.IsNullOrEmpty(txtCantidad.Text))
            {
                EntradaPresentacion entrada = new EntradaPresentacion();
                DateTime thisDay = DateTime.Today;
                entrada.fecha_registro = Convert.ToDateTime(thisDay.ToString());
                entrada.Presentacion = presentacion.get(cbxPresentaciones.SelectedItem.ToString());
                entrada.Almacen = almacen.obtener(cbxAlmacenes.SelectedItem.ToString());
                entrada.cantidad = Convert.ToDouble(txtCantidad.Text);
                entrada.registrar(entrada);
                presentacion.sumarEntrada(entrada.Presentacion.id, Convert.ToDouble(entrada.cantidad));
                MessageBoxResult dialogResult = MessageBox.Show("Se registró con exito la entrada de almacén, ¿Desea obtener el código de barras para monitorear dicha entrada?", "Confirmación", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    ReportsView.BarcodeView vista = new ReportsView.BarcodeView();
                    vista.loadReport(entrada.id);
                    vista.Show();
                }
                clearFields();
            }
        }

        private void clearFields() {
            cbxAlmacenes.SelectedItem = null;
            cbxPresentaciones.SelectedItem = null;
            txtCantidad.Clear();
                
        }

        private void btnSave_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            save();
        }

        private void btnInputsReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ReportsView.InputsView vista = new ReportsView.InputsView();
            vista.Show();

        }

    }
}
