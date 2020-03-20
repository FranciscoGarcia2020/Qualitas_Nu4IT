#region Referencia de Librerias

using System;
using System.Windows;
using System.Windows.Input;
using System.IO;
using Nu4it;
using nu4itExcel;
using nu4itFox;
using System.Threading;
using System.Collections;
using System.Data;
using System.Windows.Threading;
using Ionic.Zip;
using System.Net;

#endregion


namespace QPil.Pages
{
    /// <summary>
    /// By : Jorge Núñez
    /// Updated : Abril 2018
    /// </summary>
    public partial class SplashWindow : Window
    {

        #region OBJETOS Y VARIABLES STATICAS GLOBALES

        //OBJETOS GLOBALES 
        Login login = new Login();
        Aviso aviso = new Aviso();
        Metodos tools = new Metodos();

        //VARIABLES GLOBALES
        public static bool RespuestaYesNo = false;
        public static string User = "Usuario";
        public static string User_NombreCompleto = "";
        public static string Password = "Passsword";
        public static string HayActualizacion = "";
        public static string[] Macs = new string[0];
        public static string[] Robots = new string[0];
        public static string[] DatosLogin = new string[0];
        public static DataTable DTLogin = new DataTable();
        public static ArrayList Usuarios = new ArrayList();
        public static string Version = String.Empty;
        public static string DataBase = String.Empty;
        public static string InfoGral = String.Empty;
        public static string Licencias = String.Empty;
        public static string Novedades = "...";
        public static bool ActualizacionCorrecta = false;
        public static string versionLBL = "";
        public static string TipoINFO = "LOCAL"; /*"HTTP";*/
        public static string FechaHoraUpdate = "";
        public static string NombreNubot = "QPil";
        public static string CarpetaInstaller = "http://www.nu4itautomation.com/cliente/installer";
        public static string RutaInfoWWW = Directory.GetCurrentDirectory(); //"http://www.nu4itautomation.com/cliente/docs";
        public static string RutaNU = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\NuX";
        public static string RutaNubotVersiones = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\NuX\Versiones";
        // RUTAS WALMART
        public static string RutaAltaProvBD = "", RutaArchProv = "", RutaArchprovTemp = "", RutaBDD_COVE1 = "", RutaBDD_SATY = "", RutaCCWM = "", RutaObservacionesCove = "";

        //OTROS
        int counter = 0;
        public static string RutaNU4IT = "";
        public static string nombreexe = "";
        dynamic actu = (dynamic)null;
        private System.Timers.Timer aTimer;
        public static bool EnDesarrollo = false;
        public static string RutaLogAnterior = " ";
        string RutaActualEXE = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.ToString();

        #endregion

        //INICIACION DE ELEMENTOS
        public SplashWindow()
        {
            InitializeComponent();
            //••••••••••••••••••••••••••••••••••••••••••••• RUTAS •••••••••••••••••••••••••••••••••••••••••••••••••••••••••

            //•••••••••••••••••••••••••••••••••••••••••• DATOS NUBOT ••••••••••••••••••••••••••••••••••••••••••••••••••••••
            nombreexe = System.Reflection.Assembly.GetExecutingAssembly().Location;
            versionLBL = "0.0.0.1";
            lblVersion.Content = versionLBL;
            FechaHoraUpdate = "Actualización al: ";
            Novedades = " " + FechaHoraUpdate;
            //-------------------------------------------------------------------------------------------------------------
        }

        //AL CARGAR LA VENTANA
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        //CRONOMETRO
        bool Logueado = false;
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            counter++;
            if (counter == 2 && !ModoDesarrollador())
            {
                try
                {
                    ///Tipo de usuario
                    // string Dominio = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
                    // if (Dominio.ToString().ToUpper() == "MX.WAL-MART.COM")
                    // {
                    //     TipoINFO = "LOCAL";
                    // }
                    // else
                    // {
                    //     TipoINFO = "HTTP";
                    //     Pages.SplashWindow.RutaInfoWWW = "http://www.nu4itautomation.com/" + "walm/docs";
                    // }
                    ///Datos del servidor
                    Dispatcher.Invoke(((Action)(() => lblStatus.Content = "Obteniendo información del servidor...")));
                    if (tools.ObtenerDatosServidor(TipoINFO))
                    {
                        Dispatcher.Invoke(((Action)(() => lblStatus.Content = "Revisando licencia...")));
                        string MAC = tools.ObtenMacAddress();
                        if (true/*tools.ChecarLicencia(TipoINFO, MAC)*/)
                        {
                            ///Checar la carpeta del escritorio
                            CarpetaNubot_Escritorio();
                            ///Checar elementos necesarios
                            CheckElementosNecesarios();
                            ///Base de datos usuarios
                            Dispatcher.Invoke(((Action)(() => lblStatus.Content = "Revisando la ultima sesión iniciada.")));
                            if (LeyendoBaseDeDatos_ONLINE())
                            {
                                Logueado = true;
                                ///Tiempo de sesion e inicio de sesion
                                Dispatcher.Invoke(((Action)(() => lblStatus.Content = "Iniciando Sistema....")));
                                if (tools.TiempoDeSesion() == false)
                                    Dispatcher.Invoke(((Action)(() => login.Iniciar(tools.ObtenerDatoDeIni("USUARIOACTUAL"), tools.ObtenerDatoDeIni("PASSUSUACTUAL")))));
                                else
                                    Dispatcher.Invoke(((Action)(() => login.Show())));
                            }
                            else
                            {
                                tools.MessageShowOK_2("No se cargo correctamente la información de Login", "ERROR");
                                Dispatcher.Invoke(((Action)(() => App.Current.Shutdown())));
                            }
                        }
                        else
                        {
                            Dispatcher.Invoke(((Action)(() => Clipboard.SetText(MAC))));
                            tools.MessageShowOK_2("No tiene licencia o está vencida\nSolicitar licencia de uso del ROBOT a:\n\nNü4It Automation \nTeléfono: 50206474  \nExt. Sistemas: 2244\njorge.nunez@bestcollect.com.mx\n\nID: " + MAC, "ERROR");
                            if (tools.MessageShowYesNo_2("¿Reintentar?"))
                            {
                                Dispatcher.Invoke(((Action)(() => tools.ReiniciarAccesoDirecto())));
                            }
                            tools.EnviarCorreo("Solicitud de licencia para '" + NombreNubot + "'", tools.DatosDeComputadora(), "jorge.nunez@bestcollect.com.mx", "contacto@nu4itautomation.com;jorge.enu@walmart.com");
                        }
                        Dispatcher.Invoke(((Action)(() => this.Close())));
                        Dispatcher.Invoke(((Action)(() => aTimer.Stop())));
                        Dispatcher.Invoke(((Action)(() => aTimer.Enabled = false)));
                    }
                }
                catch (Exception ex)
                {
                    tools.MessageShowOK_2("E R R O R :\n\n\n" + ex.Message.ToString(), "ERROR");
                    Dispatcher.Invoke(((Action)(() => App.Current.Shutdown())));
                }
            }
        }

        //LEYENDO BASE DE DATOS DESDE EL SERVIDOR
        public bool LeyendoBaseDeDatos_ONLINE()
        {
            DTLogin.Columns.Add("Usuario");
            DTLogin.Columns.Add("Password");
            DTLogin.Columns.Add("Tipo");
            DTLogin.Columns.Add("Robots");
            bool continuar = false;
            try
            {
                DatosLogin = QPil.Pages.SplashWindow.DataBase.Split('\n');
                if (!DatosLogin[0].Contains("<!DOCTYPE HTML PUBLIC"))
                {
                    if (DatosLogin[0] != "")
                    {
                        for (int i = 0; i < DatosLogin.Length; i++)
                        {
                            if (DatosLogin[i] != "")
                            {
                                string[] InfoUsuario = DatosLogin[i].ToString().Split('\t');
                                if (InfoUsuario.Length >= 4)
                                {
                                    DTLogin.Rows.Add(InfoUsuario[0], InfoUsuario[1], InfoUsuario[2], InfoUsuario[3]);
                                    Usuarios.Add(InfoUsuario[0]);
                                }

                            }
                        }
                        continuar = true;
                    }
                    else { tools.MessageShowYesNo_1("No se logro conexión a la base de datos.\n\nCerrando sistema..."); }
                }
                else { tools.MessageShowYesNo_1("No existe base de datos"); }
                if (continuar)
                {
                    //GuradarLicenciasLocal(DatosLogin);
                }
            }
            catch (Exception ex)
            {
                continuar = false;
            }
            if (continuar == false)
            {
                tools.MessageShowYesNo_1("Error al consultar los usuarios");
                Dispatcher.Invoke(((Action)(() => App.Current.Shutdown())));
            }
            return continuar;
        }

        //COPIAR CARPETA
        public bool CopiarCarpeta(string CarpetaOrigen, string CarpetaDestino, bool copySubDirs)
        {
            if (Directory.Exists(CarpetaOrigen))
            {
                if (!Directory.Exists(CarpetaDestino))
                {
                    Directory.CreateDirectory(CarpetaDestino);
                    Thread.Sleep(500);
                }
                if (Directory.Exists(CarpetaDestino))
                {
                    string[] ARCHIVOS = Directory.GetFiles(CarpetaOrigen, "*", SearchOption.AllDirectories);
                    int conta = 1;
                    foreach (string archivo in ARCHIVOS)
                    {
                        Dispatcher.Invoke(((Action)(() => lblStatus.Content = "Instalando... Archivo #" + conta + " de " + ARCHIVOS.Length)));
                        try
                        {
                            string Nombre = archivo.Replace(CarpetaOrigen + @"\", "");
                            ///SubCarpeta
                            string SubCarpeta = CarpetaDestino;
                            int subc = Nombre.LastIndexOf(@"\");
                            if (subc != -1)
                            {
                                SubCarpeta = CarpetaDestino + @"\" + Nombre.Substring(0, subc);
                                if (!Directory.Exists(SubCarpeta))
                                {
                                    Directory.CreateDirectory(SubCarpeta);
                                    Thread.Sleep(500);
                                }
                            }
                            if (Directory.Exists(SubCarpeta))
                            {
                                if (!archivo.Contains("setup.exe"))
                                {
                                    File.Copy(archivo, CarpetaDestino + @"\" + Nombre);
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                        conta++;
                    }
                }
                return true;
            }
            return false;
        }

        //COPIAR A LA CARPETA COMPARTIDA
        public void CopiaACompartida(string RutaEjecutable, string EjecutableNUEVO)
        {
            try
            {
                File.Copy(RutaEjecutable, EjecutableNUEVO);
                Thread.Sleep(500);
                if (File.Exists(EjecutableNUEVO))
                {
                    tools.MessageShowOK_2("Ejecutable copiado a la Carpeta Compartida", "OK2");
                }
            }
            catch (Exception ex)
            {
                tools.MessageShowOK_2("ERROR al copiar ejecutable a la Carpeta Compartida", "ERROR");
            }
        }

        //MOVER LA VENTANA
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception)
            {

            }
        }

        //CERRAR
        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(((Action)(() => this.Close())));
        }

        //MATAR TODAS LAS INSTANCIAS DEL NUBOT
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Logueado)
            {
                try
                {
                    System.Diagnostics.Process[] myProcesses;
                    myProcesses = System.Diagnostics.Process.GetProcesses();
                    foreach (System.Diagnostics.Process myProcess in myProcesses)
                    {
                        if (myProcess.ProcessName.ToString().ToUpper().Contains("NUBOT") || myProcess.ProcessName.ToString().ToUpper().Contains("SETUP"))
                        {
                            myProcess.Kill();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //CARPETA NECESARIA PARA EL TRABAJO
        public void CarpetaNubot_Escritorio()
        {
            Dispatcher.Invoke(((Action)(() => lblStatus.Content = "Comprobando los componentes necesarios para trabajar")));
            if (!Directory.Exists(RutaNU))
            {
                try
                {
                    Dispatcher.Invoke(((Action)(() => lblStatus.Content = "Instalando...")));
                    CopiarCarpeta(CarpetaInstaller, RutaNU, true);
                }
                catch (Exception)
                {
                    tools.MessageShowOK_2("No se logró instalar la carpeta necesaria para trabajar.\n\nPor favor, dar avíso al proveedor 'Nü4it Automation'", "ERROR");
                }
            }
            ///
        }

        //VERIFICAR DOCUMENTOS Y EJECUTABLES PARA LA EJECUCION CORRECTA DEL NUBOT
        public void CheckElementosNecesarios()
        {
            // - - - CARPETAS NECESARIAS - - -
            string[] CarpetasNecesarias = new string[]
            {
                @"\Versiones", @"\Documentos", @"\Versiones\Bitacoras",
            };
            foreach (string path in CarpetasNecesarias)
            {
                Dispatcher.Invoke(((Action)(() => lblStatus.Content = "Instalando carpetas necesarias: " + path)));
                switch (path)
                {
                    case @"\Imagenes":
                        if (!Directory.Exists(Directory.GetCurrentDirectory() + path))
                            descargaArchivoNU4IT("ImagenesNBMKR.zip");
                        break;
                    default:
                        string carpeta = Directory.GetCurrentDirectory() + path;
                        if (!Directory.Exists(carpeta))
                            Directory.CreateDirectory(carpeta);
                        string carpetaDSK = RutaNU + path;
                        if (!Directory.Exists(carpetaDSK))
                            Directory.CreateDirectory(carpetaDSK);
                        break;
                }
            }
            // - - - ARCHIVOS NECESARIOS PARA TRABAJAR - - - 
            string[] RutaArchivosNecesarios = new string[]
            {
               @"\chromedriver.exe",
               @"\IEDriverServer.exe",
            };
            foreach (string item in RutaArchivosNecesarios)
            {
                if (!File.Exists(RutaNU + item))
                {
                    ///Si ya existen copiarlos a la carpeta de destino del debug
                    try
                    {
                        switch (item)
                        {
                            case @"\Versiones\x86\SQLite.Interop.dll":
                                if (!File.Exists(Directory.GetCurrentDirectory() + @"\x86\" + item.Replace(@"\Versiones\x86\", "")))
                                {
                                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\x86");
                                    File.Copy(RutaNU + item, Directory.GetCurrentDirectory() + @"\x86\" + item.Replace(@"\Versiones\x86\", ""));
                                }
                                break;
                            case @"\chromedriver.exe":
                                if (!File.Exists(Directory.GetCurrentDirectory() + @"\" + item))
                                {
                                    descargaArchivoNU4IT("DebugNBMKR.zip");
                                }
                                break;
                            default: break;
                        }
                    }
                    catch (Exception)
                    {
                        tools.MessageShowOK_2("Es necesario que el elemento: " + item + " se encuentre instalado.", "ALERTA");
                    }
                }
            }
            // - - - ARCHIVOS A ELIMINAR - - - 
            string[] ArchivosAEliminar = new string[]
            {

            };
            string[] ArchivosNU = Directory.GetFiles(RutaNU, "*", SearchOption.AllDirectories);
            foreach (string FILE in ArchivosNU)
            {
                foreach (string item in ArchivosAEliminar)
                {
                    if (FILE.Contains(item))
                    {
                        try
                        {
                            File.Delete(item);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        ///DESCARGAR ARCHIVO DE SERVIDOR
        public bool descargaArchivoNU4IT(string NoimbArciv)
        {
            String ruta = Directory.GetCurrentDirectory() + @"\" + NoimbArciv;
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
                do
                {
                    System.Threading.Thread.Sleep(1000);
                    if (!File.Exists(ruta))
                    {
                        break;
                    }
                } while (true);
            }

            WebClient client = new WebClient();
            client.Proxy = WebRequest.DefaultWebProxy;
            client.Credentials = System.Net.CredentialCache.DefaultCredentials;
            client.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
            client.DownloadFile("http://www.nu4itautomation.com/cliente/installer/" + NoimbArciv, NoimbArciv);

            ///String ruta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\" + NoimbArciv;
            int intentos = 0;
            while (!File.Exists(ruta) && intentos < 100)
            {
                System.Threading.Thread.Sleep(1000);
                intentos++;
            }
            if (File.Exists(ruta))
            {
                using (ZipFile zip = ZipFile.Read(ruta))
                {
                    System.Threading.Thread.Sleep(1000);
                    try
                    {
                        zip.ExtractAll(Directory.GetCurrentDirectory());
                        System.Threading.Thread.Sleep(500);
                        if (File.Exists(ruta))
                        {
                            File.Delete(ruta);
                            do
                            {
                                System.Threading.Thread.Sleep(1000);
                                if (!File.Exists(ruta))
                                {
                                    break;
                                }
                            } while (true);
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.ToString().IndexOf("already exists") > 0)
                        {
                            return true;
                            zip.Dispose();
                            tools.MessageShowOK_2("Esta versión es la mas reciente!");
                        }
                        else if (ex.ToString().IndexOf("ya existe") > 0)
                        {
                            return true;
                            zip.Dispose();
                            tools.MessageShowOK_2("Esta versión es la mas reciente!");
                        }
                        else
                        {
                            return false;
                            tools.MessageShowOK_2("Excepcion al descargar archivo " + Environment.NewLine + ex, "ERROR");
                        }
                    }
                }
            }
            else
            {
                tools.MessageShowOK_2("Error al descargar archivo ", "ERROR");
            }
            return false;
        }

        ///INICIANDO EL SISTEMA SI ESTA EN MODO 'DESARROLLADOR'
        public bool ModoDesarrollador()
        {
            if (RutaActualEXE.Contains(@"\bin\Debug*"))
            {
                Pages.SplashWindow.RutaCCWM = @"\\192.168.2.4\public\";

                EnDesarrollo = true;
                Dispatcher.Invoke(((Action)(() => lblStatus.Content = " [ Developer Mode ] ")));
                Dispatcher.Invoke(((Action)(() => imgUpdatisng2.Visibility = System.Windows.Visibility.Collapsed)));

                Dispatcher.Invoke(((Action)(() => Pages.Login.superMAINWINDOW = new MainWindow())));
                Dispatcher.Invoke(((Action)(() => Pages.Login.superMAINWINDOW.Show())));
                Dispatcher.Invoke(((Action)(() => Pages.Login.superMAINWINDOW.lblTitulo.Content = NombreNubot + " [ Developer Mode ] ")));

                Dispatcher.Invoke(((Action)(() => this.Hide())));
                Dispatcher.Invoke(((Action)(() => aTimer.Enabled = false)));
                Dispatcher.Invoke(((Action)(() => aTimer.Stop())));
                return true;
            }
            return false;
        }

    }
}
