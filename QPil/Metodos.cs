#region LIBRERIAS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//LIBRERIAS AGREGADAS 
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Reflection;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;
using System.Threading;
using System.Collections;
using System.Data;
using System.Net;
using System.ComponentModel;
using forms = System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Windows.Threading;
using Outlook = Microsoft.Office.Interop.Outlook;
using Nu4it;
using nu4itExcel;
using nu4itFox;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Windows.Media.Animation;

#endregion

namespace QPil
{
    /*********************************************************
     * Resources.Clases.Metodos QUE AYUDAN A LA EJECUCIÓN EN WPF
     * JORGE NÚÑEZ 
     * 14 ENERO 2017
     * ACTUALIZACION: 08 Enero 2018
     *********************************************************/

    class Metodos : UserControl
    {

        #region VARIABLES

        //VARIABLES GLOBALES
        const String ContraseniaEncriptado = "WalMnu4";
        public static string RutaInfoGeneral = Pages.SplashWindow.RutaNubotVersiones + @"\InfoGeneral_" + Pages.SplashWindow.NombreNubot + ".ini";
        public static string RutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Desktop\";
        public static string RutaDescargas = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\";

        #endregion

        #region COLORES

        //COLORES
        public static SolidColorBrush Vacio = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));          //#00000000
        public static SolidColorBrush Azul = new SolidColorBrush(Color.FromArgb(0xFF, 0x24, 0x71, 0xA3));           //#FF2471A3        
        public static SolidColorBrush AzulBoton = new SolidColorBrush(Color.FromArgb(0xFF, 0x27, 0x73, 0xA4));      //#FF2773a4   
        public static SolidColorBrush AzulClaro = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x9D, 0xE6));      //#FF009DE6
        public static SolidColorBrush AzulOscuro = new SolidColorBrush(Color.FromArgb(0xFF, 0x25, 0x51, 0x6E));     //#FF25516E
        public static SolidColorBrush AzulOscuro2 = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x3A, 0x61));    //#FF003A61
        public static SolidColorBrush RojoBoton = new SolidColorBrush(Color.FromArgb(0xFF, 0xBF, 0x08, 0x08));      //#FFBF0808
        public static SolidColorBrush RojoOscuro = new SolidColorBrush(Color.FromArgb(0xFF, 0xBF, 0x08, 0x08));     //#FF8D2828
        public static SolidColorBrush RojoOscuro2 = new SolidColorBrush(Color.FromArgb(0xFF, 0x96, 0x00, 0x2B));    //#FF96002B
        public static SolidColorBrush AzulLogin = new SolidColorBrush(Color.FromArgb(0xFF, 0x15, 0x4E, 0xB0));      //#FF154EB0
        public static SolidColorBrush AzulLoginOsc = new SolidColorBrush(Color.FromArgb(0xFF, 0x1C, 0x47, 0x91));   //#FF1C4791
        public static SolidColorBrush Naranja = new SolidColorBrush(Color.FromArgb(0xFF, 0xE0, 0x4A, 0x32));        //#FFE04A32
        public static SolidColorBrush GrisLetra = new SolidColorBrush(Color.FromArgb(0xFF, 0xCD, 0xCD, 0xCD));      //#FFCDCDCD
        public static SolidColorBrush GrisClaro = new SolidColorBrush(Color.FromArgb(0xFF, 0xCD, 0xD8, 0xE2));      //#FFCDD8E2
        public static SolidColorBrush GrisOsc = new SolidColorBrush(Color.FromArgb(0xFF, 0x9E, 0xC1, 0xE0));        //#FF9EC1E0
        public static SolidColorBrush Blanco = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));        //#FFFFFFFF
        public static SolidColorBrush Negro = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00));        //#FF000000
        public static SolidColorBrush Verde = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0x66));         //#339966
        ///BOTONES
        public static SolidColorBrush btn_Verde = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xC7, 0x1B));        //#FF00C71B
        public static SolidColorBrush btn_Naranja = new SolidColorBrush(Color.FromArgb(0xFF, 0xF9, 0x93, 0x00));         //#FFF99300
        public static SolidColorBrush btn_Rojo = new SolidColorBrush(Color.FromArgb(0xFF, 0xE0, 0x0A, 0x0A));          //#FFE00A0A
        //PASTELES
        public static SolidColorBrush Morado_pastel = new SolidColorBrush(Color.FromArgb(0xFF, 0xBD, 0xA0, 0xD1));
        public static SolidColorBrush Verde_pastel = new SolidColorBrush(Color.FromArgb(0xFF, 0xBD, 0xF0, 0xD1));
        public static SolidColorBrush Rojo_pastel = new SolidColorBrush(Color.FromArgb(0xFF, 0xEC, 0xB5, 0xB5));       //#FFECB5B5
        public static SolidColorBrush Azul_pastel = new SolidColorBrush(Color.FromArgb(0xFF, 0xA1, 0xC4, 0xDC));       //#FFA1C4DC
        #endregion

        #region OUTLOOK

        //ENVIA A LOS CORREOS DE BC LA BITACORA DEL BOT EJECUTADO
        public void EnviarLOGaCorreo(string RutaLOG, string Titulo)
        {
            try
            {
                string lineaTexto = "";
                if (File.Exists(RutaLOG))
                {
                    StreamReader sr = new StreamReader(RutaLOG);
                    lineaTexto = sr.ReadToEnd();
                    sr.Close();
                }
                Outlook.Application miOutlook = new Outlook.Application();
                Outlook.MailItem mail = (Outlook.MailItem)miOutlook.CreateItem(Outlook.OlItemType.olMailItem);
                mail.To = "jorge.nunez@bestcollect.com.mx";
                mail.Subject = "Bitacora " + Titulo + ": " + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year + "-" + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                mail.Body = lineaTexto;
                mail.CC = "eduardo.meza@bestcollect.com.mx";
                //mail.Attachments.Add(RutaLOG);
                mail.Importance = Outlook.OlImportance.olImportanceNormal;
                ((Outlook._MailItem)mail).Send();
            }
            catch (Exception)
            {

            }
        }

        //ENVIAR CORREO A
        public bool EnviarCorreo(string Titulo, string lineaTexto, string Destinatario, string CopiasA)
        {
            try
            {
                Outlook.Application miOutlook = new Outlook.Application();
                Outlook.MailItem mail = (Outlook.MailItem)miOutlook.CreateItem(Outlook.OlItemType.olMailItem);
                mail.To = Destinatario;
                mail.Subject = Titulo;
                mail.Body = lineaTexto;
                if (CopiasA != "" && CopiasA.Contains("@"))
                    mail.CC = CopiasA;
                //mail.Attachments.Add(RutaLOG);
                mail.Importance = Outlook.OlImportance.olImportanceNormal;
                ((Outlook._MailItem)mail).Send();
                return true;
            }
            catch (Exception ex)
            {
                MessageShowOK_2("No se logró enviar el correo.\n\n" + ex.Message.ToString(), "ERROR");
            }
            return false;
        }

        //PASA UN DATATABLE A FORMATO DE TABLA PARA CORREO
        public string ConvierteDTaSTRINGParaCorreo(DataTable Tabla)
        {
            string contenido = "";
            int ro = Tabla.Columns.Count;
            contenido += "<tr>";
            foreach (DataColumn dtc in Tabla.Columns)
            {
                contenido += "<th>" + dtc.ColumnName + "</th>";
            }
            contenido += "</tr>";
            foreach (DataRow i in Tabla.Rows)
            {

                if (i.RowState != DataRowState.Deleted)
                {
                    contenido += "<tr>";
                    for (int e = 0; e < ro; e++)
                    {
                        contenido += "<td>" + i[e] + "</td>";
                    }
                    contenido += "</tr>";
                }
            }
            return (contenido);
        }

        #endregion

        #region DIALOGOS DE MENSAJE ( FILE Y MESSAGE )

        //FOLDER DIALOG
        public string FolderDialog()
        {
            string Path = String.Empty;
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                Path = dialog.SelectedPath;
            return Path;
        }

        //FILE DIALOG
        public string FileDialog(string Mensaje, string TipoArchivo)
        {
            string FilePath = String.Empty;
            string FiltroArchivo = String.Empty;
            MessageBox.Show(Mensaje, "Aviso", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Title = Mensaje;
            switch (TipoArchivo)
            {
                case "Excel":
                    FiltroArchivo = "Excel Files|*.xls;*.xlsx;*.xlsb;*.xlsm";
                    break;
                default:
                    break;
            }
            dialog.Filter = FiltroArchivo;
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
                FilePath = dialog.FileName;
            return FilePath;
        }

        //MESSAGE SHOW OK
        public void MessageShowOK_1(string Aviso)
        {
            MessageBox.Show(Aviso, "Aviso", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }

        //MESSAGE SHOW YES NO
        public bool MessageShowYesNo_1(string Pregunta)
        {
            bool Respuesta = false;
            if (MessageBox.Show(Pregunta, "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Respuesta = true;
            return Respuesta;
        }

        //MESSAGE SHOW OK 2
        /// <summary>
        /// Ventana con fondo azul tipo Nü4it
        /// </summary>
        /// <param name="Aviso">Mensaje que sera mostrado</param>
        /// <param name="TipoVentana">
        /// "OK"
        /// "OK2"
        /// "ALERTA"
        /// "ALERTA2"
        /// "ERROR"
        /// </param>
        public void MessageShowOK_2(string Aviso, string TipoVentana)
        {
            try
            {
                //dynamic msgOK = (dynamic)null;
                Pages.Mensajes.MnsjOK msgOK = null;
                Dispatcher.Invoke(((Action)(() => msgOK = new Pages.Mensajes.MnsjOK(TipoVentana))));
                Dispatcher.Invoke(((Action)(() => msgOK.lblAvisoContent.Text = Aviso)));
                Dispatcher.Invoke(((Action)(() => msgOK.ShowDialog())));
            }
            catch (Exception)
            {

            }
        }

        //MESSAGE SHOW OK 2
        public void MessageShowOK_2(string Aviso)
        {
            try
            {
                dynamic msgOK = (dynamic)null;
                Dispatcher.Invoke(((Action)(() => msgOK = new Pages.Mensajes.MnsjOK(""))));
                Dispatcher.Invoke(((Action)(() => msgOK.lblAvisoContent.Text = Aviso)));
                Dispatcher.Invoke(((Action)(() => msgOK.ShowDialog())));
            }
            catch (Exception)
            {

            }
        }

        //MESSAGE SHOW YES NO 2
        Pages.Mensajes.MsgYesNo objYesNo = new Pages.Mensajes.MsgYesNo();
        public bool MessageShowYesNo_2(string Pregunta)
        {
            bool Respuesta = false;
            try
            {
                dynamic objYesNo = (dynamic)null;
                Dispatcher.Invoke(((Action)(() => objYesNo = new Pages.Mensajes.MsgYesNo())));
                Dispatcher.Invoke(((Action)(() => objYesNo.lblAvisoContent.Text = Pregunta)));
                Dispatcher.Invoke(((Action)(() => objYesNo.ShowDialog())));
                if (Pages.SplashWindow.RespuestaYesNo)
                    Respuesta = true;
            }
            catch (Exception)
            {

            }
            return Respuesta;
        }
        #endregion

        #region ARCHIVO INI

        //OBTIENDO LOS DATOS DEL ARCHIVO INI
        public string[] ObtenerDatosINI()
        {
            if (!File.Exists(RutaInfoGeneral))
            {
                CrearArchivoINI(Pages.SplashWindow.InfoGral);
                Thread.Sleep(1000);
            }
            if (File.Exists(RutaInfoGeneral))
                DesProtegerArchivo(RutaInfoGeneral);
            string lineaTexto = "";
            string[] DATOS = new string[100];
            char[] delimiterChars = { '\r', '\n' };
            if (File.Exists(RutaInfoGeneral))
            {
                StreamReader sr = new StreamReader(RutaInfoGeneral);
                lineaTexto = sr.ReadToEnd();
                sr.Close();
            }
            string datosdesc = DesencriptaTexto(lineaTexto);
            if (datosdesc == "")
            {
                File.Delete(RutaInfoGeneral);
                DATOS = ObtenerDatosINI();
            }
            DATOS = DesencriptaTexto(lineaTexto).Split(delimiterChars);
            DATOS = DATOS.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            if (File.Exists(RutaInfoGeneral))
                ProtegerArchivo(RutaInfoGeneral);
            return DATOS;
        }

        //OBTIENDO LOS DATOS DEL ARCHIVO INI
        public string ObtenerDatosINI_TEXTO()
        {
            string textoini = "";
            string[] Datos = ObtenerDatosINI();
            for (int i = 0; i < Datos.Length; i++)
            {
                textoini += Datos[i];
                if ((i + 1) != Datos.Length)
                    textoini += "\n";
            }
            return textoini;
        }

        //OBTIENDO LOS DATOS DEL ARCHIVO INI_INDIVIDUALMENTE
        public List<string> ObtenerDatosDeIni_INDIVIDUAL(string Cliente)
        {
            List<string> datos = new List<string>();
            string[] DatosIndividual = ObtenerDatosINI();
            int pos = 0, cont = 0;
            for (int i = 0; i < DatosIndividual.Length; i++)
            {
                string A = DatosIndividual[i].ToString().Replace("[", "").Replace("]", "");
                string B = A.Replace("-", "").Replace(" ", "");
                string C = B.Replace("INICIO", "").Replace("FIN", "");

                if (DatosIndividual[i].ToString().Replace("[", "").Replace("]", "").Replace("-", "").Replace(" ", "").Replace("INICIO", "").Replace("FIN", "").Equals(Cliente))
                {
                    pos = i;
                    while (!DatosIndividual[pos + 1].ToString().Replace("[", "").Replace("]", "").Replace("-", "").Replace(" ", "").Replace("INICIO", "").Replace("FIN", "").Equals(Cliente))
                    {
                        if (DatosIndividual[pos + 1].ToString() != "")
                        {
                            cont++;
                            datos.Add(DatosIndividual[pos + 1]);
                        }
                        pos++;
                        if (pos == DatosIndividual.Length - 1)
                            break;
                    }
                    break;
                }
            }
            return datos;
        }

        //OBTENIENDO LOS DATOS DEL ARCHIVO INI DE INTERFACTURA
        public String[] ObtnerDatosArchivoIni_Robot(String nombre)
        {
            String[] datos = new String[5];
            List<string> Datos = ObtenerDatosDeIni_INDIVIDUAL(nombre.ToUpper());
            foreach (var item in Datos)
            {
                string linea = item.ToString();
                /*
                if (linea.Contains("URL="))
                    datos[0] = linea.Replace("URL=", "");
                if (linea.Contains("USUARIO="))
                    datos[1] = linea.Replace("USUARIO=", "");
                if (linea.Contains("CONTRASEÑA="))
                    datos[2] = linea.Replace("CONTRASEÑA=", "");
                */
            }
            datos = Datos.ToArray();  //Linea Extra
            return datos;
        }

        //GUARDAR DATO EN EL INI
        public void GuardarDatoEnIni(string Variable, string ValorNuevo)
        {
            try
            {
                string[] DATOS = ObtenerDatosINI();
                for (int i = 0; i < DATOS.Length; i++)
                {
                    string AllTexto = "";
                    if (DATOS[i].ToString().Contains("="))
                    {
                        if (DATOS[i].ToString().Substring(0, DATOS[i].IndexOf('=')) == Variable)
                        {
                            DATOS[i] = Variable + "=" + ValorNuevo;
                            for (int x = 0; x < DATOS.Length; x++)
                            {
                                AllTexto += DATOS[x];
                                if ((x + 1) != DATOS.Length)
                                    AllTexto += "\n";
                            }
                            if (File.Exists(RutaInfoGeneral))
                                DesProtegerArchivo(RutaInfoGeneral);
                            File.Delete(RutaInfoGeneral);
                            AllTexto = EncriptaTexto(AllTexto);
                            File.AppendAllText(RutaInfoGeneral, AllTexto);
                            if (File.Exists(RutaInfoGeneral))
                                ProtegerArchivo(RutaInfoGeneral);
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        //OBTIENE EL VALOR DE LA VARIBLE QUE SE BUSCA EN EL ARCHIVO INI
        public string ObtenerDatoDeIni(string Variable)
        {
            string Dato = "";
            string[] DATOS = ObtenerDatosINI();
            foreach (var item in DATOS)
                if (item.StartsWith(Variable))
                    Dato = item.Substring(item.IndexOf("=") + 1, item.Length - item.IndexOf("=") - 1);
            return Dato;
        }

        //GUARDAR DATO EN EL INI
        public bool GuardarDatoDeArchivo(string RutaArchivo, string Variable, string ValorNuevo)
        {
            try
            {
                string[] DATOS = File.ReadAllLines(RutaArchivo);
                for (int i = 0; i < DATOS.Length; i++)
                {
                    string AllTexto = "";
                    if (DATOS[i].ToString().Contains("="))
                    {
                        if (DATOS[i].ToString().Substring(0, DATOS[i].IndexOf('=')) == Variable)
                        {
                            DATOS[i] = Variable + "=" + ValorNuevo;
                            for (int x = 0; x < DATOS.Length; x++)
                            {
                                AllTexto += DATOS[x];
                                if ((x + 1) != DATOS.Length)
                                    AllTexto += Environment.NewLine;
                            }
                            File.Delete(RutaArchivo);
                            Thread.Sleep(100);
                            File.AppendAllText(RutaArchivo, AllTexto);
                            Thread.Sleep(100);
                            if (File.Exists(RutaArchivo))
                                return true;
                            return false;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        //OBTIENE EL VALOR DE LA VARIBLE QUE SE BUSCA EN EL ARCHIVO INI
        public string ObtenerDatoDeArchivo(string RutaArchivo, string Variable)
        {
            string Dato = "";
            string[] DATOS = File.ReadAllLines(RutaArchivo);
            foreach (var item in DATOS)
                if (item.StartsWith(Variable))
                    Dato = item.Substring(item.IndexOf("=") + 1, item.Length - item.IndexOf("=") - 1);
            return Dato;
        }

        //CREAR ARCHIVO INI SI NO EXISTE POR MEDIO DEL HTTP
        public bool CrearArchivoINI(string ALLTEXT)
        {
            bool exito = false;
            try
            {
                if (File.Exists(RutaInfoGeneral))
                {
                    DesProtegerArchivo(RutaInfoGeneral);
                    Thread.Sleep(200);
                    File.Delete(RutaInfoGeneral);
                }
                ALLTEXT = EncriptaTexto(ALLTEXT);
                File.AppendAllText(RutaInfoGeneral, ALLTEXT);
                Thread.Sleep(200);
                ProtegerArchivo(RutaInfoGeneral);
                MessageShowOK_2("Instalación de actualizaciones correctamente!", "OK");
            }
            catch (Exception)
            {
                MessageShowOK_2("ERROR A CREAR INI", "OK");
            }

            return exito;
        }

        //CREAR ARCHIVO INI EN MODO LOCAL
        public void CrearArchivoINI_LOCAL()
        {
            if (!File.Exists(RutaInfoGeneral))
            {
                string ALLTEXT = ""
                    + "[▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬ " + Pages.SplashWindow.NombreNubot.ToUpper() + " ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬]" + Environment.NewLine
                    + "•" + Environment.NewLine
                    + "[----------------------- DATOS DEL ROBOT -------------------------]" + Environment.NewLine
                    + "NOMBRE=" + Pages.SplashWindow.NombreNubot + Environment.NewLine
                    + "INFOHHTP=InfoHTTP_" + Pages.SplashWindow.NombreNubot + Environment.NewLine
                    + "[--------------------- FIN DATOS DEL ROBOT -----------------------]" + Environment.NewLine
                    + "•" + Environment.NewLine
                    + "[---------------------- DATOS DEL SISTEMA ------------------------]" + Environment.NewLine
                    + "VERSION=1" + Environment.NewLine
                    + "VERLBL=1.0.0" + Environment.NewLine
                    + "LICENCIA=licencias" + Environment.NewLine
                    + "TIEMPODESESION=16/01/2017 00:00:00 a. m." + Environment.NewLine
                    + "ULTIMASESION=11/01/2017 00:00:00 a. m." + Environment.NewLine
                    + "USUARIOACTUAL=admin" + Environment.NewLine
                    + "PASSUSUACTUAL=admin" + Environment.NewLine
                    + "[--------------------- FIN DATOS DEL SISTEMA ---------------------]" + Environment.NewLine
                    + "•" + Environment.NewLine
                    + "[▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬ FIN ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬]" + Environment.NewLine
                    ;
                try
                {
                    ALLTEXT = EncriptaTexto(ALLTEXT);
                    File.AppendAllText(RutaInfoGeneral, ALLTEXT);
                    Thread.Sleep(200);
                    ProtegerArchivo(RutaInfoGeneral);
                    MessageBox.Show("Listo!\n\nReinicia el robot para efectuar los cambios.");
                }
                catch (Exception)
                {
                    MessageBox.Show("ERROR A CREAR INI");
                }
            }
        }

        #endregion

        #region MANEJO DE ARCHIVOS

        //PROTEGER EL ARHIVO CONTRA ESCRITURA BY MARIO ANDRES
        public void ProtegerArchivo(string file)
        {
            File.SetAttributes(file, FileAttributes.Archive |
                                     FileAttributes.Hidden |
                                     FileAttributes.ReadOnly);
        }

        //PROTEGER EL ARHIVO CONTRA ESCRITURA BY MARIO ANDRES
        public void DesProtegerArchivo(string file)
        {
            File.SetAttributes(file, FileAttributes.Archive |
                                     FileAttributes.Hidden |
                                     FileAttributes.Normal);
        }

        //CREAR CARPETA
        public void CheckandCreateFolder(String pathFolder)
        {
            if (!Directory.Exists(pathFolder))
            {
                Directory.CreateDirectory(pathFolder);
            }
        }

        #endregion

        #region  DES-EN-CRIPTAR TEXTO

        //ENCRIPTA EL TEXTO EN UTF8Encoding BY MARIO ANDRES
        public String EncriptaTexto(String StrTextoEncriptar)
        {
            String Resultado = String.Empty;
            try
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                byte[] ContraseniaArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(ContraseniaEncriptado));
                byte[] ArregloTexto = UTF8Encoding.UTF8.GetBytes(StrTextoEncriptar);
                hashmd5.Clear();
                tdes.Key = ContraseniaArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] ArrayResultado = cTransform.TransformFinalBlock(ArregloTexto, 0, ArregloTexto.Length);
                tdes.Clear();
                Resultado = Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);
            }
            catch (Exception ex)
            {
                string mensaje = "Error inesperado en el generado de informacion base, verifique!!";
                MessageShowOK_2(mensaje, "OK");
            }
            return Resultado;
        }

        //DESCENCRIPTA EL TEXTO BY MARIO ANDRES
        public String DesencriptaTexto(String Encriptado)
        {
            String Resultados = Encriptado;
            try
            {
                byte[] keyArray;
                byte[] Array_a_Descifrar = Convert.FromBase64String(Encriptado);
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(ContraseniaEncriptado));
                hashmd5.Clear();
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);
                tdes.Clear();
                Resultados = UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception)
            {
                string mensaje = "Error inesperado en el generado de informacion base, verifique!!";
                //MessageBox.Show(mensaje, "Actualización", MessageBoxButton.YesNo, MessageBoxImage.Question);
                MessageShowOK_2(mensaje, "OK");
            }
            return Resultados;
        }
        #endregion

        #region MANEJO DE COMPUTADORA

        //APAGAR COMPUTADORA
        public void ApagarComputadora(bool apagar)
        {
            //*************************Apagar CPU*****************************************************
            if (apagar)
            {
                Process hibernar = new Process();
                hibernar.StartInfo.UseShellExecute = false;
                hibernar.StartInfo.FileName = "shutdown";
                hibernar.StartInfo.Arguments = "-s";
                hibernar.Start();
            }
        }

        //HIMBERNA COMPUTADORA
        public void HimbernaComputadora(bool himberna)
        {
            //*************************Hibernar CPU*****************************************************
            if (himberna)
            {
                Process hibernar = new Process();
                hibernar.StartInfo.UseShellExecute = false;
                hibernar.StartInfo.FileName = "shutdown";
                hibernar.StartInfo.Arguments = "-h";
                hibernar.Start();
            }
        }

        //FUNCIÓN QUE REGRESA LA MACADDRESS DE LA COMPUTADORA DONDE SE EJECUTA EL PROGRAMA.
        public string ObtenMacAddress()
        {
            string id = "";
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            id = sMacAddress;
            return (id);
        }

        //METODO QUE REGRESA UN ARRELGO DE LAS MAC Y DE LA CONEXION DE LA MISMA
        public string[] ObtenerConexionesInternet()
        {
            string[] ids = new string[20];
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            int i = 0;
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                string sMacAddress = adapter.GetPhysicalAddress().ToString();
                ids[i] = adapter.Name + " - " + sMacAddress;
                i++;
            }
            return (ids);
        }

        //TE INDICA SI ESTA CONECTADO EL CABLE DE RED ETHERNET
        public bool ConnectionByEthernet()
        {
            bool continuar = false;
            string[] Macs = ObtenerConexionesInternet();
            string MacActual = ObtenMacAddress();
            foreach (string item in Macs)
            {
                if (item != null)
                {
                    if (item.Contains("local") && item.Contains(MacActual))
                    {
                        continuar = true;
                        break;
                    }
                }
            }
            if (!continuar)
            {
                MessageShowOK_2("Conecta el cable de Ethernet para continuar.", "OK");
            }
            return continuar;
        }

        //TE INDICA SI ESTA CONECTADO EL CABLE DE BATERIA  var bateria = System.Windows.Forms.SystemInformation.PowerStatus.BatteryChargeStatus;
        public bool CableDeLuzConectado()
        {
            bool conectado = false;
            string linea = System.Windows.Forms.SystemInformation.PowerStatus.PowerLineStatus.ToString();
            if (linea.Equals("Online"))
                conectado = true;
            if (!conectado)
            {
                MessageShowOK_2("Conecta la computadora a la corriente.", "OK");
            }
            return conectado;
        }

        #endregion

        #region LICENCIAS

        //CHECADO DE LICENCIAS PARA EL ROBOT
        public bool ChecarLicencia(string Metodo, string MAC)
        {
            bool licencia = false;
            if (Metodo == "HTTP")
                Pages.SplashWindow.Licencias = getHTTP(Pages.SplashWindow.RutaInfoWWW + "/licencias.inf").Replace("\r", "");
            else if (Metodo == "LOCAL")
            {
                if (File.Exists(Pages.SplashWindow.RutaInfoWWW + "/licencias.inf"))
                    Pages.SplashWindow.Licencias = File.ReadAllText(Pages.SplashWindow.RutaInfoWWW + "/licencias.inf").Replace("\r", "");
                else
                    MessageShowOK_2("No se encuentra instalado el archivo de Licencias!", "OK");
            }
            string[] Datos = (QPil.Pages.SplashWindow.Licencias).Split('\n');
            if (Datos[0] != "")
            {
                string[] Macs = new string[200];
                for (int i = 0; i < Datos.Length; i++)
                {
                    if (Datos[i] != "")
                    {
                        int tam = Datos[i].Length;
                        int ind = Datos[i].IndexOf("*");
                        if (ind <= 0)
                            Macs[i] = Datos[i].Substring(0, tam);
                        else
                            Macs[i] = Datos[i].Substring(0, ind);
                    }
                }
                if (Macs.Contains(MAC))
                    licencia = true;
            }
            else
            {
                MessageShowOK_2("No se obtuvo información del servidor", "ALERTA");
                licencia = false;
            }
            return (licencia);
        }

        #endregion

        #region METODOS PARA NUBOT

        //REINICIAR
        public void ReiniciarAccesoDirecto()
        {
            //string exe = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Nü4it Automation\QPil.appref-ms";
            //if (File.Exists(exe))
            //    System.Diagnostics.Process.Start(exe);
            //else
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
            App.Current.Shutdown();
        }

        //OBTENER EL PERMISO DE 24HRS
        public bool UltimaSesion()
        {
            bool permiso = false;
            DateTime TiempoGuardado = DateTime.Now;
            DateTime.TryParse(ObtenerDatoDeIni("ULTIMASESION"), out TiempoGuardado);

            DateTime TimpoActual = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            TimeSpan Comparacion = TimpoActual - TiempoGuardado;
            if (Comparacion >= TimeSpan.FromDays(1))
            {
                GuardarDatoEnIni("ULTIMASESION", TimpoActual.ToString());
                permiso = false;
            }
            else
            {
                permiso = true;
            }
            return permiso;
        }

        //TIEMPO DE SESION (1HR)
        public bool TiempoDeSesion()
        {
            bool nuevasesion = false;
            DateTime TiempoGuardado = DateTime.Now;
            DateTime.TryParse(ObtenerDatoDeIni("TIEMPODESESION"), out TiempoGuardado);
            DateTime TimpoActual = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            TimeSpan Comparacion = TimpoActual - TiempoGuardado;
            if (Comparacion > TimeSpan.FromMinutes(90))
                nuevasesion = true;
            return nuevasesion;
        }

        //OBTENER LOS DATOS DEL SERVIDOR
        public bool ObtenerDatosServidor(string TIPO)
        {
            string MAC = ObtenMacAddress();
            switch (TIPO)
            {
                case "HTTP":
                    try
                    {
                        string InfoNUBOT = getHTTP(Pages.SplashWindow.RutaInfoWWW + "/InfoNubot.inf").Replace("\r", "");
                        string[] Datos = InfoNUBOT.Replace("\n^\n", "♠").Split('♠');
                        ///Pages.SplashWindow.Version = Datos[1].Replace("VERSION:\n", "");
                        ///Pages.SplashWindow.Novedades = Datos[2].Replace("NOVEDAES:\n", "");
                        Pages.SplashWindow.DataBase = DesencriptaTexto(Datos[3].Replace("DATABASE:\n", "").Replace(" ", "+"));
                        Pages.SplashWindow.InfoGral = DesencriptaTexto(Datos[4].Replace("INFOGENERAL:\n", "").Replace(" ", "+"));
                        if (Pages.SplashWindow.DataBase != "")
                            return true;
                    }
                    catch (Exception e2)
                    {
                        MessageShowOK_1("No hay conexión a Internet, conectate e intenta nuevamente!");
                        return false;
                    }
                    break;
                case "HTML":
                    try
                    {
                        string[] txts = new string[] { "licencias", "version", "novedades", "infogral", "database" };
                        string[] Datos = GetHTMLTxt("/cliente/docs/infonubot.html", txts);
                        Pages.SplashWindow.Licencias = DesencriptaTexto(Datos[0]);
                        Pages.SplashWindow.Version = DesencriptaTexto(Datos[1]);
                        Pages.SplashWindow.Novedades = DesencriptaTexto(Datos[2]);
                        Pages.SplashWindow.InfoGral = DesencriptaTexto(Datos[3]);
                        Pages.SplashWindow.DataBase = DesencriptaTexto(Datos[4]);
                        if (Pages.SplashWindow.DataBase != "" && Pages.SplashWindow.Licencias != "" && Pages.SplashWindow.Novedades != "" && Pages.SplashWindow.Version != "")
                            return true;
                    }
                    catch (Exception e3)
                    {
                        MessageShowOK_1("Se produjo un error al busar la pagina HTML");
                    }
                    break;
                case "LOCAL":
                    try
                    {
                        if (File.Exists(Pages.SplashWindow.RutaInfoWWW + "/InfoNubot.inf"))
                        {
                            string InfoNUBOT = File.ReadAllText(Pages.SplashWindow.RutaInfoWWW + "/InfoNubot.inf").Replace("\r", "");
                            string[] Datos = InfoNUBOT.Replace("\n^\n", "♠").Split('♠');
                            Pages.SplashWindow.DataBase = DesencriptaTexto(Datos[3].Replace("DATABASE:\n", "").Replace(" ", "+"));
                            Pages.SplashWindow.InfoGral = DesencriptaTexto(Datos[4].Replace("INFOGENERAL:\n", "").Replace(" ", "+"));
                            if (Pages.SplashWindow.DataBase != "")
                                return true;
                        }
                        else
                            MessageShowOK_2("No existe el archivo local de información.", "ALERTA");
                    }
                    catch (Exception e4)
                    {

                    }
                    break;
                default: break;
            }
            return false;
        }

        //ABRIR VENTANA DESPUES DE INGRESAR LA CONTRASEÑA DE DESARROLLADOR
        public bool AbrirVentanaDESARROLLADOR()
        {
            dynamic txtboxResp = (dynamic)null;
            Dispatcher.Invoke(((Action)(() => txtboxResp = new Pages.Mensajes.TextBox("Ingresa la contraseña de Desarrollador", 152, 404))));
            Dispatcher.Invoke(((Action)(() => txtboxResp.ShowDialog())));
            bool rsp = false;
            Dispatcher.Invoke(((Action)(() => rsp = (bool)txtboxResp.DialogResult)));
            if (rsp)
            {
                string Password = "";
                Dispatcher.Invoke(((Action)(() => Password = txtboxResp.Answer)));
                if (Password == "BCnu4it")
                    return true;
                else
                    MessageBox.Show("Contraseña incorrecta!");
            }
            return false;
        }

        //OBTENER DATOS DE LA COMPUTADORA
        public string DatosDeComputadora()
        {
            string DatosComputadora = "";
            try
            {
                DatosComputadora = "DateTime: " + DateTime.Now.ToString("dd/MM/yyyy") + "  -  " + DateTime.Now.ToString("HH:MM:ss") + Environment.NewLine +
        "MAC: " + ObtenMacAddress() + Environment.NewLine +
        "UserName: " + Environment.UserName + Environment.NewLine +
        "UserDomainName: " + Environment.UserDomainName + Environment.NewLine +
        "Domain: " + System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName + Environment.NewLine +
        "HostName: " + System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().HostName + Environment.NewLine +
        "OSVersion: " + Environment.OSVersion + Environment.NewLine +
        "MachineName: " + Environment.MachineName + Environment.NewLine +
        "Version: " + Environment.Version + Environment.NewLine +
        "CurrentDirectory: " + Environment.CurrentDirectory + Environment.NewLine +
        "ProcessorCount: " + Environment.ProcessorCount + Environment.NewLine +
        "Is64BitProcess: " + Environment.Is64BitProcess + Environment.NewLine;
                return DatosComputadora;
            }
            catch (Exception)
            {

            }
            return "";
        }

        #endregion

        #region ANIMACIONES

        public void AnimacionLoaded_plush(Window Ventana)
        {
            try
            {
                ///------ Animación de Loaded --------
                Ventana.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                (Ventana.Content as Grid).BeginAnimation(Grid.OpacityProperty, new System.Windows.Media.Animation.DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(.18))));
                (Ventana.Content as Grid).BeginAnimation(Grid.WidthProperty, new System.Windows.Media.Animation.DoubleAnimation(0, (Ventana.Content as Grid).ActualWidth, new Duration(TimeSpan.FromSeconds(.18))));
                (Ventana.Content as Grid).BeginAnimation(Grid.HeightProperty, new System.Windows.Media.Animation.DoubleAnimation(0, (Ventana.Content as Grid).ActualHeight, new Duration(TimeSpan.FromSeconds(.18))));
                ///-----------------------------------
            }
            catch (Exception ex)
            {

            }
        }

        public void AnimacionUnLoaded_plush(Window Ventana)
        {
            try
            {
                ///------ Animación de Loaded --------
                Ventana.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                (Ventana.Content as Grid).BeginAnimation(Grid.OpacityProperty, new System.Windows.Media.Animation.DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(.18))));
                (Ventana.Content as Grid).BeginAnimation(Grid.WidthProperty, new System.Windows.Media.Animation.DoubleAnimation((Ventana.Content as Grid).ActualWidth, 0, new Duration(TimeSpan.FromSeconds(.18))));
                (Ventana.Content as Grid).BeginAnimation(Grid.HeightProperty, new System.Windows.Media.Animation.DoubleAnimation((Ventana.Content as Grid).ActualHeight, 0, new Duration(TimeSpan.FromSeconds(.18))));
                ///-----------------------------------
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region HTML

        //METODO PARA OBTENER EL HTML DE UNA PAGINA WEB CON MIIE
        SHDocVw.InternetExplorer MiIE;
        public string[] GetHTMLTxt(string URL, string[] ID)
        {
            MiIE = new SHDocVw.InternetExplorer();
            MiIE.Visible = false;
            string titulo = "";
            string[] texto = new string[5];
            bool seguir = false;
            do
            {
                try
                {
                    int inten = 0;
                    while (!seguir)
                    {
                        MiIE.Navigate("http://nu4itautomation.com" + URL);
                        do { } while ((Convert.ToInt16(MiIE.ReadyState) != 4) || (MiIE.Busy));
                        MiIE.Refresh();
                        do { } while ((Convert.ToInt16(MiIE.ReadyState) != 4) || (MiIE.Busy));
                        try
                        {
                            titulo = MiIE.Document.title;
                        }
                        catch (Exception)
                        {
                            titulo = MiIE.Document.IHTMLDocument2_title;
                        }
                        if (titulo.ToUpper().Contains("NU4IT"))
                            seguir = true;
                        if (titulo.ToUpper().Contains("MCAFEE"))
                        {
                            MessageShowOK_2("EL SERVIDOR HA SIDO BLOQUEADO!  =(", "ERROR");
                        }
                        if (inten == 10)
                            break;
                        Thread.Sleep(500);
                    }
                    if (seguir)
                    {
                        for (int i = 0; i < ID.Length; i++)
                            try
                            {
                                texto[i] = MiIE.Document.getElementById(ID[i]).innerhtml.ToString();
                            }
                            catch (Exception ex)
                            {
                                texto[i] = MiIE.Document.getElementById(ID[i]).IHTMLElement_innerHTML.ToString();
                            }
                    }
                }
                catch (Exception)
                {
                    seguir = false;
                }
            } while (texto[0] == null);
            if (!seguir)
            {
                MessageShowOK_2("No tienes acceso a internet, por favor verfica tu conexón.", "ERROR");
                Dispatcher.Invoke(((Action)(() => App.Current.Shutdown())));
            }
            MiIE.Quit();

            return texto;
        }

        //SUBE UN ARCHIVO HTML AL SERVIDOR
        public void SubeHTML(string Opcion, string Datos)
        {
            string version = EncriptaTexto(Pages.SplashWindow.Version.Replace("\r", ""));
            string licencias = EncriptaTexto(Pages.SplashWindow.Licencias);
            string novedades = EncriptaTexto(Pages.SplashWindow.Novedades);
            string infogral = EncriptaTexto(Pages.SplashWindow.InfoGral);
            string database = EncriptaTexto(Pages.SplashWindow.DataBase);

            switch (Opcion)
            {
                case "V":
                    version = Datos;
                    Pages.SplashWindow.Version = DesencriptaTexto(Datos);
                    break;
                case "L":
                    licencias = Datos;
                    Pages.SplashWindow.Licencias = DesencriptaTexto(Datos);
                    break;
                case "N":
                    novedades = Datos;
                    Pages.SplashWindow.Novedades = DesencriptaTexto(Datos);
                    break;
                case "I":
                    infogral = Datos;
                    Pages.SplashWindow.InfoGral = DesencriptaTexto(Datos);
                    break;
                case "D":
                    database = Datos;
                    Pages.SplashWindow.DataBase = DesencriptaTexto(Datos);
                    break;
                default:
                    break;
            }

            string html = ""
                + "<!DOCTYPE html>" + Environment.NewLine
                + "<html>" + Environment.NewLine
                + "<head>" + Environment.NewLine
                + "<title>NU4IT Automation</title>" + Environment.NewLine
                + "</head>" + Environment.NewLine
                + "<body>" + Environment.NewLine
                + "<h1>QPil" + Environment.NewLine
                + "<hr>" + Environment.NewLine

                + "<p>Actualización:    " + DateTime.Now + "</p>" + Environment.NewLine + Environment.NewLine
                + "<hr>" + Environment.NewLine

                + "<p>" + "Versión:" + "</p>" + Environment.NewLine
                + "<p id=version>" + version + "</p>" + Environment.NewLine
                + "<hr>" + Environment.NewLine

                + "<p>" + "Novedades:" + "</p>" + Environment.NewLine
                + "<p id=novedades>" + novedades + "</p>" + Environment.NewLine
                + "<hr>" + Environment.NewLine

                + "<p>" + "Licencias:" + "</p>" + Environment.NewLine
                + "<p id=licencias>" + licencias + "</p>" + Environment.NewLine
                + "<hr>" + Environment.NewLine

                + "<p>" + "Info General:" + "</p>" + Environment.NewLine
                + "<p id=infogral>" + infogral + "</p>" + Environment.NewLine
                + "<hr>" + Environment.NewLine

                + "<p>" + "Data Base:" + "</p>" + Environment.NewLine
                + "<p id=database>" + database + "</p>" + Environment.NewLine

                + Environment.NewLine + Environment.NewLine + Environment.NewLine
                + "<b>§</b>" + Environment.NewLine
                + "</body>" + Environment.NewLine
                + "</html>" + Environment.NewLine
                ;

            string FullFileName = Directory.GetCurrentDirectory() + @"\infonubot.html";

            if (File.Exists(FullFileName))
                File.Delete(FullFileName);

            File.AppendAllText(FullFileName, html);

            try
            {
                string PHPHTML = ObtenerDatoDeIni("PHPHTML");
                string Uri = "http://nu4itautomation.com/" + PHPHTML;
                WebClient WC = new WebClient();
                WC.Headers.Add("Content-Type", "binary/octet-stream");
                byte[] result = WC.UploadFile(Uri, "POST", FullFileName);
                string s = Encoding.UTF8.GetString(result, 0, result.Length);
                MessageBox.Show(Opcion + " - Se realizó actualización en el HTML!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR IMPORTANTE, POST HTML");
            }

            if (File.Exists(FullFileName))
                File.Delete(FullFileName);
        }

        #endregion

        #region STATUSSHOW

        //CERRAR EL MENSAJE DE STATUS
        Pages.Aviso aviso = new Pages.Aviso();
        public void StatusHIDE()
        {
            bool StatusActivo = false;
            Dispatcher.Invoke(((Action)(() => StatusActivo = aviso.IsActive)));
            if (StatusActivo)
                Dispatcher.Invoke(((Action)(() => aviso.Cerrar())));
        }

        //MOSTRAR EL MENSAJE DE STATUS
        public void StatusSHOW(string Mensaje)
        {
            bool StatusActivo = false;
            Dispatcher.Invoke(((Action)(() => StatusActivo = aviso.IsActive)));
            if (!StatusActivo)
                Dispatcher.Invoke(((Action)(() => aviso.Show())));
            else
            {
                Dispatcher.Invoke(((Action)(() => aviso.Cerrar())));
                Dispatcher.Invoke(((Action)(() => aviso.Show())));
            }
            Dispatcher.Invoke(((Action)(() => aviso.txtStatusContent.Text = Mensaje)));

        }
        #endregion

        #region DATOS ( DATATABLE, LINQ, STRING, LIST )

        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;

                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);

                    if (foundChild != null) break;
                }
                else
                    if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;

                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                    else
                    {
                        foundChild = FindChild<T>(child, childName);

                        if (foundChild != null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        //REGRESA EL NUMERO DE LA COLUMNA BUSCADA UN DATAGRID
        public int NumeroColumnaDATAGRID(DataGrid DTG, string Titulo)
        {
            ObservableCollection<DataGridColumn> COLUMNAS = new ObservableCollection<DataGridColumn>();
            Dispatcher.Invoke(((Action)(() => COLUMNAS = DTG.Columns)));
            int conta = -1;
            foreach (DataGridColumn columna in COLUMNAS)
            {
                if (columna.Header.ToString().Equals(Titulo) || columna.Header.ToString().Contains(Titulo))  /// || Modifica(columna.Header.ToString()).Contains(Modifica(Titulo))
                    return conta;
                conta++;
            }
            return -1;
        }

        //REGRESA EL NUMERO DE LA COLUMNA BUSCADA UN DATATABLE
        public int NumeroColumnaDATATABLE(DataTable DT, string Titulo)
        {
            int conta = 0;
            foreach (DataColumn columna in DT.Columns)
            {
                if (columna.ColumnName.ToString().Contains(Titulo) || Modifica(columna.ColumnName.ToString()).Contains(Modifica(Titulo)))
                    return conta;
                conta++;
            }
            return 0;
        }



        //LLENA UNA LISTA CON LOS ELEMENTOS UNICOS DE LA CLUMNA DE UN DATATABLE
        public List<string> DataColumnToList_String_Unique(DataTable dt, int campo)
        {
            List<String> final = new List<string>();
            try
            {
                List<String> datos = new List<String>();
                foreach (DataRow filaCampo in dt.Rows)
                {
                    try
                    {
                        string field = filaCampo.Field<dynamic>(campo).ToString();
                        datos.Add(field.TrimEnd(' '));
                    }
                    catch (Exception ex)
                    {
                        MessageShowOK_2("error en la consulta" + ex.ToString());
                    }
                }
                final = datos.Distinct().ToList();
            }
            catch (Exception ex)
            {
                MessageShowOK_2(ex.Message.ToString(), "ERROR");
            }
            return final;
        }

        //LLENA UNA LISTA CON LOS ELEMENTOS UNICOS DE LA CLUMNA DE UN DATATABLE
        public List<int> DataColumnToList_Int_Unique(DataTable dt, int campo)
        {
            List<int> final = new List<int>();
            try
            {
                List<int> datos = new List<int>();
                foreach (DataRow filaCampo in dt.Rows)
                {
                    try
                    {
                        string field = filaCampo.Field<dynamic>(campo).ToString().TrimEnd(' ');
                        datos.Add(Convert.ToInt32(field));
                    }
                    catch (Exception ex)
                    {
                        MessageShowOK_2("error en la consulta" + ex.ToString());
                    }
                }
                final = datos.Distinct().ToList();
            }
            catch (Exception ex)
            {
                MessageShowOK_2(ex.Message.ToString(), "ERROR");
            }
            return final;
        }



        //LLENA UNA LISTA CON LOS ELEMENTOS UNICOS DE LA CLUMNA DE UN DATATABLE
        public List<string> DataColumnToList_TODO(DataTable dt, int campo)
        {
            List<String> datos = new List<String>();
            try
            {
                foreach (DataRow filaCampo in dt.Rows)
                {
                    try
                    {
                        string field = filaCampo.Field<dynamic>(campo).ToString();
                        datos.Add(field.TrimEnd(' '));
                    }
                    catch (Exception)
                    {
                        datos.Add("error read");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageShowOK_2("error en la consulta" + ex.Message.ToString(), "ERROR");
            }
            return datos;
        }

        //LINQ - De un valor seleccionado lo busca en la columna de un DT y regresa el valor de la segunda columna - Elprimero
        public string BuscaValor_LINQ(DataTable DT, string ColumnaABuscar, string ValorBuscado, string ColumnaAObtener)
        {
            string ValorEncontrado = "";
            try
            {
                DataRow[] REN = DT.Select(ColumnaABuscar + "='" + ValorBuscado.TrimEnd(' ') + "'");
                if (REN.Length != 0)
                {
                    foreach (DataRow item in REN)
                    {
                        dynamic aux = item.Field<dynamic>(ColumnaAObtener);
                        if (!ValorEncontrado.Contains(aux.ToString()))
                            ValorEncontrado = aux.ToString() + " ,";
                    }
                    ValorEncontrado = ValorEncontrado.TrimEnd(',').TrimEnd(' ');
                }
            }
            catch (Exception)
            {

            }
            return ValorEncontrado;
        }

        //LINQ - De un valor seleccionado lo busca en la columna de un DT y regresa el valor de la segunda columna - TODOS
        public List<string> BuscaValores_LINQ(DataTable DT, string ColumnaABuscar, string ValorBuscado, string ColumnaAObtener)
        {
            List<string> ValorEncontrado = new List<string>();
            try
            {
                DataRow[] REN = DT.Select(ColumnaABuscar + "='" + ValorBuscado.TrimEnd(' ') + "'");
                if (REN.Length != 0)
                {
                    foreach (DataRow item in REN)
                    {
                        dynamic Dato = item.Field<dynamic>(ColumnaAObtener);
                        ValorEncontrado.Add(Dato.ToString());
                    }
                }
            }
            catch (Exception)
            {

            }
            return ValorEncontrado;
        }

        //LINQ - De un valor seleccionado lo busca en la columna de un DT y regresa el valor de la segunda columna - TODOS
        public DataTable BuscaDatos_LINQ(DataTable DTaBuscar, string ColumnaABuscar, string ValorBuscado)
        {
            DataTable DTFinal = new DataTable();
            DTFinal = DTaBuscar.Clone();
            try
            {
                DataRow[] REN = DTaBuscar.Select(ColumnaABuscar + "='" + ValorBuscado.TrimEnd(' ') + "'");
                if (REN.Length != 0)
                {
                    foreach (DataRow item in REN)
                    {
                        DTFinal.Rows.Add(item.ItemArray);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "ERROR");
            }
            return DTFinal;
        }

        //LINQ - De un valor seleccionado lo busca en la columna de un DT y regresa el valor de la segunda columna - TODOS
        public DataTable SelectDT(DataTable DTaBuscar, string Query, string OrderBy)
        {
            DataTable DTFinal = new DataTable();
            if (DTaBuscar == null)
            {
                MessageShowOK_2("DATATABLE vacío!");
                return null;
            }
            DTFinal = DTaBuscar.Clone();
            try
            {
                DataRow[] REN = null;
                if (!string.IsNullOrEmpty(OrderBy))
                    REN = DTaBuscar.Select(Query, OrderBy);
                else
                    REN = DTaBuscar.Select(Query);
                if (REN.Length != 0 && REN != null)
                {
                    foreach (DataRow item in REN)
                    {
                        DTFinal.Rows.Add(item.ItemArray);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "ERROR");
            }
            return DTFinal;
        }

        //LLENAR COMBOBOX DE LA LISTA DE DATOS QUE SE ENCUENTRAN EN UNA COLUMNA DE UN DATATABLE
        public List<string> llenarCombo(DataTable dt, String campo)
        {
            List<String> final = new List<string>();
            if (dt != null)
            {

                List<String> datos = new List<String>();
                foreach (DataRow filaCampo in dt.Rows)
                {
                    try
                    {
                        string field = filaCampo.Field<String>(campo);
                        if (!string.IsNullOrEmpty(field))
                            datos.Add(field);
                    }
                    catch (Exception ex)
                    {
                        MessageShowOK_2("error en la consulta" + ex.ToString(), "ERROR");
                    }
                }
                final = datos.Distinct().ToList();
            }
            else
                MessageShowOK_2("ERROR EN LA CONEXIÓN", "ERROR");
            return final;
        }

        //LINQ - De un valor seleccionado lo busca en la columna de un DT y regresa el valor de la segunda columna - TODOS
        public DataTable BuscaDatosDATAROW_LINQ(DataTable DTaBuscar, string ColumnaABuscar, string ValorBuscado)
        {
            DataTable DTFinal = new DataTable();
            DTFinal = DTaBuscar.Clone();
            try
            {
                DataRow[] REN = DTaBuscar.Select(ColumnaABuscar + "='" + ValorBuscado.TrimEnd(' ') + "'");
                if (REN.Length != 0)
                {
                    foreach (DataRow item in REN)
                    {
                        DTFinal.Rows.Add(item.ItemArray);
                    }
                }
            }
            catch (Exception)
            {

            }
            return DTFinal;
        }

        //LINQ - De un valor seleccionado lo busca en la columna de un DT y regresa el valor de la segunda columna - TODOS
        public DataTable BuscaDatosDATAROW_LINQ_startswidth(DataTable DTaBuscar, string ColumnaABuscar, string ValorBuscado)
        {
            DataTable DTFinal = new DataTable();
            DTFinal = DTaBuscar.Clone();
            try
            {
                DataRow[] REN = DTaBuscar.Select(ColumnaABuscar + " like ='" + ValorBuscado.TrimEnd(' ') + "%'");
                if (REN.Length != 0)
                {
                    foreach (DataRow item in REN)
                    {
                        DTFinal.Rows.Add(item.ItemArray);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageShowOK_2(ex.Message.ToString(), "ERROR");
            }
            return DTFinal;
        }

        //CONVIERTE RENGLON DE DATATABLE EN LISTA
        public List<string> DatarowTOList(DataTable DT, int NumRwnglon)
        {
            List<string> Linea = new List<string>();
            foreach (var item in DT.Rows[NumRwnglon].ItemArray)
            {
                Linea.Add(item.ToString());
            }
            return Linea;
        }

        //CONVIERTE RENGLON DE DATATABLE EN LISTA
        public string DatarowTOString(DataTable DT, int NumRwnglon, string Separador)
        {
            string RenglonString = "";
            foreach (var item in DT.Rows[NumRwnglon].ItemArray)
                RenglonString += item.ToString() + Separador;
            return RenglonString;
        }

        //CONVIERTE LISTA A STRING
        public string ListaAString(List<string> LISTA)
        {
            string Cadena = "";
            foreach (var item in LISTA)
                Cadena += item.Replace("\n", "") + Environment.NewLine;
            Cadena = Cadena.TrimEnd(Environment.NewLine.ToCharArray()).Replace("\r\r", "\r");
            return Cadena;
        }

        public static T[] ConcatArrays<T>(params T[][] list)
        {
            var result = new T[list.Sum(a => a.Length)];
            int offset = 0;
            for (int x = 0; x < list.Length; x++)
            {
                list[x].CopyTo(result, offset);
                offset += list[x].Length;
            }
            return result;
        }

        public string[] JoinArrays(string[] A, string Separador, string[] B)
        {
            string[] Array = new string[A.Length];
            for (int i = 0; i < Array.Length; i++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(A[i]) && !string.IsNullOrEmpty(B[i]))
                        Array[i] = A[i] + Separador + B[i];
                }
                catch (Exception)
                {

                }
            }
            Array = Array.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return Array;
        }

        //ARCHIVO TXT A DATATABLE
        public DataTable ArchivoTXT_a_DataTable(string RutaTXT)
        {
            DataTable DT = new DataTable();
            try
            {
                string[] Lineas = File.ReadAllLines(RutaTXT, Encoding.Default);
                if (Lineas.Length > 0)
                {
                    ///Estructura
                    string[] Titulos = Lineas[0].Split('\t');
                    foreach (string titulo in Titulos)
                    {
                        if (!string.IsNullOrEmpty(titulo))
                            DT.Columns.Add(titulo);
                        else
                            DT.Columns.Add();
                    }
                    ///Datos 
                    for (int i = 1; i < Lineas.Length; i++)
                    {
                        DT.Rows.Add(Lineas[i].Split('\t'));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageShowOK_2(ex.Message.ToString(), "ERROR");
            }
            return DT;
        }

        //TEXTO/STRING A DATATABLE
        public DataTable TextoADataTable(string Texto, char CharDivisor, char CharSeparador, bool Renglon1Titulos)
        {
            DataTable DT = new DataTable();
            try
            {
                string[] Lineas = Texto.Split(CharDivisor);
                if (Lineas.Length > 0)
                {
                    if (Renglon1Titulos)
                    {
                        ///Estructura
                        string[] Titulos = Lineas[0].Split(CharSeparador);
                        int aux = 2;
                        foreach (string titulo in Titulos)
                        {
                            if (!string.IsNullOrEmpty(titulo))
                            {
                                if (!DT.Columns.Contains(titulo))
                                {
                                    DT.Columns.Add(titulo);
                                }
                                else
                                {
                                    DT.Columns.Add(titulo + aux);
                                    aux++;
                                }
                            }
                            else
                                DT.Columns.Add();
                        }
                    }
                    else
                        for (int i = 0; i < Lineas[0].Split(CharSeparador).Length; i++)
                            DT.Columns.Add("Columna " + i);
                    ///Datos 
                    for (int i = 0; i < Lineas.Length; i++)
                    {
                        DT.Rows.Add(Lineas[i].Split(CharSeparador));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageShowOK_2(ex.Message.ToString(), "ERROR");
            }
            return DT;
        }

        //REGRESA LOS DISTINTOS DE DOS LISTAS
        public List<string> Distintos2Listas(List<string> Lista_A, List<string> Lista_B)
        {
            List<string> TOTALARCHIVOSPORDESCARGAR = new List<string>();  // DeterminantesBASEDATOS.ToList().Concat(DeterminantesDESCARGADOS.ToList()).ToList();
            //Comparacion y unicos de los dos arraglos
            var query = from dt1 in Lista_B
                        join dt2 in Lista_A
                        on dt1 equals dt2 into res
                        select new { x = dt1, y = res };
            foreach (var item in query)
                if (item.y.Count() == 0)
                    TOTALARCHIVOSPORDESCARGAR.Add(item.x);
            return TOTALARCHIVOSPORDESCARGAR;
        }

        //REGRESA UN STRING DE UN DATATABLE - MAS RAPIDO QUE LA DLL
        public string DatatableToString(DataTable DT, string Separador)
        {
            string nuevo = "";
            try
            {
                nuevo = string.Join(Environment.NewLine, DT.Rows.OfType<DataRow>().Select(x => string.Join(Separador, x.ItemArray)));
            }
            catch (Exception ex)
            {
                MessageShowOK_2(ex.Message.ToString(), "ERROR");
            }
            return nuevo;
        }

        #endregion

        #region METODOS WPF

        //LEER EL TEXTO DE CUALQUIER COMPONENTE 
        //  Dispatcher.Invoke(((Action)(() =>
        //  Dispatcher.Invoke(new Action(()=>
        public string LeerElemento(UIElement ElementoWPF)
        {
            string dato = "";
            try
            {
                if (ElementoWPF is TextBox)
                    Dispatcher.Invoke(new Action(() => dato = (ElementoWPF as TextBox).Text.ToString()));
                else if (ElementoWPF is ComboBox)
                    Dispatcher.Invoke(new Action(() => dato = (ElementoWPF as ComboBox).Text.ToString()));
                else if (ElementoWPF is DatePicker)
                    Dispatcher.Invoke(new Action(() => dato = (ElementoWPF as DatePicker).SelectedDate.Value.ToShortDateString()));
            }
            catch (Exception ex)
            {

            }
            return dato;
        }

        //DATAGRID A DATATABLE
        public DataTable DataViewAsDataTable(DataView dv)
        {
            DataTable dt = dv.Table.Clone();
            foreach (DataRowView drv in dv)
                dt.ImportRow(drv.Row);
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TituloExacto"></param>
        public QPil.Pages.Aviso CreaIndicador(string TituloExacto, string DatoEnBurbuja, string DatoEnAviso, string TExtoAvisoROBOT, bool VerNubi)
        {
            int erty = 20;
            UIElementCollection grdMenuses = Pages.Login.superMAINWINDOW.PanelRobots.Children;
            foreach (Grid menu in grdMenuses)
            {
                string lblTitulo = ((menu.Children[1] as DockPanel).Children[1] as Label).Content.ToString();
                if (lblTitulo == TituloExacto)
                {
                    Border brdIndicador = new Border()
                    {
                        CornerRadius = new CornerRadius(erty),
                        Height = erty,
                        Width = erty,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xCF, 0x23, 0x23)),
                        ///Background = new SolidColorBrush(Color.FromArgb(0xFF,  0x23, 0xCF, 0x42))
                        ToolTip = TExtoAvisoROBOT
                    };
                    Label lblss = new Label()
                    {
                        Foreground = Metodos.Blanco,
                        Content = DatoEnBurbuja,
                        FontSize = 12,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(-6),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    brdIndicador.Child = lblss;
                    ///
                    try
                    {
                        menu.Children.RemoveAt(2);
                    }
                    catch (Exception)
                    {

                    }
                    ///Animación
                    Dispatcher.Invoke(((Action)(() => brdIndicador.BeginAnimation(Border.HeightProperty, new DoubleAnimation(10, 50, new Duration(TimeSpan.FromSeconds(.1)))))));
                    Dispatcher.Invoke(((Action)(() => brdIndicador.BeginAnimation(Border.WidthProperty, new DoubleAnimation(10, 50, new Duration(TimeSpan.FromSeconds(.1)))))));
                    Thread.Sleep(500);
                    Dispatcher.Invoke(((Action)(() => brdIndicador.BeginAnimation(Border.HeightProperty, new DoubleAnimation(50, 20, new Duration(TimeSpan.FromSeconds(.4)))))));
                    Dispatcher.Invoke(((Action)(() => brdIndicador.BeginAnimation(Border.WidthProperty, new DoubleAnimation(50, 20, new Duration(TimeSpan.FromSeconds(.4)))))));
                    ///
                    menu.Children.Add(brdIndicador);
                    menu.UpdateLayout();
                    ///Aviso
                    QPil.Pages.Aviso aviso = null;
                    if (VerNubi)
                    {
                        Dispatcher.Invoke(((Action)(() => aviso = new Pages.Aviso())));
                        Dispatcher.Invoke(((Action)(() => aviso.txtStatusContent.Text = TExtoAvisoROBOT)));
                        Dispatcher.Invoke(((Action)(() => aviso.imgMinimiza.Visibility = System.Windows.Visibility.Collapsed)));
                        Dispatcher.Invoke(((Action)(() => aviso.imgCancelarHilo.Visibility = System.Windows.Visibility.Visible)));
                        Dispatcher.Invoke(((Action)(() => aviso.pgbStatus.Visibility = System.Windows.Visibility.Collapsed)));
                        Dispatcher.Invoke(((Action)(() => aviso.label.Content = "AVISO")));
                        Dispatcher.Invoke(((Action)(() => aviso.Show())));
                    }
                    return aviso;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TituloExacto"></param>
        public void EliminaIndicador(string TituloExacto, QPil.Pages.Aviso OBJaVISO)
        {
            UIElementCollection grdMenuses = Pages.Login.superMAINWINDOW.PanelRobots.Children;
            foreach (Grid menu in grdMenuses)
                if (((menu.Children[1] as DockPanel).Children[1] as Label).Content.ToString() == TituloExacto)
                {
                    try
                    {
                        menu.Children.RemoveAt(2);
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        OBJaVISO.Cerrar();
                    }
                    catch (Exception)
                    {

                    }
                }
        }

        #endregion

        #region HTTP

        //OBTIENE EL TEXTO DE UN ARCHVIO TXT ALMACENADO EN EL SERVIDOR
        public String getHTTP(string URLtxt)
        {
            String datos = "";
            int Intentos = 0;
            do
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;//Ayuda para obtener poder acceder bajo el protocolo de seguridad del servidor.
                    WebClient client = new WebClient();
                    client.Proxy = WebRequest.DefaultWebProxy;
                    client.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    client.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    datos = client.DownloadString(URLtxt);
                }
                catch (Exception ex)
                {
                    Thread.Sleep(1000);
                }
                Intentos++;
                if (Intentos > 5)
                    break;
            } while (datos == "");
            return datos;
        }

        //OBTNER TEXTO DE ARHIVO DE TEXTO
        public string ObtnerTextoDeURL(string URL)
        {
            string datos = "";
            do
            {
                try
                {
                    ///Obtencion
                    WebClient client = new WebClient();
                    client.Proxy = WebRequest.DefaultWebProxy;
                    client.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    client.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    datos = client.DownloadString(URL);
                    return datos;
                }
                catch (Exception ex)
                {
                    MessageShowOK_2(ex.ToString(), "ERROR");
                }
            } while (datos == "");
            return "";
        }

        #endregion

        #region OTROS

        //QUITAR HTML DE TEXTO
        public string RemplazaHtml_GTS(string strCadHtml)
        {
            string strCadLimpia = "";
            Regex expRegular = new Regex("<[^>]*>");
            strCadLimpia = expRegular.Replace(strCadHtml, " ");
            return strCadLimpia;
        }

        //Funcion para cambiar el mensaje regresandolo en mayusculas y considerando solo letras
        public string Modifica(string mensaje)
        {
            string resultado = "";
            int pos, largomensaje;
            mensaje = mensaje.Replace("á", "a");
            mensaje = mensaje.Replace("é", "e");
            mensaje = mensaje.Replace("í", "i");
            mensaje = mensaje.Replace("ó", "o");
            mensaje = mensaje.Replace("ú", "u");
            mensaje = mensaje.Replace("Á", "A");
            mensaje = mensaje.Replace("É", "E");
            mensaje = mensaje.Replace("Í", "I");
            mensaje = mensaje.Replace("Ó", "O");
            mensaje = mensaje.Replace("Ú", "U");
            mensaje = mensaje.Replace("º", "O");
            largomensaje = mensaje.Length;
            pos = 0;
            while (pos < largomensaje)
            {
                if (char.IsLetter(mensaje[pos])) { resultado = resultado + mensaje[pos]; }
                pos++;
            }
            resultado = resultado.ToUpper();
            return (resultado);
        }

        ///string[] strngas = DatosConvenio.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();

        #endregion

        #region DISEÑO WPF



        #endregion

    }
}
