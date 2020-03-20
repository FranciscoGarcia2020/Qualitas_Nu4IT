using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
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
using QPil.Procesos;

namespace QPil.Ejecucion_Individual
{
    /// <summary>
    /// Lógica de interacción para QContent.xaml
    /// </summary>
    public partial class QContent : UserControl
    {
        private WebClient cliente;
        public QContent(WebClient cliente)
        {
            InitializeComponent();
            this.cliente = cliente;
            cliente.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(cargado);
            cliente.DownloadProgressChanged += new DownloadProgressChangedEventHandler(cargando);
        }
        public void cargando(object sender, DownloadProgressChangedEventArgs e)
        {
            pBDescarga.Value = e.ProgressPercentage;
        }

        private void cargado(object sender, AsyncCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static Metodos tools = new Metodos();
        private static Dlls.Nu4it objnu4it = new Dlls.Nu4it();
        private void btnQContent_Click(object sender, RoutedEventArgs e)
        {
            //objnu4it.CreaArchivoLog(rutaLog);

            new Procesos.QContent().HiloDeEjecucion();
            //Procesos.QContent.ContentNavigate(rutaLog, siniestros);
            //Dispatcher.Invoke(((Action)(() => Procesos.QContent.ContentNavigate(rutaLog))));
        }
    }
}
