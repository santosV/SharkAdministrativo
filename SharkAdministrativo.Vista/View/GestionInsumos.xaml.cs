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
using System.Text.RegularExpressions;
using System.Data;
using SharkAdministrativo.SDKCONTPAQi;

namespace SharkAdministrativo.Vista
{
    /// <summary>
    /// Lógica de interacción para GestionInsumos.xaml
    /// </summary>
    public partial class GestionInsumos : Window
    {
        Grupo grupo = new Grupo();
        DataTable dt = new DataTable();
        Categoria categoria = new Categoria();
        Unidad_Medida unidad = new Unidad_Medida();
        Insumo insumo_activo = new Insumo();
        string route = "";
        bool exit = false;
        string hasChanged = "No";

        string estado_de_insumo = "Nuevo";
        public GestionInsumos()
        {
            InitializeComponent();
            cargarCombos();
            Titles();
            if (insumo_activo.id == 0)
            {
                llenarInsumos();
            }
            cargarClasificaciones();
        }

        public void llenarInsumos()
        {
            dt.Rows.Clear();
            List<Insumo> insumos = insumo_activo.obtenerTodos();
            foreach (var insumo in insumos)
            {
                dt.Rows.Add(insumo.id, insumo.Grupo.nombre, insumo.descripcion, insumo.ultimo_costo, insumo.costo_promedio, insumo.IVA, insumo.costo_con_impuesto, insumo.inventariable, insumo.Unidad_Medida.nombre, insumo.minimo, insumo.maximo, insumo.codigoInsumo);
            }
        }

        private void tblInsumos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblLista.SelectedItem;
            if (seleccion != null)
            {
                txtCodigoInsumo.IsReadOnly = true;
                SDK.fBuscaProducto(seleccion.Row.ItemArray[11].ToString());
                StringBuilder idValorClasificacion = new StringBuilder(5);
                SDK.fLeeDatoProducto("CIDVALORCLASIFICACION1", idValorClasificacion, 5);
                SDK.fBuscaIdValorClasif(Convert.ToInt32(idValorClasificacion.ToString()));
                StringBuilder codValorClasificacion = new StringBuilder(11);
                StringBuilder nomValorClasificacion = new StringBuilder(30);
                StringBuilder idUnidad = new StringBuilder(5);
                SDK.fLeeDatoCteProv("CCODIGOVALORCLASIFICACION", codValorClasificacion, 11);
                SDK.fLeeDatoCteProv("CVALORCLASIFICACION", nomValorClasificacion, 30);
                SDK.fLeeDatoProducto("CIDUNIDADBASE", idUnidad, 5);

                cbxValoresDeClasificaciones.SelectedItem = codValorClasificacion + " | " + nomValorClasificacion;
                groupInsumo.Header = "Modificando '" + seleccion.Row.ItemArray[2].ToString() + "'";
                txtDescripcion.Text = seleccion.Row.ItemArray[2].ToString();
                cbxGrupos.SelectedItem = seleccion.Row.ItemArray[1].ToString();
                txtUCosto.Text = seleccion.Row.ItemArray[3].ToString();
                txtCpromedio.Text = seleccion.Row.ItemArray[4].ToString();
                txtIva.Text = seleccion.Row.ItemArray[5].ToString();
                txtCCimpuesto.Text = seleccion.Row.ItemArray[6].ToString();
                cbxInventariable.SelectedItem = seleccion.Row.ItemArray[7].ToString();
                cbxUmedida.SelectedItem = idUnidad + " | " + seleccion.Row.ItemArray[8].ToString();
                txtMinimo.Text = seleccion.Row.ItemArray[9].ToString();
                txtMaximo.Text = seleccion.Row.ItemArray[10].ToString();
                txtCodigoInsumo.Text = seleccion.Row.ItemArray[11].ToString();

            }

        }

        public void Titles()
        {
            dt.Columns.Add("id");
            dt.Columns.Add("Grupo");
            dt.Columns.Add("Descripción");
            dt.Columns.Add("Último Costo");
            dt.Columns.Add("Costo Promedio");
            dt.Columns.Add("Porcentaje de IVA");
            dt.Columns.Add("Costo C/ Impuesto");
            dt.Columns.Add("Inventariable");
            dt.Columns.Add("Unidad De Medida");
            dt.Columns.Add("Mínimo");
            dt.Columns.Add("Máximo");
            dt.Columns.Add("Código");

            tblLista.ItemsSource = dt.DefaultView;
            tblLista.Columns[0].Visible = false;
        }


        public void indicarEstado(string estatus)
        {
            estado_de_insumo = estatus;
        }

        private void SaveClose_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exit = true;
            guardarModificar();
        }



        public void guardarModificar()
        {
            if (!String.IsNullOrEmpty(txtDescripcion.Text) && !String.IsNullOrEmpty(txtCpromedio.Text) && !String.IsNullOrEmpty(txtCCimpuesto.Text) && cbxInventariable.SelectedItem != null && !String.IsNullOrEmpty(txtUCosto.Text) && cbxGrupos.SelectedItem != null && cbxUmedida.SelectedItem != null)
            {

                SDK.tProduto cProducto = new SDK.tProduto();
                cProducto.cNombreProducto = txtDescripcion.Text;
                cProducto.cCodigoProducto = txtCodigoInsumo.Text;
                cProducto.cDescripcionProducto = txtDescripcion.Text;
                cProducto.cImpuesto1 = Double.Parse(txtIva.Text);
                cProducto.cTipoProducto = 1;
                cProducto.cMetodoCosteo = 1;
                String[] unidades = cbxUmedida.SelectedItem.ToString().Split('|');
                string idUnidad = unidades[0].Trim();
                cProducto.cPrecio1 = Double.Parse(txtUCosto.Text);
                String[] clasificacion = cbxValoresDeClasificaciones.SelectedItem.ToString().Split('|');
                string codigoClasificacion = clasificacion[0].Trim();


                insumo_activo.codigoInsumo = txtCodigoInsumo.Text;
                insumo_activo.descripcion = txtDescripcion.Text;
                insumo_activo.costo_promedio = float.Parse(txtCpromedio.Text);
                insumo_activo.costo_con_impuesto = float.Parse(txtCCimpuesto.Text);
                insumo_activo.inventariable = cbxInventariable.SelectedItem.ToString();
                insumo_activo.IVA = float.Parse(txtIva.Text);
                insumo_activo.maximo = float.Parse(txtMaximo.Text);
                insumo_activo.minimo = float.Parse(txtMinimo.Text);
                insumo_activo.ultimo_costo = float.Parse(txtUCosto.Text);
                insumo_activo.Grupo = grupo.obtener(cbxGrupos.SelectedItem.ToString());
                insumo_activo.grupo_id = insumo_activo.Grupo.id;
                string nameUnidad = unidades[1].Trim();
                insumo_activo.Unidad_Medida = unidad.obtener(nameUnidad);
                insumo_activo.unidad_id = insumo_activo.Unidad_Medida.id;
                if (tblLista.CurrentItem == null)
                {

                    if (estado_de_insumo == "Nuevo")
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
                            insumo_activo.registrar(insumo_activo);
                            llenarInsumos();
                            MessageBox.Show("EL INSUMO " + insumo_activo.descripcion + " SE REGISTRÓ CORRECTAMENTE \n ¡Puedes Agregar presentaciones a través de tu factura xml de compra! \n También puedes registrarlos manualmente en el módulo gestión de presentaciones");
                            this.Close();
                        }
                        else
                        {
                            SDK.rError(error);
                        }


                    }
                    else
                    {
                        GestionPresentaciones vista = new GestionPresentaciones();
                        vista.addInsumo(insumo_activo);
                        vista.Show();
                        this.Close();
                    }
                }
                else
                {
                    System.Data.DataRowView seleccion = (System.Data.DataRowView)tblLista.SelectedItem;
                    insumo_activo.id = Convert.ToInt32(seleccion.Row.ItemArray[0].ToString());
                    string codigoInsumo = txtCodigoInsumo.Text;
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
                        SDK.fSetDatoProducto("CBANUNIDADES", idUnidad);
                        SDK.fSetDatoProducto("CIDUNIDADBASE", idUnidad);
                        SDK.fSetDatoProducto("CCONTROLEXISTENCIA", "1");

                        error = SDK.fGuardaProducto();
                        if (error == 0)
                        {
                            insumo_activo.modificar(insumo_activo);
                            if (estado_de_insumo != "Nuevo")
                            {
                                GestionPresentaciones vista = new GestionPresentaciones();
                                vista.addInsumo(insumo_activo);
                                vista.Show();
                            }
                            if (exit == true)
                            {
                                this.Close();
                            }
                            else
                            {

                                llenarInsumos();
                                clearFields();
                            }
                        }
                        else
                        {
                            SDK.rError(error);
                        }

                    }
                    else
                    {
                        SDK.rError(error);
                    }

                }
            }
            else
            {
                if (!String.IsNullOrEmpty(txtDescripcion.Text) || !String.IsNullOrEmpty(txtUCosto.Text))
                {
                    MessageBox.Show("¡AVISO! \n > Falta Ingresar Algunos Campos Importantes \n    Es Necesario Ingresarlos");
                }
                else
                {
                    if (exit == true)
                    {
                        this.Close();
                    }
                    else
                    {

                        clearFields();
                    }
                }

            }
        }

        private void cargarCombos()
        {
            cbxInventariable.Items.Add("Si");
            cbxInventariable.Items.Add("No");
            llenarGrupos();
            llenarUnidades();
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


        public void selectGrupo(Grupo grupo)
        {
            cbxGrupos.Items.Clear();
            this.grupo = grupo;
            cargarCombos();
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
            else if (vista == "subgrupo")
            {
                groupSubgrupos.Header = "Registro Del Subgrupo";
                groupSubgrupos.Visibility = Visibility.Visible;
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

        private void EliminarPresentacion_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblLista.SelectedItem;
            if (seleccion != null)
            {
                MessageBoxResult dialogResult = MessageBox.Show("¿Está seguro de eliminar el insumo '" + seleccion.Row.ItemArray[1] + "' de la lista?", "Eliminación de Presentación", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    int error = SDK.fEliminarCteProv(seleccion.Row.ItemArray[11].ToString());
                    if (error == 0)
                    {
                        Insumo insumo = new Insumo();
                        insumo.id = Convert.ToInt32(seleccion.Row.ItemArray[0].ToString());
                        if (insumo.id > 0)
                        {
                            insumo.eliminar(insumo);
                            seleccion.Delete();
                            clearFields();
                        }
                        else
                        {
                            seleccion.Delete();
                            clearFields();
                        }
                    }
                    else {
                        SDK.rError(error);
                    }

                }
                else
                {
                    clearFields();
                }
            }
            else
            {
                MessageBox.Show("Es nesesario que seleccione la presentación que desea eliminar de la lista");
            }
        }

        private void clearFields()
        {
            txtDescripcion.Clear();
            txtCCimpuesto.Clear();
            txtCpromedio.Clear();
            txtMaximo.Clear();
            txtMinimo.Clear();
            txtUCosto.Clear();
            txtCodigoInsumo.Clear();
            cbxGrupos.SelectedItem = null;
            cbxInventariable.SelectedItem = null;
            cbxUmedida.SelectedItem = null;
            tblLista.SelectedItem = false;
            cbxValoresDeClasificaciones.SelectedItem = null;

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

        public void llenarCategorias()
        {
            List<Categoria> categorias = categoria.obtenerTodos();
            foreach (var catego in categorias)
            {
                cbxCategoria.Items.Add(catego);
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

        private void btnGuardarSubgrupo_Click(object sender, RoutedEventArgs e)
        {
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

        private void ObtenerCostos_KeyUp(object sender, KeyEventArgs e)
        {

            if (!String.IsNullOrEmpty(txtUCosto.Text) && !String.IsNullOrEmpty(txtIva.Text))
            {
                string value_ucosto = txtUCosto.Text;
                string value_iva = txtIva.Text;
                if (value_ucosto[0].ToString() == ".")
                {
                    txtUCosto.Text = "0.";
                }
                if (value_iva[0].ToString() == ".")
                {
                    txtIva.Text = "0.";
                }
                double ultimo_costo = Convert.ToDouble(txtUCosto.Text);
                double costo_promedio = Convert.ToDouble(txtUCosto.Text); ;
                double IVA = Convert.ToDouble(txtIva.Text.ToString()) / 100;
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

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            estado_de_insumo = "Agregar Presentaciones";
            guardarModificar();
        }

        private void SoloNumeros_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.OemPeriod || e.Key == Key.Tab)
                e.Handled = false;
            else
                e.Handled = true;

        }

        private void BarButtonItem_ItemClick_1(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            clearFields();
        }




        private void SaveAndNew_ItemClick_2(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            guardarModificar();
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

        private void insumos_ExportToExcel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".xsls", tablaInsumos, "InsumosElaborados");
        }

        private void insumos_ExportToPDF_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".pdf", tablaInsumos, "InsumosElaborados");
        }

        private void insumos_ExportToPNG_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exportTo(".png", tablaInsumos, "InsumosElaborados");
        }

        private void cargarClasificaciones()
        {
            int error = SDK.fPosPrimerValorClasif();
            while (error == 0)
            {
                StringBuilder codValorClasificacion = new StringBuilder(11);
                StringBuilder nomValorClasificacion = new StringBuilder(30);
                SDK.fLeeDatoCteProv("CIDVALORCLASIFICACION", codValorClasificacion, 11);
                SDK.fLeeDatoCteProv("CVALORCLASIFICACION", nomValorClasificacion, 30);

                if (nomValorClasificacion.ToString() != "(Ninguna)")
                {

                    cbxValoresDeClasificaciones.Items.Add(codValorClasificacion + " | " + nomValorClasificacion);
                }
                error = SDK.fPosSiguienteValorClasif();
            }

        }








    }
}
