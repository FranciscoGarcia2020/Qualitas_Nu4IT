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

namespace QPil
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {

        //AL INICIAR EL EJECUTABLE
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata
                {
                    DefaultValue = FindResource(typeof(Window))
                });
            }
            catch (Exception ex)
            {

            }
            base.OnStartup(e);
        }

        #region ACCIONES DE VENTANA

        //AL MINIMIZAR
        private void btnMinimizar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Window WND = (sender as Border).TemplatedParent as Window;
            WND.WindowState = WindowState.Minimized;
        }

        //AL DAR CLICK AL ENCABEZADO
        private void gridHeader_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Window WND = (((sender as Grid).Parent as Grid) as Grid).TemplatedParent as Window;
            try { WND.DragMove(); }
            catch (Exception) { }
        }

        //AL MAXIMIZAR O RESTAURAR EL TAMAÑO DE LA VENTANA
        bool Maximi = false;
        public double left = 0, top = 0, width = 0, height = 0;
        private void btnMaximResta_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Window WND = (sender as Border).TemplatedParent as Window;
            ///  inicio:; goto como lenguaje estructurado
            if (Maximi)
            {
                (sender as Border).Child = new PackIcon() { Kind = PackIconKind.WindowMaximize, Height = 27, Width = 27, Foreground = Metodos.Blanco };
                WND.ResizeMode = ResizeMode.CanResize;
                WND.WindowState = WindowState.Normal;
                WND.Left = left;
                WND.Top = top;
                WND.Width = width;
                WND.Height = height;
                WND.UpdateLayout();
                Maximi = false;
                ///  goto final;
            }
            else
            {
                ///  goto inicio;
                (sender as Border).Child = new PackIcon() { Kind = PackIconKind.WindowRestore, Height = 27, Width = 27, Foreground = Metodos.Blanco };
                WND.ResizeMode = ResizeMode.CanResize;
                WND.WindowState = WindowState.Maximized;
                WND.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Width;
                WND.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height - 40;
                WND.Left = 0;
                WND.Top = 0;
                WND.UpdateLayout();
                Maximi = true;
            }
            ///  final:;
        }

        //AL DAR CLICK EN CERRAR
        private void btnCerrar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Window WND = (sender as Border).TemplatedParent as Window;
            WND.Close();
        }

        //AL TECLEAR EN LA VENTANA
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                (sender as Window).Close();
        }

        #endregion

        #region BOTON DE HERRAMIENTA

        //
        private void btnSetti_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        //OPCIONES
        private void btnHerramientas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dynamic ventanapapa = (dynamic)null;
            ventanapapa = (sender as Border).TemplatedParent;
            string RootVntana = ventanapapa.ToString();
            Pages.Utillities uti = new Pages.Utillities(RootVntana);
            uti.Show();
        }

        #endregion

        #region ANIMACIONES

        //AL CARGAR EL GRID
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ///------ Animación de Loaded --------
            ///Ventana.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            (sender as Grid).BeginAnimation(Grid.OpacityProperty, new System.Windows.Media.Animation.DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(.18))));
            (sender as Grid).BeginAnimation(Grid.WidthProperty, new System.Windows.Media.Animation.DoubleAnimation(0, (sender as Grid).ActualWidth, new Duration(TimeSpan.FromSeconds(.18))));
            (sender as Grid).BeginAnimation(Grid.HeightProperty, new System.Windows.Media.Animation.DoubleAnimation(0, (sender as Grid).ActualHeight, new Duration(TimeSpan.FromSeconds(.18))));
            ///-----------------------------------
        }

        //AL CARGAR EL BORDER
        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            Window WND = (sender as Border).TemplatedParent as Window;
            ///Ventana.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ///------ Animación de Loaded --------
            (sender as Border).BeginAnimation(Border.OpacityProperty, new System.Windows.Media.Animation.DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(.18))));
            (sender as Border).BeginAnimation(Border.WidthProperty, new System.Windows.Media.Animation.DoubleAnimation(0, (sender as Border).ActualWidth, new Duration(TimeSpan.FromSeconds(.18))));
            (sender as Border).BeginAnimation(Border.HeightProperty, new System.Windows.Media.Animation.DoubleAnimation(0, (sender as Border).ActualHeight, new Duration(TimeSpan.FromSeconds(.18))));
            ///-----------------------------------
            Dispatcher.Invoke(((Action)(() => left = WND.Left)));
            Dispatcher.Invoke(((Action)(() => top = WND.Top)));
            Dispatcher.Invoke(((Action)(() => width = WND.Width)));
            Dispatcher.Invoke(((Action)(() => height = WND.Height)));
        }

        //INICIAR LA VENTANA
        private void BorderPrincipal_Initialized(object sender, EventArgs e)
        {
            (sender as Border).Opacity = 0;

        }
        #endregion
    }
}
