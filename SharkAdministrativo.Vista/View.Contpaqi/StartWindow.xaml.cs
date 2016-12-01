using SharkAdministrativo.SDKCONTPAQi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
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

namespace SharkAdministrativo.Vista.View.Contpaqi
{
    /// <summary>
    /// Lógica de interacción para StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            getServers();
            txtRutaEmpresa.Text = @"C:\Compac\Empresas";
        }

        /// <summary>
        /// Obtiene todos los servidores / instancias de SQL SERVER Disponibles en la red local.
        /// </summary>
        private void getServers()
        {
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            System.Data.DataTable table = instance.GetDataSources();
            List<string> instances = new List<string>();
            foreach (System.Data.DataRow row in table.Rows)
            {
                string serverName = row.ItemArray[0].ToString();
                string instanceName = row.ItemArray[1].ToString();
                cbxServers.Items.Add(serverName + "\\" + instanceName);
            }
        }

        /// <summary>
        /// Abre el explorador de archivos para que se seleccione la empresa que se desea abrir en Contpaqi Comercial.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog carpeta = new System.Windows.Forms.FolderBrowserDialog();
            carpeta.Description = "Seleccione la Empresa";
            carpeta.SelectedPath = @"C:\Compac\Empresas\";
            carpeta.ShowDialog();
            txtRutaEmpresa.Text = carpeta.SelectedPath;

            try
            {

                if (txtRutaEmpresa.Text != @"C:\Compac\Empresas")
                {
                    SDK.companyRoute = txtRutaEmpresa.Text;
                    int error = SDK.startSDK();
                    if (error == 0)
                    {
                        btnIngresar.IsEnabled = true;
                    }
                }
                /*
                La siguiente línea evade la excepción de error con los FPU al utilizar la librería de contpaqi en C.
                */
                throw new Exception("Ignora error de FPU`s");
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Accede o niega el acceso al sistema, probando la conexión con contpaqi comercial.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {

            if (cbxServers.SelectedItem != null)
            {
                SDK.companyName = txtRutaEmpresa.Text.Remove(0, 21);
                string dataBase = SDK.companyName;
                string server = cbxServers.SelectedItem.ToString();
                configurarEntorno(dataBase, server);
            }
            else
            {
                MessageBox.Show("ES NECESARIO QUE SELECCIONES TU SERVIDOR PARA LA CONFIGURACIÓN", "AVISO SHARK");
            }

        }

        /// <summary>
        /// Valida que empresa se está abriendo y si no está registrada manda llamar los métodos de configuración de entorno.
        /// </summary>
        /// <param name="companyName">Nombre de la empresa</param>
        /// <param name="server">Servidor de SQL</param>
        public void configurarEntorno(string companyName, string server)
        {
            //omite espacios en nombre de la empresa.
            companyName = companyName.Replace(" ", "_");

            string[] companyDataBases = obtenerBasesDeDatos(server);//Obtiene todas las bases de datos del servidor especificado.
            bool exist = false;
            Empresa company = new Empresa();
            foreach (var empresa in companyDataBases)
            {
                if (empresa == "Shark_" + companyName)
                {
                    SDK.companyConnection = createDataSource(server, "Shark_" + companyName);
                    company = company.obtenerPorNombre(companyName);
                    if (company.id > 0)
                    {
                        SDK.companyConnection = company.datasource;
                    }
                    else
                    {
                        MessageBox.Show("ERORR: error al obtener la empresa :(", "AVISO SHARK");
                        this.Close();
                    }
                    exist = true;
                    break;
                }
            }

            if (exist != true)
            {
                if (generarScript(server, companyName) != true)
                {
                    configurarEmpresa(createDataSource(server, "Shark_" + companyName));
                }

            }
            MainWindow view = new MainWindow();
            view.lblEmpresa.Text = "@" + SDK.companyName;
            view.Show();
            this.Close();

        }

        /// <summary>
        /// Crea la cadena de conexión con los estandares de Entity framework.
        /// </summary>
        /// <param name="server">Nombre del servidor.</param>
        /// <param name="database">Nombre de la base de datos.</param>
        /// <returns>El datasource para la conexión.</returns>
        private string createDataSource(string server, string database)
        {
            EntityConnectionStringBuilder constructorConexion = new EntityConnectionStringBuilder();
            constructorConexion.Provider = "System.Data.SqlClient";
            constructorConexion.ProviderConnectionString = @"data source=" + server + ";initial catalog=" + database + ";user id=" + txtUser.Text + ";password=" + txtPassword.Text + ";MultipleActiveResultSets=True;App=EntityFramework";
            constructorConexion.Metadata = "res://*/dbshark.csdl|res://*/dbshark.ssdl|res://*/dbshark.msl";
            return constructorConexion.ToString();
        }

        /// <summary>
        /// Crea la empresa y los datos por default en la base de datos.
        /// </summary>
        /// <param name="connection">La conexión de entity framework.</param>
        private static void configurarEmpresa(string connection)
        {

            bdsharkEntities conexion = new bdsharkEntities(connection);
            Empresa nEmpresa = new Empresa();
            nEmpresa.datasource = connection;
            nEmpresa.nombre = SDK.companyName;
            SDK.companyConnection = nEmpresa.datasource;

            Tipo_movimiento mvto1 = new Tipo_movimiento();
            mvto1.nombre = "SALIDA";
            Tipo_movimiento mvto2 = new Tipo_movimiento();
            mvto2.nombre = "TRASPASO";

            AreaProduccion area1 = new AreaProduccion();
            area1.nombre = "RESTAURTANTE";
            AreaProduccion area2 = new AreaProduccion();
            area2.nombre = "EN LINEA";
            AreaProduccion area3 = new AreaProduccion();
            area3.nombre = "COCINA";
            AreaProduccion area4 = new AreaProduccion();
            area4.nombre = "SERVICIO";

            conexion.Tipo_movimientos.Add(mvto1);
            conexion.Tipo_movimientos.Add(mvto2);
            conexion.AreasProduccion.Add(area1);
            conexion.AreasProduccion.Add(area2);
            conexion.AreasProduccion.Add(area3);
            conexion.AreasProduccion.Add(area4);
            conexion.Empresas.Add(nEmpresa);

            conexion.SaveChanges();
            if (nEmpresa.id > 0)
            {
                MessageBox.Show("Hola, bienvenido a Shark POS " + SDK.companyName + ", Se ha configurado el entorno correctamente!", "BIENVENIDA A SHARK");
            }
            else
            {
                MessageBox.Show("Lo sentimos, existen errores de configuración :(");
            }


        }

        /// <summary>
        /// Genera el script de la estructura de la base de datos.
        /// </summary>
        /// <param name="companyName"></param>
        public static bool generarScript(string server, string companyName)
        {
            bool error = false;


            string dataBaseName = "Shark_" + companyName;
            //script para crear base de datos.
            string line = "CREATE DATABASE " + dataBaseName;
            try
            {
                System.IO.File.WriteAllText(@"C:\SharkCreateDB_" + companyName + ".sql", line);
                ProcessStartInfo cmd = new ProcessStartInfo("sqlcmd", "-S " + server + " -i C:\\SharkCreateDB_" + companyName + ".sql");
                cmd.UseShellExecute = false;
                cmd.CreateNoWindow = true;
                cmd.RedirectStandardOutput = true;
                Process execute = new Process();
                execute.StartInfo = cmd;
                execute.Start();
                execute.Close();

            }
            catch (Exception ex)
            {
                Console.Write("Error: " + ex);
                error = true;
            }

            //script personalizado de la base de datos
            string script = "USE [" + dataBaseName + "]" +
                            "\nGO" +
                            "\nSET ANSI_NULLS ON" +
                            "\nGO" +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[Unidades_Medida](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL, " +
                                "\n[nombre] [varchar](30) NULL, " +
                             "\nCONSTRAINT [PK_Unidades_Medida] PRIMARY KEY CLUSTERED " +
                            "\n(" +
                                "\n[id] ASC " +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                            "\n) ON [PRIMARY]" +
                            "\nGO " +
                            "\nSET ANSI_PADDING OFF" +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON" +
                            "\nGO" +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[Tipo_movimientos](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[nombre] [varchar](50) NULL," +
                             "\nCONSTRAINT [PK_tipo_movimientos] PRIMARY KEY CLUSTERED " +
                            "\n(" +
                                "\n[id] ASC" +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                            "\n) ON [PRIMARY] " +
                            "\nGO " +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[Salidas_almacen](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[insumo_id] [int] NOT NULL," +
                                "\n[cantidad] [float] NULL," +
                                "\n[tipo_movimiento_id] [int] NOT NULL," +
                                "\n[almacen_salida] [int] NOT NULL," +
                                "\n[descripcion] [varchar](50) NULL," +
                            "\nCONSTRAINT [PK_movimientos_almacen] PRIMARY KEY CLUSTERED " +
                            "\n(" +
                                "\n[id] ASC " +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                            "\n) ON [PRIMARY] " +
                            "\nGO " +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[Recetas](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[insumo_id] [int] NOT NULL," +
                                "\n[producto_id] [int] NOT NULL," +
                                "\n[insumoElaborado_id] [int] NOT NULL," +
                                "\n[cantidad] [float] NULL," +
                                "\n[almacenes_id] [varchar](100) NULL," +
                             "\nCONSTRAINT [PK_Recetas] PRIMARY KEY CLUSTERED " +
                            "\n(" +
                                "\n[id] ASC " +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                            "\n) ON [PRIMARY] " +
                            "\nGO " +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[Proveedores](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[nombre] [varchar](250) NULL," +
                                "\n[razon_social] [varchar](250) NULL," +
                                "\n[RFC] [varchar](30) NULL," +
                                "\n[calle] [varchar](50) NULL," +
                                "\n[NoExterior] [varchar](10) NULL," +
                                "\n[codigo_postal] [varchar](10) NULL," +
                                "\n[colonia] [varchar](100) NULL," +
                                "\n[localidad] [varchar](150) NULL," +
                                "\n[municipio] [varchar](150) NULL," +
                                "\n[estado] [varchar](150) NULL," +
                                "\n[pais] [varchar](150) NULL," +
                                "\n[telefono] [varchar](15) NULL," +
                                "\n[fecha_registro] [smalldatetime] NOT NULL," +
                                "\n[empresa_id] [int] NOT NULL," +
                                "\n[sucursal] [varchar](50) NULL," +
                                "\n[tipos_proveedor] [varchar](500) NULL," +
                                "\n[codigo] [varchar](50) NULL," +
                             "\nCONSTRAINT [PK_Proveedores] PRIMARY KEY CLUSTERED " +
                            "\n(" +
                                "\n[id] ASC " +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                            "\n) ON [PRIMARY] " +
                           "\nGO " +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[Promociones](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[descripcion] [varchar](100) NULL," +
                                "\n[nombre] [varchar](50) NULL," +
                                "\n[ultimoPrecio] [float] NULL," +
                                "\n[IVA] [float] NULL," +
                                "\n[precioConImpuesto] [float] NULL," +
                                "\n[areasDisponibles] [varchar](100) NULL," +
                                "\n[imagen] [image] NULL," +
                                "\n[diasDisponibles] [varchar](20) NULL," +
                                "\n[hora_inicio] [varchar](50) NULL," +
                                "\n[hora_fin] [varchar](50) NULL," +
                                "\n[fecha_inicio] [datetime] NULL," +
                                "\n[fecha_fin] [datetime] NULL," +
                             "\nCONSTRAINT [PK_Promociones] PRIMARY KEY CLUSTERED " +
                            "\n(" +
                                "\n[id] ASC " +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                            "\n)ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] " +
                            "\nGO " +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[Productos](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[descripcion] [varchar](150) NULL," +
                                "\n[nombre] [varchar](50) NULL," +
                                "\n[ultimoPrecio] [float] NULL," +
                                "\n[IVA] [float] NULL," +
                                "\n[precioConImpuesto] [float] NULL," +
                                "\n[areasPreparacion] [varchar](50) NULL," +
                                "\n[disponlibleEn] [varchar](50) NULL," +
                                "\n[imagen] [image] NULL," +
                                "\n[codigo] [varchar](50) NULL," +
                             "\nCONSTRAINT [PK_Productos] PRIMARY KEY CLUSTERED " +
                            "\n(" +
                                "\n[id] ASC" +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                            "\n)ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] " +
                            "\nGO " +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[ProductoPromocion](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[producto_id] [int] NOT NULL," +
                                "\n[promocion_id] [int] NOT NULL," +
                                "\n[cantidad] [float] NULL," +
                             "\nCONSTRAINT [PK_ProductoPromocion] PRIMARY KEY CLUSTERED" +
                            "\n(" +
                                "\n[id] ASC " +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                            "\n)ON [PRIMARY] " +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[Presentaciones](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[descripcion] [varchar](150) NULL," +
                                "\n[ultimo_costo] [float] NULL," +
                                "\n[costo_promedio] [float] NULL," +
                                "\n[IVA] [float] NULL," +
                                "\n[costo_con_impuesto] [float] NULL," +
                                "\n[rendimiento] [float] NULL," +
                                "\n[minimo] [int] NOT NULL," +
                                "\n[proveedor_id] [int] NOT NULL," +
                                "\n[insumo_id] [int] NOT NULL," +
                                "\n[factura_id] [int] NOT NULL," +
                                "\n[almacen_id] [int] NOT NULL," +
                                "\n[noIdentificacion] [varchar](50) NULL," +
                                "\n[costo_unitario] [float] NULL," +
                                "\n[cantidad] [float] NULL," +
                                "\n[existencia] [float] NULL," +
                                "\n[codigo] [varchar](50) NULL," +
                             "\nCONSTRAINT [PK_Presentaciones] PRIMARY KEY CLUSTERED " +
                            "\n(" +
                                "\n[id] ASC " +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                            "\n) ON [PRIMARY]" +
                            "\nGO " +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[InsumosElaborados](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[insumos] [varchar](300) NULL," +
                                "\n[rendimiento] [float] NULL," +
                                "\n[grupo_id] [int] NOT NULL," +
                                "\n[descripcion] [varchar](50) NULL," +
                                "\n[unidad_id] [int] NOT NULL," +
                                "\n[costo_unitario] [float] NULL," +
                                "\n[costo_promedio] [float] NULL," +
                                "\n[costo_estandar] [float] NULL," +
                                "\n[inventariable] [varchar](10) NULL," +
                                "\n[entrada_automatica] [int] NULL," +
                                "\n[existencia] [float] NULL," +
                                "\n[codigo] [varchar](50) NULL," +
                             "\nCONSTRAINT [PK_InsumosElaborados] PRIMARY KEY CLUSTERED " +
                            "\n(" +
                                "\n[id] ASC" +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                            "\n) ON [PRIMARY] " +
                            "\nGO" +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO" +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[Insumos](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[descripcion] [varchar](150) NULL," +
                                "\n[ultimo_costo] [float] NULL," +
                                "\n[costo_promedio] [float] NULL," +
                                "\n[IVA] [float] NULL," +
                                "\n[costo_con_impuesto] [float] NULL," +
                                "\n[inventariable] [varchar](5) NULL," +
                                "\n[minimo] [float] NULL," +
                                "\n[maximo] [float] NULL," +
                                "\n[grupo_id] [int] NOT NULL," +
                                "\n[unidad_id] [int] NOT NULL," +
                                "\n[codigoInsumo] [varchar](50) NULL," +
                             "\nCONSTRAINT [PK_Insumos] PRIMARY KEY CLUSTERED " +
                            "\n(" +
                                "\n[id] ASC" +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                            "\n) ON [PRIMARY]" +
                            "\nGO " +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[Grupos](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[nombre] [varchar](30) NULL," +
                                "\n[categoria_id] [int] NOT NULL," +
                             "\nCONSTRAINT [PK_Grupos] PRIMARY KEY CLUSTERED " +
                            "\n(" +
                                "\n[id] ASC" +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                            "\n) ON [PRIMARY] " +
                            "\nGO " +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[Facturas](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[folio] [varchar](10) NULL," +
                                "\n[fecha_emision] [smalldatetime] NOT NULL," +
                                "\n[tipo_comprobante] [varchar](200) NULL," +
                                "\n[lugar_expedicion] [varchar](200) NULL," +
                                "\n[forma_pago] [varchar](150) NULL," +
                                "\n[moneda] [varchar](30) NULL," +
                                "\n[procesada] [int] NULL," +
                                "\n[tipo_cambio] [varchar](50) NULL," +
                                "\n[subtotal] [varchar](30) NULL," +
                                "\n[total] [varchar](30) NULL," +
                             "\nCONSTRAINT [PK_Facturas] PRIMARY KEY CLUSTERED " +
                            "\n(" +
                                "\n[id] ASC " +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                            "\n) ON [PRIMARY] " +
                            "\nGO " +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[EntradasPresentaciones](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[presentacion_id] [int] NOT NULL," +
                                "\n[almacen_id] [int] NOT NULL," +
                                "\n[cantidad] [float] NULL," +
                                "\n[fecha_registro] [datetime] NULL," +
                             "\nCONSTRAINT [PK_EntradasPresentaciones_1] PRIMARY KEY CLUSTERED " +
                            "\n(" +
                                "\n[id] ASC" +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                            "\n) ON [PRIMARY]" +
                            "\nGO" +
                            "\nSET ANSI_NULLS ON" +
                            "\nGO" +
                            "\nSET QUOTED_IDENTIFIER ON" +
                            "\nGO" +
                            "\nSET ANSI_PADDING ON" +
                            "\nGO" +
                            "\nCREATE TABLE [dbo].[Empresas](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[nombre] [varchar](100) NULL," +
                                "\n[rfc] [nchar](15) NULL," +
                                "\n[calle] [varchar](50) NULL," +
                                "\n[noExterior] [nchar](10) NULL," +
                                "\n[codigo_postal] [nchar](10) NULL," +
                                "\n[colonia] [varchar](50) NULL," +
                                "\n[localidad] [varchar](50) NULL," +
                                "\n[municipio] [varchar](50) NULL," +
                                "\n[estado] [varchar](50) NULL," +
                                "\n[pais] [varchar](50) NULL," +
                                "\n[datasource] [varchar](500) NULL," +
                                "\nCONSTRAINT [PK_Empresa] PRIMARY KEY CLUSTERED" +
                            "\n(" +
                                "\n[id] ASC" +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                            "\n) ON [PRIMARY]" +
                            "\nGO" +
                            "\nSET ANSI_PADDING OFF" +
                            "\nGO" +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[Categorias](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[nombre] [varchar](30) NULL," +
                             "\nCONSTRAINT [PK_Categorias] PRIMARY KEY CLUSTERED" +
                            "\n(" +
                                "\n[id] ASC" +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                            "\n) ON [PRIMARY]" +
                            "\nGO " +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[AreasProduccion](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[nombre] [varchar](50) NULL," +
                             "\nCONSTRAINT [PK_AreasProduccion] PRIMARY KEY CLUSTERED" +
                            "\n(" +
                                "\n[id] ASC" +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                            "\n) ON [PRIMARY] " +
                            "\nGO " +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO " +
                            "\nSET ANSI_NULLS ON " +
                            "\nGO " +
                            "\nSET QUOTED_IDENTIFIER ON " +
                            "\nGO " +
                            "\nSET ANSI_PADDING ON " +
                            "\nGO " +
                            "\nCREATE TABLE [dbo].[Almacenes](" +
                                "\n[id] [int] IDENTITY(1,1) NOT NULL," +
                                "\n[nombre] [varchar](50) NULL," +
                                "\n[codigo] [varchar](50) NULL," +
                             "\nCONSTRAINT [PK_Almacenes] PRIMARY KEY CLUSTERED" +
                            "\n(" +
                                "\n[id] ASC" +
                            "\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                            "\n) ON [PRIMARY] " +
                            "\nGO " +
                            "\nSET ANSI_PADDING OFF " +
                            "\nGO ";

            try
            {
                System.IO.File.WriteAllText(@"C:\SharkScriptDB_" + companyName + ".sql", script);
                ProcessStartInfo cmd = new ProcessStartInfo("sqlcmd", "-S SANTOSV\\SHARKPOS -i C:\\SharkScriptDB_" + companyName + ".sql");
                cmd.UseShellExecute = false;
                cmd.CreateNoWindow = true;
                cmd.RedirectStandardOutput = true;
                Process execute = new Process();
                execute.StartInfo = cmd;
                execute.Start();
                execute.Close();
            }
            catch (Exception ex)
            {
                Console.Write("Error: " + ex);
                error = true;
            }

            return error;
        }

        /// <summary>
        /// Obtiene todas las bases de datos disponibles en el servidor / instancia seleccionada.
        /// </summary>
        /// <param name="instancia">servidor / instancia de donde se buscara o añadirá la base de datos.</param>
        /// <returns>un arreglo de las bases de datos.</returns>
        public static String[] obtenerBasesDeDatos(string instancia)
        {
            // Las bases de datos propias de SQL Server
            string[] basesSys = { "master", "model", "msdb", "tempdb" };
            string[] bases;
            DataTable dt = new DataTable();
            // Usamos la seguridad integrada de Windows
            string sCnn = "Server=" + instancia + "; database=master; integrated security=yes";

            // La orden T-SQL para recuperar las bases de master
            string sel = "SELECT name FROM sysdatabases";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sel, sCnn);
                da.Fill(dt);
                bases = new string[dt.Rows.Count - 1];
                int k = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string s = dt.Rows[i]["name"].ToString();
                    // Solo asignar las bases que no son del sistema
                    if (Array.IndexOf(basesSys, s) == -1)
                    {
                        k += 1;
                        bases[k] = s;
                    }
                }
                if (k == -1) return null;
                // ReDim Preserve
                {
                    int i1_RPbases = bases.Length;
                    string[] copiaDe_bases = new string[i1_RPbases];
                    Array.Copy(bases, copiaDe_bases, i1_RPbases);
                    bases = new string[(k + 1)];
                    Array.Copy(copiaDe_bases, bases, (k + 1));
                };
                return bases;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener las bases de datos :(", "AVISO SHARK");
            }
            return null;
        }




    }
}
