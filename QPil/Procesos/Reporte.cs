/*Diana Miranda
 * se crea y llena el reporte usando las tablas de los procesos 
 * 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nu4it;
using nu4itExcel;
namespace QPil.Procesos
{
    class Reporte
    {
        usaR objnu4 = new usaR();
        nuExcel objnuExcel = new nuExcel();
        DataTable SISE = new DataTable();
        string LOG;

        //genera la estructura del reporte
        public void generaReporte(string log)
        {
            LOG = log;
            int j;
            int row;
            string siniestro="";
            DataRow dataRow=null;
            SISE = DatosLog.DATOS.Copy();//obtiene la tabla de SISE correspondiente
            //SISE = UIAutomation.DATOS.Copy();
            AutomatizadorExcel.PreparaInicioExcelNuevaInstancia();
            AutomatizadorExcel.ArchivoTrabajoExcel= objnuExcel.AbrirArchivoNuevo(AutomatizadorExcel.MiExcel);
            
            AutomatizadorExcel.HojaExcel = objnuExcel.ActivarPestaniaExcel(1, AutomatizadorExcel.MiExcel, AutomatizadorExcel.ArchivoTrabajoExcel);

            encabezados();
            //da formato de texto a toda la hoja de excel
            AutomatizadorExcel.HojaExcel.Cells.NumberFormat="@";
            //cambia el dato de siniestro en las tablas
            try
            {
                cambioSiniestro();
            }
            catch (Exception ex)
            {

                objnu4.ReportarLog(log, ex.ToString());
            }

            //llenado de la hoja de excel
            row = 0;
            if (SISE.Rows.Count > 0)
            {
               

                for (int i = 1; i <= SISE.Rows.Count*8; i++)
                {
                    i += 2;
                    j = 3;
                        //llena la parte de sise en excel
                    try
                    {
                        dataRow = SISE.Rows[row];

                        siniestro = dataRow["Siniestro"].ToString();
                        //objnuExcel.EscribeTexto(dataRow["Siniestro"].ToString(), i, j, AutomatizadorExcel.HojaExcel);
                        AutomatizadorExcel.HojaExcel.Cells[i, j] = dataRow["Siniestro"].ToString();
                        j++;
                        objnuExcel.EscribeTexto(dataRow["Poliza"].ToString(), i, j, AutomatizadorExcel.HojaExcel);
                        j++;
                        objnuExcel.EscribeTexto(dataRow["Modelo"].ToString(), i, j, AutomatizadorExcel.HojaExcel);
                        j++;
                        objnuExcel.EscribeTexto(dataRow["Motor"].ToString(), i, j, AutomatizadorExcel.HojaExcel);
                        j++;
                        objnuExcel.EscribeTexto(dataRow["Marca"].ToString(), i, j, AutomatizadorExcel.HojaExcel);
                        j++;
                        objnuExcel.EscribeTexto(dataRow["Serie"].ToString(), i, j, AutomatizadorExcel.HojaExcel);
                        j++;
                        objnuExcel.EscribeTexto(dataRow["Vigencia"].ToString(), i, j, AutomatizadorExcel.HojaExcel);
                    }
                    catch (Exception ex)
                    {

                        objnu4.ReportarLog(log, ex.ToString());
                    }

                    i++;
                    try
                    {


                        //llena la parte de las facturas
                        var filtroSiniestro = from SelecDatos in ManejoDeDocumentos.DT_DATOS_SAT.AsEnumerable()
                                              where SelecDatos.Field<string>("NO SINIESTRO") == siniestro
                                              select SelecDatos;
                        if (filtroSiniestro.Count() > 0)
                        {

                            foreach (var item in filtroSiniestro)
                            {
                                dataRow = item;
                                if(!string.IsNullOrEmpty(dataRow["AÑO"].ToString())&!string.IsNullOrEmpty(dataRow["SERIE"].ToString()))
                                    break;
                            }
                            //objnuExcel.EscribeTexto(dataRow["NO SINIESTRO"].ToString(), i, 3, AutomatizadorExcel.HojaExcel);
                            objnuExcel.EscribeTexto(dataRow["AÑO"].ToString(), i, 5, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["MOTOR"].ToString(), i, 10, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["MARCA"].ToString(), i, 11, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["PLACAS"].ToString(), i, 12, AutomatizadorExcel.HojaExcel);
                            objnuExcel.EscribeTexto(dataRow["SERIE"].ToString(), i, 8, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["VERSION"].ToString(), i, 24, AutomatizadorExcel.HojaExcel);
                        }
                    }
                    catch (Exception ex)
                    {
                        objnu4.ReportarLog(log, ex.ToString());

                    }

                    i++;
                    try
                    {
                        //---informacion de SAT
                        var filtroSiniestro = from SelecDatos in ManejoDeDocumentos.DT_DATOS_SAT2.AsEnumerable()
                                              where SelecDatos.Field<string>("NO SINIESTRO") == siniestro
                                              select SelecDatos;
                        if (filtroSiniestro.Count() > 0)
                        {
                            foreach (var item in filtroSiniestro)
                            {
                                dataRow = item;
                                if(!string.IsNullOrEmpty(dataRow["ESTATUS SAT"].ToString()))
                                    break;
                            }
                            //TOMAR INFORMACION DE FACTURAS
                            objnuExcel.EscribeTexto(dataRow["ESTATUS SAT"].ToString(), i, 10, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["EFECTO COMPROBANTE"].ToString(), i, 26, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["ESCANCELABLE"].ToString(), i, 27, AutomatizadorExcel.HojaExcel);
                        }
                    }
                    catch (Exception ex)
                    {
                        objnu4.ReportarLog(log, ex.ToString());

                    }
                    i++;
                    //llena polizas
                    try
                    {


                        var filtroSiniestro = from SelecDatos in ManejoDeDocumentos.Polizas6Col.AsEnumerable()
                                              where SelecDatos.Field<string>("NO SINIESTRO") == siniestro
                                              select SelecDatos;
                        if (filtroSiniestro.Count() > 0)
                        {
                            foreach (var item in filtroSiniestro)
                            {
                                dataRow = item;
                                if(!string.IsNullOrEmpty(dataRow["NPOLIZA"].ToString())&!string.IsNullOrEmpty(dataRow["MODELO"].ToString()))
                                break;
                            }
                            //objnuExcel.EscribeTexto(dataRow["NO SINIESTRO"].ToString(), i, 3, AutomatizadorExcel.HojaExcel);
                            objnuExcel.EscribeTexto(dataRow["NPOLIZA"].ToString(), i, 4, AutomatizadorExcel.HojaExcel);
                            objnuExcel.EscribeTexto(dataRow["MODELO"].ToString(), i, 5, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["MOTOR"].ToString(), i, 6, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["MARCA"].ToString(), i, 11, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["PLACAS"].ToString(), i, 12, AutomatizadorExcel.HojaExcel);
                            objnuExcel.EscribeTexto(dataRow["SERIE"].ToString(), i, 8, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["ESTATUS"].ToString(), i, 14, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["D.M."].ToString(), i, 16, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["R.T."].ToString(), i, 17, AutomatizadorExcel.HojaExcel);
                            objnuExcel.EscribeTexto(dataRow["VIGENCIA"].ToString(), i, 9, AutomatizadorExcel.HojaExcel);
                        }
                    }
                    catch (Exception ex)
                    {

                        objnu4.ReportarLog(log, ex.ToString());
                    }
                    i++;
                    try
                    {
                        //----REPUVE
                        //avisa si hay registro

                        var filtroSiniestro = from SelecDatos in ManejoDeDocumentos.DT_DATOS_REPUVE.AsEnumerable()
                                              where SelecDatos.Field<string>("NO SINIESTRO") == siniestro
                                              select SelecDatos;
                        if (filtroSiniestro.Count() > 0)
                        {
                            foreach (var item in filtroSiniestro)
                            {
                                dataRow = item;
                                if(!string.IsNullOrEmpty(dataRow["ANTECEDENTE"].ToString())&!string.IsNullOrWhiteSpace(dataRow["ANTECEDENTE"].ToString()))
                                    break;
                            }
                            //objnuExcel.EscribeTexto(dataRow["NO SINIESTRO"].ToString(), i, 3, AutomatizadorExcel.HojaExcel);

                            objnuExcel.EscribeTexto(dataRow["AÑO"].ToString(), i, 5, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["MARCA"].ToString(), i, 7, AutomatizadorExcel.HojaExcel);

                            //objnuExcel.EscribeTexto(dataRow["CLASE"].ToString(), i, 28, AutomatizadorExcel.HojaExcel);
                            //objnuExcel.EscribeTexto(dataRow["TIPO"].ToString(), i, 29, AutomatizadorExcel.HojaExcel);

                            //objnuExcel.EscribeTexto(dataRow["PLANTA"].ToString(), i, 30, AutomatizadorExcel.HojaExcel);
                            objnuExcel.EscribeTexto(dataRow["ANTECEDENTE"].ToString(), i, 31, AutomatizadorExcel.HojaExcel);
                        }
                    }
                    catch (Exception ex)
                    {

                        objnu4.ReportarLog(log, ex.ToString());
                    }

                    i++;
                    row++;

                }
                
            }
            else
            {

                row = 0;
                for (int i = 1; i <= ManejoDeDocumentos.DT_DATOS_SAT.Rows.Count*8; i++)
                {
                    i += 2;
                    j = 3;
                   

                    i++;
                    try
                    {

                        dataRow = ManejoDeDocumentos.DT_DATOS_SAT.Rows[row];

                        siniestro = dataRow["NO SINIESTRO"].ToString();
                        //llena la parte de las facturas
                        

                        //objnuExcel.EscribeTexto(dataRow["NO SINIESTRO"].ToString(), i, 3, AutomatizadorExcel.HojaExcel);
                        objnuExcel.EscribeTexto(dataRow["AÑO"].ToString(), i, 5, AutomatizadorExcel.HojaExcel);
                        //objnuExcel.EscribeTexto(dataRow["MOTOR"].ToString(), i, 10, AutomatizadorExcel.HojaExcel);
                        //objnuExcel.EscribeTexto(dataRow["MARCA"].ToString(), i, 11, AutomatizadorExcel.HojaExcel);
                        //objnuExcel.EscribeTexto(dataRow["PLACAS"].ToString(), i, 12, AutomatizadorExcel.HojaExcel);
                        objnuExcel.EscribeTexto(dataRow["SERIE"].ToString(), i, 8, AutomatizadorExcel.HojaExcel);
                        //objnuExcel.EscribeTexto(dataRow["VERSION"].ToString(), i, 24, AutomatizadorExcel.HojaExcel);
                    }
                    catch (Exception ex)
                    {
                        objnu4.ReportarLog(log, ex.ToString());

                    }

                    i++;
                    try
                    {
                        //---informacion de SAT
                        var filtroSiniestro = from SelecDatos in CaptchaSAT2.DT_DATOS_SAT2.AsEnumerable()
                                              where SelecDatos.Field<string>("NO SINIESTRO") == siniestro
                                              select SelecDatos;

                        foreach (var item in filtroSiniestro)
                        {
                            dataRow = item;
                            break;
                        }
                        //TOMAR INFORMACION DE FACTURAS
                        objnuExcel.EscribeTexto(dataRow["ESTADO CFDI"].ToString(), i, 10, AutomatizadorExcel.HojaExcel);
                        //objnuExcel.EscribeTexto(dataRow["EFECTO COMPROBANTE"].ToString(), i, 26, AutomatizadorExcel.HojaExcel);
                        //objnuExcel.EscribeTexto(dataRow["ESCANCELABLE"].ToString(), i, 27, AutomatizadorExcel.HojaExcel);
                    }
                    catch (Exception ex)
                    {
                        objnu4.ReportarLog(log, ex.ToString());

                    }
                    i++;
                    //llena polizas
                    try
                    {


                        var filtroSiniestro = from SelecDatos in ManejoDeDocumentos.Polizas6Col.AsEnumerable()
                                              where SelecDatos.Field<string>("NO SINIESTRO") == siniestro
                                              select SelecDatos;
                        foreach (var item in filtroSiniestro)
                        {
                            dataRow = item;
                            break;
                        }
                        //objnuExcel.EscribeTexto(dataRow["NO SINIESTRO"].ToString(), i, 3, AutomatizadorExcel.HojaExcel);
                        objnuExcel.EscribeTexto(dataRow["NPOLIZA"].ToString(), i, 4, AutomatizadorExcel.HojaExcel);
                        objnuExcel.EscribeTexto(dataRow["MODELO"].ToString(), i, 5, AutomatizadorExcel.HojaExcel);
                        objnuExcel.EscribeTexto(dataRow["MOTOR"].ToString(), i, 6, AutomatizadorExcel.HojaExcel);
                        //objnuExcel.EscribeTexto(dataRow["MARCA"].ToString(), i, 11, AutomatizadorExcel.HojaExcel);
                        //objnuExcel.EscribeTexto(dataRow["PLACAS"].ToString(), i, 12, AutomatizadorExcel.HojaExcel);
                        objnuExcel.EscribeTexto(dataRow["SERIE"].ToString(), i, 8, AutomatizadorExcel.HojaExcel);
                        //objnuExcel.EscribeTexto(dataRow["ESTATUS"].ToString(), i, 14, AutomatizadorExcel.HojaExcel);
                        //objnuExcel.EscribeTexto(dataRow["D.M."].ToString(), i, 16, AutomatizadorExcel.HojaExcel);
                        //objnuExcel.EscribeTexto(dataRow["R.T."].ToString(), i, 17, AutomatizadorExcel.HojaExcel);
                        objnuExcel.EscribeTexto(dataRow["VIGENCIA"].ToString(), i, 9, AutomatizadorExcel.HojaExcel);
                    }
                    catch (Exception ex)
                    {

                        objnu4.ReportarLog(log, ex.ToString());
                    }
                    i++;
                    try
                    {
                        //----REPUVE
                        //avisa si hay registro

                        var filtroSiniestro = from SelecDatos in CaptchaRepuve.DT_DATOS_REPUVE.AsEnumerable()
                                              where SelecDatos.Field<string>("NO SINIESTRO") == siniestro
                                              select SelecDatos;
                        foreach (var item in filtroSiniestro)
                        {
                            dataRow = item;
                            break;
                        }
                        //objnuExcel.EscribeTexto(dataRow["NO SINIESTRO"].ToString(), i, 3, AutomatizadorExcel.HojaExcel);

                        objnuExcel.EscribeTexto(dataRow["MODELO"].ToString(), i, 5, AutomatizadorExcel.HojaExcel);
                        objnuExcel.EscribeTexto(dataRow["MARCA"].ToString(), i, 7, AutomatizadorExcel.HojaExcel);

                        //objnuExcel.EscribeTexto(dataRow["CLASE"].ToString(), i, 28, AutomatizadorExcel.HojaExcel);
                        //objnuExcel.EscribeTexto(dataRow["TIPO"].ToString(), i, 29, AutomatizadorExcel.HojaExcel);

                        //objnuExcel.EscribeTexto(dataRow["PLANTA"].ToString(), i, 30, AutomatizadorExcel.HojaExcel);
                        //objnuExcel.EscribeTexto(dataRow["ANTECEDENTE"].ToString(), i, 31, AutomatizadorExcel.HojaExcel);

                    }
                    catch (Exception ex)
                    {

                        objnu4.ReportarLog(log, ex.ToString());
                    }

                    i++;
                    row++;

                }
                
            }
        }

        private void cambioSiniestro()
        {
            string siniestro = "";
            try
            {

                for (int i = 0; i < ManejoDeDocumentos.DT_DATOS_SAT.Rows.Count; i++)
                {
                    if (ManejoDeDocumentos.DT_DATOS_SAT.Rows[i]["NO SINIESTRO"].ToString().Length == 11) { 
                    siniestro = ManejoDeDocumentos.DT_DATOS_SAT.Rows[i]["NO SINIESTRO"].ToString();

                    siniestro = siniestro.Remove(0, 4);
                    ManejoDeDocumentos.DT_DATOS_SAT.Rows[i]["NO SINIESTRO"] = siniestro;
                }
            }
            }
            catch (Exception ex)
            {
                objnu4.ReportarLog(LOG, ex.ToString());
            }
            try
            {

            
            for (int i = 0; i < ManejoDeDocumentos.DT_DATOS_SAT2.Rows.Count; i++)
            {
                    if (ManejoDeDocumentos.DT_DATOS_SAT2.Rows[i]["NO SINIESTRO"].ToString().Length == 11)
                    {
                        siniestro = ManejoDeDocumentos.DT_DATOS_SAT2.Rows[i]["NO SINIESTRO"].ToString();

                        siniestro = siniestro.Remove(0, 4);
                        ManejoDeDocumentos.DT_DATOS_SAT2.Rows[i]["NO SINIESTRO"] = siniestro;
                    }
                    }
            }
            catch (Exception ex)
            {
                objnu4.ReportarLog(LOG, ex.ToString());
            }
            try
            {

           
            for (int i = 0; i < ManejoDeDocumentos.Polizas6Col.Rows.Count; i++)
            {
                    if (ManejoDeDocumentos.Polizas6Col.Rows[i]["NO SINIESTRO"].ToString().Length == 11)
                    {
                        siniestro = ManejoDeDocumentos.Polizas6Col.Rows[i]["NO SINIESTRO"].ToString();

                        siniestro = siniestro.Remove(0, 4);
                        ManejoDeDocumentos.Polizas6Col.Rows[i]["NO SINIESTRO"] = siniestro;
                    }
            }
            }
            catch (Exception ex)
            {

                objnu4.ReportarLog(LOG, ex.ToString());
            }
            try
            {

            
            for (int i = 0; i < ManejoDeDocumentos.DT_DATOS_REPUVE.Rows.Count; i++)
            {
                    if (ManejoDeDocumentos.DT_DATOS_REPUVE.Rows[i]["NO SINIESTRO"].ToString().Length == 11)
                    {
                        siniestro = ManejoDeDocumentos.DT_DATOS_REPUVE.Rows[i]["NO SINIESTRO"].ToString();

                        siniestro = siniestro.Remove(0, 4);
                        ManejoDeDocumentos.DT_DATOS_REPUVE.Rows[i]["NO SINIESTRO"] = siniestro;
                    }
            }
            }
            catch (Exception ex)
            {
                objnu4.ReportarLog(LOG, ex.ToString());
            }

        }

        //genera los encabezados en el reporte de excel
        private void encabezados()
        {
            List<string> encabezados = new List<string>();
            #region encabezados
            encabezados.Add("No.de Siniestro");
            //encabezados.Add("No.de Reporte");
            encabezados.Add("No.de Póliza");
            //encabezados.Add("Fecha de registro");
            //encabezados.Add("No. de Endoso");
            //encabezados.Add("No.de Inciso");
            encabezados.Add("Modelo");
            encabezados.Add("Motor");
            encabezados.Add("Marca");
            //encabezados.Add("Placas");
            encabezados.Add("Serie");
            //encabezados.Add("Estatus Póliza");
            //encabezados.Add("Reporte SAC");
            //encabezados.Add("Deducible DM");
            //encabezados.Add("Deducible RT");
            //encabezados.Add("T. Endoso");
            //encabezados.Add("F Emisión");
            encabezados.Add("Vigencia");
            //encabezados.Add("Prima");
            //encabezados.Add("Bon.Tec.");
            //encabezados.Add("Prima Total");
            //encabezados.Add("Versión ");
            encabezados.Add("Estatus SAT ");
            //encabezados.Add("Efecto del comprobante");
            //encabezados.Add(" Estatus CFDI ");
            //encabezados.Add("Clase");
            //encabezados.Add("Tipo");
            //encabezados.Add("Planta de Ensamble ");
            encabezados.Add("Antecedente");

            #endregion
            for (int i = 1; i<= SISE.Rows.Count*8; i++)
            {

                int j = 3;
                foreach (var item in encabezados)
                {
                    objnuExcel.EscribeTexto(item, i, j, AutomatizadorExcel.HojaExcel);
                    j++;

                }
                objnuExcel.FormatoNegrillaLetra("A"+i, "K" + i, AutomatizadorExcel.HojaExcel, 1);
                i++;
                objnuExcel.AsignarAltoFila(i.ToString(), 4, AutomatizadorExcel.HojaExcel);
                objnuExcel.TemaColorInteriorCelda(7, 0, "A" + i, "K" + i, AutomatizadorExcel.HojaExcel);
                i++;
                objnuExcel.EscribeTexto("SISE", i, 1, AutomatizadorExcel.HojaExcel);
                i++;
                objnuExcel.EscribeTexto("FACTURA", i, 1, AutomatizadorExcel.HojaExcel);
                i++;
                objnuExcel.EscribeTexto("SAT", i, 1, AutomatizadorExcel.HojaExcel);
                i++;
                objnuExcel.EscribeTexto("PÓLIZA", i, 1, AutomatizadorExcel.HojaExcel);
                i++;
                objnuExcel.EscribeTexto("REPUVE", i, 1, AutomatizadorExcel.HojaExcel);
                i++;
            }
            objnuExcel.AsignarAnchoColumna("B",0.40, AutomatizadorExcel.HojaExcel);
                objnuExcel.TemaColorInteriorCelda(7, 0, "B" + 1, "B" + SISE.Rows.Count*8, AutomatizadorExcel.HojaExcel);
            objnuExcel.FormatoNegrillaLetra("A1", "A55", AutomatizadorExcel.HojaExcel, 1);
        }
    }
}
