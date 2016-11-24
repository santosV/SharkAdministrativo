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
            llenarGrupos();
        }

        /// <summary>
        /// Bloquea los textfields cuando el proveedor ya está registrado.
        /// </summary>
        public void bloquearCajas()
        {
            validacion = proveedor.validar(proveedor);
            proveedor = proveedor.obtener(proveedor.nombre);
            if (validacion != "unico")
            {
                int error = SDK.fBuscaCteProv(proveedor.codigo);
                if (error == 0)
                {
                    String[] grupos = proveedor.tipos_proveedor.Split(';');
                    int i = 1;
                    foreach (string group in grupos)
                    {
                        if (!String.IsNullOrEmpty(group))
                        {
                            StringBuilder cIdValorClasificacionProv = new StringBuilder(5);
                            SDK.fLeeDatoCteProv("CIDVALORCLASIFPROVEEDOR"+i, cIdValorClasificacionProv, 5);
                            i++;
                            cbxGrupos.SelectedItems.Add(cIdValorClasificacionProv + " | " + group);
                        }
                    }
                    txtcodigoProveedor.Text = proveedor.codigo;
                }
                cbxGrupos.IsReadOnly = true;
                txtcodigoProveedor.IsReadOnly = true;
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


        public void llenarGrupos()
        {
            int i = 13;
            int error = SDK.fPosPrimerValorClasif();
            while (error == 0)
            {
                StringBuilder cCodClasificacion = new StringBuilder(5);
                SDK.fLeeDatoValorClasif("CIDCLASIFICACION", cCodClasificacion, 5);
                StringBuilder cNameValorClasificacion = new StringBuilder(20);
                SDK.fLeeDatoValorClasif("CVALORCLASIFICACION", cNameValorClasificacion, 20);
                if (!cCodClasificacion.ToString().Equals(Convert.ToString(i)))
                {
                    error = SDK.fPosSiguienteValorClasif();
                }
                else
                {
                    if ((cCodClasificacion.ToString().Equals(Convert.ToString(i)) && cNameValorClasificacion.ToString().Equals("(Ninguna)")))
                    {
                        error = SDK.fPosSiguienteValorClasif();
                        i++;
                    }
                    else
                    {
                        error = 1;
                    }
                }
            }
            Grupo grupo = new Grupo();
            cbxGrupos.Items.Clear();
            List<Grupo> grupos = grupo.obtenerTodos();
            foreach (var item in grupos)
            {
                StringBuilder cCodValorClasificacion = new StringBuilder(5);
                SDK.fLeeDatoValorClasif("CIDVALORCLASIFICACION", cCodValorClasificacion, 5);
                cbxGrupos.Items.Add(cCodValorClasificacion + " | " + item.nombre);
                SDK.fPosSiguienteValorClasif();
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
                    txtEstadoP.Text != "" && txtRfcP.Text != "" && txtNoExteriorP.Text != "" && !String.IsNullOrEmpty(txtcodigoProveedor.Text) && cbxGrupos.SelectedItem != null)
                    {
                        SDK.CteProv cProveedor = new SDK.CteProv();

                        cProveedor.cCodigoCliente = txtcodigoProveedor.Text;
                        cProveedor.cRazonSocial = txtRazonSocialP.Text;
                        cProveedor.cRFC = txtRfcP.Text;
                        cProveedor.cDenComercial = txtRazonSocialP.Text;
                        cProveedor.cEstatus = 1;

                        Empresa empresa = new Empresa();
                        Proveedor proveedor = new Proveedor();
                        proveedor.codigo = txtcodigoProveedor.Text;
                        proveedor.nombre = txtRazonSocialP.Text;
                        proveedor.razon_social = txtRazonSocialP.Text;
                        proveedor.RFC = txtRfcP.Text;
                        proveedor.sucursal = txtSucursalP.Text;
                        proveedor.calle = txtCalleP.Text;
                        proveedor.codigo_postal = txtCodigoPostalP.Text;
                        proveedor.colonia = txtColoniaP.Text;
                        proveedor.Empresa = empresa.obtenerPorNombre("Baja Salads");
                        proveedor.empresa_id = proveedor.Empresa.id;
                        proveedor.localidad = txtLocalidadP.Text;
                        proveedor.municipio = txtMunicipioP.Text;
                        proveedor.estado = txtEstadoP.Text;
                        proveedor.NoExterior = txtNoExteriorP.Text;
                        proveedor.pais = txtPaisP.Text;
                        DateTime thisDay = DateTime.Today;
                        proveedor.fecha_registro = Convert.ToDateTime(thisDay.ToString());
                        List<string> cIDClasificacionesGrupos = new List<string>();

                        foreach (var grupos in cbxGrupos.SelectedItems)
                        {
                            String[] groups = grupos.ToString().Split('|');
                            cIDClasificacionesGrupos.Add(groups[0].ToString().Trim());
                            proveedor.tipos_proveedor += groups[1].ToString().Trim() + ";";
                        }
                        int cIDCteProv = 0;
                        int error = SDK.fAltaCteProv(ref cIDCteProv, ref  cProveedor);
                        if (error == 0)
                        {
                            proveedor.registrar(proveedor);
                            MessageBox.Show("ÉXITO, SE REGISTRÓ AL PROVEEDOR '" + proveedor.razon_social + "'");
                            SDK.fBuscaIdCteProv(cIDCteProv);
                            SDK.fEditaCteProv();
                            SDK.fSetDatoCteProv("CTIPOCLIENTE", "3");
                            SDK.fSetDatoCteProv("CIDMONEDA", "1");
                            int i = 1;
                            foreach (var item in cIDClasificacionesGrupos)
                            {
                                SDK.fSetDatoCteProv("CIDVALORCLASIFPROVEEDOR" + i, item);
                                i++;
                            }
                            SDK.fGuardaCteProv();
                        }
                        else
                        {
                            SDK.rError(error);
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Hay campos importantes que están vacíos, pueden ser en la tabla de productos o en los campos de proveedor");
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
            double folio = 0;

            StringBuilder serie = new StringBuilder(12);

            factura.registrar(factura);

            SDK.fSiguienteFolio("21", serie, ref folio);

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


            }

            SDK.tDocumento lDocto = new SDK.tDocumento();

            lDocto.aCodConcepto = "21";
            lDocto.aCodigoAgente = "(Ninguno)";
            lDocto.aNumMoneda = 1;
            lDocto.aTipoCambio = 1;

            lDocto.aImporte = 0;
            lDocto.aDescuentoDoc1 = 0;
            lDocto.aDescuentoDoc2 = 0;
            lDocto.aAfecta = 0;
            lDocto.aSistemaOrigen = 205;
            lDocto.aCodigoCteProv = proveedor.codigo;
            lDocto.aFolio = folio;
            lDocto.aSistemaOrigen = 205;
            lDocto.aSerie = factura.folio;
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

                return;
            }

            foreach (var presentacion in presentaciones)
            {


                SDK.tMovimiento ltMovimiento = new SDK.tMovimiento();
                int lIdMovimiento = 0;

                SDK.fBuscaAlmacen(presentacion.Almacen.id.ToString());
                StringBuilder codigo = new StringBuilder(20);
                SDK.fLeeDatoAlmacen("CCODIGOALMACEN", codigo, 20);
                ltMovimiento.aCodAlmacen = codigo.ToString();
                ltMovimiento.aConsecutivo = 1;

                ltMovimiento.aCodProdSer = presentacion.codigo;

                ltMovimiento.aUnidades = Double.Parse(Convert.ToString(presentacion.cantidad));


                ltMovimiento.aCosto = Double.Parse(Convert.ToString(presentacion.costo_con_impuesto));


                lError = 0;
                lError = SDK.fAltaMovimiento(lIdDocumento, ref lIdMovimiento, ref ltMovimiento);

                if (lError != 0)
                {
                    SDK.rError(lError);
                    return;
                }
                else
                {
                    //entrada almacen shark

                    EntradaPresentacion entrada = new EntradaPresentacion();
                    DateTime thisDay = DateTime.Today;
                    entrada.fecha_registro = Convert.ToDateTime(thisDay.ToString());
                    Presentacion presentacionR = presentacion.obtener(presentacion);
                    entrada.Presentacion = presentacionR;

                    entrada.Almacen = almacen.obtener(presentacion.Almacen.nombre);
                    entrada.cantidad = presentacion.cantidad;
                    entrada.registrar(entrada);
                }
            }



            if (lError == 0)
            {
                System.Windows.Forms.MessageBox.Show("Se registraron correctamente los datos de proveedor y productos!");
            }


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
