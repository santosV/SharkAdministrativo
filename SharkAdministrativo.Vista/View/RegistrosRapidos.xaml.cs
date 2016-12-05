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
using SharkAdministrativo.SDKCONTPAQi;

namespace SharkAdministrativo.Vista
{
    /// <summary>
    /// Lógica de interacción para RegistrosRapidos.xaml
    /// </summary>
    public partial class RegistrosRapidos : Window
    {
        Grupo grupo = new Grupo();
        Categoria categoria = new Categoria();
        Almacen almacen = new Almacen();
        DataTable dtGrupos = new DataTable();
        DataTable dtCategoria = new DataTable();
        DataTable dtAlmacenes = new DataTable();
        DataTable dtClasificaciones = new DataTable();
        public bool esCalsificacion = false;


        public RegistrosRapidos()
        {
            InitializeComponent();

        }
        /// <summary>
        /// Carga los títulos de clasificaciones.
        /// </summary>
        private void cargarTitulosClasificaciones()
        {
            dtClasificaciones.Columns.Add("ID");
            dtClasificaciones.Columns.Add("Código");
            dtClasificaciones.Columns.Add("Nombre");
            tblClasificaciones.ItemsSource = dtClasificaciones.DefaultView;
            tblClasificaciones.Columns[0].Visible = false;
        }

        /// <summary>
        /// Carga todas las clasificaciones de producto disponibles.
        /// </summary>
        private void cargarClasif()
        {
            clearFieldsClasif();
            dtClasificaciones.Rows.Clear();
            int error = SDK.fPosPrimerValorClasif();
            int i = 1;
            string codigo = "";
            while (error == 0)
            {
                StringBuilder cIdValorClasificacion = new StringBuilder(5);
                SDK.fLeeDatoValorClasif("CIDCLASIFICACION", cIdValorClasificacion, 5);
                int idClasificacion = Convert.ToInt32(cIdValorClasificacion.ToString());
                StringBuilder cValorClasificacion = new StringBuilder(60);
                StringBuilder idValorClasificacion = new StringBuilder(3);
                StringBuilder codValorClasificacion = new StringBuilder(40);
                SDK.fLeeDatoValorClasif("CVALORCLASIFICACION", cValorClasificacion, 60);
                SDK.fLeeDatoValorClasif("CIDVALORCLASIFICACION", idValorClasificacion, 3);
                SDK.fLeeDatoValorClasif("CCODIGOVALORCLASIFICACION", codValorClasificacion, 40);

                if (codigo != idValorClasificacion.ToString())
                {
                    if (idClasificacion == 25)
                    {


                        dtClasificaciones.Rows.Add(idValorClasificacion, codValorClasificacion, cValorClasificacion);

                    }
                    SDK.fPosSiguienteValorClasif();
                    codigo = idValorClasificacion.ToString();
                }
                else
                {
                    error = 1;
                }
            }

        }

        /// <summary>
        /// Carga lo títulos de la tabla de grupos.
        /// </summary>
        private void loadGroupTitle()
        {
            dtGrupos.Columns.Add("ID");
            dtGrupos.Columns.Add("Nombre");
            dtGrupos.Columns.Add("Categoría");
            tblGrupos.ItemsSource = dtGrupos.DefaultView;
            tblGrupos.Columns[0].Visible = false;
        }

        /// <summary>
        /// Carga lo títulos de la tabla de categorías.
        /// </summary>
        private void loadCategoryTitle()
        {
            dtCategoria.Columns.Add("ID");
            dtCategoria.Columns.Add("Nombre");
            tblCategory.ItemsSource = dtCategoria.DefaultView;
        }

        /// <summary>
        /// Carga lo títulos de la tabla de almacenes.
        /// </summary>
        private void loadStorageTitle()
        {
            dtAlmacenes.Columns.Add("Codigo");
            dtAlmacenes.Columns.Add("Nombre");
            tblStorage.ItemsSource = dtAlmacenes.DefaultView;
        }


        /// <summary>
        /// Llena las filas de la tabla de grupos.
        /// </summary>
        private void fillTableGroups()
        {
            dtGrupos.Rows.Clear();
            List<Grupo> groups = grupo.obtenerTodos();
            foreach (var group in groups)
            {
                dtGrupos.Rows.Add(group.id, group.nombre, group.Categoria.nombre);
            }
        }

        /// <summary>
        /// Llena las filas de la tabla de categorías.
        /// </summary>
        private void fillTableCategory()
        {
            dtCategoria.Rows.Clear();
            List<Categoria> categories = categoria.obtenerTodos();
            foreach (var category in categories)
            {
                dtCategoria.Rows.Add(category.id, category.nombre);
            }
        }

        /// <summary>
        /// Obtiene los almacenes de contpaqi y shark.
        /// </summary>
        private void obtenerAlmacenesCONT()
        {
            dtAlmacenes.Rows.Clear();
            int error = SDK.fPosPrimerAlmacen();
            while (error == 0)
            {
                StringBuilder cCodAlmacen = new StringBuilder(30);
                StringBuilder cNombreAlmacen = new StringBuilder(60);
                SDK.fLeeDatoAlmacen("CCODIGOALMACEN", cCodAlmacen, 30);
                SDK.fLeeDatoAlmacen("CNOMBREALMACEN", cNombreAlmacen, 60);

                if (cNombreAlmacen.ToString() != "(Ninguno)")
                {

                    dtAlmacenes.Rows.Add(cCodAlmacen.ToString(), cNombreAlmacen.ToString());

                }
                error = SDK.fPosSiguienteAlmacen();
            }

        }


        /// <summary>
        /// Llena el combobox de todas las categorías disponibles.
        /// </summary>
        public void fillCategories()
        {
            cbxCategoria.Items.Clear();
            List<Categoria> categories = categoria.obtenerTodos();
            foreach (var category in categories)
            {
                cbxCategoria.Items.Add(category.nombre);
            }
        }

        /// <summary>
        /// Guarda o modifica el objeto indicado (Almcén,Grupo,Clasificación).
        /// </summary>
        public void saveModify()
        {
            if (!String.IsNullOrEmpty(txtGrupo.Text) && cbxCategoria.SelectedItem != null && !String.IsNullOrEmpty(txtAbreviatura.Text))
            {
                int error = SDK.fPosPrimerValorClasif();
                String bandera = "No encontrado";
                if (error == 0)
                {
                    for (int i = 13; i <= 18; i++)
                    {
                        if (bandera.Equals("Encontrado"))
                        {
                            break;
                        }
                        String aValorClasificacion = txtGrupo.Text;
                        String aValorAbreviatura = txtAbreviatura.Text;

                        while (bandera.Equals("No encontrado"))
                        {

                            StringBuilder cClasificacion = new StringBuilder(11);
                            StringBuilder cValorClasificacion = new StringBuilder(60);
                            StringBuilder cValorAbreviatura = new StringBuilder(40);
                            SDK.fLeeDatoValorClasif("CIDCLASIFICACION", cClasificacion, 11);
                            SDK.fLeeDatoValorClasif("CVALORCLASIFICACION", cValorClasificacion, 60);
                            SDK.fLeeDatoValorClasif("CCODIGOVALORCLASIFICACION", cValorAbreviatura, 40);
                            grupo.nombre = txtGrupo.Text;
                            grupo.Categoria = categoria.obtener(cbxCategoria.SelectedItem.ToString());
                            grupo.categoria_id = grupo.Categoria.id;
                            if (tblGrupos.SelectedItem == null)
                            {
                                if ((!cValorClasificacion.ToString().ToUpper().Equals(aValorClasificacion.ToUpper()) &&
                           !cValorAbreviatura.ToString().ToUpper().Equals(aValorAbreviatura.ToUpper())))
                                {
                                    if (cClasificacion.ToString().Equals(i.ToString()) && cValorClasificacion.ToString().Equals("(Ninguna)"))
                                    {
                                        SDK.fEditaValorClasif();
                                        SDK.fSetDatoValorClasif("CVALORCLASIFICACION", txtGrupo.Text);
                                        SDK.fSetDatoValorClasif("CCODIGOVALORCLASIFICACION", txtAbreviatura.Text);
                                        SDK.fGuardaValorClasif();
                                        grupo.registrar(grupo);
                                        bandera = "Encontrado";
                                    }
                                    else
                                    {
                                        SDK.fPosSiguienteValorClasif();

                                        if (i.ToString().Equals(cClasificacion.ToString()))
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("La abreviatura o el grupo ya existen");
                                    bandera = "Encontrado";
                                    break;
                                }

                            }
                            else
                            {
                                if (cValorAbreviatura.ToString().Equals(txtAbreviatura.Text))
                                {
                                    System.Data.DataRowView seleccion = (System.Data.DataRowView)tblGrupos.SelectedItem;
                                    Grupo group = grupo.getForID(Convert.ToInt32(seleccion.Row.ItemArray[0].ToString()));
                                    SDK.fEditaValorClasif();
                                    SDK.fSetDatoValorClasif("CVALORCLASIFICACION", txtGrupo.Text);
                                    SDK.fSetDatoValorClasif("CCODIGOVALORCLASIFICACION", txtAbreviatura.Text);
                                    SDK.fGuardaValorClasif();
                                    bandera = "Encontrado";
                                    grupo.id = group.id;
                                    grupo.Modify(grupo);
                                    break;
                                }
                                else
                                {
                                    SDK.fPosSiguienteValorClasif();
                                }

                            }
                        }
                    }
                }
                fillTableGroups();
            }
            else if (!String.IsNullOrEmpty(txtCategoria.Text))
            {
                List<Categoria> categorias = categoria.obtenerTodos();
                foreach (var categoriaN in categorias)
                {
                    if (categoriaN.nombre.Equals(txtCategoria.Text))
                    {
                        MessageBox.Show("LA CATEGORÍA YA EXISTE", "AVISO SHARK");
                        return;
                    }

                }

                categoria.nombre = txtCategoria.Text;
                if (tblCategory.SelectedItem == null)
                {
                    categoria.registrar(categoria);
                }
                else
                {
                    System.Data.DataRowView seleccion = (System.Data.DataRowView)tblCategory.SelectedItem;
                    categoria.id = Convert.ToInt32(seleccion.Row.ItemArray[0]);
                    categoria.Modify(categoria);
                }
                fillTableCategory();
            }
            else if (!String.IsNullOrEmpty(txtAlmacen.Text) && (!String.IsNullOrEmpty(txtCodigo.Text)))
            {
                for (int i = 0; i < dtAlmacenes.Rows.Count; i++)
                {
                    DataRow row = dtAlmacenes.Rows[i];
                    String abreV = row[0].ToString();
                    String nombreAlmacen = row[1].ToString();
                    if (abreV.ToString().Contains(txtCodigo.Text) || nombreAlmacen.ToString().Contains(txtAlmacen.Text))
                    {
                        MessageBox.Show("EL CÓDIGO O EL NOMBRE DEL ALMACÉN YA EXISTE", "AVISO SHARK");
                        return;
                    }
                }

                almacen.codigo = txtCodigo.Text;
                almacen.nombre = txtAlmacen.Text;

                //Creación de almacén en Contpaqi.
                SDK.tAlmacen cAlmacen = new SDK.tAlmacen();
                cAlmacen.cCodigoAlmacen = txtCodigo.Text;
                cAlmacen.cNombreAlmacen = txtAlmacen.Text;

                if (tblStorage.SelectedItem == null)
                {
                        int error = SDK.fPosPrimerAlmacen();
                        error = SDK.fPosSiguienteAlmacen();
                        string codigo = "0";
                        while (error == 0)
                        {
                            StringBuilder nombreAlmacen = new StringBuilder(30);
                            SDK.fLeeDatoAlmacen("CNOMBREALMACEN", nombreAlmacen, 30);
                            if (codigo.Equals(nombreAlmacen) || String.IsNullOrEmpty(nombreAlmacen.ToString()))
                            {
                                break;
                            }
                            if (nombreAlmacen.ToString().Equals("(Ninguno)"))
                            {
                                error = 1;
                                break;
                            }
                            else {
                                codigo = nombreAlmacen.ToString();
                                SDK.fPosSiguienteAlmacen();
                            }
                        }
                        if (error == 0)
	                    {
                            error = SDK.fInsertaAlmacen();
		                    error = SDK.fSetDatoAlmacen("CCODIGOALMACEN", cAlmacen.cCodigoAlmacen);
                            error = SDK.fSetDatoAlmacen("CNOMBREALMACEN", cAlmacen.cNombreAlmacen);
                            if (error == 0)
                            {
                                error = SDK.fGuardaAlmacen();
                                //Creación de almacén en Shark.
                                almacen.registrar(almacen);
                            }
                            else
                            {
                                SDK.rError(error);
                            }
	                    }else if(error == 1){
                            error = SDK.fEditaAlmacen();
		                    error = SDK.fSetDatoAlmacen("CCODIGOALMACEN", cAlmacen.cCodigoAlmacen);
                            error = SDK.fSetDatoAlmacen("CNOMBREALMACEN", cAlmacen.cNombreAlmacen);
                            if (error == 0)
                            {
                                error = SDK.fGuardaAlmacen();
                                //Creación de almacén en Shark.
                                almacen.registrar(almacen);
                            }
                            else
                            {
                                SDK.rError(error);
                            }
                    }
                }
                else
                {
                    System.Data.DataRowView seleccion = (System.Data.DataRowView)tblStorage.SelectedItem;
                    int error = SDK.fBuscaAlmacen(seleccion.Row.ItemArray[0].ToString());
                    if (error == 0)
                    {
                        //Modificación de almacén en Contpaqi.
                        error = SDK.fEditaAlmacen();
                        if (error == 0)
                        {
                            error = SDK.fSetDatoAlmacen("CCODIGOALMACEN", txtCodigo.Text);
                            error = SDK.fSetDatoAlmacen("CNOMBREALMACEN", txtAlmacen.Text);
                            if (error == 0)
                            {
                                error = SDK.fGuardaAlmacen();
                                //Modificación de almacén en Shark.
                                almacen.codigo = seleccion.Row.ItemArray[0].ToString();
                                almacen.Modify(almacen);
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
                    else
                    {
                        SDK.rError(error);
                    }

                }
                obtenerAlmacenesCONT();
            }
            else if (!String.IsNullOrEmpty(txtAbre.Text) && !String.IsNullOrEmpty(txtNombreC.Text))
            {
                if (tblClasificaciones.SelectedItem == null)
                {
                    for (int i = 0; i < dtClasificaciones.Rows.Count; i++)
                    {
                        DataRow row = dtClasificaciones.Rows[i];
                        String abreV = row[1].ToString();
                        if (abreV.ToString().Contains(txtAbre.Text))
                        {
                            MessageBox.Show("LA ABREVIATURA YA EXISTE", "AVISO SHARK");
                            return;
                        }
                    }
                    int error = SDK.fInsertaValorClasif();
                    if (error == 0)
                    {
                        error = SDK.fSetDatoValorClasif("CVALORCLASIFICACION", txtNombreC.Text);
                        error = SDK.fSetDatoValorClasif("CIDCLASIFICACION", "25");
                        error = SDK.fSetDatoValorClasif("CCODIGOVALORCLASIFICACION", txtAbre.Text);
                        error = SDK.fGuardaValorClasif();
                        if (error == 0)
                        {
                            cargarClasif();
                            clearFieldsClasif();
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
                else
                {
                    System.Data.DataRowView seleccion = (System.Data.DataRowView)tblClasificaciones.SelectedItem;
                    int error = SDK.fBuscaIdValorClasif(Convert.ToInt32(seleccion.Row.ItemArray[0].ToString()));
                    if (error == 0)
                    {
                        error = SDK.fEditaValorClasif();
                        if (error == 0)
                        {
                            error = SDK.fSetDatoValorClasif("CVALORCLASIFICACION", txtNombreC.Text);
                            error = SDK.fSetDatoValorClasif("CIDCLASIFICACION", "25");
                            error = SDK.fSetDatoValorClasif("CCODIGOVALORCLASIFICACION", txtAbre.Text);
                            error = SDK.fGuardaValorClasif();
                            if (error == 0)
                            {
                                cargarClasif();
                                clearFieldsClasif();
                            }
                            else
                            {
                                SDK.rError(error);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Muestra la vista solicitada.
        /// </summary>
        public void showView(int vista)
        {
            hideViews();
            if (vista == 1)
            {
                loadGroupTitle();
                fillCategories();
                fillTableGroups();
                ventanaRapida.Title = "Gestión De Grupos";
                vista_grupos.Visibility = Visibility.Visible;
            }
            else if (vista == 2)
            {
                loadCategoryTitle();
                fillTableCategory();
                ventanaRapida.Title = "Gestión De Categorías";
                vista_categorias.Visibility = Visibility.Visible;
            }
            else if (vista == 3)
            {
                loadStorageTitle();
                obtenerAlmacenesCONT();
                ventanaRapida.Title = "Gestión De Almacenes";
                vista_almacenes.Visibility = Visibility.Visible;
            }
            else if (vista == 4)
            {
                cargarTitulosClasificaciones();
                cargarClasif();
                ventanaRapida.Title = "Gestión De Clasificaciones";
                vista_clasificacionesProductos.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Esconde todas las vistas.
        /// </summary>
        public void hideViews()
        {
            vista_grupos.Visibility = Visibility.Collapsed;
            vista_categorias.Visibility = Visibility.Collapsed;
            vista_almacenes.Visibility = Visibility.Collapsed;
            vista_clasificacionesProductos.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Limpia todos los campos.
        /// </summary>
        private void clearFields()
        {
            txtCodigo.IsReadOnly = false;
            txtCodigo.Clear();
            txtAlmacen.Clear();
            txtCategoria.Clear();
            txtGrupo.Clear();
            txtAbreviatura.Clear();
            txtAbreviatura.IsReadOnly = false;
            tblGrupos.SelectedItem = null;
            tblCategory.SelectedItem = null;
            tblStorage.SelectedItem = null;
            cbxCategoria.SelectedItem = null;
        }

        /// <summary>
        /// llama al método guardar o modificar y limpia los campos para un nuevo registro..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAndNew_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            saveModify();
            clearFields();
            clearFieldsClasif();
        }

        /// <summary>
        /// Obtiene la información del objeto seleccionado en la tabla de grupos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblGrupos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblGrupos.SelectedItem;
            if (seleccion != null)
            {
                txtAbreviatura.IsReadOnly = true;


                int error = SDK.fPosPrimerValorClasif();
                while (error == 0)
                {
                    StringBuilder cClasificacion = new StringBuilder(11);
                    StringBuilder cValorClasificacion = new StringBuilder(60);
                    StringBuilder cValorAbreviatura = new StringBuilder(40);
                    SDK.fLeeDatoValorClasif("CIDCLASIFICACION", cClasificacion, 11);
                    SDK.fLeeDatoValorClasif("CVALORCLASIFICACION", cValorClasificacion, 60);
                    SDK.fLeeDatoValorClasif("CCODIGOVALORCLASIFICACION", cValorAbreviatura, 40);

                    if (cValorClasificacion.ToString().Equals(seleccion.Row.ItemArray[1].ToString()))
                    {
                        txtAbreviatura.Text = cValorAbreviatura.ToString();
                    }

                    if (cClasificacion.ToString().Equals("18"))
                    {
                        break;
                    }
                    SDK.fPosSiguienteValorClasif();
                }


                txtGrupo.Text = seleccion.Row.ItemArray[1].ToString();
                cbxCategoria.SelectedItem = seleccion.Row.ItemArray[2].ToString();
            }
        }

        /// <summary>
        /// Elimina el objeto indicado (Almcén,Grupo,Clasificación).
        /// </summary>
        private void deleteObject()
        {
            if (tblGrupos.SelectedItem != null)
            {
                System.Data.DataRowView seleccion = (System.Data.DataRowView)tblGrupos.SelectedItem;
                if (seleccion != null)
                {
                    int error = SDK.fPosPrimerValorClasif();
                    while (error == 0)
                    {
                        StringBuilder cClasificacion = new StringBuilder(11);
                        StringBuilder cValorClasificacion = new StringBuilder(60);
                        StringBuilder cValorAbreviatura = new StringBuilder(40);
                        SDK.fLeeDatoValorClasif("CIDCLASIFICACION", cClasificacion, 11);
                        SDK.fLeeDatoValorClasif("CVALORCLASIFICACION", cValorClasificacion, 60);
                        SDK.fLeeDatoValorClasif("CCODIGOVALORCLASIFICACION", cValorAbreviatura, 40);

                        if (cValorClasificacion.ToString().Equals(seleccion.Row.ItemArray[1].ToString()))
                        {
                            MessageBoxResult dialogResult = MessageBox.Show("¿ESTÁ SEGURO DE ELIMINAR EL REGISTRO '" + seleccion.Row.ItemArray[1] + "'?", "ELIMINACIÓN DE REGISTRO", MessageBoxButton.YesNo);
                            if (dialogResult == MessageBoxResult.Yes)
                            {
                                this.grupo = grupo.getForID(Convert.ToInt32(seleccion.Row.ItemArray[0]));
                                SDK.fEditaValorClasif();
                                error = SDK.fSetDatoValorClasif("CVALORCLASIFICACION", "(Ninguna)");
                                error = SDK.fGuardaValorClasif();
                                if (error == 0)
                                {
                                    grupo.delete(grupo);
                                    seleccion.Delete();
                                    clearFields();
                                    break;
                                }
                                else
                                {
                                    SDK.rError(error);
                                }

                            }

                        }

                        if (cClasificacion.ToString().Equals("18"))
                        {
                            break;
                        }
                        SDK.fPosSiguienteValorClasif();
                    }


                }
            }
            else if (tblCategory.SelectedItem != null)
            {
                System.Data.DataRowView seleccion = (System.Data.DataRowView)tblCategory.SelectedItem;
                if (seleccion != null)
                {
                    MessageBoxResult dialogResult = MessageBox.Show("¿ESTÁ SEGURO DE ELIMINAR EL REGISTRO '" + seleccion.Row.ItemArray[1] + "'?", "ELIMINACIÓN DE REGISTRO", MessageBoxButton.YesNo);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        categoria.delete(Convert.ToInt32(seleccion.Row.ItemArray[0]));
                        seleccion.Delete();
                        clearFields();
                    }
                }
            }
            else if (tblStorage.SelectedItem != null)
            {
                System.Data.DataRowView seleccion = (System.Data.DataRowView)tblStorage.SelectedItem;
                if (seleccion != null)
                {
                    MessageBoxResult dialogResult = MessageBox.Show("¿ESTÁ SEGURO DE ELIMINAR EL REGISTRO '" + seleccion.Row.ItemArray[1] + "'?", "ELIMINACIÓN DE REGISTRO", MessageBoxButton.YesNo);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        int error = SDK.fBuscaAlmacen(seleccion.Row.ItemArray[0].ToString());
                        if (error == 0)
                        {
                            error = SDK.fEditaAlmacen();

                            if (error == 0)
                            {
                                error = SDK.fSetDatoAlmacen("CNOMBREALMACEN", "(Ninguno)");

                                if (error == 0)
                                {

                                    error = SDK.fGuardaAlmacen();
                                    almacen.delete(seleccion.Row.ItemArray[0].ToString());
                                    seleccion.Delete();

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
                        else
                        {
                            SDK.rError(error);
                        }


                        clearFields();
                    }
                }
            }
            else if (tblClasificaciones.SelectedItem != null)
            {
                System.Data.DataRowView seleccion = (System.Data.DataRowView)tblClasificaciones.SelectedItem;
                if (seleccion != null)
                {
                    int error = SDK.fBuscaIdValorClasif(Convert.ToInt32(seleccion.Row.ItemArray[0].ToString()));
                    if (error == 0)
                    {
                        error = SDK.fBorraValorClasif();
                        if (error == 0)
                        {
                            seleccion.Delete();
                            clearFieldsClasif();
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
                MessageBox.Show("ES NECESARIO QUE SELECCIONE EL REGISTRO QUE DESEA ELIMINAR");
            }
        }

        /// <summary>
        /// Manda llamar el método eliminar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminar_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            deleteObject();
        }

        /// <summary>
        /// Manda llamar los métodos para limpiar campos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            clearFields();
            clearFieldsClasif();
        }

        /// <summary>
        /// Obtiene los datos de catgoría.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblCategory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblCategory.SelectedItem;
            if (seleccion != null)
            {
                txtCategoria.Text = seleccion.Row.ItemArray[1].ToString();
            }
        }
        /// <summary>
        /// Detecta el evento y obtiene los datos del almacén para ponerlo en edición.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblStorage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblStorage.SelectedItem;
            if (seleccion != null)
            {
                groupStorage.Header = "Modificando El Almacén" + seleccion.Row.ItemArray[1].ToString();
                txtCodigo.IsReadOnly = true;
                txtAlmacen.Text = seleccion.Row.ItemArray[1].ToString();
                txtCodigo.Text = seleccion.Row.ItemArray[0].ToString();
            }
        }

        /// <summary>
        /// Detecta el evento y obtiene los datos de la clasificación para ponerlo en edición.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblClasificaciones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblClasificaciones.SelectedItem;
            if (seleccion != null)
            {
                groupStorage.Header = "Modificando La Clasifiación " + seleccion.Row.ItemArray[2].ToString();
                txtAbre.IsReadOnly = true;
                txtAbre.Text = seleccion.Row.ItemArray[1].ToString();
                txtNombreC.Text = seleccion.Row.ItemArray[2].ToString();

            }
        }
        /// <summary>
        /// Limpia los datos de clasificación.
        /// </summary>
        private void clearFieldsClasif()
        {
            txtAbre.Clear();
            txtNombreC.Clear();
            tblClasificaciones.SelectedItem = false;
            txtAbre.IsReadOnly = false;
            groupStorage.Header = "Nueva Clasifiación ";
            txtTitle.Text = "Gestión De Clasifiaciones De Producto";
        }

    }
}
