#region LIBRERIAS
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
//Librerias agregadas
using System.ComponentModel;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;
using System.Threading;
using System.Collections;
using System.Data;
using SHDocVw;
using Excel = Microsoft.Office.Interop.Excel;
using Outlook = Microsoft.Office.Interop.Outlook;
using Word = Microsoft.Office.Interop.Word;
#endregion

namespace QPil.Resources
{
    /// <summary>
    /// Interaction logic for VentanaNU4IT.xaml
    /// Ventana diseñada por: Jorge Nuñez
    /// </summary>
    public partial class VentanaNU4IT : Window
    {
        dynamic classs = (dynamic)null;

        #region DESIGN DD

        //AL MINIMIZAR
        private void btnMinimiza_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        //AL MAXIMIZAR O RESTAURAR EL TAMAÑO DE LA VENTANA
        bool Maximi = false;
        double left = 0, top = 0, width = 0, height = 0;
        private void btnMaximiResta_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized || Maximi)
            {
                var imagen = new Uri(@"/QPil;component/Resources/Imagenes/Nubot/maximiza32.png", UriKind.RelativeOrAbsolute);
                btnMaximiResta.Source = new BitmapImage(imagen);
                //this.WindowState = WindowState.Normal;
                this.Left = left;
                this.Top = top;
                this.Width = width;
                this.Height = height;
                Maximi = false;
            }
            else
            {
                var imagen = new Uri(@"/QPil;component/Resources/Imagenes/Nubot/restore32.png", UriKind.RelativeOrAbsolute);
                btnMaximiResta.Source = new BitmapImage(imagen);
                //this.WindowState = WindowState.Maximized;
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

        //AL DAR CLICK AL ENCABEZADO
        private void gridHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try { this.DragMove(); }
            catch (Exception) { }
        }

        //AL DAR CLICK EN CERRAR
        private void btnCerrar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        //AL TECLEAR EN LA VENTANA
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                this.Close();
        }

        #endregion

        //INICIACION DE COMPONENTES
        public VentanaNU4IT(dynamic UserControl, int TipoVentana, bool AllowsTransparency, System.Windows.WindowStyle EstiloVentana, string Titulo)
        {
            InitializeComponent();
            this.classs = UserControl;
            this.TipoWindow = TipoVentana;
            this.AllowsTransparency = AllowsTransparency;
            this.WindowStyle = EstiloVentana;
            this.lblTitle.Content = Titulo;
        }

        //AL CARGAR LOS COMPONENTES
        int TipoWindow = 0;
        private void VentanaNu4_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Left = classs.Left;
            //this.Top = classs.Top;


            Double tamAltoUserC = 600;
            Double tamAnchoUserC = 800;

            if (classs.ActualWidth == 0)
            {
                this.Width = tamAnchoUserC;
            }



            if (classs.ActualHeight == 0)
            {
                this.Height = tamAltoUserC;
            }


            left = this.Left;
            top = this.Top;
            width = this.Width;
            height = this.Height;
            ///Tipo Ventana     0: Normal, 1: NoMaximizar, 2: SoloCerrar
            if (TipoWindow == 0)
            {
                this.btnMinimiza.Visibility = Visibility.Visible;
                this.btnMaximiResta.Visibility = Visibility.Visible;
            }
            else if (TipoWindow == 1)
            {
                this.btnMinimiza.Visibility = Visibility.Hidden;
                this.btnMaximiResta.Visibility = Visibility.Visible;
            }
            else
            {
                this.btnMinimiza.Visibility = Visibility.Hidden;
                this.btnMaximiResta.Visibility = Visibility.Hidden;
            }
            //
            gridRoot.Children.Clear();
            gridRoot.Children.Add(classs);
        }



    }
}
