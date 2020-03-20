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
    /// Interaction logic for Opciones.xaml
    /// Arturo Eden Aragon Sanchez
    /// </summary>
    public partial class ComboBox : Window
    {
        public ComboBox()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.Beep();
            this.Width = lblAvisoContent.Width / 5;
            this.Height = lblAvisoContent.Height / 5;
            Dispatcher.Invoke(((Action)(() => this.Title = "Aviso - QPil Wal - Mart " + Pages.SplashWindow.versionLBL)));
            Dispatcher.Invoke(((Action)(() => this.lblNu4it.Content = "Aviso - QPil Wal - Mart " + Pages.SplashWindow.versionLBL)));
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }


        private void btnCerrar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Hide();
            }
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.Hide();
            }
        }

        private void gridHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception) { }
        }





    }
}
