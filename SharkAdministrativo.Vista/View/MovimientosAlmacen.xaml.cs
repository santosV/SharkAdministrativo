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
    /// Lógica de interacción para MovimientosAlmacen.xaml
    /// </summary>
    public partial class MovimientosAlmacen : Window
    {
        Tipo_movimiento movimiento = new Tipo_movimiento();
        Insumo insumo = new Insumo();
        Unidad_Medida unidad = new Unidad_Medida();
        Almacen almacen = new Almacen();
        Presentacion preIns = new Presentacion();

        public MovimientosAlmacen()
        {
            InitializeComponent();
            loadMovements();
            loadPresentations();
        }



        /// <summary>
        /// Carga la información necesaria para cada combobox como almacenes, tipos de movimientos, etc.
        /// </summary>
        private void loadMovements()
        {
            List<Tipo_movimiento> movimientos = movimiento.obtenerTodos();
            List<Insumo> insumos = insumo.obtenerTodos();
            List<Almacen> almacenes = almacen.obtenerTodos();

            foreach (var mov in movimientos)
            {
                cbxMovimiento.Items.Add(mov.nombre);
            }
            foreach (var insu in insumos)
            {
                cbxInsumo.Items.Add(insu.descripcion);
            }
            foreach (var storage in almacenes)
            {
                cbxAlamcenAfectado.Items.Add(storage.nombre);
                cbxAOrigen.Items.Add(storage.nombre);
                cbxADestino.Items.Add(storage.nombre);
            }
        }

        /// <summary>
        /// Obtiene el insumo que fué seleccionado y obtener sus datos requeridos para la creación del movimiento.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxInsumo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbxInsumo.SelectedItem != null)
            {
                this.insumo = insumo.obtener(cbxInsumo.SelectedItem.ToString());
                unidad = unidad.obtenerPorId(insumo.unidad_id);
                txtMedida.Text = unidad.nombre;
            }
        }

        /// <summary>
        /// Limpia los campos de movmientos.
        /// </summary>
        private void clearFields()
        {
            txtCantidad.Clear();
            txtDescripcion.Clear();
            txtMedida.Text = "...";
            txtRazon.Clear();
            cbxADestino.SelectedItem = null;
            cbxAlamcenAfectado.SelectedItem = null;
            cbxAOrigen.SelectedItem = null;
            cbxInsumo.SelectedItem = null;
            cbxPresentaciones.SelectedItem = null;
            cbxMovimiento.SelectedItem = null;
        }

        private void esconderComboBoxes()
        {
            vista_Alamcenes.Visibility = Visibility.Collapsed;
            vista_salida.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Provee la vista dependiendo el movimiento seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMovimiento_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            esconderComboBoxes();
            if (cbxMovimiento.SelectedItem != null)
            {
                if (cbxMovimiento.SelectedItem.ToString() == "TRASPASO")
                {
                    vista_Alamcenes.Visibility = Visibility.Visible;
                    vista_almacenAfectado.Visibility = Visibility.Collapsed;
                    vista_salida.Visibility = Visibility.Collapsed;
                }
                else if (cbxMovimiento.SelectedItem.ToString() == "SALIDA")
                {
                    vista_salida.Visibility = Visibility.Visible;
                }
                else
                {
                    vista_Alamcenes.Visibility = Visibility.Collapsed;
                    vista_almacenAfectado.Visibility = Visibility.Visible;

                }
            }
            else
            {

                vista_almacenAfectado.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Llama el método limpiar campos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNuevo_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            clearFields();
        }

        private void loadPresentations()
        {
            Presentacion presentacion = new Presentacion();
            List<Presentacion> presentaciones = presentacion.getAll();
            foreach (var presentation in presentaciones)
            {
                cbxPresentaciones.Items.Add(presentation.descripcion);
            }
        }

        private void btnguardarMovimiento_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (cbxMovimiento.SelectedItem != null)
            {
                Tipo_movimiento tipo = new Tipo_movimiento();
                Presentacion presentacion = new Presentacion();
                Salida_almacen salida = new Salida_almacen();
                Insumo _insumo = insumo.obtener(cbxInsumo.SelectedItem.ToString());


                double totalExistencia = 0;
                double folio = 0;
                StringBuilder serie = new StringBuilder(12);

                salida.cantidad = Double.Parse(txtCantidad.Text);
                salida.Insumo = _insumo;
                salida.Tipo_movimiento = tipo.obtener(cbxMovimiento.SelectedItem.ToString());
                salida.descripcion = txtDescripcion.Text;


                if (cbxAlamcenAfectado.SelectedItem != null)
                {
                    salida.Almacen = almacen.obtener(cbxAlamcenAfectado.SelectedItem.ToString());
                }
                else if (cbxAOrigen.SelectedItem != null)
                {
                    salida.Almacen = almacen.obtener(cbxAOrigen.SelectedItem.ToString());
                }
                List<Presentacion> presentaciones = presentacion.obtenerPorInsumoAlmacen(salida.Insumo.id, salida.Almacen.id);
                foreach (var item in presentaciones)
                {
                    totalExistencia += Double.Parse(Convert.ToString(item.existencia));
                }
                if (cbxMovimiento.SelectedItem.ToString() == "SALIDA")
                {
                    //sdk
                    SDK.fSiguienteFolio("35", serie, ref folio);
                    Int32 lIdDocumento = crearDocumento("35", folio);

                    if (cbxAlamcenAfectado.SelectedItem != null && cbxInsumo.SelectedItem != null && !String.IsNullOrEmpty(txtCantidad.Text) && !String.IsNullOrEmpty(txtDescripcion.Text))
                    {
                        //proceso para una salida de almacen
                        salidaAlmacen(totalExistencia, presentaciones, lIdDocumento, salida,salida.Almacen.codigo);
                    }
                    
                    clearFields();
                }
                else
                {
                    SDK.fSiguienteFolio("35", serie, ref folio);

                    if (cbxPresentaciones.SelectedItem != null && cbxInsumo.SelectedItem != null && cbxAOrigen.SelectedItem != null)
                    {

                        if (totalExistencia >= Convert.ToDouble(txtCantidad.Text))
                        {

                            salida.descripcion = tipo.nombre;
                            Int32 lIdDocumentoSalida = crearDocumento("35", folio);

                            salidaAlmacen(totalExistencia, presentaciones, lIdDocumentoSalida, salida, salida.Almacen.codigo);

                            if (salida.id > 0)
                            {
                                SDK.fSiguienteFolio("34", serie, ref folio);

                                EntradaPresentacion entrada = new EntradaPresentacion();
                                DateTime thisDay = DateTime.Today;
                                entrada.fecha_registro = Convert.ToDateTime(thisDay.ToString());
                                entrada.Presentacion = presentacion.get(cbxPresentaciones.SelectedItem.ToString());
                                entrada.Almacen = almacen.obtener(cbxADestino.SelectedItem.ToString());
                                entrada.cantidad = Double.Parse(txtCantidad.Text);

                                Int32 lIdDocumentoEntrada = crearDocumento("34", folio);

                                if (movimientoAlmacen(entrada.Presentacion, lIdDocumentoEntrada, Double.Parse(txtCantidad.Text), entrada.Almacen.codigo) == true)
                                {
                                    entrada.registrar(entrada);
                                    MessageBox.Show("SE TRASPASÓ: " + txtCantidad.Text + " " + unidad.nombre + "\nDEL INSUMO: " + _insumo.descripcion + "\nDEL ALMACÉN: " + cbxAOrigen.SelectedItem.ToString() + "\nAL ALMACÉN: " + cbxADestino.SelectedItem.ToString());
                                    clearFields();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("LA CANTIDAD QUE DESEA MARCAR COMO SALIDA ES MAYOR AL NÚMERO DE EXISTENCIA EN SU ALMACÉN");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Es Necesario especificar el tipo de movimiento que desea hacer");
            }
        }

        public bool movimientoAlmacen(Presentacion presentacion, Int32 lIdDocumento, double unidad,string codigoAlmacen)
        {


            SDK.tMovimiento ltMovimiento = new SDK.tMovimiento();
            int lIdMovimiento = 0;


            ltMovimiento.aCodAlmacen = codigoAlmacen;
            ltMovimiento.aConsecutivo = 1;
            ltMovimiento.aCodProdSer = presentacion.codigo;

            ltMovimiento.aUnidades = unidad;


            ltMovimiento.aCosto = 0;


            int lError = 0;
            lError = SDK.fAltaMovimiento(lIdDocumento, ref lIdMovimiento, ref ltMovimiento);

            if (lError != 0)
            {
                SDK.rError(lError);
                return false;
            }

            return true;

        }

        public Int32 crearDocumento(String concepto, double folio)
        {
            SDK.tDocumento lDocto = new SDK.tDocumento();
            lDocto.aCodConcepto = concepto;
            lDocto.aTipoCambio = 0;

            lDocto.aImporte = 0;
            lDocto.aDescuentoDoc1 = 0;
            lDocto.aDescuentoDoc2 = 0;
            lDocto.aAfecta = 1;
            lDocto.aSistemaOrigen = 205;
            // lDocto.aCodigoCteProv = "(Ninguno)";
            lDocto.aFolio = folio;
            lDocto.aSistemaOrigen = 205;
            lDocto.aSerie = "";

            lDocto.aFecha = DateTime.Today.ToString("MM/dd/yyyy");
            int lError = 0;
            Int32 lIdDocumento = 0;
            lError = SDK.fAltaDocumento(ref lIdDocumento, ref lDocto);

            if (lError != 0)
            {
                SDK.rError(lError);

            }

            return lIdDocumento;
        }

        public void salidaAlmacen(double totalExistencia, List<Presentacion> presentaciones, Int32 lIdDocumento, Salida_almacen salida,string codigoAlmacen)
        {
            bool success = true;
            if (totalExistencia >= Convert.ToDouble(txtCantidad.Text))
            {

                int cont = 0;
                double resultado = 0;
                double cantidadSalidaContpaq = 0;
                foreach (var pExistente in presentaciones)
                {
                    cont++;
                    if (cont == 1)
                    {
                        resultado = Double.Parse(Convert.ToString(pExistente.existencia)) - Double.Parse(txtCantidad.Text);

                    }
                    else
                    {
                        resultado = Double.Parse(Convert.ToString(pExistente.existencia)) - resultado;
                    }
                    if (resultado < 0)
                    {
                        if(pExistente.existencia >0){
                            success = movimientoAlmacen(pExistente, lIdDocumento, Double.Parse(Convert.ToString(pExistente.existencia)),codigoAlmacen);
                        }
                        resultado = resultado * -1;
                        pExistente.existencia = 0;
                        cantidadSalidaContpaq = resultado;
                        pExistente.modificar(pExistente);
                    }
                    else
                    {
                        pExistente.existencia = resultado;

                        if (cont == 1)
                        {
                            success = movimientoAlmacen(pExistente, lIdDocumento, Double.Parse(txtCantidad.Text),codigoAlmacen);
                        }
                        else
                        {
                            success = movimientoAlmacen(pExistente, lIdDocumento, cantidadSalidaContpaq,codigoAlmacen);
                        }

                        pExistente.modificar(pExistente);
                        break;
                    }
                }

                if (success == true)
                {
                    salida.registrar(salida);
                    MessageBox.Show("SE REGISTRO LA SALIDA DE: " + salida.cantidad + " " + unidad.nombre + " \nDEL INSUMO: " + cbxInsumo.SelectedItem.ToString() + "\nPOR LA RAZÓN: " + salida.descripcion, "AVISO SHARK");
                }

            }
            else
            {
                MessageBox.Show("LA CANTIDAD QUE DESEA MARCAR COMO SALIDA ES MAYOR AL NÚMERO DE EXISTENCIA EN SU ALMACÉN");
            }
        }

    }
}
