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

namespace QPil.Pages.Mensajes
{
    /// <summary>
    /// Interaction logic for TextBox.xaml
    /// </summary>
    public partial class TextBox : Window
    {
        Metodos tools = new Metodos();
        string Respuesta = "";
        bool EsPassword = false;

        //INICIO DE COMPONENTES
        public TextBox(string TituloPregunta, double Alto, double Ancho)
        {
            InitializeComponent();
            txtTexto.Visibility = Visibility.Collapsed;
            txtTextoPassword.Visibility = Visibility.Collapsed;
            ///
            this.lblTitulo.Content = TituloPregunta;
            if (TituloPregunta.ToUpper().Contains("CONTRASEÑA") || TituloPregunta.ToUpper().Contains("PASSWORD"))
            {
                EsPassword = true;
                txtTextoPassword.Visibility = Visibility.Visible;
            }
            else
                txtTexto.Visibility = Visibility.Visible;
            this.Height = Alto + 10;
            this.Width = Ancho + 10;
        }

        //OK
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (Respuesta.Replace(" ", "") != "")
                Dispatcher.Invoke(((Action)(() => this.DialogResult = true)));
            else
                tools.MessageShowOK_2("Debes responder a la pregunta");
        }

        //RESPUESTA
        public string Answer
        {
            get
            {
                return Respuesta;
            }
        }

        //AL CAMBIAR EL TEXTO
        private void txtTexto_TextChanged(object sender, TextChangedEventArgs e)
        {
            Respuesta = txtTexto.Text;
        }

        private void txtTextoPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Respuesta = txtTextoPassword.Password;
        }
    }
}
