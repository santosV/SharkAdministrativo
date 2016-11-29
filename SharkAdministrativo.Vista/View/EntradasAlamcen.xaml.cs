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
using SharkAdministrativo.SDKCONTPAQi;

namespace SharkAdministrativo.Vista.View
{
    /// <summary>
    /// Lógica de interacción para EntradasAlamcen.xaml
    /// </summary>
    public partial class EntradasAlamcen : Window
    {
        int CerrarNuevo;
        Proveedor proveedor = new Proveedor();
        Presentacion presentacion = new Presentacion();
        Almacen almacen = new Almacen();
        public EntradasAlamcen()
        {
            InitializeComponent();
            loadComboBoxes();
        }

        private void loadComboBoxes()
        {
            List<Almacen> almacenes = almacen.obtenerTodos();
            foreach (var storage in almacenes)
            {
                cbxAlmacenes.Items.Add(storage.nombre);
            }
            List<Presentacion> presentaciones = presentacion.getAll();
            foreach (var presentation in presentaciones)
            {
                cbxPresentaciones.Items.Add(presentation.descripcion);
            }
        }

        private void save()
        {
            if (cbxPresentaciones.SelectedItem != null && cbxAlmacenes.SelectedItem != null && !String.IsNullOrEmpty(txtCantidad.Text))
            {


                double folio = 0;

                StringBuilder serie = new StringBuilder(12);

                SDK.fSiguienteFolio("21", serie, ref folio);


                SDK.tDocumento lDocto = new SDK.tDocumento();

                lDocto.aCodConcepto = "21";
                lDocto.aCodigoAgente = "(Ninguno)";
                lDocto.aNumMoneda = 1;
                lDocto.aTipoCambio = 1;

                //obtiene el codigo del proveedor
                Presentacion pre = presentacion.get(cbxPresentaciones.SelectedItem.ToString());
                Proveedor pro = proveedor.obtenerPorID(pre.proveedor_id);
                lDocto.aCodigoCteProv = pro.codigo;

                lDocto.aImporte = 0;
                lDocto.aDescuentoDoc1 = 0;
                lDocto.aDescuentoDoc2 = 0;
                lDocto.aAfecta = 0;
                lDocto.aSistemaOrigen = 205;
                lDocto.aFolio = folio;
                lDocto.aSistemaOrigen = 205;
                lDocto.aSerie = "";
                lDocto.aGasto1 = 0;
                lDocto.aGasto2 = 0;
                lDocto.aGasto3 = 0;
                lDocto.aFecha = DateTime.Today.ToString("MM/dd/yyyy");
                int lError = 0;
                Int32 lIdDocumento = 0;
                lError = SDK.fAltaDocumento(ref lIdDocumento, ref lDocto);

                if (lError != 0)
                {
                    SDK.rError(lError);
                    clearFields();
                    return;
                }

                SDK.tMovimiento ltMovimiento = new SDK.tMovimiento();
                int lIdMovimiento = 0;

                SDK.fBuscaAlmacen(pre.almacen_id.ToString());
                StringBuilder codigo = new StringBuilder(20);
                SDK.fLeeDatoAlmacen("CCODIGOALMACEN", codigo, 20);
                ltMovimiento.aCodAlmacen = codigo.ToString();
                ltMovimiento.aConsecutivo = 1;
                ltMovimiento.aCodProdSer = pre.codigo;

                ltMovimiento.aUnidades = Double.Parse(txtCantidad.Text);


                ltMovimiento.aCosto = Double.Parse(Convert.ToString(pre.costo_unitario));
                ltMovimiento.aPrecio = Double.Parse(Convert.ToString(pre.costo_unitario));

                lError = 0;
                lError = SDK.fAltaMovimiento(lIdDocumento, ref lIdMovimiento, ref ltMovimiento);

                if (lError != 0)
                {
                    SDK.rError(lError);
                    clearFields();
                    return;
                }
                else
                {
                    //entrada almacen shark
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

                if (CerrarNuevo == 1)
                {
                    this.Close();
                }


            }
        }

        private void clearFields()
        {
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

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            CerrarNuevo = 1;
            save();
        }

        private void btnNuevo(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            clearFields();
        }

    }
}
