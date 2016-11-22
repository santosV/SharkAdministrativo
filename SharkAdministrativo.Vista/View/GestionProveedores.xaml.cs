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
using SharkAdministrativo.SDKCONTPAQi;

namespace SharkAdministrativo.Vista
{
    /// <summary>
    /// Lógica de interacción para GestionProveedores.xaml
    /// </summary>
    public partial class GestionProveedores : Window
    {

        Grupo grupo = new Grupo();
        Categoria categoria = new Categoria();
        Empresa empresa = new Empresa();
        Proveedor proveedor = new Proveedor();
        string exit = "No";
        string hasChanged = "No";

        public GestionProveedores()
        {
            InitializeComponent();
            llenarGrupos();
            llenarEmpresas();

        }

        public void addProveedor(string name, string empresa)
        {
            hasChanged = "Yes";
            this.proveedor = proveedor.obtener(name);
            txtNombreP.Text = proveedor.nombre;
            txtCalleP.Text = proveedor.calle;
            txtCodigoPostalP.Text = proveedor.codigo_postal;
            txtColoniaP.Text = proveedor.colonia;
            txtEstadoP.Text = proveedor.estado;
            txtLocalidadP.Text = proveedor.localidad;
            txtMunicipioP.Text = proveedor.municipio;
            txtNoExteriorP.Text = proveedor.NoExterior;
            txtPaisP.Text = proveedor.pais;
            txtRazonP.Text = proveedor.razon_social;
            txtRFC.Text = proveedor.RFC;
            txtSucursalP.Text = proveedor.sucursal;
            txtTelefono.Text = proveedor.telefono;
            cbxEmpresa.SelectedItem = empresa;
            txtCodigo.Text = proveedor.codigo;
            int error = SDK.fBuscaCteProv(proveedor.codigo);
            txtCodigo.IsReadOnly = true;
            
            if (!String.IsNullOrEmpty(proveedor.tipos_proveedor))
            {

                SDK.rError(error); 
                String[] grupos = proveedor.tipos_proveedor.Split(';');
                int i = 1;
                foreach (string group in grupos)
                {
                    StringBuilder cIdValorClasificacionProv = new StringBuilder(5);
                    SDK.fLeeDatoCteProv("CIDVALORCLASIFPROVEEDOR"+i, cIdValorClasificacionProv, 5);
                    i++;
                    cbxGrupos.SelectedItems.Add(cIdValorClasificacionProv+" | "+group);
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
                SDK.fLeeDatoCteProv("CIDCLASIFICACION", cCodClasificacion, 5);
                StringBuilder cNameValorClasificacion = new StringBuilder(20);
                SDK.fLeeDatoCteProv("CVALORCLASIFICACION", cNameValorClasificacion, 20);
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
                    else {
                        error = 1;
                    }  
                }
            }
            cbxGrupos.Items.Clear();
            List<Grupo> grupos = grupo.obtenerTodos();
            foreach (var item in grupos)
            {
                StringBuilder cCodValorClasificacion = new StringBuilder(5);
                SDK.fLeeDatoCteProv("CIDVALORCLASIFICACION", cCodValorClasificacion, 5);
                cbxGrupos.Items.Add(cCodValorClasificacion + " | " + item.nombre);
                SDK.fPosSiguienteValorClasif();
            }
        }

        public void llenarEmpresas()
        {
            List<Empresa> empresas = empresa.obtenerTodos();
            foreach (var item in empresas)
            {
                cbxEmpresa.Items.Add(item.nombre);
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

        private void btnGuardarGrupo_Click(object sender, RoutedEventArgs e)
        {
            if (txtnombreGrupo.Text != "" && cbxCategoria.SelectedItem.ToString() != "" && cbxCategoria.SelectedItem != null)
            {
                Grupo grupo = new Grupo();
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

        private void btnGuardar_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exit = "Yes";
            guardarModificar();
        }

        public void guardarModificar()
        {
            List<string> cIDClasificacionesGrupos = new List<string>();
            if (!String.IsNullOrEmpty(txtNombreP.Text) && !String.IsNullOrEmpty(txtRFC.Text) && cbxGrupos.SelectedItem != null && !String.IsNullOrEmpty(txtRazonP.Text) && cbxEmpresa.SelectedItem != null)
            {
                
                SDK.CteProv cProveedor = new SDK.CteProv();

                cProveedor.cCodigoCliente = txtCodigo.Text;
                cProveedor.cRazonSocial = txtRazonP.Text;
                cProveedor.cRFC = txtRFC.Text;
                cProveedor.cDenComercial = txtNombreP.Text;
                cProveedor.cEstatus = 1;
                

                Proveedor proveedor = new Proveedor();
                proveedor.codigo = txtCodigo.Text;
                proveedor.nombre = txtNombreP.Text;
                proveedor.razon_social = txtRazonP.Text;
                proveedor.RFC = txtRFC.Text;
                proveedor.sucursal = txtSucursalP.Text;
                proveedor.telefono = txtTelefono.Text;
                proveedor.calle = txtCalleP.Text;
                proveedor.codigo_postal = txtCodigoPostalP.Text;
                proveedor.colonia = txtColoniaP.Text;
                proveedor.Empresa = empresa.obtenerPorNombre(cbxEmpresa.SelectedItem.ToString());
                proveedor.empresa_id = proveedor.Empresa.id;
                proveedor.localidad = txtLocalidadP.Text;
                proveedor.municipio = txtMunicipioP.Text;
                proveedor.estado = txtEstadoP.Text;
                proveedor.NoExterior = txtNoExteriorP.Text;
                proveedor.pais = txtPaisP.Text;
                DateTime thisDay = DateTime.Today;
                proveedor.fecha_registro = Convert.ToDateTime(thisDay.ToString());
                
                foreach (var grupos in cbxGrupos.SelectedItems)
                {
                    String[] groups = grupos.ToString().Split('|');
                    cIDClasificacionesGrupos.Add(groups[0].ToString().Trim());
                    proveedor.tipos_proveedor += groups[1].ToString().Trim() + ";";
                }
                if (hasChanged == "Yes")
                {
                    SDK.fBuscaCteProv(proveedor.codigo);
                    SDK.fEditaCteProv();
                    SDK.fSetDatoCteProv("CRAZONSOCIAL", cProveedor.cRazonSocial);
                    SDK.fSetDatoCteProv("CRFC", cProveedor.cRFC);
                    SDK.fSetDatoCteProv("CDENCOMERCIAL", cProveedor.cDenComercial);
                    int i = 1;
                    foreach (var item in cIDClasificacionesGrupos)
                    {
                        SDK.fSetDatoCteProv("CIDVALORCLASIFPROVEEDOR" + i, item);
                        i++;
                    }


                    int error = SDK.fBuscaCteProv(proveedor.codigo);
                    if (error == 0)
                    {
                        proveedor.id = this.proveedor.id;
                        proveedor.modificar(proveedor);
                        MessageBox.Show("ÉXITO, SE MODIFICÓ AL PROVEEDOR '" + proveedor.razon_social + "'");

                        if (exit == "No")
                        {
                            ClearField();
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
                else
                {
                    int cIDCteProv = 0;
                    int error = SDK.fAltaCteProv(ref cIDCteProv,ref  cProveedor);
                    if (error == 0)
                    {
                        proveedor.registrar(proveedor);
                        MessageBox.Show("ÉXITO, SE REGISTRÓ AL PROVEEDOR '" + proveedor.razon_social + "'");
                        SDK.fBuscaIdCteProv(cIDCteProv);
                        SDK.fEditaCteProv();
                        SDK.fSetDatoCteProv("CTIPOCLIENTE","3");
                        SDK.fSetDatoCteProv("CIDMONEDA", "1");
                        int i = 1;
                            foreach (var item in cIDClasificacionesGrupos)
                            {
                                SDK.fSetDatoCteProv("CIDVALORCLASIFPROVEEDOR" + i, item);
                                i++;
                            }


                        SDK.fGuardaCteProv();
                        if (exit == "No")
                        {
                            ClearField();
                        }
                        else
                        {
                            this.Close();
                        }

                    }
                    else {
                        SDK.rError(error);
                    }


                }


            }
            else
            {
                if (!String.IsNullOrEmpty(txtNombreP.Text) || cbxGrupos.SelectedItem != null)
                {
                    MessageBox.Show("IMPOSIBLE GUARDAR, EXISTEN CAMPOS IMPORTANTES SIN INGRESAR");
                }
                else
                {
                    if (exit == "Yes")
                    {
                        this.Close();
                    }
                }
            }
        }

        public void ClearField()
        {
            txtCodigo.Clear();
            txtCalleP.Clear();
            txtCodigoPostalP.Clear();
            txtColoniaP.Clear();
            txtEstadoP.Clear();
            txtLocalidadP.Clear();
            txtMunicipioP.Clear();
            txtNoExteriorP.Clear();
            txtNombreP.Clear();
            txtPaisP.Clear();
            txtRazonP.Clear();
            txtRFC.Clear();
            txtSucursalP.Clear();
            txtTelefono.Clear();
            cbxEmpresa.SelectedItem = null;
            cbxGrupos.SelectedItem = null;

        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            exit = "No";
            guardarModificar();
        }



    }
}
