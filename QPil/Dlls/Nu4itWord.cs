using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Librerias
using Word = Microsoft.Office.Interop.Word;

namespace QPil.Dlls
{
    /*****************************************************************************************************************************************************
     *                                                 Metodos.PARA LA AUTOMATIZACION DE WORD
     *                                                              Jorge Nuñez
     *                                                              Eduardo Meza
     *                                                              27/Junio/17
     *****************************************************************************************************************************************************/
    class Nu4itWord
    {
        /// <summary>
        /// Insertar texto a un campo (MargeField) ubicado en el archivo de Word
        /// </summary>
        /// <param name="MiWord">Variable de la aplicación de Word</param>
        /// <param name="MiDocumento">Variable del documento de Word</param>
        /// <param name="Field">Nombre del 'MergeField' insertado en el documento</param>
        /// <param name="Texto">Texto que sera eniado al campo seleccionado</param>
        public void InsertaTextoWord(Word.Application MiWord, Word.Document MiDocumento, string Field, string Texto)
        {
            foreach (Microsoft.Office.Interop.Word.Field field in MiDocumento.Fields)
                if (field.Code.Text.Contains(Field))
                {
                    field.Select();
                    MiWord.Selection.TypeText(Texto);
                    break;
                }
        }

        /// <summary>
        /// Guardar archivo de Word en PDF
        /// </summary>
        /// <param name="MiDocumento">Variable del documento de Word</param>
        /// <param name="RutaGuardar">Ruta donde se guardara el archivo</param>
        /// <param name="NombreArchivo">Nombre del archivo (Sin extención)</param>
        public Word.Document GuardarComoPDF(Word.Document MiDocumento, string RutaGuardar, string NombreArchivo)
        {
            if (!RutaGuardar.EndsWith(@"\"))
                RutaGuardar += @"\";
            MiDocumento.SaveAs2(RutaGuardar + NombreArchivo + ".pdf", Word.WdSaveFormat.wdFormatPDF);
            return MiDocumento;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MiDocumento"></param>
        /// <param name="RutaGuardar"></param>
        /// <param name="NombreArchivo"></param>
        public Word.Document ExportarComoPDF(Word.Document MiDocumento, string RutaGuardar, string NombreArchivo)
        {
            if (!RutaGuardar.EndsWith(@"\"))
                RutaGuardar += @"\";
            MiDocumento.ExportAsFixedFormat(RutaGuardar + NombreArchivo + ".pdf", Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
            return MiDocumento;
        }

        /// <summary>
        /// Insertar Imagen en la celda de una tabla de word
        /// </summary>
        /// <param name="MiDocumento"></param>
        /// <param name="rutaImagen"></param>
        /// <param name="row"></param>
        /// <param name="colum"></param>
        /// <param name="dimension"></param>
        public void insertarImagen(Word.Document MiDocumento, String rutaImagen, int row, int colum, int dimension)
        {
            int a = MiDocumento.Tables.Count;
            Word.Range docRange = MiDocumento.Tables[a].Cell(row, colum).Range;
            var shape = docRange.InlineShapes.AddPicture(rutaImagen);
            shape.Width = dimension;
            shape.Height = dimension;

        }

        /// <summary>
        /// Inserta pie de pagina en el lado izquierdo del documento. [By Resources.Clases.JMNI]
        /// </summary>
        /// <param name="WordDoc">Documento de Word</param>
        /// <param name="Texto">Texto del pie de pagina</param>
        /// <returns>True o False</returns>
        public bool InsertaPieDePagina_LEFT(Word.Document WordDoc, string Texto)
        {
            try
            {
                foreach (Word.Section wordSection in WordDoc.Sections)
                {
                    Word.Range footerRange = wordSection.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    footerRange.Start = footerRange.End;
                    footerRange.Text = Texto;
                    footerRange.Font.ColorIndex = Word.WdColorIndex.wdGray50;
                    footerRange.Font.Size = 8;
                    footerRange.Font.Bold = 1;
                }
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }


    }
}
