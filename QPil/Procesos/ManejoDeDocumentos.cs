using Nu4it;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Data;
using nu4itFox;
using System.Windows.Threading;
//LIBRERIAS AGREGADAS 
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Collections.Generic;


namespace QPil.Procesos
{
    class ManejoDeDocumentos : UserControl

    {
        string directorioEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\prueba";
        usaR objNu4 = new usaR();
        Dlls.Nu4it nu4it = new Dlls.Nu4it();
        AutomatizacionOCR objOCR = new AutomatizacionOCR();
        public static string VARSTR_BITACORA_LOG;
        //-----OHA-------------
        public static DataTable Facturas;
        DataTable FacturasRecibe;
        //DataTable Polizas;
        public static DataTable Polizas6Col;
        DataTable Polizas6ColRecibe;
        dynamic sat = (dynamic)null;
        //   public static bool ValidacionDocumento { get; set; }
        public static string[] Documentos { get; set; }

        public static DataTable DT_DATOS_SAT { get; set; }
        public static DataTable DT_DATOS_SAT2 { get; set; }
        public static DataTable DT_DATOS_REPUVE { get; set; }
        //TABLA DE RESULTADO DE CONSULTA EN SAT2
        public static DataTable DT_SAT2_RES { get; set; }

        string endoso = "";
        string inciso = "";
        string serie = "";
        string modelo = "";
        string rfc = "";
        string placas = "";
        string vigencia = "";
        string serie2 = "";
        string folio = "";
        string folio2 = "";
        string importe = "";
        string RFC = "";
        string motor = "";
        string rfc2 = "";
        string fecha = "";
        string fecha2 = "";
        string factura = "";
        string factura2 = "";
        string poliza = "";
        string poliza2 = "";
        string noAprobacion = "";
        string anio = "";
        string certificado = "";
        string DM, RT;
        int pos = 1;
        //-----OHA-------------

        public ManejoDeDocumentos(string RutaLog)
        {
            VARSTR_BITACORA_LOG = RutaLog;
            // CONTENIDOINI = objNu4.LeerArchivoIni("OCR");
        }
        public void obtenCarpetas(string ruta, string siniestro)
        {
            string[] directorios = Directory.GetDirectories(ruta);
            foreach (string directorio in directorios)
            {
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, directorio);
                string carpeta = System.IO.Path.GetFileName(directorio);
                string nombreDirectorioSiniestro = ruta + carpeta;
                leePDFsinOCR(nombreDirectorioSiniestro, siniestro);
                //---- OHA PRUEBA -------
                //leePDFconOCR(nombreDirectorioSiniestro);

                //---- OHA PRUEBA -------
            }
        }

        //Método que recibe el nombre de la carpeta y Procesa PDF que tengan texto con formato.
        public void leePDFsinOCR(string nombreDirectorioSiniestro, string siniestro)
        {
            //--------OHA--------
            initDTFacturas();
            initDTPolizas6Col();
            string nombreDirectorioSiniestro2 = nombreDirectorioSiniestro;
            //-------OHA----------

            //&string nombreDirectorioSiniestro = /*directorioEscritorio + @"\Siniestros\"*/ruta + carpeta;
            string[] directorioPDF = Directory.GetFiles(nombreDirectorioSiniestro);

            foreach (string documento in directorioPDF)
            {
                //----- OHA PRUEBA -----
                string documento2 = documento;
                //leePDFconOCR(nombreDirectorioSiniestro2, documento2);

                //----- OHA PRUEBA -----
                string categoria = HerramientasGral.LeeArchivoPDF(documento, 1); // obtiene todo el string del documento.
                string extraido = categoria; // se almacena en esta variable antes de usar Modifica
                categoria = objNu4.Modifica(categoria);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se busca comparar el archivo " + System.IO.Path.GetFileNameWithoutExtension(documento) + " con un patrón de Póliza. sin OCR");
                //compara con  palabras clave de Póliza
                if ((categoria.Contains("POLIZA") && categoria.Contains("COBERTURA")) &&
                    (!categoria.Contains("CONTRIBUYENTE") || !categoria.Contains("CFDI")))
                {
                    //MessageBox.Show(Path.GetFileNameWithoutExtension(documento) + " ES UNA POLIZA " + extraido);

                    if (!Directory.Exists(nombreDirectorioSiniestro + @"\Poliza"))
                    {
                        Directory.CreateDirectory(nombreDirectorioSiniestro + @"\Poliza");
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se crea el directorio: " + (nombreDirectorioSiniestro + @"\Poliza").ToString());
                    }
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se encontró que el archivo " + System.IO.Path.GetFileNameWithoutExtension(documento) + " es una Póliza. sin OCR");
                    File.Move(documento, System.IO.Path.Combine((nombreDirectorioSiniestro + @"\Poliza"), System.IO.Path.GetFileName(documento)));

                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se movió el documento " + System.IO.Path.GetFileNameWithoutExtension(documento) + " al directorio de Poliza. sin OCR");

                    String rutaArchivo = System.IO.Path.Combine((nombreDirectorioSiniestro + @"\Poliza"), System.IO.Path.GetFileName(documento)).ToString();
                    Polizas6ColRecibe = leerPoliza(extraido, rutaArchivo, siniestro);
                }
                //Si no  compara con  palabras clave de Factura
                else if ((categoria.Contains("FACTURA") || categoria.Contains("CONCESIONARIO") || categoria.Contains("CLIENTE") || categoria.Contains("RFC"))
                    && !(categoria.Contains("FINANZAS") || categoria.Contains("GOBIERNO") || categoria.Contains("SECRETARIA") /*|| categoria.Contains("ESTADO")*/
                                                                                                                              /*|| categoria.Contains("BAJA")*/ || categoria.Contains("REFRENDO")))
                {
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se busca comparar el archivo " + System.IO.Path.GetFileNameWithoutExtension(documento) + " con un patrón de Factura sin OCR. sin OCR");
                    //categoria = objOCR.ocrRectanguloSinProcesar(rutaImagen, 0, 0, 2400, 1100);//categoria = Ocr_Rectangulo_SinProcesar(rutaImagen, 1780, 240, 254, 35);
                    // MessageBox.Show(Path.GetFileNameWithoutExtension(documento) + " ES UNA FACTURA " +extraido);          
                    if (!Directory.Exists(nombreDirectorioSiniestro + @"\Factura"))
                    {
                        Directory.CreateDirectory(nombreDirectorioSiniestro + @"\Factura");
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se crea el directorio: " + (nombreDirectorioSiniestro + @"\Factura").ToString());
                    }

                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se encontró que el archivo " + System.IO.Path.GetFileNameWithoutExtension(documento) + " es una Factura. sin OCR");
                    File.Move(documento, System.IO.Path.Combine((nombreDirectorioSiniestro + @"\Factura"), System.IO.Path.GetFileName(documento)));
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se movió el documento " + System.IO.Path.GetFileNameWithoutExtension(documento) + " al directorio de Factura. sin OCR");

                    /*Regex regex = new Regex(@"[a-zA-Z-0-9- ]{3,4}(\d{6})((\D|\d){2,4})");
                    Match match = regex.Match(extraido);
                    string rfc_ = match.Value;
                    */

                    //------------OHA-------------

                    String rutaArchivo = System.IO.Path.Combine((nombreDirectorioSiniestro + @"\Factura"), System.IO.Path.GetFileName(documento)).ToString();
                    FacturasRecibe = leerFactura6Col(extraido, rutaArchivo, pos, siniestro);


                    string espera = "";


                    //------------OHA------------

                    pos++;
                }
                // Si no  compara  con  palabras clave de Tenencia
                else if ((categoria.Contains("FINANZAS") || categoria.Contains("SECRETARIA") || categoria.Contains("GOBIERNO") ||
                        categoria.Contains("CONTRIBUYENTE") || categoria.Contains("PAGO") || categoria.Contains("DELEGACION"))
                        && !(categoria.Contains("CLIENTE") || categoria.Contains("CONTRATO") || categoria.Contains("ACTA") || categoria.Contains("BAJA")
                        || categoria.Contains("COMPRA")))
                {
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se busca comparar el archivo " + System.IO.Path.GetFileNameWithoutExtension(documento) + " con un patrón de Tenencia sin OCR. sin OCR");
                    // categoria = objOCR.ocrRectanguloSinProcesar(rutaImagen, 0, 0, 2400, 1100);//categoria = Ocr_Rectangulo_SinProcesar(rutaImagen, 400, 240, 254, 35);

                    //MessageBox.Show(Path.GetFileNameWithoutExtension(documento) + " ES UNA TENENCIA");
                    if (!Directory.Exists(nombreDirectorioSiniestro + @"\Tenencia"))
                    {
                        Directory.CreateDirectory(nombreDirectorioSiniestro + @"\Tenencia");
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se crea el directorio: " + (nombreDirectorioSiniestro + @"\Tenencia").ToString());
                    }

                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se encontró que el archivo " + System.IO.Path.GetFileNameWithoutExtension(documento) + " es una Tenencia. sin OCR");
                    File.Move(documento, System.IO.Path.Combine((nombreDirectorioSiniestro + @"\Tenencia"), System.IO.Path.GetFileName(documento)));
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se movió el documento " + System.IO.Path.GetFileNameWithoutExtension(documento) + " al directorio de Tenencia. sin OCR");

                }
                // No pudo clasificar y se procede a OCR
                else
                {

                    //MessageBox.Show(Path.GetFileNameWithoutExtension(documento) + " No se pudo clasificar el archivo " + categoria);
                    leePDFconOCR(nombreDirectorioSiniestro, documento, siniestro);
                    //pos++;
                }
                DT_DATOS_SAT = Facturas;
                DT_DATOS_SAT2 = Facturas;
                DT_DATOS_REPUVE = Facturas;

                //Forms.ValidaDatosSat.DT_DATOS_SAT = DT_DATOS_SAT;
                //Forms.ValidaDatosSat.DT_DATOS_SAT2 = DT_DATOS_SAT2;
                //Forms.ValidaDatosSat.DT_DATOS_REPUVE = DT_DATOS_REPUVE;

            }
            Procesos.CaptchaSAT sat = new CaptchaSAT(VARSTR_BITACORA_LOG);
            Procesos.CaptchaSAT2 sat2 = new CaptchaSAT2(VARSTR_BITACORA_LOG);
            Procesos.CaptchaRepuve repuve = new CaptchaRepuve(VARSTR_BITACORA_LOG);

            sat.recibeDatatableSAT(Facturas);

            DataTable table = DT_DATOS_SAT;
            var tab = from t in table.AsEnumerable()
                      where t.Field<bool>("EXITO").Equals(false)
                      select t;

            DataTable SAT2 = tab.CopyToDataTable();
            sat2.recibeDatatableSAT2(SAT2, siniestro);


            repuve.recibeDatatableRepuve(Facturas, siniestro);

            
            //new Procesos.DatosLog().HiloDeEjecucion(VARSTR_BITACORA_LOG);
        }

        // Método que recibe el nombre de la carpeta y el documento que se va a procesar con OCR 
        public void leePDFconOCR(string nombreDirectorioSiniestro, string documento, string siniestro)
        {
            try
            {

                //string nombreDirectorioSiniestro = /*directorioEscritorio + @"\Siniestros\"*/ruta + carpeta;
                string rutaImagen = HerramientasGral.PDFaPNG(documento, System.IO.Path.GetFileNameWithoutExtension(documento));
                Bitmap imagen = new Bitmap(rutaImagen);
                string categoria = objOCR.reconocerTextoImagenSinFiltro(imagen);
                string extraido = categoria;
                categoria = objNu4.Modifica(categoria);

                // string categoria = objOCR.ocrRectanguloSinProcesar(rutaImagen, 0, 0, 2400, 600);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se busca comparar el archivo " + System.IO.Path.GetFileNameWithoutExtension(documento) + " con un patrón de Póliza");
                if ((categoria.Contains("POLIZA") || categoria.Contains("POLIZADE")) && !categoria.Contains("CONTRIBUYENTE"))
                {
                    //MessageBox.Show(Path.GetFileNameWithoutExtension(documento) + " ES UNA POLIZA ");
                    if (!Directory.Exists(nombreDirectorioSiniestro + @"\Poliza"))
                    {
                        Directory.CreateDirectory(nombreDirectorioSiniestro + @"\Poliza");
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se crea el directorio: " + (nombreDirectorioSiniestro + @"\Poliza").ToString());
                    }
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se encontró que el archivo " + System.IO.Path.GetFileNameWithoutExtension(documento) + " es una Póliza");
                    File.Move(documento, System.IO.Path.Combine((nombreDirectorioSiniestro + @"\Poliza"), System.IO.Path.GetFileName(documento)));

                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se movió el documento " + System.IO.Path.GetFileNameWithoutExtension(documento) + " al directorio de Poliza");
                    Regex regex = new Regex(@"\d{10}");
                    //Match match = regex.Match(extraido);
                    //string poliza = match.Value;
                    //MessageBox.Show(poliza);

                    String rutaArchivo = System.IO.Path.Combine((nombreDirectorioSiniestro + @"\Poliza"), System.IO.Path.GetFileName(documento)).ToString();
                    Polizas6ColRecibe = leerPoliza(extraido, rutaArchivo, siniestro);


                }
                //Comapara Si la captura contiene palabras clave es Factura
                else if ((categoria.Contains("FACTURA") || categoria.Contains("CONCESIONARIO") || categoria.Contains("CLIENTE"))
                         && !categoria.Contains("CONTRIBUYENTE"))
                {
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se busca comparar el archivo " + System.IO.Path.GetFileNameWithoutExtension(documento) + " con un patrón de Factura");
                    //categoria = objOCR.ocrRectanguloSinProcesar(rutaImagen, 0, 0, 2400, 1100);//categoria = Ocr_Rectangulo_SinProcesar(rutaImagen, 1780, 240, 254, 35);
                    //MessageBox.Show(Path.GetFileNameWithoutExtension(documento) + " ES UNA FACTURA");
                    if (!Directory.Exists(nombreDirectorioSiniestro + @"\Factura"))
                    {
                        Directory.CreateDirectory(nombreDirectorioSiniestro + @"\Factura");
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se crea el directorio: " + (nombreDirectorioSiniestro + @"\Factura").ToString());
                    }

                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se encontró que el archivo " + System.IO.Path.GetFileNameWithoutExtension(documento) + " es una Factura");
                    File.Move(documento, System.IO.Path.Combine((nombreDirectorioSiniestro + @"\Factura"), System.IO.Path.GetFileName(documento)));
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se movió el documento " + System.IO.Path.GetFileNameWithoutExtension(documento) + " al directorio de Factura");
                    //Regex regex = new Regex(@"([A-Z])\w{11,12}");
                    //Match match = regex.Match(extraido);
                    //string folio = match.Value;
                    String rutaArchivo = System.IO.Path.Combine((nombreDirectorioSiniestro + @"\Factura"), System.IO.Path.GetFileName(documento)).ToString();
                    FacturasRecibe = leerFactura6Col(extraido, rutaArchivo, pos, siniestro);
                    pos++;
                }
                // Si no  compara la captura con  palabras clave de Tenencia
                else if ((categoria.Contains("FINANZAS") || categoria.Contains("SECRETARIA") || categoria.Contains("GOBIERNO") ||
                        categoria.Contains("CONTRIBUYENTE") || categoria.Contains("PAGO")) && !categoria.Contains("CLIENTE"))
                {
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se busca comparar el archivo " + System.IO.Path.GetFileNameWithoutExtension(documento) + " con un patrón de Tenencia");
                    // categoria = objOCR.ocrRectanguloSinProcesar(rutaImagen, 0, 0, 2400, 1100);//categoria = Ocr_Rectangulo_SinProcesar(rutaImagen, 400, 240, 254, 35);                   

                    //MessageBox.Show(Path.GetFileNameWithoutExtension(documento) + " ES UNA TENENCIA");
                    if (!Directory.Exists(nombreDirectorioSiniestro + @"\Tenencia"))
                    {
                        Directory.CreateDirectory(nombreDirectorioSiniestro + @"\Tenencia");
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se crea el directorio: " + (nombreDirectorioSiniestro + @"\Tenencia").ToString());
                    }

                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se encontró que el archivo " + System.IO.Path.GetFileNameWithoutExtension(documento) + " es una Tenencia");
                    File.Move(documento, System.IO.Path.Combine((nombreDirectorioSiniestro + @"\Tenencia"), System.IO.Path.GetFileName(documento)));
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se movió el documento " + System.IO.Path.GetFileNameWithoutExtension(documento) + " al directorio de Tenencia");
                }
                // No pudo clasificar
                else
                {
                    //MessageBox.Show(Path.GetFileNameWithoutExtension(documento) + " No se pudo clasificar el archivo " + categoria);

                    if (!Directory.Exists(nombreDirectorioSiniestro + @"\No Clasificado"))
                    {
                        Directory.CreateDirectory(nombreDirectorioSiniestro + @"\No Clasificado");
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se crea el directorio: " + (nombreDirectorioSiniestro + @"\No Clasificado").ToString());
                    }
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "No se pudo clasificar el archivo " + System.IO.Path.GetFileNameWithoutExtension(documento));
                    File.Move(documento, System.IO.Path.Combine((nombreDirectorioSiniestro + @"\No Clasificado"), System.IO.Path.GetFileName(documento)));
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se movió el documento " + System.IO.Path.GetFileNameWithoutExtension(documento) + " al directorio de No Clasificado");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Excepción " + ex);
            }
        }

        //-------OHA-------
        public DataTable initDTFacturas()
        {
            //-------- DATATABLE -----------
            Facturas = new DataTable();
            Facturas.Clear();
            Facturas.Columns.Add("ID", typeof(int));
            Facturas.Columns.Add("NO SINIESTRO", typeof(string));
            Facturas.Columns.Add("RFC EMISOR", typeof(string));//0
            Facturas.Columns.Add("SERIE", typeof(string));//0
            Facturas.Columns.Add("FOLIO", typeof(string));//1
            Facturas.Columns.Add("NO APROBACION", typeof(string));//1
            Facturas.Columns.Add("AÑO", typeof(string));//2
            Facturas.Columns.Add("CERTIFICADO", typeof(string));//2
            Facturas.Columns.Add("RUTA PDF", typeof(string));//2
            //Facturas.Columns.Add("RFC EMISOR", typeof(string));
            Facturas.Columns.Add("RFCRECEPTOR", typeof(string));
            Facturas.Columns.Add("FOLIOFISCAL", typeof(string));
            Facturas.Columns.Add("ESTATUS SAT", typeof(string));
            Facturas.Columns.Add("ANTECEDENTE",typeof(string));
            Facturas.Columns.Add("EXITO", typeof(bool));
            return Facturas;

        }


        private void initDTPolizas6Col()
        {
            //-------- DATATABLE -----------
            Polizas6Col = new DataTable();
            Polizas6Col.Clear();
            Polizas6Col.Columns.Add("NPOLIZA", typeof(string));//0
            Polizas6Col.Columns.Add("NO SINIESTRO", typeof(string));
            Polizas6Col.Columns.Add("ENDOSO", typeof(string));//0
            Polizas6Col.Columns.Add("INCISO", typeof(string));//0
            Polizas6Col.Columns.Add("SERIE", typeof(string));//1
            Polizas6Col.Columns.Add("MODELO", typeof(string));//1
            Polizas6Col.Columns.Add("MOTOR", typeof(string));//2
            Polizas6Col.Columns.Add("PLACAS", typeof(string));//2   
            Polizas6Col.Columns.Add("VIGENCIA", typeof(string));//2   
            Polizas6Col.Columns.Add("D.M.", typeof(string));
            Polizas6Col.Columns.Add("R.T.", typeof(string));

            //   Polizas6Col.Columns.Add("RUTA", typeof(string));//2
            //Facturas.Columns.Add("IMPORTE", typeof(string));//2

        }

        private DataTable leerFactura6Col(string cadArchivoLeido, string rutaArchivo, int pos, string siniestro)
        {
            //initDTFacturas();

            string input = cadArchivoLeido;

            input = Regex.Replace(input, @"\s+", "|");//reemplaza tabulador con |

            nufox fox = new nufox();

            //serie = serie.Substring(0, 15);

            //------------------SERIE MOTOR---------------------
            serie = fox.StrExtract(input, "Chasis|", "|", 1, 1);//4JGC...
            if (serie.Equals("") | serie.Length < 8)
            {

                //serie = fox.StrExtract(input, "serie:", "|", 1, 1);
                serie = fox.StrExtract(input, "SER~'", "|", 1, 1);//11BC1A...
                if (serie.Equals("") | serie.Length < 17)
                {
                    serie = fox.StrExtract(input, "SERIE|", "|", 1, 1);
                    if (serie.Equals("") | serie.Length < 17)
                    {
                        serie = fox.StrExtract(input, "SERIE:|", "|", 1, 1);
                        if (serie.Equals("") | serie.Length < 17)
                        {
                            serie = fox.StrExtract(input, "Número|de|Serie:", "|", 1, 1);
                            if (serie.Equals("") | serie.Length < 17)
                            {
                                serie = fox.StrExtract(input, "Serie|carro", "|", 1, 1);
                                if (serie.Equals("") | serie.Length < 17)
                                {
                                    serie = fox.StrExtract(input, "chasis:", "|", 1, 1);
                                    if (serie.Equals("") | serie.Length < 17)
                                    {
                                        serie = fox.StrExtract(input, "chasis", "|", 1, 1);
                                        if (serie.Equals("") | serie.Length < 17)
                                        {
                                            serie = fox.StrExtract(input, "CHASIS", "|", 1, 1);
                                            if (serie.Equals("") | serie.Length < 17)
                                            {
                                                serie = fox.StrExtract(input, "CHASIS:", "|", 1, 1);
                                                if (serie.Equals("") | serie.Length < 17)
                                                {
                                                    serie = fox.StrExtract(input, "Serie|carro|:", "|", 1, 1);
                                                    if (serie.Equals("") | serie.Length < 17)
                                                    {
                                                        serie = fox.StrExtract(input, "CHASIS|AÑo|:'0\\|•••.....|", "|", 1, 1);//para la 259
                                                        if (serie.Equals("") | serie.Length < 17)
                                                        {



                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }

            serie = serie.Replace("|", "").Replace(":", "").Replace("!", "");

            Console.WriteLine("serie trae:" + serie);

            //------------------FOLIO---------------------
            folio = fox.StrExtract(input, "folio", "|", 1, 1);
            if (folio.Equals("") | folio.Length == 1)
            {
                folio = fox.StrExtract(input, "folio:", "|", 1, 1);
                if (folio.Equals("") | folio.Length == 1)
                {
                    folio = fox.StrExtract(input, "FOLIO", "|", 1, 1);
                    if (folio.Equals("") | folio.Length == 1)
                    {
                        folio = fox.StrExtract(input, "FOLIO:", "|", 1, 1);
                        if (folio.Equals("") | folio.Length == 1)
                        {
                            folio = fox.StrExtract(input, "Número de Folio:", "|", 1, 1);
                            if (folio.Equals("") | folio.Length == 1)
                            {
                                Match m2 = Regex.Match(input, @"[a-zA-Z-0-9- ]{3,4}(\d{6})((\D|\d){2,4})");//FOLIO
                                folio = m2.Value;
                                if (m2.Success)
                                {
                                    Console.WriteLine("valor regexp m :" + m2.Value);
                                }
                                else
                                {
                                    Console.WriteLine("no se encontro valor que coincida con valor de regexp en m");
                                }
                            }
                        }
                    }
                }
            }

            folio = folio.Replace("|", "");

            Console.WriteLine("folio trae:" + folio);

            //------------------RFC---------------------
            rfc = fox.StrExtract(input, "rfc", "|", 1, 1);
            if (rfc.Equals("") | rfc.Length < 6)
            {
                rfc = fox.StrExtract(input, "rfc:", "|", 1, 1);
                if (rfc.Equals("") | rfc.Length < 6)
                {
                    rfc = fox.StrExtract(input, "RFC", "|", 1, 1);
                    if (rfc.Equals("") | rfc.Length < 6)
                    {
                        rfc = fox.StrExtract(input, "RFC:", "|", 1, 1);
                        if (rfc.Equals("") | rfc.Length < 6)
                        {
                            rfc = fox.StrExtract(input, "R.F.C.", "|", 1, 1);
                            if (rfc.Equals("") | rfc.Length < 6)
                            {


                                rfc = fox.StrExtract(input, "R.F.C.:", "|", 1, 1);
                                if (rfc.Equals("") | rfc.Length < 6)
                                {

                                    Match m2 = Regex.Match(input, @"[a-zA-Z-0-9- ]{3,4}(\d{6})((\D|\d){2,4})");//RFC
                                    rfc = m2.Value;
                                    if (m2.Success)
                                    {
                                        Console.WriteLine("valor regexp m :" + m2.Value);
                                    }
                                    else
                                    {
                                        Console.WriteLine("no se encontro valor que coincida con valor de regexp en m");
                                    }

                                }




                            }
                        }
                    }
                }
            }

            rfc = rfc.Replace("|", "");

            Console.WriteLine("rfc trae:" + rfc);

            //-----------------RFC Emisor, RFC receptor, Serie


            List<string> DatosSat2 = ObtenerDatosSat2(cadArchivoLeido);



            //------------------FECHA---------------------
            fecha = fox.StrExtract(input, "fecha", "|", 1, 1);
            if (fecha.Equals("") | fecha.Length > 10 | fecha.Length < 4)
            {
                fecha = fox.StrExtract(input, "fecha:", "|", 1, 1);
                if (fecha.Equals("") | fecha.Length > 10 | fecha.Length < 4)
                {
                    fecha = fox.StrExtract(input, "FECHA", "|", 1, 1);
                    if (fecha.Equals("") | fecha.Length > 10 | fecha.Length < 4)
                    {
                        fecha = fox.StrExtract(input, "FECHA:", "|", 1, 1);
                        if (fecha.Equals("") | fecha.Length > 10 | fecha.Length < 4)
                        {
                            fecha = fox.StrExtract(input, "Fecha de Vencimiento", "|", 1, 1);
                            if (fecha.Equals("") | fecha.Length > 10 | fecha.Length < 4)
                            {

                                Match m2 = Regex.Match(input, @"\d{2}(\/|\-|\s)(\d{2}|[a-zA-Z]{3})(\/|\-|\s)\d{2,4}");//FECHA POLIZAS
                                fecha = m2.Value;
                                if (m2.Success)
                                {
                                    Console.WriteLine("valor regexp m :" + m2.Value);
                                }
                                else
                                {
                                    Console.WriteLine("no se encontro valor que coincida con valor de regexp en m");
                                }
                            }
                        }
                    }
                }
            }

            fecha = fecha.Replace("|", "");

            Console.WriteLine("fecha trae:" + fecha);

            //------------------FACTURA---------------------
            factura = fox.StrExtract(input, "Clzve|0", "|", 1, 2);//1787
            if (factura.Equals("") | factura.Length == 1)
            {
                factura = fox.StrExtract(input, "Folio:", "|", 1, 1);//PT-45052
                if (factura.Equals("") | factura.Length == 1)
                {

                    factura = fox.StrExtract(input, "RENAULT|.|INVENTARIO|", "|", 1, 1);//4338


                    if (factura.Equals("") | factura.Length == 1)
                    {
                        factura = fox.StrExtract(input, "Factura :", "|", 1, 1);
                        if (factura.Equals("") | factura.Length == 1)
                        {
                            factura = fox.StrExtract(input, "CONTROL|INTERNO:|", "|", 1, 1);//EAN000001118

                            if (factura.Equals("") | factura.Length == 1)
                            {

                                Match m2 = Regex.Match(input, @"[a-zA-Z]{3}[0]{5}?[\||\-|\s][0-9]{4,5}");//FACTURA
                                factura = m2.Value;
                                if (m2.Success)
                                {
                                    Console.WriteLine("valor regexp m :" + m2.Value);
                                }
                                else
                                {
                                    Console.WriteLine("no se encontro valor que coincida con valor de regexp en m");
                                }
                            }
                        }
                    }
                }
            }

            factura = factura.Replace("|", "");

            Console.WriteLine("factura trae:" + factura);

            //--------------SERIE------------------------

            Match m = Regex.Match(input, @"[0-9]*[A-Z]{1,3}[A-Z0-9]{12,17}[0-9]{1,3}");//SERIE
            serie2 = m.Value;
            if (m.Success)
            {
                Console.WriteLine("valor regexp m :" + m.Value);
            }
            else
            {
                Console.WriteLine("no se encontro valor que coincida con valor de regexp en m");
            }


            //--------------FOLIO------------------------

            m = Regex.Match(input, @"[a-zA-Z-0-9- ]{3,4}(\d{6})((\D|\d){2,4})");//FOLIO
            folio2 = m.Value;
            if (m.Success)
            {

                Console.WriteLine("valor regexp m :" + m.Value);
            }
            else
            {
                Console.WriteLine("no se encontro valor que coincida con valor de regexp en m");
            }

            m = Regex.Match(input, @"[a-zA-Z-0-9- ]{3,4}(\d{6})((\D|\d){2,4})");//RFC origibnal 5coincidencias

            rfc2 = m.Value;
            rfc2 = rfc2.Replace("|", "").Replace(":", "").Replace("!", "");
            if (m.Success)
            {
                // ... Write value.
                Console.WriteLine("valor regexp m :" + m.Value);
            }
            else
            {
                Console.WriteLine("no se encontro valor que coincida con valor de regexp en m");
            }




            //--------------FECHA------------------------
            // Regex regex = new Regex(@"\d{2}/\d{2}/\d{2,4}");//FECHA bien
            //Regex regex = new Regex(@"\d{2}(\/|\-)\d{2}(\/|\-)\d{2,4}");//FECHA mal
            //Regex regex = new Regex(@"[0-3]\d{1}(\/|\s|\-)[0-1]\d{1}(\/|\s|\-)[1-2]?([9]?|[0]?)\d{2,3}");//FECHA mal

            //m = Regex.Match(input, @"\d{2}(\/|\-)\d{2}(\/|\-)\d{2,4}");//FECHA
            m = Regex.Match(input, @"\d{2}(\/|\-)(\d{2}|[a-zA-Z]{3})(\/|\-)\d{2,4}");//FECHA OK PARA POLIZAS
            fecha2 = m.Value;
            if (m.Success)
            {
                // ... Write value.
                Console.WriteLine("valor regexp m :" + m.Value);
            }
            else
            {
                m = Regex.Match(input, @"\d{2,4}(\/|\-)\d{2}(\/|\-)\d{2}");
                fecha2 = m.Value;
                if (m.Success)
                {
                    Console.WriteLine("valor regexp m :" + m.Value);
                }
                else
                {
                    Console.WriteLine("no se encontro valor que coincida con valor de regexp en m");
                }
            }




            //Regex regex = new Regex(@"^[a-zA-Z]{2,3}[-]?[0-9]+$");//Factura
            //Regex regex = new Regex(@"[a-zA-Z]{2,3}[\-|\s][0-9]+");//Factura pero trae CP tambien
            //Regex regex = new Regex(@"[a-zA-Z]{2,3}[\-|\s][0-9]{3,8}");//Factura bien
            //Regex regex = new Regex(@"[[:upper:]][a-zA-Z]{2,3}[\-|\s] [0-9]{3,8}");//Factura no funciona

            m = Regex.Match(input, @"[a-zA-Z]{2,3}[\-|\s][0-9]{3,8}");//FACTURA
            factura2 = m.Value;
            if (m.Success)
            {
                // ... Write value.
                Console.WriteLine("valor regexp m :" + m.Value);
            }
            else
            {
                Console.WriteLine("no se encontro valor que coincida con valor de regexp en m");
            }

            //----------------------------------------------------
            //Facturas.Rows.Add(rfc, serie, folio, noAprobacion, anio, certificado);
            //Facturas.Rows.Add(rfc2, factura, factura, rfc, serie, folio);
            //Facturas.Rows.Add(rfc2, serie, factura, noAprobacion, fecha, certificado);
            Facturas.Rows.Add(pos, siniestro, rfc2, serie, factura, noAprobacion, fecha, certificado, rutaArchivo, DatosSat2[0], DatosSat2[1]);

            return Facturas;
        }

        public List<string> ObtenerDatosSat2(string cadArchivoLeido)
        {
            List<string> encontrados = new List<string>();
            int pos = 0;
            try
            {
                char[] param = { '\r', '\n' };
                List<string> result = new List<string>();
                MatchCollection matches = Regex.Matches(cadArchivoLeido, @"([a-zA-Z0-9]){12}\w+");
                foreach (Match item in matches)
                {
                    result.Add(item.Value);
                }
                string[] arregloResultado = cadArchivoLeido.Split(param);
                int rfcEmisor = nu4it.UbicadoEnPos(arregloResultado, "RFC", 0);
                int rfcReceptor = nu4it.OcurrenciasEnArreglo(arregloResultado, "RFC", 1);
                //int intSerie = nu4it.UbicadoEnPos(arregloResultado, "SERIE", 0);

                if (rfcReceptor > 1)
                {
                    for (int i = 0; i < rfcReceptor; i++)
                    {
                        pos = nu4it.UbicadoEnPos(arregloResultado, "RFC", 0);
                    }
                    rfcReceptor = pos;
                }

                string RFCEMISOR = Regex.Match(arregloResultado[rfcEmisor], @"([a-zA-Z0-9]){12}\w+").Value;
                encontrados.Add(RFCEMISOR);
                string RFCRECEPTOR = Regex.Match(arregloResultado[rfcReceptor], @"([a-zA-Z0-9]){12}\w+").Value;
                encontrados.Add(RFCRECEPTOR);
                //string SERIE = Regex.Match(arregloResultado[intSerie], @"([a-zA-Z0-9]){12}\w+").Value;
                //encontrados.Add(SERIE);

            }
            catch (Exception ex)
            {
                if (encontrados.Count < 2)
                {
                    int veces = 2 - encontrados.Count;
                    for (int i = 0; i < veces; i++)
                    {
                        encontrados.Add("");
                    }
                }
                return encontrados;
            }

            return encontrados;
        }

        private DataTable leerPoliza(string cadArchivoLeido, string rutaArchivo, string siniestro)
        {
            string input = cadArchivoLeido;

            input = Regex.Replace(input, @"\s+", "|");//remplaza tabulador con |


            nufox fox = new nufox();

            string cadena = fox.StrExtract(input, "INCISO", "R.F.C.", 1);
            if (!string.IsNullOrEmpty(cadena))
            {
                poliza = cadena.Substring(1, 10);
                endoso = cadena.Substring(12, 6);
                inciso = cadena.Substring(19, 4);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se extrajo poliza: " + poliza + " endoso: " + endoso + " inciso:" + inciso);
            }
            else
            {
                poliza = "";
                endoso = "";
                inciso = "";

            }



            //------------------SERIE---------------------
            serie = fox.StrExtract(input, "serie", "|", 1, 1);
            if (serie.Equals("") | serie.Length == 1)
            {
                serie = fox.StrExtract(input, "serie:", "|", 1, 1);
                if (serie.Equals("") | serie.Length == 1)
                {
                    serie = fox.StrExtract(input, "SERIE", "|", 1, 1);
                    if (serie.Equals("") | serie.Length == 1)
                    {
                        serie = fox.StrExtract(input, "SERIE:", "|", 1, 1);
                        if (serie.Equals("") | serie.Length == 1)
                        {
                            serie = fox.StrExtract(input, "Número de Serie:", "|", 1, 1);
                        }

                        else
                        {
                            objNu4.ReportarLog(VARSTR_BITACORA_LOG, "No se pudo extraer serie");
                            serie = "";
                        }
                    }
                }
            }


            serie = serie.Replace("|", "");
            objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se extrajo serie: " + serie);

            //------------------MODELO---------------------
            modelo = fox.StrExtract(input, "modelo", "|", 1, 1);
            if (modelo.Equals("") | modelo.Length == 1)
            {
                modelo = fox.StrExtract(input, "modelo:", "|", 1, 1);
                if (modelo.Equals("") | modelo.Length == 1)
                {
                    modelo = fox.StrExtract(input, "MODELO", "|", 1, 1);
                    if (modelo.Equals("") | modelo.Length == 1)
                    {
                        modelo = fox.StrExtract(input, "MODELO:", "|", 1, 1);
                    }
                    else
                    {
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "No se extrajo modelo");
                        modelo = "";

                    }
                }
            }



            modelo = modelo.Replace("|", "");
            objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se extrajo modelo: " + modelo);

            //------------------motor---------------------
            motor = fox.StrExtract(input, "motor", "|", 1, 1);
            if (motor.Equals("") | motor.Length == 1)
            {
                motor = fox.StrExtract(input, "motor:", "|", 1, 1);
                if (motor.Equals("") | motor.Length == 1)
                {
                    motor = fox.StrExtract(input, "MOTOR", "|", 1, 1);
                    if (motor.Equals("") | motor.Length == 1)
                    {
                        motor = fox.StrExtract(input, "MOTOR:", "|", 1, 1);
                        if (motor.Equals("") | motor.Length == 1)
                        {
                            motor = fox.StrExtract(input, "MOTOR", "|", 1, 1);

                        }
                        else
                        {
                            objNu4.ReportarLog(VARSTR_BITACORA_LOG, "No se extrajo motor ");
                            motor = "";
                        }

                    }

                }
            }




            motor = motor.Replace("|", "");

            objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se extrajo motor: " + motor);





            //------------------PLACAS---------------------
            placas = fox.StrExtract(input, "placas", "|", 1, 1);
            if (placas.Equals("") | placas.Length == 1)
            {
                placas = fox.StrExtract(input, "placas:", "|", 1, 1);
                if (placas.Equals("") | placas.Length == 1)
                {
                    placas = fox.StrExtract(input, "PLACAS", "|", 1, 1);
                    if (placas.Equals("") | placas.Length == 1)
                    {
                        placas = fox.StrExtract(input, "PLACAS:", "|", 1, 1);
                        if (placas.Equals("") | placas.Length == 1)
                        {

                        }
                        else
                        {
                            objNu4.ReportarLog(VARSTR_BITACORA_LOG, "No se pudo extraer placas");
                            placas = "";
                        }
                    }
                }
            }



            placas = placas.Replace("|", "");


            //------------------VIGENCIA---------------------
            cadena = fox.StrExtract(input, "DESDE", "TOTAL", 1, 1);
            string v1, v2;
            if (!string.IsNullOrEmpty(cadena))
            {

                v1 = cadena;
                v1 = cadena.Substring(1, 31);
                v1 = v1.Replace("|", " ");
                string cadena2 = fox.StrExtract(input, "HASTA", "TOTAL", 1, 1);
                if (!string.IsNullOrEmpty(cadena2))
                {
                    v2 = cadena2.Substring(1, 31);
                    v2 = v2.Replace("|", " ");
                }
                else v2 = "";
                vigencia = "Desde: " + v1 + " Hasta: " + v2;
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se extrajo vigencia: " + vigencia);
            }
            else
            {
                vigencia = "";

            }

            //------------------DM---------------------
            cadena = fox.StrExtract(input, "SUMA", "%", 1);
            if (!string.IsNullOrEmpty(cadena))
            {
                string dm;
                dm = cadena.Substring(cadena.Length - 2);

                DM = dm.Replace("|", " ");
                DM = DM + "%";
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se extrajo DM: " + DM);
            }
            else
            {
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "No se pudo extraer DM");
                DM = "";

            }
            string rt = fox.StrExtract(input, "%", "%");
            if (!string.IsNullOrEmpty(cadena))
            {

                rt = cadena.Substring(cadena.Length - 2);

                RT = rt.Replace("|", " ");
                RT = RT + "%";
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se extrajo RT: " + RT);
            }
            else
            {
                RT = "";
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "N o se pudo extraer RT ");

            }

            Polizas6Col.Rows.Add(poliza, siniestro, endoso, inciso, serie, modelo, motor, placas, vigencia, DM, RT);

            return Polizas6Col;
        }
    }
}
