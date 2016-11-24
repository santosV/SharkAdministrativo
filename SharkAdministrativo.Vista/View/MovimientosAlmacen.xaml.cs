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
        }

        /// <summary>
        /// Carga la información necesaria para cada combobox como almacenes, tipos de movimientos, etc.
        /// </summary>
        private void loadMovements(){
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
            if (cbxInsumo.SelectedItem!=null)
            {
                this.insumo = insumo.obtener(cbxInsumo.SelectedItem.ToString());
                unidad = unidad.obtenerPorId(insumo.unidad_id);
                txtMedida.Text = unidad.nombre;
            }
        }

        /// <summary>
        /// Limpia los campos de movmientos.
        /// </summary>
        private void clearFields() {
            txtCantidad.Clear();
            txtDescripcion.Clear();
            txtMedida.Text = "...";
            txtRazon.Clear();
            cbxADestino.SelectedItem = null;
            cbxAlamcenAfectado.SelectedItem = null;
            cbxAOrigen.SelectedItem = null;
            cbxInsumo.SelectedItem = null;
            cbxMovimiento.SelectedItem = null;
        }

        /// <summary>
        /// Provee la vista dependiendo el movimiento seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMovimiento_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbxMovimiento.SelectedItem != null)
            {
                if (cbxMovimiento.SelectedItem.ToString() == "TRASPASO")
                {
                    vista_Alamcenes.Visibility = Visibility.Visible;
                    vista_almacenAfectado.Visibility = Visibility.Collapsed;
                    vista_salida.Visibility = Visibility.Collapsed;
                }
                else if(cbxMovimiento.SelectedItem.ToString() == "SALIDA")
                {
                    vista_salida.Visibility = Visibility.Visible;
                }
                else
                {
                    vista_Alamcenes.Visibility = Visibility.Collapsed;
                    vista_almacenAfectado.Visibility = Visibility.Visible;

                }
            }
            else {
                vista_Alamcenes.Visibility = Visibility.Collapsed;
                vista_salida.Visibility = Visibility.Collapsed;
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
    }
}
