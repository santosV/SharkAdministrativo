using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Shapes;
using SharkAdministrativo.Modelo;
using System.Data;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using DevExpress.XtraEditors.Repository;
using System.Windows.Media;
using SharkAdministrativo.SDKCONTPAQi;

namespace SharkAdministrativo.Vista
{
    /// <summary>
    /// Lógica de interacción para GestionInsumosElaborados.xaml
    /// </summary>
    public partial class GestionInsumosElaborados : Window
    {
        Categoria categoria = new Categoria();
        Grupo grupo = new Grupo();
        Unidad_Medida unidad = new Unidad_Medida();
        DataTable dtIElaborados = new DataTable();
        DataTable dtReceta = new DataTable();
        DataTable dtProductos = new DataTable();
        InsumoElaborado insumoElaborado = new InsumoElaborado();
        Almacen almacen = new Almacen();
        Producto producto = new Producto();
        Insumo insumo = new Insumo();
        List<Insumo> insumos = new List<Insumo>();
        bool hasChanged = false;
        string from = "";

        public GestionInsumosElaborados()
        {
            InitializeComponent();
            cargarCombos();
            loadTitles();
            tblIElaborados.SelectedItem = null;
            loadTitlesRecipe();
            cargarTitlesProducts();
            cargarAreas();
            cargarClasificaciones();

        }





        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        public void loadTitles()
        {
            dtIElaborados.Columns.Add("ID");
            dtIElaborados.Columns.Add("Descripción");
            dtIElaborados.Columns.Add("Grupo");
            dtIElaborados.Columns.Add("Rendimiento");
            dtIElaborados.Columns.Add("Unidad De Medida");
            dtIElaborados.Columns.Add("Código");
            cargarInsumosElaborados();
            tblIElaborados.ItemsSource = dtIElaborados.DefaultView;
            tblIElaborados.Columns[0].Visible = false;

        }

        public void cargarReceta(string indicador, int id)
        {
            Receta receta = new Receta();
            dtReceta.Rows.Clear();
            List<Receta> ingredientes = receta.obtenerIngredientesDeReceta(indicador, id);
            double gasto = 0;
            double total = 0;
            foreach (var ingrediente in ingredientes)
            {

                string string_almacenes = null;
                String[] almacenes = ingrediente.almacenes_id.Split(';');
                foreach (var storage in almacenes)
                {
                    if (!String.IsNullOrEmpty(storage))
                    {
                        this.almacen = almacen.getForId(Convert.ToInt32(storage));
                        string_almacenes = almacen.nombre + ";";
                    }
                }
                string costo = Convert.ToString(ingrediente.Insumo.ultimo_costo);
                string cantidad = Convert.ToString(ingrediente.cantidad);
                gasto += Double.Parse(costo) * Double.Parse(cantidad);
                total += gasto;
                dtReceta.Rows.Add(ingrediente.id, ingrediente.Insumo.descripcion, ingrediente.cantidad, string_almacenes, gasto);
            }
            txtCostoReceta.Text = "Costo De Receta: $" + total;
        }
        public void cargarTitlesProducts()
        {
            dtProductos.Columns.Add("ID");
            dtProductos.Columns.Add("Descripción");
            dtProductos.Columns.Add("Nombre");
            dtProductos.Columns.Add("Último Precio");
            dtProductos.Columns.Add("IVA");
            dtProductos.Columns.Add("Costo C/ Impuesto");
            dtProductos.Columns.Add("Áreas De Preparación");
            dtProductos.Columns.Add("Disponlible");
            dtProductos.Columns.Add("Código");
            tblProductos.ItemsSource = dtProductos.DefaultView;
            cargarProductos();
            tblProductos.Columns[0].Visible = false;

        }

        private void cargarClasificaciones()
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

        void cargarProductos()
        {
            Producto producto = new Producto();
            List<Producto> productos = producto.obtenerTodos();
            dtProductos.Rows.Clear();


            foreach (var item in productos)
            {
                string areasPreparacion = "";
                string areasDisponibles = "";
                String[] areas_id = item.areasPreparacion.Split(';');
                String[] disponibles_id = item.disponlibleEn.Split(';');
                foreach (var a in areas_id)
                {
                    if (!String.IsNullOrEmpty(a))
                    {
                        AreaProduccion area = this.area.obtenerPorID(Convert.ToInt32(a));
                        areasPreparacion += area.nombre + ";";
                    }
                }
                foreach (var a in disponibles_id)
                {
                    if (!String.IsNullOrEmpty(a))
                    {
                        AreaProduccion area = this.area.obtenerPorID(Convert.ToInt32(a));
                        areasDisponibles += area.nombre + ";";
                    }
                }

                dtProductos.Rows.Add(item.id, item.descripcion, item.nombre, item.ultimoPrecio, item.IVA, item.precioConImpuesto, areasPreparacion, areasDisponibles,item.codigo);
            }
        }

        public void cargarIngredientes()
        {
            insumos = insumo.obtenerTodos();
            foreach (var item in insumos)
            {
                cbxIngredientes.Items.Add(item.descripcion);
            }
        }

        public void cargarAlmacenes()
        {
            List<Almacen> almacenes = almacen.obtenerTodos();
            foreach (var item in almacenes)
            {
                cbxAlmacenes.Items.Add(item.nombre);
            }
        }

        public void cargarInsumosElaborados()
        {
            dtIElaborados.Rows.Clear();
            List<InsumoElaborado> insumosElaborados = insumoElaborado.obtenerTodos();
            foreach (var IE in insumosElaborados)
            {
                dtIElaborados.Rows.Add(IE.id, IE.descripcion, IE.Grupo.nombre, IE.rendimiento, IE.Unidad_Medida.nombre, IE.codigo);
            }

        }

        private void btnGuardarUnidad_Click(object sender, RoutedEventArgs e)
        {
            if (txtNombreUnidad.Text != "")
            {
                Unidad_Medida medida = new Unidad_Medida();
                SDK.tUnidad unidadDeMedida = new SDK.tUnidad();
                unidadDeMedida.cNombreUnidad = txtNombreUnidad.Text;
                unidadDeMedida.cAbreviatura = txtNombreUnidad.Text.Substring(0, 1);
                medida.nombre = txtNombreUnidad.Text;
                Int32 cIdUnidadDeMedida = 0;
                int error = SDK.fAltaUnidad(ref cIdUnidadDeMedida, ref unidadDeMedida);

                if (error == 0)
                {

                    medida.registrar(medida);
                    llenarUnidades();
                    cbxUmedida.SelectedItem = unidadDeMedida.cNombreUnidad;
                    groupUnidades.Visibility = Visibility.Collapsed;
                }
                else
                {
                    SDK.rError(error);
                }
            }
        }

        public Boolean validarCampos()
        {
            bool validez = false;
            if (!String.IsNullOrEmpty(txtDescripcion.Text) && !String.IsNullOrEmpty(txtUCosto.Text) && !String.IsNullOrEmpty(txtCpromedio.Text) && !String.IsNullOrEmpty(txtEstandar.Text) && cbxInventariable.SelectedItem != null && cbxUmedida.SelectedItem != null && cbxGrupos.SelectedItem != null)
            {
                validez = true;
            }
            return validez;
        }

        public void guardarModificar()
        {

            if (validarCampos())
            {
                SDK.tProduto cProducto = new SDK.tProduto();
                cProducto.cNombreProducto = txtDescripcion.Text;
                cProducto.cCodigoProducto = txtCodigo.Text;
                cProducto.cDescripcionProducto = txtDescripcion.Text;
                cProducto.cTipoProducto = 1;
                cProducto.cMetodoCosteo = 1;
                String[] unidades = cbxUmedida.SelectedItem.ToString().Split('|');
                string idUnidad = unidades[0].Trim();
                cProducto.cPrecio1 = Double.Parse(txtUCosto.Text);
                String[] clasificacion = cbxValoresDeClasificaciones.SelectedItem.ToString().Split('|');
                string codigoClasificacion = clasificacion[0].Trim();


                InsumoElaborado insumo = new InsumoElaborado();
                insumo.descripcion = txtDescripcion.Text;
                insumo.costo_unitario = Double.Parse(txtUCosto.Text);
                insumo.costo_promedio = Double.Parse(txtCpromedio.Text);
                insumo.costo_estandar = Double.Parse(txtEstandar.Text);
                insumo.codigo = txtCodigo.Text;


                if (chksAutomatico.IsChecked == true)
                {
                    insumo.entrada_automatica = 1;
                }
                else
                {
                    insumo.entrada_automatica = 0;
                }
                insumo.inventariable = cbxInventariable.SelectedItem.ToString();
                insumo.rendimiento = Double.Parse(txtRendimiento.Text);
                insumo.Grupo = grupo.obtener(cbxGrupos.SelectedItem.ToString());
                string nameUnidad = unidades[1].Trim();
                insumo.Unidad_Medida = unidad.obtener(nameUnidad);
                insumo.unidad_id = insumo.Unidad_Medida.id;
                insumo.grupo_id = insumo.Grupo.id;
                if (tblIElaborados.SelectedItem == null)
                {
                    Int32 aldProducto = 0;
                    int error = SDK.fAltaProducto(ref aldProducto, ref cProducto);
                    if (error == 0)
                    {
                        SDK.fBuscaProducto(cProducto.cCodigoProducto);
                        SDK.fEditaProducto();
                        SDK.fSetDatoProducto("CIDVALORCLASIFICACION1", codigoClasificacion);
                        SDK.fSetDatoProducto("CBANUNIDADES", idUnidad);
                        SDK.fSetDatoProducto("CIDUNIDADBASE", idUnidad);
                        SDK.fSetDatoProducto("CCONTROLEXISTENCIA", "1");
                        SDK.fGuardaProducto();
                        insumo.registrar(insumo);
                        this.insumoElaborado = insumo;
                        clearFields();
                    }
                    else
                    {
                        SDK.rError(error);
                    }
                }
                else
                {
                    System.Data.DataRowView seleccion = (System.Data.DataRowView)tblIElaborados.SelectedItem;
                    insumo.id = Convert.ToInt32(seleccion.Row.ItemArray[0].ToString());
                    string codigoInsumo = txtCodigo.Text;
                    try
                    {
                        int error = SDK.fBuscaProducto(codigoInsumo);
                        SDK.fEditaProducto();
                        if (error == 0)
                        {
                            error = SDK.fSetDatoProducto("CNOMBREPRODUCTO", cProducto.cNombreProducto);
                            error = SDK.fSetDatoProducto("CCODIGOPRODUCTO", cProducto.cCodigoProducto);
                            error = SDK.fSetDatoProducto("CDESCRIPCIONPRODUCTO", cProducto.cNombreProducto);
                            error = SDK.fSetDatoProducto("CIMPUESTO1", Convert.ToString(cProducto.cImpuesto1));
                            error = SDK.fSetDatoProducto("CPRECIO1", Convert.ToString(cProducto.cPrecio1));
                            error = SDK.fSetDatoProducto("CIDVALORCLASIFICACION1", codigoClasificacion);
                            error = SDK.fSetDatoProducto("CBANUNIDADES", idUnidad);
                            error = SDK.fSetDatoProducto("CIDUNIDADBASE", idUnidad);
                            error = SDK.fSetDatoProducto("CCONTROLEXISTENCIA", "1");
                            error = SDK.fGuardaProducto();
                            if (error == 0)
                            {


                                insumoElaborado.modificar(insumo);
                            }
                            else
                            {
                                SDK.rError(error);
                            }
                        }
                    }
                    catch (System.ArithmeticException) { }
                }
                limpiarCampos();
                cargarInsumosElaborados();
            }

        }

        private void cbxGrupos_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbxGrupos.SelectedItem == "Nuevo")
            {
                txtnombreGrupo.Clear();
                cbxCategoria.Clear();
                cargarvista("grupo");
            }
            else
            {
                groupGrupo.Visibility = Visibility.Collapsed;
            }
        }

        public void cargarvista(string vista)
        {

            if (vista == "grupo")
            {
                cbxCategoria.Items.Clear();
                groupGrupo.Visibility = Visibility.Visible;
                List<Categoria> categorias = categoria.obtenerTodos();
                cbxCategoria.Items.Add("Nuevo");
                foreach (var category in categorias)
                {
                    cbxCategoria.Items.Add(category.nombre);
                }
                if (categoria.id > 0)
                {
                    cbxCategoria.SelectedItem = categoria.nombre;
                }
            }
            else if (vista == "categoria")
            {
                groupCategoria.Visibility = Visibility.Visible;
            }
        }

        private void SoloNumeros_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.OemPeriod || e.Key == Key.Tab)
                e.Handled = false;
            else
                e.Handled = true;

        }

        private void cbxUmedida_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbxUmedida.SelectedItem == "Nuevo")
            {
                groupUnidades.Visibility = Visibility.Visible;
            }
            else
            {
                groupUnidades.Visibility = Visibility.Collapsed;
            }

        }

        private void btnGuardarGrupo_Click(object sender, RoutedEventArgs e)
        {
            if (txtnombreGrupo.Text != "" && cbxCategoria.SelectedItem.ToString() != "" && cbxCategoria.SelectedItem != null)
            {
                grupo.nombre = txtnombreGrupo.Text;
                grupo.Categoria = categoria.obtener(cbxCategoria.SelectedItem.ToString());
                grupo.registrar(grupo);
                if (grupo.id > 0)
                {
                    llenarGrupos();
                    cbxGrupos.SelectedItem = grupo.nombre;
                }
                groupGrupo.Visibility = Visibility.Collapsed;
            }
        }

        private void llenarGrupos()
        {
            cbxGrupos.Items.Clear();
            List<Grupo> grupos = grupo.obtenerTodos();
            cbxGrupos.Items.Add("Nuevo");
            foreach (var group in grupos)
            {
                cbxGrupos.Items.Add(group.nombre);
            }
            cbxGrupos.SelectedItem = grupo.nombre;


        }

        private void cargarCombos()
        {
            cbxInventariable.Items.Add("Si");
            cbxInventariable.Items.Add("No");
            llenarGrupos();
            llenarUnidades();
        }

        private void llenarUnidades()
        {

            cbxUmedida.Items.Clear();
            cbxUmedida.Items.Add("Nuevo");
            int error = SDK.fPosPrimerUnidad();
            while (error == 0)
            {
                StringBuilder nomUnidad = new StringBuilder(20);
                StringBuilder idUnidad = new StringBuilder(5);
                SDK.fLeeDatoUnidad("CNOMBREUNIDAD", nomUnidad, 20);
                SDK.fLeeDatoUnidad("CIDUNIDAD", idUnidad, 5);

                if (nomUnidad.ToString() != "(Ninguno)")
                {

                    cbxUmedida.Items.Add(idUnidad + " | " + nomUnidad);
                }
                error = SDK.fPosSiguienteUnidad();
            }

        }

        private void btnGuardarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (txtCategoria.Text != "")
            {
                categoria.nombre = txtCategoria.Text;
                categoria.registrar(categoria);
                if (categoria.id > 0)
                {
                    cargarvista("grupo");
                }
                groupCategoria.Visibility = Visibility.Collapsed;
            }
        }

        private void cbxCategoria_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbxCategoria.SelectedItem == "Nuevo")
            {
                btnGuardarGrupo.Visibility = Visibility.Collapsed;
                cargarvista("categoria");
                groupGrupo.Visibility = Visibility.Visible;
            }
            else
            {
                btnGuardarGrupo.Visibility = Visibility.Visible;
                cargarvista("grupo");
                groupCategoria.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveAndNew_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            guardarModificar();
        }

        private void eliminarIElaborado()
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblIElaborados.SelectedItem;
            if (seleccion != null)
            {
                MessageBoxResult dialogResult = MessageBox.Show("¿Está seguro de eliminar el insumo '" + seleccion.Row.ItemArray[1] + "'?", "Eliminación de Insumo Elaborado", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    int error = SDK.fEliminarCteProv(txtCodigo.Text);

                    if (error == 0)
                    {

                        insumoElaborado.eliminar(Convert.ToInt32(seleccion.Row.ItemArray[0].ToString()));
                        seleccion.Delete();
                        clearFields();
                    }
                    else
                    {
                        SDK.rError(error);
                    }

                }
            }
            else
            {
                MessageBox.Show("SELECCIONA EL INSUMO QUE DESEAS ELIMINAR");
            }

        }


        public void clearFields()
        {
            cbxGrupos.SelectedItem = null;
            cbxInventariable.SelectedItem = null;
            cbxUmedida.SelectedItem = null;
            txtDescripcion.Clear();
            txtUCosto.Clear();
            txtCpromedio.Clear();
            txtEstandar.Clear();
            txtRendimiento.Clear();
            chksAutomatico.IsChecked = false;
            txtCodigo.Clear();
            cbxValoresDeClasificaciones.SelectedItem = null;
            tblIElaborados.SelectedItem = false;
            groupInsumoElaborado.Header = "Nuevo Insumo Elaborado";
            Title.Text = "Gestion De Insumos Elaborados";
        }

        public void limpiarCampos()
        {
            cbxGrupos.SelectedItem = null;
            cbxInventariable.SelectedItem = null;
            cbxUmedida.SelectedItem = null;
            txtDescripcion.Clear();
            txtUCosto.Clear();
            txtCpromedio.Clear();
            txtEstandar.Clear();
            txtRendimiento.Clear();
            chksAutomatico.IsChecked = false;
            txtCodigo.Clear();
            cbxValoresDeClasificaciones.SelectedItem = null;
            Title.Text = "Gestion De Insumos Elaborados";
        }


        private void ObtenerCostos_KeyUp(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtUCosto.Text))
            {
                txtCpromedio.Text = txtUCosto.Text;
                txtEstandar.Text = Convert.ToString(Double.Parse(txtUCosto.Text.ToString()) - 2.1);
            }
        }

        private void ObtenerCostosProductos_KeyUp(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtUPrecioP.Text) && !String.IsNullOrEmpty(txtIVAP.Text))
            {
                try
                {
                    string costop = txtUPrecioP.Text;
                    string ivap = txtIVAP.Text;
                    double precioImpuesto = (Double.Parse(costop) * (Double.Parse(ivap) / 100)) + Double.Parse(costop);
                    txtPCImpuestoP.Text = Convert.ToString(precioImpuesto);
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
            else
            {
                txtPCImpuestoP.Clear();
            }
        }

        public void showView(int vista)
        {
            esconderVistas();
            if (vista == 1)
            {
                Title.Text = "Gestión De Insumos Elaborados";
                vista_InsumosElaborados.Visibility = Visibility.Visible;
                btnGeneral.IsVisible = true;
            }
            else if (vista == 2)
            {
                vista_Receta.Visibility = Visibility.Visible;
                btnADDRECETAMENU.IsVisible = true;
            }
            else if (vista == 3)
            {
                Title.Text = "Gestión De Productos";
                vista_Producto.Visibility = Visibility.Visible;
                btnProducto.IsVisible = true;
            }
        }

        private void esconderVistas()
        {
            vista_Producto.Visibility = Visibility.Collapsed;
            vista_Receta.Visibility = Visibility.Collapsed;
            vista_InsumosElaborados.Visibility = Visibility.Collapsed;
            btnGeneral.IsVisible = false;
            btnADDRECETAMENU.IsVisible = false;
            btnProducto.IsVisible = false;

        }

        private void goToAddRecipe_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (tblIElaborados.SelectedItem != null)
            {
                from = "IE";
                Title.Text = "Receta de '" + insumoElaborado.descripcion + "'";
                showView(2);
                cargarReceta("IE", this.insumoElaborado.id);
                hasChanged = false;
            }
            else if (validarCampos() == true)
            {
                from = "IE";
                guardarModificar();
                Title.Text = "Receta de '" + insumoElaborado.descripcion + "'";
                showView(2);
                cargarReceta("IE", this.insumoElaborado.id);
                hasChanged = false;

            }
            else
            {
                MessageBox.Show("La andas cagando papu :V");
            }
        }

        private void goToAddRecipeFromProducts_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (tblProductos.SelectedItem != null)
            {
                from = "P";
                Title.Text = "Receta de '" + producto.descripcion + "'";
                showView(2);
                cargarReceta("P", this.producto.id);
                hasChanged = false;
            }
            else if (validarCampos() == true)
            {
                from = "P";
                guardarModificarProducto();
                Title.Text = "Receta de '" + producto.descripcion + "'";
                showView(2);
                cargarReceta("P", this.producto.id);
                hasChanged = false;

            }
            else
            {
                MessageBox.Show("La andas cagando papu :V");
            }
        }

        private void btnListo_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (hasChanged)
            {
                MessageBox.Show("SE GUARDARON LOS CAMBIOS CORRECTAMENTE");
            }
            clearFields();
            if (from == "IE")
            {
                showView(1);
            }
            else
            {
                showView(3);
            }

        }


        private void tblIElaborados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblIElaborados.SelectedItem;

            if (seleccion != null)
            {
                txtCodigo.IsReadOnly = true;
                int error = SDK.fBuscaProducto(seleccion.Row.ItemArray[5].ToString());
                SDK.rError(error);
                StringBuilder idValorClasificacion = new StringBuilder(5);
                SDK.fLeeDatoProducto("CIDVALORCLASIFICACION1", idValorClasificacion, 5);
                SDK.fBuscaIdValorClasif(Convert.ToInt32(idValorClasificacion.ToString()));
                StringBuilder codValorClasificacion = new StringBuilder(11);
                StringBuilder nomValorClasificacion = new StringBuilder(30);
                StringBuilder idUnidad = new StringBuilder(5);
                SDK.fLeeDatoValorClasif("CCODIGOVALORCLASIFICACION", codValorClasificacion, 11);
                SDK.fLeeDatoValorClasif("CVALORCLASIFICACION", nomValorClasificacion, 30);
                SDK.fLeeDatoProducto("CIDUNIDADBASE", idUnidad, 5);
                cbxValoresDeClasificaciones.SelectedItem = codValorClasificacion + " | " + nomValorClasificacion;
                this.insumoElaborado = insumoElaborado.getForId(Convert.ToInt32(seleccion.Row.ItemArray[0].ToString()));
                txtDescripcion.Text = insumoElaborado.descripcion;
                cbxGrupos.SelectedItem = seleccion.Row.ItemArray[2].ToString();
                cbxInventariable.SelectedItem = insumoElaborado.inventariable;
                cbxUmedida.SelectedItem = idUnidad + " | " + seleccion.Row.ItemArray[4].ToString();
                txtUCosto.Text = Convert.ToString(insumoElaborado.costo_unitario);
                txtCpromedio.Text = Convert.ToString(insumoElaborado.costo_promedio);
                txtEstandar.Text = Convert.ToString(insumoElaborado.costo_estandar);
                txtRendimiento.Text = Convert.ToString(insumoElaborado.rendimiento);
                chksAutomatico.IsChecked = false;
                groupInsumoElaborado.Header = "MODIFICANDO INSUMO " + insumoElaborado.descripcion;
                txtCodigo.Text = seleccion.Row.ItemArray[5].ToString();
                if (insumoElaborado.entrada_automatica == 1)
                {
                    chksAutomatico.IsChecked = true;
                }
            }
        }

        private void Nuevo_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            clearFields();
        }

        private void btnEliminar_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            eliminarIElaborado();
        }

        private void cbxIngredientes_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbxIngredientes.SelectedItem != null)
            {
                foreach (var item in insumos)
                {
                    if (item.descripcion == cbxIngredientes.SelectedItem.ToString())
                    {
                        txtUnidadMedida.Text = item.Unidad_Medida.nombre;
                    }
                }
            }
            else
            {
                txtUnidadMedida.Text = "...";
            }
        }

        public void loadTitlesRecipe()
        {
            dtReceta.Columns.Add("ID");
            dtReceta.Columns.Add("Ingrediente / Insumo");
            dtReceta.Columns.Add("Cantidad");
            dtReceta.Columns.Add("Almacenes De Descarga");
            dtReceta.Columns.Add("Gasto");
            cargarIngredientes();
            cargarAlmacenes();
            tblReceta.ItemsSource = dtReceta.DefaultView;
            tblReceta.Columns[0].Visible = false;
            tblReceta.SelectedItem = false;


        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtCantidadR.Text) && cbxIngredientes.SelectedItem != null && cbxAlmacenes.SelectedItem != null)
            {
                hasChanged = true;
                double total = 0;
                string costo = null;
                string almacenes = "";
                Receta receta = new Receta();
                receta.Insumo = insumo.obtener(cbxIngredientes.SelectedItem.ToString());
                receta.insumo_id = receta.Insumo.id;
                receta.cantidad = Double.Parse(txtCantidadR.Text);
                costo = Convert.ToString(receta.Insumo.ultimo_costo);
                double gasto = Double.Parse(costo) * Double.Parse(txtCantidadR.Text);
                if (this.insumoElaborado != null)
                {
                    receta.InsumoElaborado = insumoElaborado.getForId(insumoElaborado.id);
                    receta.insumoElaborado_id = receta.InsumoElaborado.id;
                }
                else
                {
                    receta.Producto = producto.obtenerPorID(producto.id);
                    receta.producto_id = receta.producto_id;
                }

                String[] obtenerTotal = txtCostoReceta.Text.Split('$');
                total += Double.Parse(obtenerTotal[1]);
                total += gasto;
                txtCostoReceta.Text = "Costo De Receta: $" + total;

                foreach (var item in cbxAlmacenes.SelectedItems)
                {
                    if (!String.IsNullOrEmpty(item.ToString()))
                    {
                        almacenes += item.ToString() + ";";
                        receta.almacenes_id += almacen.obtenerID(item.ToString()) + ";";
                    }
                }
                if (tblReceta.SelectedItem == null)
                {
                    receta.registrar(receta);
                    dtReceta.Rows.Add(receta.id, receta.Insumo.descripcion, receta.cantidad, almacenes, gasto);
                }
                else
                {
                    System.Data.DataRowView seleccion = (System.Data.DataRowView)tblReceta.SelectedItem;
                    receta.id = Convert.ToInt32(seleccion.Row.ItemArray[0].ToString());
                    receta.modificar(receta);
                    if (receta.InsumoElaborado != null)
                    {
                        cargarReceta("IE", this.insumoElaborado.id);
                    }
                    else
                    {
                        cargarReceta("P", this.producto.id);
                    }
                }


                clearFieldsReceta();
            }
        }


        public void clearFieldsReceta()
        {
            cbxIngredientes.SelectedItem = null;
            cbxAlmacenes.SelectedItem = null;
            txtCantidadR.Clear();
        }

        private void tblReceta_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblReceta.SelectedItem;
            if (seleccion != null)
            {
                cbxIngredientes.SelectedItem = seleccion.Row.ItemArray[1].ToString();
                txtCantidadR.Text = seleccion.Row.ItemArray[2].ToString();
                String[] almacenes = seleccion.Row.ItemArray[3].ToString().Split(';');
                foreach (var storage in almacenes)
                {
                    cbxAlmacenes.SelectedItems.Add(storage);
                }

            }
        }

        private void EliminarIngredienteDeReceta_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblReceta.SelectedItem;
            if (seleccion != null)
            {
                MessageBoxResult dialogResult = MessageBox.Show("¿Está seguro de eliminar el ingrediente '" + seleccion.Row.ItemArray[1] + "' de la receta?", "Eliminación de Ingrediente", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    hasChanged = true;
                    Receta ingrediente = new Receta();
                    ingrediente.id = Convert.ToInt32(seleccion.Row.ItemArray[0].ToString());
                    ingrediente.eliminarIngrediente(ingrediente);
                    seleccion.Delete();
                    clearFieldsReceta();
                }
            }
            else
            {
                MessageBox.Show("Es Necesario que seleccione el ingrediente que desea elimianr");
            }
        }

        private void btnNuevaArea_Click(object sender, RoutedEventArgs e)
        {

            if (btnNuevaArea.Content.ToString() == "Nuevo")
            {
                vistaNAP.Visibility = Visibility.Visible;
                btnNuevaArea.Content = "Cancelar";
            }
            else if (btnNuevaArea.Content.ToString() == "Cancelar")
            {
                vistaNAP.Visibility = Visibility.Collapsed;
                btnNuevaArea.Content = "Nuevo";
            }
        }
        AreaProduccion area = new AreaProduccion();
        private void cargarAreas()
        {
            cbxPreparacion.Items.Clear();
            cbxAreaDisponible.Items.Clear();
            List<AreaProduccion> areas = area.obtenerTodos();
            foreach (var item in areas)
            {
                cbxAreaDisponible.Items.Add(item.nombre);
                cbxPreparacion.Items.Add(item.nombre);
            }
        }


        private void btnGuardarArea_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtNombreArea.Text))
            {
                AreaProduccion area = new AreaProduccion();
                area.nombre = txtNombreArea.Text;
                area.registrar(area);
                if (area.id > 0)
                {
                    txtNombreArea.Clear();
                    cargarAreas();
                    btnNuevaArea.Content = "Nuevo";
                    vistaNAP.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MessageBox.Show("Es Necesario que ingrese el nombre de la nueva área de producción");
            }

        }

        private void GuardarYNuevo_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (validarDatosProducto() == true)
            {
                guardarModificarProducto();
            }
            else
            {
                MessageBox.Show("EXISTEN CAMPOS IMPORTANTES SIN INGRESAR");
            }

        }

        public Boolean validarDatosProducto()
        {
            bool validacion = false;
            if (!String.IsNullOrEmpty(txtDescripcionP.Text) && !String.IsNullOrEmpty(txtNombreP.Text) && !String.IsNullOrEmpty(txtPCImpuestoP.Text) && !String.IsNullOrEmpty(txtUPrecioP.Text) && !String.IsNullOrEmpty(txtIVAP.Text) && cbxAreaDisponible.SelectedItem != null && cbxPreparacion.SelectedItem != null)
            {
                validacion = true;
            }
            return validacion;
        }

        public void guardarModificarProducto()
        {
            if (validarDatosProducto())
            {

                SDK.tProduto cProducto = new SDK.tProduto();
                cProducto.cCodigoProducto = txtCodigoP.Text;
                cProducto.cNombreProducto = txtDescripcionP.Text;
                cProducto.cDescripcionProducto = txtDescripcionP.Text;
                String[] clasificacion = cbxValoresDeClasificacionesP.SelectedItem.ToString().Split('|');
                string codigoClasificacion = clasificacion[0].Trim();
                cProducto.cPrecio1 = Double.Parse(txtUPrecioP.Text);
                cProducto.cImpuesto1 = Double.Parse(txtIVAP.Text);
                cProducto.cTipoProducto = 1;
                cProducto.cMetodoCosteo = 1;
                producto.codigo = txtCodigoP.Text;
                producto.descripcion = txtDescripcionP.Text;
                producto.areasPreparacion = "";
                producto.disponlibleEn = "";
                producto.IVA = Double.Parse(txtIVAP.Text);
                producto.nombre = txtNombreP.Text;
                producto.precioConImpuesto = Double.Parse(txtPCImpuestoP.Text);
                producto.ultimoPrecio = Double.Parse(txtUPrecioP.Text);
                foreach (var item in cbxPreparacion.SelectedItems)
                {
                    if (!String.IsNullOrEmpty(item.ToString()))
                    {
                        AreaProduccion area = this.area.obtener(item.ToString());
                        producto.areasPreparacion += area.id + ";";
                    }
                }
                foreach (var item in cbxAreaDisponible.SelectedItems)
                {
                    if (!String.IsNullOrEmpty(item.ToString()))
                    {
                        AreaProduccion area = this.area.obtener(item.ToString());
                        producto.disponlibleEn = area.id + ";";
                    }
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
                        producto.imagen = convertirAByte(imagen);
                    }
                }



                if (tblProductos.SelectedItem == null)
                {
                    Int32 aldProducto = 0;
                    int error = SDK.fAltaProducto(ref aldProducto, ref cProducto);
                    if (error == 0)
                    {
                        SDK.fEditaProducto();
                        SDK.fSetDatoProducto("CIDVALORCLASIFICACION1", codigoClasificacion);
                        SDK.fGuardaProducto();
                        producto.registrar(producto);
                        MessageBox.Show("Se registró el producto " + producto.nombre);
                    }
                    else
                    {
                        SDK.rError(error);
                    }
                }
                else
                {
                    int error = SDK.fBuscaProducto(txtCodigoP.Text);
                    SDK.fEditaProducto();
                    if (error == 0)
                    {
                        error = SDK.fSetDatoProducto("CNOMBREPRODUCTO", cProducto.cNombreProducto);
                        error = SDK.fSetDatoProducto("CCODIGOPRODUCTO", cProducto.cCodigoProducto);
                        error = SDK.fSetDatoProducto("CDESCRIPCIONPRODUCTO", cProducto.cNombreProducto);
                        error = SDK.fSetDatoProducto("CIMPUESTO1", Convert.ToString(cProducto.cImpuesto1));
                        error = SDK.fSetDatoProducto("CPRECIO1", Convert.ToString(cProducto.cPrecio1));
                        error = SDK.fSetDatoProducto("CIDVALORCLASIFICACION1", codigoClasificacion);
                        error = SDK.fGuardaProducto();
                        if (error == 0)
                        {
                            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblProductos.SelectedItem;
                            Producto p = new Producto();
                            p = producto.obtenerPorID(Convert.ToInt32(seleccion.Row.ItemArray[0].ToString()));
                            producto.id = p.id;
                            producto.modificar(producto);
                        }
                        else {
                            SDK.rError(error);
                        }
                    }
                    
                }
                cargarProductos();
                clearFieldsProducts();
            }
        }

        public void clearFieldsProducts()
        {
            this.Title.Text = "Gestión De Productos";
            txtDescripcionP.Clear();
            txtNombreP.Clear();
            txtUPrecioP.Clear();
            txtIVAP.Clear();
            txtCodigoP.Clear();
            cbxValoresDeClasificacionesP.SelectedItem = null;
            txtPCImpuestoP.Clear();
            cbxAreaDisponible.SelectedItem = null;
            cbxPreparacion.SelectedItem = null;
            tblProductos.SelectedItem = false;
            img.Source = null;
            txtCortoName.Text = "Sin Nombre Disponible";
            groupProducto.Header = "Nuevo Producto";
            txtCodigoP.IsReadOnly = false;
        }


        public byte[] convertirAByte(System.Drawing.Image imagen)
        {
            System.Drawing.ImageConverter imgCon = new System.Drawing.ImageConverter();
            return (byte[])imgCon.ConvertTo(imagen, typeof(byte[]));
        }

        public System.Windows.Media.Imaging.BitmapImage convertirAImagen(byte[] bytes)
        {
            using (MemoryStream mStream = new MemoryStream(bytes))
            {
                //System.Drawing.Bitmap img = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(mStream);
                System.Windows.Media.Imaging.BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();
                bImg.BeginInit();
                bImg.StreamSource = new MemoryStream(mStream.ToArray());
                bImg.EndInit();
                return bImg;
            }
        }




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


        private void tblProductos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblProductos.SelectedItem;

            if (seleccion != null)
            {
                this.producto = this.producto.obtenerPorID(Convert.ToInt32(seleccion.Row.ItemArray[0].ToString()));
                cbxAreaDisponible.SelectedItem = null;
                cbxPreparacion.SelectedItem = null;
                Producto producto = new Producto();
                producto = producto.obtenerPorID(Convert.ToInt32(seleccion.Row.ItemArray[0].ToString()));
                if (producto.imagen != null)
                {
                    BitmapImage img1 = convertirAImagen(producto.imagen);
                    img.Source = img1;
                }
                txtCortoName.Text = producto.nombre;
                txtDescripcion.Text = producto.descripcion;
                txtPCImpuestoP.Text = Convert.ToString(producto.precioConImpuesto);
                txtUPrecioP.Text = Convert.ToString(producto.ultimoPrecio);
                txtDescripcionP.Text = producto.descripcion;
                txtNombreP.Text = producto.nombre;
                txtIVAP.Text = Convert.ToString(producto.IVA);
                String[] produccion = seleccion.Row.ItemArray[6].ToString().Split(';');
                String[] disponible = seleccion.Row.ItemArray[7].ToString().Split(';');
                groupProducto.Header = "MODIFICANDO " + producto.descripcion;
                txtCodigoP.Text = seleccion.Row.ItemArray[8].ToString();
                txtCodigoP.IsReadOnly = true;
                foreach (var item in produccion)
                {
                    if (!String.IsNullOrEmpty(item.ToString()))
                    {
                        cbxPreparacion.SelectedItems.Add(item);
                    }
                }
                foreach (var item in disponible)
                {
                    if (!String.IsNullOrEmpty(item.ToString()))
                    {
                        cbxAreaDisponible.SelectedItems.Add(item);
                    }
                }

                SDK.fBuscaProducto(seleccion.Row.ItemArray[8].ToString());
                StringBuilder idValorClasificacion = new StringBuilder(5);
                SDK.fLeeDatoProducto("CIDVALORCLASIFICACION1", idValorClasificacion, 5);
                SDK.fBuscaIdValorClasif(Convert.ToInt32(idValorClasificacion.ToString()));
                StringBuilder codValorClasificacion = new StringBuilder(11);
                StringBuilder nomValorClasificacion = new StringBuilder(30);
                SDK.fLeeDatoValorClasif("CCODIGOVALORCLASIFICACION", codValorClasificacion, 11);
                SDK.fLeeDatoValorClasif("CVALORCLASIFICACION", nomValorClasificacion, 30);

                cbxValoresDeClasificacionesP.SelectedItem = codValorClasificacion + " | " + nomValorClasificacion;
            }

        }

        private void txtNombreP_KeyUp(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtNombreP.Text))
            {
                txtCortoName.Text = txtNombreP.Text;
            }
            else
            {
                txtCortoName.Text = "Sin Nombre Disponible";
            }
        }

        private void eliminarProducto_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblProductos.SelectedItem;
            if (seleccion != null)
            {
                MessageBoxResult dialogResult = MessageBox.Show("¿ESTÁ SEGURO DE ELIMINAR EL PRODUCTO '" + seleccion.Row.ItemArray[1] + "'?", "ELIMINACIÓN DE PRODUCTO", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    int error = SDK.fEliminarCteProv(txtCodigoP.Text);

                    if (error == 0)
                    {
                        Producto producto = new Producto();
                        producto.id = Convert.ToInt32(seleccion.Row.ItemArray[0].ToString());
                        producto.eliminar(producto);
                        cargarProductos();
                        clearFieldsProducts();
                    }
                    else {
                        SDK.rError(error);
                    }
                }
            }
            else
            {
                MessageBox.Show("EN NECESARIO QUE SELECCIONE EL PRODUCTO QUE DESEA ELIMINAR");
            }
        }

        private void NuevoProducto_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            clearFieldsProducts();
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

        private void insumosElaborados_ExportToExcel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".xsls", tablaIElaborado, "InsumosElaborados");
        }

        private void insumosElaborados_ExportToPDF_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".pdf", tablaIElaborado, "InsumosElaborados");
        }

        private void insumosElaborados_ExportToPNG_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".png", tablaIElaborado, "InsumosElaborados");
        }

        private void productos_ExportToExcel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".xsls", tablaProductos, "InsumosElaborados");
        }

        private void productos_ExportToPDF_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".pdf", tablaProductos, "InsumosElaborados");
        }

        private void productos_ExportToPNG_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".png", tablaProductos, "InsumosElaborados");
        }


        private void btnProductReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ReportsView.ProductView vista = new ReportsView.ProductView();
            vista.Show();
        }

        private void btnRecipeReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ReportsView.RecipeView vista = new ReportsView.RecipeView();
            vista.Show();
        }


    }
}
