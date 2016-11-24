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
using System.Globalization;
using Microsoft.Win32;
using System.Data;
using System.IO;

namespace SharkAdministrativo.Vista
{
    /// <summary>
    /// Lógica de interacción para Promociones.xaml
    /// </summary>
    public partial class Promociones : Window
    {
        AreaProduccion area = new AreaProduccion();
        Promocion promocion = new Promocion();
        DataTable dtPromociones = new DataTable();
        DataTable dtProductos = new DataTable();

        public Promociones()
        {
            InitializeComponent();
            cargarAreas();
            cargarHoras();
            cargarDias();
            loadTiltesPromo();
            cargarComboBoxProductos();
            cargarVista(1);
            loadTitlesDetalles();
        }

        /// <summary>
        /// Carga los títulos de la tabla de detalles de promocíones.
        /// </summary>
        void loadTitlesDetalles()
        {
            dtProductos.Columns.Add("ID");
            dtProductos.Columns.Add("Nombre");
            dtProductos.Columns.Add("Cantidad");
            dtProductos.Columns.Add("Total");
            tblProductos.ItemsSource = dtProductos.DefaultView;
            tblProductos.Columns[0].Visible = false;
        }

        /// <summary>
        /// Provee la vista para agregar detalles a la promoción.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetalle_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblPromociones.SelectedItem;
            if (seleccion != null)
            {
                this.promocion = promocion.obtenerPorId(Convert.ToInt32(seleccion.Row.ItemArray[0].ToString()));
                cargarVista(2);
                cargarDetallePromocion();
                Title.Text = "AGREGA LOS PRODUCTOS DE LA PROMOCIÓN " + this.promocion.descripcion;
            }
            else
            {
                MessageBox.Show("ES NECESARIO QUE SELECCIONE LA PROMOCIÓN");
            }

        }

        /// <summary>
        /// Carga días de la semana.
        /// </summary>
        void cargarDias()
        {
            string[] dias = new string[] { "L", "Ma", "Mi", "J", "V", "S", "D" };
            cbxDiasDisponibles.ItemsSource = dias;
        }

        /// <summary>
        /// Carga las promociones disponibles en shark.
        /// </summary>
        private void cargarPromociones()
        {
            dtPromociones.Rows.Clear();
            List<Promocion> promociones = promocion.obtenerTodos();
            foreach (var promo in promociones)
            {
                dtPromociones.Rows.Add(promo.id, promo.nombre, promo.ultimoPrecio, promo.IVA, promo.precioConImpuesto, promo.areasDisponibles, promo.diasDisponibles, promo.hora_inicio, promo.hora_fin, promo.fecha_inicio, promo.fecha_fin);
            }

        }

        /// <summary>
        /// Carga los títulos de la tabla de promociónes.
        /// </summary>
        public void loadTiltesPromo()
        {
            dtPromociones.Columns.Add("ID");
            dtPromociones.Columns.Add("Nombre");
            dtPromociones.Columns.Add("Precio");
            dtPromociones.Columns.Add("Porcentaje IVA");
            dtPromociones.Columns.Add("Precio Con Impuesto");
            dtPromociones.Columns.Add("Disponible En");
            dtPromociones.Columns.Add("Días");
            dtPromociones.Columns.Add("Hora Inicio");
            dtPromociones.Columns.Add("Hora Fín");
            dtPromociones.Columns.Add("Fecha Inicio");
            dtPromociones.Columns.Add("Fecha Fin");
            tblPromociones.ItemsSource = dtPromociones.DefaultView;
            tblPromociones.Columns[0].Visible = false;
            cargarPromociones();

        }

        /// <summary>
        /// Abre el explorador de archivos y asigna la imagén a la promoción.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog(); //abre el explorador de archivos de windows.
            openFileDialog.Multiselect = false; //permite la multiselección.
            openFileDialog.Filter = "Imágenes|*.png;*.jpg;*.jpeg"; //define el tipo de archivo a buscar.
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //obtiene la selección.
            if (openFileDialog.ShowDialog() == true)//verifica si se seleccionó un archivo XML.
            {
                BitmapImage icono = new BitmapImage();
                icono.BeginInit();
                icono.UriSource = new Uri(openFileDialog.FileName);
                icono.EndInit();
                img.Source = icono;
            }
        }

        /// <summary>
        /// Agrega el nombre de la promoción a la vista previa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNombreP_KeyUp(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtNombre.Text))
            {
                txtCortoName.Text = txtNombre.Text;
            }
            else
            {
                txtCortoName.Text = "Sin Nombre Disponible";
            }
        }

        /// <summary>
        /// Carga todos los productos disponibles en Shark y Contpaqi.
        /// </summary>
        void cargarComboBoxProductos()
        {
            Producto producto = new Producto();
            List<Producto> productos = producto.obtenerTodos();
            foreach (var item in productos)
            {
                cbxProductos.Items.Add(item.nombre);
            }
        }

        /// <summary>
        /// Carga los combobox de horas.
        /// </summary>
        public void cargarHoras()
        {
            string hora = "";
            string minutos = "";
            for (int i = 0; i <= 24; i++)
            {

                if (i < 10)
                {
                    hora = "0" + i;
                }
                else
                {
                    hora = Convert.ToString(i);
                }

                for (int j = 0; j < 60; j++)
                {
                    if (j < 10)
                    {
                        minutos += ":0" + j;
                    }
                    else
                    {
                        minutos += ":" + j;
                    }
                    cbxHInicio.Items.Add(hora + minutos);
                    cbxHFinalizacion.Items.Add(hora + minutos);
                    minutos = "";
                }
            }
        }

        /// <summary>
        /// Carga las áreas disponibles.
        /// </summary>
        public void cargarAreas()
        {
            List<AreaProduccion> areas = area.obtenerTodos();
            foreach (var item in areas)
            {
                cbxDisponible.Items.Add(item.nombre);
            }
        }

        /// <summary>
        /// Carga la vista solicitada.
        /// </summary>
        /// <param name="vista"></param>
        public void cargarVista(int vista)
        {
            vista_promocion.Visibility = Visibility.Collapsed;
            btnGeneral.IsVisible = false;
            vista_detalle.Visibility = Visibility.Collapsed;
            btnDetalle.IsVisible = false;
            if (vista == 1)
            {
                vista_promocion.Visibility = Visibility.Visible;
                btnGeneral.IsVisible = true;
            }
            else
            {
                vista_detalle.Visibility = Visibility.Visible;
                btnDetalle.IsVisible = true;
            }
        }

        /// <summary>
        /// Llama el método cargarVista();
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListo_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            cargarVista(1);
        }

        /// <summary>
        /// Captura los eventos de teclado para permitir solo números.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoloNumeros_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.OemComma || e.Key == Key.Tab)
                e.Handled = false;
            else
                e.Handled = true;
        }

        /// <summary>
        /// Captura los eventos de teclado para obtener los costos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObtenerCostosPromociones_KeyUp(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtPrecio.Text) && !String.IsNullOrEmpty(txtIVA.Text))
            {
                try
                {
                    string costop = txtPrecio.Text;
                    string ivap = txtIVA.Text;
                    double precioImpuesto = (Double.Parse(costop) * (Double.Parse(ivap) / 100)) + Double.Parse(costop);
                    txtPCImpuesto.Text = Convert.ToString(precioImpuesto);
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
            else
            {
                txtPCImpuesto.Clear();
            }
        }

        /// <summary>
        /// Guarda o modifica una promoción en Shark.
        /// </summary>
        public void guardarModificarPromocion()
        {
            if (validarDatos() == true)
            {
                promocion.diasDisponibles = "";
                promocion.areasDisponibles = "";
                promocion.descripcion = txtDescripcion.Text;
                promocion.nombre = txtNombre.Text;
                promocion.ultimoPrecio = Double.Parse(txtPrecio.Text);
                promocion.IVA = Double.Parse(txtIVA.Text);
                promocion.precioConImpuesto = Double.Parse(txtPCImpuesto.Text);
                promocion.hora_inicio = cbxHInicio.SelectedItem.ToString();
                promocion.hora_fin = cbxHFinalizacion.SelectedItem.ToString();

                string fecha_de_inicio = cbxFInicio.Text;
                string fecha_de_fin = cbxFFinalizacion.Text;
                if (promocion.fecha_inicio.ToString() != fecha_de_inicio)
                {
                    DateTime inicia = DateTime.Parse(fecha_de_inicio, System.Globalization.CultureInfo.InvariantCulture);
                    promocion.fecha_inicio = inicia;
                }

                if (promocion.fecha_fin.ToString() != fecha_de_fin)
                {
                    DateTime finaliza = DateTime.Parse(fecha_de_fin, System.Globalization.CultureInfo.InvariantCulture);
                    promocion.fecha_fin = finaliza;
                }

                if (img.Source != null)
                {
                    String[] route = img.Source.ToString().Split('/');
                    string URI = "";
                    foreach (var ruta in route)
                    {
                        if (ruta != "file:")
                        {
                            URI += ruta + "/";
                        }
                    }
                    URI = URI.TrimEnd('/');
                    URI = URI.TrimStart('/');
                    URI = URI.TrimStart('/');



                    if (URI != "System.Windows.Media.Imaging.BitmapImage")
                    {
                        System.Drawing.Image imagen = System.Drawing.Image.FromFile(URI);
                        promocion.imagen = convertirAByte(imagen);
                    }
                }



                foreach (var item in cbxDisponible.SelectedItems)
                {
                    promocion.areasDisponibles += item.ToString() + ";";
                }
                foreach (var item in cbxDiasDisponibles.SelectedItems)
                {
                    promocion.diasDisponibles += item.ToString() + ";";
                }

                if (tblPromociones.SelectedItem == null)
                {
                    promocion.registrar(promocion);
                }
                else
                {
                    promocion.modificar(promocion);
                    MessageBox.Show("SE MODIFICÓ CORRECTAMENTE LA PROMOCIÓN " + promocion.descripcion);
                }

                cargarPromociones();
                clearFieldsPromo();
            }
        }

        /// <summary>
        /// Elimina la promoción seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eliminarPromocion_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblPromociones.SelectedItem;
            if (seleccion != null)
            {
                MessageBoxResult dialogResult = MessageBox.Show("¿ESTÁ SEGURO DE ELIMINAR LA PROMOCIÓN '" + seleccion.Row.ItemArray[1] + "'?", "ELIMINACIÓN DE PROMOCIÓN", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    Promocion promocion = new Promocion();
                    promocion.id = Convert.ToInt32(seleccion.Row.ItemArray[0].ToString());
                    promocion.eliminar(promocion);
                    cargarPromociones();
                    clearFieldsPromo();
                }
            }
            else
            {
                MessageBox.Show("EN NECESARIO QUE SELECCIONE LA PROMOCIÓN QUE DESEA ELIMINAR");
            }
        }


        /// <summary>
        /// Limpia los campos de promociones.
        /// </summary>
        void clearFieldsPromo()
        {

            txtDescripcion.Clear();
            txtNombre.Clear();
            txtCortoName.Text = "Sin Nombre Disponible";
            img.Source = null;
            txtPrecio.Clear();
            txtIVA.Clear();
            txtPCImpuesto.Clear();
            cbxDisponible.SelectedItem = null;
            cbxDiasDisponibles.SelectedItem = null;
            cbxFFinalizacion.Clear();
            cbxFInicio.Clear();
            cbxHInicio.SelectedItem = null;
            cbxHFinalizacion.SelectedItem = null;
            tblPromociones.SelectedItem = false;
            groupPromocion.Header = "Nueva Promoción";
        }
        /// <summary>
        /// Valida que estén ingresados todos los datos en la promoción a crear
        /// </summary>
        /// <returns></returns>
        public bool validarDatos()
        {
            bool validacion = false;
            if (!String.IsNullOrEmpty(txtDescripcion.Text) && !String.IsNullOrEmpty(txtNombre.Text) && !String.IsNullOrEmpty(txtPCImpuesto.Text) && !String.IsNullOrEmpty(txtPrecio.Text) && cbxDiasDisponibles.SelectedItem != null && cbxDisponible.SelectedItem != null && cbxHFinalizacion.SelectedItem != null && cbxHInicio.SelectedItem != null && !String.IsNullOrEmpty(cbxFInicio.Text) && !String.IsNullOrEmpty(cbxFFinalizacion.Text))
            {
                validacion = true;
            }
            return validacion;
        }

        /// <summary>
        /// Manda llamar el método para guardar o modificar una promoción.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPromocion_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            guardarModificarPromocion();
        }

        /// <summary>
        /// Convierte a byte una imágen.
        /// </summary>
        /// <param name="imagen"></param>
        /// <returns></returns>
        public byte[] convertirAByte(System.Drawing.Image imagen)
        {
            System.Drawing.ImageConverter imgCon = new System.Drawing.ImageConverter();
            return (byte[])imgCon.ConvertTo(imagen, typeof(byte[]));
        }

        /// <summary>
        /// Convierte a Imagen un arreglo de bytes.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public System.Windows.Media.Imaging.BitmapImage convertirAImagen(byte[] bytes)
        {
            using (MemoryStream mStream = new MemoryStream(bytes))
            {
                System.Windows.Media.Imaging.BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();
                bImg.BeginInit();
                bImg.StreamSource = new MemoryStream(mStream.ToArray());
                bImg.EndInit();
                return bImg;
            }
        }

        /// <summary>
        /// Captura el evento en la tabla promociones para ponerla en edición.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblPromociones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblPromociones.SelectedItem;
            if (seleccion != null)
            {
                this.promocion = this.promocion.obtenerPorId(Convert.ToInt32(seleccion.Row.ItemArray[0].ToString()));
                txtDescripcion.Text = promocion.descripcion;
                txtNombre.Text = promocion.nombre;
                txtPrecio.Text = Convert.ToString(promocion.ultimoPrecio);
                txtIVA.Text = Convert.ToString(promocion.IVA);
                txtPCImpuesto.Text = Convert.ToString(promocion.precioConImpuesto);
                String[] areasDisponibles = promocion.areasDisponibles.Split(';');
                String[] diasDisponibles = promocion.diasDisponibles.Split(';');
                foreach (var item in areasDisponibles)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        cbxDisponible.SelectedItems.Add(item);
                    }
                }
                foreach (var item in diasDisponibles)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        cbxDiasDisponibles.SelectedItems.Add(item);
                    }
                }
                cbxHInicio.SelectedItem = promocion.hora_inicio;
                cbxHFinalizacion.SelectedItem = promocion.hora_fin;
                cbxFInicio.Text = promocion.fecha_inicio.ToString();
                cbxFFinalizacion.Text = promocion.fecha_fin.ToString();
                txtCortoName.Text = promocion.nombre;
                if (promocion.imagen != null)
                {
                    BitmapImage img1 = convertirAImagen(promocion.imagen);
                    img.Source = img1;
                }
                groupPromocion.Header = "MODIFICANDO " + promocion.descripcion;
            }
        }

        /// <summary>
        /// Valida los campos de detalles.
        /// </summary>
        /// <returns></returns>
        private bool validarCamposDetalle()
        {
            bool validacion = false;
            if (!String.IsNullOrEmpty(txtCantidadP.Text) && cbxProductos.SelectedItem != null)
            {
                validacion = true;
            }
            return validacion;
        }


        /// <summary>
        /// Carga cada uno de los productos que conforma la promoción.
        /// </summary>
        public void cargarDetallePromocion()
        {
            dtProductos.Rows.Clear();
            ProductoPromocion detalle = new ProductoPromocion();
            List<ProductoPromocion> detalles = detalle.obtenerTodos(this.promocion.id);
            double total = 0;
            foreach (var item in detalles)
            {
                dtProductos.Rows.Add(item.id, item.Producto.nombre, item.cantidad, item.Producto.ultimoPrecio * item.cantidad);
                string precio = Convert.ToString(item.Producto.ultimoPrecio);
                string cantidad = Convert.ToString(item.cantidad);
                total += Double.Parse(cantidad) * Double.Parse(precio);
            }
            txtTotal.Text = "Total Promoción: $" + this.promocion.ultimoPrecio + ", Total En Producto: $" + total;
        }

        /// <summary>
        /// Limpia los campos de detalle.
        /// </summary>
        void clearFieldDetalle()
        {
            txtCantidadP.Clear();
            cbxProductos.SelectedItem = null;
            tblProductos.SelectedItem = false;
        }

        /// <summary>
        /// Agrega un producto a la relación de detalles de la promoción.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addProduct_Click(object sender, RoutedEventArgs e)
        {
            if (validarCamposDetalle())
            {
                Producto producto = new Producto();
                ProductoPromocion detalle_promocion = new ProductoPromocion();
                detalle_promocion.Promocion = promocion.obtenerPorId(this.promocion.id);
                detalle_promocion.promocion_id = detalle_promocion.Promocion.id;
                detalle_promocion.Producto = producto.obtener(cbxProductos.SelectedItem.ToString());
                detalle_promocion.producto_id = detalle_promocion.Producto.id;
                detalle_promocion.cantidad = Double.Parse(txtCantidadP.Text);

                if (tblProductos.SelectedItem == null)
                {
                    detalle_promocion.registrar(detalle_promocion);
                }
                else
                {
                    System.Data.DataRowView seleccion = (System.Data.DataRowView)tblProductos.SelectedItem;
                    detalle_promocion.id = Convert.ToInt32(seleccion.Row.ItemArray[0].ToString());
                    detalle_promocion.modificar(detalle_promocion);
                }
                cargarDetallePromocion();
                clearFieldDetalle();
            }
            else
            {
                MessageBox.Show("EXISTEN CAMPOS IMPORTANTES SIN INGRESAR");
            }

        }
        /// <summary>
        /// Manda llamar el método para limpiar los campos de promoción.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NuevoPromo_ItemClick_1(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            clearFieldsPromo();
        }

        /// <summary>
        /// Detecta el evento en la tabla de productos y los pone en edición.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblProductos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblProductos.SelectedItem;
            if (seleccion != null)
            {
                ProductoPromocion detalle = new ProductoPromocion();
                detalle = detalle.obtener(Convert.ToInt32(seleccion.Row.ItemArray[0].ToString()));
                cbxProductos.SelectedItem = seleccion.Row.ItemArray[1].ToString();
                txtCantidadP.Text = Convert.ToString(detalle.cantidad);
            }
        }

        /// <summary>
        /// Elimina un producto en el detalle  de la promoción.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eliminarProductoDePromocion_ItemClick_1(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblProductos.SelectedItem;
            if (seleccion != null)
            {
                MessageBoxResult dialogResult = MessageBox.Show("¿ESTÁ SEGURO DE ELIMINAR EL PRODUCTO '" + seleccion.Row.ItemArray[1] + "' DE LA PROMOCIÓN?", "ELIMINACIÓN DE PRODUCTO DE ELIMINACIÓN", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    ProductoPromocion detalle = new ProductoPromocion();
                    detalle.id = Convert.ToInt32(seleccion.Row.ItemArray[0].ToString());
                    detalle.eliminar(detalle);
                    cargarDetallePromocion();
                    clearFieldDetalle();
                }
            }
            else
            {
                MessageBox.Show("EN NECESARIO QUE SELECCIONE EL PRODUCTO QUE DESEA ELIMINAR");
            }
        }
        /// <summary>
        /// Manda llamar el método para limpiar los campos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nuevoProducto_ItemClick_1(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            clearFieldDetalle();
        }

        /// <summary>
        /// Manda llamar el frame de los reportes de promociones.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PromotionsReport_ItemClick_1(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ReportsView.PromotionView vista = new ReportsView.PromotionView();
            vista.Show();
        }


    }
}
