using System;
using Nu4it;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace QPil.Procesos
{
    class CaptchaSAT2
    {
        AutomatizacionOCR objOCR = new AutomatizacionOCR();
        usaR objNu4 = new usaR(); //Objeto para usar funciones generales
        public static string VARSTR_BITACORA_LOG;
        string[] CONTENIDOINI;
        string ESTADOCFDI = "";
        string EFECTOCOMPROBANTE = "";
        string ESCANCELABLE = "";
        string id, folioFiscal, rfcEmisor, rfcReceptor, rutaPDF, Siniestro;
        public static DataTable DT_DATOS_SAT2 { get; set; }
        public static bool ValidacionBoton { get; set; }
        IWebDriver driver;

        DataTable TABLA = new DataTable();

        public CaptchaSAT2(string RutaLog)
        {
            VARSTR_BITACORA_LOG = RutaLog;
            //CONTENIDOINI = objNu4.LeerArchivoIni("OCR");
        }
        public bool recibeDatatableSAT2(DataTable tabla, string siniestro = null)
        {
            bool res = false;
            try
            {
                Siniestro = siniestro;
                foreach (DataRow fila in tabla.Rows)
                {
                    procesoCaptchaSAT2(fila);
                }
                res = true;
                return res;
            }
            catch (Exception ex)
            {
                res = false;
                return res;
            }
        }
        public void recibeDatatableSAT2(DataRow fila, IWebDriver driver)
        {
            procesoCaptchaSAT2(fila, driver);
        }

        public bool procesoCaptchaSAT2(DataRow datos, IWebDriver Driver = null)
        {
            bool exito = true;
            id = datos["ID"].ToString();//"AUT091023H43";
            folioFiscal = datos["FOLIOFISCAL"].ToString();
            rfcEmisor = datos["RFC EMISOR"].ToString();
            rfcReceptor = datos["RFCRECEPTOR"].ToString();
            rutaPDF = datos["RUTA PDF"].ToString();
            ChromeOptions options = new ChromeOptions();
            try
            {
                int posUrl;
                string URL;
                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                options.AddUserProfilePreference("--disable-impl-side-painting", "--homepage=about:blank");
                if (Driver != null)
                {
                    driver = Driver;
                }
                else
                {
                    driver = new ChromeDriver(driverService, options);
                }
                WebDriverWait waitent = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                WebDriverWait waitL = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                //posUrl = objNu4.UbicadoEnPos(CONTENIDOINI, "URLSAT2=", 0);
                //URL = CONTENIDOINI[posUrl];
                URL = "https://verificacfdi.facturaelectronica.sat.gob.mx/";//URL.Replace("URLSAT2=", "");
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(URL);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se navega a la URL: " + URL);
                string script = "arguments[0].setAttribute('value', arguments[1]);";
                var objetivo = driver.FindElement(By.Id("__VIEWSTATE"));
                ((IJavaScriptExecutor)driver).ExecuteScript(script, objetivo, "YkwLZs2Q+Za3PLiMnRb7sn9TksN0X7rY0IPdYTceL1Jb90k006NG1S9" +
                    "Ldq1nsx83SwdmUSdQDnoZXACLNcUs/ZQCKnz8DKgEulArCRI768lXkeOgxEcRHUl+lfkEN8q1Ua/sZm5hY7T/q33PEAdiYVQ8+mQP8OtzfXm7u8sxF0x" +
                    "/yc/4ra+WnHms4PFcREMZgvabEPzpOYRd/dNy1vv876ZYLGehfpWHGzoCpzt7VLXywrAA1zYVpsNnq1mfayWs/z+KKxUFruLhYHzzu4N0oiltGgD9OmSqmT" +
                    "gqIfqRACIZEnCCfnmSZfVxjc7Un1NbnhQtJDfO/+7srHX1RE599pzeSxzGZ144sw9l3lUpOm6m2gyyC1vzxvu3AoemI5axrpsrptGpn6MtnhGDh58QfsZLM" +
                    "X8AMFWZn2DgkRhbmr/0Uemw1LcwdyHMBpcKxrE3nIIT6v5uhBhCeGCXy2llVsuaXKubpHH8zIbXTI7yuakw9PiNpsmvBOXPfnJT/Zlzq9LNuVul0tGSVqVFyZgmGLNV7D4=");

                string script2 = "arguments[0].setAttribute('src', arguments[1]);";
                var objetivo2 = driver.FindElement(By.Id("ctl00_MainContent_ImgCaptcha"));
                ((IJavaScriptExecutor)driver).ExecuteScript(script2, objetivo2, "https://verificacfdi.facturaelectronica.sat.gob.mx/GeneraCaptcha.aspx?Data=/AQlIWdTi+N5Eew846YMD+aKZl3bU/hupRo9f9glG8t/5S1WV0UuFikiNRjGL5dWp783wNBvqpLIyqh4Ek26zur+8F6x9WqbIrejTcJjAwE=");
                Thread.Sleep(2000);
                IWebElement elemento = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_MainContent_TxtUUID")));
                elemento.Click();
                elemento.SendKeys(folioFiscal);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se ingresó folio fiscal: ");
                driver.FindElement(By.Id("ctl00_MainContent_TxtRfcEmisor")).SendKeys(rfcEmisor);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se ingresó RFC de Emisor: ");
                driver.FindElement(By.Id("ctl00_MainContent_TxtRfcReceptor")).SendKeys(rfcReceptor);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se ingresó RFC de Receptor: ");

                driver.FindElement(By.Id("ctl00_MainContent_TxtCaptchaNumbers")).SendKeys("11797");
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se ingresó Captcha: ");
                driver.FindElement(By.Id("ctl00_MainContent_BtnBusqueda")).Click();
                 var encuentra = driver.FindElements(By.XPath("//*[@id=\"ctl00_MainContent_PnlNoResultados\"]/div"));
                var encuentraError = driver.FindElements(By.XPath("//*[@id=\"ctl00_MainContent_VsResumenErrores\"]"));


                //if (encuentra.Count == 1 || encuentraError.Count == 1)
                //{

                var encuentraTabla = driver.FindElements(By.Id("ctl00_MainContent_LblRfcEmisor"));
                int intentos = 0;
                do
                {

                    encuentraTabla = driver.FindElements(By.Id("ctl00_MainContent_LblRfcEmisor"));
                    intentos++;
                } while (encuentraTabla.Count == 0 && intentos < 99);

                if (encuentraTabla.Count == 0)
                {
                    DialogResult desicion = MessageBox.Show(new Form { TopMost = true }, "¿Desea editar?", "Los datos ingresados son invalidos", MessageBoxButtons.YesNo);
                    switch (desicion)
                    {
                        case DialogResult.Yes:

                            //
                            Forms.ValidaDatosSat form = new Forms.ValidaDatosSat(VARSTR_BITACORA_LOG, rutaPDF, folioFiscal, rfcEmisor, rfcReceptor, id, driver);
                            form.ShowDialog();
                            if (ValidacionBoton)
                            {
                                try
                                {
                                    driver.Close();
                                }
                                catch (Exception e)
                                {

                                   
                                }
                              
                                exito = false;
                            }
                          
                            exito = true;
                            break;
                        case DialogResult.No:
                            driver.Close();
                            exito = false;
                            break;
                    }

                }
                else
                {
                    //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0,document.body.scrollHeight)");
                    IWebElement divContenedor = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_MainContent_PnlResultados")));
                    IWebElement elemento3 = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_MainContent_LblEfectoComprobante")));
                    EFECTOCOMPROBANTE = elemento3.Text;
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El efecto de comprobante es: " + EFECTOCOMPROBANTE);
                    elemento3 = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_MainContent_LblEstado")));
                    ESTADOCFDI = elemento3.Text;
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El estado del CFDI es: " + ESTADOCFDI);
                    elemento3 = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_MainContent_LblEsCancelable")));
                    ESCANCELABLE = elemento3.Text;
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El Estatus de cancelación es: " + ESCANCELABLE);
                    //crarTablaSat2();
                    DataRow[] row = Procesos.ManejoDeDocumentos.DT_DATOS_SAT2.Select("ID ='" + id + "'");
                    for (int i = 0; i < row.Length; i++)
                    {
                        row[i]["ESTATUS SAT"] = ESTADOCFDI;
                        row[i].AcceptChanges();
                    }

                    //DT_DATOS_SAT2.Rows.Add(Siniestro, EFECTOCOMPROBANTE, ESTADOCFDI, ESCANCELABLE);

                    if (EFECTOCOMPROBANTE != "Ingreso")
                    {
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El efecto de comprobante es no es \"Ingreso\": ");
                    }
                    if (ESTADOCFDI != "Vigente")
                    {
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El estado del CFDI no es \"Vigente\"");
                    }
                    if (ESCANCELABLE != "Cancelable con aceptación")
                    {
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El Estatus de cancelación no es \"Cancelable con aceptación\"");

                    }

                    driver.Close();
                }

                //}
                //else
                //{
                //    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0,document.body.scrollHeight)");
                //    IWebElement divContenedor = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_MainContent_PnlResultados")));
                //    IWebElement elemento3 = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_MainContent_LblEfectoComprobante")));
                //    EFECTOCOMPROBANTE = elemento3.Text;
                //    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El efecto de comprobante es: " + EFECTOCOMPROBANTE);
                //    elemento3 = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_MainContent_LblEstado")));
                //    ESTADOCFDI = elemento3.Text;
                //    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El estado del CFDI es: " + ESTADOCFDI);
                //    elemento3 = waitent.Until(ExpectedConditions.ElementExists(By.Id("ctl00_MainContent_LblEsCancelable")));
                //    ESCANCELABLE = elemento3.Text;
                //    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El Estatus de cancelación es: " + ESCANCELABLE);
                //    crarTablaSat2();
                //    DT_DATOS_SAT2.Rows.Add(Siniestro, EFECTOCOMPROBANTE, ESTADOCFDI, ESCANCELABLE);

                //    if (EFECTOCOMPROBANTE != "Ingreso")
                //    {
                //        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El efecto de comprobante es no es \"Ingreso\": ");
                //    }
                //    if (ESTADOCFDI != "Vigente")
                //    {
                //        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El estado del CFDI no es \"Vigente\"");
                //    }
                //    if (ESCANCELABLE != "Cancelable con aceptación")
                //    {
                //        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El Estatus de cancelación no es \"Cancelable con aceptación\"");

                //    }

                //    driver.Close();

                //}
            }
            catch (Exception o)
            {
                MessageBox.Show("Se encontró el siguiente error: " + o);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se encontró el siguiente error: " + o);
                exito = false;
            }
            return exito;
        }


        private void crarTablaSat2()
        {
            DT_DATOS_SAT2 = new DataTable();
            DT_DATOS_SAT2.Clear();
            DT_DATOS_SAT2.Columns.Add("NO SINIESTRO", typeof(string));
            DT_DATOS_SAT2.Columns.Add("EFECTO COMPROBANTE", typeof(string));
            DT_DATOS_SAT2.Columns.Add("ESTADO CFDI", typeof(string));
            DT_DATOS_SAT2.Columns.Add("ES CANCELABLE", typeof(string));
        }
    }
}