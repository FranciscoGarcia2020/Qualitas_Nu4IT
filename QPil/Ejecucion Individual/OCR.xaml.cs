using System;
using System.Collections.Generic;
using System.Data;
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

namespace QPil.Ejecucion_Individual
{
    /// <summary>
    /// Lógica de interacción para OCR.xaml
    /// </summary>
    public partial class OCR : UserControl
    {
        

        private static string rutaLog = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\prueba\RepuveLog" + DateTime.Now.ToString("dd/MM/yy").Replace("/", "") + "_" + DateTime.Now.ToString("hh:mm").Replace(":", "") + ".log", user = "";
        private static string rutaLogSat = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\prueba\SatLog" + DateTime.Now.ToString("dd/MM/yy").Replace("/", "") + "_" + DateTime.Now.ToString("hh:mm").Replace(":", "") + ".log";
        private static string rutaLogSat2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\prueba\Sat2Log" + DateTime.Now.ToString("dd/MM/yy").Replace("/", "") + "_" + DateTime.Now.ToString("hh:mm").Replace(":", "") + ".log";
        private static Metodos tools = new Metodos();
        private static  Dlls.Nu4it objnu4it = new Dlls.Nu4it();
        private static void reportar(string reporte)
        {
            objnu4it.ReportarLog(rutaLog, reporte);
        }


        public OCR()
        {
            InitializeComponent();
        }
       
        private void BtnRepube_Click(object sender, RoutedEventArgs e)
        {
            Procesos.CaptchaRepuve repuve = new Procesos.CaptchaRepuve(rutaLog);
            DataTable DT_DATOS_REPUVE = new DataTable();//comentar cuando OCR Funcione
            DT_DATOS_REPUVE.Columns.Add("SERIE", typeof(string));//comentar cuando OCR Funcione
            DT_DATOS_REPUVE.Rows.Add("WAU9FD8T3EA007986"); //comentar cuando OCR Funcione
            DT_DATOS_REPUVE.Rows.Add("9BWHE21J834052941"); //comentar cuando OCR Funcione
            DT_DATOS_REPUVE.Rows.Add("93YB62JT4BJ066169"); //comentar cuando OCR Funcione
            DT_DATOS_REPUVE.Rows.Add("3N1BC1AS6CK268248"); //comentar cuando OCR Funcione
            //repuve.recibeDatatableRepuve(DT_DATOS_REPUVE);

            
        }

        private void BtnSAT_Click(object sender, RoutedEventArgs e)
        {
            Procesos.CaptchaSAT sat = new Procesos.CaptchaSAT(rutaLogSat);
            DataTable DT_DATOS_SAT = new DataTable();//comentar cuando OCR Funcione
            DT_DATOS_SAT.Columns.Add("RFC", typeof(string));//comentar cuando OCR Funcione
            DT_DATOS_SAT.Columns.Add("SERIE", typeof(string));//comentar cuando OCR Funcione
            DT_DATOS_SAT.Columns.Add("FOLIO", typeof(string));//comentar cuando OCR Funcione
            DT_DATOS_SAT.Columns.Add("NO.APROBACION", typeof(string));//comentar cuando OCR Funcione
            DT_DATOS_SAT.Columns.Add("AÑO", typeof(string));//comentar cuando OCR Funcione
            DT_DATOS_SAT.Columns.Add("CERTIFICADO", typeof(string));//comentar cuando OCR Funcione
            DT_DATOS_SAT.Columns.Add("RUTA PDF", typeof(string));//comentar cuando OCR Funcione
            DT_DATOS_SAT.Rows.Add("AUT091023H43", "E", "0000897", "424694", "2011", "00001000000202409238", @"C:\Users\Axel Alvallar\Desktop\prueba\Siniestros\a\20190128105758536.pdf");//comentar cuando OCR Funcione
            //DT_DATOS_SAT.Rows.Add("AUT091023H49", "E", "0000897", "424694", "2011", "00001000000202409238", @"C:\Users\Axel Alvallar\Desktop\prueba\Siniestros\a\idc--59254917F.pdf");
            sat.recibeDatatableSAT(DT_DATOS_SAT);
        }
        private void BtnSAT2_Click(object sender, RoutedEventArgs e)
        {
            Procesos.CaptchaSAT2 sat2 = new Procesos.CaptchaSAT2(rutaLogSat2);
            DataTable DT_DATOS_SAT2 = new DataTable();//comentar cuando OCR Funcione
            DT_DATOS_SAT2.Columns.Add("FOLIOFISCAL", typeof(string));//comentar cuando OCR Funcione
            DT_DATOS_SAT2.Columns.Add("RFCEMISOR", typeof(string));//comentar cuando OCR Funcione
            DT_DATOS_SAT2.Columns.Add("RFCRECEPTOR", typeof(string));//comentar cuando OCR Funcione
            DT_DATOS_SAT2.Rows.Add("F96A317BE5354F448C5FA993BC42055F", "PAG150819C57", "DIC860428M2A");//comentar cuando OCR Funcione
            DT_DATOS_SAT2.Rows.Add("4F12059CA3D54ADE8162D74B2AC05A92", "OKE100921P88", "XAXX010101000"); //comentar cuando OCR Funcione
            sat2.recibeDatatableSAT2(DT_DATOS_SAT2);
        }

    }
}
