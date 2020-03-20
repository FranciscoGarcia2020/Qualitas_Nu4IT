#region LIBRERIAS Y REFERENCIAS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;

#endregion

namespace QPil
{
    /// <summary>
    /// Cliente: X
    /// By: Jorge Núñez
    /// Mantenimiento: X
    /// Updated : Abril 2018
    /// </summary>
    public partial class MainWindow : Window
    {

        //VARIABLES LOCALES
        public static int AvisosAbiertos = 0;
        string MENUSeleccionando = "";
        bool MenuExpandido = false;
        string UsuarioVisible = "";
        bool bndrea = false;
        public static string InfoProyecto = "Proyecto: QPil" + "\r\n" + "Cliente: X" + "\r\n" + "Area: X" + "\r\n" + "Desarrolladores: X";
        public static string[] ListaMenus = new string[]
        {
            "ADMINISTRADOR",
            "SISE",
            "QCONTENT",
            "OCR"
        };

        //OBJETOS LOCALES
        Metodos tools = new Metodos();

        #region DISEÑO PRINCIPAL DE LA VENTANA

        /*******************************************************************************************************
         *                  DISEÑO DE ANIMACION, ACCIONES Y METODOS PARA BOTONES DEL MENU SUPERIOR
         *******************************************************************************************************/

        //MOUSE OVER BOTON AZUL
        private void BotoAzulOver(object sender, MouseEventArgs e)
        {
            (sender as Border).Height = 24;
        }

        //MOUSE LEAVE VACIO
        private void BotonLeaveVacio(object sender, MouseEventArgs e)
        {
            (sender as Border).Height = 27;
        }

        //CERRAR
        private void btnCerar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.Shutdown();
        }

        //REINICIAR
        private void btnRestart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            tools.ReiniciarAccesoDirecto();
        }

        //BOTON DE MINIMIZAR
        private void btnMinimizar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        //BOTON DE HERRAMIENTAS
        private void btnSetti_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (gridMenu.Visibility == Visibility.Collapsed)
                    gridMenu.Visibility = Visibility.Visible;
                gridMenu.Children.Clear();
                gridMenu.BeginAnimation(Grid.HeightProperty, new DoubleAnimation(0, 390, new Duration(TimeSpan.FromSeconds(.1))));
                gridMenu.Children.Add(new Pages.Settings());
                ObjectAnimationUsingKeyFrames ok = new ObjectAnimationUsingKeyFrames();
                Storyboard.SetTarget(ok, gridMenu);
                Storyboard.SetTargetProperty(ok, new PropertyPath(Grid.VisibilityProperty));
                ok.KeyFrames.Add(new DiscreteObjectKeyFrame() { KeyTime = TimeSpan.FromSeconds(0.1), Value = Visibility.Visible });
            }
            catch (Exception)
            {

            }
        }

        //DESCARGAR ACTUALIZACIONES
        private void btnDownl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (gridMenu.Visibility == Visibility.Collapsed)
                    gridMenu.Visibility = Visibility.Visible;
                gridMenu.Children.Clear();
                gridMenu.BeginAnimation(Grid.HeightProperty, new DoubleAnimation(0, 390, new Duration(TimeSpan.FromSeconds(.1))));
                gridMenu.Children.Add(new Pages.Update());
                ObjectAnimationUsingKeyFrames ok = new ObjectAnimationUsingKeyFrames();
                Storyboard.SetTarget(ok, gridMenu);
                Storyboard.SetTargetProperty(ok, new PropertyPath(Grid.VisibilityProperty));
                ok.KeyFrames.Add(new DiscreteObjectKeyFrame() { KeyTime = TimeSpan.FromSeconds(0.1), Value = Visibility.Visible });
            }
            catch (Exception)
            {

            }
        }

        //AL MAXIMIZAR O RESTAURAR EL TAMAÑO DE LA VENTANA
        bool Maximi = false;
        public double left = 0, top = 0, width = 0, height = 0;
        private void btnMaximResta_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Maximi)
            {
                //var imagen = new Uri(@"/QPil;component/Resources/Imagenes/QPil/maximiza32.png", UriKind.RelativeOrAbsolute);
                //btnMaximResta.Source = new BitmapImage(imagen);
                btnMaximResta.Child = new PackIcon() { Kind = PackIconKind.WindowMaximize, Height = 27, Width = 27, Foreground = Metodos.Blanco };
                //this.WindowState = WindowState.Normal;
                this.Left = left;
                this.Top = top;
                this.Width = width;
                this.Height = height;
                Maximi = false;
            }
            else
            {
                //var imagen = new Uri(@"/QPil;component/Resources/Imagenes/QPil/restore32.png", UriKind.RelativeOrAbsolute);
                //btnMaximResta.Source = new BitmapImage(imagen);
                btnMaximResta.Child = new PackIcon() { Kind = PackIconKind.WindowRestore, Height = 27, Width = 27, Foreground = Metodos.Blanco };
                //this.WindowState = WindowState.Maximized;
                this.Left = 0;
                this.Top = 0;
                this.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Width;
                this.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height - 40;
                Maximi = true;
            }
        }

        //CUANDO SE ESTE CERRANDO EL MAIN WINDOWQ
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                System.Diagnostics.Process[] myProcesses;
                myProcesses = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process myProcess in myProcesses)
                    if (myProcess.ProcessName.ToString().ToUpper().Contains(Pages.SplashWindow.NombreNubot.ToUpper()))
                        myProcess.Kill();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*******************************************************************************************************
         *                           ACCIONES Y METODOS - BOTONES DEL MENU DE CLIENTES
         *******************************************************************************************************/

        //AL DARLE DOBLE CLICK AL LABEL "STATUS" PARA ABRIR EL MENU DE DESARROLLADORES
        private void STATUS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (tools.AbrirVentanaDESARROLLADOR())
            {
                deve = new Pages.Desarrollador.Developers(Pages.SplashWindow.TipoINFO);
                deve.ShowDialog();
            }
        }

        //ABRIR LA CARPETA DONDE ESTA INSALADO EL NUBOT
        private void txtstatus_Mousedobleclick(object sender, MouseButtonEventArgs e)
        {
            if (tools.AbrirVentanaDESARROLLADOR())
            {
                Process.Start(Directory.GetCurrentDirectory());
            }
        }

        static double STVleftDS = 0, STVtopDS = 0, STVwidthDS = 0, STVheightDS = 0;
        public static double leftDS
        {
            get { return STVleftDS; }
            set { STVleftDS = value; }
        }
        public static double topDS
        {
            get { return STVtopDS; }
            set { STVtopDS = value; }
        }
        public static double widthDS
        {
            get { return STVwidthDS; }
            set { STVwidthDS = value; }
        }
        public static double heightDS
        {
            get { return STVheightDS; }
            set { STVheightDS = value; }
        }

        ///Forma recortada..... public static double STVwidthDSassaass { get; set; }

        //AL CAMBIAR EL TAMAÑO DE VENTANA
        private void VentanaNu4MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.Height != 0.0)
            {
                scrPanelRobots.Height = this.Height - 282;
            }
            ValoresDeDiseño();
        }

        //AL DAR SELECCIONAR EL ENCABEZADO DEL CONTROL USER
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception)
            {

            }
            ValoresDeDiseño();
        }

        //
        private void VentanaNu4MainWindow_LocationChanged(object sender, EventArgs e)
        {
            ValoresDeDiseño();
        }

        //LEFT - TOP - WIDTH - HEIGTH
        public void ValoresDeDiseño()
        {
            try
            {
                Pages.Login.superMAINWINDOW.Width = this.Width;
                Pages.Login.superMAINWINDOW.Height = this.Height;
                Pages.Login.superMAINWINDOW.Top = this.Top;
                Pages.Login.superMAINWINDOW.Left = this.Left;
            }
            catch (Exception)
            {

            }
            try
            {
                STVleftDS = this.Left;
                STVtopDS = this.Top;
                STVwidthDS = this.Width;
                STVheightDS = this.Height;
            }
            catch (Exception)
            {


            }
        }

        //AL DARLE DOBLE CLICK A LA ORILLA DE LA VENTANA PARA MAXIMIZAR
        private void lblTitulo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnMaximResta_MouseDown(sender, e);
        }

        #endregion

        #region DISEÑO Y ANIMACION DE WINDOW

        //AL YA NO TENER EL FOCO DEL MOUSE
        private void gridMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                DoubleAnimation animacionALTURA = null;
                Dispatcher.Invoke(((Action)(() => animacionALTURA = new DoubleAnimation(390, 0, new Duration(TimeSpan.FromSeconds(.1))))));
                Dispatcher.Invoke(((Action)(() => gridMenu.BeginAnimation(Grid.HeightProperty, animacionALTURA))));
                ObjectAnimationUsingKeyFrames ok = new ObjectAnimationUsingKeyFrames();
                Storyboard.SetTarget(ok, gridMenu);
                Storyboard.SetTargetProperty(ok, new PropertyPath(Grid.VisibilityProperty));
                ok.KeyFrames.Add(new DiscreteObjectKeyFrame() { KeyTime = TimeSpan.FromSeconds(0.2), Value = Visibility.Collapsed });
                gridMenu.Children.Clear();
            }
            catch (Exception)
            {

            }
        }

        //MENU DE ROBOTS
        private void btnMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MenuExpandido)
            {
                ContraerMenu();
                MenuExpandido = false;
            }
            else
            {
                ExpanderMenu();
                MenuExpandido = true;
            }
        }

        //AL PASAR EL MOUSE ENCIMA
        private void PanelRobots_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        //AL PASAR EL MOUSE CLICK
        private void PanelRobots_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        //VARIABLES ANIMACION
        Thickness margin_A1 = new Thickness(190, 44, 0, 32);
        Thickness margin_B1 = new Thickness(55, 55, 0, 32);
        Thickness margin_A2 = new Thickness(190, 50, 5, 37);
        Thickness margin_B2 = new Thickness(55, 55, 5, 37);
        Thickness margin_A3 = new Thickness(190, 0, 0, 0);
        Thickness margin_B3 = new Thickness(3, 0, 0, 0);

        //ANIMACION DEL MENU
        public void ExpanderMenu()
        {
            if (!MenuExpandido)
            {
                //gridFONDO.BeginAnimation(Grid.MarginProperty, new ThicknessAnimation(margin_B1, margin_A1, new Duration(TimeSpan.FromSeconds(0.25))));
                //gridPrincipal.BeginAnimation(Grid.MarginProperty, new ThicknessAnimation(margin_B2, margin_A2, new Duration(TimeSpan.FromSeconds(0.25))));
                //StatusBar.BeginAnimation(Grid.MarginProperty, new ThicknessAnimation(margin_B3, margin_A3, new Duration(TimeSpan.FromSeconds(0.25))));
                //gridClientes.BeginAnimation(Grid.OpacityProperty, new DoubleAnimation(0.8, 1, new Duration(TimeSpan.FromSeconds(0.6))));
                ///imgUsuario.Margin = new Thickness(43, 10, 43, 10);
                imgUsuario.Margin = new Thickness(0, 5, 0, 0);
                imgUsuario.Width = 84;
                imgUsuario.Height = 84;
                gridClientes.BeginAnimation(Grid.WidthProperty, new DoubleAnimation(47, 190, new Duration(TimeSpan.FromSeconds(0.25))));
                PanelRobots.BeginAnimation(StackPanel.WidthProperty, new DoubleAnimation(47, 190, new Duration(TimeSpan.FromSeconds(0.25))));
                lblNombreVisible.Text = this.UsuarioVisible.ToUpper();
                ///rctGridClientes.BeginAnimation(Rectangle.OpacityProperty, new DoubleAnimation(1, 0.92, new Duration(TimeSpan.FromSeconds(0.5))));
                if (bndrea)
                {
                    rctFondo.Visibility = Visibility.Visible;
                    rctFondo.BeginAnimation(Rectangle.OpacityProperty, new DoubleAnimation(0, 0.3, new Duration(TimeSpan.FromSeconds(0.38))));
                    bndrea = true;
                }
                else
                    bndrea = true;
                MenuExpandido = true;
            }
        }

        //ANIMACION DEL MENU
        public void ContraerMenu()
        {
            //gridFONDO.BeginAnimation(Grid.MarginProperty, new ThicknessAnimation(margin_A1, margin_B1, new Duration(TimeSpan.FromSeconds(0.25))));
            //gridPrincipal.BeginAnimation(Grid.MarginProperty, new ThicknessAnimation(margin_A2, margin_B2, new Duration(TimeSpan.FromSeconds(0.25))));
            //StatusBar.BeginAnimation(Grid.MarginProperty, new ThicknessAnimation(margin_A3, margin_B3, new Duration(TimeSpan.FromSeconds(0.25))));
            // gridClientes.BeginAnimation(Grid.OpacityProperty, new DoubleAnimation(0.8, 1, new Duration(TimeSpan.FromSeconds(0.6))));
            ///imgUsuario.BeginAnimation(Grid.WidthProperty, new DoubleAnimation(85, , new Duration(TimeSpan.FromSeconds(0.25))));
            imgUsuario.Margin = new Thickness(0, 35, 0, 0);
            imgUsuario.Width = 40;
            imgUsuario.Height = 40;
            gridClientes.BeginAnimation(Grid.WidthProperty, new DoubleAnimation(gridClientes.ActualWidth, 47, new Duration(TimeSpan.FromSeconds(0.25))));
            PanelRobots.BeginAnimation(StackPanel.WidthProperty, new DoubleAnimation(PanelRobots.Width, 47, new Duration(TimeSpan.FromSeconds(0.25))));
            lblNombreVisible.Text = UsuarioVisible.Substring(0, 1).ToUpper();
            ///rctGridClientes.BeginAnimation(Rectangle.OpacityProperty, new DoubleAnimation(0.92, 1, new Duration(TimeSpan.FromSeconds(0.5))));
            rctFondo.Visibility = Visibility.Collapsed;
            rctFondo.BeginAnimation(Rectangle.OpacityProperty, new DoubleAnimation(0.3, 0, new Duration(TimeSpan.FromSeconds(0.38))));
            MenuExpandido = false;
            if (btnMenu.IsChecked.Value)
                btnMenu.IsChecked = false;
        }

        Grid grdAnteriror_MENU = new Grid();

        //MOUSE OVER DE LOS BOTONES DE OPCIONES (PANEL IZQUIERDO)
        private void Menu_MouseEnter(object sender, MouseEventArgs e)
        {
            if (grdAnteriror_MENU != (sender as Grid))
            {
                ///((sender as Grid).Children[0] as Label).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x2C, 0x49));
                SolidColorBrush ColorFondo = Application.Current.Resources["PrimaryHueDarkBrush"] as SolidColorBrush;
                ((sender as Grid).Children[0] as Label).Background = ColorFondo;
                ((sender as Grid).Children[0] as Label).BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x29, 0x7A, 0xB0));
            }
        }

        //MOUSE LEAVE DE LOS BOTONES DE OPCIONES (PANEL IZQUIERDO)
        private void Menu_MouseLeave(object sender, MouseEventArgs e)
        {
            if (grdAnteriror_MENU != (sender as Grid))
            {
                ((sender as Grid).Children[0] as Label).Background = Metodos.Vacio;
                ((sender as Grid).Children[0] as Label).BorderBrush = Metodos.Vacio;
            }
        }

        //MOUSE CLICK DE LOS BOTONES DE OPCIONES (PANEL IZQUIERDO)
        private void Menu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MENUSeleccionando = (((sender as Grid).Children[1] as DockPanel).Children[1] as Label).Content.ToString();
        }

        Grid grdAnteriror_SUBMENU = new Grid();

        //MOUSE OVER DE LOS BOTONES DE OPCIONES (PANEL IZQUIERDO)
        private void SubMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            if (grdAnteriror_SUBMENU != (sender as Grid))
            {
                ///((sender as Grid).Children[0] as Label).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x2C, 0x49));
                ///LinearGradientBrush ColorFondo = Application.Current.Resources["AzulDegradadoNU4IT"] as LinearGradientBrush;
                SolidColorBrush ColorFondo2 = Application.Current.Resources["PrimaryHueDarkBrush"] as SolidColorBrush;
                //((sender as Grid).Children[0] as Label).Opacity = 0.8;
                ((sender as Grid).Children[0] as Label).Background = ColorFondo2;
                ((sender as Grid).Children[0] as Label).BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x29, 0x7A, 0xB0));
            }
        }

        //MOUSE LEAVE DE LOS BOTONES DE OPCIONES (PANEL IZQUIERDO)
        private void SubMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            if (grdAnteriror_SUBMENU != (sender as Grid))
            {
                ((sender as Grid).Children[0] as Label).Background = Metodos.Vacio;
                ((sender as Grid).Children[0] as Label).BorderBrush = Metodos.Vacio;
            }
        }

        //AL DARLE CLICK AL BOTON, IDENTIFICAR EL "CONTENT" PARA REALIZAR SU ACCIÓN
        private void SubMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (grdAnteriror_SUBMENU != (sender as Grid))
                {
                    try
                    {
                        ///((sender as Grid).Children[0] as Label).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x31, 0x51));
                        SolidColorBrush ColorFondo = Application.Current.Resources["PrimaryHueDarkBrush"] as SolidColorBrush;
                        ((sender as Grid).Children[0] as Label).Background = ColorFondo;
                        ((sender as Grid).Children[0] as Label).BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x37, 0xB5, 0x54));
                        (grdAnteriror_SUBMENU.Children[0] as Label).Background = Metodos.Vacio;
                        (grdAnteriror_SUBMENU.Children[0] as Label).BorderBrush = Metodos.Vacio;
                        grdAnteriror_SUBMENU = (sender as Grid);
                    }
                    catch (Exception)
                    {
                        ///Botones de Menu y Submenu
                        grdAnteriror_SUBMENU = new Grid() { Name = "Menu_0" };
                        grdAnteriror_SUBMENU.Children.Add(new Label() { BorderThickness = new Thickness(8, 0, 0, 0), Background = Metodos.Vacio, BorderBrush = Metodos.Vacio });
                        SubMenu_MouseDown(sender, e);
                        return;
                    }
                }
                ///Acción
                string Submenu = ((sender as Grid).Children[1] as Label).Content.ToString();
                SeleccionandoSUBMenu(MENUSeleccionando, Submenu);
                ///Animación
                ContraerMenu();
            }
            catch (Exception ex)
            {
                tools.MessageShowOK_2(ex.ToString(), "ERROR");
            }
        }
        #endregion

        //CARFGANDO ELEMENTOS DE LA MAIN WINDOW
        public MainWindow()
        {
            InitializeComponent();
        }

        //AL HABER CARGADO EL FORMULARIO PRINCIPAL (MAIN WINDOW)
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarComponentes(null, null);
        }

        //METODO QUE OFRECE EJECUTA OPERACIONES EN FORMA ASINCRÓNICA
        public void HiloDeEjecucion_CargaComponentes()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += CargarComponentes;
            worker.RunWorkerAsync();
        }

        public void CargarComponentes(object sender, DoWorkEventArgs e)
        {
            ///Dispatcher.Invoke(((Action)(() => VentanaNu4MainWindow.BeginAnimation(Window.OpacityProperty, new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(1.5)))))));
            ///Alto y Ancho
            try
            {
                Dispatcher.Invoke(((Action)(() => left = this.Left)));
                Dispatcher.Invoke(((Action)(() => top = this.Top)));
                Dispatcher.Invoke(((Action)(() => width = this.Width)));
                Dispatcher.Invoke(((Action)(() => height = this.Height)));
            }
            catch (Exception)
            {

            }
            ValoresDeDiseño();
            ///Iniciando elementos de diseño
            Dispatcher.Invoke(((Action)(() => PanelRobots.Children.Clear())));
            Dispatcher.Invoke(((Action)(() => stkSubmenus.Children.Clear())));
            Dispatcher.Invoke(((Action)(() => PanelRobots.Width = 190)));
            Dispatcher.Invoke(((Action)(() => grdSubmenus.Width = 0)));
            Dispatcher.Invoke(((Action)(() => grdSubmenus.Height = 0)));
            Dispatcher.Invoke(((Action)(() => rctSubMenus.Width = 0)));
            Dispatcher.Invoke(((Action)(() => rctSubMenus.Height = 0)));
            Dispatcher.Invoke(((Action)(() => gridFONDO.Margin = margin_B1)));
            Dispatcher.Invoke(((Action)(() => gridPrincipal.Margin = margin_B2)));
            Dispatcher.Invoke(((Action)(() => StatusBar.Margin = margin_B3)));
            Dispatcher.Invoke(((Action)(() => gridClientes.Width = 47)));
            Dispatcher.Invoke(((Action)(() => gridClientes.Opacity = 1)));
            Dispatcher.Invoke(((Action)(() => PanelRobots.Width = 47)));
            Dispatcher.Invoke(((Action)(() => gridMenu.Background = Metodos.Vacio)));
            Dispatcher.Invoke(((Action)(() => gridMenu.Height = 0)));
            Dispatcher.Invoke(((Action)(() => gridMenu.Visibility = Visibility.Collapsed)));
            Dispatcher.Invoke(((Action)(() => gridPrincipal.Children.Clear())));
            Dispatcher.Invoke(((Action)(() => gridPrincipal.Children.Add(new Pages.Home()))));
            Dispatcher.Invoke(((Action)(() => txtStatus.Content = "  Versión: " + Pages.SplashWindow.versionLBL + "   •   ID: " + tools.ObtenMacAddress() + "   •   " + Pages.SplashWindow.FechaHoraUpdate)));
            ///Actualizaciones
            string HayVersion = Pages.SplashWindow.HayActualizacion;
            if (HayVersion == "Tienes la versión mas reciente.")
            {
                //var userCorrect = new Uri(@"/QPil;component/Resources/Imagenes/QPil/download32sinact.png", UriKind.RelativeOrAbsolute);
                //btnDownlImg.Source = new BitmapImage(userCorrect);
                Dispatcher.Invoke(((Action)(() => btnDownlImg.Child = new PackIcon() { Kind = PackIconKind.Download, Height = 27, Width = 27, Foreground = Metodos.Blanco })));
            }
            if (HayVersion == "Hay una versión disponible mas actualizada.")
            {
                //var userCorrect = new Uri(@"/QPil;component/Resources/Imagenes/QPil/download32.png", UriKind.RelativeOrAbsolute);
                //btnDownlImg.Source = new BitmapImage(userCorrect);
                Dispatcher.Invoke(((Action)(() => btnDownlImg.Child = new PackIcon() { Kind = PackIconKind.Download, Height = 27, Width = 27, Foreground = Metodos.btn_Verde })));
            }
            Dispatcher.Invoke(((Action)(() => MenusPorUsuario())));

        }

        #region MENUS Y SUBMENUS

        //ABRIR MENU
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            if (MenuExpandido)
            {
                ContraerMenu();
                MenuExpandido = false;
            }
            else
            {
                ExpanderMenu();
                MenuExpandido = true;
            }
        }

        //AL SACAR EL MOUSE DEL STACKPANEL
        private void stkSubmenus_MouseLeave(object sender, MouseEventArgs e)
        {
            int AlturaSubmenu = 0;
            foreach (var item in stkSubmenus.Children)
                AlturaSubmenu += 41;
            ///
            grdSubmenus.BeginAnimation(Grid.WidthProperty, new DoubleAnimation(190, 0, new Duration(TimeSpan.FromSeconds(0.25))));
            rctSubMenus.BeginAnimation(Rectangle.WidthProperty, new DoubleAnimation(190, 0, new Duration(TimeSpan.FromSeconds(0.25))));
            ///
            if (MenuExpandido)
                PanelRobots.Width = 190;
            else
                PanelRobots.Width = 47;
        }

        //AL DAR CLICK EN EL STACKPANEL
        private void stkSubmenus_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        //AL PASAR EL MOUSE EN EL STACKPANEL
        private void stkSubmenus_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        //CLICK IZQUIERDO EN MENU
        private void Menu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (grdAnteriror_MENU != (sender as Grid))
            {
                try
                {
                    ///((sender as Grid).Children[0] as Label).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x31, 0x51));
                    SolidColorBrush ColorFondo = Application.Current.Resources["PrimaryHueDarkBrush"] as SolidColorBrush;
                    ((sender as Grid).Children[0] as Label).Background = ColorFondo;
                    ((sender as Grid).Children[0] as Label).BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x37, 0xB5, 0x54));
                    (grdAnteriror_MENU.Children[0] as Label).Background = Metodos.Vacio;
                    (grdAnteriror_MENU.Children[0] as Label).BorderBrush = Metodos.Vacio;
                    grdAnteriror_MENU = (sender as Grid);
                }
                catch (Exception)
                {
                    ///Botones de Menu y Submenu
                    grdAnteriror_MENU = new Grid() { Name = "Menu_0" };
                    grdAnteriror_MENU.Children.Add(new Label() { BorderThickness = new Thickness(8, 0, 0, 0), Background = Metodos.Vacio, BorderBrush = Metodos.Vacio });
                    Menu_MouseLeftButtonDown(sender, e);
                    return;
                }
            }
            ///Armar submenus
            ArmarSubMenu((((sender as Grid).Children[1] as DockPanel).Children[1] as Label).Content.ToString(), (sender as Grid).Name);
            stkSubmenus.UpdateLayout();
            string Menu = (sender as Grid).Name.Replace("Menu_", "");
            int NUM = Convert.ToInt32(Menu) * 41;
            grdSubmenus.Margin = new Thickness(0, NUM, 0, 0);
            rctSubMenus.Margin = new Thickness(0, NUM, 0, 0);
            int AlturaSubmenu = 0;
            foreach (var item in stkSubmenus.Children)
                AlturaSubmenu += 41;
            grdSubmenus.Height = AlturaSubmenu;
            rctSubMenus.Height = AlturaSubmenu;
            ///
            double Ancho = 0;
            if (MenuExpandido)
                PanelRobots.Width = 190;
            else
                PanelRobots.Width = 47;
            ///Animacion
            grdSubmenus.BeginAnimation(Grid.WidthProperty, new DoubleAnimation(0, 190, new Duration(TimeSpan.FromSeconds(0.25))));
            grdSubmenus.BeginAnimation(Grid.OpacityProperty, new DoubleAnimation(0.6, 1, new Duration(TimeSpan.FromSeconds(0.25))));
            rctSubMenus.BeginAnimation(Rectangle.WidthProperty, new DoubleAnimation(0, 190, new Duration(TimeSpan.FromSeconds(0.25))));
            rctSubMenus.BeginAnimation(Rectangle.OpacityProperty, new DoubleAnimation(0, 0.75, new Duration(TimeSpan.FromSeconds(0.25))));
        }

        //CLICK DERECHO EN MENU
        private void Menu_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ///MessageBox.Show("");
        }

        //AL DAR CLICK EN 
        bool BandeeXpand = false;
        private void gridMain_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MenuExpandido && BandeeXpand)
            {
                ContraerMenu();
                BandeeXpand = false;
            }
        }

        #endregion

        #region ARMAR COMPONENTES PARA LOS MENUS

        //ARMAR MENUS Y SUBMENUS
        List<string[]> Menus = new List<string[]>();
        List<List<Grid>> SubMenus = new List<List<Grid>>();
        public void ArmarComponentes_PorMenu()
        {
            int conta = 0;
            foreach (string[] Menu in Menus)
            {
                string TituloMenu = Menu[0].ToString();
                string RutaImagen = Menu[1].ToString();
                Grid grd_MENU = CrearMENU(TituloMenu, RutaImagen, conta);
                PanelRobots.Children.Add(grd_MENU);
                /// Agregar SUBMenu
                List<Grid> SubMenu = new List<Grid>();
                int contaSUB = 0;
                for (int i = 2; i < Menu.Length; i++)
                {
                    Grid grd_SUBMENU = CrearSUBMENU(Menu[i], contaSUB);
                    SubMenu.Add(grd_SUBMENU);
                    contaSUB++;
                }
                SubMenus.Add(SubMenu);
                conta++;
            }
            PanelRobots.UpdateLayout();
        }

        //CREAR MENU
        public Grid CrearMENU(string lblContent, string RutaImagen, int aux)
        {
            if (RutaImagen == "")
                RutaImagen = @"/QPil;component/Resources/Imagenes/Icons/logo-nu2.ico";
            ///
            Grid grd = new Grid();
            grd.Name = "Menu_" + aux;
            grd.MouseDown += Menu_MouseDown;
            grd.MouseLeftButtonDown += Menu_MouseLeftButtonDown;
            grd.MouseRightButtonDown += Menu_MouseRightButtonDown;
            grd.MouseEnter += Menu_MouseEnter;
            grd.MouseLeave += Menu_MouseLeave;
            Label lblBorde = new Label() { BorderThickness = new Thickness(8, 0, 0, 0) };
            grd.Children.Add(lblBorde);
            DockPanel dck = new DockPanel();
            dck.Children.Add(new Image() { Source = new BitmapImage(new Uri(RutaImagen, UriKind.RelativeOrAbsolute)), Margin = new Thickness(13, 8, 5, 7), Width = 26 });
            dck.Children.Add(new Label() { Content = lblContent, Foreground = Metodos.Blanco, FontWeight = FontWeights.Bold, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center });
            grd.Children.Add(dck);
            grd.ToolTip = lblContent;
            return grd;
        }

        //CREAR SUBMENU
        public Grid CrearSUBMENU(string lblContent, int aux)
        {
            Grid grd = new Grid();
            grd.Name = "SUBMenu_" + aux;
            grd.MouseDown += SubMenu_MouseDown;
            grd.MouseEnter += SubMenu_MouseEnter;
            grd.MouseLeave += SubMenu_MouseLeave;
            grd.Height = 41;
            grd.Children.Add(new Label() { BorderThickness = new Thickness(0, 0, 8, 0) });
            grd.Children.Add(new Label() { Content = lblContent, Foreground = Metodos.Blanco, FontWeight = FontWeights.Bold, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center });
            return grd;
        }

        //ARMAR SUBMENU POR CADA CLICK
        public void ArmarSubMenu(string lblContent, string NameGrid)
        {
            Grid grd = new Grid();
            stkSubmenus.Children.Clear();
            stkSubmenus.UpdateLayout();
            int numList = Convert.ToInt32(NameGrid.Replace("Menu_", ""));
            foreach (Grid item in SubMenus[numList])
            {
                System.Windows.Media.Effects.Effect stlSOmbra = Application.Current.FindResource("MaterialDesignShadowDepth1") as System.Windows.Media.Effects.Effect;
                item.Effect = stlSOmbra;
                stkSubmenus.Children.Add(item);
            }
            stkSubmenus.UpdateLayout();
        }
        #endregion

        #region DISEÑO POR USUARIO

        //DISEÑO DEL NUBOT DEPENDIENDO DEL USUARIO LOGUEADO
        public void DiseñoNubot()
        {
            if (Pages.SplashWindow.User.ToUpper() == "USUARIO" || Pages.SplashWindow.User.ToUpper() == "ADMIN")
            {
                this.UsuarioVisible = "DESARROLLO";
                //LinearGradientBrush gradientBrush12 = new LinearGradientBrush(Color.FromArgb(0xFF, 0x7C, 0x11, 0xAE), Color.FromArgb(0xFF, 0x40, 0x0C, 0x59), new Point(0.5, 0), new Point(0.5, 1));
                //LinearGradientBrush gradientBrush22 = new LinearGradientBrush(Color.FromArgb(0xFF, 0x7C, 0x11, 0xAE), Colors.Black, new Point(0.5, 0), new Point(0.5, 1));
                //gridClientes.Background = gradientBrush12;
                //imgCarita.Background = gradientBrush22;
                ///var imagePath = @"pack://application:,,,/QPil;component/Resources/Imagenes/Nu4it/devnu4.png";
                var imagePath = @"pack://application:,,,/QPil;component/Resources/Imagenes/Nu4it/devnu4.png";
                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(imagePath, UriKind.Absolute)));
                imgUsuario.Background = brush; new ImageBrush() { ImageSource = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute)) };
            }
            else
            {
                if (Pages.Login.APODO_ini != "")
                    this.UsuarioVisible = Pages.Login.APODO_ini.ToUpper();
                else
                {
                    try
                    {
                        this.UsuarioVisible = Pages.SplashWindow.User_NombreCompleto.Split(' ')[0];
                    }
                    catch (Exception)
                    {
                        this.UsuarioVisible = Pages.SplashWindow.User.ToUpper();
                    }
                }
            }
            if (this.UsuarioVisible == "")
            {
                this.UsuarioVisible = Pages.SplashWindow.User.ToUpper();
            }
            lblNombreVisible.Text = this.UsuarioVisible.ToUpper();
            lblNombreVisible.ToolTip = Pages.SplashWindow.User_NombreCompleto;
            ///
            MenuExpandido = false;
            ExpanderMenu();
            gridClientes.BeginAnimation(Grid.WidthProperty, new DoubleAnimation(47, 190, new Duration(TimeSpan.FromSeconds(0.6))));
            gridClientes.BeginAnimation(Grid.OpacityProperty, new DoubleAnimation(.8, 1, new Duration(TimeSpan.FromSeconds(0.6))));
        }

        #endregion

        #region OBJETOS DE LAS CLASES

        ///OTRAS
        Pages.Desarrollador.Developers deve = new Pages.Desarrollador.Developers(Pages.SplashWindow.TipoINFO);
        Pages.Home home = new QPil.Pages.Home();

        #endregion

        #region MENUS POR USUARIO

        //CREAR MENUS POR USUARIO
        public void MenusPorUsuario()
        {
            ///Menuses
            Menus = new List<string[]>();
            List<string> ListaACtivados = new List<string>();
            string Menuses = "";
            #region ADMIN
            if (Pages.SplashWindow.User == "admin" || Pages.SplashWindow.User == "Usuario")
                Menuses = "TODOS";
            else
            {
                DataTable DATOS = tools.BuscaDatos_LINQ(Pages.SplashWindow.DTLogin, "Usuario", Pages.SplashWindow.User);
                Menuses = DATOS.Rows[0].Field<string>(3).Replace(" ", "");
            }
            if (Menuses.ToUpper() == "TODOS")
            {

                foreach (string item in ListaMenus)
                    ListaACtivados.Add(item);
            }
            else
                ListaACtivados = Menuses.Split(',').ToList();
            #endregion
            foreach (string item in ListaACtivados.ToList().OrderBy(q => q).ToList())
            {
                switch (item.ToUpper().Replace("\r", "").Replace("\n", "").Replace(" ", ""))
                {
                    //case "ADMINISTRADOR":
                    //    Menus.Add(new string[] { "Menú desarrollador", @"/QPil;component/Resources/Imagenes/Nu4it/devnu432.png", "Administrar", "Actualización", "Otros", "Manual de operación" });
                    //    break;
                    case "ADMINISTRADOR":
                        Menus.Add(new string[] { "Menú", @"/QPil;component/Resources/Imagenes/Nu4it/devnu432.png", "Inicio" });
                        break;
                    //case "QCONTENT":
                    //    Menus.Add(new string[] { "QContent", @"/QPil;component/Resources/Imagenes/Nu4it/devnu432.png", "Descarga de Documentos" });
                    //    break;
                    //case "OCR":
                    //    Menus.Add(new string[] { "Ocr", @"/QPil;component/Resources/Imagenes/Nu4it/devnu432.png", "Captcha Repuve y SAT" });
                    //    break;
                    default: break;
                }
            }
            ///Armado de componentes
            ArmarComponentes_PorMenu();
            ///OTROS
            DiseñoNubot();
        }

        //Objetos de controles de Usuario
        Ejecucion_Individual.OCR ocr = new Ejecucion_Individual.OCR();
        Ejecucion_Individual.Sise sise = new Ejecucion_Individual.Sise();
      //  Ejecucion_Individual.QContent qcontent = new Ejecucion_Individual.QContent(new System.Net.WebClient());
        //SELECCIONANDO SUBMENU
        public void SeleccionandoSUBMenu(string Menu, string Content_SubMenu)
        {
            BandeeXpand = true;

            #region AJUSTES
            ///--------------------------- EN AJUSTES ---------------------------
            QPil.Pages.EnAjustes working = new QPil.Pages.EnAjustes();
            if (!Pages.SplashWindow.EnDesarrollo)
            {
                if (
                    Menu.Equals("*****")
                   )
                {
                    gridPrincipal.Children.Clear();
                    gridPrincipal.Children.Add(working);
                    return;
                }
            }
            ///------------------------------------------------------------------ 
            #endregion

            switch (Menu)
            {
                #region NU4IT

                case "Menú":
                //    gridPrincipal.Children.Clear();
                //    switch (Content_SubMenu)
                //    {
                //        case "Administrar":
                //            deve = new Pages.Desarrollador.Developers(Pages.SplashWindow.TipoINFO);
                //            deve.ShowDialog();
                //            break;
                //        case "Actualización": gridPrincipal.Children.Add(working); break;
                //        case "Otros": gridPrincipal.Children.Add(working); break;
                //        case "Manual de operación": gridPrincipal.Children.Add(working); break;
                //        default: break;
                //    }
                //    break;
                //case "Sise":
                    gridPrincipal.Children.Clear();
                    switch (Content_SubMenu)
                    {
                        case "Inicio":
                            gridPrincipal.Children.Add(sise);
                            break;
                        default: break;
                    }
                    break;
                //case "QContent":
                //    gridPrincipal.Children.Clear();
                //    switch (Content_SubMenu)
                //    {
                //        case "Descarga de Documentos":
                //            gridPrincipal.Children.Add(qcontent);

                //            break;
                //        default: break;
                //    }
                //    break;
                //case "Ocr":
                //    gridPrincipal.Children.Clear();
                //    switch (Content_SubMenu)
                //    {
                //        case "Captcha Repuve y SAT":
                //            gridPrincipal.Children.Add(ocr);
                //            break;
                //        default: break;
                //    }
                //    break;
                #endregion

                default: gridPrincipal.Children.Clear(); gridPrincipal.Children.Add(home); break;
            }
        }

        #endregion


    }
}
