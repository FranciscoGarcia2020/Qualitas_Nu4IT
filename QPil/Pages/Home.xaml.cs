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
using System.Windows.Navigation;
using System.Windows.Shapes;
///Animaciones
using System.Windows.Media.Animation;

namespace QPil.Pages
{
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
            grdNovedades.Opacity = 0;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Pages.SplashWindow.Novedades != "")
            {
                lblVersion.Content = Pages.SplashWindow.versionLBL;
                lblAvisoNovedades.Content = Pages.SplashWindow.Novedades;
                DoubleAnimation animacionOPACIDAD = null;
                Dispatcher.Invoke(((Action)(() => animacionOPACIDAD = new DoubleAnimation(0, 0.7, new Duration(TimeSpan.FromSeconds(.3))))));
                Dispatcher.Invoke(((Action)(() => grdNovedades.BeginAnimation(Grid.OpacityProperty, animacionOPACIDAD))));
                aTimer = new System.Timers.Timer(1000);
                aTimer.Elapsed += OnTimedEvent;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else
            {
                grdNovedades.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        #region NOVEDADES

        int counter = 0;
        public System.Timers.Timer aTimer;
        //CRONOMETRO
        public void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            counter++;
            if (counter == 2)
            {
                try
                {
                    Expandir();
                }
                catch (Exception ex)
                {

                }
            }
            if (counter == 6)
            {
                try
                {
                    Contraer();
                    counter = 0;
                    Dispatcher.Invoke(((Action)(() => aTimer.Stop())));
                    Dispatcher.Invoke(((Action)(() => aTimer.Enabled = false)));
                }
                catch (Exception)
                {


                }
            }
        }

        bool Expandido = false;
        private void imgBoton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Expandido)
                Contraer();
            else
                Expandir();
        }

        public void Expandir()
        {
            DoubleAnimation animacionOPACIDAD = null;
            Dispatcher.Invoke(((Action)(() => animacionOPACIDAD = new DoubleAnimation(0.7, 1, new Duration(TimeSpan.FromSeconds(.3))))));
            Dispatcher.Invoke(((Action)(() => grdNovedades.BeginAnimation(Grid.OpacityProperty, animacionOPACIDAD))));
            DoubleAnimation animacionALTURA = null;
            Dispatcher.Invoke(((Action)(() => animacionALTURA = new DoubleAnimation(31, 200, new Duration(TimeSpan.FromSeconds(.3))))));
            Dispatcher.Invoke(((Action)(() => grdNovedades.BeginAnimation(Grid.HeightProperty, animacionALTURA))));
            DoubleAnimation animacionANCHO = null;
            Dispatcher.Invoke(((Action)(() => animacionANCHO = new DoubleAnimation(35, 500, new Duration(TimeSpan.FromSeconds(.3))))));
            Dispatcher.Invoke(((Action)(() => grdNovedades.BeginAnimation(Grid.WidthProperty, animacionANCHO))));
            ///Dispatcher.Invoke(((Action)(() => imgBoton.Source = new BitmapImage(new Uri(@"/QPil;component/Resources/Imagenes/QPil/minimiza32.png", UriKind.RelativeOrAbsolute)))));
            Dispatcher.Invoke(((Action)(() => lblEtiquetamNov.Foreground = Metodos.Blanco)));
            Dispatcher.Invoke(((Action)(() => lblVersion.Foreground = Metodos.Blanco)));
            Dispatcher.Invoke(((Action)(() => dckAviso.Visibility = Visibility.Visible)));
            Dispatcher.Invoke(((Action)(() => grdNovedades.HorizontalAlignment = HorizontalAlignment.Right)));
            Dispatcher.Invoke(((Action)(() => grdNovedades.VerticalAlignment = VerticalAlignment.Top)));
            Expandido = true;
        }

        public void Contraer()
        {
            DoubleAnimation animacionOPACIDAD = null;
            Dispatcher.Invoke(((Action)(() => animacionOPACIDAD = new DoubleAnimation(1, 0.7, new Duration(TimeSpan.FromSeconds(.3))))));
            Dispatcher.Invoke(((Action)(() => grdNovedades.BeginAnimation(Grid.OpacityProperty, animacionOPACIDAD))));
            DoubleAnimation animacionALTURA = null;
            Dispatcher.Invoke(((Action)(() => animacionALTURA = new DoubleAnimation(200, 31, new Duration(TimeSpan.FromSeconds(.3))))));
            Dispatcher.Invoke(((Action)(() => grdNovedades.BeginAnimation(Grid.HeightProperty, animacionALTURA))));
            DoubleAnimation animacionANCHO = null;
            Dispatcher.Invoke(((Action)(() => animacionANCHO = new DoubleAnimation(500, 35, new Duration(TimeSpan.FromSeconds(.3))))));
            Dispatcher.Invoke(((Action)(() => grdNovedades.BeginAnimation(Grid.WidthProperty, animacionANCHO))));
            ///Dispatcher.Invoke(((Action)(() => imgBoton.Source = new BitmapImage(new Uri(@"/QPil;component/Resources/Imagenes/Botones/add32.png", UriKind.RelativeOrAbsolute)))));
            Dispatcher.Invoke(((Action)(() => lblEtiquetamNov.Foreground = Metodos.Blanco)));
            Dispatcher.Invoke(((Action)(() => lblVersion.Foreground = Metodos.Blanco)));
            Dispatcher.Invoke(((Action)(() => dckAviso.Visibility = Visibility.Collapsed)));
            Dispatcher.Invoke(((Action)(() => grdNovedades.HorizontalAlignment = HorizontalAlignment.Right)));
            Dispatcher.Invoke(((Action)(() => grdNovedades.VerticalAlignment = VerticalAlignment.Top)));
            Expandido = false;
        }

        private void grdNovedades_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void grdNovedades_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void grdNovedades_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void imgBoton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Expandido)
                    Expandir();
            }
            catch (Exception)
            {

            }
        }

        private void imgBoton_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Expandido)
                    Contraer();
            }
            catch (Exception)
            {

            }
        }

        #endregion

    }
}
