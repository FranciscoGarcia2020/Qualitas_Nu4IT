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

namespace QPil.Pages
{
    public partial class Utillities : Window
    {

        //VARIBLES GLOBALES
        Metodos tools = new Metodos();

        //INICIANDO COMPONENTES
        public Utillities(string RootVntana)
        {
            InitializeComponent();
            ///
        }


        //SELECCIONANDO OPCION
        private void btnAlgunaOpcion_Click(object sender, RoutedEventArgs e)
        {
            string Content = (sender as Button).Content.ToString();
            switch (Content)
            {
                default: break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //tools.AnimacionLoaded_plush(this);
        }
    }
}
