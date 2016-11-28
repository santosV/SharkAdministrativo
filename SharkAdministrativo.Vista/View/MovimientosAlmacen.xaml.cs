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
    /// Lógica de interacción para MovimientosAlmacen.xaml
    /// </summary>
    public partial class MovimientosAlmacen : Window
    {
        Tipo_movimiento movimiento = new Tipo_movimiento();
        Insumo insumo = new Insumo();
        Unidad_Medida unidad = new Unidad_Medida();
        Almacen almacen = new Almacen();
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
                if (cbxMovimiento.SelectedItem.ToString() == "SALIDA")
                {
                    if (cbxAlamcenAfectado.SelectedItem != null && cbxInsumo.SelectedItem != null && !String.IsNullOrEmpty(txtCantidad.Text) && !String.IsNullOrEmpty(txtDescripcion.Text))
                    {
                        Salida_almacen salida = new Salida_almacen();
                        Insumo _insumo = insumo.obtener(cbxInsumo.SelectedItem.ToString());

                        salida.Almacen = almacen.obtener(cbxAlamcenAfectado.SelectedItem.ToString());
                        salida.cantidad = Double.Parse(txtCantidad.Text);
                        salida.Insumo = _insumo;
                        salida.Tipo_movimiento = tipo.obtener(cbxMovimiento.SelectedItem.ToString());
                        salida.descripcion = txtDescripcion.Text;
                        List<Presentacion> presentaciones = presentacion.obtenerPorInsumoAlmacen(salida.Insumo.id, salida.Almacen.id);
                        int cont = 0;
                        double resultado = 0;
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
                                resultado = resultado * -1;
                                pExistente.existencia = 0;
                                pExistente.modificar(pExistente);
                            }
                            else
                            {
                                pExistente.existencia = resultado;
                                pExistente.modificar(pExistente);
                                break;
                            }
                        }
                        salida.registrar(salida);
                        if (salida.id > 0)
                        {
                            MessageBox.Show("SE REGISTRO LA SALIDA DE: " + salida.cantidad + " " + unidad.nombre + " \nDEL INSUMO: " + _insumo.descripcion + " POR LA RAZÓN: " + salida.descripcion);
                        }
                    }
                }
                else
                {
                    if (cbxPresentaciones.SelectedItem!=null && cbxInsumo.SelectedItem!=null && cbxAOrigen.SelectedItem!=null)
                    {
                        Salida_almacen salida = new Salida_almacen();
                        Insumo _insumo = insumo.obtener(cbxInsumo.SelectedItem.ToString());

                        salida.Almacen = almacen.obtener(cbxAOrigen.SelectedItem.ToString());
                        salida.cantidad = Double.Parse(txtCantidad.Text);
                        salida.Insumo = _insumo;
                        salida.Tipo_movimiento = tipo.obtener(cbxMovimiento.SelectedItem.ToString());
                        salida.descripcion = tipo.nombre;
                        salida.registrar(salida);

                        if (salida.id > 0)
                        {
                            EntradaPresentacion entrada = new EntradaPresentacion();
                            DateTime thisDay = DateTime.Today;
                            entrada.fecha_registro = Convert.ToDateTime(thisDay.ToString());
                            entrada.Presentacion = presentacion.get(cbxPresentaciones.SelectedItem.ToString());
                            entrada.Almacen = almacen.obtener(cbxADestino.SelectedItem.ToString());
                            entrada.cantidad = Double.Parse(txtCantidad.Text);
                            entrada.registrar(entrada);
                            if (entrada.id > 0)
                            {
                                MessageBox.Show("SE TRASPASÓ: " + txtCantidad.Text + " " + unidad.nombre + "\nDEL INSUMO: " + _insumo.descripcion + "\nDEL ALMACÉN: " + cbxAOrigen.SelectedItem.ToString() + "\nAL ALMACÉN: " + cbxADestino.SelectedItem.ToString());
                                clearFields();
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Es Necesario especificar el tipo de movimiento que desea hacer");
            }
        }
    }
}
