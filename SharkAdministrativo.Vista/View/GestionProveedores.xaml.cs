﻿using System;
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

        public void addProveedor(string name, string empresa) {
            grupo.obtenerTodos();
            grupo.obtenerTodos();
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
            if (!String.IsNullOrEmpty(proveedor.tipos_proveedor))
            {       
                String[] grupos = proveedor.tipos_proveedor.Split(';');
                foreach (string group in grupos)
                {
                    cbxGrupos.SelectedItems.Add(group);
                }
            }
            
           
        }

        public void llenarGrupos() {
            cbxGrupos.Items.Clear();
            List<Grupo> grupos = grupo.obtenerTodos();
            cbxGrupos.Items.Add("Nuevo");
            foreach (var item in grupos)
            {
                cbxGrupos.Items.Add(item.nombre);
            }
        }

        public void llenarEmpresas() {
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

        public void guardarModificar() {
            if (!String.IsNullOrEmpty(txtNombreP.Text) && !String.IsNullOrEmpty(txtRFC.Text) && cbxGrupos.SelectedItem != null && !String.IsNullOrEmpty(txtRazonP.Text) && cbxEmpresa.SelectedItem != null)
            {
                Proveedor proveedor = new Proveedor();
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
                    proveedor.tipos_proveedor += grupos + ";";
                }
                if (hasChanged == "Yes")
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
                else {
                    proveedor.registrar(proveedor);
                    if (proveedor.id > 0)
                    {
                        MessageBox.Show("ÉXITO, SE REGISTRÓ AL PROVEEDOR '" + proveedor.razon_social + "'");
                        if (exit == "No")
                        {
                            ClearField();
                        }
                        else {
                            this.Close();
                        }

                    }
                    else
                    {
                        MessageBox.Show("ERROR, NO FUE POSIBLE REGISTRAR AL PROVEEDOR '" + proveedor.razon_social + "'");
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

        public void ClearField() {
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
