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
using Nu4it;
using System.Data;
using System.IO;
using QPil.Procesos;

namespace QPil.Ejecucion_Individual
{
    /// <summary>
    /// Lógica de interacción para Sise.xaml
    /// </summary>
    public partial class Sise : UserControl
    {
        usaR objNu4 = new usaR();
        private static Metodos tools = new Metodos();
        private static Dlls.Nu4it objnu4it = new Dlls.Nu4it();
        private static string rutaLog; 

        
        public Sise()
        {
            InitializeComponent();
            rutaLog= Directory.GetCurrentDirectory() + @"\" + objnu4it.GeneraNombreArchivo("BITQPilProcSise","log");
            
        }
        private void btnSise_Click(object sender, RoutedEventArgs e)
        {
           
          new  Procesos.UIAutomation().HiloDeEjecucion(rutaLog);

            //dataGrid.Columns.Clear();
            //dataGrid.ItemsSource = null;
            //dataGrid.ItemsSource = Procesos.UIAutomation.DATOS.AsDataView();
        }
        //private void BtnLOG_Click(object sender, RoutedEventArgs e)
        //{
        //    new Procesos.DatosLog().HiloDeEjecucion(rutaLog);

        //    dataGrid.Columns.Clear();
        //    dataGrid.ItemsSource = null;
        //    dataGrid.ItemsSource = Procesos.DatosLog.DATOS.AsDataView();
        //}
    }
}
