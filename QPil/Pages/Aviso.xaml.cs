#region lIBRERIAS Y REFERENCIAS

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
using System.Threading;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Diagnostics;

#endregion

namespace QPil.Pages
{
    /// <summary>
    /// Interaction logic for Aviso.xaml
    /// By Jorge Núñez 
    /// ENERO 2017
    /// MARZO 2018
    /// </summary>
    public partial class Aviso : Window
    {

        #region INICIO DE COMPONENTES

        public static string PAUSACANCEL = "";

        //
        public Aviso()
        {
            InitializeComponent();
            imgCancelarHilo.Visibility = Visibility.Collapsed;
            pgbStatus.Visibility = Visibility.Collapsed;
        }

        //
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                bbnd = true;
                this.DragMove();
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region MINIMIZAR

        //
        private void imgMinimiza_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bbnd = true;
            Minimiza();
        }

        //
        public void Minimiza()
        {
            MainWindow.AvisosAbiertos--;
            ///Creando hilo
            ThreadStart delegado = new ThreadStart(HiloCerrar);
            Thread hilo = new Thread(delegado);
            hilo.Start();

            System.Windows.Media.Animation.Storyboard storyboard = this.FindResource("Unloaded") as System.Windows.Media.Animation.Storyboard;
            storyboard.Begin();
        }

        /// <summary>
        /// 
        /// </summary>
        public void HiloMinimizar()
        {
            Thread.Sleep(1200);
            Dispatcher.Invoke(((Action)(() => this.WindowState = System.Windows.WindowState.Minimized)));
        }

        #endregion

        #region CERRAR

        //
        private void imgCancelarHilo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bbnd = true;
            Dispatcher.Invoke(((Action)(() => Cerrar())));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Cerrar()
        {
            MainWindow.AvisosAbiertos--;
            ///Creando hilo
            ThreadStart delegado = new ThreadStart(HiloCerrar);
            Thread hilo = new Thread(delegado);
            hilo.Start();
            System.Windows.Media.Animation.Storyboard storyboard = this.FindResource("Unloaded") as System.Windows.Media.Animation.Storyboard;
            storyboard.Begin();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Cerrar2()
        {
            for (int i = 0; i < 110; i++)
            {
                Dispatcher.Invoke(((Action)(() => this.Height = 110 - i)));
                Dispatcher.Invoke(((Action)(() => this.Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height - 150 + i)));
                if (i == 105)
                {
                    Dispatcher.Invoke(((Action)(() => this.Close())));
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void HiloCerrar()
        {
            Thread.Sleep(1200);
            Dispatcher.Invoke(((Action)(() => this.Close())));
        }

        private void AvisoWindow_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region ABRIR 

        //AL CARGAR EL FORMULARIO
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            int numVent = MainWindow.AvisosAbiertos;
            this.WindowStartupLocation = WindowStartupLocation.Manual;

            int T = 135;
            if (numVent == 0)
                T = 150;
            else if (numVent > 0)
                T = T * (numVent + 1);
            else
            {
                T = 150;
                MainWindow.AvisosAbiertos = 0;
            }




            //List<string> ListaVentanas = new List<string>();
            //Process[] myProcesses = Process.GetProcesses();
            //foreach (Process myPr in myProcesses)
            //{
            //    ListaVentanas.Add(myPr.MainWindowTitle);
            //    if (myPr.MainWindowTitle.ToString().ToUpper().Contains("AVISO NU4IT"))
            //    {
            //        ShowWindow(myPr.Handle, 4);
            //        break;
            //    }
            //}

            this.Left = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Width - 350;
            this.Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height - T;
            lblVersion.Content = "QPil:  " + SplashWindow.versionLBL;
            ///
            Dispatcher.Invoke(((Action)(() => this.Abrir())));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Abrir()
        {
            MainWindow.AvisosAbiertos++;
            System.Windows.Media.Animation.Storyboard storyboard = this.FindResource("Loaded") as System.Windows.Media.Animation.Storyboard;
            storyboard.Begin();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Abrir2()
        {
            for (int i = 0; i < 110; i++)
            {
                Dispatcher.Invoke(((Action)(() => this.Height = i)));
                Dispatcher.Invoke(((Action)(() => this.Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height - 40 - i)));

            }
        }

        #endregion

        #region PAUSA

        //
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (PAUSACANCEL == "DETENIDO")
            {
                imgPausa.Visibility = System.Windows.Visibility.Collapsed;
                imgUpdating.Visibility = System.Windows.Visibility.Visible;
                pgbStatus.Visibility = System.Windows.Visibility.Visible;
                PAUSACANCEL = "SIGUE";
            }
            else if (PAUSACANCEL == "SIGUE")
            {
                imgPausa.Visibility = System.Windows.Visibility.Visible;
                imgUpdating.Visibility = System.Windows.Visibility.Collapsed;
                pgbStatus.Visibility = System.Windows.Visibility.Collapsed;
                PAUSACANCEL = "DETENIDO";
            }
            else if (PAUSACANCEL == "CANCELADO")
            {

            }
        }

        #endregion

        #region APIS WINDOWS Y HANDELS

        //
        protected delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        protected static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        protected static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")]
        protected static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        protected static bool EnumTheWindows(IntPtr hWnd, IntPtr lParam)
        {
            int tamano = GetWindowTextLength(hWnd) + 1;
            StringBuilder sb = new StringBuilder(tamano);
            GetWindowText(hWnd, sb, tamano);
            Console.WriteLine(sb.ToString());
            return true;
        }

        //METODO PARA ATIVAR UNA VENTANA : DANIEL SANCHEZ
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public void ActivaVentana(string proceso, string _proceso, string title)
        {
            var procesos = Process.GetProcesses();
            var prc = Process.GetProcessesByName(proceso);
            Process[] localByName = Process.GetProcessesByName(_proceso);
            for (int i = 0; i < localByName.Length; i++)
            {
                if (localByName[i].MainWindowTitle.ToUpper().Contains(title.ToUpper()))
                {
                    SetForegroundWindow(localByName[i].MainWindowHandle);
                }
                if (localByName[i].MainWindowTitle.ToUpper().Contains(title.ToUpper()))
                {
                    SetForegroundWindow(localByName[i].MainWindowHandle);
                }
            }
            if (localByName.Length == 0)
            {
                foreach (var item in procesos)
                {
                    Console.WriteLine(item.ProcessName.ToString().ToUpper());
                    if (item.ProcessName.ToString().ToUpper().Contains(proceso.ToUpper()))
                    {
                        Dispatcher.Invoke(((Action)(() => SetForegroundWindow(item.MainWindowHandle))));
                        Dispatcher.Invoke(((Action)(() => ShowWindow(item.MainWindowHandle, 4))));
                        //break;
                    }
                }
            }
        }

        //MAXIMIZAR MIIE : JORGE NUÑEZ
        /// Hide = 0;
        /// Show Normal = 1;
        /// Minimize Window = 2;
        /// Maximize Window = 3
        /// Activate Window = 4;
        /// Restore Window = 9;
        /// SW_SHOW = 5;
        /// Default Window = 10;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        #endregion

        //SI LA NOTIFICACION FUE DADO UN CLICK
        bool bbnd = false;
        private void AvisoWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!bbnd)
            {
            }
        }

        private void txtStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
