#region Referencia de Librerias
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
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
using System.Runtime.InteropServices;
using System.Windows.Media.Animation;
using Nu4it;
using nu4itExcel;
using nu4itFox;

#endregion

namespace QPil.Resources
{
    /// <summary>
    /// Interaction logic for Plantilla.xaml
    /// Desarrollador: Jorge Núñez
    /// Fecha: 11/Mayo/2017
    /// </summary>
    public partial class Plantilla : UserControl
    {
        #region VARIABLES GLOBALES

        //VARIABLES GLOBALES
        QPil.Metodos tools = new QPil.Metodos();
        usaR objNu4 = new usaR();
        nuExcel objNuExcel = new nuExcel();
        nufox objNuFox = new nufox();
        #endregion

        #region •••••• HILO DINAMICO ••••••

        //•••••••••••••••••• HILO DINAMICO by JORGE NUÑEZ ••••••••••••••••••
        string TipoUsuario = "";
        string RUTA_ARCHIVO_LOG = "";
        bool loadeded = false;
        dynamic aviso = (dynamic)null;
        string Metodo = "";
        int seg = 0;
        int min = -1;
        int counter = 0;
        public System.Timers.Timer aTimer;

        //METODO QUE OFRECE EJECUTA OPERACIONES EN FORMA ASINCRÓNICA
        public void HiloDeEjecucion(string METODO)
        {
            this.Metodo = METODO;
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += FuncionPrincipalHILO;
            worker.RunWorkerCompleted += HiloTerminado;
            worker.RunWorkerAsync();
        }

        //CRONOMETRO
        public void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            seg++;
            if (counter % 60 == 0)
            {
                min++;
                seg = 0;
            }
            Dispatcher.Invoke(((Action)(() => aviso.label.Content = "Status: " + min.ToString("00") + ":" + seg.ToString("00"))));
            counter++;
        }

        //HILO TERMINADO
        public void HiloTerminado(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                loadeded = false;
                seg = 0;
                min = -1;
                counter = 0;
                aTimer.Stop();
                aTimer.Enabled = false;
            }
            catch (Exception)
            {

            }
        }

        //STATUS LOG
        public void StatusLog(string Reporte)
        {
            Dispatcher.Invoke(((Action)(() => aviso.txtStatusContent.Text = Reporte)));
            Dispatcher.Invoke(((Action)(() => objNu4.ReportarLog(RUTA_ARCHIVO_LOG, Reporte))));
        }

        /*▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬
         *                                                FUNCION PRINCIPAL
         *▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬*/
        public void FuncionPrincipalHILO(object sender, DoWorkEventArgs e)
        {
            bool ExitoFP = false;
            try
            {
                ///----- Cronometro -----
                aTimer = new System.Timers.Timer(1000);
                aTimer.Elapsed += OnTimedEvent;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
                ///----- Bitacora -----
                RUTA_ARCHIVO_LOG = Pages.SplashWindow.RutaNubotVersiones + @"\Bitacoras\Bit_" + this.ToString() + DateTime.Now.ToString("dd/MM/yy").Replace("/", "") + "_" + DateTime.Now.ToString("hh:mm").Replace(":", "") + ".log";
                objNu4.CreaArchivoLog(RUTA_ARCHIVO_LOG);
                objNu4.ReportarLog(RUTA_ARCHIVO_LOG, "La MAC: " + tools.ObtenMacAddress() + " y  usuario: " + QPil.Pages.Login.User + " inició el robot.");
                ///----- Inicio -----
                DateTime Inicio = DateTime.Now;
                StatusLog("Proceso Iniciado");
                Dispatcher.Invoke(((Action)(() => aviso = new Pages.Aviso())));
                Dispatcher.Invoke(((Action)(() => aviso.Show())));
                Dispatcher.Invoke(((Action)(() => this.IsEnabled = false)));
                Thread.Sleep(500);
                switch (this.Metodo)
                {
                    case "Iniciar componentes": Funcion(); break;
                    default: break;
                }
                ///----- Resultado -----
                if (ExitoFP == true)
                {
                    DateTime fin = DateTime.Now;
                    StatusLog("PROCESO FINALIZADO CON EXITO.");
                    tools.MessageShowOK_2("PROCESO FINALIZADO CON EXITO. \nHora de inicio: " + Inicio.ToString() + ".\nHora al terminar: " + fin.ToString() + ". \n\nMe tarde: " + (fin - Inicio).TotalMinutes.ToString("F1", System.Globalization.CultureInfo.InvariantCulture) + " minutos.", "OK");
                }
                if (ExitoFP == false)
                {
                    StatusLog("No se pudo concluir el proceso.");
                    tools.MessageShowOK_2("NO SE PUDO CONCLUIR EL PROCESO", "ERROR");
                }
            }
            catch (Exception ex)
            {
                objNu4.ReportarLog(RUTA_ARCHIVO_LOG, ex.ToString());
                tools.MessageShowOK_2("Ocurrio algo inesperado; favor de avisar al proveedor.\n\n\nNü4It Automation \n Teléfono: 50206474  \n Ext.Sistemas: 2244", "ERROR");
            }
            finally
            {
                ///----- Finalizar -----
                tools.EnviarLOGaCorreo(RUTA_ARCHIVO_LOG, "Plantilla");
                Dispatcher.Invoke(((Action)(() => this.IsEnabled = true)));
                Dispatcher.Invoke(((Action)(() => aviso.txtStatusContent.Text = "Robot Terminado...!")));
                Dispatcher.Invoke(((Action)(() => aviso.Cerrar())));
            }
        }

        #endregion

        //INICIO DE COMPONENTES
        public Plantilla()
        {
            InitializeComponent();
        }

        //METODO PROINCIPAL AL INICIAR EL BOTON DE "INICIAR PROCESO"
        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            HiloDeEjecucion("FUNCION");
        }

        //FUNCION PRINCIPAL
        public bool Funcion()
        {
            bool exito = true;

            return exito;
        }

        #region HERRAMIENTAS

        //MOSTAR EL MENU DE HERRAMIENTAS
        private void btnSetti_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (grdHerramientas.Height == 27)
            {
                DoubleAnimation myDoubleAnimation = new DoubleAnimation(27, 250, new Duration(TimeSpan.FromSeconds(0.2)));
                grdHerramientas.BeginAnimation(Grid.HeightProperty, myDoubleAnimation);
            }
            else
            {
                DoubleAnimation myDoubleAnimation = new DoubleAnimation(250, 27, new Duration(TimeSpan.FromSeconds(0.2)));
                grdHerramientas.BeginAnimation(Grid.HeightProperty, myDoubleAnimation);
            }
        }

        #endregion
    }
}
