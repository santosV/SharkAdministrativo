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
using System.Data;
using System.Xml;
using SharkAdministrativo.SDKCONTPAQi;

namespace SharkAdministrativo.Vista
{
    /// <summary>
    /// Lógica de interacción para CapturaExtendido.xaml
    /// </summary>
    public partial class CapturaExtendido : Window
    {

        public Factura factura { get; set; }
        public Proveedor proveedor { get; set; }
        Insumo insumo = new Insumo();
        Almacen almacen = new Almacen();
        Unidad_Medida unidad = new Unidad_Medida();
        DataTable dt = new DataTable();
        string validacion;
        public CapturaExtendido()
        {
            InitializeComponent();
            obtenerValoresDeClasificaciones();
        }

        /// <summary>
        /// Bloquea los textfields cuando el proveedor ya está registrado.
        /// </summary>
        public void bloquearCajas()
        {
            validacion = proveedor.validar(proveedor);
            if (validacion != "unico")
            {
                txtRazonSocialP.IsReadOnly = true;
                txtRfcP.IsReadOnly = true;
                txtLocalidadP.IsReadOnly = true;
                txtMunicipioP.IsReadOnly = true;
                txtEstadoP.IsReadOnly = true;
                txtPaisP.IsReadOnly = true;
                txtColoniaP.IsReadOnly = true;
                txtCalleP.IsReadOnly = true;
                txtNoExteriorP.IsReadOnly = true;
                txtCodigoPostalP.IsReadOnly = true;
                txtSucursalP.IsReadOnly = true;
                if (validacion == "sucursal")
                {
                    txtSucursalP.IsReadOnly = false;
                }
            }
        }

        /// <summary>
        /// Obtiene la información de los objetos para mandar llamar los modelos correspondientes.
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Acción DevExpress</param>
        private void btnSaveAndClose_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            string folio = "";
            folio = factura.validarRegistro(factura.folio);
            if (factura.folio != folio)
            {
                if (validacion == "unico")
                {
                    if (txtRazonSocialP.Text != "" && txtCalleP.Text != "" && txtCodigoPostalP.Text != ""
                    && txtPaisP.Text != "" && txtMunicipioP.Text != "" &&
                    txtEstadoP.Text != "" && txtRfcP.Text != "" && txtNoExteriorP.Text != "")
                    {
                        proveedor.nombre = txtRazonSocialP.Text;
                        proveedor.NoExterior = txtNoExteriorP.Text;
                        proveedor.localidad = txtLocalidadP.Text;
                        proveedor.calle = txtCalleP.Text;
                        proveedor.colonia = txtColoniaP.Text;
                        proveedor.codigo_postal = txtCodigoPostalP.Text;
                        proveedor.municipio = txtMunicipioP.Text;
                        proveedor.estado = txtEstadoP.Text;
                        proveedor.pais = txtPaisP.Text;
                        proveedor.RFC = txtRfcP.Text;
                        proveedor.sucursal = txtSucursalP.Text;
                        DateTime thisDay = DateTime.Today;
                        proveedor.fecha_registro = Convert.ToDateTime(thisDay.ToString());

                        //resgistrar proveedor en contpaq
                        proveedor.registrar(proveedor);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Hay campos importantes que están vacíos, por favor ingréselos");
                    }
                }
                else if (validacion == "sucursal")
                {
                    if (txtSucursalP.Text != "")
                    {
                        proveedor.registrar(proveedor);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Datos del actual proveedor: \nSe tiene registrado este proveedor con el RFC: \n" + proveedor.RFC + "Pero hay datos que difieren con el registro existente, En caso de tratarse de otra sucursal ingrese la sucursal por favor");
                    }
                }
                obtenerConceptos();
            }
            else
            {
                MessageBox.Show("ERROR EN FACTURA FOLIO - " + factura.folio + "\nYa ha sido utilizada / registrada.");
            }
        }

        /// <summary>
        /// Obtiene todos los datos de Insumo de la tabla y atrapa los errores si es que existen, si todo sale bien manda llamar el método para registrar.
        /// </summary>
        public void obtenerConceptos()
        {
            int cont = 0;
            bool sucess = true;
            List<Presentacion> presentaciones = new List<Presentacion>();
            foreach (DataRow item in dt.Rows)
            {
                Presentacion presentacion = new Presentacion();
                Unidad_Medida unidad = new Unidad_Medida();
                Categoria categoria = new Categoria();
                Almacen almacen = new Almacen();

                presentacion.codigo = item.ItemArray[13].ToString();
                presentacion.Insumo = insumo.obtener(item.ItemArray[0].ToString());
                presentacion.descripcion = item.ItemArray[1].ToString();
                presentacion.Almacen = almacen.obtener(item.ItemArray[6].ToString());
                presentacion.cantidad = Double.Parse(item.ItemArray[4].ToString());
                presentacion.costo_con_impuesto = Double.Parse(item.ItemArray[9].ToString());
                presentacion.costo_promedio = Double.Parse(item.ItemArray[8].ToString());
                presentacion.costo_unitario = Double.Parse(item.ItemArray[3].ToString());
                presentacion.IVA = Double.Parse(item.ItemArray[11].ToString());
                presentacion.noIdentificacion = item.ItemArray[7].ToString();
                presentacion.Proveedor = proveedor.obtenerPorRFC(txtRfcP.Text);
                presentacion.rendimiento = Double.Parse(item.ItemArray[10].ToString());
                presentacion.ultimo_costo = Double.Parse(item.ItemArray[3].ToString());
                presentacion.existencia = presentacion.rendimiento * presentacion.cantidad;
                if (presentacion.Almacen.id > 0 && presentacion.Insumo.id > 0 && presentacion.Proveedor.id > 0 && !String.IsNullOrEmpty(presentacion.descripcion) && presentacion.costo_unitario != null && presentacion.rendimiento != null)
                {
                    presentaciones.Add(presentacion);
                    cont++;
                }
                else
                {
                    MessageBox.Show("Es Necesario que ingrese todos los capos solicitados en el insumo: \n" + insumo.descripcion);

                    presentaciones.Clear();
                    sucess = false;
                    return;
                }
            }
            if (sucess == true)
            {
                registrarPresentaciones(presentaciones);
            }

        }

        /// <summary>
        /// Manda llamar los modelos correspondientes para guardar en la base de datos.
        /// </summary>
        /// <param name="presentaciones">Lista de Insumo.</param>
        public void registrarPresentaciones(List<Presentacion> presentaciones)
        {
            int error = 0;
            Double folio = 0;
            SDK.tDocumento lDocto = new SDK.tDocumento();
            SDK.tMovimiento lMovto = new SDK.tMovimiento();

            factura.registrar(factura);
            foreach (var presentacion in presentaciones)
            {

                if (presentacion.verificarRegistro(presentacion) == false)
                {
                    presentacion.Factura = factura;

                    //registro de presentacion a contpaq
                    SDK.tProduto cProducto = new SDK.tProduto();
                    cProducto.cCodigoProducto = presentacion.codigo;
                    cProducto.cNombreProducto = presentacion.descripcion;
                    cProducto.cDescripcionProducto = presentacion.descripcion;
                    String[] clasificacion = cbxValoresDeClasificaciones.SelectedItem.ToString().Split('|');
                    string codigoClasificacion = clasificacion[0].Trim();
                    string ultimo_costo = presentacion.ultimo_costo.ToString();
                    cProducto.cPrecio1 = Double.Parse(ultimo_costo);
                    cProducto.cImpuesto1 = Double.Parse(Convert.ToString(presentacion.IVA));
                    cProducto.cTipoProducto = 1;
                    cProducto.cMetodoCosteo = 1;

                    Int32 aldProducto = 0;
                    error = SDK.fAltaProducto(ref aldProducto, ref cProducto);
                    if (error == 0)
                    {
                        SDK.fEditaProducto();
                        SDK.fSetDatoProducto("CIDVALORCLASIFICACION1", codigoClasificacion);
                        SDK.fGuardaProducto();
                        presentacion.registrar(presentacion);

                    }
                    else
                    {
                        SDK.rError(error);
                    }


                }
                StringBuilder serie = new StringBuilder();
                folio = Double.Parse(factura.folio);

                //fetch de crear un concepto nuevo para la compra
                SDK.fSiguienteFolio("19", serie, ref folio);
                lDocto.aCodConcepto = "19";
                lDocto.aFolio = folio;
                lDocto.aSerie = "";

                //registro de documento(entrada de almacen) a contpaq

                EntradaPresentacion entrada = new EntradaPresentacion();
                DateTime thisDay = DateTime.Today;
                entrada.fecha_registro = Convert.ToDateTime(thisDay.ToString());
                Presentacion presentacionR = presentacion.obtener(presentacion);
                entrada.Presentacion = presentacionR;

                entrada.Almacen = almacen.obtener(presentacion.Almacen.nombre);
                entrada.cantidad = presentacion.cantidad;
                entrada.registrar(entrada);
            }

            //registro de la compra
            lDocto.aFecha = DateTime.Today.ToString("MM/dd/yyyy");
            lDocto.aImporte = 223;
            lDocto.aCodigoCteProv = "879";
            lDocto.aTipoCambio = 1;
            lDocto.aNumMoneda = 1;
            lDocto.aSistemaOrigen = 1;

            Int32 aIdDocumento = 0;
            error = SDK.fAltaDocumento(ref aIdDocumento, ref lDocto);
            if (error != 0)
            {
                SDK.rError(error);
                return;
            }
            else
            {

                MessageBox.Show("Documeto Creado");

            }


            lMovto.aCodAlmacen = almacen.obtener(cbxAlmacen.SelectedItem.ToString()).codigo;
            lMovto.aCodProdSer = "PREO";
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblInsumos.SelectedItem;
            if (seleccion != null)
            {
            lMovto.aPrecio = Double.Parse(seleccion.Row.ItemArray[3].ToString());
            lMovto.aUnidades = Double.Parse(seleccion.Row.ItemArray[4].ToString());
            }
            lMovto.aConsecutivo = 1;
            

            Int32 aIdMovimiento = 0;
            error = SDK.fAltaMovimiento(aIdDocumento, ref aIdMovimiento, ref lMovto);
            if (error != 0)
            {
                SDK.rError(error);
                return;
            }
            else
            {
                MessageBox.Show("Movimiento Creado");
            }


            System.Windows.Forms.MessageBox.Show("Se registraron correctamente los datos de proveedor e Insumo!");

            this.Close();
        }

        /// <summary>
        /// Obtiene de CONTPAQI los valores de las clasificaciones disponibles.
        /// </summary>
        public void obtenerValoresDeClasificaciones()
        {
            int error = SDK.fPosPrimerValorClasif();
            while (error == 0)
            {
                StringBuilder codValorClasificacion = new StringBuilder(11);
                StringBuilder nomValorClasificacion = new StringBuilder(30);
                StringBuilder cIdClasificacion = new StringBuilder(5);
                SDK.fLeeDatoValorClasif("CIDVALORCLASIFICACION", codValorClasificacion, 11);
                SDK.fLeeDatoValorClasif("CVALORCLASIFICACION", nomValorClasificacion, 30);
                SDK.fLeeDatoValorClasif("CIDCLASIFICACION", cIdClasificacion, 30);
                int idClasificacion = Convert.ToInt32(cIdClasificacion.ToString());
                if (idClasificacion >= 25 && idClasificacion <= 30)
                {
                    if (nomValorClasificacion.ToString() != "(Ninguna)")
                    {
                        cbxValoresDeClasificaciones.Items.Add(codValorClasificacion + " | " + nomValorClasificacion);
                    }
                }
                error = SDK.fPosSiguienteValorClasif();
            }

        }

        /// <summary>
        /// Carga en la vista todos los datos referentes al objeto factura.
        /// </summary>
        /// <param name="factura">Objeto a colocar</param>
        public void cargarDatosFactura(Factura factura)
        {
            this.factura = factura;
            txtSubTotalF.Text = factura.subtotal;
            txtTotalF.Text = factura.total;
            txtFechaF.Text = Convert.ToString(factura.fecha_emision);
            txtFormaPagoF.Text = factura.forma_pago;
            txtTipoCambioF.Text = factura.tipo_cambio;
            txtTipoComprobanteF.Text = factura.tipo_comprobante;
            txtMonedaF.Text = factura.moneda;
            ventana.Title = "Captura | Proveedor | Insumo | Factura Folio " + factura.folio;

        }

        /// <summary>
        /// Carga en la vista todos los datos referentes al objeto proveedor.
        /// </summary>
        /// <param name="proveedor">Objeto a colocar</param>
        public void cargarDatosProveedor(Proveedor proveedor)
        {
            this.proveedor = proveedor;
            txtRazonSocialP.Text = proveedor.nombre;
            txtPaisP.Text = proveedor.pais;
            txtRfcP.Text = proveedor.RFC;
            txtSucursalP.Text = proveedor.sucursal;
            txtLocalidadP.Text = proveedor.localidad;
            txtColoniaP.Text = proveedor.colonia;
            txtCodigoPostalP.Text = proveedor.codigo_postal;
            txtEstadoP.Text = proveedor.estado;
            txtMunicipioP.Text = proveedor.municipio;
            txtNoExteriorP.Text = proveedor.NoExterior;
            txtCalleP.Text = proveedor.calle;
            bloquearCajas();
        }

        /// <summary>
        /// Carga en la vista todos los datos referentes al objeto Insumo.
        /// </summary>
        /// <param name="ruta">ruta del XML</param>
        public void cargarDatosInsumos(string ruta)
        {
            XmlDocument xml_factura = new XmlDocument();
            xml_factura.Load(ruta); //Carga el documento de acuerdo a la ruta que se obtuvo del explorador de archivos.
            XmlNodeList conceptos = xml_factura.GetElementsByTagName("cfdi:Conceptos");
            cargarTitulos();
            foreach (XmlElement nodo in conceptos)
            {
                XmlNodeList concepto = xml_factura.GetElementsByTagName("cfdi:Concepto");
                foreach (XmlElement item in concepto)
                {

                    Presentacion presentacion = new Presentacion();
                    //presentacion.costo_unitario = float.Parse(item.GetAttribute("importe").ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    //insumo.costo_referenc = float.Parse(item.GetAttribute("importe").ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    presentacion.ultimo_costo = float.Parse(item.GetAttribute("valorUnitario").ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    presentacion.noIdentificacion = item.GetAttribute("noIdentificacion");

                    presentacion.descripcion = item.GetAttribute("descripcion");
                    presentacion.cantidad = float.Parse(item.GetAttribute("cantidad").ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    dt.Rows.Add("", presentacion.descripcion, proveedor.nombre, presentacion.ultimo_costo, presentacion.cantidad, "", "", presentacion.noIdentificacion, "", "", "", "", presentacion.ultimo_costo * presentacion.cantidad);

                }
            }
            tblInsumos.ItemsSource = dt.DefaultView;

        }

        /// <summary>
        /// Carga los titulos de la tabla de Insumo.
        /// </summary>
        private void cargarTitulos()
        {
            dt.Columns.Add("Insumo Base", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Proveedor");
            dt.Columns.Add("Último Costo", typeof(string));
            dt.Columns.Add("Cantidad", typeof(string));
            dt.Columns.Add("Unidad De Medida");
            dt.Columns.Add("Almacén");
            dt.Columns.Add("No. Identificación");
            dt.Columns.Add("Costo Promedio");
            dt.Columns.Add("Costo Con Impuesto");
            dt.Columns.Add("Rendimiento");
            dt.Columns.Add("IVA");
            dt.Columns.Add("Total", typeof(string));
            dt.Columns.Add("Codigo");
            tblInsumos.ItemsSource = dt.DefaultView;

            llenarAlmacen();
            llenarInsumoBase();
        }


        public void llenarInsumoBase()
        {
            List<Insumo> Insumo = insumo.obtenerTodos();

            foreach (var item in Insumo)
            {
                cbxInsumos.Items.Add(item.descripcion);
            }
        }

        public void llenarAlmacen()
        {
            List<Almacen> Almacen = almacen.obtenerTodos();
            foreach (var item in Almacen)
            {
                cbxAlmacen.Items.Add(item.nombre);
            }
        }

        private void cbxInsumos_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbxInsumos.SelectedItem != null)
            {

                insumo = insumo.obtener(cbxInsumos.SelectedItem.ToString());
                unidad = unidad.obtenerPorId(insumo.unidad_id);
                txtUnidad.Text = unidad.nombre;

                System.Data.DataRowView seleccion = (System.Data.DataRowView)tblInsumos.SelectedItem;
                if (seleccion != null)
                {
                    txtDescripcion.Text = cbxInsumos.SelectedItem.ToString() + " " + seleccion.Row.ItemArray[1].ToString();
                }
                else
                {
                    txtDescripcion.Text = cbxInsumos.SelectedItem.ToString();
                }

            }
            else
            {
                txtUnidad.Text = "...";
                txtDescripcion.Clear();
            }
        }

        private void tblInsumos_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblInsumos.SelectedItem;
            if (seleccion != null)
            {
                txtDescripcion.Text = seleccion.Row.ItemArray[1].ToString();
                if (!String.IsNullOrEmpty(seleccion.Row.ItemArray[0].ToString()))
                {
                    cbxInsumos.SelectedItem = seleccion.Row.ItemArray[0].ToString();
                }
                if (!String.IsNullOrEmpty(seleccion.Row.ItemArray[6].ToString()))
                {
                    cbxAlmacen.SelectedItem = seleccion.Row.ItemArray[6].ToString();
                }
                txtCostoUnitario.Text = seleccion.Row.ItemArray[3].ToString();
                txtCantidad.Text = seleccion.Row.ItemArray[4].ToString();
                txtRendimiento.Text = seleccion.Row.ItemArray[10].ToString();
                txtCCimpuesto.Text = seleccion.Row.ItemArray[9].ToString();
                txtCpromedio.Text = seleccion.Row.ItemArray[8].ToString();
            }
        }

        private void SoloNumeros_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.OemComma || e.Key == Key.Tab)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void ObtenerCostos_KeyUp(object sender, KeyEventArgs e)
        {

            if (!String.IsNullOrEmpty(txtCostoUnitario.Text) && !String.IsNullOrEmpty(txtIVA.Text))
            {
                string value_ucosto = txtCostoUnitario.Text;
                string value_iva = txtIVA.Text;
                if (value_ucosto[0].ToString() == ",")
                {
                    txtCostoUnitario.Text = "0,";
                }
                if (value_iva[0].ToString() == ",")
                {
                    txtIVA.Text = "0,";
                }
                double ultimo_costo = Convert.ToDouble(txtCostoUnitario.Text);
                double costo_promedio = Convert.ToDouble(txtCostoUnitario.Text); ;
                double IVA = Convert.ToDouble(txtIVA.Text.ToString()) / 100;
                string costo_con_impuesto = txtCCimpuesto.Text;
                txtCCimpuesto.Text = Convert.ToString((ultimo_costo * IVA) + ultimo_costo);
                txtCpromedio.Text = Convert.ToString(ultimo_costo);
            }
            else
            {
                txtCpromedio.Clear();
                txtCCimpuesto.Clear();
            }


        }

        void clearFields()
        {
            cbxInsumos.SelectedItem = null;
            cbxAlmacen.SelectedItem = null;
            txtDescripcion.Clear();
            txtCpromedio.Clear();
            txtCostoUnitario.Clear();
            txtRendimiento.Clear();
            txtCantidad.Clear();
        }

        public void addToList()
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblInsumos.SelectedItem;
            if (seleccion != null)
            {
                if (cbxInsumos.SelectedItem != null) { seleccion["Insumo Base"] = cbxInsumos.SelectedItem.ToString(); }
                if (cbxAlmacen.SelectedItem != null) { seleccion["Almacén"] = cbxAlmacen.SelectedItem.ToString(); }
                seleccion["Descripción"] = txtDescripcion.Text;
                seleccion["Proveedor"] = txtRazonSocialP.Text;
                seleccion["Último Costo"] = txtCostoUnitario.Text;
                seleccion["Cantidad"] = txtCantidad.Text;
                seleccion["Unidad De Medida"] = txtUnidad.Text;
                seleccion["Costo Promedio"] = txtCpromedio.Text;
                seleccion["Costo Con Impuesto"] = txtCCimpuesto.Text;
                seleccion["IVA"] = txtIVA.Text;
                seleccion["Rendimiento"] = txtRendimiento.Text;
                seleccion["Codigo"] = txtCodigo.Text;


                tblInsumos.SelectedItem = seleccion;
                clearFields();
            }
            else
            {
                MessageBox.Show("Selecciona el insumo, ingresa la información faltante y agrega a la lista");
            }
        }

        private void btnGuardarList_Click(object sender, RoutedEventArgs e)
        {
            addToList();
        }

        public void guardarTodo()
        {

        }

        private void btnGuardarList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                addToList();
            }
        }

    }
}
