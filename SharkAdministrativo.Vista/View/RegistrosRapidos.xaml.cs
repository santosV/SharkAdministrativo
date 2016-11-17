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

        public RegistrosRapidos()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// Carga lo títulos de la tabla de grupos.
        /// </summary>
        private void loadGroupTitle() {
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
        private void fillTableGroups() {
            dtGrupos.Rows.Clear();
            List<Grupo> groups = grupo.obtenerTodos();
            foreach (var group in groups)
            {
                dtGrupos.Rows.Add(group.id,group.nombre,group.Categoria.nombre);
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
                   
                    dtAlmacenes.Rows.Add(cCodAlmacen.ToString(),cNombreAlmacen.ToString());

                }
                error = SDK.fPosSiguienteAlmacen();
            }
           
        }


        /// <summary>
        /// Llena el combobox de todas las categorías disponibles.
        /// </summary>
        public void fillCategories() {
            cbxCategoria.Items.Clear();
            List<Categoria> categories = categoria.obtenerTodos();
            foreach (var category in categories)
            {
                cbxCategoria.Items.Add(category.nombre);
            }
        }

        /// <summary>
        /// Guarda o modifica el objeto indicado.
        /// </summary>
        public void saveModify() {
            if (!String.IsNullOrEmpty(txtGrupo.Text) && cbxCategoria.SelectedItem != null)
            {
                

                int error = SDK.fPosPrimerValorClasif();
                String bandera = "No encontrado";
                if(error==0){
                    for (int i = 13; i <= 18; i++)
                    {
                        if (bandera.Equals("Encontrado")) {
                            break;
                        }
                        String aValorClasificacion = txtGrupo.Text;
                        String aValorAbreviatura = txtAbreviatura.Text;
                        while (bandera.Equals("No encontrado")) {
                            
                            StringBuilder cClasificacion = new StringBuilder(11);
                            StringBuilder cValorClasificacion = new StringBuilder(60);
                            StringBuilder cValorAbreviatura = new StringBuilder(3);
                            SDK.fLeeDatoValorClasif("CIDCLASIFICACION",cClasificacion,11);
                            SDK.fLeeDatoValorClasif("CVALORCLASIFICACION",cValorClasificacion,60);
                            SDK.fLeeDatoValorClasif("CCODIGOVALORCLASIFICACION", cValorAbreviatura, 3);
                            

                            if ((!cValorClasificacion.ToString().Equals(aValorClasificacion) && 
                                !cValorAbreviatura.ToString().Equals(aValorAbreviatura)))
                            {
                                if (cClasificacion.ToString().Equals(i.ToString()) && cValorClasificacion.ToString().Equals("(Ninguna)"))
                                {

                                    grupo.nombre = txtGrupo.Text;
                                    grupo.Categoria = categoria.obtener(cbxCategoria.SelectedItem.ToString());
                                    grupo.categoria_id = grupo.Categoria.id;

                                    if (tblGrupos.SelectedItem == null)
                                    {


                                        SDK.fEditaValorClasif();
                                        SDK.fSetDatoValorClasif("CVALORCLASIFICACION", txtGrupo.Text);
                                        SDK.fSetDatoValorClasif("CCODIGOVALORCLASIFICACION", txtAbreviatura.Text);
                                        SDK.fGuardaValorClasif();
                                        grupo.registrar(grupo);
                                        bandera = "Encontrado";
                                        break;
                                    }
                                    else
                                    {
                                        System.Data.DataRowView seleccion = (System.Data.DataRowView)tblGrupos.SelectedItem;
                                        Grupo group = grupo.getForID(Convert.ToInt32(seleccion.Row.ItemArray[0].ToString()));
                                        grupo.id = group.id;
                                        grupo.Modify(grupo);

                                    }

                                }
                                else
                                {
                                    SDK.fPosSiguienteValorClasif();
                                }
                            }
                            else
                            {
                                MessageBox.Show("La abreviatura o el grupo ya existen");
                                bandera = "Encontrado";
                                break;
                            }

                            
                        }
                    }
                }

                
                
                fillTableGroups();

            }else if(!String.IsNullOrEmpty(txtCategoria.Text)){
                categoria.nombre = txtCategoria.Text;
                if (tblCategory.SelectedItem == null)
                {
                    categoria.registrar(categoria);
                }
                else {
                    System.Data.DataRowView seleccion = (System.Data.DataRowView)tblCategory.SelectedItem;
                    categoria.id = Convert.ToInt32(seleccion.Row.ItemArray[0]);
                    categoria.Modify(categoria);
                }
                fillTableCategory();

            }else if (!String.IsNullOrEmpty(txtAlmacen.Text) && (!String.IsNullOrEmpty(txtCodigo.Text)))
            {
                almacen.codigo = txtCodigo.Text;
                almacen.nombre = txtAlmacen.Text;

                SDK.tAlmacen cAlmacen = new SDK.tAlmacen();
                cAlmacen.cCodigoAlmacen = txtCodigo.Text;
                cAlmacen.cNombreAlmacen = txtAlmacen.Text;

                if (tblStorage.SelectedItem == null)
                {
                    int error = SDK.fInsertaAlmacen();
                    if (error == 0)
                    {
                        error = SDK.fSetDatoAlmacen("CCODIGOALMACEN", cAlmacen.cCodigoAlmacen);
                        error = SDK.fSetDatoAlmacen("CNOMBREALMACEN", cAlmacen.cNombreAlmacen);
                        if (error == 0)
                        {
                            error = SDK.fGuardaAlmacen();
                            almacen.registrar(almacen);
                        }
                        else
                        {
                            SDK.rError(error);
                        }
                    }
                    else {
                        SDK.rError(error);
                    }
                }
                else
                {
                    
                    System.Data.DataRowView seleccion = (System.Data.DataRowView)tblStorage.SelectedItem;
                   
                    int error = SDK.fBuscaAlmacen(seleccion.Row.ItemArray[0].ToString());
                    if (error == 0)
                    {

                        error = SDK.fEditaAlmacen();

                        if (error == 0)
                        {

                            error = SDK.fSetDatoAlmacen("CCODIGOALMACEN", txtCodigo.Text);
                            error = SDK.fSetDatoAlmacen("CNOMBREALMACEN", txtAlmacen.Text);

                            if (error == 0)
                            {

                                error = SDK.fGuardaAlmacen();
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
                
               
                
            }
        

        /// <summary>
        /// Muestra la vista solicitada.
        /// </summary>
        public void showView(int vista) {
            hideViews();
            if (vista==1)
            {
                loadGroupTitle();
                fillCategories();
                fillTableGroups();
                ventanaRapida.Title = "Gestión De Grupos";
                vista_grupos.Visibility = Visibility.Visible;
            }else if (vista==2)
            {
                loadCategoryTitle();
                fillTableCategory();
                ventanaRapida.Title = "Gestión De Categorías";
                vista_categorias.Visibility = Visibility.Visible;
            }else if (vista==3)
            {
                loadStorageTitle();
                obtenerAlmacenesCONT();
                ventanaRapida.Title = "Gestión De Almacenes";
                vista_almacenes.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Esconde todas las vistas.
        /// </summary>
        public void hideViews() {
            vista_grupos.Visibility = Visibility.Collapsed;
            vista_categorias.Visibility = Visibility.Collapsed;
            vista_almacenes.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Limpia todos los campos.
        /// </summary>
        private void clearFields() {
            txtCodigo.Clear();
            txtAlmacen.Clear();
            txtCategoria.Clear();
            txtGrupo.Clear();
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
        }

        /// <summary>
        /// Obtiene la información del objeto seleccionado en la tabla de grupos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblGrupos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblGrupos.SelectedItem;
            if (seleccion!=null)
            {
                txtGrupo.Text = seleccion.Row.ItemArray[1].ToString();
                cbxCategoria.SelectedItem = seleccion.Row.ItemArray[2].ToString();
            }
        }

        private void deleteObject() {
            if (tblGrupos.SelectedItem != null)
            {
                System.Data.DataRowView seleccion = (System.Data.DataRowView)tblGrupos.SelectedItem;
                if (seleccion != null)
                {
                    MessageBoxResult dialogResult = MessageBox.Show("¿ESTÁ SEGURO DE ELIMINAR EL REGISTRO '" + seleccion.Row.ItemArray[1] + "'?", "ELIMINACIÓN DE REGISTRO", MessageBoxButton.YesNo);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        this.grupo = grupo.getForID(Convert.ToInt32(seleccion.Row.ItemArray[0]));
                        grupo.delete(grupo);
                        seleccion.Delete();
                        clearFields();
                    }
                }
            }else if(tblCategory.SelectedItem!=null){
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

                              //error = SDK.fSetDatoAlmacen("CCODIGOALMACEN", "(Ninguno)");
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
            }else {
                MessageBox.Show("ES NECESARIO QUE SELECCIONE EL REGISTRO QUE DESEA ELIMINAR");
            }
        }

        private void btnEliminar_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            deleteObject();
        }

        private void btnNew_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            clearFields();
        }

        private void tblCategory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblCategory.SelectedItem;
            if (seleccion!=null)
            {
                txtCategoria.Text = seleccion.Row.ItemArray[1].ToString();
            }
        }

        private void tblStorage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView seleccion = (System.Data.DataRowView)tblStorage.SelectedItem;
            if (seleccion != null)
            {
                groupStorage.Header = "Modificando... " + seleccion.Row.ItemArray[1].ToString();
                txtCodigo.IsReadOnly = true;
                txtAlmacen.Text = seleccion.Row.ItemArray[1].ToString();
                txtCodigo.Text = seleccion.Row.ItemArray[0].ToString();
            }
        }

    }
}
