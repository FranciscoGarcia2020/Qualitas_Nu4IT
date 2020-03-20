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
    public partial class MnsjOK : Window
    {
        string Tipo = "X";

        //INICIO DE COMPONENTES
        public MnsjOK(string TipoVentana)
        {
            InitializeComponent();
            this.Tipo = TipoVentana;
            this.Focusable = true;
            this.Focus();
        }

        //OK
        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //OK O ESC
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Close();
            }
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.Close();
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
            ///Dispatcher.Invoke(((Action)(() => Console.Beep())));
            //this.Width = lblAvisoContent.Width / 5;
            //this.Height = lblAvisoContent.Height / 5;
            ///Dispatcher.Invoke(((Action)(() => this.Title = "Aviso - QPil " + Pages.SplashWindow.versionLBL)));
            Dispatcher.Invoke(((Action)(() => this.lblNu4it.Content = "QPil " + Pages.SplashWindow.versionLBL)));
            ///Tipo de ventana 
            string Rutaimagen = "";
            switch (this.Tipo.ToUpper())
            {
                case "OK": Rutaimagen = "/QPil;component/Resources/Imagenes/Botones/ok.png"; rctColor.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x4C, 0xAF, 0x50)); break;
                case "OK2": Rutaimagen = "/QPil;component/Resources/Imagenes/Botones/ok2.png"; rctColor.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x4C, 0xAF, 0x50)); break;
                case "ALERTA": Rutaimagen = "/QPil;component/Resources/Imagenes/Botones/alerta.png"; rctColor.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC1, 0x07)); break;
                case "ALERTA2": Rutaimagen = "/QPil;component/Resources/Imagenes/Botones/alerta2.png"; rctColor.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC1, 0x07)); break;
                case "ERROR": Rutaimagen = "/QPil;component/Resources/Imagenes/Botones/error.png"; rctColor.Fill = Metodos.btn_Rojo; break;
                default: Rutaimagen = "/QPil;component/Resources/Imagenes/Icons/logo-nu2.ico"; rctColor.Fill = Metodos.GrisOsc; break;
            }
            var imagen = new Uri(Rutaimagen, UriKind.RelativeOrAbsolute);
            imgTipoAviso.Source = new BitmapImage(imagen);
            imgFondo.Source = new BitmapImage(imagen);
            ///---Context menu
            ContextMenu cntMenu = new ContextMenu();
            MenuItem cmiCopiar = new MenuItem();
            cmiCopiar.Header = "Copiar texto";
            cmiCopiar.Click += (a, b) =>
            {
                try
                {
                    Clipboard.SetText(lblAvisoContent.Text.ToString());
                    MessageBox.Show("Info. copiada a portapapeles!");
                }
                catch (Exception ex)
                {

                }
            };
            cntMenu.Items.Add(cmiCopiar);
            lblAviso.ContextMenu = cntMenu;
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
