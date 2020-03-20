using System;
using System.Collections.Generic;
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
using System.Collections;
using System.Diagnostics;
//Librerias
using System.Data;
using System.Threading;
using System.IO;
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace QPil.Pages
{
    public partial class Login : Window
    {
        Pages.Aviso aviso = new Pages.Aviso();
        Metodos tools = new Metodos();
        public static dynamic superMAINWINDOW = (dynamic)null;
        public string PermisosUsuario = "";
        public static string User = "Usuario";
        Uri userCorrect = new Uri(@"\QPil;component\Resources\Imagenes\Nubot\user32correcto.png", UriKind.RelativeOrAbsolute);
        Uri userInCorrect = new Uri(@"\QPil;component\Resources\Imagenes\Nubot\user32erroneo.png", UriKind.RelativeOrAbsolute);
        Uri passCorrect = new Uri(@"\QPil;component\Resources\Imagenes\Nubot\pass32correcto.png", UriKind.RelativeOrAbsolute);
        Uri passWrong = new Uri(@"\QPil;component\Resources\Imagenes\Nubot\pass32incorrecto.png", UriKind.Relative);

        //
        public Login()
        {
            InitializeComponent();
            gridLogin.Opacity = 0;
        }

        //MOVER EL CONTRLUSER
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();

            }
            catch (Exception)
            {

            }
        }



        //AL CAMBIAR EL TEXTO DEL TEXTBOX
        private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            bnd = false;
            string UserLog = txtUser.Text;
            if (SplashWindow.Usuarios.Contains(UserLog))
            {
                imgUser.Source = new BitmapImage(userCorrect);
            }
            else
            {
                imgUser.Source = new BitmapImage(userInCorrect);
            }
        }

        //AL INGRESAR ENTER AL TXT DE PASSWORD
        bool bnd = false;
        private void txtPass_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && bnd == false)
            {
                bnd = true;
                HiloDeEjecucion("Iniciando sistema");
            }
        }

        ///QPil.Resources.Imagenes.Cliente.LoadingSPARK asd = new QPil.Resources.Imagenes.Cliente.LoadingSPARK();

        //INICIAR EL SISTEMA
        public void Iniciar(string UserLog, string PassLog)
        {
            ///
            InfoUSer(UserLog);
            if (SplashWindow.Usuarios.Contains(UserLog))
            {
                SplashWindow.User = UserLog;
                User = SplashWindow.User;
                ///Hay password guardado?
                string PassCorrecto = "";
                if (PASS_ini != "")
                    PassCorrecto = PASS_ini;
                else
                    PassCorrecto = tools.BuscaValor_LINQ(SplashWindow.DTLogin, "Usuario", UserLog, "Password");
                ///Si no es el usuario logueado con el del ini, no cintar la pass guardada
                if (UserLog != USER_ini)
                    PassCorrecto = tools.BuscaValor_LINQ(SplashWindow.DTLogin, "Usuario", UserLog, "Password");
                ///Comparacion de contraseñas
                if (PassLog == PassCorrecto)
                {
                    Dispatcher.Invoke(((Action)(() => lblStatus.Content = "Iniciando sistema...")));
                    Pages.SplashWindow.User_NombreCompleto = tools.BuscaValor_LINQ(SplashWindow.DTLogin, "Usuario", UserLog.ToLower(), "Tipo");// SplashWindow.DTLogin.Rows[0]["Tipo"].ToString();
                    ///Animación de GIF
                    Dispatcher.Invoke(((Action)(() => da = new DoubleAnimation(0, 389, new Duration(TimeSpan.FromSeconds(3))))));
                    Dispatcher.Invoke(((Action)(() => rt = new RotateTransform())));
                    Dispatcher.Invoke(((Action)(() => imageSPARK.RenderTransform = rt)));
                    Dispatcher.Invoke(((Action)(() => imageSPARK.RenderTransformOrigin = new Point(0.474, 0.501))));
                    Dispatcher.Invoke(((Action)(() => da.RepeatBehavior = RepeatBehavior.Forever)));
                    Dispatcher.Invoke(((Action)(() => rt.BeginAnimation(RotateTransform.AngleProperty, da))));
                    SplashWindow.Password = PassLog;
                    Dispatcher.Invoke(((Action)(() => imgPass.Source = new BitmapImage(passCorrect))));
                    Thread.Sleep(2500);
                    //-----
                    DateTime TimpoActual = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                    tools.GuardarDatoEnIni("USUARIOACTUAL", UserLog);
                    tools.GuardarDatoEnIni("PASSUSUACTUAL", PassLog);
                    tools.GuardarDatoEnIni("TIEMPODESESION", TimpoActual.ToString());
                    //-----           
                    Dispatcher.Invoke(((Action)(() => superMAINWINDOW = new MainWindow())));
                    Dispatcher.Invoke(((Action)(() => superMAINWINDOW.Show())));
                    ///Dispatcher.Invoke(((Action)(() => asd.Show())));
                    Dispatcher.Invoke(((Action)(() => this.Hide())));
                }
                else
                {
                    ///Animación de GIF
                    Dispatcher.Invoke(((Action)(() => da = new DoubleAnimation(0, 389, new Duration(TimeSpan.FromSeconds(3))))));
                    Dispatcher.Invoke(((Action)(() => rt = new RotateTransform())));
                    Dispatcher.Invoke(((Action)(() => imageSPARK.RenderTransform = rt)));
                    Dispatcher.Invoke(((Action)(() => imageSPARK.RenderTransformOrigin = new Point(0.474, 0.501))));
                    Dispatcher.Invoke(((Action)(() => da.RepeatBehavior = RepeatBehavior.Forever)));
                    ///-----Dispatcher.Invoke(((Action)(() => imageSPARK.BeginAnimation(RotateTransform.AngleProperty, null))));
                    ///
                    bool activado = false;
                    Dispatcher.Invoke(((Action)(() => activado = this.Activate())));
                    if (!activado)
                    {
                        Dispatcher.Invoke(((Action)(() => this.Show())));
                    }
                    Dispatcher.Invoke(((Action)(() => imgPass.Source = new BitmapImage(passWrong))));
                    Dispatcher.Invoke(((Action)(() => lblStatus.Content = "Password incorrecto")));
                }
            }
            else
            {
                Dispatcher.Invoke(((Action)(() => imgUser.Source = new BitmapImage(userInCorrect))));
                Dispatcher.Invoke(((Action)(() => imgPass.Source = new BitmapImage(passWrong))));
                Dispatcher.Invoke(((Action)(() => lblStatus.Content = "Usuario y pass incorrecto")));
            }
        }

        //BOTON DE CERRAR
        private void Cerrar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.Shutdown();
        }

        //AL INGRESAR AL TXTPASSWORD
        private void txtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            bnd = false;
        }

        //CERRANDO
        private void Window_Closing(object sender, CancelEventArgs e)
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

        #region •••••• HILO DINAMICO ••••••

        //•••••••••••••••••• HILO DINAMICO by JORGE NUÑEZ ••••••••••••••••••

        //METODO QUE OFRECE EJECUTA OPERACIONES EN FORMA ASINCRÓNICA
        public void HiloDeEjecucion(string METODO)
        {
            this.Metodo = METODO;
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += FuncionPrincipalHILO;
            worker.RunWorkerCompleted += HiloTerminado;
            worker.RunWorkerAsync();
        }

        //HILO TERMINADO
        public void HiloTerminado(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        //FUNCION HILO
        string Metodo = "";
        public void FuncionPrincipalHILO(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(((Action)(() => aviso = new Pages.Aviso())));
            try
            {
                switch (this.Metodo)
                {
                    case "Iniciando sistema": IniciandoSistema(sender, e); break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                tools.MessageShowOK_2("Error: \n" + ex.Message.ToString(), "ERROR");
            }
            finally
            {

            }
        }

        #endregion

        //INICIANDO SISTEMA CORRECTAMENTE
        DoubleAnimation da = null;
        RotateTransform rt = null;
        public void IniciandoSistema(object sender, DoWorkEventArgs e)
        {
            string Usuario = "";
            string Password = "";
            Dispatcher.Invoke(((Action)(() => lblStatus.Content = "Verificando datos...")));
            Thread.Sleep(1000);
            ///
            Dispatcher.Invoke(((Action)(() => Usuario = txtUser.Text.ToString())));
            Dispatcher.Invoke(((Action)(() => Password = txtPass.Password.ToString())));
            Iniciar(Usuario, Password);
        }

        //AL CARGAR EL FORMULARIO
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblVersion.Content = Pages.SplashWindow.versionLBL;
        }

        //INFO DEL USER
        public static String NOMBRE_ini = "";
        public static String APODO_ini = "";
        public static String CORREO_ini = "";
        public static String PASS_ini = "";
        public static String USER_ini = "";
        public static String EJECUCIONES_ini = "";
        public void InfoUSer(string USER)
        {
            string ArchivoUser = Pages.SplashWindow.RutaNubotVersiones + @"\InfoUSER_Nubot.inf";
            if (!File.Exists(ArchivoUser))
            {
                File.AppendAllText(ArchivoUser, "------------------------------ INFO USUARIO DE NUBOT ------------------------------" + Environment.NewLine +
                    "NOMBRE=" + Environment.NewLine +
                    "APODO=" + Environment.NewLine +
                    "CORREO=" + Environment.NewLine +
                    "USUARIO=" + USER + Environment.NewLine +
                    "PASS=" + Environment.NewLine +
                    "EJECUCIONES=" + Environment.NewLine
                    );
                Thread.Sleep(100);
            }
            if (File.Exists(ArchivoUser))
            {
                NOMBRE_ini = tools.ObtenerDatoDeArchivo(ArchivoUser, "NOMBRE").Replace("\n", "").Replace("\r", "");
                APODO_ini = tools.ObtenerDatoDeArchivo(ArchivoUser, "APODO").Replace("\n", "").Replace("\r", "");
                CORREO_ini = tools.ObtenerDatoDeArchivo(ArchivoUser, "CORREO").Replace("\n", "").Replace("\r", "");
                PASS_ini = tools.ObtenerDatoDeArchivo(ArchivoUser, "PASS").Replace("\n", "").Replace("\r", "");
                USER_ini = tools.ObtenerDatoDeArchivo(ArchivoUser, "USUARIO").Replace("\n", "").Replace("\r", "");
                EJECUCIONES_ini = tools.ObtenerDatoDeArchivo(ArchivoUser, "EJECUCIONES").Replace("\n", "").Replace("\r", "");
            }
        }

        //BOTON INICIAR SESION

        private void btnIniciar_MouseDown(object sender, RoutedEventArgs e)
        {
            HiloDeEjecucion("Iniciando sistema");
        }

    }
}
