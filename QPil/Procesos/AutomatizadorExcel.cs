/*Fernando Rivelino Ortiz Martínez
 Nü4ItAutomation
 En está clase se lleva todo lo referente a la automatización de excel:
 Lectura de Archivos
 Modificación de la información en los Archivos
 Dar formato a rangos*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;

using Nu4it;
using nu4itExcel;

namespace QPil
{
    class AutomatizadorExcel
    {
        private static readonly int NO = 0, SI = 1, MAX = 10000;

        public static Excel.Application MiExcel;//Instancia de Excel
        public static Excel.Workbook ArchivoTrabajoExcel;
        public static Excel.Worksheet HojaExcel;
        public static Excel.Range Rango;
        private static usaR objNu4 = new usaR(); //Objeto para usar funciones generales 
        private static nuExcel objNuExcel = new nuExcel(); //Objeto para usar funciones sobre Excel

        //Funciones que se pueden usuar en cualquier proyecto
        #region MétodosGenerales

        //Método que prepara la instancia de Excel que automatizara el Bot
        public static void PreparaInicioExcel()
        {
            objNuExcel.CerrarInstaciasExcelVacias();
            MiExcel = objNuExcel.ObtenerObjetoExcel();
            objNuExcel.InstanciaExcelVisible(MiExcel);
            objNuExcel.ActivarMensajesAlertas(MiExcel, NO);
        }

        //Método que crea una instancia nueva para que sea automatizada por el bot
        public static void PreparaInicioExcelNuevaInstancia()
        {
            objNuExcel.CerrarInstaciasExcelVacias();
            MiExcel = objNuExcel.InstanciaNueva();
            objNuExcel.InstanciaExcelVisible(MiExcel);
            objNuExcel.ActivarMensajesAlertas(MiExcel, NO);
        }

        //Método para que al finalizar el proceso del bot la instancia de Excel sea restaurada a las condiciones para el usuario 
        public static void NormalizarExcel()
        {
            try { objNuExcel.ActivarRecalculo(MiExcel, SI); } catch { }
            objNuExcel.ActivarMensajesAlertas(MiExcel, SI);
            objNuExcel.CerrarInstaciasExcelVacias();
        }

        //Función para limpiar el clipBoard Antes de ser usado para copiar información del Excel
        private static void limpiarClipboard()
        {
            Thread staThread = new Thread(x => { try { Clipboard.Clear(); } catch { } });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start(); staThread.Join();
        }

        //Método para buscar si hay un Archivo abierto con el nombre paasado por parametro (NombreArchivo)
        public static bool ObtenerArchivoExcelAbierto(string NombreArchivo, string VARSTR_BITACORA_LOG)
        {
            bool ArchivoObtenido = true;
            string NomArchPosible;
            int abiertos, pos, contAbiertos;
            contAbiertos = objNuExcel.CantidadArchivosAbiertos(MiExcel);
            if (contAbiertos > 0)
            {
                abiertos = objNuExcel.ContarArchivoAbiertosNombre(MiExcel, NombreArchivo);
                if (abiertos > 0)
                {
                    if (abiertos == 1)
                    {
                        pos = objNuExcel.PosArchivoAbiertoNombre(MiExcel, NombreArchivo);
                        NomArchPosible = objNuExcel.NombreArchivoAbiertoPos(MiExcel, pos);
                        bool result = HerramientasGral.MsgDuda("Encontre este archivo " + NomArchPosible + " Abierto.\n Deseas utilizarlo?");
                        if (result)
                        {
                            ArchivoTrabajoExcel = objNuExcel.ObtenerArchivoAbierto(MiExcel, pos);
                            objNuExcel.ActivarArchivo(ArchivoTrabajoExcel);
                        }
                        else { ArchivoObtenido = false; }
                    }
                    else //si hay mas de un archivo abierto con el mismo nombre de DATOSADICIONALES
                    {
                        ArchivoObtenido = true; //Para que marque ERROR y se detenga
                        objNu4.ReportarLog(VARSTR_BITACORA_LOG, "Hay mas de un archivo abierto con el nombre " + NombreArchivo + " y no se con cual trabajar!");
                        HerramientasGral.MsgAlertar("Hay mas de un archivo abierto con el nombre " + NombreArchivo + " y no se con cual trabajar!");
                    }
                }
                else { ArchivoObtenido = false; } //NO hay archivos con el nombre buscado abiertos 
            }
            else { ArchivoObtenido = false; } //NO hay archivos abiertos 
            return ArchivoObtenido;
        }

        //Método para abrir y activar el archivo de Excel pasado por parametro (RutaNombreArchivo)
        public static void AbrirElArchivoUsar(string RutaNombreArchivo)
        {
            MiExcel = objNuExcel.InstanciaNueva();
            ArchivoTrabajoExcel = objNuExcel.AbrirArchivo(RutaNombreArchivo, MiExcel);
        }

        //Método que SÓLO valida que en el archivo que se está usando exista la pestaña(Hoja) con nombre pasado por parametro (Usa Modifica)
        public static bool EncontrarEnArchivoPestania(string NomPestania)
        {
            bool PestaniEncontrada = true;
            int pestania;
            pestania = objNuExcel.HojaTrabajoSolicitada(ArchivoTrabajoExcel, NomPestania);
            if (pestania > 0)
            {
                HojaExcel = objNuExcel.ActivarPestaniaExcel(pestania, MiExcel, ArchivoTrabajoExcel);
                #region PestaniaOcultaDebeMostrarse
                //EN CASO DE QUE LA PESTAÑA ESTE OCULTA DESCOMENTAR SIGUIENTE CODIGO Y COMENTAR LÍNEA ANTERIOR DE CÓDIGO
                //try { HojaExcel = objNuExcel.ActivarPestaniaExcel(pestania, MiExcel, ArchivoTrabajoExcel); }
                //catch
                //{
                //    HojaExcel = ArchivoTrabajoExcel.Worksheets[pestania];
                //    HojaExcel.Visible = Excel.XlSheetVisibility.xlSheetVisible;
                //    HojaExcel = objNuExcel.ActivarPestaniaExcel(pestania, MiExcel, ArchivoTrabajoExcel);
                //}
                #endregion

            }
            else { PestaniEncontrada = false; }//no existe ninguna pestaña con el nombre que se esta buscando
            return PestaniEncontrada;
        }

        //Método que SÓLO valida que en el archivo que se está usando exista la pestaña(Hoja) con nombre pasado por parametro (NO Usa Modifica por eso debe se Exacto)
        public static bool EncontrarEnArchivoPestaniaExacta(string NomPestania)
        {
            bool PestaniEncontrada = true;
            int pestania;
            pestania = objNuExcel.HojaTrabajoSolicitada(ArchivoTrabajoExcel, NomPestania, NO);
            if (pestania > 0)
            {
                HojaExcel = objNuExcel.ActivarPestaniaExcel(pestania, MiExcel, ArchivoTrabajoExcel);
                #region PestaniaOcultaDebeMostrarse
                //EN CASO DE QUE LA PESTAÑA ESTE OCULTA DESCOMENTAR SIGUIENTE CODIGO Y COMENTAR LÍNEA ANTERIOR DE CÓDIGO
                //try { HojaExcel = objNuExcel.ActivarPestaniaExcel(pestania, MiExcel, ArchivoTrabajoExcel); }
                //catch
                //{
                //    HojaExcel = ArchivoTrabajoExcel.Worksheets[pestania];
                //    HojaExcel.Visible = Excel.XlSheetVisibility.xlSheetVisible;
                //    HojaExcel = objNuExcel.ActivarPestaniaExcel(pestania, MiExcel, ArchivoTrabajoExcel);
                //}
                #endregion

            }
            else { PestaniEncontrada = false; }//no existe ninguna pestaña con el nombre que se esta buscando
            return PestaniEncontrada;
        }

        //Método para que se trabaje con la primer pestaña del archivo
        public static bool PrimerPestaniaPidiendoValidacion()
        {
            bool Sigue = true;
            string NomHoja;
            HojaExcel = objNuExcel.ActivarPestaniaExcel(1, MiExcel, ArchivoTrabajoExcel);
            #region PestaniaOcultaDebeMostrarse
            //EN CASO DE QUE LA PESTAÑA ESTE OCULTA DESCOMENTAR SIGUIENTE CODIGO Y COMENTAR LÍNEA ANTERIOR DE CÓDIGO 
            //try { HojaExcel = objNuExcel.ActivarPestaniaExcel(1, MiExcel, ArchivoTrabajoExcel); }
            //catch
            //{
            //    HojaExcel = ArchivoTrabajoExcel.Worksheets[1];
            //    HojaExcel.Visible = Excel.XlSheetVisibility.xlSheetVisible;
            //    HojaExcel = objNuExcel.ActivarPestaniaExcel(1, MiExcel, ArchivoTrabajoExcel);
            //}
            #endregion
            NomHoja = objNuExcel.NombreHojaEn(1, ArchivoTrabajoExcel);
            bool result = HerramientasGral.MsgDuda("¿Quiere utilizar la hoja " + NomHoja + "?");
            if (!result)
            {
                HerramientasGral.MsgAlertar("Por favor coloque la hoja con la que desea trabajar en la posición inicial y después presione botón Aceptar. ");
            }
            HojaExcel = objNuExcel.ActivarPestaniaExcel(1, MiExcel, ArchivoTrabajoExcel);
            return Sigue;
        }

        //Método para validar que la hoja del archivo que se está usando tiene los encabezados necesarios para trabajar (TítulosNecesarios)
        public static bool ValidarEncabezados(int FILA_ENCABEZADOS, List<string> TitulosNecesarios, int[] COLUMNAS_EXCEL_ENCABEZADOS, string VARSTR_BITACORA_LOG)
        {
            bool EncabezadosValidos = true;
            int ultCol, EncabezadosEncontrados, indCol, NUM_ENC_BUSCAR, Recorrido;
            string texCel, EncabezadosFaltantes;
            try { MiExcel.CutCopyMode = 0; } catch { }
            try { Clipboard.Clear(); } catch { }
            if (FILA_ENCABEZADOS != 0)
            {
                ultCol = objNuExcel.UltimaColumna(HojaExcel, Convert.ToString(FILA_ENCABEZADOS));
                indCol = 1;
                NUM_ENC_BUSCAR = TitulosNecesarios.Count;
                EncabezadosEncontrados = 0;
                while ((indCol <= ultCol) && (EncabezadosEncontrados < NUM_ENC_BUSCAR))
                {
                    texCel = objNuExcel.TextoCelda(HojaExcel, FILA_ENCABEZADOS, indCol);
                    if (!string.IsNullOrEmpty(texCel))
                    {
                        texCel = objNu4.Modifica(texCel);

                        for (Recorrido = 0; Recorrido < NUM_ENC_BUSCAR; Recorrido++)
                        {
                            if (texCel == TitulosNecesarios.ElementAt(Recorrido))
                            {
                                if (COLUMNAS_EXCEL_ENCABEZADOS[Recorrido] == 0)
                                {
                                    COLUMNAS_EXCEL_ENCABEZADOS[Recorrido] = indCol;
                                    EncabezadosEncontrados++;
                                }
                            }
                        }
                    }
                    indCol++;
                }
                if (EncabezadosEncontrados == NUM_ENC_BUSCAR)
                {
                    EncabezadosValidos = true;
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "SI SE ENCONTRARON TODOS LOS ENCABEZADOS NECESARIOS PARA TRABAJAR");
                }
                else
                {
                    EncabezadosValidos = false;
                    EncabezadosFaltantes = "";
                    List<string> FaltanEncabezados = new List<string>();
                    for (Recorrido = 0; Recorrido < COLUMNAS_EXCEL_ENCABEZADOS.Length; Recorrido++)
                    {
                        if (COLUMNAS_EXCEL_ENCABEZADOS[Recorrido] == 0) { FaltanEncabezados.Add(TitulosNecesarios.ElementAt(Recorrido)); }
                    }
                    EncabezadosFaltantes = string.Join(",", FaltanEncabezados);
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "NO SE ENCONTRARON LOS ENCABEZADOS: " + EncabezadosFaltantes);
                    HerramientasGral.MsgAlertar("NO SE ENCONTRARON LOS ENCABEZADOS: " + EncabezadosFaltantes);
                }
            }
            else
            {
                EncabezadosValidos = false;
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "NO SE ENCONTRO EL RENGLON DE ENCABEZADOS NECESARIOS PARA TRABAJAR");
                HerramientasGral.MsgAlertar("NO SE ENCONTRO EL RENGLON DE ENCABEZADOS NECESARIOS PARA TRABAJAR");
            }
            return EncabezadosValidos;
        }

        //Método para validar que en el archivo que se está usando exista la pestaña(Hoja) con la que se va a trabajar (NomPestaniaBuscar) y depués invoca la función para validar encabezados
        public static bool ValidarPestaniaEncabezados(string NomPestaniaBuscar, List<string> EncabezadosBuscar, int[] COLUMNAS_EXCEL_ENCABEZADOS, string EncabezadoUno, string EncabezadoDos, string VARSTR_BITACORA_LOG)
        {
            bool ArchivoValido = true;
            int pestania, FILA_ENCABEZADOS;
            pestania = objNuExcel.HojaTrabajoSolicitada(ArchivoTrabajoExcel, NomPestaniaBuscar);
            if (pestania > 0)
            {
                HojaExcel = objNuExcel.ActivarPestaniaExcel(pestania, MiExcel, ArchivoTrabajoExcel);
                FILA_ENCABEZADOS = objNuExcel.filaTitulos_2(ArchivoTrabajoExcel, HojaExcel, EncabezadoUno, EncabezadoDos);
                #region PestaniaOcultaDebeMostrarse
                //EN CASO DE QUE LA PESTAÑA ESTE OCULTA DESCOMENTAR SIGUIENTE CODIGO Y COMENTAR LÍNEA ANTERIOR DE CÓDIGO
                //try { HojaExcel = objNuExcel.ActivarPestaniaExcel(pestania, MiExcel, ArchivoTrabajoExcel); }
                //catch
                //{
                //    HojaExcel = ArchivoTrabajoExcel.Worksheets[pestania];
                //    HojaExcel.Visible = Excel.XlSheetVisibility.xlSheetVisible;
                //    HojaExcel = objNuExcel.ActivarPestaniaExcel(pestania, MiExcel, ArchivoTrabajoExcel);
                //}
                #endregion
                ArchivoValido = ValidarEncabezados(FILA_ENCABEZADOS, EncabezadosBuscar, COLUMNAS_EXCEL_ENCABEZADOS, VARSTR_BITACORA_LOG);
                if (!ArchivoValido)
                {
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "LOS ENCABEZADOS NO FUERON VALIDOS");
                    HerramientasGral.MsgAlertar("LOS ENCABEZADOS NO FUERON VALIDOS ");
                }
            }
            else //no existe ninguna pestaña con el nombre que se esta buscando
            {
                ArchivoValido = false;
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "No se encontro la pestaña " + NomPestaniaBuscar);
                HerramientasGral.MsgAlertar("No se encontro la pestaña " + NomPestaniaBuscar);
            }
            return ArchivoValido;
        }

        //Método para pasar al DataTable (TABLALLENAR) la información de la pestaña de Excel
        public static void PasarInfoExcelDataTable(int FILA_ENCABEZADOS, int[] COLUMNAS_EXCEL_ENCABEZADOS, DataTable TABLALLENAR)
        {
            int posTit, tottitulos, columnaExcel, indRen, totRenDT, totRenArr, aux;
            string[] DATOS = new string[MAX];
            tottitulos = COLUMNAS_EXCEL_ENCABEZADOS.Length;
            FILA_ENCABEZADOS++;
            for (posTit = 0; posTit < tottitulos; posTit++)
            {
                columnaExcel = COLUMNAS_EXCEL_ENCABEZADOS[posTit];
                DATOS = objNuExcel.LeerColumna(FILA_ENCABEZADOS, columnaExcel, MiExcel, ArchivoTrabajoExcel, HojaExcel);
                try { MiExcel.CutCopyMode = 0; } catch { }
                try { Clipboard.Clear(); } catch { }
                totRenArr = DATOS.Length;
                if (posTit == 0)
                {
                    for (indRen = 0; indRen < totRenArr; indRen++)
                    {
                        DataRow RenglonInsertar = TABLALLENAR.NewRow();
                        RenglonInsertar[0] = DATOS[indRen];
                        TABLALLENAR.Rows.Add(RenglonInsertar);
                    }
                }
                else
                {
                    totRenDT = TABLALLENAR.Rows.Count;
                    for (indRen = 0; indRen < totRenDT; indRen++)
                    {
                        if (indRen < totRenArr) { TABLALLENAR.Rows[indRen][posTit] = DATOS[indRen]; }
                        else { TABLALLENAR.Rows[indRen][posTit] = ""; }
                    }
                    if (totRenDT < totRenArr)
                    {
                        for (aux = totRenDT; aux < totRenArr; aux++)
                        {
                            TABLALLENAR.Rows.Add();
                            TABLALLENAR.Rows[aux][posTit] = DATOS[aux];
                        }
                    }
                }
            }
        }

        //Método para pasar al DataTable (TABLALLENAR) la información de la pestaña de Excel

        public static void PasarInfoExcelDataTable_2(int FILA_ENCABEZADOS, Dictionary<string, List<string>> ENCOPC, DataTable TABLALLENAR)
        {//Esta funcion concidera que solo hay una columna valida por encabezado
            int posTit, tottitulos, columnaExcel, indRen, totRenDT, totRenArr, aux, i = 0;
            string[] DATOS = new string[MAX];
            tottitulos = ENCOPC.Keys.Count;
            FILA_ENCABEZADOS++;
            int[] COLUMNAS_EXCEL_ENCABEZADOS = new int[tottitulos];
            foreach (var item in ENCOPC.Keys)
            {
                List<string> valores = ENCOPC[item];
                aux = valores.Count;
                COLUMNAS_EXCEL_ENCABEZADOS[i] = int.Parse(valores[aux - 1]);
                i++;
            }

            for (posTit = 0; posTit < tottitulos; posTit++)
            {
                columnaExcel = COLUMNAS_EXCEL_ENCABEZADOS[posTit];
                DATOS = objNuExcel.LeerColumna(FILA_ENCABEZADOS, columnaExcel, MiExcel, ArchivoTrabajoExcel, HojaExcel);
                try { MiExcel.CutCopyMode = 0; } catch { }
                try { Clipboard.Clear(); } catch { }
                totRenArr = DATOS.Length;
                if (posTit == 0)
                {
                    for (indRen = 0; indRen < totRenArr; indRen++)
                    {
                        DataRow RenglonInsertar = TABLALLENAR.NewRow();
                        RenglonInsertar[0] = DATOS[indRen];
                        TABLALLENAR.Rows.Add(RenglonInsertar);
                    }
                }
                else
                {
                    totRenDT = TABLALLENAR.Rows.Count;
                    for (indRen = 0; indRen < totRenDT; indRen++)
                    {
                        if (indRen < totRenArr) { TABLALLENAR.Rows[indRen][posTit] = DATOS[indRen]; }
                        else { TABLALLENAR.Rows[indRen][posTit] = ""; }
                    }
                    if (totRenDT < totRenArr)
                    {
                        for (aux = totRenDT; aux < totRenArr; aux++)
                        {
                            TABLALLENAR.Rows.Add();
                            TABLALLENAR.Rows[aux][posTit] = DATOS[aux];
                        }
                    }
                }
            }
        }

        //Método que inserta filas en la HojaExcel   
        public static void InertarFilasExcel(string Fila, int CantidadRenglonesInsertar)
        {
            int RenglonesInsertados = 0;
            string Celda = "A" + Fila;
            objNuExcel.ActivarRecalculo(MiExcel, NO);
            while (RenglonesInsertados < CantidadRenglonesInsertar)
            {
                objNuExcel.InsertarFilasArriba(Celda, HojaExcel);
                objNuExcel.EsperarExcelListo(MiExcel);
                RenglonesInsertados++;
            }
        }

        //Función para limpiar toda la pestaña del Excel
        public static void LimpiarTodaHoja()
        {
            HojaExcel.Cells.Clear();
        }

        //Función para poner borde a las celdas que estén dentro del rango pasado por parametro
        public static void BordearRango(string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            Rango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            Rango.Select();
            Rango.BorderAround(Missing.Value, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic);
        }

        //Función para lectura del TEXTO de una columna de Excel Usando ClipoBoard 
        public static List<string> LeerColumnaTexto(int renglonIni, int columnaIni, Excel.Worksheet Pestania)
        {
            string primero, ult, col, InfoEnPortaPapeles = "", Informacion;
            int ultimo, i, RenRango, TamArreglo;
            Excel.Range Ran;
            col = objNuExcel.ColumnaCorrespondiente(columnaIni);//Obtiene la letra de la primera columna a seleccionar
            ultimo = objNuExcel.UltimoRenglon(ArchivoTrabajoExcel, col);
            primero = col + renglonIni;
            ult = col + ultimo;
            Ran = Pestania.get_Range(primero, ult);//Define Rango
            List<string> Datos2 = new List<string>();
            RenRango = Ran.Rows.Count;
            if (ultimo >= renglonIni)
            {
                string[] InfoColumna = new string[0];
                try { InfoEnPortaPapeles = objNu4.clipboardObtenerTexto(); } catch { }
                limpiarClipboard();
                Ran.Select();
                Ran.Copy();
                Informacion = objNu4.clipboardObtenerTexto();
                limpiarClipboard();
                try { objNu4.clipboardAlmacenaTexto(InfoEnPortaPapeles); } catch { }
                InfoColumna = Informacion.Split('\n'); TamArreglo = InfoColumna.Length;
                for (i = 0; i < TamArreglo; i++)
                {
                    Datos2.Add(InfoColumna[i].Replace("\r", ""));
                }
            }
            else { for (i = 0; i < RenRango; i++) { Datos2.Add(""); } }
            return (Datos2);

        }

        //Función para lectura del VALOR2 de una columna de Excel SIN USAR ClipoBoard
        public static List<string> LeerColumnaValue2(int renglonIni, int columnaIni, Excel.Worksheet Pestania)
        {
            string primero, ult, col;
            int ultimo, i, RenRango;
            
            Excel.Range Ran;
            col = objNuExcel.ColumnaCorrespondiente(columnaIni);//Obtiene la letra de la primera columna a seleccionar
            ultimo = objNuExcel.UltimoRenglon(ArchivoTrabajoExcel, col);
            primero = col + renglonIni;
            ult = col + ultimo;
            Ran = Pestania.get_Range(primero, ult);//Define Rango
            List<string> Datos2 = new List<string>();
            RenRango = Ran.Rows.Count;
            if (ultimo >= renglonIni)
            {
                if (Ran.Count > 1)
                {
                    object[,] DatosEnRenglon = Ran.Value2;


                    for (i = 0; i <= RenRango; i++)
                    {
                        if (DatosEnRenglon[i, 1] != null) { Datos2.Add(DatosEnRenglon[i, 1] + ""); }
                        else { Datos2.Add(""); }
                    }
                }
                else
                    Datos2.Add(Ran.Value2);
            }
            return Datos2;

        }

        //Función para lectura del VALOR de una columna de Excel SIN USAR ClipoBoard 
        public static List<string> LeerColumnaValue(int renglonIni, int columnaIni, Excel.Worksheet Pestania)
        {
            string primero, ult, col;
            int ultimo, i, RenRango;

            Excel.Range Ran;
            col = objNuExcel.ColumnaCorrespondiente(columnaIni);//Obtiene la letra de la primera columna a seleccionar
            ultimo = objNuExcel.UltimoRenglon(ArchivoTrabajoExcel, col);
            primero = col + renglonIni;
            ult = col + ultimo;
            Ran = Pestania.get_Range(primero, ult);//Define Rango
            List<string> Datos2 = new List<string>();
            RenRango = Ran.Rows.Count;
            if (ultimo >= renglonIni)
            {
                object[,] DatosEnRenglon = Ran.Value;

                for (i = 1; i <= RenRango; i++)
                {
                    if (DatosEnRenglon[i, 1] != null) { Datos2.Add(DatosEnRenglon[i, 1] + ""); }
                    else { Datos2.Add(""); }
                }
            }
            return (Datos2);
        }

        //Función que llena DataTable con información del excel: Value2 de todas las columnas 
        public static void PasarInformacionExcelV2DT(int FILA_ENCABEZADOS, int[] COLUMNAS_EXCEL_ENCABEZADOS, DataTable TABLALLENAR)
        {
            int posTit, tottitulos, columnaExcel, indRen, totRenDT, totRenArr, aux;
            List<string> DATOS = new List<string>();
            tottitulos = COLUMNAS_EXCEL_ENCABEZADOS.Length;
            FILA_ENCABEZADOS++;
            for (posTit = 0; posTit < tottitulos; posTit++)
            {
                columnaExcel = COLUMNAS_EXCEL_ENCABEZADOS[posTit];
                //DATOS = LeerColumnaValue2(FILA_ENCABEZADOS, columnaExcel, HojaExcel);
                try { MiExcel.CutCopyMode = 0; } catch { }
                try { Clipboard.Clear(); } catch { }
                totRenArr = DATOS.Count;
                if (posTit == 0)
                {
                    for (indRen = 0; indRen < totRenArr; indRen++)
                    {
                        DataRow RenglonInsertar = TABLALLENAR.NewRow();
                        RenglonInsertar[0] = DATOS.ElementAt(indRen); // DATOS[indRen];
                        TABLALLENAR.Rows.Add(RenglonInsertar);
                    }
                }
                else
                {
                    totRenDT = TABLALLENAR.Rows.Count;
                    for (indRen = 0; indRen < totRenDT; indRen++)
                    {
                        if (indRen < totRenArr) { TABLALLENAR.Rows[indRen][posTit] = DATOS.ElementAt(indRen); } //DATOS[indRen]; }
                        else { TABLALLENAR.Rows[indRen][posTit] = ""; }
                    }
                    if (totRenDT < totRenArr)
                    {
                        for (aux = totRenDT; aux < totRenArr; aux++)
                        {
                            TABLALLENAR.Rows.Add();
                            TABLALLENAR.Rows[aux][posTit] = DATOS.ElementAt(aux); // // DATOS[aux];
                        }
                    }
                }
            }
        }

        //Función que llena DataTable con información del excel: Value de todas las columnas
        public static void PasarInformacionExcelValueDT(int FILA_ENCABEZADOS, int[] COLUMNAS_EXCEL_ENCABEZADOS, DataTable TABLALLENAR)
        {
            int posTit, tottitulos, columnaExcel, indRen, totRenDT, totRenArr, aux;
            List<string> DATOS = new List<string>();
            tottitulos = COLUMNAS_EXCEL_ENCABEZADOS.Length;
            FILA_ENCABEZADOS++;
            for (posTit = 0; posTit < tottitulos; posTit++)
            {
                columnaExcel = COLUMNAS_EXCEL_ENCABEZADOS[posTit];
                DATOS = LeerColumnaValue(FILA_ENCABEZADOS, columnaExcel, HojaExcel);
                try { MiExcel.CutCopyMode = 0; } catch { }
                try { Clipboard.Clear(); } catch { }
                totRenArr = DATOS.Count;
                if (posTit == 0)
                {
                    for (indRen = 0; indRen < totRenArr; indRen++)
                    {
                        DataRow RenglonInsertar = TABLALLENAR.NewRow();
                        RenglonInsertar[0] = DATOS.ElementAt(indRen); // DATOS[indRen];
                        TABLALLENAR.Rows.Add(RenglonInsertar);
                    }
                }
                else
                {
                    totRenDT = TABLALLENAR.Rows.Count;
                    for (indRen = 0; indRen < totRenDT; indRen++)
                    {
                        if (indRen < totRenArr) { TABLALLENAR.Rows[indRen][posTit] = DATOS.ElementAt(indRen); } //DATOS[indRen]; }
                        else { TABLALLENAR.Rows[indRen][posTit] = ""; }
                    }
                    if (totRenDT < totRenArr)
                    {
                        for (aux = totRenDT; aux < totRenArr; aux++)
                        {
                            TABLALLENAR.Rows.Add();
                            TABLALLENAR.Rows[aux][posTit] = DATOS.ElementAt(aux); // // DATOS[aux];
                        }
                    }
                }
            }
        }

        //Función que llena DataTable con información del excel: Texto de todas las columnas
        public static void PasarInformacionExcelTextoDT(int FILA_ENCABEZADOS, int[] COLUMNAS_EXCEL_ENCABEZADOS, DataTable TABLALLENAR)
        {
            int posTit, tottitulos, columnaExcel, indRen, totRenDT, totRenArr, aux;
            List<string> DATOS = new List<string>();
            tottitulos = COLUMNAS_EXCEL_ENCABEZADOS.Length;
            FILA_ENCABEZADOS++;
            for (posTit = 0; posTit < tottitulos; posTit++)
            {
                columnaExcel = COLUMNAS_EXCEL_ENCABEZADOS[posTit];
                DATOS = LeerColumnaTexto(FILA_ENCABEZADOS, columnaExcel, HojaExcel);
                try { MiExcel.CutCopyMode = 0; } catch { }
                try { Clipboard.Clear(); } catch { }
                totRenArr = DATOS.Count;
                if (posTit == 0)
                {
                    for (indRen = 0; indRen < totRenArr; indRen++)
                    {
                        DataRow RenglonInsertar = TABLALLENAR.NewRow();
                        RenglonInsertar[0] = DATOS.ElementAt(indRen); // DATOS[indRen];
                        TABLALLENAR.Rows.Add(RenglonInsertar);
                    }
                }
                else
                {
                    totRenDT = TABLALLENAR.Rows.Count;
                    for (indRen = 0; indRen < totRenDT; indRen++)
                    {
                        if (indRen < totRenArr) { TABLALLENAR.Rows[indRen][posTit] = DATOS.ElementAt(indRen); } //DATOS[indRen]; }
                        else { TABLALLENAR.Rows[indRen][posTit] = ""; }
                    }
                    if (totRenDT < totRenArr)
                    {
                        for (aux = totRenDT; aux < totRenArr; aux++)
                        {
                            TABLALLENAR.Rows.Add();
                            TABLALLENAR.Rows[aux][posTit] = DATOS.ElementAt(aux); // // DATOS[aux];
                        }
                    }
                }
            }
        }

        //Función que regresa el renglon que determina como fila donde están los titulos(Encabezados)
        public static int FilaTitulos(Excel.Worksheet Pestania, string Titulo1, string Titulo2)
        {
            int RengloTitulos = 0, renglon, RenglonMaxBuscar = 51, oportunidad, indCol, TotColumnas;
            bool Encontrado = false;
            string primerCasilla, ultimaCasilla;

            oportunidad = 1;
            do
            {
                renglon = 1;
                while (renglon < RenglonMaxBuscar && !Encontrado && oportunidad < 3)
                {
                    try
                    {
                        primerCasilla = "A" + Convert.ToString(renglon);
                        ultimaCasilla = "AX" + Convert.ToString(renglon);
                        Rango = Pestania.get_Range(primerCasilla, ultimaCasilla);
                        object[,] InfoRango = Rango.Value2;
                        TotColumnas = Rango.Columns.Count;
                        List<string> EncabezadoEnArchivo = new List<string>();

                        for (indCol = 1; indCol <= TotColumnas; indCol++)
                        {
                            if (InfoRango[1, indCol] != null)
                            {
                                if (oportunidad == 1) { EncabezadoEnArchivo.Add(InfoRango[1, indCol] + ""); }
                                else { EncabezadoEnArchivo.Add(objNu4.Modifica(InfoRango[1, indCol] + "")); }
                            }
                            else { EncabezadoEnArchivo.Add(""); }
                        }

                        if (EncabezadoEnArchivo.Contains(Titulo1) && EncabezadoEnArchivo.Contains(Titulo2))
                        {
                            RengloTitulos = renglon;
                            Encontrado = true;
                        }
                        else if (EncabezadoEnArchivo.Contains(Titulo1) || EncabezadoEnArchivo.Contains(Titulo2)) { RengloTitulos = renglon; }
                        renglon++;
                    }
                    catch { renglon++; }
                }
                if (RengloTitulos == 0)
                {
                    oportunidad++;
                    Titulo1 = objNu4.Modifica(Titulo1);
                    Titulo2 = objNu4.Modifica(Titulo2);
                }
                else { Encontrado = true; }
            } while (!Encontrado && oportunidad < 3);
            return RengloTitulos;
        }

        /*Las consolidaciones deben verse en los archivos combinando las celdas del transporte
          La función revisa la columna pasada por parametro y las que tienen el mismo transporte las combina*/
        public static void CombinarCeldasUnidades(int Columna, int PrimerRenglon, int CantidadRenglones, Excel.Worksheet Pestania)
        {
            int UltimaFila, indFila, RenIniRango, RenFinRango, cantceldas;
            string unidad, LetColumna, uniComparar, celdainicial, celdafinal;
            bool MismaUnidad;
            LetColumna = objNuExcel.ColumnaCorrespondiente(Columna);
            UltimaFila = (PrimerRenglon + CantidadRenglones) - 1;
            indFila = PrimerRenglon;
            while (indFila < UltimaFila)
            {
                MismaUnidad = true;
                cantceldas = 0;
                RenIniRango = indFila;
                unidad = objNuExcel.LeerTextoCelda(Pestania, indFila, Columna);
                while (MismaUnidad && indFila <= UltimaFila)
                {
                    indFila++;
                    uniComparar = objNuExcel.LeerTextoCelda(Pestania, indFila, Columna);
                    if (uniComparar != unidad) { MismaUnidad = false; }
                    else { cantceldas++; }
                }
                indFila--;
                if (cantceldas > 0)
                {
                    if (unidad != "" && unidad != null && unidad != " ") //Para que no combine celdas vacias
                    {
                        try { MiExcel.CutCopyMode = 0; } catch { }
                        try { Clipboard.Clear(); } catch { }
                        objNuExcel.EsperarExcelListo(MiExcel);
                        RenFinRango = indFila;
                        celdainicial = LetColumna + Convert.ToString(RenIniRango);
                        celdafinal = LetColumna + Convert.ToString(RenFinRango);
                        objNuExcel.CombinarCeldas(celdainicial, celdafinal, Pestania);
                        objNuExcel.AlineacionHorizontal(3, celdainicial, celdafinal, Pestania);
                        objNuExcel.AlineacionVertical(3, celdainicial, celdafinal, Pestania);
                        try { MiExcel.CutCopyMode = 0; } catch { }
                        try { Clipboard.Clear(); } catch { }
                        objNuExcel.EsperarExcelListo(MiExcel);
                    }
                }
                indFila++;
            }

        }
        public static bool ValidacionEncabezados(int FILA_ENCABEZADOS, Dictionary<string, List<string>> ENCOPC, string VARSTR_BITACORA_LOG)
        {/*trabaja con Diccionario, tanto con los titulos en en el area de list como con titulos en area de key
            */
            bool EncabezadosValidos = true;
            int ultCol, EncabezadosEncontrados, indCol, NUM_ENC_BUSCAR, Recorrido;
            string texCel, EncabezadosFaltantes;
            int[] ColumnasExcelEncabezados = new int[ENCOPC.Keys.Count];
            for (int j = 0; j < ColumnasExcelEncabezados.Length; j++) { ColumnasExcelEncabezados[j] = 0; }
            try { MiExcel.CutCopyMode = 0; } catch { }
            try { Clipboard.Clear(); } catch { }
            if (FILA_ENCABEZADOS != 0)
            {
                ultCol = objNuExcel.UltimaColumna(HojaExcel, Convert.ToString(FILA_ENCABEZADOS));
                indCol = 1;
                NUM_ENC_BUSCAR = ENCOPC.Keys.Count;
                EncabezadosEncontrados = 0;
                Recorrido = 0;
                while (indCol <= ultCol)
                {
                    texCel = objNuExcel.TextoCelda(HojaExcel, FILA_ENCABEZADOS, indCol);
                    if (!string.IsNullOrEmpty(texCel))
                    {
                        //si deseas una busqueda exacta en el caso de tener varias columnas similares como doc1 doc2, etc .Comenta la sig linea
                        texCel = objNu4.Modifica(texCel);

                        List<string> OpcionesEncabezado = new List<string>();

                        //si el valor se encuentra en los key lo optiene directo
                        if (ENCOPC.Keys.Contains<string>(texCel))
                        {
                            OpcionesEncabezado = ENCOPC[texCel];
                            if (OpcionesEncabezado.Contains(texCel) == false)
                                OpcionesEncabezado.Add(texCel);
                        }
                        else
                        {
                            //recupera la lista si el valor a buscar se encuentra en values
                            List<string> llave = ENCOPC.Keys.ToList<string>();
                            foreach (var item in llave)
                            {
                                OpcionesEncabezado = ENCOPC[item];
                                if (OpcionesEncabezado.Contains(texCel))
                                    break;
                            }

                        }
                        if (OpcionesEncabezado.Contains(texCel))
                        {
                            OpcionesEncabezado.Add(indCol.ToString());
                            // EncabezadosEncontrados++;
                            try { ColumnasExcelEncabezados[Recorrido] = indCol; } catch { }
                            Recorrido++;

                        }


                    }
                    indCol++;
                }

                List<string> key = ENCOPC.Keys.ToList<string>();
                List<string> valores;
                int aux, val;
                List<string> FaltanEncabezados = new List<string>();
                //EncabezadosEncontrados = 0;
                foreach (var item in key)
                {
                    val = 0;
                    valores = ENCOPC[item];
                    aux = valores.Count() - 1;
                    try
                    {
                        val = Int32.Parse(valores[aux]);
                    }
                    catch (Exception)
                    {
                        FaltanEncabezados.Add(item);


                    }

                    if (val != 0)
                        EncabezadosEncontrados++;
                }
                if (EncabezadosEncontrados == NUM_ENC_BUSCAR)
                {
                    EncabezadosValidos = true;
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "SI SE ENCONTRARON TODOS LOS ENCABEZADOS NECESARIOS PARA TRABAJAR");
                }
                else
                {
                    EncabezadosValidos = false;
                    EncabezadosFaltantes = "";
                    EncabezadosFaltantes = string.Join(",", FaltanEncabezados);
                    objNu4.ReportarLog(VARSTR_BITACORA_LOG, "NO SE ENCONTRARON LOS ENCABEZADOS: " + EncabezadosFaltantes);
                    MessageBox.Show(new Form() { TopMost = true }, "NO SE ENCONTRARON LOS ENCABEZADOS: " + EncabezadosFaltantes);
                }
            }
            else
            {
                EncabezadosValidos = false;
                objNu4.ReportarLog(VARSTR_BITACORA_LOG, "NO SE ENCONTRO EL RENGLON DE ENCABEZADOS NECESARIOS PARA TRABAJAR");
                MessageBox.Show(new Form() { TopMost = true }, "NO SE ENCONTRO EL RENGLON DE ENCABEZADOS NECESARIOS PARA TRABAJAR");
            }
            return EncabezadosValidos;
        }


        #endregion

        public static List<string> Siniestros(string LOG)
        {

            List<string> siniestros = new List<string>();
            
            string archivo = HerramientasGral.DialogoSeleccionarArchivoUsar("C:\\", "de siniestros", LOG);
           AbrirElArchivoUsar(archivo);
            objNuExcel.ActivarMensajesAlertas(MiExcel, 0);
            HojaExcel = objNuExcel.ActivarPestaniaExcel(1, MiExcel, ArchivoTrabajoExcel);
            siniestros = LeerColumnaValue2(1, 1, HojaExcel);
            objNu4.ReportarLog(LOG, "Se abre el archivo " + archivo + " y se lee la primer columna del primera pagina para obtener los siniestros");
            objNuExcel.ActivarMensajesAlertas(MiExcel, 1);
            objNuExcel.CerrarArchivo(ArchivoTrabajoExcel);
            objNuExcel.CerrarInstaciasExcelVacias();
            return siniestros;

        }


    }
}
