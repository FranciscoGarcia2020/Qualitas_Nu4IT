using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;

namespace QPil.Dlls
{
    class Nu4it
    {

        const int NO = 0;
        const int SI = 1;

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
        public string[] LeerArchivoIni(string nombreRobot)
        {
            char[] delimiterChars = { '\r', '\n' };
            string lineaTexto, RutaDelINI = Directory.GetCurrentDirectory();
            string[] ContenidoArchivo = new string[0];
            RutaDelINI = RutaDelINI + @"\InfoGeneral_" + nombreRobot + ".ini";

            if (File.Exists(RutaDelINI))
            {
                StreamReader sr = new StreamReader(RutaDelINI);
                lineaTexto = sr.ReadToEnd();
                ContenidoArchivo = lineaTexto.Split(delimiterChars);
                sr.Close();
            }
            return (ContenidoArchivo);

        }
        //Procedimiento que regresa un string que se pretende será el  nombre de un archivo 
        //armado con el name pasado por parametro; fecha y hora en que se creo el archivo y la extencion pasada pos parametro  
        public string GeneraNombreArchivo(string name, string extencion)
        {
            string NombreArchivo = name;
            string fechahoracreacion = "";
            string formatofecha = "";
            DateTime fechhora = DateTime.Now;
            formatofecha = "yyyy/MM/dd HH:mm:ss";
            fechahoracreacion = fechhora.ToString(formatofecha);
            fechahoracreacion = fechahoracreacion.Replace("p.m.", "PM");
            fechahoracreacion = fechahoracreacion.Replace("a.m.", "AM");
            fechahoracreacion = fechahoracreacion.Replace(" ", "");
            fechahoracreacion = fechahoracreacion.Replace(":", "");
            fechahoracreacion = fechahoracreacion.Replace("/", "");
            NombreArchivo = NombreArchivo + fechahoracreacion + "." + extencion;
            return (NombreArchivo);
        }

        //Procedimiento que crea el archivo donde se reportan las exepciones ocurridas
        public void ArchivoExcepciones(string mensaje)
        {
            string ruta, filename;
            ruta = Directory.GetCurrentDirectory();
            filename = GeneraNombreArchivo("Excepcion_", "txt");
            ruta = ruta + @"\" + filename;
            StreamWriter arerr = new StreamWriter(ruta);
            arerr.Write(mensaje);
            arerr.Close();
        }

        //Procedimiento que crea el archivo LOG
        public void CreaArchivoLog(string rutaNomArchivo)
        {

            string fechahoracreacion = "", formatofecha = "";
            DateTime fechhora = DateTime.Now;
            formatofecha = "yyyy/MM/dd HH:mm:ss";
            fechahoracreacion = fechhora.ToString(formatofecha);

            StreamWriter sr = new StreamWriter(rutaNomArchivo);
            sr.WriteLine("Archivo LOG de reportes del " + fechahoracreacion);
            sr.WriteLine("*******************************************************");
            sr.Close();
        }

        //Procedimiento que actualiza el archivo LOG
        public void ReportarLog(string rutaNomArchivo, string reporte)
        {
            try
            {
                string fechahoraReporte = "", formatofecha = "";
                DateTime fechhora = DateTime.Now;
                formatofecha = "HH:mm:ss";
                fechahoraReporte = fechhora.ToString(formatofecha);

                StreamWriter sw = File.AppendText(rutaNomArchivo);
                sw.Write(fechahoraReporte + ": ");
                sw.WriteLine(reporte);
                sw.Close();
            }
            catch (Exception)
            {

            }
        }

        //Función que sube al servidor FTP el archivo zip usando el usuario y claves pasados por parametro
        public void SubeFTP(string NomUsuarioFTP, string ClaveFTP, string RutaNombreArchivoZIPaSubir, string RutaFTPDondeSubiraNombreArchivo)
        {
            try
            {
                WebClient cliente = new WebClient();
                cliente.Credentials = new NetworkCredential(NomUsuarioFTP, ClaveFTP);
                cliente.UploadFile(RutaFTPDondeSubiraNombreArchivo, RutaNombreArchivoZIPaSubir);
            }
            catch (Exception ex)
            {
                ArchivoExcepciones(ex.ToString());
            }
        }

        //Procedimiento que se encarga de descargar el archivo ZIP desde el servidor FTP usando el usuario y claves pasados por parametro
        public void DescargaFTP(string NomUsuarioFTP, string ClaveFTP, string RutaNombreArchivoDescargarDelFTP, string RutaDescargaActualizacion)
        {
            try
            {
                WebClient cliente = new WebClient();
                cliente.Credentials = new NetworkCredential(NomUsuarioFTP, ClaveFTP);
                cliente.DownloadFile(RutaNombreArchivoDescargarDelFTP, RutaDescargaActualizacion);
            }
            catch (Exception ex)
            {
                ArchivoExcepciones(ex.ToString());
            }
        }

        //*************************************************************************************************************
        //Procedimiento que valida que exista en la ruta pasada por parametro la carpeta pasada por parametro
        //En caso de que no exista la carpeta en la ruta la creara garantizando así la existencia de esa carpeta en esa ruta
        public void GarantizarCarpeta(string ruta, string CarpetaCrear)
        {
            string[] CarpetasExistentes = Directory.GetDirectories(ruta);
            int encontrado, TotalCarpetas, IndCarpeta;
            encontrado = NO;
            TotalCarpetas = CarpetasExistentes.Length;
            IndCarpeta = 0;
            ruta = ruta + @"\" + CarpetaCrear;
            while ((encontrado == NO) && (IndCarpeta < TotalCarpetas))
            {
                if (CarpetasExistentes[IndCarpeta] == ruta) { encontrado = SI; }
                IndCarpeta++;
            }
            if (encontrado == NO) { Directory.CreateDirectory(ruta); }
        }

        /*Metodo de Busqueda Fonetica,Compara y checa si dos cadenas son iguales. Retorna un Boleano True y/o false*/
        public Boolean BFonetica(String Primera_palabra, String Segunda_palabra)
        {
            String miPalabra = "";
            String datoPalabra = "";
            Boolean relacion = false;

            Primera_palabra = Primera_palabra.ToUpper();
            Segunda_palabra = Segunda_palabra.ToUpper();

            StringBuilder P_palabra = new StringBuilder(Primera_palabra);
            StringBuilder S_palabra = new StringBuilder(Segunda_palabra);
            StringBuilder p = new StringBuilder(Segunda_palabra);

            for (int i = 1; i <= 2; i++)
            {

                if (i == 1)
                {
                    p = P_palabra;
                }
                else
                {
                    p = S_palabra;
                }

                p.Replace("*", "").ToString();
                p.Replace("-", "").ToString();
                p.Replace("_", "").ToString();
                p.Replace(".", "").ToString();
                p.Replace("/t", "").ToString();
                p.Replace("\t", "").ToString();
                p.Replace("\n", "").ToString();
                p.Replace("\r", "").ToString();
                p.Replace("/r", "").ToString();
                p.Replace("/", "").ToString();
                p.Replace("#", "").ToString();
                p.Replace(":", "").ToString();
                p.Replace(",", "").ToString();
                p.Replace("  ", "").ToString();
                p.Replace(" ", "").ToString();
                p.Replace("´", "").ToString();

                p.Replace("Á", "A");
                p.Replace("É", "E");
                p.Replace("Í", "I");
                p.Replace("Ó", "O");
                p.Replace("Ú", "U");

                p.Replace("V", "B");
                p.Replace("C", "S");
                p.Replace("K", "S");
                p.Replace("Z", "S");
                p.Replace("H", "");
                p.Replace("Y", "I");
                p.Replace("j", "G");
                p.Replace("M", "N");
                p.Replace("W", "U");
                p.Replace("Ñ", "N");

                p.Replace("0", "");
                p.Replace("1", "");
                p.Replace("2", "");
                p.Replace("3", "");
                p.Replace("4", "");
                p.Replace("5", "");
                p.Replace("6", "");
                p.Replace("7", "");
                p.Replace("8", "");
                p.Replace("9", "");

                if (i == 1)
                {
                    miPalabra = p.ToString();
                }
                else
                {
                    datoPalabra = p.ToString();
                }
            }

            if (miPalabra == datoPalabra)
            {
                relacion = true;
            }

            return relacion;
        }

        //Regresa el nombre del mes de acuerdo al número que se pasa por parametro
        public string NombreMes(int numero)
        {
            string nomMes = "";
            switch (numero)
            {
                case 1:
                    nomMes = "Enero";
                    break;
                case 2:
                    nomMes = "Febrero";
                    break;
                case 3:
                    nomMes = "Marzo";
                    break;
                case 4:
                    nomMes = "Abril";
                    break;
                case 5:
                    nomMes = "Mayo";
                    break;
                case 6:
                    nomMes = "Junio";
                    break;
                case 7:
                    nomMes = "Julio";
                    break;
                case 8:
                    nomMes = "Agosto";
                    break;
                case 9:
                    nomMes = "Septiembre";
                    break;
                case 10:
                    nomMes = "Octubre";
                    break;
                case 11:
                    nomMes = "Noviembre";
                    break;
                case 12:
                    nomMes = "Diciembre";
                    break;
                default:
                    nomMes = "ERROR";
                    break;
            }
            return (nomMes);
        }

        //Regresa el nombre del mes de actual
        public string NombreMesActual()
        {
            string mes = "", fechahoraActual = "", formatofecha = "", month;
            int nummes;
            DateTime fechhora = DateTime.Now;
            formatofecha = "yyyy/MM/dd HH:mm:ss";
            fechahoraActual = fechhora.ToString(formatofecha);
            month = fechahoraActual.Substring(5, 2);
            nummes = Convert.ToInt16(month);
            mes = NombreMes(nummes);
            return mes;
        }

        //Funcion que regresa el número de veces que aparece en el arreglo la palabra a buscar pasados por parametro
        public int OcurrenciasEnArreglo(string[] Arreglo, string palabrabuscar, int ConMod)
        {
            int apariciones, indPos, TamanioArreglo;
            string ContenidoComparar;
            TamanioArreglo = Arreglo.Length;
            apariciones = 0;
            if (ConMod == SI) { palabrabuscar = Modifica(palabrabuscar); }
            for (indPos = 0; indPos < TamanioArreglo; indPos++)
            {
                ContenidoComparar = Arreglo[indPos];
                if (ConMod == SI) { ContenidoComparar = Modifica(ContenidoComparar); }
                if (ContenidoComparar.IndexOf(palabrabuscar) >= 0) { apariciones++; }
            }
            return (apariciones);
        }

        //Función que regresa la posición del arreglo donde esté ubicado la palabra a buscar pasados por parametro
        public int UbicadoEnPos(string[] Arreglo, string palabrabuscar, int ConMod)
        {
            int ubicacion, indPos, TamanioArreglo, UbEnc, NoEsta;
            string ContenidoComparar;
            NoEsta = -1;
            TamanioArreglo = Arreglo.Length;
            indPos = 0;
            UbEnc = NO;
            if (ConMod == SI) { palabrabuscar = Modifica(palabrabuscar); }
            while ((indPos < TamanioArreglo) && (UbEnc == NO))
            {
                ContenidoComparar = Arreglo[indPos];
                if (ConMod == SI) { ContenidoComparar = Modifica(ContenidoComparar); }
                if (ContenidoComparar.IndexOf(palabrabuscar) >= 0) { UbEnc = SI; }
                indPos++;
            }
            if (UbEnc == SI)
            {
                indPos--;
                ubicacion = indPos;
            }
            else { ubicacion = NoEsta; }
            return (ubicacion);
        }

        //Funcion que determina si el año pasado por parametro es bisiesto
        public int EsBisiesto(int Anio)
        {
            int Bisiesto, Residuo4, Residuo100, Residuo400;
            Residuo4 = Anio % 4;
            Residuo100 = Anio % 100;
            Residuo400 = Anio % 400;
            if ((Residuo4 == 0) && ((Residuo100 != 0) || (Residuo400 == 0))) { Bisiesto = SI; }
            else { Bisiesto = NO; }
            return (Bisiesto);
        }

        //Función que regresa en un string el contenido del DataTable 
        public string ConvierteDTaSTRING(DataTable Tabla)
        {
            string contenido = "";
            int ro = Tabla.Columns.Count;
            foreach (DataRow i in Tabla.Rows)
            {
                if (i.RowState != DataRowState.Deleted)
                {
                    for (int e = 0; e < ro; e++)
                    {
                        contenido += i[e] + "\t";
                    }
                    contenido += Environment.NewLine;
                }
            }
            return (contenido);
        }

        //Función que regresa una fecha en formato garantizado dd/mm/yyyy o ERROR en caso de alguna irregularidad de acuerdo a la deduccion y validación de un de una fecha pasada como string
        public string GenerarFechaFormatoControlado(string acertijo)
        {
            string fechaRespuesta = "", actual, formatofecha = "dd/MM/yyyy", fechaIngresada, iniciales, diaDeducido, mesDeducido, anioDeducido, DiaActual, MesActual, AnioActual, candidata1, candidata2;
            int tamNomMes, tamAcertijo, posSeparador, DiaPosible, MesPosible, AnioPosible, DiasDif1, DiasDif2;
            string[] FECHAACERTIJO = new string[0];
            DateTime fechactual = DateTime.Now;
            actual = fechactual.ToString(formatofecha);
            DiaActual = actual.Substring(0, 2);
            MesActual = actual.Substring(3, 2);
            AnioActual = actual.Substring(6, 4);
            diaDeducido = "";
            mesDeducido = "";
            anioDeducido = "";
            if (acertijo != "")
            {
                acertijo = acertijo.Replace("-", "/"); acertijo = acertijo.Replace(".", "/"); acertijo = acertijo.Replace("_", "/"); acertijo = acertijo.Replace(" ", "/"); acertijo = acertijo.Replace("\\", "/");
                posSeparador = acertijo.IndexOf("/");
                if (posSeparador >= 0)
                {
                    FECHAACERTIJO = acertijo.Split('/');
                    tamAcertijo = FECHAACERTIJO.Length;
                    if (tamAcertijo == 3)
                    {
                        fechaIngresada = Modifica(acertijo);
                        if (fechaIngresada == "") //Eso quiere decir que que no habia letras en el acertijo. Sólo números y simbolos(tal vez separadores: / - . \ _)
                        {
                            tamAcertijo = acertijo.Length;
                            if (tamAcertijo >= 8) // ** dd/mm/yyyy mm/dd/yyyy yyyy/mm/dd yyyy/dd/mm ** d/mm/yyyy=dd/m/yyyy mm/d/yyyy=m/dd/yyyy ~~ yyyy/mm/d=yyyy/m/dd yyyy/mm/d=yyyy/m/dd ** d/m/yyyy m/d/yyyy yyyy/m/d yyyy/d/m ~~ dd/mm/yy mm/dd/yy yy/mm/dd yy/dd/mm 
                            {
                                if (FECHAACERTIJO[2].Length == 4) //** dd/mm/yyyy=d/mm/yyyy=dd/mm/yyyy=d/m/yyyy ** mm/dd/yyyy=mm/d/yyyy=m/dd/yyyy=m/d/yyyy
                                {
                                    anioDeducido = FECHAACERTIJO[2];
                                    DiaPosible = Convert.ToInt16(FECHAACERTIJO[0]);
                                    MesPosible = Convert.ToInt16(FECHAACERTIJO[1]);
                                    if (DiaPosible <= 12 || MesPosible <= 12)
                                    {
                                        if (DiaPosible > 12)
                                        {
                                            if (DiaPosible <= 31)
                                            {
                                                diaDeducido = FECHAACERTIJO[0];
                                                mesDeducido = FECHAACERTIJO[1];
                                            }
                                            else { fechaRespuesta = "ERROR"; } //No puede haber dias mayores a 31
                                        }
                                        else
                                        {
                                            if (MesPosible > 12)
                                            {
                                                if (MesPosible <= 31)
                                                {
                                                    diaDeducido = FECHAACERTIJO[1];
                                                    mesDeducido = FECHAACERTIJO[0];
                                                }
                                                else { fechaRespuesta = "ERROR"; } // No puede haber dias mayores a 31
                                            }
                                            else // Los dos fueron menor a 12 OJO *****************************************************************
                                            {
                                                try
                                                {
                                                    candidata1 = FECHAACERTIJO[0] + "/" + FECHAACERTIJO[1] + "/" + FECHAACERTIJO[2];
                                                    candidata2 = FECHAACERTIJO[1] + "/" + FECHAACERTIJO[0] + "/" + FECHAACERTIJO[2];
                                                    DiasDif1 = (Convert.ToDateTime(candidata1) - Convert.ToDateTime(actual)).Days;
                                                    DiasDif2 = (Convert.ToDateTime(candidata2) - Convert.ToDateTime(actual)).Days;
                                                    if (DiasDif1 > 0 && DiasDif2 > 0)
                                                    {
                                                        if (DiasDif1 < DiasDif2)
                                                        {
                                                            diaDeducido = FECHAACERTIJO[0];
                                                            mesDeducido = FECHAACERTIJO[1];
                                                        }
                                                        else
                                                        {
                                                            diaDeducido = FECHAACERTIJO[1];
                                                            mesDeducido = FECHAACERTIJO[0];
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (DiasDif1 <= 0 && DiasDif2 <= 0)
                                                        {
                                                            if (DiasDif1 > DiasDif2)
                                                            {
                                                                diaDeducido = FECHAACERTIJO[0];
                                                                mesDeducido = FECHAACERTIJO[1];
                                                            }
                                                            else
                                                            {
                                                                diaDeducido = FECHAACERTIJO[1];
                                                                mesDeducido = FECHAACERTIJO[0];
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DiasDif1 > 0) // DiasDif2<=0
                                                            {
                                                                diaDeducido = FECHAACERTIJO[0];
                                                                mesDeducido = FECHAACERTIJO[1];
                                                            }
                                                            else // DiasDif2>0 y DiasDif1<=0
                                                            {
                                                                diaDeducido = FECHAACERTIJO[1];
                                                                mesDeducido = FECHAACERTIJO[0];
                                                            }
                                                        }
                                                    }
                                                }
                                                catch { fechaRespuesta = "ERROR"; }
                                            }
                                        }
                                    }
                                    else { fechaRespuesta = "ERROR"; } //Los dos datos son mayores a 12 y no puede haber un mes mayor a 12
                                }
                                else //** yyyy/mm/dd=yyyy/mm/d=yyyy/m/dd=yyyy/m/d ** yyyy/dd/mm=yyyy/d/mm=yyyy/dd/m=yyyy/d/m ~~ dd/mm/yy mm/dd/yy yy/mm/dd yy/dd/mm
                                {
                                    if (FECHAACERTIJO[0].Length == 4) //** yyyy/mm/dd=yyyy/mm/d=yyyy/m/dd=yyyy/m/d ** yyyy/dd/mm=yyyy/d/mm=yyyy/dd/m=yyyy/d/m
                                    {
                                        anioDeducido = FECHAACERTIJO[0];
                                        DiaPosible = Convert.ToInt16(FECHAACERTIJO[2]);
                                        MesPosible = Convert.ToInt16(FECHAACERTIJO[1]);
                                        if (DiaPosible <= 12 || MesPosible <= 12)
                                        {
                                            if (DiaPosible > 12)
                                            {
                                                if (DiaPosible <= 31)
                                                {
                                                    diaDeducido = FECHAACERTIJO[2];
                                                    mesDeducido = FECHAACERTIJO[1];
                                                }
                                                else { fechaRespuesta = "ERROR"; } //No puede haber dias mayores a 31
                                            }
                                            else
                                            {
                                                if (MesPosible > 12)
                                                {
                                                    if (MesPosible <= 31)
                                                    {
                                                        diaDeducido = FECHAACERTIJO[1];
                                                        mesDeducido = FECHAACERTIJO[2];
                                                    }
                                                    else { fechaRespuesta = "ERROR"; } //No puede haber dias mayores a 31
                                                }
                                                else // Los dos fueron menor a 12 OJO //****
                                                {
                                                    try
                                                    {
                                                        candidata1 = FECHAACERTIJO[2] + "/" + FECHAACERTIJO[1] + "/" + FECHAACERTIJO[0];
                                                        candidata2 = FECHAACERTIJO[1] + "/" + FECHAACERTIJO[2] + "/" + FECHAACERTIJO[0];
                                                        DiasDif1 = (Convert.ToDateTime(candidata1) - Convert.ToDateTime(actual)).Days;
                                                        DiasDif2 = (Convert.ToDateTime(candidata2) - Convert.ToDateTime(actual)).Days;
                                                        if (DiasDif1 > 0 && DiasDif2 > 0)
                                                        {
                                                            if (DiasDif1 < DiasDif2)
                                                            {
                                                                diaDeducido = FECHAACERTIJO[2];
                                                                mesDeducido = FECHAACERTIJO[1];
                                                            }
                                                            else
                                                            {
                                                                diaDeducido = FECHAACERTIJO[1];
                                                                mesDeducido = FECHAACERTIJO[2];
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DiasDif1 <= 0 && DiasDif2 <= 0)
                                                            {
                                                                if (DiasDif1 > DiasDif2)
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[2];
                                                                    mesDeducido = FECHAACERTIJO[1];
                                                                }
                                                                else
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[1];
                                                                    mesDeducido = FECHAACERTIJO[2];
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (DiasDif1 > 0) // DiasDif2<=0
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[2];
                                                                    mesDeducido = FECHAACERTIJO[1];
                                                                }
                                                                else // DiasDif2>0 y DiasDif1<=0
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[1];
                                                                    mesDeducido = FECHAACERTIJO[2];
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch { fechaRespuesta = "ERROR"; }
                                                }
                                            }
                                        }
                                        else { fechaRespuesta = "ERROR"; } //Los dos datos son mayores a 12 y no puede haber un mes mayor a 12
                                    }
                                    else // dd/mm/yy ** mm/dd/yy ** yy/mm/dd ** yy/dd/mm ** ERROR
                                    {
                                        if (tamAcertijo == 8) // dd/mm/yy ** mm/dd/yy ** yy/mm/dd ** yy/dd/mm
                                        {
                                            DiaPosible = Convert.ToInt16(FECHAACERTIJO[0]);
                                            MesPosible = Convert.ToInt16(FECHAACERTIJO[1]);
                                            AnioPosible = Convert.ToInt16(FECHAACERTIJO[2]);
                                            //Copiar de aqui********
                                            if (DiaPosible <= 12 || MesPosible <= 12 || AnioPosible <= 12)
                                            {
                                                if (DiaPosible > 12) //DiaPosible no puede ser el mes 
                                                {
                                                    if (AnioPosible > 12) //AnioPosible tampoco puede ser el mes PLT MesPosible es el mes
                                                    {
                                                        mesDeducido = FECHAACERTIJO[1];
                                                        if (DiaPosible <= 31 || AnioPosible <= 31)
                                                        {
                                                            if (DiaPosible > 31) //DiaPosible no puede ser el dia PLT:
                                                            {
                                                                diaDeducido = FECHAACERTIJO[2];
                                                                anioDeducido = "20" + FECHAACERTIJO[0];
                                                            }
                                                            else
                                                            {
                                                                if (AnioPosible > 31) //AnioPosible no puede ser el dia PLT:
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[0];
                                                                    anioDeducido = "20" + FECHAACERTIJO[2];
                                                                }
                                                                else //AnioPosible y DiaPosible pueden ser el dia
                                                                {
                                                                    try
                                                                    {
                                                                        candidata1 = FECHAACERTIJO[0] + "/" + FECHAACERTIJO[1] + "/20" + FECHAACERTIJO[2];
                                                                        candidata2 = FECHAACERTIJO[2] + "/" + FECHAACERTIJO[1] + "/20" + FECHAACERTIJO[0];
                                                                        DiasDif1 = (Convert.ToDateTime(candidata1) - Convert.ToDateTime(actual)).Days;
                                                                        DiasDif2 = (Convert.ToDateTime(candidata2) - Convert.ToDateTime(actual)).Days;
                                                                        if (DiasDif1 > 0 && DiasDif2 > 0)
                                                                        {
                                                                            if (DiasDif1 < DiasDif2)
                                                                            {
                                                                                diaDeducido = FECHAACERTIJO[0];
                                                                                anioDeducido = "20" + FECHAACERTIJO[2];
                                                                            }
                                                                            else
                                                                            {
                                                                                diaDeducido = FECHAACERTIJO[2];
                                                                                anioDeducido = "20" + FECHAACERTIJO[0];
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (DiasDif1 <= 0 && DiasDif2 <= 0)
                                                                            {
                                                                                if (DiasDif1 > DiasDif2)
                                                                                {
                                                                                    diaDeducido = FECHAACERTIJO[0];
                                                                                    anioDeducido = "20" + FECHAACERTIJO[2];
                                                                                }
                                                                                else
                                                                                {
                                                                                    diaDeducido = FECHAACERTIJO[2];
                                                                                    anioDeducido = "20" + FECHAACERTIJO[0];
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (DiasDif1 > 0) // DiasDif2<=0
                                                                                {
                                                                                    diaDeducido = FECHAACERTIJO[0];
                                                                                    anioDeducido = "20" + FECHAACERTIJO[2];
                                                                                }
                                                                                else // DiasDif2>0 y DiasDif1<=0
                                                                                {
                                                                                    diaDeducido = FECHAACERTIJO[2];
                                                                                    anioDeducido = "20" + FECHAACERTIJO[0];
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    catch { fechaRespuesta = "ERROR"; }
                                                                }
                                                            }
                                                        }
                                                        else { fechaRespuesta = "ERROR"; } //Ninguno de los datos es menor o igual a 31 no puede haber un dia mayor a 31
                                                    }
                                                    else //DiaPosible no puede ser el mes pero el AnioPosible SI puede ser el mes 
                                                    {
                                                        if (MesPosible > 12) // MesPosible no puede ser el mes PLT anio Posible es el mes 
                                                        {
                                                            mesDeducido = FECHAACERTIJO[2];
                                                            //Y como el año nunca esta en medio en ningún formato
                                                            diaDeducido = FECHAACERTIJO[1];
                                                            anioDeducido = "20" + FECHAACERTIJO[0];
                                                        }
                                                        else // AnioPosible y MesPosible pueden ser el mes pero el diaPosible NO PLT Seguramente el DiaPosible es el año
                                                        {
                                                            anioDeducido = "20" + FECHAACERTIJO[0];
                                                            try
                                                            {
                                                                candidata1 = FECHAACERTIJO[1] + "/" + FECHAACERTIJO[2] + "/20" + FECHAACERTIJO[0];
                                                                candidata2 = FECHAACERTIJO[2] + "/" + FECHAACERTIJO[1] + "/20" + FECHAACERTIJO[0];
                                                                DiasDif1 = (Convert.ToDateTime(candidata1) - Convert.ToDateTime(actual)).Days;
                                                                DiasDif2 = (Convert.ToDateTime(candidata2) - Convert.ToDateTime(actual)).Days;
                                                                if (DiasDif1 > 0 && DiasDif2 > 0)
                                                                {
                                                                    if (DiasDif1 < DiasDif2)
                                                                    {
                                                                        diaDeducido = FECHAACERTIJO[1];
                                                                        mesDeducido = FECHAACERTIJO[2];
                                                                    }
                                                                    else
                                                                    {
                                                                        diaDeducido = FECHAACERTIJO[2];
                                                                        mesDeducido = FECHAACERTIJO[1];
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (DiasDif1 <= 0 && DiasDif2 <= 0)
                                                                    {
                                                                        if (DiasDif1 > DiasDif2)
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[1];
                                                                            mesDeducido = FECHAACERTIJO[2];
                                                                        }
                                                                        else
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[2];
                                                                            mesDeducido = FECHAACERTIJO[1];
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (DiasDif1 > 0) // DiasDif2<=0
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[1];
                                                                            mesDeducido = FECHAACERTIJO[2];
                                                                        }
                                                                        else // DiasDif2>0 y DiasDif1<=0
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[2];
                                                                            mesDeducido = FECHAACERTIJO[1];
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            catch { fechaRespuesta = "ERROR"; }
                                                        }
                                                    }
                                                }
                                                else //DiaPosible SI Puede ser el mes
                                                {
                                                    if (AnioPosible > 12) //AnioPosible NO puede ser el mes
                                                    {
                                                        if (MesPosible > 12) // MesPosible tampoco puede ser el mes PLT DiaPosible es el mes
                                                        {
                                                            mesDeducido = FECHAACERTIJO[0];
                                                            //Y como el año nunca esta en medio en ningún formato
                                                            diaDeducido = FECHAACERTIJO[1];
                                                            anioDeducido = FECHAACERTIJO[2];
                                                        }
                                                        else // MesPosible y DiaPosible son menores a 12 PLT AnioPosible es el año
                                                        {
                                                            anioDeducido = FECHAACERTIJO[2];
                                                            try
                                                            {
                                                                candidata1 = FECHAACERTIJO[0] + "/" + FECHAACERTIJO[1] + "/20" + FECHAACERTIJO[2];
                                                                candidata2 = FECHAACERTIJO[1] + "/" + FECHAACERTIJO[0] + "/20" + FECHAACERTIJO[2];
                                                                DiasDif1 = (Convert.ToDateTime(candidata1) - Convert.ToDateTime(actual)).Days;
                                                                DiasDif2 = (Convert.ToDateTime(candidata2) - Convert.ToDateTime(actual)).Days;

                                                                if (DiasDif1 > 0 && DiasDif2 > 0)
                                                                {
                                                                    if (DiasDif1 < DiasDif2)
                                                                    {
                                                                        diaDeducido = FECHAACERTIJO[0];
                                                                        mesDeducido = FECHAACERTIJO[1];
                                                                    }
                                                                    else
                                                                    {
                                                                        diaDeducido = FECHAACERTIJO[1];
                                                                        mesDeducido = FECHAACERTIJO[0];
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (DiasDif1 <= 0 && DiasDif2 <= 0)
                                                                    {
                                                                        if (DiasDif1 > DiasDif2)
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[0];
                                                                            mesDeducido = FECHAACERTIJO[1];
                                                                        }
                                                                        else
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[1];
                                                                            mesDeducido = FECHAACERTIJO[0];
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (DiasDif1 > 0) // DiasDif2<=0
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[0];
                                                                            mesDeducido = FECHAACERTIJO[1];
                                                                        }
                                                                        else // DiasDif2>0 y DiasDif1<=0
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[1];
                                                                            mesDeducido = FECHAACERTIJO[0];
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            catch { fechaRespuesta = "ERROR"; }
                                                        }
                                                    }
                                                    else // DiaPosible y AnioPosible SI pueden ser el mes
                                                    {
                                                        if (MesPosible > 12) // Mes Posible no puede ser el mes PLT es el año
                                                        {
                                                            anioDeducido = FECHAACERTIJO[1]; //Sin embargo sería algo malo porque el año nunca se da en medio en ningun formato
                                                            try
                                                            {
                                                                candidata1 = FECHAACERTIJO[0] + "/" + FECHAACERTIJO[2] + "/20" + FECHAACERTIJO[0];
                                                                candidata2 = FECHAACERTIJO[2] + "/" + FECHAACERTIJO[0] + "/20" + FECHAACERTIJO[0];
                                                                DiasDif1 = (Convert.ToDateTime(candidata1) - Convert.ToDateTime(actual)).Days;
                                                                DiasDif2 = (Convert.ToDateTime(candidata2) - Convert.ToDateTime(actual)).Days;
                                                                if (DiasDif1 > 0 && DiasDif2 > 0)
                                                                {
                                                                    if (DiasDif1 < DiasDif2)
                                                                    {
                                                                        diaDeducido = FECHAACERTIJO[0];
                                                                        mesDeducido = FECHAACERTIJO[2];
                                                                    }
                                                                    else
                                                                    {
                                                                        diaDeducido = FECHAACERTIJO[2];
                                                                        mesDeducido = FECHAACERTIJO[0];
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (DiasDif1 <= 0 && DiasDif2 <= 0)
                                                                    {
                                                                        if (DiasDif1 > DiasDif2)
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[0];
                                                                            mesDeducido = FECHAACERTIJO[2];
                                                                        }
                                                                        else
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[2];
                                                                            mesDeducido = FECHAACERTIJO[0];
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (DiasDif1 > 0) // DiasDif2<=0
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[0];
                                                                            mesDeducido = FECHAACERTIJO[2];
                                                                        }
                                                                        else // DiasDif2>0 y DiasDif1<=0
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[2];
                                                                            mesDeducido = FECHAACERTIJO[0];
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            catch { fechaRespuesta = "ERROR"; }
                                                        }
                                                        else //Todos los datos son menores a 12 y todos pueden ser el mes y dia Y el año es menor al actual considerando el siglo XXI a partir del siglo XXII ya no seria cierto
                                                        {
                                                            diaDeducido = FECHAACERTIJO[0];
                                                            mesDeducido = FECHAACERTIJO[1];
                                                            anioDeducido = FECHAACERTIJO[2];
                                                        }
                                                    }
                                                }
                                            }
                                            else { fechaRespuesta = "ERROR"; }//Ningun dato es menor o igual a 12 y no puede haber mes mayor a 12
                                            //*******Copiar hasta aca
                                        }
                                        else { fechaRespuesta = "ERROR"; } //El formato de la fecha no fue correcto
                                    }
                                }
                            }
                            else // ** dd/m/yy d/mm/yy d/m/yy ** mm/d/yy m/dd/yy m/d/yy ** yy/dd/m yy/d/mm yy/d/m ** yy/mm/d yy/m/dd yy/m/d
                            {
                                if (tamAcertijo >= 6)
                                {
                                    if (FECHAACERTIJO[0].Length == 1) { FECHAACERTIJO[0] = "0" + FECHAACERTIJO[0]; }
                                    if (FECHAACERTIJO[1].Length == 1) { FECHAACERTIJO[1] = "0" + FECHAACERTIJO[1]; }
                                    if (FECHAACERTIJO[2].Length == 1) { FECHAACERTIJO[2] = "0" + FECHAACERTIJO[2]; }
                                    DiaPosible = Convert.ToInt16(FECHAACERTIJO[0]);
                                    MesPosible = Convert.ToInt16(FECHAACERTIJO[1]);
                                    AnioPosible = Convert.ToInt16(FECHAACERTIJO[2]);
                                    //Pegarlo Aqui :)
                                    if (DiaPosible <= 12 || MesPosible <= 12 || AnioPosible <= 12)
                                    {
                                        if (DiaPosible > 12) //DiaPosible no puede ser el mes 
                                        {
                                            if (AnioPosible > 12) //AnioPosible tampoco puede ser el mes PLT MesPosible es el mes
                                            {
                                                mesDeducido = FECHAACERTIJO[1];
                                                if (DiaPosible <= 31 || AnioPosible <= 31)
                                                {
                                                    if (DiaPosible > 31) //DiaPosible no puede ser el dia PLT:
                                                    {
                                                        diaDeducido = FECHAACERTIJO[2];
                                                        anioDeducido = "20" + FECHAACERTIJO[0];
                                                    }
                                                    else
                                                    {
                                                        if (AnioPosible > 31) //AnioPosible no puede ser el dia PLT:
                                                        {
                                                            diaDeducido = FECHAACERTIJO[0];
                                                            anioDeducido = "20" + FECHAACERTIJO[2];
                                                        }
                                                        else //AnioPosible y DiaPosible pueden ser el dia
                                                        {
                                                            try
                                                            {
                                                                candidata1 = FECHAACERTIJO[0] + "/" + FECHAACERTIJO[1] + "/20" + FECHAACERTIJO[2];
                                                                candidata2 = FECHAACERTIJO[2] + "/" + FECHAACERTIJO[1] + "/20" + FECHAACERTIJO[0];
                                                                DiasDif1 = (Convert.ToDateTime(candidata1) - Convert.ToDateTime(actual)).Days;
                                                                DiasDif2 = (Convert.ToDateTime(candidata2) - Convert.ToDateTime(actual)).Days;
                                                                if (DiasDif1 > 0 && DiasDif2 > 0)
                                                                {
                                                                    if (DiasDif1 < DiasDif2)
                                                                    {
                                                                        diaDeducido = FECHAACERTIJO[0];
                                                                        anioDeducido = "20" + FECHAACERTIJO[2];
                                                                    }
                                                                    else
                                                                    {
                                                                        diaDeducido = FECHAACERTIJO[2];
                                                                        anioDeducido = "20" + FECHAACERTIJO[0];
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (DiasDif1 <= 0 && DiasDif2 <= 0)
                                                                    {
                                                                        if (DiasDif1 > DiasDif2)
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[0];
                                                                            anioDeducido = "20" + FECHAACERTIJO[2];
                                                                        }
                                                                        else
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[2];
                                                                            anioDeducido = "20" + FECHAACERTIJO[0];
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (DiasDif1 > 0) // DiasDif2<=0
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[0];
                                                                            anioDeducido = "20" + FECHAACERTIJO[2];
                                                                        }
                                                                        else // DiasDif2>0 y DiasDif1<=0
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[2];
                                                                            anioDeducido = "20" + FECHAACERTIJO[0];
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            catch { fechaRespuesta = "ERROR"; }
                                                        }
                                                    }
                                                }
                                                else { fechaRespuesta = "ERROR"; } //Ninguno de los datos es menor o igual a 31 no puede haber un dia mayor a 31
                                            }
                                            else //DiaPosible no puede ser el mes pero el AnioPosible SI puede ser el mes 
                                            {
                                                if (MesPosible > 12) // MesPosible no puede ser el mes PLT anio Posible es el mes 
                                                {
                                                    mesDeducido = FECHAACERTIJO[2];
                                                    //Y como el año nunca esta en medio en ningún formato
                                                    diaDeducido = FECHAACERTIJO[1];
                                                    anioDeducido = "20" + FECHAACERTIJO[0];
                                                }
                                                else // AnioPosible y MesPosible pueden ser el mes pero el diaPosible NO PLT Seguramente el DiaPosible es el año
                                                {
                                                    anioDeducido = "20" + FECHAACERTIJO[0];
                                                    try
                                                    {
                                                        candidata1 = FECHAACERTIJO[1] + "/" + FECHAACERTIJO[2] + "/20" + FECHAACERTIJO[0];
                                                        candidata2 = FECHAACERTIJO[2] + "/" + FECHAACERTIJO[1] + "/20" + FECHAACERTIJO[0];
                                                        DiasDif1 = (Convert.ToDateTime(candidata1) - Convert.ToDateTime(actual)).Days;
                                                        DiasDif2 = (Convert.ToDateTime(candidata2) - Convert.ToDateTime(actual)).Days;
                                                        if (DiasDif1 > 0 && DiasDif2 > 0)
                                                        {
                                                            if (DiasDif1 < DiasDif2)
                                                            {
                                                                diaDeducido = FECHAACERTIJO[1];
                                                                mesDeducido = FECHAACERTIJO[2];
                                                            }
                                                            else
                                                            {
                                                                diaDeducido = FECHAACERTIJO[2];
                                                                mesDeducido = FECHAACERTIJO[1];
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DiasDif1 <= 0 && DiasDif2 <= 0)
                                                            {
                                                                if (DiasDif1 > DiasDif2)
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[1];
                                                                    mesDeducido = FECHAACERTIJO[2];
                                                                }
                                                                else
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[2];
                                                                    mesDeducido = FECHAACERTIJO[1];
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (DiasDif1 > 0) // DiasDif2<=0
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[1];
                                                                    mesDeducido = FECHAACERTIJO[2];
                                                                }
                                                                else // DiasDif2>0 y DiasDif1<=0
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[2];
                                                                    mesDeducido = FECHAACERTIJO[1];
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch { fechaRespuesta = "ERROR"; }
                                                }
                                            }
                                        }
                                        else //DiaPosible SI Puede ser el mes
                                        {
                                            if (AnioPosible > 12) //AnioPosible NO puede ser el mes
                                            {
                                                if (MesPosible > 12) // MesPosible tampoco puede ser el mes PLT DiaPosible es el mes
                                                {
                                                    mesDeducido = FECHAACERTIJO[0];
                                                    //Y como el año nunca esta en medio en ningún formato
                                                    diaDeducido = FECHAACERTIJO[1];
                                                    anioDeducido = FECHAACERTIJO[2];
                                                }
                                                else // MesPosible y DiaPosible son menores a 12 PLT AnioPosible es el año
                                                {
                                                    anioDeducido = FECHAACERTIJO[2];
                                                    try
                                                    {
                                                        candidata1 = FECHAACERTIJO[0] + "/" + FECHAACERTIJO[1] + "/20" + FECHAACERTIJO[2];
                                                        candidata2 = FECHAACERTIJO[1] + "/" + FECHAACERTIJO[0] + "/20" + FECHAACERTIJO[2];
                                                        DiasDif1 = (Convert.ToDateTime(candidata1) - Convert.ToDateTime(actual)).Days;
                                                        DiasDif2 = (Convert.ToDateTime(candidata2) - Convert.ToDateTime(actual)).Days;
                                                        if (DiasDif1 > 0 && DiasDif2 > 0)
                                                        {
                                                            if (DiasDif1 < DiasDif2)
                                                            {
                                                                diaDeducido = FECHAACERTIJO[0];
                                                                mesDeducido = FECHAACERTIJO[1];
                                                            }
                                                            else
                                                            {
                                                                diaDeducido = FECHAACERTIJO[1];
                                                                mesDeducido = FECHAACERTIJO[0];
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DiasDif1 <= 0 && DiasDif2 <= 0)
                                                            {
                                                                if (DiasDif1 > DiasDif2)
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[0];
                                                                    mesDeducido = FECHAACERTIJO[1];
                                                                }
                                                                else
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[1];
                                                                    mesDeducido = FECHAACERTIJO[0];
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (DiasDif1 > 0) // DiasDif2<=0
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[0];
                                                                    mesDeducido = FECHAACERTIJO[1];
                                                                }
                                                                else // DiasDif2>0 y DiasDif1<=0
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[1];
                                                                    mesDeducido = FECHAACERTIJO[0];
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch { fechaRespuesta = "ERROR"; }
                                                }
                                            }
                                            else // DiaPosible y AnioPosible SI pueden ser el mes
                                            {
                                                if (MesPosible > 12) // Mes Posible no puede ser el mes PLT es el año
                                                {
                                                    anioDeducido = FECHAACERTIJO[1]; //Sin embargo sería algo malo porque el año nunca se da en medio en ningun formato
                                                    try
                                                    {
                                                        candidata1 = FECHAACERTIJO[0] + "/" + FECHAACERTIJO[2] + "/20" + FECHAACERTIJO[0];
                                                        candidata2 = FECHAACERTIJO[2] + "/" + FECHAACERTIJO[0] + "/20" + FECHAACERTIJO[0];
                                                        DiasDif1 = (Convert.ToDateTime(candidata1) - Convert.ToDateTime(actual)).Days;
                                                        DiasDif2 = (Convert.ToDateTime(candidata2) - Convert.ToDateTime(actual)).Days;
                                                        if (DiasDif1 > 0 && DiasDif2 > 0)
                                                        {
                                                            if (DiasDif1 < DiasDif2)
                                                            {
                                                                diaDeducido = FECHAACERTIJO[0];
                                                                mesDeducido = FECHAACERTIJO[2];
                                                            }
                                                            else
                                                            {
                                                                diaDeducido = FECHAACERTIJO[2];
                                                                mesDeducido = FECHAACERTIJO[0];
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DiasDif1 <= 0 && DiasDif2 <= 0)
                                                            {
                                                                if (DiasDif1 > DiasDif2)
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[0];
                                                                    mesDeducido = FECHAACERTIJO[2];
                                                                }
                                                                else
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[2];
                                                                    mesDeducido = FECHAACERTIJO[0];
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (DiasDif1 > 0) // DiasDif2<=0
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[0];
                                                                    mesDeducido = FECHAACERTIJO[2];
                                                                }
                                                                else // DiasDif2>0 y DiasDif1<=0
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[2];
                                                                    mesDeducido = FECHAACERTIJO[0];
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch { fechaRespuesta = "ERROR"; }
                                                }
                                                else //Todos los datos son menores a 12 y todos pueden ser el mes y dia Y el año es menor al actual considerando el siglo XXI a partir del siglo XXII ya no seria cierto
                                                {
                                                    diaDeducido = FECHAACERTIJO[0];
                                                    mesDeducido = FECHAACERTIJO[1];
                                                    anioDeducido = FECHAACERTIJO[2];
                                                }
                                            }
                                        }
                                    }
                                    else { fechaRespuesta = "ERROR"; }//Ningun dato es menor o igual a 12 y no puede haber mes mayor a 12
                                    //Hasta aca llego el pegado
                                }
                                else { fechaRespuesta = "ERROR"; }
                            }
                        }//fin del if(fechaIngresada=="")  
                        else // Eso quiere decir que HABIA LETRAS EN EL acertijo tal vez el nombre o las tres iniciales del nombre del mes
                        {
                            tamNomMes = fechaIngresada.Length;
                            if (tamNomMes >= 3)
                            {
                                acertijo = acertijo.ToUpper();
                                iniciales = fechaIngresada.Substring(0, 3);
                                iniciales = iniciales.ToUpper();
                                acertijo = acertijo.Replace(fechaIngresada, iniciales);

                                if (iniciales == "ENE") { mesDeducido = "01"; }
                                else if (iniciales == "FEB") { mesDeducido = "02"; }
                                else if (iniciales == "MAR") { mesDeducido = "03"; }
                                else if (iniciales == "ABR") { mesDeducido = "04"; }
                                else if (iniciales == "MAY") { mesDeducido = "05"; }
                                else if (iniciales == "JUN") { mesDeducido = "06"; }
                                else if (iniciales == "JUL") { mesDeducido = "07"; }
                                else if (iniciales == "AGO") { mesDeducido = "08"; }
                                else if (iniciales == "SEP") { mesDeducido = "09"; }
                                else if (iniciales == "OCT") { mesDeducido = "10"; }
                                else if (iniciales == "NOV") { mesDeducido = "11"; }
                                else if (iniciales == "DIC") { mesDeducido = "12"; }
                                else mesDeducido = "ERROR";
                                if (mesDeducido != "ERROR")
                                {
                                    tamAcertijo = acertijo.Length;
                                    if (tamAcertijo >= 10) // ** dd/MMM/yyyy=d/MMM/yyyy ** MMM/dd/yyyy=MMM/d/yyyy ** yyyy/MMM/dd=yyyy/MMM/d ** yyyy/dd/MMM=yyyy/d/MMM 
                                    {
                                        posSeparador = acertijo.IndexOf("/");
                                        if (posSeparador == 4) // ** yyyy/MMM/dd=yyyy/MMM/d ** yyyy/dd/MMM=yyyy/d/MMM
                                        {
                                            anioDeducido = FECHAACERTIJO[0];
                                            if (FECHAACERTIJO[1].ToUpper() == fechaIngresada) { diaDeducido = FECHAACERTIJO[2]; }// ** yyyy/MMM/dd = yyyy/MMM/d
                                            else { diaDeducido = FECHAACERTIJO[1]; } // ** yyyy/dd/MMM=yyyy/d/MMM
                                        }
                                        else // ** dd/MMM/yyyy=d/MMM/yyyy ** MMM/dd/yyyy=MMM/d/yyyy
                                        {
                                            anioDeducido = FECHAACERTIJO[2];
                                            if (posSeparador == 3) { diaDeducido = FECHAACERTIJO[1]; } //** MMM/dd/yyyy=MMM/d/yyyy
                                            else { diaDeducido = FECHAACERTIJO[0]; } //** dd/MMM/yyyy=d/MMM/yyyy
                                        }
                                    }
                                    else //La longitud del acertijo es menor a 10 **** dd/MMM/yy=d/MMM/yy ** yy/MMM/dd=yy/MMM/d ** yy/dd/MMM=yy/d/MMM ** MMM/dd/yy=MMM/d/yy 
                                    {
                                        posSeparador = acertijo.IndexOf("/");
                                        if (posSeparador == 3) // ** MMM/dd/yy=MMM/d/yy 
                                        {
                                            diaDeducido = FECHAACERTIJO[1];
                                            anioDeducido = "20" + FECHAACERTIJO[2];
                                        }
                                        else // ** dd/MMM/yy=d/MMM/yy ** yy/MMM/dd=yy/MMM/d ** yy/dd/MMM=yy/d/MMM
                                        {
                                            if (FECHAACERTIJO[2] == iniciales) //** yy/dd/MMM=yy/d/MMM //El año no está en medio en ningun formato
                                            {
                                                anioDeducido = "20" + FECHAACERTIJO[0];
                                                diaDeducido = FECHAACERTIJO[1];
                                            }
                                            else // ** dd/MMM/yy=d/MMM/yy ** yy/MMM/dd=yy/MMM/d
                                            {
                                                if ((FECHAACERTIJO[0].Length < 2) && (FECHAACERTIJO[2].Length == 2))//****d/MMM/yy
                                                {
                                                    anioDeducido = "20" + FECHAACERTIJO[2];
                                                    diaDeducido = "0" + FECHAACERTIJO[0];
                                                }
                                                else
                                                {
                                                    if ((FECHAACERTIJO[0].Length == 2) && (FECHAACERTIJO[2].Length < 2)) // yy/MMM/d
                                                    {
                                                        anioDeducido = "20" + FECHAACERTIJO[0];
                                                        diaDeducido = "0" + FECHAACERTIJO[2];
                                                    }
                                                    else
                                                    {
                                                        if ((FECHAACERTIJO[0].Length == 2) && (FECHAACERTIJO[2].Length == 2)) // yy/MMM/dd o dd/MMM/yy
                                                        {
                                                            DiaPosible = Convert.ToInt16(FECHAACERTIJO[0]);
                                                            AnioPosible = Convert.ToInt16(FECHAACERTIJO[2]);
                                                            if ((AnioPosible > 31) || (DiaPosible > 31))
                                                            {
                                                                if (DiaPosible <= 31)
                                                                {
                                                                    diaDeducido = FECHAACERTIJO[0];
                                                                    anioDeducido = "20" + FECHAACERTIJO[2];
                                                                }
                                                                else
                                                                {
                                                                    if (AnioPosible <= 31)
                                                                    {
                                                                        diaDeducido = FECHAACERTIJO[2];
                                                                        anioDeducido = "20" + FECHAACERTIJO[0];
                                                                    }
                                                                    else { fechaRespuesta = "ERROR"; } //**AMBOS DATOS SON MAYORES A 31 Y NO PUEDE HABER MAS DE 31 DÍAS EN UN MES
                                                                }
                                                            }
                                                            else //AMBOS DATOS SON MENORES o iguales A 31
                                                            {
                                                                try
                                                                {
                                                                    candidata1 = FECHAACERTIJO[0] + "/" + mesDeducido + "/20" + FECHAACERTIJO[2];
                                                                    candidata2 = FECHAACERTIJO[2] + "/" + mesDeducido + "/20" + FECHAACERTIJO[0];
                                                                    DiasDif1 = (Convert.ToDateTime(candidata1) - Convert.ToDateTime(actual)).Days;
                                                                    DiasDif2 = (Convert.ToDateTime(candidata2) - Convert.ToDateTime(actual)).Days;
                                                                    if (DiasDif1 > 0 && DiasDif2 > 0)
                                                                    {
                                                                        if (DiasDif1 < DiasDif2)
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[0];
                                                                            anioDeducido = "20" + FECHAACERTIJO[2];
                                                                        }
                                                                        else
                                                                        {
                                                                            diaDeducido = FECHAACERTIJO[2];
                                                                            anioDeducido = "20" + FECHAACERTIJO[0];
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (DiasDif1 <= 0 && DiasDif2 <= 0)
                                                                        {
                                                                            if (DiasDif1 > DiasDif2)
                                                                            {
                                                                                diaDeducido = FECHAACERTIJO[0];
                                                                                anioDeducido = "20" + FECHAACERTIJO[2];
                                                                            }
                                                                            else
                                                                            {
                                                                                diaDeducido = FECHAACERTIJO[2];
                                                                                anioDeducido = "20" + FECHAACERTIJO[0];
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (DiasDif1 > 0) // DiasDif2<=0
                                                                            {
                                                                                diaDeducido = FECHAACERTIJO[0];
                                                                                anioDeducido = "20" + FECHAACERTIJO[2];
                                                                            }
                                                                            else // DiasDif2>0 y DiasDif1<=0
                                                                            {
                                                                                diaDeducido = FECHAACERTIJO[2];
                                                                                anioDeducido = "20" + FECHAACERTIJO[0];
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                catch { fechaRespuesta = "ERROR"; }
                                                            }
                                                        }
                                                        else { fechaRespuesta = "ERROR"; } //El formato de fecha no es válido
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else { fechaRespuesta = "ERROR"; } //Las letras en el acertijo no corresponden al nombre de ningun mes **AL MENOS NO EN ESPAÑOL
                            }
                            else { fechaRespuesta = "ERROR"; } //Había menos de tres letras en el acertijo **NO ES POSIBLE DETERMINAR EL NOMBRE MES
                        } // Fin del else de que habia letras en el acertijo **PROBABLEMENTE EL MES FUE DADO CON NOMBRE
                        /*Aqui ya se obtuvo una fecha o ERROR*****************************************************************************************************
                         //Debemos validar que hay dos digitos por dia y dos por mes y cuatro por anio y que los dias son validos para el mes
                        */
                        //***************************************************************************************************************************************
                        if ((fechaRespuesta != "ERROR") && (fechaRespuesta != actual))
                        {
                            if (diaDeducido.Length < 2) { diaDeducido = "0" + diaDeducido; }
                            if (anioDeducido.Length == 2) { anioDeducido = "20" + anioDeducido; }
                            if (mesDeducido.Length < 2) { mesDeducido = "0" + mesDeducido; }
                            DiaPosible = Convert.ToInt16(diaDeducido);
                            MesPosible = Convert.ToInt16(mesDeducido);
                            if (DiaPosible <= 31 && MesPosible <= 12)
                            {
                                if (DiaPosible >= 29)
                                {
                                    if (DiaPosible >= 30)
                                    {
                                        if (DiaPosible == 30) //Solo hay problema con Febrero
                                        {
                                            if (MesPosible != 2) { fechaRespuesta = diaDeducido + "/" + mesDeducido + "/" + anioDeducido; } //Todos los otros meses tienen 30 dias}
                                            else { fechaRespuesta = "ERROR"; } //Febrero no puede tener mas de 29 Días
                                        }
                                        else //DiaPosible = 31 No puede ser Feb,Abr,Jun,Sep,Nov
                                        {
                                            if (MesPosible == 2 || MesPosible == 4 || MesPosible == 6 || MesPosible == 9 || MesPosible == 11) { fechaRespuesta = "ERROR"; } //Esos meses no tienen 31 días
                                            else { fechaRespuesta = diaDeducido + "/" + mesDeducido + "/" + anioDeducido; } //Los otros meses tienen 31 dias}
                                        }
                                    }
                                    else //DiaPosible = 29 PLT Sólo hay problema con Febrero
                                    {
                                        if (MesPosible == 2) //Hay que checar si el año es biciesto
                                        {
                                            AnioPosible = Convert.ToInt16(anioDeducido);
                                            if (EsBisiesto(AnioPosible) == SI) { fechaRespuesta = diaDeducido + "/" + mesDeducido + "/" + anioDeducido; }
                                            else { fechaRespuesta = "ERROR"; } //Es febrero tiene 29 dias pero el año no es biciesto
                                        }
                                        else { fechaRespuesta = diaDeducido + "/" + mesDeducido + "/" + anioDeducido; } //Todos los otros meses tienen 29 dias
                                    }
                                }
                                else { fechaRespuesta = diaDeducido + "/" + mesDeducido + "/" + anioDeducido; } //Todos los meses tienen 28 dias
                            }
                            else { fechaRespuesta = "ERROR"; } //No puede haber dia mayor a 31 ni mes mayor a 12
                        }
                        //*****************************************************************************************************************************************
                    }
                    else { fechaRespuesta = "ERROR"; } //El acertijo no corresponde a un formato de fecha porque no hay tres datos que corresponderian al dia, mes y año
                }
                else { fechaRespuesta = "ERROR"; } //El acertijo no corresponde a un formato de fecha adecuado porque no hay / como separador de los datos de la fecha
            }//Fin del if(acertijo!="")  ***Eso quiere decir que el acertijo era una cadena vacia pero no se hace nada y se regresa cadena vacia
            return (fechaRespuesta);
        }

        /************************************************************************************
            Guardar el archivo ARCHIVO LOG en Servidor de NU4it
            Nombre: Jorge Núñez
            Fecha: 19/09/2016
        /************************************************************************************/
        public void SubeLOG(string ruta_LOG, string NombreRobot)
        {
            string lineaTexto = "";
            if (File.Exists(ruta_LOG))
            {
                StreamReader sr = new StreamReader(ruta_LOG);
                lineaTexto = sr.ReadToEnd();
                sr.Close();
            }
            lineaTexto = lineaTexto.Replace("\n", ".,.");
            lineaTexto = lineaTexto.Replace(" ", "%20");
            lineaTexto = lineaTexto.Replace("(", "|");
            lineaTexto = lineaTexto.Replace(")", "|");
            int Tamanio = lineaTexto.Length;
            Console.WriteLine("# Caracteres: " + Tamanio);
            int aux = 0, maximo = 2000;
            if (Tamanio < maximo)
            {
                StatusRobots(NombreRobot, lineaTexto);
            }
            if (Tamanio > maximo)
            {
                int ToLi = Convert.ToInt32(Tamanio / maximo);
                for (int i = 0; i < ToLi; i++)
                {
                    string lineaTextoNEW = lineaTexto.Substring(aux, maximo) + "~";
                    StatusRobots(NombreRobot, lineaTextoNEW);
                    aux += maximo;
                }
                int Sobra = Tamanio - aux;
                string lineaTextoNEW2 = "~" + lineaTexto.Substring(aux, Sobra);
                StatusRobots(NombreRobot, lineaTextoNEW2);
            }
        }

        public Boolean StatusRobots(string NombreRobot, String texto)
        {
            Boolean bit;
            try
            {
                String pathServer = "http://www.nu4itautomation.com/bitacora.php?";
                String Cliente = NombreRobot + "_status";
                char unSeparador = '&';
                SHDocVw.InternetExplorer Browser;
                Browser = new SHDocVw.InternetExplorer();
                Browser.Navigate(pathServer + "cliente=" + Cliente + unSeparador + "texto=" + texto);
                System.Threading.Thread.Sleep(2000);
                Browser.Visible = false;
                Browser.Quit();
                bit = true;
                Console.WriteLine("SE ENVIO AL ARCHIVO: http://www.nu4itautomation.com/doc/" + Cliente + ".txt");
            }
            catch (Exception)
            {
                Console.WriteLine("NO SE ENVIO");
                bit = false;
            }
            return bit;
        }


        /*************************************************************************************
           Metodo para obtener el contenido del clipboard
           Nombre: Marco A. Villeda
           Fecha: 19/09/2016
        /************************************************************************************/
        public string clipboardObtenerTexto()
        {
            string clipboard = "Sin valor...";
            Thread staThread = new Thread(x =>
            {
                try
                {
                    clipboard = Clipboard.GetText();
                }
                catch (Exception ex)
                {
                    clipboard = ex.Message;
                }
            });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            return clipboard;
        }

        /*************************************************************************************
          Metodo para agregar contenido al clipboard
          Nombre: Marco A. Villeda
          Fecha: 19/09/2016
       /************************************************************************************/
        public void clipboardAlmacenaTexto(string valor)
        {
            Thread staThread = new Thread(x =>
            {
                try
                {
                    Clipboard.SetText(valor);
                }
                catch (Exception e)
                {
                }

            });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
        }

        /*************************************************************************************
        Función que regresa la posición del arreglo donde esté ubicado la palabra a buscar Exacta (==) (sin indexof)
        modificación por Marco A. Villeda
        19/09/2016
       /************************************************************************************/
        public int UbicadoEnPos_2(string[] Arreglo, string palabrabuscar, int ConMod)
        {
            int ubicacion, indPos, TamanioArreglo, UbEnc, NoEsta;
            string ContenidoComparar;
            NoEsta = -1;
            TamanioArreglo = Arreglo.Length;
            indPos = 0;
            UbEnc = NO;
            if (ConMod == SI) { palabrabuscar = Modifica(palabrabuscar); }
            while ((indPos < TamanioArreglo) && (UbEnc == NO))
            {
                ContenidoComparar = Arreglo[indPos];
                if (ConMod == SI) { ContenidoComparar = Modifica(ContenidoComparar); }
                if (ContenidoComparar == palabrabuscar) { UbEnc = SI; }
                indPos++;
            }
            if (UbEnc == SI)
            {
                indPos--;
                ubicacion = indPos;
            }
            else { ubicacion = NoEsta; }
            return (ubicacion);
        }

        public String pasartDTaString(DataTable tab, int inicio, Boolean titulos)
        {
            String texto = "";
            String titles = "";
            int filas = tab.Rows.Count, column = tab.Columns.Count, conta = 0;
            String[] aux = new String[filas], aux2 = new String[column];

            if (titulos)
                foreach (DataColumn item in tab.Columns)
                    titles += item.ColumnName + "\t";

            titles = titles + Environment.NewLine;

            for (int i = inicio; i < tab.Rows.Count; i++)
            {
                for (int e = 0; e < column; e++)
                {
                    if (String.IsNullOrEmpty(tab.Rows[i].Field<string>(e)))
                    {
                        aux2[e] = "";
                    }
                    else
                    {
                        aux2[e] = tab.Rows[i].Field<string>(e);
                        aux2[e] = aux2[e].Replace("\r", "").Replace("\n", "");
                    }
                }
                aux[conta] = String.Join("\t", aux2); conta++;
            }
            texto = String.Join(Environment.NewLine, aux);
            texto = titles + texto;
            return texto;
        }
        public DataTable StringaDT(string texto, bool titulo)
        {
            DataTable tabla = new DataTable();
            int inicio = 1;
            string[] contenido = texto.Replace("\r", "").Split('\n');

            if (contenido.Length != 0)
            {
                string[] listTitulos = contenido[0].Split('\t');
                if (titulo == true)
                {
                    for (int i = 0; i < listTitulos.Length; i++)
                        tabla.Columns.Add(listTitulos[i]);
                }
                else
                {
                    inicio = 0;
                    for (int i = 0; i < listTitulos.Length; i++)
                    {
                        tabla.Columns.Add("Columna " + i);
                        listTitulos[i] = "Columna " + i;
                    }
                }

                for (int i = 0; i < contenido.Length - 1; i++)
                {
                    tabla.Rows.Add();
                    for (int x = 0; x < listTitulos.Length; x++)
                    {
                        string[] datos = contenido[i + inicio].Split('\t');

                        for (int d = 0; d < datos.Length; d++)
                        {
                            try
                            {
                                tabla.Rows[i][listTitulos[d]] = datos[d];
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }
            return tabla;
        }

        public DataTable pasartStringaDT_1(string texto, Boolean conTitulos)
        {
            DataTable BD = new DataTable();
            int indiceInicio = 1;
            string[] TextoArchivo = texto.Replace("\r", "").Split('\n');
            //Obteniendo datos para encabezados
            if (TextoArchivo.Length != 0)
            {
                string[] Titulos = TextoArchivo[0].Split('\t');
                if (conTitulos == true)
                {
                    for (int i = 0; i < Titulos.Length; i++)
                        BD.Columns.Add(Titulos[i]);
                }
                else
                {
                    indiceInicio = 0;
                    for (int i = 0; i < Titulos.Length; i++)
                    {
                        BD.Columns.Add("Columna " + i);
                        Titulos[i] = "Columna " + i;
                    }
                }
                //Agregando las demas columnas
                for (int i = 0; i < TextoArchivo.Length - 1; i++)
                {
                    BD.Rows.Add();
                    for (int x = 0; x < Titulos.Length; x++)
                    {
                        string[] datos = TextoArchivo[i + indiceInicio].Split('\t');
                        //Insertando datos
                        for (int d = 0; d < datos.Length; d++)
                            try { BD.Rows[i][Titulos[d]] = datos[d]; }
                            catch (Exception) { }
                    }
                }
            }
            return (BD);
        }
    }
}
