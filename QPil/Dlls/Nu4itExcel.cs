using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
//LIBRERIAS NUEVAS
using System.Windows.Forms;
using System.IO;
using Office = Microsoft.Office.Core;
using VBIDE = Microsoft.Vbe.Interop;
using Nu4it;
using nu4itExcel;

namespace QPil.Dlls
{
    class Nu4itExcel
    {
        Dlls.Nu4it objNu4it = new Dlls.Nu4it();

        Excel.Worksheet sheetExcel;
        Excel.Range rangoExcel, rngUltimo, colUltimo, unRango;

        const int SI = 1;
        const int NO = 0;

        #region ModulosTiempoExcel

        /*
          Fernando Rivelino Ortiz Martinez 
          Nu4itAutomation
          Funcion que regresa la cantidad de dias que han pasado desde 30 de diciembre de 1899 
          a la fecha que es pasada como parmetro en enteros del anio, mes y dia 
        */
        public int CalculaDifDias(int anio, int mes, int dia)
        {
            int diasTranscurridos;
            DateTime FechaIni = new DateTime(1899, 12, 30);
            DateTime fechafin = new DateTime(anio, mes, dia);
            diasTranscurridos = (fechafin - FechaIni).Days;
            return (diasTranscurridos);
        }

        public double Seconds()
        {
            System.TimeSpan st = System.DateTime.Now.Subtract(System.DateTime.Today);
            return st.Duration().TotalMilliseconds / 1000;
        }

        #endregion

        #region ModulosObtenerContenidoCeldas

        public String ContenidoCelda(Excel.Application appExcel, int row, int col)
        {
            String valor;

            var valcelda = appExcel.Application.ActiveWorkbook.ActiveSheet.Cells[row, col].Value;
            valor = Convert.ToString(valcelda);

            return valor;
        }

        public String ContenidoCelda(Excel.Application appExcel, String Celda)
        {
            String valor;

            var valcelda = appExcel.Application.ActiveWorkbook.ActiveSheet.Range[Celda].Value;
            valor = Convert.ToString(valcelda);

            return valor;
        }

        public String TextoCelda(Excel.Application appExcel, int row, int col)
        {
            String valor;

            var valcelda = appExcel.Application.ActiveWorkbook.ActiveSheet.Cells[row, col].Text;
            valor = Convert.ToString(valcelda);

            return valor;
        }

        public String TextoCelda(Excel.Application appExcel, String Celda)
        {
            String valor;

            var valcelda = appExcel.Application.ActiveWorkbook.ActiveSheet.Range[Celda].Text;
            valor = Convert.ToString(valcelda);

            return valor;
        }

        public String LeerContenidoCelda(Excel.Worksheet Pestania, int row, int col)
        {
            string valor;
            var valcelda = Pestania.Cells[row, col].Value;
            valor = Convert.ToString(valcelda);

            return valor;
        }

        public String LeerContenidoCelda(Excel.Worksheet Pestania, string celda)
        {
            string valor;
            var valcelda = Pestania.Range[celda].Value;
            valor = Convert.ToString(valcelda);

            return valor;
        }

        public String LeerTextoCelda(Excel.Worksheet Pestania, int row, int col)
        {
            string valor;
            var valcelda = Pestania.Cells[row, col].Text;
            valor = Convert.ToString(valcelda);

            return valor;
        }

        public String LeerTextoCelda(Excel.Worksheet Pestania, string celda)
        {
            string valor;
            var valcelda = Pestania.Range[celda].Text;
            valor = Convert.ToString(valcelda);

            return valor;
        }

        public Int64 ObtenCveColorCelda(string Celda, Excel.Worksheet Pestania)
        {
            Int64 ClaveColor;
            unRango = Pestania.get_Range(Celda);
            ClaveColor = Convert.ToInt64(unRango.Cells.Interior.Color);
            return ClaveColor;
        }

        //Procedimiento que lee una columna de excel y regresa su contenido en un arreglo
        public string[] LeerColumna(int renglonIni, int columnaIni, Excel.Application AppExcel, Excel.Workbook Archivo, Excel.Worksheet Pestania)
        {
            string rngCompleto, elemento, primero, ult, col;
            int i, j, NumRow, ultimo;
            char[] delimiterChars = { '\r', '\n' };

            Excel.Range Ran;
            col = ColumnaCorrespondiente(columnaIni);//Obtiene la letra de la primera columna a seleccionar
            ultimo = UltimoRenglon(Archivo, col);
            primero = col + renglonIni;
            ult = col + ultimo;
            Ran = Pestania.get_Range(primero, ult);//Define Rango
            Ran.Select();//Lo selecciona
            NumRow = AppExcel.Selection.Rows.Count; //Se obtuvo el numero de filas seleccionadas.
            string[] Datos1, Datos2 = new string[NumRow];
            if (ultimo >= renglonIni)
            {
                Ran.Copy();
                //rngCompleto = Clipboard.GetText();//Se pega como un string en Clipboard
                rngCompleto = objNu4it.clipboardObtenerTexto();
                Datos1 = rngCompleto.Split(delimiterChars);
                i = 0;
                for (j = 0; j < Datos1.Length - 1; j++)//debe ser -1 por que por el formato de los saltos quedan los ultimos 2//elementos del arreglo Datos1 vacios 
                {
                    if (j % 2 == 0 || j == 0)//filtra los elementos en posicion de nums nones que estan vacios
                    {
                        elemento = Convert.ToString(Datos1[j]);
                        Datos2[i] = elemento;//los añade a otro arreglo que será el devuelto
                        i++;
                    }
                }
            }
            else
            {
                for (i = 0; i < NumRow; i++) { Datos2[i] = ""; }
            }
            return (Datos2);
        }

        #endregion

        #region ModulosManipularObjetoExcel

        public Excel.Application ObtenerObjetoExcel()
        {
            Excel.Application appExcel;
            try
            {
                appExcel = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            }
            catch
            {
                appExcel = new Excel.Application();
            }
            return (appExcel);
        }

        public void DestruirObjetoExcel(Excel.Application appExcel)
        {
            appExcel.Quit();
        }

        public void ActivarRecalculo(Excel.Application appExcel, int TipoRecalculo)
        {
            if (TipoRecalculo == 0) { appExcel.Calculation = Excel.XlCalculation.xlCalculationManual; }
            else { appExcel.Calculation = Excel.XlCalculation.xlCalculationAutomatic; }
        }

        public void ActivarMensajesAlertas(Excel.Application appExcel, int Activar)
        {
            if (Activar == SI) { appExcel.DisplayAlerts = true; }
            else { appExcel.DisplayAlerts = false; }
        }

        public void InstanciaExcelVisible(Excel.Application appExcel)
        {
            appExcel.Visible = true;
        }

        #endregion

        #region ModulosManipularArchivosExcel

        //by CARLOS ALVARADO LUIS
        public Excel.Workbook AbrirArchivo(string RutaNombreArchivo, Excel.Application appExcel, bool readOnly = false, bool reparar = false)
        {
            Excel.Workbook Archivo;
            Excel.Workbooks books = appExcel.Workbooks;
            if (reparar)
            {
                Archivo = books.Open(RutaNombreArchivo, 0, readOnly, Missing.Value, Missing.Value, Missing.Value, true, Missing.Value, Missing.Value, false, false, Missing.Value, false, Missing.Value, Excel.XlCorruptLoad.xlRepairFile);
            }
            else
            {
                Archivo = books.Open(RutaNombreArchivo, 0, readOnly, Missing.Value, Missing.Value, Missing.Value, true, Missing.Value, Missing.Value, false, false, Missing.Value, false, Missing.Value, Excel.XlCorruptLoad.xlNormalLoad);
            }
            return (Archivo);
        }

        public Excel.Workbook AbrirArchivoNuevo(Excel.Application appExcel)
        {
            Excel.Workbook UnArchivo = null;

            Excel.Workbooks books = appExcel.Workbooks;
            UnArchivo = books.Add(Missing.Value);

            return UnArchivo;
        }

        public int CantidadArchivosAbiertos(Excel.Application appExcel)
        {
            int Cuantos;
            Cuantos = appExcel.Workbooks.Count;
            return (Cuantos);
        }

        public int ContarArchivoAbiertosNombre(Excel.Application appExcel, string NomArchivoBuscar)
        {
            int cuantos, CantEnc, indArchAbierto;
            string nombreArchivo, nombreArchivoModificado;
            cuantos = appExcel.Workbooks.Count;
            CantEnc = 0;
            if (cuantos > 0)
            {
                for (indArchAbierto = 1; indArchAbierto <= cuantos; indArchAbierto++)
                {
                    nombreArchivo = appExcel.Workbooks[indArchAbierto].Name;
                    nombreArchivoModificado = objNu4it.Modifica(nombreArchivo);
                    if (nombreArchivoModificado.IndexOf(NomArchivoBuscar) >= 0) { CantEnc++; }
                }
            }
            return (CantEnc);
        }

        public int PosArchivoAbiertoNombre(Excel.Application appExcel, string NomArchivoBuscar)
        {
            int cuantos, indArchAbierto, PosArchivoAbiertoNombreBuscado;
            string nombreArchivo, nombreArchivoModificado;
            cuantos = appExcel.Workbooks.Count;
            PosArchivoAbiertoNombreBuscado = 0;
            if (cuantos > 0)
            {
                for (indArchAbierto = 1; indArchAbierto <= cuantos; indArchAbierto++)
                {
                    nombreArchivo = appExcel.Workbooks[indArchAbierto].Name;
                    nombreArchivoModificado = objNu4it.Modifica(nombreArchivo);
                    if (nombreArchivoModificado.IndexOf(NomArchivoBuscar) >= 0) { PosArchivoAbiertoNombreBuscado = indArchAbierto; }
                }
            }
            return (PosArchivoAbiertoNombreBuscado);
        }

        public string NombreArchivoAbiertoPos(Excel.Application appExcel, int pos)
        {
            string NomArchivo = "";
            int Cuantos;
            Cuantos = appExcel.Workbooks.Count;
            if (pos > 0 && pos <= Cuantos) { NomArchivo = appExcel.Workbooks[pos].Name; }
            else NomArchivo = "ERROR";
            return (NomArchivo);
        }

        public Excel.Workbook ObtenerArchivoAbierto(Excel.Application appExcel, int pos)
        {
            Excel.Workbook UnArchivo;
            UnArchivo = null;
            int Cuantos;
            Cuantos = appExcel.Workbooks.Count;
            if (pos > 0 && pos <= Cuantos) { UnArchivo = appExcel.Workbooks[pos]; }
            return (UnArchivo);
        }

        public void ActivarArchivo(Excel.Workbook Archivo)
        {
            Archivo.Activate();
        }

        public void ArchivoGuardar(Excel.Workbook Archivo)
        {
            Archivo.Save();
        }

        public void ArchivoGuardarComo(Excel.Workbook Archivo, string RutaNombreArchivo)
        {
            Archivo.SaveAs(RutaNombreArchivo);
        }

        public void CerrarArchivo(Excel.Workbook Archivo)
        {
            Archivo.Close();
        }

        #endregion

        #region ModulosManipularHojasPestaniasExcel

        /*Fernando Rivelino Ortiz Martínez
        //Función que regresa la posición en la que se encuentra la hoja (namehoja) en el archvio de excel (Libro) pasados por parametro
        //En caso de que no exista una hoja con ese nombre regresa 0=NO (en Excel las posiciones se cuentan desde 1)*/
        public int HojaTrabajoSolicitada(Excel._Workbook Libro, string namehoja)
        {
            int pagsol = 0, pos;
            string nombreencontrado = "";
            string[] NombreHojas = new string[10000];
            pos = 0;
            foreach (Microsoft.Office.Interop.Excel.Worksheet hojas in Libro.Sheets)
            {
                NombreHojas[pos] = hojas.Name;
                nombreencontrado = NombreHojas[pos];
                nombreencontrado = objNu4it.Modifica(nombreencontrado);
                if (nombreencontrado == namehoja) { pagsol = pos + 1; }
                pos++;
            }
            return (pagsol);
        }

        public int HojaTrabajoSolicitada(Excel._Workbook Libro, string namehoja, int modifica)
        {
            int pagsol = 0, pos;
            string nombreencontrado = "";
            string[] NombreHojas = new string[10000];
            pos = 0;
            foreach (Microsoft.Office.Interop.Excel.Worksheet hojas in Libro.Sheets)
            {
                NombreHojas[pos] = hojas.Name;
                nombreencontrado = NombreHojas[pos];
                if (modifica == SI) { nombreencontrado = objNu4it.Modifica(nombreencontrado); }
                if (nombreencontrado == namehoja) { pagsol = pos + 1; }
                pos++;
            }
            return (pagsol);
        }

        public Excel.Worksheet ActivarPestaniaExcel(int posPestania, Excel.Application appExcel, Excel.Workbook Archivo)
        {
            Excel.Worksheet Pestania;
            //((Excel.Worksheet)this.MiExcel.ActiveWorkbook.Sheets[continuar]).Select();
            //appExcel.ActiveWorkbook.Sheets[posPestania].Select();
            Archivo.Sheets[posPestania].Select();
            Pestania = (Excel.Worksheet)Archivo.ActiveSheet;
            return (Pestania);
        }

        public Excel.Worksheet CrearNuevaHoja(Excel.Workbook UnArchivo)
        {
            Excel.Worksheet hojaNueva;
            hojaNueva = (Excel.Worksheet)UnArchivo.Worksheets.Add();
            return (hojaNueva);
        }

        public void PonerNombreHoja(Excel.Worksheet Pestania, string Nombre)
        {
            Pestania.Name = Nombre;
        }

        public string NombreHojaEn(int pos, Excel.Workbook Archivo)
        {
            string nom = "ERROR_POS_HOJA";
            int posHoja = 1;
            if (pos > 0)
            {
                foreach (Microsoft.Office.Interop.Excel.Worksheet hojas in Archivo.Sheets)
                {
                    if (posHoja == pos) { nom = hojas.Name; }
                    posHoja++;
                }
            }
            return nom;
        }

        public int CantidadHojas(Excel.Workbook Archivo)
        {
            int TotHojas = 0;
            foreach (Microsoft.Office.Interop.Excel.Worksheet hojas in Archivo.Sheets)
            {
                TotHojas++;
            }
            return TotHojas;
        }

        public void BorrarHojaDeArchivo(Excel.Worksheet Pestania)
        {
            Pestania.Delete();
        }

        //▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬//
        //    FUNCION QUE OBTIENE LA MACRO DE UN ARCHIVO DE TEXTOY LA EJECUTA EN EL ARCHIVO DE TRABAJO DE EXCEL
        //                                   JORGE NÚÑEZ     -   24/NOV/16
        // Recibe el libro de excel donde esta la pestaña a copiar, nombre de pestaña, el archivo de excel donde 
        // se va a pegar, y el lugar donde se va a pegar la pestaña, indicando el numero de la pestaña en donde se quiere pegar (Antes/Despues).
        //Ejemplo:
        //      CopiarPestania(MiExcel, ATE_Nuevo, "Hoja1", ATE_Descargado, "Final", 0);
        //      CopiarPestania(MiExcel, ATE_Nuevo, "Hoja1", ATE_Descargado, "Antes", 1);
        //      CopiarPestania(MiExcel, ATE_Nuevo, "Hoja1", ATE_Descargado, "Despues", 1);
        //▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬//

        public void CopiarPestania(Excel.Application appExcel, Excel.Workbook ArchivoACopiar, int PestaniaCopiar, Excel.Workbook ArchivoAPegar, string AntesDespuesFinal, int LugarPestaniaPegar)
        {
            ((Excel.Worksheet)appExcel.ActiveWorkbook.Sheets[PestaniaCopiar]).Select();
            Excel.Worksheet HOJA = (Excel.Worksheet)ArchivoACopiar.ActiveSheet;
            if (AntesDespuesFinal == "Antes")
            {
                ArchivoACopiar.Sheets.Copy(Missing.Value, ArchivoAPegar.Sheets[LugarPestaniaPegar]);
            }
            if (AntesDespuesFinal == "Despues")
            {
                ArchivoACopiar.Sheets.Copy(ArchivoAPegar.Sheets[LugarPestaniaPegar], Missing.Value);
            }
            if (AntesDespuesFinal == "Final")
            {
                ArchivoACopiar.Sheets.Copy(Missing.Value, ArchivoAPegar.Sheets[ArchivoAPegar.Sheets.Count]);
            }
        }


        public void MostrarColumnasOcultas(Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range("A1", "XFD1048575");
            unRango.Select();
            unRango.EntireColumn.Hidden = false;
        }

        #endregion

        #region ModulosObtenerInformacionEstructuraHoja

        /*************************************************************************************
            Metodo para regresar el numero de filas que tiene un documento de Excel
            Nombre: Daniel Sanchez Cervantes
            Fecha: 20/10/2015
        *************************************************************************************/
        public int UltimoRenglon(Excel.Workbook booksExcel, String valColumna)
        {
            int irowUltimo;
            String celda;

            try
            {
                celda = valColumna + "1048575";
                booksExcel.Application.ActiveWorkbook.ActiveSheet.Range(celda).select();
            }
            catch (Exception)
            {
                celda = valColumna + "65536";
                booksExcel.Application.ActiveWorkbook.ActiveSheet.Range(celda).select();
            }

            sheetExcel = (Excel.Worksheet)booksExcel.ActiveSheet;
            rangoExcel = sheetExcel.get_Range(celda);
            //rangoExcel = sheetExcel.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            //rangoExcel = rangoExcel.End[Excel.XlDirection.xlDown];
            rngUltimo = rangoExcel.End[Excel.XlDirection.xlUp];
            irowUltimo = rngUltimo.Row;
            booksExcel.Application.ActiveWorkbook.ActiveSheet.Range("A1").select();
            return irowUltimo;
        }

        /*************************************************************************************
            Metodo para regresar el numero de columnas que tiene un documento de Excel
            Nombre: Daniel Sanchez Cervantes
            Fecha: 20/10/2015
        *************************************************************************************/
        public int UltimaColumna(Excel.Workbook booksExcel, String valRow)
        {
            int icolUltimo;
            String celda;


            try
            {
                celda = "XFD" + valRow;
                booksExcel.Application.ActiveWorkbook.ActiveSheet.Range(celda).select();

            }
            catch
            {
                celda = "IV" + valRow;
                booksExcel.Application.ActiveWorkbook.ActiveSheet.Range(celda).select();
            }


            sheetExcel = (Excel.Worksheet)booksExcel.ActiveSheet;
            rangoExcel = sheetExcel.get_Range(celda);
            //rangoExcel = sheetExcel.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            //rangoExcel = rangoExcel.End[Excel.XlDirection.xlToRight];            
            colUltimo = rangoExcel.End[Excel.XlDirection.xlToLeft];
            icolUltimo = colUltimo.Column;
            booksExcel.Application.ActiveWorkbook.ActiveSheet.Range("A1").select();
            return icolUltimo;
        }

        //Función que regresa la letra de la columa de excel correspondiente al número pasado por parametro 
        public String ColumnaCorrespondiente(int num)
        {
            string columna = "";
            switch (num)
            {
                case 1: columna = "A"; break;
                case 2: columna = "B"; break;
                case 3: columna = "C"; break;
                case 4: columna = "D"; break;
                case 5: columna = "E"; break;
                case 6: columna = "F"; break;
                case 7: columna = "G"; break;
                case 8: columna = "H"; break;
                case 9: columna = "I"; break;
                case 10: columna = "J"; break;
                case 11: columna = "K"; break;
                case 12: columna = "L"; break;
                case 13: columna = "M"; break;
                case 14: columna = "N"; break;
                case 15: columna = "O"; break;
                case 16: columna = "P"; break;
                case 17: columna = "Q"; break;
                case 18: columna = "R"; break;
                case 19: columna = "S"; break;
                case 20: columna = "T"; break;
                case 21: columna = "U"; break;
                case 22: columna = "V"; break; ;
                case 23: columna = "W"; break;
                case 24: columna = "X"; break;
                case 25: columna = "Y"; break;
                case 26: columna = "Z"; break;
                case 27: columna = "AA"; break;
                case 28: columna = "AB"; break;
                case 29: columna = "AC"; break;
                case 30: columna = "AD"; break;
                case 31: columna = "AE"; break;
                case 32: columna = "AF"; break;
                case 33: columna = "AG"; break;
                case 34: columna = "AH"; break;
                case 35: columna = "AI"; break;
                case 36: columna = "AJ"; break;
                case 37: columna = "AK"; break;
                case 38: columna = "AL"; break;
                case 39: columna = "AM"; break;
                case 40: columna = "AN"; break;
                case 41: columna = "AO"; break;
                case 42: columna = "AP"; break;
                case 43: columna = "AQ"; break;
                case 44: columna = "AR"; break;
                case 45: columna = "AS"; break;
                case 46: columna = "AT"; break;
                case 47: columna = "AU"; break;
                case 48: columna = "AV"; break;
                case 49: columna = "AW"; break;
                case 50: columna = "AX"; break;
                case 51: columna = "AY"; break;
                case 52: columna = "AZ"; break;
                case 53: columna = "BA"; break;
                case 54: columna = "BB"; break;
                case 55: columna = "BC"; break;
                case 56: columna = "BD"; break;
                case 57: columna = "BE"; break;
                case 58: columna = "BF"; break;
                case 59: columna = "BG"; break;
                case 60: columna = "BH"; break;
                case 61: columna = "BI"; break;
                case 62: columna = "BJ"; break;
                case 63: columna = "BK"; break;
                case 64: columna = "BL"; break;
                case 65: columna = "BM"; break;
                case 66: columna = "BN"; break;
                case 67: columna = "BO"; break;
                case 68: columna = "BP"; break;
                case 69: columna = "BQ"; break;
                case 70: columna = "BR"; break;
                case 71: columna = "BS"; break;
                case 72: columna = "BT"; break;
                case 73: columna = "BU"; break;
                case 74: columna = "BV"; break;
                case 75: columna = "BW"; break;
                case 76: columna = "BX"; break;
                case 77: columna = "BY"; break;
                case 78: columna = "BZ"; break;
                case 79: columna = "CA"; break;
                case 80: columna = "CB"; break;
                case 81: columna = "CC"; break;
                case 82: columna = "CD"; break;
                case 83: columna = "CE"; break;
                case 84: columna = "CF"; break;
                case 85: columna = "CG"; break;
                case 86: columna = "CH"; break;
                case 87: columna = "CI"; break;
                case 88: columna = "CJ"; break;
                case 89: columna = "CK"; break;
                case 90: columna = "CL"; break;
                case 91: columna = "CM"; break;
                case 92: columna = "CN"; break;
                case 93: columna = "CO"; break;
                case 94: columna = "CP"; break;
                case 95: columna = "CQ"; break;
                case 96: columna = "CR"; break;
                case 97: columna = "CS"; break;
                case 98: columna = "CT"; break;
                case 99: columna = "CU"; break;
                case 100: columna = "CV"; break;
                case 101: columna = "CW"; break;
                case 102: columna = "CX"; break;
                case 103: columna = "CY"; break;
                case 104: columna = "CZ"; break;
                case 105: columna = "DA"; break;
                default: columna = ""; break;
            }
            return (columna);
        }


        ///*******************************************************************
        //    Funcion para encontrar la fila de los 
        //    titulos en un archivo en excel 
        //*******************************************************************/
        //public int filaTitulos(Excel.Workbook bookExcel, String Titulo1, String Titulo2)
        //{
        //    int Fila = 0;
        //    int filaRespaldo = 0;
        //    Boolean titulos = false;
        //    String valores = "";
        //    String[] colTitulos;
        //    String[] celTitulos;

        //    for (int col = 1; col < 51; col++)
        //    {
        //        for (int cell = 1; cell < 51; cell++)
        //        {
        //            valores = valores + "\t" + bookExcel.Application.ActiveWorkbook.ActiveSheet.Cells[col, cell].Value;
        //            if (cell == 50)
        //            {
        //                valores = valores + "\n";
        //            }
        //        }
        //    }

        //    colTitulos = valores.Split('\n');

        //    for (int j = 0; j < colTitulos.Length; j++)
        //    {
        //        celTitulos = colTitulos[j].Split('\t');
        //        for (int k = 0; k < celTitulos.Length; k++)
        //        {
        //            if (objNu4it.BFonetica(celTitulos[k], Titulo1) == true)
        //            {
        //                for (int i = 0; i < celTitulos.Length; i++)
        //                {
        //                    if (objNu4it.BFonetica(celTitulos[i], Titulo2) == true)
        //                    {
        //                        Fila = j + 1;
        //                        j = colTitulos.Length - 1;
        //                        i = celTitulos.Length - 1;
        //                        titulos = true;
        //                        break;
        //                    }
        //                }
        //                if (titulos == false)
        //                {
        //                    filaRespaldo = j;
        //                    Fila = filaRespaldo + 1;
        //                }
        //            }
        //            if (OBJ.BFonetica(celTitulos[k], Titulo2) == true && titulos == false)
        //            {
        //                filaRespaldo = j;
        //                Fila = filaRespaldo + 1;
        //            }
        //        }
        //    }
        //    return Fila;
        //}

        //▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬//
        //                      FUNCION QUE BUSCA EL RENGLON DONDE SE ENCUENTRAN LOS TITULOS 
        //                                   JORGE NÚÑEZ     -   21/OCT/26
        //▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬//

        public int filaTitulos_2(Excel.Workbook books, Excel.Worksheet Pestania, string Titulo1, string Titulo2)
        {
            int rengloncontitulos = 0;                                  //Variable donde se almacenara el numero renglon reultante
            int rangodebusqueda = 51;                                   //RAngo de X renglones los que recorera
            int[] bandera = new int[10];                                //Bandera donde guarda el lugar posible del renglon de titulos
            string[] DATOS = new string[51];                            //Array que almacena los 51 elementos del renglon
            int renglon = 1;                                            //Renglon inicial donde empezara la lectura (Excel)
            int bnd = 0;

            while (renglon < rangodebusqueda)
            {
                try
                {
                    string primerCasilla = Convert.ToString("A") + Convert.ToString(renglon);
                    string ultimaCasilla = Convert.ToString("AX") + Convert.ToString(renglon);
                    unRango = Pestania.get_Range(primerCasilla, ultimaCasilla);
                    unRango.Select();
                    unRango.Copy();
                    string DatosEnRenglon = objNu4it.clipboardObtenerTexto(); //Clipboard.GetText();            //Obtiene del clipboard la CADENA copiada del clipboard.
                    DATOS = DatosEnRenglon.Split('\t');                //Separa los datos del clipboard y los guarda en el array
                    int TituloEncontrados = 1;                              //Numero de titulos dentro del array
                    for (int i = 0; i < DATOS.Length; i++)                  //Por cada elemento del array entonces...
                    {
                        DATOS[i] = objNu4it.Modifica(DATOS[i]);               //Modifica el texto de la celda, cambia el valor a mayusculas y elimina acentos
                        if (DATOS[i] == objNu4it.Modifica(Titulo1) || DATOS[i] == objNu4it.Modifica(Titulo2))     //Si el elemento es igual al Titulo1 o al 2 entonces...
                        {
                            if (TituloEncontrados < 2)                      //Si el numero de titulos en el renglon es menor a 2 sigue buscando
                            {
                                bandera[bnd] = renglon;                 //Talvez sea este encabezado lo guardo en la bandera
                                bnd++;
                            }
                            else
                            {
                                rengloncontitulos = renglon;            //Si el numero de titulos en el renglon es mayor a 2 entonces este es el RENGLON DE TITULOS
                                renglon = rangodebusqueda;                  //Detiene el ciclo WHILE dandole el valor maximo al condicionante
                                ///break;                                      //Detiene el ciclo for - MODIFICACION EL 12/07/2017
                                return rengloncontitulos;
                            }
                            TituloEncontrados++;                            //Aumneta +1 el numero de titulos en el renglon
                        }
                    }
                    renglon++;                                              //Aumenta +1 al renglon que se esta leyendo
                }
                catch
                {
                    renglon++;                                              //Aumenta +1 al renglon que se esta leyendo
                }
            }
            if (renglon == rangodebusqueda)
                rengloncontitulos = bandera[0];                         //Si no se encontro los 2 encabezados en el mismo renglon, la primer bandera sera usada como la fila de encabezados                     
            return (rengloncontitulos);
        }

        public int filaTitulos_2(Excel.Workbook books, string Titulo1, string Titulo2)
        {
            Excel.Worksheet Pestania;

            int rengloncontitulos = 0;                                  //Variable donde se almacenara el numero renglon reultante
            int rangodebusqueda = 51;                                   //RAngo de X renglones los que recorera
            int[] bandera = new int[10];                                //Bandera donde guarda el lugar posible del renglon de titulos
            string[] DATOS = new string[51];                            //Array que almacena los 51 elementos del renglon
            int renglon = 1;                                            //Renglon inicial donde empezara la lectura (Excel)
            int bnd = 0;
            Pestania = (Excel.Worksheet)books.Application.ActiveWorkbook.ActiveSheet;
            while (renglon < rangodebusqueda)
            {
                try
                {
                    string primerCasilla = Convert.ToString("A") + Convert.ToString(renglon);
                    string ultimaCasilla = Convert.ToString("AX") + Convert.ToString(renglon);
                    unRango = Pestania.get_Range(primerCasilla, ultimaCasilla);
                    unRango.Select();
                    unRango.Copy();
                    string DatosEnRenglon = objNu4it.clipboardObtenerTexto(); //Clipboard.GetText();            //Obtiene del clipboard la CADENA copiada del clipboard.
                    DATOS = DatosEnRenglon.Split('\t');                //Separa los datos del clipboard y los guarda en el array
                    int TituloEncontrados = 1;                              //Numero de titulos dentro del array
                    for (int i = 0; i < DATOS.Length; i++)                  //Por cada elemento del array entonces...
                    {
                        DATOS[i] = objNu4it.Modifica(DATOS[i]);               //Modifica el texto de la celda, cambia el valor a mayusculas y elimina acentos
                        if (DATOS[i] == objNu4it.Modifica(Titulo1) || DATOS[i] == objNu4it.Modifica(Titulo2))     //Si el elemento es igual al Titulo1 o al 2 entonces...
                        {
                            if (TituloEncontrados < 2)                      //Si el numero de titulos en el renglon es menor a 2 sigue buscando
                            {
                                bandera[bnd] = renglon;                 //Talvez sea este encabezado lo guardo en la bandera
                                bnd++;
                            }
                            else
                            {
                                rengloncontitulos = renglon;            //Si el numero de titulos en el renglon es mayor a 2 entonces este es el RENGLON DE TITULOS
                                renglon = rangodebusqueda;                  //Detiene el ciclo WHILE dandole el valor maximo al condicionante
                                return rengloncontitulos;
                                break;                                      //Detiene el ciclo for
                            }
                            TituloEncontrados++;                            //Aumneta +1 el numero de titulos en el renglon
                        }
                    }
                    renglon++;                                              //Aumenta +1 al renglon que se esta leyendo
                }
                catch
                {
                    renglon++;                                              //Aumenta +1 al renglon que se esta leyendo
                }
            }
            if (renglon == rangodebusqueda)
                rengloncontitulos = bandera[0];                         //Si no se encontro los 2 encabezados en el mismo renglon, la primer bandera sera usada como la fila de encabezados                     
            return (rengloncontitulos);
        }


        public int filaTitulos_3(Excel.Application AplicacionExcel, Excel.Workbook books, string NombrePestania, string Titulo1, string Titulo2)
        {
            Excel.Worksheet Pestania;
            int rengloncontitulos = 0;                                  //Variable donde se almacenara el numero renglon reultante
            int rangodebusqueda = 51;                                   //RAngo de X renglones los que recorera
            int[] bandera = new int[10];                                //Bandera donde guarda el lugar posible del renglon de titulos
            string[] DATOS = new string[51];                            //Array que almacena los 51 elementos del renglon
            int renglon = 1;                                            //Renglon inicial donde empezara la lectura (Excel)
            int bnd = 0;
            try
            {
                ((Excel.Worksheet)AplicacionExcel.ActiveWorkbook.Sheets[NombrePestania]).Select();                   //---Nombre o numero de la hoja activa
                Pestania = (Excel.Worksheet)books.ActiveSheet;
            }
            catch
            {
                ((Excel.Worksheet)AplicacionExcel.ActiveWorkbook.Sheets[1]).Select();                   //---Nombre o numero de la hoja activa
                Pestania = (Excel.Worksheet)books.ActiveSheet;
            }
            while (renglon < rangodebusqueda)
            {
                try
                {
                    string primerCasilla = Convert.ToString("A") + Convert.ToString(renglon);
                    string ultimaCasilla = Convert.ToString("AX") + Convert.ToString(renglon);
                    unRango = Pestania.get_Range(primerCasilla, ultimaCasilla);
                    unRango.Select();
                    unRango.Copy();
                    string DatosEnRenglon = objNu4it.clipboardObtenerTexto(); //Clipboard.GetText();            //Obtiene del clipboard la CADENA copiada del clipboard.
                    DATOS = DatosEnRenglon.Split('\t');                //Separa los datos del clipboard y los guarda en el array
                    int TituloEncontrados = 1;                              //Numero de titulos dentro del array
                    for (int i = 0; i < DATOS.Length; i++)                  //Por cada elemento del array entonces...
                    {
                        DATOS[i] = objNu4it.Modifica(DATOS[i]);               //Modifica el texto de la celda, cambia el valor a mayusculas y elimina acentos
                        if (DATOS[i] == objNu4it.Modifica(Titulo1) || DATOS[i] == objNu4it.Modifica(Titulo2))     //Si el elemento es igual al Titulo1 o al 2 entonces...
                        {
                            if (TituloEncontrados < 2)                      //Si el numero de titulos en el renglon es menor a 2 sigue buscando
                            {
                                bandera[bnd] = renglon;                 //Talvez sea este encabezado lo guardo en la bandera
                                bnd++;
                            }
                            else
                            {
                                rengloncontitulos = renglon;            //Si el numero de titulos en el renglon es mayor a 2 entonces este es el RENGLON DE TITULOS
                                renglon = rangodebusqueda;                  //Detiene el ciclo WHILE dandole el valor maximo al condicionante
                                break;                                      //Detiene el ciclo for
                            }
                            TituloEncontrados++;                            //Aumneta +1 el numero de titulos en el renglon
                        }
                    }
                    renglon++;                                              //Aumenta +1 al renglon que se esta leyendo
                }
                catch
                {
                    renglon++;                                              //Aumenta +1 al renglon que se esta leyendo
                }
            }
            if (renglon == rangodebusqueda)
                rengloncontitulos = bandera[0];                         //Si no se encontro los 2 encabezados en el mismo renglon, la primer bandera sera usada como la fila de encabezados                     
            return (rengloncontitulos);
        }

        #endregion

        #region ModulosCambiarInformacionDatosHoja

        public void DesactivarFiltros(Excel.Worksheet Pestania)
        {
            Pestania.AutoFilterMode = false;
        }

        public void EscribeTexto(string texto, int renglon, int columna, Excel.Worksheet Pestania)
        {
            Pestania.Cells[renglon, columna] = texto;
        }

        public void CopiarRango(string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            unRango.Copy();
        }

        public void BorrarRango(string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            unRango.Delete();
        }

        public void PegarPortaPapelesRango(string CeldaPegar, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaPegar);
            unRango.Select();
            Pestania.Paste();
        }

        //▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬//
        //                      FUNCION QUE INSERTA FORMULA EN UNA CELDA ESPECIFICA EN EL EXCEL 
        //                                   JORGE NÚÑEZ     -   21/OCT/26
        //▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬//
        public void EscribeFormula(string formula, int renglon, int columna, Excel.Worksheet Pestania)
        {
            Pestania.Cells[renglon, columna].Formula = formula;
        }

        //▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬//
        //                      FUNCION QUE DA AUTORRELLENO DE UNA CELDA A UN RANGO DE CELDAS
        //                                   JORGE NÚÑEZ     -   21/OCT/26
        //▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬//
        public void DarAutoRelleno(string CeldaDatoCopiar, string CeldaIni, string CeldaFin, Excel.Worksheet HojaExcel)
        {
            Excel.Range RangoInicio = HojaExcel.get_Range(CeldaDatoCopiar + ":" + CeldaDatoCopiar);
            Excel.Range RangoDestino = HojaExcel.get_Range(CeldaIni + ":" + CeldaFin);
            RangoInicio.AutoFill(RangoDestino, Excel.XlAutoFillType.xlFillDefault);
        }

        #endregion

        #region ModulosDarFormatoRangoCeldas

        public void FormatoTamanioTipoLetra(string Tamanio, string Tipo, string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            unRango.Font.Size = Tamanio;
            unRango.Font.Name = Tipo;
        }

        public void FormatoNegrillaLetra(string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania, int Activar)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            if (Activar == SI) { unRango.Font.Bold = true; }
            else { unRango.Font.Bold = false; }
        }

        public void FormatoCursivaLetra(string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania, int Activar)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            if (Activar == SI) { unRango.Font.Italic = true; }
            else { unRango.Font.Italic = false; }
        }

        public void FormatoSubRayar(string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania, int Activar)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            if (Activar == SI) { unRango.Font.Underline = true; }
            else { unRango.Font.Underline = false; }
        }

        public void AlineacionVertical(int TipoAlineacion, string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            switch (TipoAlineacion)
            {
                case 1:
                    unRango.VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;
                    //Excel.XlHAlign.xlHAlignLeft;
                    break;
                case 2:
                    unRango.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                    //Excel.XlHAlign.xlHAlignRight;
                    break;
                case 3:
                    unRango.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    break;
                case 4:
                    unRango.VerticalAlignment = Excel.XlVAlign.xlVAlignJustify;
                    break;
                default:
                    unRango.VerticalAlignment = Excel.XlVAlign.xlVAlignDistributed;
                    break;
            }
        }

        public void AlineacionHorizontal(int TipoAlineacion, string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            switch (TipoAlineacion)
            {
                case 1:
                    unRango.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    break;
                case 2:
                    unRango.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    break;
                case 3:
                    unRango.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    break;
                case 4:
                    unRango.HorizontalAlignment = Excel.XlHAlign.xlHAlignJustify;
                    break;
                default:
                    unRango.HorizontalAlignment = Excel.XlHAlign.xlHAlignGeneral;
                    break;
            }
        }

        public void FormatearCeldas(string TipoFormato, string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            unRango.NumberFormat = TipoFormato;
        }

        public void BordearCeldasRango(string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            Excel.Borders border = unRango.Borders;
            border.LineStyle = Excel.XlLineStyle.xlContinuous;
            border.Weight = 2d;
        }

        public void BordearRango(string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            unRango.BorderAround(Missing.Value, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic);
        }

        public void QuitarBordeRango(string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            unRango.Borders.LineStyle = Excel.XlLineStyle.xlLineStyleNone;
        }

        public void CopiarPegarFormato(string CelIniCopFor, string CelFinCopFor, string CelIniPegFor, string CelFinPegFor, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CelIniCopFor, CelFinCopFor);
            unRango.Select();
            unRango.Copy();
            unRango = Pestania.get_Range(CelIniPegFor, CelFinPegFor);
            unRango.Select();
            unRango.PasteSpecial(Excel.XlPasteType.xlPasteFormats, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
        }

        public void AsignarAnchoColumna(string LetraColumna, double ValorAncho, Excel.Worksheet Pestania)
        {
            LetraColumna = LetraColumna + "1";
            unRango = Pestania.get_Range(LetraColumna);
            unRango.Select();
            unRango.EntireColumn.ColumnWidth = ValorAncho;
        }

        public void AsignarAltoFila(string NumFila, double ValorAlto, Excel.Worksheet Pestania)
        {
            NumFila = "A" + NumFila;
            unRango = Pestania.get_Range(NumFila);
            unRango.Select();
            unRango.EntireRow.RowHeight = ValorAlto;
        }

        public void AjustarAnchoColumna(string LetraColumna, Excel.Worksheet Pestania)
        {
            LetraColumna = LetraColumna + "1";
            unRango = Pestania.get_Range(LetraColumna);
            unRango.Select();
            unRango.EntireColumn.AutoFit();
        }

        public void AjustarAnchoColumnaRango(string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            unRango.EntireColumn.AutoFit();
        }

        public void AjustarAnchoColumnaTodaHoja(Excel.Worksheet Pestania)
        {
            Pestania.Cells.Select();
            Pestania.Cells.EntireColumn.AutoFit();
        }

        public void CombinarCeldas(string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            unRango.Merge();
        }

        public void ColorInteriorCelda(int color, int degradado, string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            unRango.Interior.Color = color;
            switch (degradado)
            {
                case 0:
                    unRango.Interior.TintAndShade = 0;
                    break;
                case 1:
                    unRango.Interior.TintAndShade = 0.799981688894314;
                    break;
                case 2:
                    unRango.Interior.TintAndShade = 0.599993896298105;
                    break;
                case 3:
                    unRango.Interior.TintAndShade = 0.399975585192419;
                    break;
                case 4:
                    unRango.Interior.TintAndShade = -0.249977111117893;
                    break;
                case 5:
                    unRango.Interior.TintAndShade = -0.499984740745262;
                    break;
                default:
                    unRango.Interior.TintAndShade = 0;
                    break;
            }
        }

        public void TemaColorInteriorCelda(int tema, int degradado, string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {

            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();

            switch (tema)
            {
                case 1:
                    unRango.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent1;
                    break;
                case 2:
                    unRango.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent2;
                    break;
                case 3:
                    unRango.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent3;
                    break;
                case 4:
                    unRango.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent4;
                    break;
                case 5:
                    unRango.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent5;
                    break;
                case 6:
                    unRango.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent6;
                    break;
                case 7:
                    unRango.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorLight1;
                    break;
                case 8:
                    unRango.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorLight2;
                    break;
                case 9:
                    unRango.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorDark1;
                    break;
                case 10:
                    unRango.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorDark2;
                    break;
                default:
                    unRango.Interior.ThemeColor = Excel.XlThemeColor.xlThemeColorDark1;
                    break;
            }

            if (tema < 7)
            {
                switch (degradado)
                {
                    case 0:
                        unRango.Interior.TintAndShade = 0;
                        break;
                    case 1:
                        unRango.Interior.TintAndShade = 0.799981688894314;
                        break;
                    case 2:
                        unRango.Interior.TintAndShade = 0.599993896298105;
                        break;
                    case 3:
                        unRango.Interior.TintAndShade = 0.399975585192419;
                        break;
                    case 4:
                        unRango.Interior.TintAndShade = -0.249977111117893;
                        break;
                    case 5:
                        unRango.Interior.TintAndShade = -0.499984740745262;
                        break;
                    default:
                        unRango.Interior.TintAndShade = 0;
                        break;
                }
            }
            else
            {
                if (tema < 9)
                {
                    switch (degradado)
                    {
                        case 0:
                            unRango.Interior.TintAndShade = 0;
                            break;
                        case 1:
                            unRango.Interior.TintAndShade = 0.499984740745262;
                            break;
                        case 2:
                            unRango.Interior.TintAndShade = 0.349986266670736;
                            break;
                        case 3:
                            unRango.Interior.TintAndShade = 0.249977111117893;
                            break;
                        case 4:
                            unRango.Interior.TintAndShade = 0.149998474074526;
                            break;
                        case 5:
                            unRango.Interior.TintAndShade = 4.99893185216834E-02;
                            break;
                        default:
                            unRango.Interior.TintAndShade = 0;
                            break;
                    }
                }
                else
                {
                    switch (degradado)
                    {
                        case 0:
                            unRango.Interior.TintAndShade = 0;
                            break;
                        case 1:
                            unRango.Interior.TintAndShade = -4.99893185216834E-02;
                            break;
                        case 2:
                            unRango.Interior.TintAndShade = -0.149998474074526;
                            break;
                        case 3:
                            unRango.Interior.TintAndShade = -0.249977111117893;
                            break;
                        case 4:
                            unRango.Interior.TintAndShade = -0.349986266670736;
                            break;
                        case 5:
                            unRango.Interior.TintAndShade = -0.499984740745262;
                            break;
                        default:
                            unRango.Interior.TintAndShade = 0;
                            break;
                    }

                }
            }
        }

        public void ColorLetra(int color, string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {
            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();
            unRango.Font.Color = color;
        }

        public void TemaColorLetra(int tema, int degradado, string CeldaInicial, string CeldaFinal, Excel.Worksheet Pestania)
        {

            unRango = Pestania.get_Range(CeldaInicial, CeldaFinal);
            unRango.Select();

            switch (tema)
            {
                case 1:
                    unRango.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent1;
                    break;
                case 2:
                    unRango.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent2;
                    break;
                case 3:
                    unRango.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent3;
                    break;
                case 4:
                    unRango.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent4;
                    break;
                case 5:
                    unRango.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent5;
                    break;
                case 6:
                    unRango.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent6;
                    break;
                case 7:
                    unRango.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorLight1;
                    break;
                case 8:
                    unRango.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorLight2;
                    break;
                case 9:
                    unRango.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorDark1;
                    break;
                case 10:
                    unRango.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorDark2;
                    break;
                default:
                    unRango.Font.ThemeColor = Excel.XlThemeColor.xlThemeColorDark1;
                    break;
            }

            if (tema < 7)
            {
                switch (degradado)
                {
                    case 0:
                        unRango.Font.TintAndShade = 0;
                        break;
                    case 1:
                        unRango.Font.TintAndShade = 0.799981688894314;
                        break;
                    case 2:
                        unRango.Font.TintAndShade = 0.599993896298105;
                        break;
                    case 3:
                        unRango.Font.TintAndShade = 0.399975585192419;
                        break;
                    case 4:
                        unRango.Font.TintAndShade = -0.249977111117893;
                        break;
                    case 5:
                        unRango.Font.TintAndShade = -0.499984740745262;
                        break;
                    default:
                        unRango.Font.TintAndShade = 0;
                        break;
                }
            }
            else
            {
                if (tema < 9)
                {
                    switch (degradado)
                    {
                        case 0:
                            unRango.Font.TintAndShade = 0;
                            break;
                        case 1:
                            unRango.Font.TintAndShade = 0.499984740745262;
                            break;
                        case 2:
                            unRango.Font.TintAndShade = 0.349986266670736;
                            break;
                        case 3:
                            unRango.Font.TintAndShade = 0.249977111117893;
                            break;
                        case 4:
                            unRango.Font.TintAndShade = 0.149998474074526;
                            break;
                        case 5:
                            unRango.Font.TintAndShade = 4.99893185216834E-02;
                            break;
                        default:
                            unRango.Font.TintAndShade = 0;
                            break;
                    }
                }
                else
                {
                    switch (degradado)
                    {
                        case 0:
                            unRango.Font.TintAndShade = 0;
                            break;
                        case 1:
                            unRango.Font.TintAndShade = -4.99893185216834E-02;
                            break;
                        case 2:
                            unRango.Font.TintAndShade = -0.149998474074526;
                            break;
                        case 3:
                            unRango.Font.TintAndShade = -0.249977111117893;
                            break;
                        case 4:
                            unRango.Font.TintAndShade = -0.349986266670736;
                            break;
                        case 5:
                            unRango.Font.TintAndShade = -0.499984740745262;
                            break;
                        default:
                            unRango.Font.TintAndShade = 0;
                            break;
                    }

                }
            }
        }

        #endregion

        #region ModulosObtenerDatosRangoSeleccionado

        public int ColumnaInicialSeleccionada(Excel.Application appExcel)
        {
            int ColIni;
            ColIni = appExcel.Selection.Column;
            return (ColIni);
        }

        public int RenglonInicialSeleccionado(Excel.Application appExcel)
        {
            int RenIni;
            RenIni = appExcel.Selection.Row;
            return (RenIni);
        }

        public int CantidadRenglonesSeleccionados(Excel.Application appExcel)
        {
            int CantRenglones;
            CantRenglones = appExcel.Selection.Rows.Count;
            return (CantRenglones);
        }

        public int CantidadColumnasSeleccionadas(Excel.Application appExcel)
        {
            int CantColumnas;
            CantColumnas = appExcel.Selection.Columns.Count;
            return (CantColumnas);
        }

        #endregion

        #region Macros

        //▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬//
        //    FUNCION QUE OBTIENE LA MACRO DE UN ARCHIVO DE TEXTOY LA EJECUTA EN EL ARCHIVO DE TRABAJO DE EXCEL
        //                                   JORGE NÚÑEZ     -   25/OCT/26
        //                                using VBIDE = Microsoft.Vbe.Interop;        
        //▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬//
        public int EjecutarMacro(Excel.Application appExcel, Excel.Workbook ArchivoTrabajo, string RutaMacroTXT, string NombreMACRO)
        {
            int Exito = 0;
            ///Agregrando Modulo
            Microsoft.Vbe.Interop.VBComponent MODULO = ArchivoTrabajo.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
            Microsoft.Vbe.Interop.CodeModule CODIGO = MODULO.CodeModule;
            int NumLinea = CODIGO.CountOfLines + 1;
            ///Leyendo archivo de texto donde esta la macro
            try
            {
                string lineaTexto = "";
                if (File.Exists(RutaMacroTXT))
                {
                    StreamReader sr = new StreamReader(RutaMacroTXT);
                    lineaTexto = sr.ReadToEnd();
                    sr.Close();
                    ///Armando texto para enviarlo al modulo/macro
                    string TextoCodigo = "Public Sub " + NombreMACRO + "()\r\n";
                    TextoCodigo += lineaTexto;
                    CODIGO.InsertLines(NumLinea, TextoCodigo);
                    ///Ejecutar macro
                    appExcel.GetType().InvokeMember("Run", BindingFlags.Default | BindingFlags.InvokeMethod, null, appExcel, new object[] { NombreMACRO });
                    MODULO.CodeModule.DeleteLines(1, MODULO.CodeModule.CountOfLines);
                    Exito = 1;
                }
                else
                {
                    Exito = 0;
                }
            }
            catch
            {
                Exito = 0;
            }
            return (Exito);
        }

        public int EjecutarMacroTEXTO(Excel.Application appExcel, Excel.Workbook ArchivoTrabajo, string TextoCodigo, string NombreMACRO)
        {
            int Exito = 0;
            ///Agregrando Modulo
            Microsoft.Vbe.Interop.VBComponent MODULO = ArchivoTrabajo.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
            Microsoft.Vbe.Interop.CodeModule CODIGO = MODULO.CodeModule;
            int NumLinea = CODIGO.CountOfLines + 1;
            ///Leyendo archivo de texto donde esta la macro
            try
            {
                ///Armando texto para enviarlo al modulo/macro
                CODIGO.InsertLines(NumLinea, TextoCodigo);
                ///Ejecutar macro
                appExcel.GetType().InvokeMember("Run", BindingFlags.Default | BindingFlags.InvokeMethod, null, appExcel, new object[] { NombreMACRO });
                MODULO.CodeModule.DeleteLines(1, MODULO.CodeModule.CountOfLines);
                Exito = 1;
            }
            catch
            {
                Exito = 0;
            }
            return (Exito);
        }
        //▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬//

        #endregion

        public int FilaTitulos_6(Excel.Worksheet HOJA, int maxfilas, int maxcols)
        {
            int rengloncontitulos = 0;

            //A1
            for (int i = 1; i <= maxfilas; i++)
            {
                string cellIni = "A" + i;//objNuExcel.ColumnaCorrespondiente(i) + i;
                string cellFin = ColumnaCorrespondiente(maxcols) + i; //objNuExcel.ColumnaCorrespondiente(i + 2) + i;
                Excel.Range rango = HOJA.get_Range(cellIni, cellFin);
                rango.Select();
                rango.Copy();
                string[] DatosEnRenglon = objNu4it.clipboardObtenerTexto().Split('\t');

                int bnd = 0;
                for (int x = 0; x < DatosEnRenglon.Length; x++)
                {
                    String dato = DatosEnRenglon[x];
                    dato = objNu4it.Modifica(dato);

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

        public void ajustarTextoAnchoAltoRangoColumnas(String columnaInical, String renglonInicial, String columnaFinal, String renglonfinal, double anchoClumna, double altoColumna, Excel.Worksheet HOJA)
        {

            Excel.Range rango = HOJA.get_Range(columnaInical + renglonInicial + ":" + columnaFinal + renglonfinal);


            rango.Select();
            rango.EntireColumn.ColumnWidth = anchoClumna;
            rango.EntireRow.RowHeight = altoColumna;
            rango.EntireRow.WrapText = true;
            //rango.EntireColumn.AutoFit();
            //rango.EntireRow.AutoFit();
            rango.Borders.LineStyle = Excel.XlLineStyle.xlDouble;
            //rango.Borders.Weight = Excel.XlBorderWeight.xlMedium;

            AlineacionVertical(3, columnaInical + renglonInicial, columnaFinal + renglonfinal, HOJA);
            AlineacionHorizontal(3, columnaInical + renglonInicial, columnaFinal + renglonfinal, HOJA);


        }


    }
}
