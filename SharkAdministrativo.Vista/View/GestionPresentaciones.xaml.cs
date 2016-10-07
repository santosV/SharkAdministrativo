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
using DevExpress.Xpf.Ribbon;
using System.Data;

namespace SharkAdministrativo.Vista
{
    /// <summary>
    /// Lógica de interacción para GestionPresentaciones.xaml
    /// </summary>
    public partial class GestionPresentaciones : Window
    {
        Insumo insumo = new Insumo();
        Proveedor proveedor = new Proveedor();
        Almacen almacen = new Almacen();
        DataTable dtPLista = new DataTable();
        DataTable dtPRegistradas = new DataTable();
        List<Presentacion> presentaciones = new List<Presentacion>();
        Presentacion presentacion = new Presentacion();
        string route = "";
        string hasChanged="No";
        public GestionPresentaciones()
        {
            
            InitializeComponent();
            titlesPresentacionesLista();
            llenarAlmacenes();
            llenarProveedores();
            llenarInsumos();
        }


        public void llenarPresentaciones() {
            dtPLista.Rows.Clear();
            presentaciones = presentacion.obtenerTodosPorInsumo(insumo.id);
            foreach (var item in presentaciones)
            {
                this.presentacion = item;
                dtPLista.Rows.Add(item.id, item.descripcion, item.rendimiento, item.Insumo.Unidad_Medida.nombre, item.Proveedor.nombre); 
            }
        }


        public void llenarAlmacenes() {
            cbxAlmacen.Items.Clear();
            List<Almacen> almacenes = almacen.obtenerTodos();
            cbxAlmacen.Items.Add("Nuevo");
            foreach (var storage in almacenes)
            {
                cbxAlmacen.Items.Add(storage.nombre);
            }
        }

        public void llenarProveedores()
        {
            cbxProveedor.Items.Clear();
            List<Proveedor> proveedores = proveedor.obtenerTodos();
            cbxProveedor.Items.Add("Nuevo");
            foreach (var provee in proveedores)
            {
                cbxProveedor.Items.Add(provee.nombre);
            }
        }

        public void titlesPresentacionesLista()
        {
            dtPLista.Columns.Add("id");
            dtPLista.Columns.Add("Descripción");
            dtPLista.Columns.Add("Rendimiento");
            dtPLista.Columns.Add("Unidad");
            dtPLista.Columns.Add("Proveedor");
            tblPresentaciones.ItemsSource = dtPLista.DefaultView;
            tblPresentaciones.Columns[0].Visible = false;
        }

        public void llenarInsumos() {
            List<Insumo> insumos = insumo.obtenerTodos();
            foreach (var item in insumos)
            {
                cbxInsumoBase.Items.Add(item.descripcion);
            }
        }

        
        public void addInsumo( Insumo insumo){
            route = "InsumosFirst";
            this.insumo = insumo;
            this.presentacion.Insumo = this.insumo;
            indicarInsumoBase();
            cbxInsumoBase.Items.Add(insumo.descripcion);
            cbxInsumoBase.SelectedItem = insumo.descripcion;
        }

        public void indicarInsumoBase() {
            title.Text = "AGREGA LAS PRESENTACIONES DEL INSUMO '" + insumo.descripcion + "'";
            Unidad_Medida unidad = new Unidad_Medida();
            unidad = unidad.obtenerPorId(insumo.unidad_id);
            txtUnidadDeMedida.Text = unidad.nombre;
        }

        private void addPresentacion_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtDescripcion.Text) && cbxInsumoBase.SelectedItem != null && cbxProveedor.SelectedItem != null && cbxAlmacen.SelectedItem != null && !String.IsNullOrEmpty(txtCPromedio.Text) && !String.IsNullOrEmpty(txtCCImpuesto.Text) && !String.IsNullOrEmpty(txtUcosto.Text) && !String.IsNullOrEmpty(txtRendimiento.Text) && !String.IsNullOrEmpty(txtCantidad.Text))
            {
                Presentacion presentacion = new Presentacion();
                presentacion.cantidad = Double.Parse(txtCantidad.Text);
                presentacion.costo_con_impuesto = Double.Parse(txtCCImpuesto.Text);
                presentacion.costo_promedio = Double.Parse(txtCPromedio.Text);
                presentacion.ultimo_costo = Double.Parse(txtUcosto.Text);
                presentacion.rendimiento = Double.Parse(txtRendimiento.Text);
                presentacion.IVA = Double.Parse(txtIVA.Text);
                presentacion.costo_unitario = Double.Parse(txtUcosto.Text);
                presentacion.descripcion = txtDescripcion.Text;
                presentacion.Insumo = insumo.obtener(cbxInsumoBase.SelectedItem.ToString());
                presentacion.Almacen = almacen.obtener(cbxAlmacen.SelectedItem.ToString());
                presentacion.Proveedor = proveedor.obtener(cbxProveedor.SelectedItem.ToString());
                presentacion.existencia = presentacion.rendimiento*presentacion.cantidad;
                presentacion.registrar(presentacion);
                if (Convert.ToDouble(txtCantidad.Text)>0)
                {
                    EntradaPresentacion entrada = new EntradaPresentacion();
                    DateTime thisDay = DateTime.Today;
                    entrada.fecha_registro = Convert.ToDateTime(thisDay.ToString());
                    entrada.Presentacion = presentacion;
                    entrada.Almacen = presentacion.Almacen;
                    entrada.cantidad = presentacion.cantidad;
                    entrada.registrar(entrada);
                }
                hasChanged = "Yes";
                clearFields();
                dtPLista.Rows.Add(presentacion.id, presentacion.descripcion, presentacion.rendimiento, this.presentacion.Insumo.Unidad_Medida.nombre, presentacion.Proveedor.nombre);
                
                
            }
            else {
                MessageBox.Show("¡AVISO! \n > Falta Ingresar Algunos Campos Importantes \n    Es Necesario Ingresarlos");
            }
        }



        private void clearFields() {
            txtCantidad.Clear();
            txtCCImpuesto.Clear();
            txtCPromedio.Clear();
            txtRendimiento.Clear();
            txtUcosto.Clear();
            tblPresentaciones.SelectedItem = false;
            cbxInsumoBase.SelectedItem = insumo.descripcion;
        }

        private void EliminarPresentacion_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblPresentaciones.SelectedItem;
            if (seleccion != null)
            {
                MessageBoxResult dialogResult = MessageBox.Show("¿Está seguro de eliminar la presentación '" + seleccion.Row.ItemArray[1] + "' de la lista?", "Eliminación de Presentación", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    Presentacion presentacion = new Presentacion();
                    presentacion.id = Convert.ToInt32(seleccion.Row.ItemArray[0].ToString());
                    if (presentacion.id > 0)
                    {
                        presentacion.eliminar(presentacion);
                        seleccion.Delete();
                        clearFields();
                    }
                    else{
                        seleccion.Delete();
                        clearFields();
                    }

                    
                }
                else {
                    clearFields();
                }
            }
            else
            {
                MessageBox.Show("Es nesesario que seleccione la presentación que desea eliminar de la lista");
            }
        }

        private void AddList_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (hasChanged=="Yes")
            {
                MessageBox.Show("SE GUARDARON LOS CAMBIOS EN EL INSUMO '"+insumo.descripcion+"'");
            }
            if (route == "InsumosFirst")
            {
                GestionInsumos vista = new GestionInsumos();
                vista.Show();
            }
            this.Close();
  
        }

        private void SoloNumeros_KeyDown(object sender, KeyEventArgs e)
        {

        
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.OemComma || e.Key == Key.Tab)
                e.Handled = false;
            else
                e.Handled = true;

        }

        private void txtUCosto_KeyUp(object sender, KeyEventArgs e)
        {

            if (!String.IsNullOrEmpty(txtUcosto.Text) && !String.IsNullOrEmpty(txtIVA.Text))
            {
                string value_ucosto = txtUcosto.Text;
                string value_iva = txtIVA.Text;
                if (value_ucosto[0].ToString() == ",")
                {
                    txtUcosto.Text = "0,";
                }
                if (value_iva[0].ToString() == ",")
                {
                    txtIVA.Text = "0,";
                }
                double ultimo_costo = Convert.ToDouble(txtUcosto.Text);
                double costo_promedio = Convert.ToDouble(txtUcosto.Text); ;
                double IVA = Convert.ToDouble(txtIVA.Text.ToString()) / 100;
                string costo_con_impuesto = txtCCImpuesto.Text;
                txtCCImpuesto.Text = Convert.ToString((ultimo_costo * IVA) + ultimo_costo);
                txtCPromedio.Text = Convert.ToString(ultimo_costo);
            }
            else
            {
                txtCPromedio.Clear();
                txtCCImpuesto.Clear();
            }


        }

        private void cbxInsumoBase_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbxInsumoBase.SelectedItem != null)
            {
                txtDescripcion.Text = cbxInsumoBase.SelectedItem.ToString();
                this.insumo = insumo.obtener(cbxInsumoBase.SelectedItem.ToString());
                llenarPresentaciones();
                indicarInsumoBase();
            }
            else {
                dtPLista.Rows.Clear();
                txtDescripcion.Clear();
                txtUnidadDeMedida.Text = "...";
            }
        }

        private void addAlmacen_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtAlmacen.Text))
            {
                Almacen almacen = new Almacen();
                almacen.nombre = txtAlmacen.Text;
                almacen.registrar(almacen);
                if (almacen.id>0)
                {
                    llenarAlmacenes();
                    cbxAlmacen.SelectedItem = almacen.nombre;
                    txtAlmacen.Clear();
                    addPresentacion.Visibility = Visibility.Visible;
                    groupAlmacen.Visibility = Visibility.Collapsed;
                }
            }

        }

        private void cbxAlmacen_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbxAlmacen.SelectedItem != null)
            {
                if (cbxAlmacen.SelectedItem.ToString() == "Nuevo")
                {
                    addPresentacion.Visibility = Visibility.Collapsed;
                    txtAlmacen.Focus();
                    groupAlmacen.Visibility = Visibility.Visible;
                }
                else
                {
                    addPresentacion.Visibility = Visibility.Visible;
                    groupAlmacen.Visibility = Visibility.Collapsed;
                }
            }
            else {
                addPresentacion.Visibility = Visibility.Visible;
                groupAlmacen.Visibility = Visibility.Collapsed;
            }
        }

        public void exportTo(string exportTo, DevExpress.Xpf.Grid.TableView view, string name)
        {
            System.Windows.Forms.FolderBrowserDialog carpeta = new System.Windows.Forms.FolderBrowserDialog();
            carpeta.Description = "Seleccione la carpeta destino";
            carpeta.ShowDialog();
            DateTime thisDay = DateTime.Today;
            string fecha = thisDay.ToString("D");
            string rout = carpeta.SelectedPath;
            string nombre = name;
            if (exportTo == ".xsls")
            {
                if (!String.IsNullOrEmpty(rout))
                {
                    view.ExportToXlsx(rout + @"\Shark_" + nombre + "_" + fecha + ".xlsx");
                    System.Windows.MessageBoxResult dialogResult = System.Windows.MessageBox.Show("El Reporte se creó satisfactoriamente en la ubicación especificada, ¿Desea Abrir el Archivo? '", "Creación De Reporte", System.Windows.MessageBoxButton.YesNo);
                    if (dialogResult == System.Windows.MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(rout + @"\Shark_" + nombre + "_" + fecha + ".xlsx");
                    }
                }
            }
            else if (exportTo == ".png")
            {
                if (!String.IsNullOrEmpty(rout))
                {
                    view.ExportToImage(rout + @"\Shark_" + nombre + "_" + fecha + ".png");
                    System.Windows.MessageBoxResult dialogResult = System.Windows.MessageBox.Show("El Reporte se creó satisfactoriamente en la ubicación especificada, ¿Desea Abrir el Archivo? '", "Creación De Reporte", System.Windows.MessageBoxButton.YesNo);
                    if (dialogResult == System.Windows.MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(rout + @"\Shark_" + nombre + "_" + fecha + ".png");
                    }
                }
            }
            else if (exportTo == ".pdf")
            {
                if (!String.IsNullOrEmpty(rout))
                {
                    view.ExportToPdf(rout + @"\Shark_" + nombre + "_" + fecha + ".pdf");
                    System.Windows.MessageBoxResult dialogResult = System.Windows.MessageBox.Show("El Reporte se creó satisfactoriamente en la ubicación especificada, ¿Desea Abrir el Archivo? '", "Creación De Reporte", System.Windows.MessageBoxButton.YesNo);
                    if (dialogResult == System.Windows.MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(rout + @"\Shark_" + nombre + "_" + fecha + ".pdf");
                    }
                }

            }

        }

        private void presentaciones_ExportToExcel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".xsls",  tablaPresentaciones, "Presentaciones");
        }

        private void proveedores_ExportToPDF_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".pdf", tablaPresentaciones, "Presentaciones");
        }

        private void proveedores_ExportToPNG_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".png", tablaPresentaciones, "Presentaciones");
        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ReportsView.PresentationView vista = new ReportsView.PresentationView();
            vista.Show();
        }


    }
}
