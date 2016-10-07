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

        private void cbxInsumo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbxInsumo.SelectedItem!=null)
            {
                this.insumo = insumo.obtener(cbxInsumo.SelectedItem.ToString());
                unidad = unidad.obtenerPorId(insumo.unidad_id);
                txtMedida.Text = unidad.nombre;
            }
        }

        private void cbxMovimiento_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbxMovimiento.SelectedItem != null)
            {
                if (cbxMovimiento.SelectedItem.ToString() == "TRASPASO")
                {
                    vista_Alamcenes.Visibility = Visibility.Visible;
                    vista_almacenAfectado.Visibility = Visibility.Collapsed;
                }
                else
                {
                    vista_Alamcenes.Visibility = Visibility.Collapsed;
                    vista_almacenAfectado.Visibility = Visibility.Visible;

                }
            }
            else {
                vista_Alamcenes.Visibility = Visibility.Collapsed;
                vista_almacenAfectado.Visibility = Visibility.Visible;
            }
        }
    }
}
