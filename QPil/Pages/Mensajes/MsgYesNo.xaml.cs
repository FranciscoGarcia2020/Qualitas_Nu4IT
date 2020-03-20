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

namespace QPil.Pages.Mensajes
{
    /// <summary>
    /// Interaction logic for MnsjOK.xaml
    /// By: Jorge Nuñez
    /// Date: 23 de Abril de 2018
    /// </summary>
    public partial class MsgYesNo : Window
    {
        //INICIO DE COMPONENTES
        public MsgYesNo()
        {
            InitializeComponent();
            this.Focusable = true;
            this.Focus();
            btnIniciar.Focusable = true;
            btnIniciar.Focus();
        }

        //OK
        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            Button boton = sender as Button;
            if (boton.Content.ToString() == "SI")
            {
                Pages.SplashWindow.RespuestaYesNo = true;
            }
            if (boton.Content.ToString() == "NO")
            {
                Pages.SplashWindow.RespuestaYesNo = false;
            }
            this.Hide();
        }

        //OK O ESC
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SplashWindow.RespuestaYesNo = true;
                this.Hide();
            }
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                SplashWindow.RespuestaYesNo = false;
                this.Hide();
            }
            if (e.Key == System.Windows.Input.Key.Y)
            {
                SplashWindow.RespuestaYesNo = true;
                this.Hide();
            }
            if (e.Key == System.Windows.Input.Key.N)
            {
                SplashWindow.RespuestaYesNo = false;
                this.Hide();
            }
        }

        //AL CARGAR EL FORMULARIO
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ///------ Animación de Loaded --------
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            (this.Content as Grid).BeginAnimation(Grid.OpacityProperty, new System.Windows.Media.Animation.DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(.18))));
            (this.Content as Grid).BeginAnimation(Grid.WidthProperty, new System.Windows.Media.Animation.DoubleAnimation(0, (this.Content as Grid).ActualWidth, new Duration(TimeSpan.FromSeconds(.18))));
            (this.Content as Grid).BeginAnimation(Grid.HeightProperty, new System.Windows.Media.Animation.DoubleAnimation(0, (this.Content as Grid).ActualHeight, new Duration(TimeSpan.FromSeconds(.18))));
            ///-----------------------------------
            SplashWindow.RespuestaYesNo = false;
            Dispatcher.Invoke(((Action)(() => this.lblNu4it.Content = "QPil " + Pages.SplashWindow.versionLBL)));
        }

        //MOVER LA VENTANA
        private void gridHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception) { }
        }

        //CERRAR
        private void btnCerrar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
