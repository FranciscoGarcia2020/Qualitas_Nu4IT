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
using System.IO;
using System.Data;
using System.Net;
using MaterialDesignThemes.Wpf;

namespace QPil.Pages.Usuario
{
    public partial class MiCuenta : Window
    {
        //VARIABLES GLOBALES
        Metodos tools = new Metodos();
        string ArchivoUser = Pages.SplashWindow.RutaNubotVersiones + @"\InfoUSER_Nubot.inf";


        //INICIAR COMPONENTES
        public MiCuenta()
        {
            InitializeComponent();

        }

        //AL CARGAR LA VENTANA
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUser.Text = Pages.SplashWindow.User;
            txtContra1.Text = Pages.SplashWindow.Password;
            ///
            if (!File.Exists(ArchivoUser))
            {
                File.AppendAllText(ArchivoUser, "------------------------------ INFO USUARIO DE NUBOT ------------------------------" + Environment.NewLine +
                    "NOMBRE=" + Environment.NewLine +
                    "APODO=" + Environment.NewLine +
                    "CORREO=" + Environment.NewLine +
                    "USUARIO=" + Pages.SplashWindow.User + Environment.NewLine +
                    "PASS=" + Pages.SplashWindow.Password + Environment.NewLine +
                    "EJECUCIONES=" + Environment.NewLine
                    );
            }
        }

        //GUARDAR LOS CAMBIOS
        private void btnGuardarContra_Click(object sender, RoutedEventArgs e)
        {
            if (tools.GuardarDatoDeArchivo(ArchivoUser, "PASS", txtContra1.Text.Replace("\n", "").Replace("\r", "")))
            {
                tools.GuardarDatoDeArchivo(ArchivoUser, "USUARIO", Pages.SplashWindow.User);
                if (tools.ObtenerDatoDeArchivo(ArchivoUser, "PASS") == txtContra1.Text.Replace("\n", "").Replace("\r", ""))
                {
                    Pages.SplashWindow.Password = txtContra1.Text.Replace("\n", "").Replace("\r", "");
                    tools.MessageShowOK_2("Datos guardados correctamente!", "OK");
                }
                else
                    tools.MessageShowOK_2("Error al guardar los datos!", "ERROR");
            }
        }

        #region DESIGN DD

        //AL MINIMIZAR
        private void btnMinimizar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        //AL MAXIMIZAR O RESTAURAR EL TAMAÑO DE LA VENTANA
        bool Maximi = false;
        double left = 0, top = 0, width = 0, height = 0;
        private void btnMaximResta_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized || Maximi)
            {
                btnMaximResta.Child = new PackIcon() { Kind = PackIconKind.WindowMaximize, Height = 27, Width = 27, Foreground = Metodos.Blanco };
                this.Left = left;
                this.Top = top;
                this.Width = width;
                this.Height = height;
                Maximi = false;
            }
            else
            {
                btnMaximResta.Child = new PackIcon() { Kind = PackIconKind.WindowRestore, Height = 27, Width = 27, Foreground = Metodos.Blanco };
                this.Left = 0;
                this.Top = 0;
                this.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Width;
                this.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height - 40;
                Maximi = true;
            }
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
        }

        //AL DAR CLICK EN CERRAR
        private void btnCerrar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        //AL DAR CLICK AL ENCABEZADO
        private void gridHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try { this.DragMove(); }
            catch (Exception) { }
        }

        //AL TECLEAR EN LA VENTANA
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                this.Close();
        }

        #endregion
    }
}
