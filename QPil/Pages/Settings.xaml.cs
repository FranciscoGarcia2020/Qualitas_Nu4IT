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
using System.Threading;
using System.IO;
using System.Windows.Media.Animation;

namespace QPil.Pages
{
    public partial class Settings : UserControl
    {

        #region ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬ DISEÑO ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        Label lblAnteriror = new Label();
        private void MouseEnterLbl(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            if (lblAnteriror != label)
            {
                label.Background = Application.Current.Resources["PrimaryHueMidBrush"] as SolidColorBrush;
                label.BorderBrush = Metodos.Azul;
            }
        }

        private void MouseLeaveLbl(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            if (lblAnteriror != label)
            {
                label.Background = Metodos.Vacio;
                label.BorderBrush = Metodos.Vacio;
            }
        }

        private void MouseDownLbl(object sender, MouseButtonEventArgs e)
        {
            Label label = sender as Label;
            if (lblAnteriror != label)
            {
                lblAnteriror.Background = Metodos.Vacio;
                lblAnteriror.BorderBrush = Metodos.Vacio;
                label.Background = Application.Current.Resources["PrimaryHueMidBrush"] as SolidColorBrush;
                //label.BorderBrush = Application.Current.Resources["PrimaryHueLightBrush"] as SolidColorBrush;
                lblAnteriror = label;
            }
            SeleccionandoOpcion(label.Content.ToString());
        }

        #endregion

        //VARIBLES GLOBALES
        Metodos tools = new Metodos();
        List<string> Herramientas = new List<string>();

        //INICIANDO COMPONENTES
        public Settings()
        {
            InitializeComponent();
            //Lista de opciones o paginas de navegacion
            Herramientas = new List<string>();
            Herramientas.Add("Mi Cuenta");
            Herramientas.Add("Utilidades");
            //Agregando las opciones
            AgregarBoton(Herramientas);
        }

        //SELECCIONANDO OPCION
        public void SeleccionandoOpcion(string Contetnido)
        {
            switch (Contetnido.ToUpper())
            {
                case "MI CUENTA":
                    Pages.Usuario.MiCuenta micuenta = new Pages.Usuario.MiCuenta();
                    micuenta.ShowDialog();
                    break;
                case "AYUDA":
                    tools.MessageShowOK_2("¿Necesitas ayuda?\n\nEnvia un correo a:\njorge.nunez@bestcollect.com.mx");
                    break;
                case "UTILIDADES":
                    Pages.Utillities utili = new Pages.Utillities("");
                    utili.Show();
                    break;
                case "CERRAR SESIÓN":
                    //if (MessageBox.Show("¿Esta seguro de cerrar sesión?", "Cerrar Sesión", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    if (tools.MessageShowYesNo_2("¿Esta seguro de cerrar sesión?"))
                    {
                        DateTime timepoSesion = Convert.ToDateTime(tools.ObtenerDatoDeIni("TIEMPODESESION"));
                        DateTime tiempoNuevo = Convert.ToDateTime("00:00:00");
                        tools.GuardarDatoEnIni("TIEMPODESESION", tiempoNuevo.ToString());
                        //Borrando el Login y licencia
                        if (File.Exists(Directory.GetCurrentDirectory() + @"\Login.txt"))
                            File.Delete(Directory.GetCurrentDirectory() + @"\Login.txt");
                        if (File.Exists(Directory.GetCurrentDirectory() + @"\Licence.txt"))
                        {
                            tools.DesProtegerArchivo(Directory.GetCurrentDirectory() + @"\Licence.txt");
                            File.Delete(Directory.GetCurrentDirectory() + @"\Licence.txt");
                        }
                        //Reinciando
                        string exe = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Nü4it Automation\QPil.appref-ms";
                        if (File.Exists(exe))
                            System.Diagnostics.Process.Start(exe);
                        else
                            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        Dispatcher.Invoke(((Action)(() => App.Current.Shutdown())));
                    }
                    break;
                default: break;
            }
        }

        //AGREGANDO BOTONES CON LA MISMA APARIENCIA
        public void AgregarBoton(List<string> ListaBots)
        {
            int vacios = 0;
            foreach (var item in ListaBots)
            {
                if (item != " ")
                {
                    Label label = new Label();
                    label.Name = "btn" + item.Replace(" ", "").Replace("&", "");
                    label.Content = item;
                    label.HorizontalAlignment = HorizontalAlignment.Left;
                    label.VerticalAlignment = VerticalAlignment.Top;
                    label.Width = 168;
                    label.Background = Metodos.Vacio;
                    label.Foreground = Metodos.Blanco;
                    label.BorderBrush = Metodos.Vacio;
                    label.Height = 30;
                    label.FontWeight = FontWeights.Normal;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;
                    label.MouseEnter += MouseEnterLbl;
                    label.MouseLeave += MouseLeaveLbl;
                    label.MouseDown += MouseDownLbl;
                    PanelRobots.Children.Add(label);
                    PanelRobots.UpdateLayout();
                }
                if (item == " ")
                {
                    vacios++;
                    Label label = new Label();
                    label.Name = "btnVacio" + vacios;
                    label.HorizontalAlignment = HorizontalAlignment.Left;
                    label.VerticalAlignment = VerticalAlignment.Top;
                    label.Width = 150;
                    label.Background = Metodos.Vacio;
                    label.Foreground = Metodos.Blanco;
                    label.BorderBrush = Metodos.Vacio;
                    label.Height = 30;
                    label.FontWeight = FontWeights.Normal;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;
                    PanelRobots.Children.Add(label);
                    PanelRobots.UpdateLayout();
                }


            }
        }

        private void GridMenuArea_MouseLeave(object sender, MouseEventArgs e)
        {

        }

    }
}
