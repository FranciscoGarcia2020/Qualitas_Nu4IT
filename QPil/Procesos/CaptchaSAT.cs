using Nu4it;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QPil.Procesos
{
    class CaptchaSAT
    {
        AutomatizacionOCR objOCR = new AutomatizacionOCR();
        usaR objNu4 = new usaR(); //Objeto para usar funciones generales
        public static string VARSTR_BITACORA_LOG;
        string[] CONTENIDOINI;
        string id, no_siniestro, rfc, serie, folio, noAprobacion, anio, certificado, rutaPDF;
        public static DataTable DT_DATOS_SAT { get; set; }
        string mensaje = "";
        string NoSerie, EstadoC, coincideSC = "", estatus = "";
        public static DataTable DTSat;
        public static bool ValidacionBoton { get; set; }

        public CaptchaSAT(string RutaLog)
        {
            VARSTR_BITACORA_LOG = RutaLog;
            //CONTENIDOINI = objNu4.LeerArchivoIni("OCR");
        }
        public void recibeDatatableSAT(DataTable tabla)
        {
            //TABLA = tabla;

            foreach (DataRow fila in tabla.Rows)
            {
                ProcesoCaptchaSat(fila);
            }

        }
        public void recibeDatatableSAT(DataRow fila)
        {
            ProcesoCaptchaSat(fila);
        }
       
        public bool ProcesoCaptchaSat(DataRow datos)
        {
            bool Exito = true;
            try
            {


                id = datos["ID"].ToString();//"AUT091023H43";
                no_siniestro = datos["NO SINIESTRO"].ToString();
                rfc = datos["RFC EMISOR"].ToString();//"AUT091023H43";
                serie = datos["SERIE"].ToString(); //"E";
                folio = datos["FOLIO"].ToString();//"0000897";
                noAprobacion = datos["NO APROBACION"].ToString();//"424694";
                anio = datos["AÑO"].ToString(); //"2011";
                certificado = datos["CERTIFICADO"].ToString();//"00001000000202409238";
                rutaPDF = datos["RUTA PDF"].ToString();

                int posUrl;
                string URL;
                IWebDriver driver;
                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                driver = new ChromeDriver(driverService, new ChromeOptions());
                WebDriverWait waitent = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                WebDriverWait waitL = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                //posUrl = objNu4.UbicadoEnPos(CONTENIDOINI, "URLSATOCR=", 0);
                //URL = CONTENIDOINI[posUrl];
                URL = "https://tramitesdigitales.sat.gob.mx/Sicofi.ValidacionCFD/Default.aspx";// URL.Replace("URLSATOCR=", "");
                driver.Navigate().GoToUrl(URL);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se navega a la URL: " + URL);

                //string inputT = driver.FindElement(By.Id("__PREVIOUSPAGE")).GetAttribute("value");
                //string input2T = driver.FindElement(By.Id("__EVENTVALIDATION")).GetAttribute("value");
                //string scripts = driver.FindElement(By.XPath("//*[@id=\"form1\"]/script[2]")).GetAttribute("src");
                //string scripts2 = driver.FindElement(By.XPath("//*[@id=\"form1\"]/script[3]")).GetAttribute("src");
                //string src = driver.FindElement(By.XPath("//*[@id=\"divCaptcha\"]/div/div[1]/img")).GetAttribute("src");
                //string value = driver.FindElement(By.Id("__VIEWSTATE")).GetAttribute("value");
                //Strings que se utilizan para obtener los Atributos de los elementos a reemplazar con los scripts


                string input = "arguments[0].setAttribute('value', arguments[1]);";
                var objetivoSI = driver.FindElement(By.Id("__PREVIOUSPAGE"));
                ((IJavaScriptExecutor)driver).ExecuteScript(input, objetivoSI, "kw1v1Xc7eFOxV6y6DdhFnU1lP8ugRMpXyWWGq8ADb6IlehKMuIc8PF1mWeK1v-6bMs8JRFXUYb5tb8hJREXfxofuLW6GTEYUq53IOLkF2HBZXbHxy9sJqplkQdP-h12rZdRH_Q2");

                string input2 = "arguments[0].setAttribute('value', arguments[1]);";
                var objetivoI2 = driver.FindElement(By.Id("__EVENTVALIDATION"));
                ((IJavaScriptExecutor)driver).ExecuteScript(input2, objetivoI2, "/wEWAwKL157oAQKYvZuABQKEwKXjCJ/4ubs7SdS44Du68Pu6dVXwgyky");
                string script = "arguments[0].setAttribute('value', arguments[1]);";
                var objetivo = driver.FindElement(By.Id("__VIEWSTATE"));
                ((IJavaScriptExecutor)driver).ExecuteScript(script, objetivo, "/wEPDwUKMTQ1NjE4NzczNw9kFgICAw9kFgICAQ8FJDNhZjVhOGYzLTVhZTgtNDIzZS05ZDNjLWJhNWZjYjRkNTlkZmQYAgUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFD2ltZ0J0bkFjdHVhbGl6YQUPQ2FwdGNoYUNvbnRyb2wxDxQrAAEy9woAAQAAAP////8BAAAAAAAAAAwCAAAATUVkcy5Bc2YuQ2FwdGNoYUNvbnRyb2wsIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsBQEAAAAjRWRzLkFzZi5DYXB0Y2hhQ29udHJvbC5DYXB0Y2hhSW1hZ2UNAAAAB19oZWlnaHQGX3dpZHRoBV9yYW5kDF9nZW5lcmF0ZWRBdAtfcmFuZG9tVGV4dBFfcmFuZG9tVGV4dExlbmd0aBBfcmFuZG9tVGV4dENoYXJzD19mb250RmFtaWx5TmFtZQlfZm9udFdhcnAQX2JhY2tncm91bmROb2lzZQpfbGluZU5vaXNlBV9ndWlkDl9mb250V2hpdGVsaXN0AAADAAEAAQEEBAQBAQgIDVN5c3RlbS5SYW5kb20NCDJFZHMuQXNmLkNhcHRjaGFDb250cm9sLkNhcHRjaGFJbWFnZStGb250V2FycEZhY3RvcgIAAAA4RWRzLkFzZi5DYXB0Y2hhQ29udHJvbC5DYXB0Y2hhSW1hZ2UrQmFja2dyb3VuZE5vaXNlTGV2ZWwCAAAAMkVkcy5Bc2YuQ2FwdGNoYUNvbnRyb2wuQ2FwdGNoYUltYWdlK0xpbmVOb2lzZUxldmVsAgAAAAIAAAAyAAAAtAAAAAkDAAAAHlkC8Kl71ogGBAAAAAV5VjdZZgUAAAAGBQAAAB5BQkNERUZHSEpLTU5QUVJTVFVWV1hZWjIzNDY3ODkGBgAAAAAF+f///zJFZHMuQXNmLkNhcHRjaGFDb250cm9sLkNhcHRjaGFJbWFnZStGb250V2FycEZhY3RvcgEAAAAHdmFsdWVfXwAIAgAAAAEAAAAF+P///zhFZHMuQXNmLkNhcHRjaGFDb250cm9sLkNhcHRjaGFJbWFnZStCYWNrZ3JvdW5kTm9pc2VMZXZlbAEAAAAHdmFsdWVfXwAIAgAAAAEAAAAF9////zJFZHMuQXNmLkNhcHRjaGFDb250cm9sLkNhcHRjaGFJbWFnZStMaW5lTm9pc2VMZXZlbAEAAAAHdmFsdWVfXwAIAgAAAAAAAAAGCgAAACQzYWY1YThmMy01YWU4LTQyM2UtOWQzYy1iYTVmY2I0ZDU5ZGYGCwAAAOEBYXJpYWw7YXJpYWwgYmxhY2s7Y29taWMgc2FucyBtcztjb3VyaWVyIG5ldztlc3RyYW5nZWxvIGVkZXNzYTtmcmFua2xpbiBnb3RoaWMgbWVkaXVtO2dlb3JnaWE7bHVjaWRhIGNvbnNvbGU7bHVjaWRhIHNhbnMgdW5pY29kZTttYW5nYWw7bWljcm9zb2Z0IHNhbnMgc2VyaWY7cGFsYXRpbm8gbGlub3R5cGU7c3lsZmFlbjt0YWhvbWE7dGltZXMgbmV3IHJvbWFuO3RyZWJ1Y2hldCBtczt2ZXJkYW5hBAMAAAANU3lzdGVtLlJhbmRvbQMAAAAFaW5leHQGaW5leHRwCVNlZWRBcnJheQAABwgICAwAAAAhAAAACQwAAAAPDAAAADgAAAAIAAAAAI8EonfHwlF0+/ohJ8Mz9kQJMIBRdK34aRbnTC1kptpfCzJ7U2zGJHsDVYJebuCcGMUjowkM6JUg5cDnJUvyTFjV4Pg58/AqaiAVtXmAunFukNUBQXX+ThoPhBQ5L1MJEorjGFt95jJN266rfjKJqCihTK1cbCQnVDG9mBhoNeVbPBs7N62CEzU8gJJbVHO5aMH8wW/zusBGCgv2Von68wh2tsl3r+zVXW8uH2fXO8YX7iZ2ZQNEPBvjHqkB+PG7M65f21o9ljI84pxGagPS4R0P7ZBVcgSmSIgCNV4LZBu7xuFDj63cJ7En+O5ftvJ4TUES");

                string script2 = "arguments[0].setAttribute('src', arguments[1]);";
                var objetivo2 = driver.FindElement(By.XPath("//*[@id=\"divCaptcha\"]/div/div[1]/img"));
                ((IJavaScriptExecutor)driver).ExecuteScript(script2, objetivo2, "https://tramiteLsdigitales.sat.gob.mx/Sicofi.ValidacionCFD/CaptchaImage.aspx?guid=3af5a8f3-5ae8-423e-9d3c-ba5fcb4d59df&s=1");


                IWebElement elemento = waitent.Until(ExpectedConditions.ElementExists(By.Name("CaptchaControl1")));
                //Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                //ss.SaveAsFile("file_name_string.Png", ScreenshotImageFormat.Png);
                //string valorcaptcha = objOCR.ocrRectanguloProcesado(Directory.GetCurrentDirectory() + @"\file_name_string.Png", 383, 412, 171, 45);
                string valorcaptcha = "yV7Yf";
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se toma screenshot y se procesar CAPTCHA con valor: " + valorcaptcha);
                elemento.SendKeys(valorcaptcha);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se digita el captcha con valor: " + valorcaptcha);
                elemento = waitent.Until(ExpectedConditions.ElementExists(By.Id("btnContinuar")));
                elemento.Click();
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se manda click al elemento Continuar");

                try
                {

                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_ContentPlaceHolder1_txtRFC")));
                    elemento.SendKeys(rfc);
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se digita el RFC del emisor: " + rfc);
                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_ContentPlaceHolder1_txtSerie")));
                    elemento.SendKeys(serie);
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se digita la serie: " + serie);
                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_ContentPlaceHolder1_txtFolio")));
                    elemento.SendKeys(folio);
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se digita el Folio del Comprobante: " + folio);
                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_ContentPlaceHolder1_txtNoAprobacion")));
                    elemento.SendKeys(noAprobacion);
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se digita el Número de Aprobación: " + noAprobacion);
                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_ContentPlaceHolder1_txtAnioAprobacion")));
                    elemento.SendKeys(anio);
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se digita el Año de Aprobación: " + anio);
                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_ContentPlaceHolder1_btnConsultar")));
                    elemento.Click();
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se manda click al botón Consultar");

                    bool alerta = isAlertPresent(driver);
                    try
                    {
                        elemento = driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_lblAviso"));
                        mensaje = elemento.Text;
                    }
                    catch (Exception)
                    {

                    }


                    int Validacion = 0;
                    if (alerta == true)//case1
                    {
                        Validacion = 1;
                    }
                    else if (mensaje == "El folio verificado no fue asignado por el Servicio de Administración Tributaria")
                    {
                        Validacion = 2;
                    }

                    switch (Validacion)
                    {
                        case 1:
                            DialogResult desicion = MessageBox.Show(new Form { TopMost = true }, "¿Desea editar?", "Los datos ingresados son invalidos", MessageBoxButtons.YesNo);
                            switch (desicion)
                            {
                                case DialogResult.Yes:

                                    driver.Close();
                                    Forms.ValidaDatosSat form = new Forms.ValidaDatosSat(VARSTR_BITACORA_LOG, rutaPDF, rfc, serie, folio, noAprobacion, anio, certificado, id, driver);
                                    form.ShowDialog();
                                    if (ValidacionBoton)
                                    {
                                        Exito = false;
                                        DataRow[] row = Procesos.ManejoDeDocumentos.DT_DATOS_SAT.Select("ID ='" + id + "'");
                                        for (int j = 0; j < row.Length; j++)
                                        {
                                            row[j]["EXITO"] = false;
                                            row[j].AcceptChanges();
                                        }
                                    }
                                    else
                                        Exito = true;
                                    break;
                                case DialogResult.No:
                                    driver.Close();
                                    DataRow[] row2 = Procesos.ManejoDeDocumentos.DT_DATOS_SAT.Select("ID ='" + id + "'");
                                    for (int j = 0; j < row2.Length; j++)
                                    {
                                        row2[j]["EXITO"] = false;
                                        row2[j].AcceptChanges();
                                    }
                                    break;
                            }
                            break;

                        case 2:
                            DialogResult desicion2 = MessageBox.Show("¿Desea editar?", "Los datos ingresados son invalidos", MessageBoxButtons.YesNo);
                            switch (desicion2)
                            {
                                case DialogResult.Yes:

                                    driver.Close();
                                    Forms.ValidaDatosSat form = new Forms.ValidaDatosSat(VARSTR_BITACORA_LOG, rutaPDF, rfc, serie, folio, noAprobacion, anio, certificado, id, driver);
                                    form.ShowDialog();
                                    if (ValidacionBoton)
                                    {
                                        Exito = false;
                                        DataRow[] row = Procesos.ManejoDeDocumentos.DT_DATOS_SAT.Select("ID ='" + id + "'");
                                        for (int j = 0; j < row.Length; j++)
                                        {
                                            row[j]["EXITO"] = false;
                                            row[j].AcceptChanges();
                                        }
                                    }
                                    else
                                        Exito = true;
                                    break;
                                case DialogResult.No:
                                    DataRow[] row2 = Procesos.ManejoDeDocumentos.DT_DATOS_SAT.Select("ID ='" + id + "'");
                                    for (int j = 0; j < row2.Length; j++)
                                    {
                                        row2[j]["EXITO"] = false;
                                        row2[j].AcceptChanges();
                                    }
                                    driver.Close();
                                    Exito = false;
                                    break;
                            }
                            break;
                    }
                    if (Validacion == 0)
                    {
                        IWebElement div = driver.FindElement(By.Id("ctl00_ContentPlaceHolder1_divExistenResultados"));
                        IWebElement tabla = div.FindElement(By.Id("ctl00_ContentPlaceHolder1_gvCSD"));
                        var coleccion_tr = tabla.FindElements(By.TagName("tr"));
                        var coleccion_td = tabla.FindElements(By.TagName("td"));
                        int NoTD = coleccion_td.Count();
                        int i = 0;
                        bool coincide = false;
                        CrearTablaSat();
                        foreach (var td in coleccion_td)
                        {


                            if (i == 0)
                            {
                                NoSerie = td.Text;
                                if (NoSerie == certificado)
                                {
                                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Coincide " + NoSerie);
                                    coincideSC = NoSerie;
                                    //  MessageBox.Show("Coincide " + NoSerie);
                                    coincide = true;
                                }
                            }
                            if (i == 4)
                            {
                                if (coincide == true)
                                {
                                    EstadoC = td.Text;
                                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Estatus: " + EstadoC);
                                    estatus = EstadoC;
                                    //  MessageBox.Show("Estatus " + EstadoC);
                                }

                                i = -1;
                            }
                            i++;
                        }
                        Exito = true;
                        driver.Close();
                        DTSat.Rows.Add(no_siniestro, coincideSC, EstadoC);
                        DataRow[] row = Procesos.ManejoDeDocumentos.DT_DATOS_SAT.Select("ID ='" + id + "'");
                        for (int j = 0; j < row.Length; j++)
                        {
                            row[j]["EXITO"] = true;
                            row[j].AcceptChanges();
                        }
                    }
                }
                catch (OpenQA.Selenium.WebDriverTimeoutException e)
                {
                    driver.Close();
                    ProcesoCaptchaSat(datos);
                    Exito = false;
                }
                catch (OpenQA.Selenium.UnhandledAlertException e)
                {
                    driver.Close();
                    ProcesoCaptchaSat(datos);
                }

            }

            catch (Exception o)
            {
                MessageBox.Show("Se encontró el siguiente error: " + o);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se encontró el siguiente error: " + o);
                Exito = false;
            }
            return Exito;
        }

        private void CrearTablaSat()
        {
            DTSat = new DataTable();
            DTSat.Clear();
            DTSat.Columns.Add("NO SINIESTRO", typeof(string));
            DTSat.Columns.Add("COINCIDE", typeof(string));
            DTSat.Columns.Add("ESTATUS", typeof(string));
        }


        public bool isAlertPresent(IWebDriver driver)
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }   // try 
            catch (NoAlertPresentException Ex)
            {
                return false;
            }   // catch 
        }


    }
}

