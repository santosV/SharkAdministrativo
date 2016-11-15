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
        public struct ValorClasificacion
        {
            public int cClasificacionDe;
            public int cNumClasificacion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongDescripcion)]
            public string cValorClasificacion;
        }

  
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tUnidad
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongNombre)]
            public string cNombreUnidad;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongAbreviatura)]
            public string cAbreviatura;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = constantes.kLongAbreviatura)]
            public string cDespliegue;
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


        //Funciones De Conexión.
        [DllImport("KERNEL32")]
        public static extern int SetCurrentDirectory(string pPtrDirActual);
        [DllImport("MGWSERVICIOS.dll")]
        public static extern int fSetNombrePAQ(string aNombrePAQ);
        [DllImport("MGWSERVICIOS.dll")]
        public static extern void fTerminaSDK();

        //Funciones De Empresa.
        [DllImport("MGWSERVICIOS.dll")]
        public static extern int fAbreEmpresa(string Directorio);
        [DllImport("MGWSERVICIOS.dll")]
        public static extern void fCierraEmpresa();
       
        
        //Funciones De Clasificación.
        [DllImport("MGWSERVICIOS.dll")]
        public static extern int fBuscaClasificacion(int aClasificacionDe, int aNumClasificacion);
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fPosUltimoClasificacion();
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fLeeDatoClasificacion(string aCampo, StringBuilder aVal, int aLen);
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fBuscaValorClasif(int aClasificacionDe, int aNumClasificacion, string aCodValorClasif);
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fPosPrimerValorClasif();

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fPosSiguienteValorClasif();

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fLeeDatoValorClasif(string aCampo, StringBuilder aValor, int aLen);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fBuscaIdValorClasif (int aIdValorClasif);

        

        //Funciones De Error.
        [DllImport("mgwservicios.dll")]
        public static extern void fError(int NumeroError, StringBuilder Mensaje, int Longitud);

        //Funciones De Documento.
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
        [DllImport("MGWSERVICIOS.DLL")]
        public static extern int fDocumentoUUID(StringBuilder aCodigoConcepto, StringBuilder aSerie, double aFolio, StringBuilder atPtrCFDIUUID);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fSetDatoDocumento(string aCampo, string aValor);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fEditarDocumento();




        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fGuardaDocumento();

        //Funciones De Producto.
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fAltaProducto(ref Int32 aldProducto, ref tProduto astProducto);


        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fBuscaProducto(string aCodProducto);
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fEditaProducto();
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fGuardaProducto();
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fEliminarProducto(string aCodProducto);
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fSetDatoProducto(string aCampo, string aValor);

        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fLeeDatoProducto(string aCampo, StringBuilder aValor, int longitud);

        //Funciones de Almacen
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fPosPrimerAlmacen();
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fPosSiguienteAlmacen();
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fLeeDatoAlmacen(String aCampo, StringBuilder aValor, int longitud);

        //Funciones De Unidad y peso.
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fAltaUnidad(ref Int32 aldUnidad, ref tUnidad astUnidad);
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fPosPrimerUnidad();
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fPosSiguienteUnidad(); 
        [DllImport("MGWSERVICIOS.dll")]
        public static extern Int32 fLeeDatoUnidad(string aCampo, StringBuilder aValor, int longitud);

        
        //Funciones De Memoria.
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        extern static uint _controlfp(uint newcw, uint mask);

        const uint _MCW_EM = 0x0008001f;
        const uint _EM_INVALID = 0x00000010;

        public static void FixFPU()
        {
            {
                _controlfp(_MCW_EM, _EM_INVALID);
            }
        }


        public static void alzheimer()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
        }

        /// <summary>
        /// Personaliza el error detallado.
        /// </summary>
        /// <param name="iError"></param>
        public static void rError(int iError)
        {
            StringBuilder sMensaje = new StringBuilder(512);

            if (iError != 0)
            {
                SDK.fError(iError, sMensaje, 512);
                System.Windows.Forms.MessageBox.Show("Error: " + sMensaje, "Aviso Shark");

            }
        }

        /// <summary>
        /// Inicializa el SDK y abre la empresa.
        /// </summary>
        /// <returns></returns>
        public static int startSDK()
        {
            int success = 1;
            SetCurrentDirectory(SDK.systemRoute);
            int error = int.MinValue;
            try
            {
                error = SDK.fSetNombrePAQ(SDK.systemName);
                if (error != 0)
                {
                    rError(error);
                }
                else
                {

                    error = SDK.fAbreEmpresa(SDK.companyRoute);
                    if (error != 0)
                    {
                        rError(error);
                    }
                    else
                    {
                        success = 0;
                    }
                }
                throw new Exception("Ignora los FPu");
            }catch(Exception ex){}
            return success;
        }

        /// <summary>
        /// Cierra la empresa y el SDK.
        /// </summary>
        public static void closeSDK()
        {
            fCierraEmpresa();
            fTerminaSDK();
        }






    }//Fin clase SDK
}//Fin namespace
