using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Animation;

namespace QPil.Pages
{
    public partial class Update : UserControl
    {
        Metodos tools = new Metodos();
        dynamic actu = (dynamic)null;

        public Update()
        {
            InitializeComponent();
            lblVersion.Content = Pages.SplashWindow.versionLBL;
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            SplashWindow.ActualizacionCorrecta = false;
            //Dispatcher.Invoke(((Action)(() => actu = new Pages.Descargar())));
            //Dispatcher.Invoke(((Action)(() => actu.ShowDialog())));
            //if (SplashWindow.ActualizacionCorrecta)
            //{
            //    Dispatcher.Invoke(((Action)(() => App.Current.Shutdown())));
            //    Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\QPil.lnk");
            //}
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string HayVersion = SplashWindow.HayActualizacion;
            lblAvisoContent.Text = HayVersion;
            if (HayVersion == "Tienes la versión mas reciente.")
            {
                btnActualizar.IsEnabled = false;
                btnActualizar.Content = "Actualizadó";
            }
            else
            {
                lblAvisoContent.Text = "Hay una nueva versión disponible, descargala ahora...";
                btnActualizar.IsEnabled = true;
                btnActualizar.Content = "Actualizar";
            }
        }

        private void GridMenuArea_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
