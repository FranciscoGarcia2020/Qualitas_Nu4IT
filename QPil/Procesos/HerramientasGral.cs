/*Fernando Rivelino Ortiz Martínez
 Nü4ItAutomation
 En está clase hay métodos genericos (Se pueden usar en cualquier proyecto) 
 pero que es posible el código requiera ajustes especiificos para el proyecto
 Además hay funciones que requieren enviar MessageBox por eso no van en la Dll
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Threading;
//Libreria para obtener la MacAddress de la computadora donde se pretende ejecutar el programa
using System.Net.NetworkInformation;

using System.Data;
using System.Threading;
using System.IO; // Manejo de Archivos y directorios
using System.Diagnostics; // Manejo de procesos activos

//Librerias Realizadas en NUit4Automation
using Nu4it;
using nu4itExcel;
using nu4itFox;
using Aspose.Pdf;
using Aspose.Pdf.Devices;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace QPil
{
    class HerramientasGral
    {

        private static readonly int NO = 0, SI = 1, MAX = 10000;
        public static bool VARBOOL_RESPUESTA_DUDA;

        //**************************************************************************************************************************************
        //METODOS GENERALES PARA CUALQUIER PROYECTO
        //**************************************************************************************************************************************

        #region MétodosGenerales


        //Función que regresa la MacAddress de la computadora donde se ejecuta el programa
        public static string ObtenMacAddress()
        {
            string id = "";
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            id = sMacAddress;
            return (id);
        }

        //Función que verifica si la ejecución se está llevando a cabo en ambiente de desarrollo y regresa true en ese caso y false en caso contrario
        public static bool AmbienteDesarrollo(string VersionProyecto)
        {
            bool EstoyDesarrollo = true;
            string Ruta = Directory.GetCurrentDirectory();
            if (Ruta.IndexOf(@"bin\Debug") < 0 && VersionProyecto.IndexOf(".vshost") < 0) { EstoyDesarrollo = false; }
            return EstoyDesarrollo;
        }


        //Método que busca en el campo (Campo) del DataTable (TABLA) el dato DatoBuscar y regresa la posción del renglon del DataTable de la primera ocurrencia o -1 en caso contrario
        public static int EnTablaElDatoDelCampoUbicadoEn(DataTable TABLA, string DatoBuscar, string Campo)
        {
            int IndPos, PosBuena, TotRenglones;
            bool DatoLocalizado = false;
            string DatoComparar;
            IndPos = 0;
            PosBuena = -1;
            TotRenglones = TABLA.Rows.Count;
            while (IndPos < TotRenglones && !DatoLocalizado)
            {
                DatoComparar = TABLA.Rows[IndPos][Campo].ToString();
                if (DatoComparar == DatoBuscar)
                {
                    PosBuena = IndPos;
                    DatoLocalizado = true;
                }
                IndPos++;
            }
            return PosBuena;
        }

        //Método que crea la estrucutura de un DatatTable con los Encabezados pasados por parametro (Encabezados)
        public static DataTable CrearEstructuraDT(List<string> Encabezados)
        {
            DataTable NuevaTabla = new DataTable();
            NuevaTabla.Clear();
            foreach (string Titulo in Encabezados) { NuevaTabla.Columns.Add(Titulo, typeof(string)); }
            return NuevaTabla;
        }

        //Método para abrir el cuadro de diálogo para seleccionar un archivo de Excel y regresa en string la ruta y nombre del archivo seleccionado
        public static string DialogoSeleccionarArchivoUsar(string RutaInicialSeleccionarArchivo, string NombreArchivo, string RutaBitacoraLog)
        {
            string ArchivoResultado = "";
            usaR objNu4 = new usaR();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Escoge el archivo " + NombreArchivo + " para trabajar";
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm;*.xlsb;";
            openFileDialog1.InitialDirectory = RutaInicialSeleccionarArchivo;
            if (openFileDialog1.ShowDialog() == true)
            {
                ArchivoResultado = openFileDialog1.FileName;
            }
            else
            {
                ArchivoResultado = "";
                objNu4.ReportarLog(RutaBitacoraLog, "No se puede trabajar porque no se selecciona ningún archivo " + NombreArchivo);
                MsgAlertar("No se puede trabajar porque no se selecciona ningún archivo " + NombreArchivo);
            }
            return (ArchivoResultado);
        }

        //Método para Obtener el Archivo de Excel con el que el bot va a trabajar. En caso de obtenerlo regresa True y en caso contrario False
        public static bool ObtenerArchivoExcelUsar(string NombreArchivo, string IdentificadorEnIni, string[] ContenidoIni, string RutaBitacoraLog)
        {
            string ArchivoResultado = "", RutaArchivo, NomArch;
            bool ArchivoObtenido = true;
            usaR objNu4 = new usaR();
            int PosRutaINI, OcIni, PosArchivo;
            string[] ArchivosEnRuta = new string[0];
            objNu4.ReportarLog(RutaBitacoraLog, "Se buscará archivo abierto");
            ArchivoObtenido = AutomatizadorExcel.ObtenerArchivoExcelAbierto(NombreArchivo, RutaBitacoraLog);
            if (!ArchivoObtenido)
            {
                objNu4.ReportarLog(RutaBitacoraLog, "Se buscará archivo cerrado");
                OcIni = objNu4.OcurrenciasEnArreglo(ContenidoIni, IdentificadorEnIni, NO);
                if (OcIni == 1)
                {
                    PosRutaINI = objNu4.UbicadoEnPos(ContenidoIni, IdentificadorEnIni, NO);
                    RutaArchivo = ContenidoIni[PosRutaINI];
                    RutaArchivo = RutaArchivo.Replace(IdentificadorEnIni, "");
                    ArchivosEnRuta = Directory.GetFiles(RutaArchivo);
                    OcIni = objNu4.OcurrenciasEnArreglo(ArchivosEnRuta, NombreArchivo, SI);
                    if (OcIni > 0)
                    {
                        if (OcIni == 1)
                        {
                            PosArchivo = objNu4.UbicadoEnPos(ArchivosEnRuta, NombreArchivo, SI);
                            NomArch = System.IO.Path.GetFileName(ArchivosEnRuta[PosArchivo]);
                            bool result = MsgDuda("Encontre este archivo " + NomArch + " en la ruta.\n Deseas utilizarlo? ");
                            if (result)
                            {
                                ArchivoResultado = ArchivosEnRuta[PosArchivo];
                                objNu4.ReportarLog(RutaBitacoraLog, "SE ABRIRA EL ARCHIVO: " + ArchivoResultado);
                            }
                            else { ArchivoResultado = DialogoSeleccionarArchivoUsar(RutaArchivo, NombreArchivo, RutaBitacoraLog); }//boton no del mensaje

                        }
                        else //hay mas de dos archivos en la ruta con el mismo nombre
                        {
                            objNu4.ReportarLog(RutaBitacoraLog, "Hay mas de un archivo " + NombreArchivo + " en la ruta y no se cual leer");
                            MsgAlertar("Hay mas de un archivo " + NombreArchivo + " en la ruta y no se cual leer");
                            ArchivoResultado = DialogoSeleccionarArchivoUsar(RutaArchivo, NombreArchivo, RutaBitacoraLog);
                        }
                    }
                    else //cuando no hay archivos
                    {
                        objNu4.ReportarLog(RutaBitacoraLog, "No hay archivos " + NombreArchivo + " en la ruta");
                        MsgAlertar("No hay archivos " + NombreArchivo + " en la ruta");
                        ArchivoResultado = DialogoSeleccionarArchivoUsar(RutaArchivo, NombreArchivo, RutaBitacoraLog);
                    }
                }
                else
                {
                    ArchivoResultado = "";
                    objNu4.ReportarLog(RutaBitacoraLog, "NO SE IDENTIFICO LA RUTA DEL ARCHIVO EN EL INI.");
                    MsgAlertar("NO SE IDENTIFICO LA RUTA DEL ARCHIVO EN EL INI.");
                }
                if (!string.IsNullOrEmpty(ArchivoResultado)) { AutomatizadorExcel.AbrirElArchivoUsar(ArchivoResultado); ArchivoObtenido = true; }
                else { ArchivoObtenido = false; }
            }
            return ArchivoObtenido;
        }

        //Método que elimina los registros del DatatTable TABLA que en el campo (Campo) el dato sea nuloo o vacío
        public static DataTable QuitarRenglonesNullosCampo(DataTable TABLA, string Campo)
        {
            DataTable TablaSinNulos = new DataTable();
            TablaSinNulos.Clear();
            TablaSinNulos = TABLA.Copy();
            int Pos;
            var Vacios = from RegVacio in TABLA.AsEnumerable()
                         where string.IsNullOrEmpty(RegVacio.Field<string>(Campo))
                         select RegVacio;

            foreach (var RegEliminar in Vacios)
            {
                Pos = EnTablaElDatoDelCampoUbicadoEn(TablaSinNulos, "", Campo);
                TablaSinNulos.Rows.RemoveAt(Pos);
            }
            return TablaSinNulos;
        }

        //Procedimiento que cierra Drivers Abiertos de Chrome para automatizar portal 
        public static void cerrarDriver()
        {
            Thread.Sleep(500);
            Process[] myProcesses = Process.GetProcessesByName("chromedriver");
            foreach (Process myProcess in myProcesses) { myProcess.Kill(); }
        }

        public static void MsgAlertar(string Alerta)
        {
            QPil.Pages.Mensajes.MnsjOK objAlerta = new QPil.Pages.Mensajes.MnsjOK(Alerta);
        }

        public static void MsgAviso(string Mensaje)
        {
            QPil.Pages.Mensajes.MnsjOK objAviso = new QPil.Pages.Mensajes.MnsjOK(Mensaje);
        }

        public static bool MsgDuda(string Pregunta)
        {
            bool Respuesta = true;
            QPil.Pages.Mensajes.MsgYesNo objAviso = new QPil.Pages.Mensajes.MsgYesNo();
            objAviso.lblAvisoContent.Text = Pregunta;
            objAviso.ShowDialog();
            Respuesta = VARBOOL_RESPUESTA_DUDA;
            return Respuesta;
        }

        #endregion


        //obtiene el primer elemento, de una lista, donde se encuentra la palabra a buscar 
        public static string UbicadoList(List<string> lista, string palabraBuscar)
        {
           string patternSearch = palabraBuscar;

            string resultFind = lista.Find(
                 delegate (string current)
                 {
                     return current.Contains(patternSearch);
                 }
            );

            return resultFind;
        }

        //Método que recibe la ruta del documento PDF de entrada y el nombre del archivo de salida y devuelve la ruta de la imagen generada
        public static string PDFaPNG(string rutaPDF, string nombreArchivoSalida) 
        {
            // El directorio de los documentos

            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\PDFaImagen\"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\PDFaImagen\");

            }

            string dataDir = "PDFaImagen/";
            int pageCount = 1;
            FileStream imagenPdf;
            // Abre documento
            string rutaPNG = dataDir + nombreArchivoSalida + ".png";
            Document documentoPdf = new Document(rutaPDF);
            using (imagenPdf = new FileStream(rutaPNG, FileMode.Create))
            {
                // Crea una imagen JPEG con atributos especificados.
                // Ancho, Alto, Resolucion, Calidad
                // Calidad [0-100], 100 es la maxima
                // Crea un objeto tipo resolución
                Resolution resolucion = new Resolution(300);

                // JpegDevice jpegDevice = new JpegDevice(500, 700, resolution, 100);
                JpegDevice jpegDevice = new JpegDevice(resolucion, 100);

                //Convierte una página y guarda la imagen
                jpegDevice.Process(documentoPdf.Pages[pageCount], imagenPdf);

                // Cierra stream
                imagenPdf.Close();

            }
            return rutaPNG;

        } 

        //Método que recibe la ruta del PDF, tiene sobrecarga si no se le indica el número de paginas a leer; leerá todas las páginas. 
        public static string LeeArchivoPDF(string rutaPDF, int? paginas = null)
        {
            StringBuilder text = new StringBuilder();

            if (File.Exists(rutaPDF))
            {
                PdfReader pdfReader = new PdfReader(rutaPDF);

                if (paginas.HasValue)
                {
                    for (int page = 1; page <= paginas; page++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                        currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                        text.Append(currentText);
                    }
                    pdfReader.Close();
                }
                else
                {
                    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                        currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                        text.Append(currentText);
                    }
                    pdfReader.Close();
                }
            }
            return text.ToString();
        }
    }
}
