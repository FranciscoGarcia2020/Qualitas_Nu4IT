using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nu4it;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Windows.Threading;
using System.ComponentModel;

namespace QPil.Procesos
{
    class QContent : System.Windows.Controls.UserControl
    {
        static usaR objNu4 = new usaR(); //Objeto para usar funciones generales
        private string rutaLog;
        private Metodos tools = new Metodos();
        private Ejecucion_Individual.QContent pbBarra;
        Procesos.ManejoDeDocumentos document;
        public void HiloDeEjecucion()
        {
            rutaLog = Directory.GetCurrentDirectory() + DateTime.Now.ToString("dd/MM/yy").Replace("/", "") + "_" + DateTime.Now.ToString("hh:mm").Replace(":", "") + ".log";
            objNu4.CreaArchivoLog(rutaLog);
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += FuncionPrincipalHILO;
            worker.RunWorkerAsync();
        }
        private void FuncionPrincipalHILO(object sender, DoWorkEventArgs e)
        {
            objNu4.ReportarLog(rutaLog, "Iniciando QContent descarga de documentos");
            ContentNavigate(rutaLog);
        }

        public void ContentNavigate(string RutaLog)
        {

            //List<string> siniestros = AutomatizadorExcel.Siniestros(RutaLog);
            string rutaDescargas = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            rutaDescargas = rutaDescargas + "\\pruebas\\Siniestros"; string siniestro = "04182635987";
            #region Proceso QContent


            /*
            IWebDriver driver = null;
            List<string> Siniestros = new List<string>();
            string rutaURL = "http://110.10.100.195:16200/cs/";
            string usuario = "edgarcia";
            string contrasenia = "g3a2q4e5";
            //string siniestro = "04181288094";
            List<string> links = new List<string>();
            ChromeOptions options = new ChromeOptions();
            string rutaDescargas = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            rutaDescargas = rutaDescargas + "\\pruebas\\Siniestros";
            try
            {
                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                options.AddArguments("disable-infobars");
                options.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
                options.AddArgument("no-sandbox");
                options.AddUserProfilePreference("download.default_directory", rutaDescargas);
                //options.AddArguments("-incognito");
                options.AddArguments("--disable-popup-blocking");
                //options.AddArguments("--disable-extensions");
                //options.AddUserProfilePreference("plugins.plugins_disabled", ["Chrome PDF Viewer"]);
                options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);//Linea para dehabilitar visor de PDF en el explorador
                driver = new ChromeDriver(driverService, options);
                driver.Navigate().GoToUrl(rutaURL);
                objNu4.ReportarLog(RutaLog, "abriendo explorador");
                //tools.StatusSHOW("abriendo Explorador");
                WebDriverWait waitent = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                IWebElement elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[1]/tbody/tr[3]/td/table/tbody/tr/td[3]/a")));
                objNu4.ReportarLog(RutaLog, "encuentro link de acceso");
                driver.FindElement(By.XPath("/html/body/table[1]/tbody/tr[3]/td/table/tbody/tr/td[3]/a")).Click();
                elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='j_username']")));
                driver.FindElement(By.XPath("//*[@id='j_username']")).SendKeys(usuario);
                //ingresa usuario
                objNu4.ReportarLog(RutaLog, "ingresando usuario");

                //Actions action = new Actions(driver);
                //action.SendKeys(OpenQA.Selenium.Keys.Tab);
                elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='j_password']")));
                //Ingresa contraseña 
                driver.FindElement(By.XPath("//*[@id='j_password']")).SendKeys(contrasenia);
                objNu4.ReportarLog(RutaLog, "ingresando contrasenia");
                driver.FindElement(By.XPath("//*[@id='loginForm']/ul/li[3]/div/input")).Click();
                objNu4.ReportarLog(RutaLog, "logueando");
                //desde esta linea comenzaria el ciclo por numero de siniestro
                elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='ygtvlabelel27']")));
                //se busca boton de busqueda para desplegar submenu de busqueda
                driver.FindElement(By.XPath("//*[@id='ygtvlabelel27']")).Click();
                //tools.StatusSHOW("Comenzando la busqueda de documentos");
                //objNu4.ReportarLog(RutaLog, "encuentro link de busqueda avanzada y hago click");
                objNu4.ReportarLog(RutaLog, "expandiendo opcion de busqueda");
                driver.SwitchTo().Frame("ygtvc27_iframe");
                objNu4.ReportarLog(RutaLog, "entrando al frame ygtvc27_iframe");
                objNu4.ReportarLog(RutaLog, "buscando link de busqueda avanzada");
                elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table/tbody/tr[2]/td[7]/a")));
                driver.FindElement(By.XPath("/html/body/table/tbody/tr[2]/td[7]/a")).Click();
                //Accede a la opcion de busqueda avanzada para poder realizar busqueda por N| de Siniestro
                foreach (string siniestro in siniestros)
                {
                    objNu4.ReportarLog(RutaLog, "siniestro numero" + siniestro);
                    driver.SwitchTo().DefaultContent();
                    //tools.StatusSHOW("Buscando documentos de N");
                    driver.SwitchTo().Frame("contentFrame");
                    objNu4.ReportarLog(RutaLog, "entrando a frame contentFrame");
                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.Name("xSiniestro")));
                    driver.FindElement(By.Name("xSiniestro")).SendKeys(siniestro);
                    //se ingresa numero de siniestro para obtener documentos
                    objNu4.ReportarLog(RutaLog, "enviando un numero de siniesto a buscar");
                    elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='pageContent']/table/tbody/tr[2]/td[2]/div/form[2]/div/input[1]")));
                    objNu4.ReportarLog(RutaLog, "buscando boton de busqueda");
                    driver.FindElement(By.XPath("//*[@id='pageContent']/table/tbody/tr[2]/td[2]/div/form[2]/div/input[1]")).Click();
                    //driver.SwitchTo().DefaultContent();
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
                    try
                    {
                        //en esta parte quitamos una ventana emergente que en ocaciones aparece
                        driver.SwitchTo().Alert().Accept();
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
                        driver.SwitchTo().DefaultContent();
                    }
                    catch (Exception ex)
                    {
                        objNu4.ReportarLog(RutaLog, ex.ToString());
                        driver.SwitchTo().DefaultContent();
                        //throw;
                    }
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
                    driver.SwitchTo().Frame("ygtvc27_iframe");
                    objNu4.ReportarLog(RutaLog, "entrando a frame ygtvc27_iframe");
                    driver.SwitchTo().Frame("tab_1_content");
                    objNu4.ReportarLog(RutaLog, "entrando a frame tab_1_content");
                    var opciones = driver.FindElements(By.TagName("a"));//
                                                                        //obtenemos la lista de los links que redireccionan a los dcumentos a descargar
                    objNu4.ReportarLog(RutaLog, "buscando documentos asignados");
                    List<IWebElement> correcto = new List<IWebElement>();
                    int total = opciones.Count;
                    foreach (IWebElement item in opciones)
                    {
                        //proceso sin web client
                        try
                        {
                            driver.SwitchTo().Frame("tab_1_content");
                            objNu4.ReportarLog(RutaLog, "entrando a frame tab_1_content");
                        }
                        catch (Exception)
                        {

                        }
                        //proceso sin web client
                        // valida los tres documentos a descargar
                        if (item.Text.Contains("Factura") || item.Text.Contains("Tenencia") || item.Text.Contains("Póliza"))
                        {
                            objNu4.ReportarLog(RutaLog, "Link encontrado" + item.Text);
                            objNu4.ReportarLog(RutaLog, "haciendo click en " + item.Text);
                            item.Click();
                            string href = item.GetAttribute("href");
                            links.Add(href);
                            objNu4.ReportarLog(RutaLog, "link de descarga " + href);
                            //se obtiene link de descarga de documento para descargarlo. 
                            //verifica si existe la carpeta con numero de siniestro, si no existe la crea
                            objNu4.ReportarLog(RutaLog, "creando carpeta " + rutaDescargas + @"\" + siniestro);
                            if (!Directory.Exists(rutaDescargas + @"\" + siniestro))
                                Directory.CreateDirectory(rutaDescargas + @"\" + siniestro);
                            try
                            {
                                //using (WebClient webClient = new WebClient())
                                //{
                                objNu4.ReportarLog(RutaLog, "Descargando Documentos");
                                objNu4.ReportarLog(RutaLog, "href= " + href + ", ruta guardar= " + rutaDescargas + @"\" + siniestro + "\\" + item.Text.Replace(" ", "") + ".pdf");

                                //rutaDescargas = rutaDescargas + @"\" + siniestro + @"\";
                                objNu4.ReportarLog(RutaLog, rutaDescargas);
                                string[] archivosantes = TotalArchivosDownloads(rutaDescargas, ".pdf");

                                string[] archivosnuevos = TotalArchivosDownloads(rutaDescargas, ".pdf");

                                objNu4.ReportarLog(RutaLog, "enviando url al mismo driver");
                                driver.Navigate().GoToUrl(href);
                                try
                                {
                                    //en esta parte quitamos una ventana emergente que en ocaciones aparece
                                    objNu4.ReportarLog(RutaLog, "buscando y cerrando cerrando ventana para permitir mas descargas");
                                    driver.SwitchTo().Alert().Accept();
                                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
                                    driver.SwitchTo().DefaultContent();
                                }
                                catch (Exception ex)
                                {
                                    objNu4.ReportarLog(RutaLog, ex.ToString());
                                    driver.SwitchTo().DefaultContent();
                                    //throw;
                                }

                                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(4));
                                while (archivosantes.Length == archivosnuevos.Length)
                                {
                                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
                                    archivosnuevos = TotalArchivosDownloads(rutaDescargas, ".pdf");
                                }


                                //MessageBox.Show(new Form() { TopMost = true }, "Descarga Exitosa");

                            }
                            catch (StaleElementReferenceException ex)
                            {
                                objNu4.ReportarLog(RutaLog, "try de descarga---------" + ex.ToString());
                            }
                            objNu4.ReportarLog(RutaLog, "intentando regresar a primer window");

                            driver.SwitchTo().DefaultContent();
                            driver.SwitchTo().Frame("ygtvc27_iframe");
                            objNu4.ReportarLog(RutaLog, "entrando a frame ygtvc27_iframe");

                        }
                    }*/
            if (!Directory.Exists(rutaDescargas + @"\" + siniestro))
                Directory.CreateDirectory(rutaDescargas + @"\" + siniestro);
            MoverArchivos(rutaDescargas, rutaDescargas + @"\" + siniestro);/*Aqui se debe agregar el nombre del archivo que se esta descargando para que se puedan renombrar los archivos*/
                                                                           /* objNu4.ReportarLog(RutaLog, "accediendo al frame ygtvc27_iframe");
                                                                            driver.SwitchTo().DefaultContent();
                                                                            driver.SwitchTo().Frame("ygtvc27_iframe");
                                                                            elemento = waitent.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table/tbody/tr[2]/td[7]/a")));
                                                                            objNu4.ReportarLog(RutaLog, "buscando link de busqueda avanzada");
                                                                            //driver.FindElement(By.XPath("/html/body/table/tbody/tr[2]/td[7]/a")).Click();
                                                                            elemento.Click();

                                                                            objNu4.ReportarLog(RutaLog, "saliendo de los frames");
                                                                            //driver.SwitchTo().DefaultContent();
                                                                            //driver.SwitchTo().DefaultContent();
                                                                            //driver.SwitchTo().DefaultContent();
                                                                            objNu4.ReportarLog(RutaLog, "comenzando proceso de clasificacion " + rutaDescargas);
                                                                            Siniestros.Add(siniestro);
                                                                        }
                                                                        foreach (string siniestro in Siniestros)
                                                                        {
                                                                            Dispatcher.Invoke(((Action)(() => document = new ManejoDeDocumentos(RutaLog))));
                                                                            Dispatcher.Invoke(((Action)(() => document.obtenCarpetas(rutaDescargas + @"\", siniestro))));
                                                                        }
                                                                        //res = true;
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        objNu4.ReportarLog(RutaLog, ex.ToString());
                                                                        MessageBox.Show(new Form() { TopMost = true }, "Error .----------- " + ex.ToString());
                                                                        //  driver.Quit();
                                                                        //res = false;
                                                                    }
                                                                    */
            #endregion
            Dispatcher.Invoke(((Action)(() => document = new ManejoDeDocumentos(RutaLog))));
            Dispatcher.Invoke(((Action)(() => document.obtenCarpetas(rutaDescargas + @"\", siniestro))));
        }







        //Descomentar parte del nombre cuando se ejecute con QContent.
        private static bool MoverArchivos(string pathsource, string pathdest/*, string nombreNuevo*/)
        {
            bool res = false;
            try
            {
                string[] totalarchivos = Directory.GetFiles(pathsource, "*.pdf");

                foreach (var item in totalarchivos)
                {
                    string name = Path.GetFileName(item);
                    if (!File.Exists(pathdest + @"\" + name))
                    {
                        //Esta declaración garantiza que el archivo es pero el identificador no se mantiene.
                        using (FileStream fs = File.Create(pathdest + @"\" + name)) { }
                    }

                    // Asegúrese de que el objetivo no existe.
                    if (File.Exists(pathdest + @"\" + name))
                        File.Delete(pathdest + @"\" + name);

                    // moviendo el archivo
                    File.Move(pathsource + @"\" + name, pathdest + @"\" + name);
                }

                // See if the original exists now.
                if (File.Exists(pathsource))
                {
                    Console.WriteLine("The original file still exists, which is unexpected.");
                }
                else
                {
                    Console.WriteLine("The original file no longer exists, which is expected.");
                }
            }
            catch (Exception)
            {

                throw;
            }
            return res;
        }


        //verifica total de archivos cargados en un directorio determinado
        private static string[] TotalArchivosDownloads(string RutaDescargas, string extencion)
        {
            int i = 0;
            string[] xlsAux = Directory.GetFiles(RutaDescargas, "*" + extencion);
            foreach (string FileName in xlsAux)
            {
                xlsAux[i] = FileName;
                i++;
            }
            return (xlsAux);
        }
    }
}