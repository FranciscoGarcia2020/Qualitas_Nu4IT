using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QPil.Resources
{
    /// <summary>
    /// Interaction logic for WindowUniversal.xaml
    /// </summary>
    public partial class WindowUniversal : Window
    {
        dynamic classs = (dynamic)null;
        public static double widthActual = 0;
        public static double heigthActual = 0;
        public static double topActual = 0;
        public static double leftthActual = 0;

        public WindowUniversal(dynamic UserControl)
        {
            InitializeComponent();
            this.classs = UserControl;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = classs.ToString();
            //this.Width = classs.Width + 17;
            //this.Height = classs.Height + 42;
            gridRoot.Children.Clear();
            gridRoot.Children.Add(classs);
        }

        //CUANDO SE ESTE CERRANDO EL MAIN WINDOW
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //Pages.Login.superMAINWINDOW.gridPrincipal.Children.Add(classs);
            }
            catch (Exception ex)
            {

            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WindowUniversal.widthActual = this.Width;
            WindowUniversal.heigthActual = this.Height;
            WindowUniversal.topActual = this.Top;
            WindowUniversal.leftthActual = this.Left;
        }
    }
}
