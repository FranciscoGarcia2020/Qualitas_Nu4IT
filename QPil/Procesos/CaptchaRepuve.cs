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
using System.Threading;

namespace QPil.Procesos
{
    class CaptchaRepuve
    {
        Forms.AbrirForm abreForm;
        Forms.ValidaDatosSat form;
        //Forms.escanear form;
        //private static Metodos tools = new Metodos();
        AutomatizacionOCR objOCR = new AutomatizacionOCR();
        usaR objNu4 = new usaR(); //Objeto para usar funciones generales
        public static string VARSTR_BITACORA_LOG;
        string[] CONTENIDOINI;
        string modelo;
        string año;
        string marca;
        string clase;
        string tipo;
        string planta;
        string id, serie, rutaPDF, no_siniestro;
        public static DataTable DT_DATOS_REPUVE { get; set; }
        public static bool ValidacionBoton { get; internal set; }
        string Siniestro;
        IWebDriver driver;
        public CaptchaRepuve(string RutaLog)
        {
            VARSTR_BITACORA_LOG = RutaLog;
            // CONTENIDOINI = objNu4.LeerArchivoIni("OCR");
        }
        public void recibeDatatableRepuve(DataTable tabla, string siniestro)
        {
            Siniestro = siniestro;
            foreach (DataRow fila in tabla.Rows)
            {
                ProcesoCaptchaRepuve(fila);
            }
        }

        public void recibeDatatableRepuve(DataRow fila, IWebDriver Driver)
        {
            ProcesoCaptchaRepuve(fila, Driver);
        }

        public bool ProcesoCaptchaRepuve(DataRow datos, IWebDriver Driver = null)
        {

            id = datos["ID"].ToString();
            no_siniestro = datos["NO SINIESTRO"].ToString();
            serie = datos["SERIE"].ToString();
            rutaPDF = datos["RUTA PDF"].ToString();
            bool Exito = true;
            bool encontre = false;
            int posUrl;
            string valorcaptcha = "";
            string URL;
            string anchoPantalla = Screen.PrimaryScreen.Bounds.Width.ToString();
            string altoPantalla = Screen.PrimaryScreen.Bounds.Height.ToString();
            string resolucion = anchoPantalla + "x" + altoPantalla;
            var driverService = ChromeDriverService.CreateDefaultService();
            ChromeOptions options = new ChromeOptions();
            driverService.HideCommandPromptWindow = true;
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
            //driver = new ChromeDriver(driverService, new ChromeOptions());
            WebDriverWait waitent = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            WebDriverWait waitL = new WebDriverWait(driver, TimeSpan.FromSeconds(2));

            // MessageBox.Show("La resolucion es:" + resolucion);
            try
            {
            //    while (encontre == false)
            //    {



                    //posUrl = objNu4.UbicadoEnPos(CONTENIDOINI, "URLREPUVE=", 0);
                    //URL = CONTENIDOINI[posUrl];
                    URL = "http://www2.repuve.gob.mx:8080/ciudadania/";/* URL.Replace("URLREPUVE=", "");*/
                    driver.Navigate().GoToUrl(URL);
                    driver.Manage().Window.Maximize();
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se accede a la URL: " + URL);
                //ZOOM
               // IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                //js.ExecuteScript("document.body.style.zoom='50%'");
                IWebElement elemento = waitent.Until(ExpectedConditions.ElementExists(By.Id("placa")));
                    //elemento.SendKeys("mev4026");//206yey , 812TAY, 203PFJ
                    //objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se ingresa el número de placa: mev4026");
                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.Id("nrpv")));
                    elemento.SendKeys(OpenQA.Selenium.Keys.Tab);

                string errorcap="";
                try
                {
                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"txtError\"]")));
                    errorcap = elemento.Text;
                }
                catch (Exception e)
                {
                    
                }
                do
                {
                    elemento = driver.FindElement(By.XPath("/html/body/main/form/div[1]/div/div[3]/div[5]/div[1]/img"));
                    int w = elemento.Size.Width;
                    int h = elemento.Size.Height;
                    int x = elemento.Location.X;
                    int y = elemento.Location.Y;

                    Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                    ss.SaveAsFile("file_name_string.Png", ScreenshotImageFormat.Png);
                    switch (resolucion)
                    {
                        case "1920x1080":
                            valorcaptcha = objOCR.ocrRectanguloProcesado(Directory.GetCurrentDirectory() + @"\file_name_string.Png", x, y, w, h);
                            objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se manda RESOLUCION CASE 1");
                            break;
                        case "1366x768":
                            valorcaptcha = objOCR.ocrRectanguloProcesado(Directory.GetCurrentDirectory() + @"\file_name_string.Png", x, y - 330, w, h);
                            objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se manda RESOLUCION CASE 2");
                            break;
                        case "1600x900":
                            valorcaptcha = objOCR.ocrRectanguloProcesado(Directory.GetCurrentDirectory() + @"\file_name_string.Png", x, y - 35, w, h);
                            objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se manda RESOLUCION CASE 3");
                            break;
                        default:
                            valorcaptcha = objOCR.ocrRectanguloProcesado(Directory.GetCurrentDirectory() + @"\file_name_string.Png", x, y, w, h);
                            break;
                    }
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se toma captura a la pantalla y se procesa captcha con valor: " + valorcaptcha);
                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.Id("vin")));
                    elemento.SendKeys(serie);
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se ingresa el número de serie: " + serie);
                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.Id("captcha")));
                    elemento.SendKeys(valorcaptcha);
                    System.Threading.Thread.Sleep(1000);
                    System.Threading.Thread.Sleep(1500);
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se ingresa el captcha: " + valorcaptcha);
                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/main/form/div[1]/div/div[3]/div[7]/div[2]/button[2]")));
                    elemento.Click();
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se manda click al botón buscar");
                    try
                    {
                        elemento = driver.FindElement(By.XPath("//*[@id=\"txtError\"]"));
                      //  elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"txtError\"]")));
                        errorcap = elemento.Text;
                    }
                    catch (Exception e)
                    {
                        errorcap = "";
                    }
                } while (errorcap== "El texto de la imagen y el que captura deben ser iguales"|| errorcap== "Falta escribir el texto de la Imagen");


                    var encuentra = driver.FindElements(By.XPath("/ html/body/div[2]/div/div/div/div[1]/div/ul/li[2]/a"));//"/html/body/div[2]/div/div/div/div[1]/div/ul/li[3]/a" 

                    if ((encuentra.Count == 0 || encuentra == null))
                    {
                        elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"txtError\"]")));
                    string error = "NIV Invalido ("+serie+")";
                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"txtError\"]")));
                    if (elemento.Text == "No se ha seleccionado ningún criterio de búsqueda." || elemento.Text== error)
                        {
                            //tools.MessageShowYesNo_2("¿Desea editar?");
                            DialogResult desicion = MessageBox.Show(new Form { TopMost = true }, "¿Desea editar?", "Los datos ingresados son invalidos", MessageBoxButtons.YesNo);
                            switch (desicion)
                            {
                                case DialogResult.Yes:

                                    //driver.Close();
                                    Forms.ValidaDatosSat form = new Forms.ValidaDatosSat(VARSTR_BITACORA_LOG, rutaPDF, serie, id, driver);
                                    form.ShowDialog();
                                    if (ValidacionBoton)
                                    {
                                        Exito = false; 
                                        
                                    }
                                  
                                  
                                    break;
                                case DialogResult.No:
                                    //driver.Close();

                                    Exito = false;
                                    break;
                            }
                        }
                        else
                        {
                            driver.Close();
                            encontre = false;
                        }
                    try
                    {
                        driver.Close();
                    }
                    catch (Exception e)
                    {
  
                    } 
                }
                    else
                    {

                        elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("/ html/body/div[2]/div/div/div/div[1]/div/ul/li[2]/a")));
                        elemento.Click();
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se manda click al elemento PGJ");
                        elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"tab-avisoRobo\"]/div")));
                        string reportePGJ = elemento.Text;
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El vehiculo con serie WAU9FD8T3EA007986tiene el siguiente reporte en PGJ: " + reportePGJ);

                        elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[2]/div/div/div/div[1]/div/ul/li[3]/a")));
                        elemento.Click();
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se manda click al elemento OCRA");
                        elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"tab-ocra\"]/div[1]")));
                        string reporteOCRA = elemento.Text;
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El vehiculo con serie WAU9FD8T3EA007986  tiene el siguiente reporte en OCRA: " + reporteOCRA);
                   
                    #region
                    //elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[2]/div/div/div/div[1]/div/ul/li[1]/a"))); //*[@id="tab-info-vehiculo"]/table/tbody/tr[1]/td[2]/small
                    //elemento.Click();
                    //IWebElement div = driver.FindElement(By.Id("tab-info-vehiculo"));
                    //IWebElement tabla = div.FindElement(By.TagName("tbody"));
                    //var coleccion_tr = tabla.FindElements(By.TagName("tr"));
                    //int tags = 0;

                    //while (tags < 14)
                    //{
                    //    foreach (var item in coleccion_tr)
                    //    {
                    //        string datoExtraer = item.Text;
                    //        if (tags == 0 || tags == 1 || tags == 2 || tags == 3 || tags == 4 || tags == 14)
                    //        {
                    //            switch (tags)
                    //            {
                    //                case 0:
                    //                    string[] marcaA = datoExtraer.Split(':');
                    //                    marca = marcaA[1];
                    //                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "La marca de esa serie es: " + marca);
                    //                    tags++;
                    //                    break;
                    //                case 1:
                    //                    string[] modeloA = datoExtraer.Split(':');
                    //                    modelo = modeloA[1];
                    //                    tags++;
                    //                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El modelo de esa serie es: " + modelo);
                    //                    break;
                    //                case 2:
                    //                    string[] añoA = datoExtraer.Split(':');
                    //                    año = añoA[1];
                    //                    tags++;
                    //                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El año de esa serie es: " + año);
                    //                    break;
                    //                case 3:
                    //                    string[] claseA = datoExtraer.Split(':');
                    //                    clase = claseA[1];
                    //                    tags++;
                    //                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "La clase de esa serie es: " + clase);
                    //                    break;
                    //                case 4:
                    //                    string[] tipoA = datoExtraer.Split(':');
                    //                    tipo = tipoA[1];
                    //                    tags++;
                    //                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "El tipo de esa serie es: " + tipo);
                    //                    break;
                    //                case 14:
                    //                    string[] plantaA = datoExtraer.Split(':');
                    //                    planta = plantaA[1];
                    //                    tags++;
                    //                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "La planta de ensamblaje de esa serie es: " + planta);
                    //                    break;
                    //                default:
                    //                    break;
                    //            }

                    //CrearTablaDT_DATOS_REPUVE();
                    //DT_DATOS_REPUVE.Rows.Add(Siniestro, marca, modelo, año, clase, tipo, planta);

                    #endregion

                    DataRow[] row = Procesos.ManejoDeDocumentos.DT_DATOS_REPUVE.Select("ID ='" + id + "'");
                        for (int i = 0; i < row.Length; i++)
                        {
                            row[i]["ANTECEDENTE"] = reportePGJ;
                            row[i].AcceptChanges();
                        }
                    ////     }
                    //     else
                    //     {
                    //         tags++;
                    //     }
                    //  }

                    //  }
                    try
                    {
                        driver.Close();
                    }
                    catch (Exception e)
                    {
                        
                    }
                        Exito = true;
                        encontre = true;
                    }
                
            }
            catch (System.OutOfMemoryException m)
            {
                MessageBox.Show("Favor de verificar la resolución de pantalla con el proveedor: " + m);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Favor de verificar la resolución de pantalla con el proveedor: " + m);
                Exito = false;
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException t)
            {

                MessageBox.Show("Favor de revisar su conexión a Internet error: " + t);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Favor de revisar su conexión a Internet error: " + t);
                Exito = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Se encontró el siguiente error: " + e);
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Se encontró el siguiente error: " + e);
                Exito = false;
            }


            //
            return Exito;

        }


        private void CrearTablaDT_DATOS_REPUVE()

        {
            DT_DATOS_REPUVE = new DataTable();
            DT_DATOS_REPUVE.Clear();
            DT_DATOS_REPUVE.Columns.Add("NO SINIESTRO", typeof(string));
            DT_DATOS_REPUVE.Columns.Add("MARCA", typeof(string));
            DT_DATOS_REPUVE.Columns.Add("MODELO", typeof(string));
            DT_DATOS_REPUVE.Columns.Add("AÑO", typeof(string));
            DT_DATOS_REPUVE.Columns.Add("CLASE", typeof(string));
            DT_DATOS_REPUVE.Columns.Add("TIPO", typeof(string));
            DT_DATOS_REPUVE.Columns.Add("PLANTA", typeof(string));
        }
    }
}
