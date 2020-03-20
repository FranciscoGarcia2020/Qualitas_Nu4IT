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
using System.Collections;
using System.IO;
using System.Data;
using System.Net;
using forms = System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using Nu4it;
using nu4itExcel;
using nu4itFox;

namespace QPil.Pages.Desarrollador
{
    public partial class Developers : Window
    {
        //OBJETOS GLOBALES
        Metodos tools = new Metodos();
        Nu4it.usaR objNu4 = new Nu4it.usaR();
        DataTable DatosUsuarios = new DataTable();
        String Version = "";
        String Novedades = "";
        String DataBase = "";
        String InfoGral = "";
        String Licencias = "";
        string Tipo = "";

        //INICIO DE COMPONENTES
        public Developers(string Tipo)
        {
            InitializeComponent();
            this.Tipo = Tipo;
        }

        //AL CARGAR LA VENTANA
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string InfoNUBOT = "";
            string[] Datos = new string[] { };
            switch (this.Tipo)
            {
                case "LOCAL":
                    InfoNUBOT = File.ReadAllText(Pages.SplashWindow.RutaInfoWWW + @"/InfoNubot.inf").Replace("\r", "");
                    Datos = InfoNUBOT.Replace("\n^\n", "♠").Split('♠');
                    Licencias = File.ReadAllText(Pages.SplashWindow.RutaInfoWWW + @"/licencias.inf").Replace("\r", "");
                    break;
                case "HTTP":
                    InfoNUBOT = tools.getHTTP(Pages.SplashWindow.RutaInfoWWW + @"/InfoNubot.inf").Replace("\r", "");
                    Datos = InfoNUBOT.Replace("\n^\n", "♠").Split('♠');
                    Licencias = tools.getHTTP(Pages.SplashWindow.RutaInfoWWW + @"/licencias.inf").Replace("\r", "");
                    break;
                default: break;
            }
            ///
            Version = Datos[1].Replace("VERSION:\n", "");
            Novedades = Datos[2].Replace("NOVEDAES:\n", "");
            DataBase = tools.DesencriptaTexto(Datos[3].Replace("DATABASE:\n", "").Replace(" ", "+"));
            InfoGral = tools.DesencriptaTexto(Datos[4].Replace("INFOGENERAL:\n", "").Replace(" ", "+"));
            //VERSION
            txtVersionNueva.Text = Version;
            //NOVEDADES
            txtNovedades.CaretPosition = txtNovedades.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward);
            txtNovedades.CaretPosition.InsertTextInRun(Novedades);
            //LICENCIAS
            txtLicencias.CaretPosition = txtLicencias.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward);
            txtLicencias.CaretPosition.InsertTextInRun(Licencias);
            //INFO GRAL
            txtArchivoINI.CaretPosition = txtArchivoINI.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward);
            txtArchivoINI.CaretPosition.InsertTextInRun(InfoGral);
            //ADMIN USUARIOS
            LeyendoBaseDeDatos_ONLINE();
            dataGrid.ItemsSource = DatosUsuarios.AsDataView();
            List<string> Usuarios = tools.DataColumnToList_String_Unique(DatosUsuarios, 0);
            txtUsuario.ItemsSource = Usuarios;
            //
            cmbAccion.Items.Add("Editar");
            cmbAccion.Items.Add("Agregar");
            cmbAccion.Items.Add("Eliminar");
            cmbAccion.SelectedIndex = 0;
            cmbRobots.ItemsSource = MainWindow.ListaMenus;
        }

        //LEYENDO BASE DE DATOS DESDE EL SERVIDOR
        public bool LeyendoBaseDeDatos_ONLINE()
        {
            DatosUsuarios.Columns.Add("Usuario");
            DatosUsuarios.Columns.Add("Password");
            DatosUsuarios.Columns.Add("Tipo");
            DatosUsuarios.Columns.Add("Robots");
            bool continuar = false;
            try
            {
                string[] stringDatosUsuarios = DataBase.Split('\n');
                if (!stringDatosUsuarios[0].Contains("<!DOCTYPE HTML PUBLIC"))
                {
                    if (stringDatosUsuarios[0] != "")
                    {
                        for (int i = 0; i < stringDatosUsuarios.Length; i++)
                        {
                            if (stringDatosUsuarios[i] != "")
                            {
                                string[] InfoUsuario = stringDatosUsuarios[i].ToString().Split('\t');
                                if (InfoUsuario.Length >= 4)
                                {
                                    DatosUsuarios.Rows.Add(InfoUsuario[0].Replace("\n", "").Replace("\r", ""), InfoUsuario[1].Replace("\n", "").Replace("\r", ""), InfoUsuario[2].Replace("\n", "").Replace("\r", ""), InfoUsuario[3].Replace("\n", "").Replace("\r", ""));
                                }
                            }
                        }
                        continuar = true;
                    }
                    else { MessageBox.Show("No se logro conexión a la base de datos.\n\nCerrando sistema..."); }
                }
                else { MessageBox.Show("No existe base de datos"); }
                if (continuar)
                {
                    //GuradarLicenciasLocal(DatosLogin);
                }
            }
            catch (Exception ex)
            {
                continuar = false;
            }
            return continuar;
        }

        #region DATABASE

        private void cmbAccion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAccion.SelectedItem.ToString() == "Agregar")
            {
                btnAgregar.Content = "Agregar";
                txtUsuario.IsEditable = true;
                txtPassword.Text = "";
                txtID.Text = "";
                listboxRobots.Items.Clear();
                btnAccion.IsEnabled = true;
                btnEliminar.IsEnabled = true;
                cmbRobots.IsEnabled = true;
            }
            if (cmbAccion.SelectedItem.ToString() == "Editar")
            {
                btnAgregar.Content = "Actualizar";
                txtUsuario.IsEditable = false;
                txtUsuario.SelectedIndex = 0;
                btnAccion.IsEnabled = true;
                btnEliminar.IsEnabled = true;
                cmbRobots.IsEnabled = true;
            }
            if (cmbAccion.SelectedItem.ToString() == "Eliminar")
            {
                btnAgregar.Content = "Eliminar";
                txtUsuario.IsEditable = false;
                txtUsuario.SelectedIndex = 0;
                btnAccion.IsEnabled = false;
                btnEliminar.IsEnabled = false;
                cmbRobots.IsEnabled = false;
            }
        }

        private void btnAccion_Click(object sender, RoutedEventArgs e)
        {
            //Activar
            Button btn = sender as Button;
            if (btn.Content.ToString() == "Activar")
            {
                try
                {
                    string robot = cmbRobots.SelectedValue.ToString();
                    if (!listboxRobots.Items.Contains(robot))
                        listboxRobots.Items.Add(robot);
                }
                catch (Exception ex)
                {
                    try
                    {
                        string robot = cmbRobots.Text.ToString();
                        listboxRobots.Items.Remove(robot);
                    }
                    catch (Exception ec)
                    {

                    }
                }
            }
            //Eliminar
            if (btn.Content.ToString() == "Eliminar")
            {
                try
                {
                    string robot = listboxRobots.SelectedValue.ToString();
                    listboxRobots.Items.Remove(robot);
                }
                catch (Exception ex)
                {
                    try
                    {
                        string robot = cmbRobots.Text.ToString();
                        listboxRobots.Items.Remove(robot);
                    }
                    catch (Exception ec)
                    {

                    }
                }
            }
        }

        private void txtUsuario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                listboxRobots.Items.Clear();
                string Usuario = txtUsuario.SelectedItem.ToString();
                DataTable DatosDelUsuario = tools.BuscaDatos_LINQ(DatosUsuarios, "Usuario", Usuario);
                if (DatosDelUsuario.Rows.Count != 0)
                {
                    txtID.Text = DatosDelUsuario.Rows[0]["Tipo"].ToString();
                    txtPassword.Text = DatosDelUsuario.Rows[0]["Password"].ToString();
                    string[] lista = DatosDelUsuario.Rows[0]["Robots"].ToString().Split(',');
                    foreach (string item in lista)
                        listboxRobots.Items.Add(item);
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            string UsuarioNEW = txtUsuario.Text;
            string PassWordNEW = txtPassword.Text;
            string ID = txtID.Text;
            string robots = "";
            for (int i = 0; i < listboxRobots.Items.Count; i++)
            {
                robots += listboxRobots.Items[i].ToString();
                if ((i + 1) != listboxRobots.Items.Count)
                    robots += ",";
            }
            robots.TrimEnd(',');
            if (btnAgregar.Content.ToString() == "Agregar")
                if (UsuarioNEW != "" && PassWordNEW != "" && ID != "" && robots != "")
                {
                    DatosUsuarios.Rows.Add(UsuarioNEW, PassWordNEW, ID, robots);
                    MessageBox.Show("Usuario agregado");
                }
                else
                    MessageBox.Show("No puedes dejar algun registro vacío");
            else if (btnAgregar.Content.ToString() == "Actualizar")
            {
                if (UsuarioNEW != "" && PassWordNEW != "" && ID != "" && robots != "")
                {
                    for (int u = 0; u < DatosUsuarios.Rows.Count; u++)
                    {
                        if (DatosUsuarios.Rows[u]["Usuario"].ToString() == UsuarioNEW)
                        {
                            DatosUsuarios.Rows[u]["Tipo"] = ID;
                            DatosUsuarios.Rows[u]["Password"] = PassWordNEW;
                            DatosUsuarios.Rows[u]["Robots"] = robots;
                            MessageBox.Show("Usuario actualizado");
                            break;
                        }
                    }
                }
                else
                    MessageBox.Show("No puedes dejar algun registro vacío");
            }
            else if (btnAgregar.Content.ToString() == "Eliminar")
            {
                DataTable DatosDelUsuario = tools.BuscaDatos_LINQ(DatosUsuarios, "Usuario", UsuarioNEW);
                if (DatosDelUsuario.Rows.Count != 0)
                {
                    for (int u = 0; u < DatosUsuarios.Rows.Count; u++)
                        if (DatosDelUsuario.Rows[u]["Usuario"].ToString() == UsuarioNEW)
                        {
                            DatosDelUsuario.Rows.RemoveAt(u);
                            MessageBox.Show("Usuario elminado");
                        }
                    MessageBox.Show("No se eliminoal usuario");
                }
                else
                    MessageBox.Show("No existe el usuario elegido");
            }
            dataGrid.ItemsSource = DatosUsuarios.AsDataView();
        }
        #endregion

        #region ARCHIVO INI

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string ArchivoINI = "";
            Dispatcher.Invoke(((Action)(() => ArchivoINI = new TextRange(txtArchivoINI.Document.ContentStart, txtArchivoINI.Document.ContentEnd).Text.ToString())));
            tools.CrearArchivoINI(ArchivoINI);
            txtArchivoINI.Document.Blocks.Clear();
        }

        #endregion

        //GUARDAR TODOS LOS CAMBIOS EN LOS ARCHIVOS DEL SERVIDOR DE BC
        private void btnGuardgarADminUsuarios_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Ecabezado = "" +
                        MainWindow.InfoProyecto + "\r\n" +
                        "Fecha Actualizacion: " + DateTime.Now.ToString("dd-MM-yy") + " " + DateTime.Now.ToString("HH:mm:ss");
                string Version = "VERSION:\r\n" + txtVersionNueva.Text;
                string Novedades = "NOVEDAES:\r\n" + new TextRange(txtNovedades.Document.ContentStart, txtNovedades.Document.ContentEnd).Text.ToString();
                ///Database
                DataTable DTdatabaseF = ((DataView)dataGrid.ItemsSource).ToTable();
                string Tabla = string.Join(Environment.NewLine, DTdatabaseF.Rows.OfType<DataRow>().Select(x => string.Join("\t", x.ItemArray)));
                string DataBase = "DATABASE:\r\n" + tools.EncriptaTexto(Tabla.Replace("\t\r\n", "\r\n") + "\r\n");
                ///InfoGral
                string iniahora = new TextRange(txtArchivoINI.Document.ContentStart, txtArchivoINI.Document.ContentEnd).Text.ToString();
                string Infogral = "INFOGENERAL:\r\n" + tools.EncriptaTexto(iniahora);
                string TextoFinal = Ecabezado + "\r\n^\r\n" + Version + "\r\n^\r\n" + Novedades + "\r\n^\r\n" + DataBase + "\r\n^\r\n" + Infogral;
                ///GUARDAR - INFOGENERAL
                switch (this.Tipo)
                {
                    case "LOCAL":
                        File.WriteAllText(Pages.SplashWindow.RutaInfoWWW + @"/InfoNubot.inf", TextoFinal);
                        tools.MessageShowOK_2("Datos de InfoNubot guardados correctamente!");
                        break;
                    case "HTTP":
                        string phpUpdater = Pages.SplashWindow.RutaInfoWWW + @"/updateFILE.php?" + "filename=InfoNubot.inf" + "&" + "texto=" + TextoFinal;
                        WebClient client = new WebClient();
                        client.Proxy = WebRequest.DefaultWebProxy;
                        client.Credentials = System.Net.CredentialCache.DefaultCredentials;
                        client.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                        string datos = client.DownloadString(phpUpdater);
                        if (datos.Contains("correctamente"))
                            tools.MessageShowOK_2("Datos de InfoNubot guardados correctamente!");
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                tools.MessageShowOK_2("Error: " + ex.Message.ToString(), "ERROR");
            }
            ///GUARDAR - INFOGENERAL
            try
            {
                string licenciasnuevas = new TextRange(txtLicencias.Document.ContentStart, txtLicencias.Document.ContentEnd).Text.ToString();
                string phpUpdater = "http://www.nu4itautomation.com/cliente/docs/updateFILE.php?" + "filename=licencias.inf" + "&" + "texto=" + licenciasnuevas;
                ///GUARDAR - LICENCIAS
                switch (this.Tipo)
                {
                    case "LOCAL":
                        File.WriteAllText(Pages.SplashWindow.RutaInfoWWW + @"/licencias.inf", licenciasnuevas);
                        tools.MessageShowOK_2("Datos de Licencias guardados correctamente!");
                        break;
                    case "HTTP":
                        WebClient client = new WebClient();
                        client.Proxy = WebRequest.DefaultWebProxy;
                        client.Credentials = System.Net.CredentialCache.DefaultCredentials;
                        client.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                        string datos = client.DownloadString(phpUpdater);
                        if (datos.Contains("correctamente"))
                            tools.MessageShowOK_2("Datos de Licencias guardados correctamente!");
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                tools.MessageShowOK_2("Error: " + ex.Message.ToString(), "ERROR");
            }
        }
    }
}
