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
using System.Data.OleDb;
using System.Windows.Threading;
using Nu4it;
using nu4itExcel;
using nu4itFox;

#endregion

namespace QPil.Resources.Clases
{
    /// <summary>
    /// By Jorge Nuñez
    /// 2017
    /// </summary>
    class JMNI : UserControl
    {
        //VARIBALE GLOBALES
        Metodos tools = new Metodos();
        Nu4it.usaR objNu4 = new Nu4it.usaR();
        nu4itExcel.nuExcel objNu4Excel = new nu4itExcel.nuExcel();
        public static Excel.Application MiExcel;
        public static Excel.Workbook ArchivoTrabajoExcel;
        public static Excel.Worksheet HojaExcel;

        #region EXCEL

        //ABRIR UN EXCEL Y REGRESA EL OBJETO DE MIEXCEL, ARCHIVOTRABAJO, HOJAEXCEL
        public Excel.Workbook AbrirArchivoExcel(string Ruta)
        {
            //Iniciado
            MiExcel = new Excel.Application();
            //Asignacion
            MiExcel.DisplayAlerts = false;
            MiExcel.Visible = false;
            Excel.Workbooks books = MiExcel.Workbooks;
            ArchivoTrabajoExcel = books.Open(Ruta);
            HojaExcel = ArchivoTrabajoExcel.Sheets[1];
            MiExcel.Visible = false;
            return ArchivoTrabajoExcel;
        }

        public Excel.Application AppExcel()
        {
            Excel.Application MiExcelNuevo = new Excel.Application();
            MiExcelNuevo.DisplayAlerts = false;
            MiExcelNuevo.Visible = true;
            MiExcel = MiExcelNuevo;
            return MiExcelNuevo;
        }

        public Excel.Workbook OpenBookExcel(string Ruta)
        {
            Excel.Workbooks books = AppExcel().Workbooks;
            Excel.Workbook ArchivoTrabajoExcelOPEN = books.Open(Ruta);
            ArchivoTrabajoExcel = ArchivoTrabajoExcelOPEN;
            return ArchivoTrabajoExcelOPEN;
        }

        public Excel.Workbook AddBookExcel()
        {
            Excel.Workbooks books = AppExcel().Workbooks;
            Excel.Workbook ArchivoTrabajoExcelADD = books.Add();
            ArchivoTrabajoExcel = ArchivoTrabajoExcelADD;
            return ArchivoTrabajoExcelADD;
        }

        public Excel.Worksheet SelectSheetExcel(Excel.Workbook ArchivoTrabajoExcel)
        {
            Excel.Worksheet HojaExcelSEL = ArchivoTrabajoExcel.Sheets[1];
            MiExcel.Visible = true;
            return HojaExcelSEL;
        }

        //OBTIENE DE UN ARCHIVO DE TEXTO LOS DATOS Y CREA UN DATATABLE
        public DataTable TXTaDataTable(string RutaTXT)
        {
            DataTable BD = new DataTable();
            string[] TextoArchivo = File.ReadAllLines(RutaTXT, Encoding.Default);
            //Obteniendo datos para encabezados
            string[] Titulos = TextoArchivo[0].Split('\t');
            for (int i = 0; i < Titulos.Length; i++)
                BD.Columns.Add(Titulos[i]);
            //Agregando las demas columnas
            for (int i = 0; i < TextoArchivo.Length - 1; i++)
            {
                BD.Rows.Add();
                for (int x = 0; x < Titulos.Length; x++)
                {
                    string[] datos = TextoArchivo[i + 1].Split('\t');
                    //Insertando datos
                    for (int d = 0; d < datos.Length; d++)
                        BD.Rows[i][Titulos[d]] = datos[d];
                }
            }
            return (BD);
        }

        public void AjustarFechas(Excel.Worksheet item, string Celda)
        {
            string[] APagosFECHAS = ObtenerColumna(Celda, item, MiExcel);
            APagosFECHAS = CambiarFormatoFechas(APagosFECHAS);
            string FechaPagos = string.Join(Environment.NewLine, APagosFECHAS);
            objNu4.clipboardAlmacenaTexto(FechaPagos);
            item.get_Range(Celda).EntireColumn.NumberFormat = "@";
            Thread.Sleep(200);
            item.get_Range(Celda).PasteSpecial();
            Thread.Sleep(200);
            item.get_Range(Celda).EntireColumn.NumberFormat = "dd/MMM/yyyy";
            Thread.Sleep(200);
            Excel.Range rangoFecha = item.get_Range(Celda).EntireColumn;
            rangoFecha.Replace("-", "/", Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, false, false, false);
        }

        public string[] CambiarFormatoFechas(string[] Fechas)
        {
            for (int i = 0; i < Fechas.Length; i++)
            {
                try
                {
                    //
                    if (Fechas[i].Contains("ene"))
                        Fechas[i] = Fechas[i].Replace("ene", "01").Replace("-", "/");
                    if (Fechas[i].Contains("feb"))
                        Fechas[i] = Fechas[i].Replace("feb", "02").Replace("-", "/");
                    if (Fechas[i].Contains("mar"))
                        Fechas[i] = Fechas[i].Replace("mar", "03").Replace("-", "/");
                    if (Fechas[i].Contains("abr"))
                        Fechas[i] = Fechas[i].Replace("abr", "04").Replace("-", "/");
                    if (Fechas[i].Contains("may"))
                        Fechas[i] = Fechas[i].Replace("may", "05").Replace("-", "/");
                    if (Fechas[i].Contains("jun"))
                        Fechas[i] = Fechas[i].Replace("jun", "06").Replace("-", "/");
                    if (Fechas[i].Contains("jul"))
                        Fechas[i] = Fechas[i].Replace("jul", "07").Replace("-", "/");
                    if (Fechas[i].Contains("ago"))
                        Fechas[i] = Fechas[i].Replace("ago", "08").Replace("-", "/");
                    if (Fechas[i].Contains("sep"))
                        Fechas[i] = Fechas[i].Replace("sep", "09").Replace("-", "/");
                    if (Fechas[i].Contains("oct"))
                        Fechas[i] = Fechas[i].Replace("oct", "10").Replace("-", "/");
                    if (Fechas[i].Contains("nov"))
                        Fechas[i] = Fechas[i].Replace("nov", "11").Replace("-", "/");
                    if (Fechas[i].Contains("dic"))
                        Fechas[i] = Fechas[i].Replace("dic", "12").Replace("-", "/");
                    //
                    string[] todo = Fechas[i].Split('/');
                    switch (todo[2])
                    {
                        case "10": todo[2] = "2010"; break;
                        case "11": todo[2] = "2011"; break;
                        case "12": todo[2] = "2012"; break;
                        case "13": todo[2] = "2013"; break;
                        case "14": todo[2] = "2014"; break;
                        case "15": todo[2] = "2015"; break;
                        case "16": todo[2] = "2016"; break;
                        case "17": todo[2] = "2017"; break;
                        case "18": todo[2] = "2018"; break;
                        case "19": todo[2] = "2019"; break;
                        case "20": todo[2] = "2020"; break;
                        default:
                            break;
                    }
                    Fechas[i] = todo[0] + "/" + todo[1] + "/" + todo[2];
                    Fechas[i] = Fechas[i].Replace("/", "-");
                }
                catch (Exception op)
                {

                }
            }
            return Fechas;
        }

        //METODO QUE OBTIENE UNA COLUMNA DE EXCEL Y LA ALMACENA EN UN STRING
        public String[] ObtenerColumna(String iniColumna, Excel.Worksheet SheetExcel, Excel.Application appExcel)
        {
            // Clipboard.Clear();

            String c = "";
            String r = "";

            if (iniColumna.Length == 2)
            {
                c = Convert.ToString(iniColumna[0]);
                r = Convert.ToString(iniColumna[1]);
            }
            else
            {
                if (iniColumna.Length == 3)
                {
                    c = Convert.ToString(iniColumna[0]) + Convert.ToString(iniColumna[1]);
                    r = Convert.ToString(iniColumna[2]);
                }
            }

            Excel.Range Rango;
            String[] Col;
            int row;

            Rango = SheetExcel.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            row = Convert.ToInt32(Rango.Row.ToString());
            Rango = SheetExcel.get_Range(iniColumna, c + row);
            Rango.Select();
            System.Threading.Thread.Sleep(500);
            Rango.Copy();
            //String datos = Clipboard.GetText();
            String datos = objNu4.clipboardObtenerTexto();
            datos = datos.Replace("\r", "");
            Col = datos.Split('\n');
            return Col;
        }

        //METODO QUE COPIA LAS PESTAÑA DE LOS ARCHIVOS Y LOS ACUMULA
        public Excel.Workbook AcumuladorDeArchivos(Excel.Workbooks Libros, List<string> ArchivosExcel)
        {
            Excel.Workbook ArchivoFinal = null;
            foreach (var archivo in ArchivosExcel)
            {
                //Abriendo archivos
                Excel.Workbook ArchivoTrabajoExcel = Libros.Open(archivo);
                //Cambiando nombre de pestaña por el nombre del archivo
                string nombrearchivo = ArchivoTrabajoExcel.Name.Replace(".xls", "");
                ((Excel.Worksheet)ArchivoTrabajoExcel.ActiveSheet).Name = nombrearchivo;
                //Copiando pestañas
                ((Excel.Worksheet)ArchivoTrabajoExcel.ActiveSheet).Copy(ArchivoFinal.Worksheets[ArchivoFinal.Sheets.Count]);
                //Cerrando archivo
                ArchivoTrabajoExcel.Close();
            }
            return ArchivoFinal;
        }

        //METODO QUE COPIA LAS PESTAÑA DE LOS ARCHIVOS Y LOS ACUMULA
        public Excel.Workbook AcumuladorDeArchivos(List<string> ArchivosExcel)
        {
            Excel.Application MiExcel = new Excel.Application();
            MiExcel.Visible = true;
            MiExcel.DisplayAlerts = false;
            MiExcel.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
            Excel.Workbooks books = MiExcel.Workbooks;
            Excel.Workbook ArchivoFinal = books.Add();
            foreach (var archivo in ArchivosExcel)
            {
                //Abriendo archivos
                Excel.Workbook ArchivoTrabajoExcel = books.Open(archivo);
                //Cambiando nombre de pestaña por el nombre del archivo
                string nombrearchivo = ArchivoTrabajoExcel.Name.Replace(".xls", "");
                ((Excel.Worksheet)ArchivoTrabajoExcel.ActiveSheet).Name = nombrearchivo;
                //Copiando pestañas
                ((Excel.Worksheet)ArchivoTrabajoExcel.ActiveSheet).Copy(ArchivoFinal.Worksheets[ArchivoFinal.Sheets.Count]);
                //Cerrando archivo
                ArchivoTrabajoExcel.Close();
            }
            return ArchivoFinal;
        }

        //COPIAR EL CONTENIDO DE CADA PESTAÑA DE TODOS LOS EXCELS EN UN ACUMULADO
        public Excel.Workbook AcumuladorDeArchivosEnUnaPestaña(List<string> ArchivosExcel, string PestañaAPegar)
        {
            Excel.Application MiExcel = new Excel.Application();
            MiExcel.Visible = true;
            MiExcel.DisplayAlerts = false;
            MiExcel.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
            Excel.Workbooks books = MiExcel.Workbooks;
            Excel.Workbook ArchivoFinal = books.Add();
            //Agregando pestaña Final
            Excel.Worksheet HOJAFINAL = (Excel.Worksheet)ArchivoFinal.Worksheets.Add();
            HOJAFINAL.Name = PestañaAPegar;
            //Abriendo cada archivo
            foreach (var archivo in ArchivosExcel)
            {
                //Abriendo archivos
                Excel.Workbook ArchivoTrabajoExcel = books.Open(archivo);
                Excel.Worksheet HOJA = (Excel.Worksheet)ArchivoTrabajoExcel.ActiveSheet;
                HOJA.Activate();
                HOJA.Cells.NumberFormat = "@";
                //Copiando datos
                string datos = CopiaDatosExcel(HOJA);
                ArchivoTrabajoExcel.Close();
                //Pegando datos
                HOJAFINAL.Activate();
                HOJAFINAL.Cells.NumberFormat = "@";
                PegarDatosExcel(HOJAFINAL, datos);
            }
            return ArchivoFinal;
        }

        //ACUMULA LOS DATOS DE TODAS LAS PESTAÑAS EN UNA SOLA LLAMADA: "ACUMULADO"
        public void AcumulaPestañas(Excel.Workbook ARCHIVO, string PestañaAPegar)
        {
            Excel.Workbook ArchivoTrabajoExcel = ARCHIVO;
            Excel.Worksheet HOJA = (Excel.Worksheet)ARCHIVO.Worksheets.Add();
            HOJA.Name = PestañaAPegar;
            foreach (Excel.Worksheet pestaña in ArchivoTrabajoExcel.Sheets)
            {
                pestaña.Activate();
                string datos = CopiaDatosExcel(pestaña);

                HOJA.Activate();
                PegarDatosExcel(HOJA, datos);
            }
        }

        //INSERTANDO TITULOS
        public void InsertarTitulos(Excel.Workbook ARCHIVO, Excel.Worksheet HOJA, string[] Titulos)
        {
            HOJA = (Excel.Worksheet)ARCHIVO.ActiveSheet;
            //---Insertando renglon
            Excel.Range Rango = HOJA.get_Range("A1");
            Rango.EntireRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
            //---Poniendo uno nuevos titulos
            int c = 0;
            string col = "";
            foreach (var item in Titulos)
            {
                c++;
                col = columnacorrespondiente(c);
                HOJA.Cells["1", col] = item;
            }
            //---Cambiando formato de los datos obtenidos
            Rango = HOJA.get_Range("A1", col + "1");
            Rango.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkBlue);
            Rango.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            Rango.EntireRow.Font.Bold = true;
        }

        //COPIA TODO LO DE LA PESTAÑA
        public string CopiaDatosExcel(Excel.Worksheet HOJA)
        {
            string datos = "";
            HOJA.Activate();
            Excel.Range Renglon = HOJA.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            int row = Convert.ToInt32(Renglon.Row.ToString());
            int col = Convert.ToInt32(Renglon.Column.ToString());
            string CellIni = "A" + 2;
            string CellFin = columnacorrespondiente(col) + row;
            Excel.Range Rango = HOJA.get_Range(CellIni, CellFin);
            Rango.Select();
            Thread.Sleep(200);
            Rango.Copy();
            Thread.Sleep(200);
            datos = objNu4.clipboardObtenerTexto();
            return datos;
        }

        //COPIA TODO LO DE LA PESTAÑA Y PASARLO A DATATABLE
        public DataTable CopiaDatosExcel_DataTable(Excel.Worksheet HOJA, int FilaTitulos)
        {
            DataTable Datos = new DataTable();
            try
            {
                HOJA.Activate();
                Excel.Range Renglon = HOJA.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                int row = Convert.ToInt32(Renglon.Row.ToString());
                int col = Convert.ToInt32(Renglon.Column.ToString());
                string CellIni = "A" + FilaTitulos;
                string CellFin = columnacorrespondiente(col) + row;
                Excel.Range Rango = HOJA.get_Range(CellIni, CellFin);
                Rango.Select();
                Thread.Sleep(200);
                Rango.Copy();
                Thread.Sleep(200);
                //Obtencion
                string strdatos = objNu4.clipboardObtenerTexto();
                string[] Renglones = strdatos.Split('\n');
                Renglones = Renglones.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                //Titulos
                string[] Titulos = Renglones[0].Split('\t');
                for (int T = 0; T < Titulos.Length; T++)
                    if (Titulos[T].ToString() != "" && Titulos[T].ToString() != " " && Titulos[T].ToString() != null)
                        Datos.Columns.Add(Titulos[T].ToString().Trim());
                    else
                        Datos.Columns.Add();
                //Datos
                List<string> renglones = Renglones.ToList();
                renglones.RemoveAt(0);
                Renglones = renglones.ToArray();
                for (int i = 0; i < Renglones.Length; i++)
                {
                    Datos.Rows.Add();
                    string[] Columnas = Renglones[i].Split('\t');
                    for (int j = 0; j < Columnas.Length; j++)
                        Datos.Rows[i][j] = Columnas[j];
                }
            }
            catch (Exception ex)
            {


            }
            return Datos;
        }

        //PEGAR DATOS EN EL ULTIMO RENGLON ENCONTRADO
        public void PegarDatosExcel_Celda(Excel.Worksheet HOJA, string Datos, string Celda)
        {
            Excel.Range CeldaPegar = HOJA.get_Range(Celda);
            CeldaPegar.Select();
            Thread.Sleep(200);
            objNu4.clipboardAlmacenaTexto(Datos);
            Thread.Sleep(200);
            CeldaPegar.PasteSpecial();
        }

        public void PegarDataTableAExcel_conLimite(Excel.Worksheet HOJA, DataTable DT, int indiceColumna, string celdapeg)
        {
            //Almecena datos
            string Datos = "";
            string[] columnaCEDIS = (from dts in DT.AsEnumerable()
                                     select dts.Field<string>(indiceColumna)).ToArray();
            Datos = string.Join(Environment.NewLine, columnaCEDIS);
            Datos = Datos.Replace("\r", "");
            //Pegar
            Excel.Range CeldaPegar = HOJA.get_Range(celdapeg);
            Thread.Sleep(200);
            objNu4.clipboardAlmacenaTexto(Datos);
            Thread.Sleep(200);
            CeldaPegar.PasteSpecial();
        }

        //PEGAR DATOS EN EL ULTIMO RENGLON ENCONTRADO
        public void PegarDatosExcel(Excel.Worksheet HOJA, string Datos)
        {
            Excel.Range Renglon = HOJA.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            int row = Convert.ToInt32(Renglon.Row.ToString()) + 1;
            Excel.Range CeldaPegar = HOJA.get_Range("A" + row);
            CeldaPegar.Select();
            Thread.Sleep(200);
            objNu4.clipboardAlmacenaTexto(Datos);
            Thread.Sleep(200);
            CeldaPegar.PasteSpecial();
        }

        //MARGINANDO CELDAS
        public void MarginandoCeldas(Excel.Workbook ARCHIVO, Excel.Worksheet HOJA)
        {
            HOJA.Select();
            HOJA.Activate();
            //Variables
            int RenglonTitulosDatos = 1;
            string ColIniStr = columnacorrespondiente(1);
            int RenFin = UltimoRenglon(HOJA);
            int ColFin = UltimaColumna(HOJA);
            string ColFinStr = columnacorrespondiente(ColFin);
            string CellIni = "A1";
            string CellFin = ColFinStr + RenFin.ToString();
            //Marginando
            Excel.Range Rango = HOJA.get_Range(CellIni, CellFin);
            //Rango.ReadingOrder = (int)Excel.Constants.xlContext;
            Rango.HorizontalAlignment = 3;
            Rango.VerticalAlignment = 2;
            Rango.EntireColumn.AutoFit();
            Rango.EntireRow.AutoFit();
            Excel.Borders border = Rango.Borders;
            border.LineStyle = Excel.XlLineStyle.xlContinuous;
            border.Weight = 1d;
        }

        //MARGINANDO CELDAS
        public void MarginandoCeldas2(Excel.Workbook ARCHIVO, Excel.Worksheet HOJA)
        {
            HOJA.Select();
            HOJA.Activate();
            //Variables
            int RenglonTitulosDatos = 1;
            string ColIniStr = columnacorrespondiente(1);
            int RenFin = objNu4Excel.UltimoRenglon(ARCHIVO, ColIniStr);
            int ColFin = objNu4Excel.UltimaColumna(ARCHIVO, RenglonTitulosDatos.ToString());
            string ColFinStr = columnacorrespondiente(ColFin);
            string CellIni = "A1";
            string CellFin = ColFinStr + RenFin.ToString();
            //Marginando
            Excel.Range Rango = HOJA.get_Range(CellIni, CellFin);
            Rango.HorizontalAlignment = 3;
            Rango.VerticalAlignment = 2;
            Rango.EntireColumn.AutoFit();
            Rango.EntireRow.AutoFit();
            Excel.Borders border = Rango.Borders;
            border.LineStyle = Excel.XlLineStyle.xlContinuous;
            border.Weight = 1d;
        }

        public void MoverPestaña(Excel.Workbook LIBRO, Excel.Worksheet HOJA, int LUGAR)
        {
            HOJA.Select();
            HOJA.Move(LIBRO.Sheets[LUGAR]);
        }

        public void MoverColumnas(Excel.Worksheet HOJA, string strColumnaUNOCortar, string strColumnaDOSCortar, string strColumnaPegar)
        {
            HOJA.get_Range(strColumnaUNOCortar + "1", strColumnaDOSCortar + "1").EntireColumn.Cut();
            HOJA.get_Range(strColumnaPegar + "1").EntireColumn.Insert(Excel.XlInsertShiftDirection.xlShiftToRight);
        }

        //Eliminando renglones inserbibles
        public void EliminaRegnlones(Excel.Application APPEXC, Excel.Workbook LIBRO, Excel.Worksheet HOJA, string[] Contenido)
        {
            Excel.Range Rango;
            string[] colAFac = LeerColumna(1, 1, APPEXC, LIBRO, HOJA);
            //Agregando opciones
            List<string> lista = new List<string>();
            lista = Contenido.ToList<string>();
            lista.Add("\r\n");
            lista.Add("\r");
            lista.Add("\n");
            int r = 1;
            foreach (var item in colAFac)
            {
                string Renglon = item.ToString();
                foreach (var cont in lista)
                {
                    if (Renglon.Contains(cont) || Renglon.StartsWith(cont) || Renglon.Equals("")) //|| Renglon.StartsWith(" ")
                    {
                        try
                        {
                            Rango = HOJA.get_Range("A" + r);
                            Rango.Select();
                            Rango.EntireRow.Delete();
                        }
                        catch
                        {
                            Rango = HOJA.get_Range("A" + r);
                            Rango.Select();
                            HOJA.Cells[r, "A"].EntireRow.Delete();
                        }
                        r--;
                        break;
                    }
                }
                r++;
            }
        }

        //REGRESA EN UN STRING TODA LA COLUMNA LEIDA DE EXCEL
        public string[] LeerColumna(int intRow, int intCol, Excel.Application APPEXC, Excel.Workbook LIBRO, Excel.Worksheet HOJA)
        {
            string[] DatosColumna = new string[0];
            int RenFin = UltimoRenglon(HOJA);
            string ColStr = columnacorrespondiente(intCol);
            string CellIni = ColStr + intRow.ToString();
            string CellFin = ColStr + RenFin.ToString();
            Excel.Range Rango = HOJA.get_Range(CellIni, CellFin);
            try
            {
                Rango.MergeCells = false;
            }
            catch
            {

            }
            Rango.Select();
            Rango.Copy();
            string portapapeles = objNu4.clipboardObtenerTexto();
            portapapeles = portapapeles.Replace("\r", "").TrimEnd('\n');
            if (portapapeles != null && portapapeles != "")
                DatosColumna = portapapeles.Split('\n');
            return DatosColumna;
        }

        //METODO QUE OBTIENE UNA COLUMNA DE EXCEL Y LA ALMACENA EN UN STRING
        public String[] ObtenerColumna(int iniColumnaX, int iniRenglonX, Excel.Worksheet SheetExcel)
        {
            SheetExcel.Activate();

            Excel.Range Rango;
            String[] Col;
            Rango = SheetExcel.get_Range(columnacorrespondiente(iniColumnaX) + iniRenglonX);
            int UltRow = Convert.ToInt32(SheetExcel.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing).Row.ToString());
            Rango = SheetExcel.get_Range(columnacorrespondiente(iniColumnaX) + iniRenglonX, columnacorrespondiente(iniColumnaX) + UltRow);
            Rango.Select();
            System.Threading.Thread.Sleep(200);
            Rango.Copy();
            //String datos = Clipboard.GetText();
            String datos = objNu4.clipboardObtenerTexto();
            System.Threading.Thread.Sleep(200);
            datos = datos.Replace("\r", "");
            Col = datos.Split('\n');
            return Col;
        }

        //TRAE EL ULTIMO RENGLON DEL EXCEL
        public int UltimoRenglon(Excel.Worksheet HOJA)
        {
            int row = 0;
            Excel.Range Renglon = HOJA.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            row = Convert.ToInt32(Renglon.Row.ToString());
            return row;
        }

        //TRAE EL ULTIMO DE RENGLON
        public int UltimaColumna(Excel.Worksheet HOJA)
        {
            int col = 0;
            Excel.Range Renglon = HOJA.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            col = Convert.ToInt32(Renglon.Column.ToString());
            return col;
        }

        //AGREGANDO PESTANIA
        public Excel.Worksheet AgregandoPestania(Excel.Workbook ARCHIVO, string NombrePestania)
        {
            Excel.Worksheet HOJA = (Excel.Worksheet)ARCHIVO.ActiveSheet;
            try
            {
                HOJA = (Excel.Worksheet)ARCHIVO.Worksheets.Add();
                HOJA.Name = NombrePestania;
            }
            catch
            {
                Console.WriteLine("ERROR al agregando la pestaña");
            }
            return HOJA;
        }

        //ELIMINANDO PESTANIAS EXTRAS
        public void EliminaPestaniasExtras(Excel.Workbook ARCHIVO)
        {
            foreach (Excel.Worksheet SheetExcel in ARCHIVO.Worksheets)
            {
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        if (SheetExcel.Name == "Hoja" + i)
                            SheetExcel.Delete();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        //DANDO FORMATO A COLUMNA
        public void DarFormatoColumna(Excel.Worksheet HOJA, int indcol, int formato)
        {
            string Columna = columnacorrespondiente(indcol);
            Excel.Range Rango = HOJA.get_Range(Columna + "1");
            Rango.EntireColumn.NumberFormat = formato;
        }

        //PEGANDO RESULTADOS A EXCEL PARA FACT-RECH
        public bool GuardandoArchivo(Excel.Workbook ARCHIVO, string RutaCarpeta, string Nombre, string NombreExtra)
        {
            bool exito = false;
            //Guardando Archivo
            if (!System.IO.Directory.Exists(RutaCarpeta))
                System.IO.Directory.CreateDirectory(RutaCarpeta);
            string Archivo = RutaCarpeta + Nombre + " " + NombreExtra + ".xls";
            if (File.Exists(Archivo))
            {
                Archivo = Archivo.Replace(".xls", "") + " " + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                Archivo += ".xls";
            }
            try
            {
                ARCHIVO.SaveAs(Archivo, Type.Missing, null, null, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                //tools.MessageShowOK_2("Archivo guardado correctamente en: " + RutaCarpeta + "\n\n\t" + Nombre + " " + NombreExtra);
            }
            catch (Exception ex)
            {
                //tools.MessageShowOK_2("Se cerro el archivo excel antes de poder guardar cambios.");
            }
            return exito;
        }

        //PEGANDO RESULTADOS A EXCEL PARA FACT-RECH
        public bool GuardandoArchivo_XLSB(Excel.Workbook ARCHIVO, string RutaCarpeta, string Nombre, string NombreExtra, string Formato)
        {
            bool exito = false;
            //Guardando Archivo
            if (!System.IO.Directory.Exists(RutaCarpeta))
                System.IO.Directory.CreateDirectory(RutaCarpeta);
            string Archivo = RutaCarpeta + Nombre + " " + NombreExtra + ".xlsb";
            if (File.Exists(Archivo))
            {
                Archivo = Archivo.Replace(".xlsb", "") + " " + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                Archivo += ".xlsb";
            }
            Excel.XlFileFormat FormatoEXCEL = Excel.XlFileFormat.xlExcel12;
            try
            {
                switch (Formato)
                {
                    case "xlsb":
                        FormatoEXCEL = Excel.XlFileFormat.xlExcel12;
                        break;
                    default:
                        break;
                }
                ARCHIVO.SaveAs(Archivo, FormatoEXCEL, null, null, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                //tools.MessageShowOK_2("Archivo guardado correctamente en: " + RutaCarpeta + "\n\n\t" + Nombre + " " + NombreExtra);
                return true;
            }
            catch (Exception ex)
            {
                //tools.MessageShowOK_2("Se cerro el archivo excel antes de poder guardar cambios.");
            }
            return exito;
        }

        //COLUMNA CORRESPONDIENTE
        public string columnacorrespondiente(int col)
        {
            string columnastr = "";
            switch (col)
            {
                case 1: columnastr = "A"; break;
                case 2: columnastr = "B"; break;
                case 3: columnastr = "C"; break;
                case 4: columnastr = "D"; break;
                case 5: columnastr = "E"; break;
                case 6: columnastr = "F"; break;
                case 7: columnastr = "G"; break;
                case 8: columnastr = "H"; break;
                case 9: columnastr = "I"; break;
                case 10: columnastr = "J"; break;
                case 11: columnastr = "K"; break;
                case 12: columnastr = "L"; break;
                case 13: columnastr = "M"; break;
                case 14: columnastr = "N"; break;
                case 15: columnastr = "O"; break;
                case 16: columnastr = "P"; break;
                case 17: columnastr = "Q"; break;
                case 18: columnastr = "R"; break;
                case 19: columnastr = "S"; break;
                case 20: columnastr = "T"; break;
                case 21: columnastr = "U"; break;
                case 22: columnastr = "V"; break;
                case 23: columnastr = "W"; break;
                case 24: columnastr = "X"; break;
                case 25: columnastr = "Y"; break;
                case 26: columnastr = "Z"; break;
                case 27: columnastr = "AA"; break;
                case 28: columnastr = "AB"; break;
                case 29: columnastr = "AC"; break;
                case 30: columnastr = "AD"; break;
                case 31: columnastr = "AE"; break;
                case 32: columnastr = "AF"; break;
                case 33: columnastr = "AG"; break;
                case 34: columnastr = "AH"; break;
                case 35: columnastr = "AI"; break;
                case 36: columnastr = "AJ"; break;
                case 37: columnastr = "AK"; break;
                case 38: columnastr = "AL"; break;
                case 39: columnastr = "AM"; break;
                case 40: columnastr = "AN"; break;
                case 41: columnastr = "AO"; break;
                case 42: columnastr = "AP"; break;
                case 43: columnastr = "AQ"; break;
                case 44: columnastr = "AR"; break;
                case 45: columnastr = "AS"; break;
                case 46: columnastr = "AT"; break;
                case 47: columnastr = "AU"; break;
                case 48: columnastr = "AV"; break;
                case 49: columnastr = "AW"; break;
                case 50: columnastr = "AX"; break;
                case 51: columnastr = "AY"; break;
                case 52: columnastr = "AZ"; break;
                case 53: columnastr = "BA"; break;
                case 54: columnastr = "BB"; break;
                case 55: columnastr = "BC"; break;
                case 56: columnastr = "BD"; break;
                case 57: columnastr = "BE"; break;
                case 58: columnastr = "BF"; break;
                case 59: columnastr = "BG"; break;
                case 60: columnastr = "BH"; break;
                case 61: columnastr = "BI"; break;
                case 62: columnastr = "BJ"; break;
                case 63: columnastr = "BK"; break;
                case 64: columnastr = "BL"; break;
                case 65: columnastr = "BM"; break;
                case 66: columnastr = "BN"; break;
                case 67: columnastr = "BO"; break;
                case 68: columnastr = "BP"; break;
                case 69: columnastr = "BQ"; break;
                case 70: columnastr = "BR"; break;
                case 71: columnastr = "BS"; break;
                case 72: columnastr = "BT"; break;
                case 73: columnastr = "BU"; break;
                default:
                    break;
            }
            return columnastr;
        }

        public bool ExportarConsultaExcel(DataTable dt, String nombreHoja, String nomArch, int NumColor)
        {
            try
            {
                ///Variables
                String datosDTExportar = "", encabezados = "";
                Excel.Application MiExcel = objNu4Excel.ObtenerObjetoExcel();
                MiExcel.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlNormal;
                objNu4Excel.InstanciaExcelVisible(MiExcel);
                Excel.Workbook ArchivoTrabajoExcel = objNu4Excel.AbrirArchivoNuevo(MiExcel);
                objNu4Excel.ActivarMensajesAlertas(MiExcel, 0);
                Excel.Worksheet HojaExcel = objNu4Excel.ActivarPestaniaExcel(1, MiExcel, ArchivoTrabajoExcel);
                ///Inicio de encabezados
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    encabezados += dt.Columns[i].ColumnName + "\t";
                }
                Dispatcher.Invoke(((Action)(() => datosDTExportar = string.Join(Environment.NewLine, dt.Rows.OfType<DataRow>().Select(x => string.Join("\t", x.ItemArray))))));
                objNu4Excel.PonerNombreHoja(HojaExcel, nombreHoja);
                objNu4.clipboardAlmacenaTexto(encabezados);
                objNu4Excel.PegarPortaPapelesRango("A1", HojaExcel);
                Dispatcher.Invoke(((Action)(() => System.Windows.Forms.Clipboard.Clear())));
                ///Datos
                int renTitulos = FilaTitulos_6(HojaExcel, 50, 50);
                String columnaInicial = objNu4Excel.ColumnaCorrespondiente(1);
                int numeroColumna = objNu4Excel.UltimaColumna(ArchivoTrabajoExcel, renTitulos.ToString());
                string columnaFinal = objNu4Excel.ColumnaCorrespondiente(numeroColumna);
                String renglonInicial = renTitulos.ToString();
                String renglonFinal = objNu4Excel.UltimoRenglon(ArchivoTrabajoExcel, columnaFinal).ToString();
                ///Lectura de datos en Excel
                Excel.Range rango = HojaExcel.get_Range(columnaInicial + renglonInicial, columnaFinal + renglonFinal);
                rango.Font.Size = "10";
                rango.Font.Name = "Calibri";
                rango.Font.Bold = true;
                rango.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                rango.Interior.Color = NumColor;
                objNu4.clipboardAlmacenaTexto(datosDTExportar);
                objNu4Excel.PegarPortaPapelesRango("A2", HojaExcel);
                Dispatcher.Invoke(((Action)(() => System.Windows.Forms.Clipboard.Clear())));
                ///Marginando celdas
                MarginandoCeldas(ArchivoTrabajoExcel, HojaExcel);
                ///Guardar
                string rutaGuardar = "";
                dynamic dialog = (dynamic)null;
                Dispatcher.Invoke(((Action)(() => dialog = new System.Windows.Forms.SaveFileDialog())));
                String nombrearch = objNu4.GeneraNombreArchivo(nomArch + "_", "xlsx");
                Dispatcher.Invoke(((Action)(() => dialog.Title = "Ruta del archivo a GUARDAR...")));
                Dispatcher.Invoke(((Action)(() => dialog.FileName = nombrearch)));
                Dispatcher.Invoke(((Action)(() => dialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm;*.xlsb;")));
                Dispatcher.Invoke(((Action)(() => dialog.ShowDialog())));
                string rutaGuarda = dialog.FileName.ToString();
                try
                {
                    if (!string.IsNullOrEmpty(rutaGuarda) && rutaGuarda != nombrearch)
                    {
                        rutaGuardar = dialog.FileName.ToString();
                        objNu4Excel.ArchivoGuardarComo(ArchivoTrabajoExcel, rutaGuardar);
                        ///---tools.MessageShowOK_2("Archivo guardado correctamente en: " + rutaGuardar);
                    }
                    else
                    {
                        ///---tools.MessageShowOK_2("No se guardo archivo");
                    }
                    tools.MessageShowOK_2("Listo!", "OK");
                }
                catch (Exception ex)
                {
                    tools.MessageShowOK_2("Error no se guardo archivo " + ex.Message.ToString(), "ERROR");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int FilaTitulos_6(Excel.Worksheet HOJA, int maxfilas, int maxcols)
        {
            int rengloncontitulos = 0;

            //A1
            for (int i = 1; i <= maxfilas; i++)
            {
                string cellIni = "A" + i;//objNuExcel.ColumnaCorrespondiente(i) + i;
                string cellFin = objNu4Excel.ColumnaCorrespondiente(maxcols) + i; //objNuExcel.ColumnaCorrespondiente(i + 2) + i;
                Excel.Range rango = HOJA.get_Range(cellIni, cellFin);
                rango.Select();
                rango.Copy();
                string[] DatosEnRenglon = objNu4.clipboardObtenerTexto().Split('\t');

                int bnd = 0;
                for (int x = 0; x < DatosEnRenglon.Length; x++)
                {
                    String dato = DatosEnRenglon[x];
                    dato = objNu4.Modifica(dato);

                    if (dato != "" && DatosEnRenglon[x] != " " && dato != null && dato != "\r\n" && dato != "\n" && dato != "\r" && dato != "\t")
                    {
                        bnd++;
                    }
                    if (bnd > 2)
                    {
                        break;
                    }
                }
                if (bnd > 2)
                {
                    rengloncontitulos = i;
                    break;
                }
            }
            return rengloncontitulos;
        }

        #endregion

        #region PORTALES


        //OBTENER TABLA TABULADA EN UN STRNG por INNERHTML
        public string ObtenerTablaHTML_ID(SHDocVw.InternetExplorer MiIE, string ID)
        {
            string TABLA = "";
            var Table = MiIE.Document.getElementByID(ID);
            string INNER = Table.innerhtml.ToString();
            ///Quitar elemenetnso HTML5
            string sinHTML = RemplazaHtml(INNER);
            ///Otros
            string A = sinHTML.Replace(INNER, "").Replace("\r\n\r\n", "•").Replace("\r\n", "♣").Replace("•♣", "♥").Replace("•", "♣").Replace("♣♣", "♣");
            string B = A.Replace(" ♣ ", "♣").Replace(" ♣", "♣").Replace("♣♣♣", "♥").Replace(" ♣ ", " ♣").Replace(" ♣", "•♣");
            string C = B.Replace("♣♣ ", "♣");
            string D = C.Replace("♥", "\r");
            string E = D.Replace("♣•", "•");
            TABLA = E.Replace("♣", "\t");
            return TABLA;
        }

        //OBTENER TABLA TABULADA EN UN STRNG por INNERHTML
        public string ObtenerTablaHTML_ClassName(SHDocVw.InternetExplorer MiIE, string ClassName, int Index)
        {
            string TABLA = "";
            var Table = MiIE.Document.getElementsByClassName(ClassName)[Index];
            string INNER = Table.innerhtml.ToString();
            ///Quitar elemenetnso HTML5
            string sinHTML = RemplazaHtml(INNER);
            ///Otros
            string A = sinHTML.Replace(INNER, "").Replace("\r\n\r\n", "•").Replace("\r\n", "♣").Replace("•♣", "♥").Replace("•", "♣").Replace("♣♣", "♣");
            string B = A.Replace(" ♣ ", "♣").Replace(" ♣", "♣").Replace("♣♣♣", "♥").Replace(" ♣ ", " ♣").Replace(" ♣", "•♣");
            string C = B.Replace("♣♣ ", "♣");
            string D = C.Replace("♥", "\r");
            string E = D.Replace("♣•", "•");
            TABLA = E.Replace("♣", "\t");
            return TABLA;
        }

        //QUITAR HTML DE TEXTO
        public static string RemplazaHtml(string strCadHtml)
        {
            string strCadLimpia = "";
            Regex expRegular = new Regex("<[^>]*>");
            strCadLimpia = expRegular.Replace(strCadHtml, " ");
            return strCadLimpia;
        }

        //REGRESA UN TRU SI ES QUE LA VENTANA BUSCADA SE ENCUENTRA EN LA LISTA DE VENTANAS DE IE
        public bool BuscaVentana(string Titulo)
        {
            bool continuar = false;
            Thread.Sleep(2500);
            string[] tituloPagActual = VentanasIE();
            for (int i = 0; i < tituloPagActual.Length; i++)
            {
                if (objNu4.Modifica(tituloPagActual[i]) != "" && tituloPagActual[i] != null)
                {
                    if (objNu4.Modifica(tituloPagActual[i]).Contains(objNu4.Modifica(Titulo)))
                    {
                        continuar = true;
                        break;
                    }
                }
            }
            return continuar;
        }

        //ASIGNA AL OBJETO DE MIIE LA PESTAÑA CON EL TITULO ENONTRADO
        public SHDocVw.InternetExplorer AsignaVentana(string Titulo)
        {
            SHDocVw.ShellWindows shellwindows = new SHDocVw.ShellWindows();
            foreach (var window in shellwindows)
            {
                SHDocVw.InternetExplorer foundbrowser = ((SHDocVw.InternetExplorer)window);
                if (foundbrowser.Name.ToString().EndsWith("Internet Explorer"))
                {
                    string TituloA = "";
                    try
                    {
                        TituloA = objNu4.Modifica(foundbrowser.Document.title.ToString());
                    }
                    catch (Exception)
                    {
                        TituloA = objNu4.Modifica(foundbrowser.Document.IHTMLDocument2_title.ToString());
                    }
                    if (objNu4.Modifica(TituloA).Contains(objNu4.Modifica(Titulo)))   //IDENTIFICAR QUE VENTANA SE ABRIO PARA REALIZAR SUS PASOS SISGUIENTES
                    {
                        return foundbrowser;
                    }
                }
            }
            SHDocVw.InternetExplorer NoEncontre = new SHDocVw.InternetExplorer();
            return NoEncontre;
        }

        //REGRESA LA LISTA DE VEBTANAS DE IE
        public string[] VentanasIE()
        {
            string[] ListaVentanas = new string[10];
            SHDocVw.ShellWindows shellwindows = new SHDocVw.ShellWindows();
            int count = 0;
            foreach (var window in shellwindows)
            {
                SHDocVw.InternetExplorer foundbrowser = ((SHDocVw.InternetExplorer)window);
                if (foundbrowser.Name.ToString().EndsWith("Internet Explorer"))
                {
                    try
                    {
                        ListaVentanas[count] = foundbrowser.Document.title.ToString();
                    }
                    catch (Exception)
                    {
                        ListaVentanas[count] = "Error";
                    }
                    count++;
                }
            }
            ListaVentanas = ListaVentanas.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return ListaVentanas;
        }

        //WAIT: Espera el elemento web, si no esta por 20 intentos/segundos
        public bool wait(SHDocVw.InternetExplorer MiIE, string Tipo, string Elemento)
        {
            bool seguir = false;
            int intentos = 0;
            int FoundElements = 0;
            if (Tipo == "TagName")
            {
                do
                {
                    try
                    {
                        intentos++;
                        if (intentos == 10)
                            MiIE.Refresh2();
                        if (intentos == 20)
                        {
                            seguir = false;
                            break;
                        }
                        FoundElements = MiIE.Document.getElementsByTagName(Elemento).Length;
                        if (FoundElements >= 1)
                        {
                            seguir = true;
                            break;
                        }
                    }
                    catch (Exception)
                    {

                    }
                    Thread.Sleep(5000);
                } while (FoundElements == 0);
            }

            if (Tipo == "ClassName")
            {
                do
                {
                    try
                    {
                        intentos++;
                        if (intentos == 10)
                            MiIE.Refresh2();
                        if (intentos == 20)
                        {
                            seguir = false;
                            break;
                        }
                        FoundElements = MiIE.Document.getElementsByClassName(Elemento).Length;
                        if (FoundElements >= 1)
                        {
                            seguir = true;
                            break;
                        }
                    }
                    catch (Exception)
                    {

                    }
                    Thread.Sleep(5000);
                } while (FoundElements == 0);
            }
            if (Tipo == "Name")
            {
                do
                {
                    try
                    {
                        intentos++;
                        if (intentos == 10)
                            MiIE.Refresh2();
                        if (intentos == 20)
                        {
                            seguir = false;
                            break;
                        }
                        FoundElements = MiIE.Document.getElementsByName(Elemento).Length;
                        if (FoundElements >= 1)
                        {
                            seguir = true;
                            break;
                        }
                    }
                    catch (Exception)
                    {

                    }
                    Thread.Sleep(5000);
                } while (FoundElements == 0);
            }
            if (Tipo == "A")
            {
                do
                {
                    try
                    {
                        intentos++;
                        if (intentos == 10)
                            MiIE.Refresh2();
                        if (intentos == 20)
                        {
                            seguir = false;
                            break;
                        }
                        var Asss = MiIE.Document.getElementsByTagName("a");
                        for (int i = 0; i < Asss.Length; i++)
                            if (Asss[i].innerhtml.Contains(Elemento))
                                FoundElements = 1;
                        if (FoundElements >= 1)
                        {
                            seguir = true;
                            break;
                        }
                    }
                    catch (Exception)
                    {

                    }
                    Thread.Sleep(5000);
                } while (FoundElements == 0);
            }
            return seguir;
        }

        //KILL INSTANCIAS
        public void killinstans(String proceso)
        {
            Process[] myProcesses;
            myProcesses = Process.GetProcesses();

            foreach (Process myProcess in myProcesses)
            {
                if (myProcess.ProcessName.Contains(proceso))
                {
                    myProcess.Kill();
                }
            }
        }

        //OBTENER NOMBRE DE LOS ARCHIVOS QUE SE ENCUENTRAN EN DESCARGAS
        public string[] TotalArchivosDownloads(string rutaDescarga)
        {
            int i = 0;
            string[] xlsAux = Directory.GetFiles(rutaDescarga, "*.xls");
            foreach (string FileName in xlsAux)
            {
                xlsAux[i] = FileName;
                i++;
            }
            return (xlsAux);
        }

        //OBTIENE EL NOMBRE DEL NUEVO ARCHIVO EN A CARPETA DE DESCARGAS
        public string NombreArchivoNuevo(string[] FilesBefore, string[] FilesAfter)
        {
            string Archivo = "";
            foreach (string xl in FilesBefore)
                FilesAfter = Array.FindAll(FilesAfter, s => !s.Equals(xl));
            Archivo = FilesAfter[0].ToString();
            return (Archivo);
        }
        #endregion

        #region ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬ MOVEMOUSE Y HANDELS ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        //METODO PARA ATIVAR UNA VENTANA : DANIEL SANCHEZ

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public void ActivaVentana(string proceso, string _proceso, string title)
        {
            var procesos = Process.GetProcesses();
            var prc = Process.GetProcessesByName(proceso);
            Process[] localByName = Process.GetProcessesByName(_proceso);
            for (int i = 0; i < localByName.Length; i++)
            {
                if (localByName[i].MainWindowTitle.ToUpper().Contains(title.ToUpper()))
                {
                    SetForegroundWindow(localByName[i].MainWindowHandle);
                }
                if (localByName[i].MainWindowTitle.ToUpper().Contains(title.ToUpper()))
                {
                    SetForegroundWindow(localByName[i].MainWindowHandle);
                }
            }
        }

        //DAR CLICK EN UN PIXEL : JORGE NUÑEZ
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwflags, int dx, int dy, int cbuttons, int dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out Point lpPoint);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        public void DarClick(string Tipo, int X, int Y)
        {
            if (Tipo == "L" || Tipo == "I")
            {
                if (X == 0 && Y == 0)
                {
                    X = System.Windows.Forms.Cursor.Position.X;
                    Y = System.Windows.Forms.Cursor.Position.Y;
                }
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
                //System.Windows.Forms.SendKeys.SendWait("{ESC}");
            }
            if (Tipo == "R" || Tipo == "D")
            {
                if (X == 0 && Y == 0)
                {
                    X = System.Windows.Forms.Cursor.Position.X;
                    Y = System.Windows.Forms.Cursor.Position.Y;
                }
                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
                //System.Windows.Forms.SendKeys.SendWait("{ESC}");
            }
        }

        #endregion


    }
}
