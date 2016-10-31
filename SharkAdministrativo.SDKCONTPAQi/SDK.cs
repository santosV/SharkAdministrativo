using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace SharkAdministrativo.SDKCONTPAQi
{
    public class constantes // Declaración de constantes
    {
        public const int kLongFecha = 24;
        public const int kLongSerie = 12;
        public const int kLongCodigo = 31;
        public const int kLongNombre = 61;
        public const int kLongReferencia = 21;
        public const int kLongDescripcion = 61;
        public const int kLongCuenta = 101;
        public const int kLongMensaje = 3001;
        public const int kLongNombreProducto = 256;
        public const int kLongAbreviatura = 4;
        public const int kLongCodValorClasif = 4;
        public const int kLongDenComercial = 51;
        public const int kLongRepLegal = 51;
        public const int kLongTextoExtra = 51;
        public const int kLongRFC = 21;
        public const int kLongCURP = 21;
        public const int kLongDesCorta = 21;
        public const int kLongNumeroExtInt = 7;
        public const int kLongNumeroExpandido = 31;
        public const int kLongCodigoPostal = 7;
        public const int kLongTelefono = 16;
        public const int kLongEmailWeb = 51;

        public const int kLongSelloSat = 176;
        public const int kLonSerieCertSAT = 21;
        public const int kLongFechaHora = 36;
        public const int kLongSelloCFDI = 176;
        public const int kLongCadOrigComplSAT = 501;
        public const int kLongitudUUID = 37;
        public const int kLongitudRegimen = 101;
        public const int kLongitudMoneda = 61;
        public const int kLongitudFolio = 17;
        public const int kLongitudMonto = 31;
        public const int kLogitudLugarExpedicion = 401;

    }//Fin constantes


     public class SDK
    {
        public static string companyRoute { get; set; }
        public static string companyName { get; set; }
        public static string systemRoute = @"C:\Program Files (x86)\Compac\COMERCIAL";
        public static string systemName = "CONTPAQ I COMERCIAL";


        // Declaración de la estructura del producto

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tProduto
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string cCodigoProducto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongNombre)]
            public string cNombreProducto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongNombreProducto)]
            public string cDescripcionProducto;
            public int cTipoProducto; // 1 = Producto, 2 = Paquete, 3 = Servicio
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public string cFechaAltaProducto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public string cFechaBaja;
            public int cStatusProducto; // 0 - Baja Lógica, 1 - Alta
            public int cControlExistencia;
            public int cMetodoCosteo; // 1 = Costo Promedio en Base a Entradas, 2 = Costo Promedio en Base a Entradas Almacen, 3 = Último costo, 4 = UEPS, 5 = PEPS, 6 = Costo específico, 7 = Costo Estandar
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string cCodigoUnidadBase;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string cCodigoUnidadNoConvertible;
            public double cPrecio1;
            public double cPrecio2;
            public double cPrecio3;
            public double cPrecio4;
            public double cPrecio5;
            public double cPrecio6;
            public double cPrecio7;
            public double cPrecio8;
            public double cPrecio9;
            public double cPrecio10;
            public double cImpuesto1;
            public double cImpuesto2;
            public double cImpuesto3;
            public double cRetencion1;
            public double cRetencion2;
            // N.D.8386 La estructura debe recibir el nombre de la característica padre. (ALRH)
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongNombre)]
            public string cNombreCaracteristica1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongNombre)]
            public string cNombreCaracteristica2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongNombre)]
            public string cNombreCaracteristica3;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion1;//[ kLongCodValorClasif + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion2;//[ kLongCodValorClasif + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion3;//[ kLongCodValorClasif + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion4;//[ kLongCodValorClasif + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion5;//[ kLongCodValorClasif + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion6;//[ kLongCodValorClasif + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongTextoExtra)]
            public string cTextoExtra1;//[ kLongTextoExtra + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongTextoExtra)]
            public string cTextoExtra2;//[ kLongTextoExtra + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongTextoExtra)]
            public string cTextoExtra3;//[ kLongTextoExtra + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public string cFechaExtra;//[ kLongFecha + 1 ];
            public double cImporteExtra1;
            public double cImporteExtra2;
            public double cImporteExtra3;
            public double cImporteExtra4;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tDocumento
        {

            public Double aFolio;
            public int aNumMoneda;
            public Double aTipoCambio;
            public Double aImporte;
            public Double aDescuentoDoc1;
            public Double aDescuentoDoc2;
            public int aSistemaOrigen;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public String aCodConcepto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongSerie)]
            public String aSerie;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public String aFecha;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public String aCodigoCteProv;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public String aCodigoAgente;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongReferencia)]
            public String aReferencia;
            public int aAfecta;
            public int aGasto1;
            public int aGasto2;
            public int aGasto3;

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tMovimiento
        {
            public int aConsecutivo;
            public Double aUnidades;
            public Double aPrecio;
            public Double aCosto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public String aCodProdSer;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public String aCodAlmacen;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongReferencia)]
            public String aReferencia;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public String aCodClasificacion;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tSeriesCapas
        {
            public double aUnidades;
            public double aTipoCambio;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodigo)]
            public string aSeries;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongDescripcion)]
            public string aPedimento;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongDescripcion)]
            public string aAgencia;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public string aFechaPedimento;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongDescripcion)]
            public string aNumeroLote;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public string aFechaFabricacion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongFecha)]
            public string aFechaCaducidad;


        }


        //Funciones
        [DllImport("KERNEL32")]
        public static extern int SetCurrentDirectory(string pPtrDirActual);

        [DllImport("mgwservicios.dll")]
        public static extern void fTerminaSDK();

        [DllImport("mgwservicios.DLL")]
        public static extern int fSetNombrePAQ(String aNombrePAQ);

        [DllImport("mgwservicios.dll")]
        public static extern int fAbreEmpresa(string Directorio);

        [DllImport("mgwservicios.dll")]
        public static extern void fCierraEmpresa();

        [DllImport("mgwservicios.dll")]
        public static extern void fError(int NumeroError, StringBuilder Mensaje, int Longitud);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fAltaDocumento(ref Int32 aIdDocumento, ref tDocumento atDocumento);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fAltaMovimiento(Int32 aIdDocumento, ref Int32 aIdMovimiento, ref tMovimiento atMovimiento);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fBuscarDocumento(string aCodConcepto, string aSerie, string aFolio);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fEmitirDocumento([MarshalAs(UnmanagedType.LPStr)] string aCodConcepto, [MarshalAs(UnmanagedType.LPStr)] string aSerie, double aFolio, [MarshalAs(UnmanagedType.LPStr)] string aPassword, [MarshalAs(UnmanagedType.LPStr)] string aArchivoAdicional);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fEntregEnDiscoXML([MarshalAs(UnmanagedType.LPStr)] string aCodConcepto, [MarshalAs(UnmanagedType.LPStr)] string aSerie, double aFolio, int aFormato, string aFormatoAmigable);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fAltaProducto(ref Int32 aldProducto,ref tProduto astProducto);
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fBuscaProducto(string aCodProducto);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fEliminarProducto(string aCodProducto);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fEditaProducto();
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fGuardaProducto();

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fSetDatoProducto(string aCampo, string aValor);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fLeeDatoProducto(string aCampo, StringBuilder aValor, int longitud);
      

        
        [DllImport("MGWSERVICIOS.DLL")]
        public static extern int fDocumentoUUID(StringBuilder aCodigoConcepto, StringBuilder aSerie, double aFolio, StringBuilder atPtrCFDIUUID);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fSetDatoDocumento(string aCampo, string aValor);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fEditarDocumento();

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fGuardaDocumento();



        // Función para el manejo de errores en SDK
        public static void rError(int iError)
        {
            StringBuilder sMensaje = new StringBuilder(512);

            if (iError != 0)
            {
                SDK.fError(iError, sMensaje, 512);
                System.Windows.Forms.MessageBox.Show("Error: " + sMensaje,"Aviso Shark");   
           
            }
        }



    }//Fin clase SDK
}//Fin namespace
