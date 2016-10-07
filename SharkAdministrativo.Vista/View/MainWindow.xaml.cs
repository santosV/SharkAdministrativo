using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Ribbon;
using System.Data;
using Microsoft.Win32;
using System.Xml;
using SharkAdministrativo.Modelo;

namespace SharkAdministrativo.Vista
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DXRibbonWindow
    {
        Factura factura = new Factura();
        List<string> errores = new List<string>();
        DataTable dtFacturas = new DataTable();
        DataTable dtProveedores = new DataTable();
        Proveedor proveedor = new Proveedor();
        int add = 0;
        public MainWindow()
        {
            InitializeComponent();
            loadtitles();
            Vista1_facturas.Visibility = Visibility.Visible;
        }


        

        /// <summary>
        /// ABRE EL EXPLORADOR DE ARCHIVOS DE WINDOWS PARA SELECCIONAR LAS CFDI.
        /// </summary>
        /// <param name="sender">DevExpress param</param>
        /// <param name="e">Action param</param>
        private void actionCargarXML(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {


            OpenFileDialog openFileDialog = new OpenFileDialog(); //abre el explorador de archivos de windows.
            openFileDialog.Multiselect = true; //permite la multiselección.
            openFileDialog.Filter = "Ficheros Xml|*.xml"; //define el tipo de archivo a buscar.
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //obtiene la selección.
            if (openFileDialog.ShowDialog() == true)//verifica si se seleccionó un archivo XML.
            {
                foreach (string url in openFileDialog.FileNames) //obtiene las rutas de los archivos seleccionados.
                {
          
                    
                    obtenerFactura(url);

                }
            }
        }

        /// <summary>
        /// OBTIENE TODOS LOS DATOS DEL CFDI Y CREA LOS OBJETOS CORRESPONDIENTES.
        /// </summary>
        /// <param name="url">Dirección / úbicación del archivo XML</param>
        private void obtenerFactura(string url)
        {
            XmlDocument xml_factura = new XmlDocument();
            xml_factura.Load(url); //Carga el documento de acuerdo a la ruta que se obtuvo del explorador de archivos.
            XmlNodeList comprobante = xml_factura.GetElementsByTagName("cfdi:Comprobante");
            factura = new Factura();
            foreach (XmlElement nodo in comprobante)
            {
                try
                {
                    if (nodo.GetAttribute("folio") != "")
                    {
                        factura.total = nodo.GetAttribute("total");
                        factura.subtotal = nodo.GetAttribute("subTotal");
                        factura.folio = nodo.GetAttribute("folio");
                        factura.forma_pago = nodo.GetAttribute("formaDePago");
                        factura.lugar_expedicion = nodo.GetAttribute("LugarExpedicion");
                        factura.moneda = nodo.GetAttribute("Moneda");
                        factura.procesada = 0;
                        factura.total = nodo.GetAttribute("total");
                        factura.subtotal = nodo.GetAttribute("subTotal");
                        factura.tipo_cambio = nodo.GetAttribute("TipoCambio");
                        string descuento = nodo.GetAttribute("Descuento");
                        factura.fecha_emision = Convert.ToDateTime(nodo.GetAttribute("fecha"), System.Globalization.CultureInfo.InvariantCulture);
                        factura.tipo_comprobante = nodo.GetAttribute("tipoDeComprobante");
                        obtenerEmpresa(xml_factura, url);
                    }
                    else
                    {
                        errores.Add(url);
                        errores.Add(factura.folio);
                        errores.Add("LA FACTURA NO CUENTA CON FOLIO");
                        btnErrores.Content = "Facturas (" + errores.Count / 3 + ")";
                    }

                }
                catch (Exception e)
                {
                    errores.Add(url);
                    errores.Add(factura.folio);
                    errores.Add("ERROR DE FORMATO EN LOS CAMPOS DE LA FACTURA");
                }
            }
        }

        private void goToPromocion_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Promociones vista = new Promociones();
            vista.Show();
        }

        /// <summary>
        /// Obtiene los datos del XML (Factura) acerca del receptor (Empresa).
        /// </summary>
        /// <param name="xml_factura">El XML (factura) a obtener</param>
        /// <param name="url">Ubicación del XML</param>
        private void obtenerEmpresa(XmlDocument xml_factura, string url)
        {
            XmlNodeList receptor = xml_factura.GetElementsByTagName("cfdi:Receptor");
            XmlNodeList domicilio_fiscal_receptor = xml_factura.GetElementsByTagName("cfdi:Domicilio");
            Empresa empresa = new Empresa();
            string razon = "";
            bool error = true;
            foreach (XmlElement nodo in receptor)
            {

                empresa = empresa.obtenerPorRFC(nodo.GetAttribute("rfc"));
                if (empresa.rfc != null)
                {
                    if (empresa.nombre.Trim() != nodo.GetAttribute("nombre"))
                    {
                        foreach (XmlElement domicilio in domicilio_fiscal_receptor)
                        {

                            /* if (empresa.calle.Trim() != domicilio.GetAttribute("calle")) { razon = "calle"; }
                            else if (empresa.colonia.Trim() != domicilio.GetAttribute("colonia")) { razon = "colonia"; }
                            else if (empresa.codigo_postal.Trim() != domicilio.GetAttribute("codigoPostal")) { razon = "código postal"; }
                            else if (empresa.municipio.Trim() != domicilio.GetAttribute("municipio")) { razon = "municipio"; }
                            else if (empresa.estado.Trim() != domicilio.GetAttribute("estado")) { razon = "estado"; }
                            else if (empresa.pais.Trim() != domicilio.GetAttribute("pais")) { razon = "país"; }
                            else if (empresa.noExterior.Trim() != domicilio.GetAttribute("noExterior")) { razon = "número exterior"; }
                            else {  }
                             */
                            proveedor.Empresa = empresa;
                            obtenerProveedor(xml_factura, url); error = false;
                        }
                        if (error == true)
                        {
                            MessageBox.Show("ERROR EN FACTURA CON EL FOLIO " + factura.folio + "\nEl campo " + razon + " receptor capturado en la factura no coincide con su empresa, se canceló la operación");
                        }
                    }
                    else
                    {
                        MessageBox.Show("ERROR EN FACTURA CON EL FOLIO " + factura.folio + "\nEl nombre receptor de la factura está mal capturado, no coincide con su empresa, se canceló la operación");
                    }
                }
                else
                {
                    MessageBox.Show("ERROR EN FACTURA CON EL FOLIO " + factura.folio + "\nEl RFC receptor de la factura no coincide con ninguna de sus empresas registradas, se canceló la operación");
                }

            }//foreach
        }

        /// <summary>
        /// Extrae los datos del XML referente al emisor (Proveedor).
        /// </summary>
        /// <param name="xml_factura">La factura XML</param>
        /// <param name="url">La ubicación del XML</param>
        private void obtenerProveedor(XmlDocument xml_factura, string url)
        {
            XmlNodeList emisor = xml_factura.GetElementsByTagName("cfdi:Emisor");
            XmlNodeList domicilio_fiscal_emisor = xml_factura.GetElementsByTagName("cfdi:DomicilioFiscal");

            foreach (XmlElement item in emisor)
            {
                if (item.GetAttribute("rfc") != "")
                {
                    proveedor.RFC = item.GetAttribute("rfc");
                    proveedor.nombre = item.GetAttribute("nombre");
                    proveedor.sucursal = item.GetAttribute("sucursal");
                }
                else
                {
                    errores.Add("ERROR EN FACTURA CON FOLIO " + factura.folio + ", \nEror en los campos del emisor\nRUTA: " + url);
                }

                foreach (XmlElement nodo in domicilio_fiscal_emisor)
                {
                    proveedor.calle = nodo.GetAttribute("calle");
                    proveedor.colonia = nodo.GetAttribute("colonia");
                    proveedor.codigo_postal = nodo.GetAttribute("codigoPostal");
                    proveedor.municipio = nodo.GetAttribute("municipio");
                    proveedor.estado = nodo.GetAttribute("estado");
                    proveedor.pais = nodo.GetAttribute("pais");
                    proveedor.localidad = nodo.GetAttribute("localidad");
                    proveedor.NoExterior = nodo.GetAttribute("noExterior");
                }
            }
            setTable(url);
        }

        /// <summary>
        /// Coloca los datos en una fila del DataGrid.
        /// </summary>
        /// <param name="url">ubicación del XML.</param>
        private void setTable(string url)
        {
            if (add != 1)
            {
                dtFacturas.Rows.Add(url, factura.folio, factura.fecha_emision, factura.tipo_comprobante, factura.fecha_emision, factura.fecha_emision, factura.lugar_expedicion, factura.forma_pago, factura.moneda, factura.tipo_cambio, proveedor.RFC, proveedor.nombre, factura.subtotal, factura.total);

                tblFacturas.ItemsSource = dtFacturas.DefaultView;
            }
            txtFacturasTitle.Text = "Facturas XML Cargadas (" + (dtFacturas.Rows.Count) + ") ";

        }

        /// <summary>
        /// Al presionar la tecla enter obtiene las rutas de los XML que se desean ejecutar.
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Acción DevExpress</param>
        private void tblFacturas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (tblFacturas.SelectedItems.Count > 0)
                {
                    List<string> rutas = new List<string>();
                    for (int i = 0; i < tblFacturas.SelectedItems.Count; i++)
                    {
                        System.Data.DataRowView seleccion = (System.Data.DataRowView)tblFacturas.SelectedItems[i];
                        rutas.Add(Convert.ToString(seleccion.Row.ItemArray[0]));
                    }
                    realizarCaptura(rutas);
                }
            }
        }

        /// <summary>
        /// Obtiene los objetos seleccionados
        /// </summary>
        /// <param name="rutas">ruta del XML.</param>
        private void realizarCaptura(List<string> rutas)
        {
            foreach (var url in rutas)
            {
                this.add = 1;
                obtenerFactura(url);
                CapturaExtendido vista = new CapturaExtendido();

                vista.cargarDatosFactura(this.factura);
                vista.cargarDatosProveedor(this.proveedor);
                vista.cargarDatosInsumos(url);
                this.add = 0;
                vista.Show();
            }

        }

        


        int cont = 0;
        private void ocultarVistas()
        {
            cont++;
            if (cont>1)
            {
            Vista1_facturas.Visibility = Visibility.Collapsed;
            Vista2_Proveedores.Visibility = Visibility.Collapsed;
            vista3_shark.Visibility = Visibility.Collapsed;
            }
        }

        public void loadtitles() {
            dtFacturas.Columns.Add("route", typeof(string));
            dtFacturas.Columns.Add("Folio", typeof(string));
            dtFacturas.Columns.Add("Fecha De Emisión", typeof(string));
            dtFacturas.Columns.Add("Tipo De Comprobante", typeof(string));
            dtFacturas.Columns.Add("Año", typeof(string));
            dtFacturas.Columns.Add("Mes", typeof(string));
            dtFacturas.Columns.Add("Lugar De Expedición", typeof(string));
            dtFacturas.Columns.Add("Forma De Pago", typeof(string));
            dtFacturas.Columns.Add("Moneda", typeof(string));
            dtFacturas.Columns.Add("Tipo De Cambio", typeof(string));
            dtFacturas.Columns.Add("RFC Emisor", typeof(string));
            dtFacturas.Columns.Add("Razón Social", typeof(string));
            dtFacturas.Columns.Add("SubTotal", typeof(string));
            dtFacturas.Columns.Add("Total", typeof(string));
            tblFacturas.ItemsSource = dtFacturas.DefaultView;
            tblFacturas.Columns[0].Visible = false;
            dtProveedores.Columns.Add("id");
            dtProveedores.Columns.Add("Nombre");
            dtProveedores.Columns.Add("Tipo De Proveedor");
            dtProveedores.Columns.Add("Calle");
            dtProveedores.Columns.Add("Colonia");
            dtProveedores.Columns.Add("No. Exterior");
            dtProveedores.Columns.Add("Municipio");
            dtProveedores.Columns.Add("Estado");
            dtProveedores.Columns.Add("País");
            dtProveedores.Columns.Add("Código Postal");
            dtProveedores.Columns.Add("Empresa A La Que Provee");
            tblProveedores.ItemsSource = dtProveedores.DefaultView;
            tblProveedores.Columns[0].Visible = false;
        }




        private void RibbonControl_SelectedPageChanged(object sender, RibbonPropertyChangedEventArgs e)
        {
            try
            {
                ocultarVistas();
                if (Control_menu.SelectedPage == vista_proveedores)
                {
                    Vista2_Proveedores.Visibility = Visibility.Visible;
                }
                else if (Control_menu.SelectedPage == vista_insumos)
                {
                    Vista1_facturas.Visibility = Visibility.Visible;
                }else if(Control_menu.SelectedPage == vista_oficina || Control_menu.SelectedPage == vista_perfiles){
                    vista3_shark.Visibility = Visibility.Visible;
                }
                llenarProveedores();
            }
            catch (Exception ae)
            {
                Console.Write(ae);
            }
        }



        private void registrarProveedor_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GestionProveedores vista = new GestionProveedores();
            vista.Show();
        }

        private void btnInsumos_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GestionInsumos vista = new GestionInsumos();
            vista.Show();
        }

 

        private void btnPresentaciones_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GestionPresentaciones vista = new GestionPresentaciones();
            vista.Show();
        }

        void llenarProveedores() {
            dtProveedores.Rows.Clear();
            List<Proveedor> proveedores = proveedor.obtenerTodos();
            foreach (var item in proveedores)
            {
                dtProveedores.Rows.Add(item.id,item.nombre,item.tipos_proveedor,item.calle,item.colonia,item.NoExterior,item.municipio,item.estado,item.pais,item.codigo_postal, item.Empresa.nombre);   
            }
            txtProveedoresTitle.Text = "Proveedores Registrados (" + proveedores.Count()+")";
        }

        

        private void btnElaborados_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GestionInsumosElaborados vista = new GestionInsumosElaborados();
            vista.showView(1);
            vista.Show();
        }

        private void btnNuevoProducto_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GestionDeProductos vista = new GestionDeProductos();
            vista.Show();
        }

        private void tblProveedores_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblProveedores.SelectedItem;
            if (seleccion != null)
            {
                if (tblProveedores.SelectedItems.Count > 1)
                {
                    txtProveedoresTitle.Text = "Proveedores Seleccionados (" + tblProveedores.SelectedItems.Count + ")";
                }
                else
                {
                    txtProveedoresTitle.Text = "Seleccionaste a " + seleccion.Row.ItemArray[1];
                }
            }
        }

        private void EliminarPresentacion_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblProveedores.SelectedItem;
            if (seleccion != null)
            {
                MessageBoxResult dialogResult = MessageBox.Show("¿Está seguro de eliminar el proveedor '" + seleccion.Row.ItemArray[1] + "'?", "Eliminación de Proveedor", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    Proveedor proveedor = new Proveedor();
                    proveedor.id = Convert.ToInt32(seleccion.Row.ItemArray[0].ToString());
                    if (proveedor.id > 0)
                    {
                        proveedor.eliminar(proveedor);
                        seleccion.Delete();
                    }
                    else
                    {
                        seleccion.Delete();
                    }

                    tblProveedores.SelectedItem = null;
                    txtProveedoresTitle.Text = "Proveedores Registrados (" + dtProveedores.Rows.Count + ")";
                }
            }
            else
            {
                MessageBox.Show("Es nesesario que seleccione el proveedor que desea eliminar");
            }
        }

        private void Deseleccionar_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            tblProveedores.SelectedItem = null;
            txtProveedoresTitle.Text = "Proveedores Registrados (" + dtProveedores.Rows.Count + ")";
        }

        private void tblProveedores_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (tblProveedores.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < tblProveedores.SelectedItems.Count; i++)
                    {
                        System.Data.DataRowView seleccion = (System.Data.DataRowView)tblProveedores.SelectedItems[i];
                        GestionProveedores vista = new GestionProveedores();
                        vista.addProveedor(seleccion.Row.ItemArray[1].ToString(), seleccion.Row.ItemArray[10].ToString());
                        vista.Show();
                    }
                    
                }
            }
        }

        private void Actualizar_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            llenarProveedores();
        }

        private void btnProductos_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GestionInsumosElaborados vista = new GestionInsumosElaborados();
            vista.showView(3);
            vista.Show();
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
                        System.Diagnostics.Process.Start(rout + @"\Shark_"+nombre+"_" + fecha + ".xlsx");
                    }
                }
            }
            else if (exportTo == ".png")
            {
                if (!String.IsNullOrEmpty(rout))
                {
                    view.ExportToImage(rout + @"\Shark_"+nombre+"_" + fecha + ".png");
                    System.Windows.MessageBoxResult dialogResult = System.Windows.MessageBox.Show("El Reporte se creó satisfactoriamente en la ubicación especificada, ¿Desea Abrir el Archivo? '", "Creación De Reporte", System.Windows.MessageBoxButton.YesNo);
                    if (dialogResult == System.Windows.MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(rout + @"\Shark_"+nombre+"_" + fecha + ".png");
                    }
                }
            }
            else if (exportTo == ".pdf")
            {
                if (!String.IsNullOrEmpty(rout))
                {
                    view.ExportToPdf(rout + @"\Shark_"+nombre+"_" + fecha + ".pdf");
                    System.Windows.MessageBoxResult dialogResult = System.Windows.MessageBox.Show("El Reporte se creó satisfactoriamente en la ubicación especificada, ¿Desea Abrir el Archivo? '", "Creación De Reporte", System.Windows.MessageBoxButton.YesNo);
                    if (dialogResult == System.Windows.MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(rout + @"\Shark_"+nombre+"_" + fecha + ".pdf");
                    }
                }

            }

        }

        private void proveedores_ExportToExcel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".xsls",tablaProveedores,"Proveedores");
        }

        private void proveedores_ExportToPDF_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".pdf", tablaProveedores, "Proveedores");
        }

        private void proveedores_ExportToPNG_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".png", tablaProveedores, "Proveedores");
        }

        private void btnProviderReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ReportsView.ProviderView vista = new ReportsView.ProviderView();
            vista.Show();
        }

        private void btnGroup_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            RegistrosRapidos vista = new RegistrosRapidos();
            vista.showView(1);
            vista.Show();
        }

        private void btnCategory_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            RegistrosRapidos vista = new RegistrosRapidos();
            vista.showView(2);
            vista.Show();
        }

        private void btnSotrage_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            RegistrosRapidos vista = new RegistrosRapidos();
            vista.showView(3);
            vista.Show();
        }

        private void btnEntradas_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            View.EntradasAlamcen vista = new View.EntradasAlamcen();
            vista.Show();
        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            View.MovimientosAlmacen vista = new View.MovimientosAlmacen();
            vista.Show();
        }

    }
}
