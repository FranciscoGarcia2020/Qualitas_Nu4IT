/*
 * Diana Miranda
 * 
 * esta clase toma el control de putty con AutoIt
 * envia comandos para obtener las consultas de sise
 * generando una tabla DATOS que contiene las consultas
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AutoIt;
using System.IO;
using Nu4it;
using System.ComponentModel;
using System.Data;
using nu4itFox;
using nu4itExcel;
namespace QPil.Procesos
{
    class UIAutomation
    {
        public usaR objNu4 = new usaR();
        public nufox objNuFox = new nufox();
        public nuExcel objnuExcel = new nuExcel();
        private Metodos tools = new Metodos();
        Procesos.QContent PQ = new Procesos.QContent();
        private Dlls.Nu4it objnu4it = new Dlls.Nu4it();
        private string[] ARRSTR_CONTENIDOINI;
        private string LOG;
        int ROW;
        public string SINIESTRO;
        public string POLIZA;
        public string ENDOSO;
        public string INCISO;


        public static DataTable DATOS { get; set; } = new DataTable();

        //crea un hilo para trabajar
        public void HiloDeEjecucion(string log)
        {
           
            LOG = log;
            objNu4.CreaArchivoLog(LOG);
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += FuncionPrincipalHILO;
            if (worker.IsBusy != true)
                worker.RunWorkerAsync();
            do
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));

            } while (worker.IsBusy == false);

        }

        //la funcion principal llamada por el hilo
        public void FuncionPrincipalHILO(object sender, DoWorkEventArgs e)
        {
            bool res = true;
            
                res = UIAutomationSise2();
            while (res == false)
            {
                //AutoItX.WinKill()
                res = UIAutomationSise2();
            } 

        }
     

        //entrega los datos de la tabla Datos al log
        private void informacionTablaSise()
        {
            string tabla;
            AutomatizadorExcel.PreparaInicioExcelNuevaInstancia();
            AutomatizadorExcel.ArchivoTrabajoExcel = objnuExcel.AbrirArchivoNuevo(AutomatizadorExcel.MiExcel);

            AutomatizadorExcel.HojaExcel = objnuExcel.ActivarPestaniaExcel(1, AutomatizadorExcel.MiExcel, AutomatizadorExcel.ArchivoTrabajoExcel);

            encabezados();
            tabla=objnu4it.ConvierteDTaSTRING(DATOS);
            objnu4it.clipboardAlmacenaTexto(tabla);
            objnuExcel.PegarPortaPapelesRango("A2", AutomatizadorExcel.HojaExcel);
            
        }

        private void encabezados()
        {
            List<string> encabezados = new List<string>();
            #region encabezados
            encabezados.Add("No.de Siniestro");
            encabezados.Add("No.de Reporte");
            encabezados.Add("No.de Póliza");
            encabezados.Add("Fecha de registro");
            encabezados.Add("No. de Endoso");
            encabezados.Add("No.de Inciso");
            encabezados.Add("Modelo");
            encabezados.Add("Motor");
            encabezados.Add("Marca");
            encabezados.Add("Placas");
            encabezados.Add("Serie");
            encabezados.Add("Estatus Póliza");
            encabezados.Add("Reporte SAC");
            encabezados.Add("Deducible DM");
            encabezados.Add("Deducible RT");
            encabezados.Add("T. Endoso");
            encabezados.Add("F Emisión");
            encabezados.Add("Vigencia");
            encabezados.Add("Prima");
            encabezados.Add("Bon.Tec.");
            encabezados.Add("Prima Total");


            #endregion
            int j = 1;
                foreach (var item in encabezados)
                {
                    objnuExcel.EscribeTexto(item, 1, j, AutomatizadorExcel.HojaExcel);
                    j++;

                }
                objnuExcel.FormatoNegrillaLetra("A" + 1, "AE" + 1, AutomatizadorExcel.HojaExcel, 1);
                
            
        }


        //maneja las primas pendientes
        private void infoOperaciones(List<string> resultado)
        {
            string aux;
            string remesa, importe, recibo;
            int index;
            objNu4.ReportarLog(LOG, "ingreso a infoOperaciones");
           
            try
            {

                //Primas Pendientes
                index = resultado.FindIndex(
                delegate (string current)
                {
                    return current.Contains("Importe  Recibo");
                }
                );

                do
                {
                    index++;
                    aux = resultado[index];
                    remesa = aux.Remove(0, 43);
                    remesa = remesa.Remove(10);
                    if (remesa == "9999999999" || remesa == "         ")
                    {

                        aux = aux.Remove(0, 11);
                        importe = aux.Remove(9);
                        recibo = aux.Remove(0, 12);
                        recibo = recibo.Remove(9);
                        aux = DATOS.Rows[ROW]["Primas pendientes"].ToString() + "Importe " + importe + " recibo " + recibo;
                        DATOS.Rows[ROW]["Primas pendientes"] = aux;
                        objNu4.ReportarLog(LOG, "Primas Pendientes " + aux);
                    }
                    aux = resultado[index];
                } while (!aux.Contains("Prima Neta"));

                if (string.IsNullOrEmpty(DATOS.Rows[ROW]["Primas pendientes"].ToString()))
                {
                    DATOS.Rows[ROW]["Primas pendientes"] = "Sin pendientes";
                    objNu4.ReportarLog(LOG, "Primas Pendientes: sin pendientes");
                }


            }
            catch (Exception ex)
            {

                objNu4.ReportarLog(LOG, ex.ToString());
            }
          
            try
            {
                aux = HerramientasGral.UbicadoList(resultado, "Bon.Tec.:");
                aux = objNuFox.StrExtract(aux, "Bon.Tec.: ", "% R.");
                DATOS.Rows[ROW]["Bon.Tec."] = aux;
                objNu4.ReportarLog(LOG, "Bon.Tec. " + aux);
            }
            catch (Exception ex)
            {
                objNu4.ReportarLog(LOG, ex.ToString());
            }
        }

        //maneja la informacion de la consulta Administracion Siniestro
        public void infoAdmiSiniestro(List<string> salida, List<string> reporte, List<string> continuaReporte, List<string> servicio)
        {
            objNu4.ReportarLog(LOG, "inicia la consulta infoAdmiSiniestro");
            string aux, inciso, modelo;
            int index;

            DATOS.Rows.Add();
            ROW = DATOS.Rows.Count - 1;
            try
            {

                //aux = HerramientasGral.UbicadoList(salida, "Siniestro :");
                //aux = objNuFox.StrExtract(aux, ": ", "", 5);
                DATOS.Rows[ROW]["Siniestro"] = SINIESTRO;
                objNu4.ReportarLog(LOG, "sinietro " + SINIESTRO);

                //reporte
                aux = HerramientasGral.UbicadoList(salida, "Numero de Poliza");
                aux = objNuFox.StrExtract(aux, ": ", " ");
                DATOS.Rows[ROW]["Reporte"] = aux;
                objNu4.ReportarLog(LOG, "reporte " + aux);
                //datosDiferentes(salida,"Reporte", "No. de Reporte:", "No. de Reporte:", "",row);

                //poliza
                aux = HerramientasGral.UbicadoList(salida, "Numero de Endoso");
                aux = objNuFox.StrExtract(aux, ": ", " ");
                DATOS.Rows[ROW]["Poliza"] = aux;
                POLIZA = aux;
                objNu4.ReportarLog(LOG, "poliza" + aux);
                //datosDiferentes(salida, "Poliza", "Poliza", ": ", " ", row);

                //fecha de registro
                aux = HerramientasGral.UbicadoList(salida, "Numero de Endoso");
                aux = objNuFox.StrExtract(aux, "                       ");
                DATOS.Rows[ROW]["Fecha de registro"] = aux;
                objNu4.ReportarLog(LOG, "fecha de registro " + aux);

                //endoso
                aux = HerramientasGral.UbicadoList(salida, "Numero de Inciso");
                aux = objNuFox.StrExtract(aux, ": ");
                DATOS.Rows[ROW]["Endoso"] = aux;
                ENDOSO = aux;
                objNu4.ReportarLog(LOG, "endoso " + aux);

                //inciso
                index = salida.FindLastIndex(
                delegate (string current)
                {
                    return current.Contains("Numero de Inciso");
                }
                );
                aux = salida[index + 1];
                inciso = objNuFox.StrExtract(aux, "Agente         : ", " ");

                DATOS.Rows[ROW]["Inciso"] = inciso;
                INCISO = inciso;
                objNu4.ReportarLog(LOG, "inciso " + inciso);

                //modelo
                modelo = objNuFox.StrExtract(aux, "Mod.:", " ");
                DATOS.Rows[ROW]["Modelo"] = modelo;
                objNu4.ReportarLog(LOG, "modelo " + modelo);

                //motor
                aux = objNuFox.StrExtract(aux, "Motor:");
                DATOS.Rows[ROW]["Motor"] = aux;
                objNu4.ReportarLog(LOG, "motor " + aux);

                //vigencia
                
                aux = salida.FindLast(
                     delegate (string current)
                     {
                         return current.Contains("Vigencia");
                     }
                );
                //aux = HerramientasGral.UbicadoList(salida, "Vigencia");
                aux = objNuFox.StrExtract(aux, "Moneda         : ");
                DATOS.Rows[ROW]["Vigencia"] = aux;
                objNu4.ReportarLog(LOG, "vigencia " + aux);

                //fecha ocurrido
                aux = HerramientasGral.UbicadoList(salida, "Fecha reclamo");
                //aux = HerramientasGral.UbicadoList(salida, "Fecha ocurrido");
                aux = objNuFox.StrExtract(aux, ": ", " ");
                DATOS.Rows[ROW]["Fecha Ocurrido"] = aux;
                objNu4.ReportarLog(LOG, "fecha ocurrido " + aux);
                //datosDiferentes(salida, "Fecha Ocurrido", "F.Ocurr", "F.Ocurr: ", " ",row);


                //inicia reporte
                objNu4.ReportarLog(LOG, "inicia informacion desde la lista reporte");
                //asegurado                                          
                aux = HerramientasGral.UbicadoList(reporte, "7.Asegurado:");
                aux = objNuFox.StrExtract(aux, ": ");
                aux = aux.Replace("Poliza Vencida", "");
                DATOS.Rows[ROW]["Asegurado"] = aux;
                objNu4.ReportarLog(LOG, "asegurado " + aux);


                //serie
                aux = reporte.FindLast(
                    delegate (string current)
                    {
                        return current.Contains("12.Serie:");
                    }
               );
                //aux = HerramientasGral.UbicadoList(reporte, "Serie");
                aux = objNuFox.StrExtract(aux, "12.Serie:  ", " ", 1);
                DATOS.Rows[ROW]["Serie"] = aux;
                objNu4.ReportarLog(LOG, "serie " + aux);

                //placas
                aux = HerramientasGral.UbicadoList(reporte, "Placas:");
                aux = objNuFox.StrExtract(aux, ": ", " ", 2);
                DATOS.Rows[ROW]["Placas"] = aux;
                objNu4.ReportarLog(LOG, "placas " + aux);

                //c.ocurrio
                aux = HerramientasGral.UbicadoList(reporte, "C.ocurrio:");
                aux = objNuFox.StrExtract(aux, ": ");
                DATOS.Rows[ROW]["Como ocurrio"] = aux;
                objNu4.ReportarLog(LOG, "como ocurrio " + aux);

                //Fecha de reporte

                aux = HerramientasGral.UbicadoList(reporte, "1.Fec. Rep.:");
                aux = objNuFox.StrExtract(aux, ": ", "2.");
                DATOS.Rows[ROW]["Fecha Reporte"] = aux;
                objNu4.ReportarLog(LOG, "Fecha Reporte " + aux);


                //marca
                aux = HerramientasGral.UbicadoList(reporte, "10.Marca Veh:");

                aux = objNuFox.StrExtract(aux, "10.Marca Veh: ", "11.Modelo");
                DATOS.Rows[ROW]["Marca"] = aux;
                objNu4.ReportarLog(LOG, "marca " + aux);



                //continua reporte
                objNu4.ReportarLog(LOG, "se buscan los datos en la lista continuaReporte");

                //reporte sac
                aux = HerramientasGral.UbicadoList(continuaReporte, "Reporte SAC:");
                aux = objNuFox.StrExtract(aux, "Reporte SAC:");
                DATOS.Rows[ROW]["Reporte SAC"] = aux;
                objNu4.ReportarLog(LOG, "reporte sac " + aux);

                //cobertura
                index = continuaReporte.FindIndex(
                delegate (string current)
                {
                    return current.Contains("Cobertura");
                }
                );
                aux = continuaReporte[index + 1];
                aux = objNuFox.StrExtract(aux, " ", " ");
                DATOS.Rows[ROW]["Cobertura"] = aux;
                objNu4.ReportarLog(LOG, "cobertura " + aux);

                //deducible DM
                index = continuaReporte.FindIndex(
                    delegate (string current)
                    {
                        return current.Contains("D.M.");
                    }
                    );
                aux = continuaReporte[index];
                inciso = aux;//la variable inciso se usa en este caso como auxiliar, ya termino su utilidad
                aux = objNuFox.StrExtract(aux, "D.M.  :  ", "R.T.");
                DATOS.Rows[ROW]["Deducible D.M."] = aux;
                objNu4.ReportarLog(LOG, "DM " + aux);
                //deducible RT
                aux = objNuFox.StrExtract(inciso, ": ", "R.C.", 2);
                DATOS.Rows[ROW]["Deducible R.T."] = aux;
                objNu4.ReportarLog(LOG, "RT " + aux);


                //comentario
                aux = HerramientasGral.UbicadoList(servicio, "11.Comentar.:");
                aux = objNuFox.StrExtract(aux, ": ");
                DATOS.Rows[ROW]["Comentario"] = aux;
                objNu4.ReportarLog(LOG, "comentario " + aux);


                //estatus de la poliza
                aux = HerramientasGral.UbicadoList(reporte, "7.Asegurado:");
                if (aux.Contains("Poliza"))
                {

                    DATOS.Rows[ROW]["Estatus Poliza"] = "Poliza Vencida";
                    objNu4.ReportarLog(LOG, "Estatus de la Poliza: Poliza Vencida ");
                }
                else
                {
                    DATOS.Rows[ROW]["Estatus Poliza"] = "Poliza Vigente";
                    objNu4.ReportarLog(LOG, "Estatus de la Poliza: Poliza Vigente ");
                }
            }
            catch (Exception ex)
            {

                objNu4.ReportarLog(LOG, ex.ToString());
            }

        }

        private void datosDiferentes(List<string> salida, string columna, string dato, string limiteIni, string limiteFin, int row)
        {
            List<string> resultFindAll;
            //buscamos el dato tantas veces como aparecen en la consulta
            resultFindAll = salida.FindAll(
            delegate (string current)
            {
                return current.Contains(dato);
            }
            );
            //los limpiamos para tener solo el dato que requerimos
            for (int i = 0; i < resultFindAll.Count; i++)
            {
                resultFindAll[i] = objNuFox.StrExtract(resultFindAll[i], limiteIni, limiteFin);
            }
            //si tenemos mas de uno revisamos si se duplican
            if (resultFindAll.Count > 1)
            {

                for (int i = 0; i < resultFindAll.Count; i++)
                {
                    //obtiene los elementos iguales al actual
                    List<string> resultFindAll2 = resultFindAll.FindAll(
                    delegate (string current)
                    {
                        return current.Contains(resultFindAll[i]);
                    }
                    );
                    //si se obtuvo un elemento es justo el que tenemos, si son mas estan repetidos, borramos el que tenemos
                    //los que siguen seran borrados en su iteracion si no son unicos
                    if (resultFindAll2.Count > 1)
                    {
                        resultFindAll.RemoveAt(i);
                    }
                }
            }
            //ya que tenemos los elementos pasamos a compararlos con el primer registro
            //si son diferentes creamos una nueva fila con siniestro y registro
            foreach (var item in resultFindAll)
            {

                if (DATOS.Rows[row]["Reporte"].ToString() != item)
                {
                    DATOS.Rows.Add();
                    int rowsAux = DATOS.Rows.Count - 1;
                    DATOS.Rows[rowsAux]["Siniestro"] = SINIESTRO;
                    DATOS.Rows[rowsAux]["Reporte"] = item;

                }
            }
        }

        //maneja la informacion de la consulta de poliza
        public void infoPoliza(List<string> salida)
        {


            string aux;
            objNu4.ReportarLog(LOG, "ingreso a infoPoliza");
            try
            {

                //emision
                aux = HerramientasGral.UbicadoList(salida, "F.Emision:");
                aux = objNuFox.StrExtract(aux, "F.Emision: ", " ");

                DATOS.Rows[ROW]["Fecha Emision"] = aux;

                objNu4.ReportarLog(LOG, "Fecha Emision " + aux);

            }
            catch (Exception ex)
            {

                objNu4.ReportarLog(LOG, ex.ToString());
            }
        }

        //maneja la informacion de la consulta Operaciones
        public void infoOperaciones2(List<string> salida)
        {

            string aux;
            objNu4.ReportarLog(LOG, "ingresa a infoOperaciones2");
            //codigo para debug
            //string resultadoAux;
            //resultadoAux = string.Join("\r\n", salida.ToArray());
            //System.Windows.Forms.MessageBox.Show("A consulta infoOperaciones2 entra \r\n" + resultadoAux);
            try
            {

                //suma asegurada
                aux = HerramientasGral.UbicadoList(salida, "Suma Aseg. :");
                aux = objNuFox.StrExtract(aux, "Suma Aseg. : ");

                DATOS.Rows[ROW]["Suma asegurada"] = aux;

                objNu4.ReportarLog(LOG, "Suma asegurada " + aux);

                //tipo endoso

                int index = salida.FindIndex(
              delegate (string current)
              {
                  return current.Contains("R./SR");
              }
          );
                index += 2;
                aux = salida[index];
                aux = aux.Remove(0, 58);

                DATOS.Rows[ROW]["Tipo de Endoso"] = aux;

                objNu4.ReportarLog(LOG, "Tipo de Endoso " + aux);

                //Prima Total
                aux = HerramientasGral.UbicadoList(salida, "Prima Total: ");
                aux = objNuFox.StrExtract(aux, "Prima Total: ", "Rec.Finan.:");

                DATOS.Rows[ROW]["Prima Total"] = aux;

                objNu4.ReportarLog(LOG, "Prima Total " + aux);

                //Prima 
                aux = HerramientasGral.UbicadoList(salida, "Prima      : ");
                aux = objNuFox.StrExtract(aux, "Prima      : ", "% I.V.A.  : ");

                DATOS.Rows[ROW]["Prima"] = aux;

                objNu4.ReportarLog(LOG, "Prima" + aux);
            }
            catch (Exception ex)
            {

                objNu4.ReportarLog(LOG, ex.ToString());
            }
        }

        //maneja la informacion de la consulta Coberturas de Polizas/Endosos
        public void infoCoberturas(List<string> resultado)
        {
            //codigo debug
            //string resultadoAux;
            //resultadoAux = string.Join("\r\n", resultado.ToArray());
            //System.Windows.Forms.MessageBox.Show("A consulta infoCobertura entra \r\n" +resultadoAux);
            string aux;
            objNu4.ReportarLog(LOG, "ingresa a infoCoberturas");
            try
            {
                //suma asegurada Cobertura
                aux = HerramientasGral.UbicadoList(resultado, "Pma. E.E.");
                aux = objNuFox.StrExtract(aux, "Suma.Aseg.: ");
                DATOS.Rows[ROW]["Suma.Aseg Cober"] = aux;
                objNu4.ReportarLog(LOG, "Suma.Aseg Cober " + aux);

                //suma RC 
                aux = HerramientasGral.UbicadoList(resultado, "Suma R.C. : ");
                aux = objNuFox.StrExtract(aux, "Suma R.C. : ");
                DATOS.Rows[ROW]["Suma R.C."] = aux;
                objNu4.ReportarLog(LOG, "Suma R.C. " + aux);

                //suma GM
                aux = HerramientasGral.UbicadoList(resultado, "Suma G.M. : ");
                aux = objNuFox.StrExtract(aux, "Suma G.M. : ");
                DATOS.Rows[ROW]["Suma G.M."] = aux;
                objNu4.ReportarLog(LOG, "Suma G.M. " + aux);

                //suma EE 
                aux = HerramientasGral.UbicadoList(resultado, "Suma E.E. : ");
                aux = objNuFox.StrExtract(aux, "Suma E.E. : ");
                DATOS.Rows[ROW]["Suma E.E."] = aux;
                objNu4.ReportarLog(LOG, "Suma E.E. " + aux);

                //suma ADAP
                aux = HerramientasGral.UbicadoList(resultado, "Suma ADAP : ");
                aux = objNuFox.StrExtract(aux, "Suma ADAP : ");
                DATOS.Rows[ROW]["Suma ADAP"] = aux;
                objNu4.ReportarLog(LOG, "Suma ADAP " + aux);

                //Suma Aj.Au.
                aux = HerramientasGral.UbicadoList(resultado, "Suma Aj.Au: ");
                aux = objNuFox.StrExtract(aux, "Suma Aj.Au: ");
                DATOS.Rows[ROW]["Suma Aj.Au."] = aux;
                objNu4.ReportarLog(LOG, "Suma Aj.Au. " + aux);

                //Suma G.L. 
                aux = HerramientasGral.UbicadoList(resultado, "Suma G.L. : ");
                aux = objNuFox.StrExtract(aux, "Suma G.L. : ");
                DATOS.Rows[ROW]["Suma G.L."] = aux;
                objNu4.ReportarLog(LOG, "Suma G.L. " + aux);

                //Suma AV. 
                aux = HerramientasGral.UbicadoList(resultado, "uma A.V. : ");
                aux = objNuFox.StrExtract(aux, "uma A.V. : ");
                DATOS.Rows[ROW]["Suma A.V."] = aux;
                objNu4.ReportarLog(LOG, "Suma A.V. " + aux);

            }
            catch (Exception ex)
            {

                objNu4.ReportarLog(LOG, ex.ToString());
            }
        }

        //creacion de la estructura de la tabla para sise
        private void TablaSise()
        {

            List<string> titulos = new List<string>();

            titulos.Add("Siniestro");
            titulos.Add("Reporte");
            titulos.Add("Poliza");
            titulos.Add("Fecha de registro");
            titulos.Add("Endoso");
            titulos.Add("Inciso");
            titulos.Add("Modelo");
            titulos.Add("Motor");
            titulos.Add("Marca");
            titulos.Add("Placas");
            titulos.Add("Serie");
            titulos.Add("Estatus Poliza");
            titulos.Add("Reporte SAC");
            titulos.Add("Deducible D.M.");
            titulos.Add("Deducible R.T.");
            titulos.Add("Tipo de Endoso");
            titulos.Add("Fecha Emision");
            titulos.Add("Vigencia");
            titulos.Add("Prima");
            titulos.Add("Bon.Tec.");
            titulos.Add("Prima Total");

            titulos.Add("Suma asegurada");

            titulos.Add("Fecha Ocurrido");
            titulos.Add("Asegurado");
            //Consulta de control de siniestros

            titulos.Add("Fecha Reporte");
            titulos.Add("Como ocurrio");

            titulos.Add("Cobertura");
            titulos.Add("Comentario");
            titulos.Add("Primas pendientes");//No. de recibos que no tienen detalle(validar)
                                             //consulta de coberturas de poliza/endoso
            titulos.Add("Suma.Aseg Cober");
            titulos.Add("Suma R.C.");
            titulos.Add("Suma G.M.");
            titulos.Add("Suma E.E.");
            titulos.Add("Suma ADAP");
            titulos.Add("Suma Aj.Au.");
            titulos.Add("Suma G.L.");
            titulos.Add("Suma A.V.");

            DATOS = HerramientasGral.CrearEstructuraDT(titulos);

        }

        //TRATA DE MEJORAR EL RENDIMIENTO EN UN DIA SIN REPOSITORIO
        public bool UIAutomationSise2()
        {

            bool exito = true;
            //List<string> resultado = new List<string>();//resultado limpio para enviar a las consultas
            //List<string> resultado2 = new List<string>();//contiene el resultado como lo trae el clipboard
            //List<string> listAux = new List<string>();
            //string resultadoAux = "";

            //List<string> comandos = new List<string>();
            //List<string> siniestros = new List<string>();

            //String IP = "";
            //String PORT = "";
            //String USER = "";
            //String PASS = "";

            //string usuario = "";
            //string password = "";
            //string siniestro = "";
            //string direccion = Directory.GetCurrentDirectory() + @"\";
            //string oficina = "";
            //string ramo = "";
            //string ejercicio = "";



            //objnu4it.ReportarLog(LOG, direccion);
            //#region datos INI

            ////ARRSTR_CONTENIDOINI = objNu4.LeerArchivoIni("QPil");
            ////ARRSTR_CONTENIDOINI = ARRSTR_CONTENIDOINI.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            ////IP = ARRSTR_CONTENIDOINI[ objNu4.UbicadoEnPos(ARRSTR_CONTENIDOINI, "ip", 1)];
            ////IP = IP.Remove(0, 3);
            //IP = "110.10.0.11";
            ////PORT =  ARRSTR_CONTENIDOINI[objNu4.UbicadoEnPos(ARRSTR_CONTENIDOINI, "PUERTO", 1)];
            ////PORT = PORT.Replace("PORT=", "");
            //PORT = "22";
            ////USER = ARRSTR_CONTENIDOINI[objNu4.UbicadoEnPos(ARRSTR_CONTENIDOINI, "user", 1)];
            ////USER = USER.Replace("USER=", "");
            //USER = "sincab";
            ////PASS =ARRSTR_CONTENIDOINI[objNu4.UbicadoEnPos(ARRSTR_CONTENIDOINI, "pass", 1)];
            ////PASS = PASS.Replace("PASS=", "");\
            //PASS = "QSc4b-18";
            ////usuario = ARRSTR_CONTENIDOINI[objNu4.UbicadoEnPos(ARRSTR_CONTENIDOINI, "usuario", 1)];
            ////usuario = usuario.Replace("USUARIO=", "");
            //usuario = "cvalverde";
            ////password = ARRSTR_CONTENIDOINI[objNu4.UbicadoEnPos(ARRSTR_CONTENIDOINI, "password", 1)];
            ////password = password.Replace("PASSWORD=", "");
            //password = "Valverde/3";
            ////oficina = ARRSTR_CONTENIDOINI[objNu4.UbicadoEnPos(ARRSTR_CONTENIDOINI, "OFICINA", 1)];
            ////oficina = oficina.Replace("OFICINA=", "");
            //oficina = "010";
            //#endregion
            ////traemos los siniestros desde un archivo
            //siniestros = AutomatizadorExcel.Siniestros(LOG);
            ////genera la estructura de la tabla
            //TablaSise();

            //try
            //{

            //    /// Inicio de aplicacion putty
            //    AutoIt.AutoItX.Run("putty.exe", direccion);
            //    objNu4.ReportarLog(LOG, "conectando putty alojado en " + direccion);
            //    Thread.Sleep(500);
            //    AutoItX.WinActivate("PuTTY Configuration");
            //    if (AutoItX.WinWaitActive("PuTTY Configuration", "", 10) != 0)
            //    {
            //        /// Inicio de conexion
            //        AutoIt.AutoItX.Send(IP);
            //        AutoItX.Send("{TAB}");
            //        AutoIt.AutoItX.Send(PORT);
            //        AutoItX.Send("{ENTER}");
            //        AutoItX.WinActivate(IP + " - PuTTY");
            //        objNu4.ReportarLog(LOG, "obteniendo putty " + AutoItX.WinActive().ToString());
            //        var handle = AutoItX.WinGetHandle(IP + " - PuTTY");

            //        //log en el servidor
            //        if (AutoItX.WinWaitActive(IP + " - PuTTY", "", 3) != 0)
            //        {
            //            validar("login as:", IP);

            //            AutoItX.WinActivate(IP + " - PuTTY", "");
            //            AutoIt.AutoItX.Send(USER);
            //            AutoItX.Send("{ENTER}");
            //            objNu4.ReportarLog(LOG, "Ingresando usuario " + USER);

            //            validar(USER + "@" + IP + "'s password:", IP);

            //            AutoItX.WinActivate(IP + " - PuTTY", "");
            //            AutoIt.AutoItX.Send(PASS);
            //            AutoItX.Send("{ENTER}");
            //            objNu4.ReportarLog(LOG, "Ingresando password " + PASS);

            //            validar("Ingrese Codigo de Usuario :", IP);

            //            AutoItX.WinActivate(IP + " - PuTTY", "");
            //            AutoIt.AutoItX.Send(usuario);
            //            AutoItX.Send("{ENTER}");
            //            objNu4.ReportarLog(LOG, "ingresa usuario para sise " + usuario);

            //            Thread.Sleep(1000);
            //            do
            //            {
            //                resultado = copiar(IP);

            //                //si no encuentra la cadena regresa null
            //                resultadoAux = HerramientasGral.UbicadoList(resultado, "Sesiones abiertas");
            //                if (resultadoAux == null)
            //                    resultadoAux = HerramientasGral.UbicadoList(resultado, "Ingrese su Password :");
            //            } while (resultadoAux == null);
            //            objNu4.ReportarLog(LOG, resultadoAux);

            //            //cierra las sesiones abiertas previamente
            //            if (resultadoAux.Contains("Sesiones abiertas"))
            //            {
            //                AutoItX.WinActivate(IP + " - PuTTY", "");
            //                AutoItX.Send("n");
            //                AutoItX.Send("{ENTER}");
            //                objNu4.ReportarLog(LOG, "comando n");
            //                validar("Ingrese su Password :", IP);

            //                AutoItX.WinActivate(IP + " - PuTTY", "");
            //                AutoIt.AutoItX.Send(password);
            //                AutoItX.Send("{ENTER}");
            //                objNu4.ReportarLog(LOG, "ingresa la contraseña " + password);

            //                #region cerrarSesiones
            //                ////valida la entrada del password para poder pasar a cerrar las sesiones
            //                //do
            //                //{
            //                //    objNu4.ReportarLog(LOG, "entro al do while");
            //                //    Thread.Sleep(3000);
            //                //    AutoItX.WinActivate(IP + " - PuTTY", "");
            //                //    AutoIt.AutoItX.Send(password);
            //                //    AutoItX.Send("{ENTER}");
            //                //    objNu4.ReportarLog(LOG, password);

            //                //   resultado=copiar(IP);

            //                //    resultadoAux = "";
            //                //    //si no encuentra la cadena regresa null
            //                //    resultadoAux = HerramientasGral.UbicadoList(resultado, "PASSWD");
            //                //    objNu4.ReportarLog(LOG, resultadoAux);
            //                //} while (resultadoAux != null);

            //                ////revisa que se encuentre en la ventana de lista de sesiones

            //                //do
            //                //{
            //                //    Thread.Sleep(1000);
            //                //    AutoItX.WinActivate(IP + " - PuTTY", "");
            //                //    AutoItX.Send("1");
            //                //    objnu4it.ReportarLog(LOG, "comando 1 {31} en ascii");

            //                //    resultado=copiar(IP);
            //                //    resultadoAux = "";
            //                //    resultadoAux = HerramientasGral.UbicadoList(resultado, "Lista de Sesiones a Liberar");
            //                //    objNu4.ReportarLog(LOG, resultadoAux);
            //                //} while (resultadoAux != null) ;
            //                #endregion
            //            }
            //            if (resultadoAux.Contains("Ingrese su Password :"))
            //            {
            //                AutoItX.WinActivate(IP + " - PuTTY", "");
            //                AutoIt.AutoItX.Send(password);
            //                AutoItX.Send("{ENTER}");
            //                objNu4.ReportarLog(LOG, "ingresa la contraseña " + password);
            //            }


            //            //inicia el ingreso de los comandos
            //            #region comandos de inicio
            //            validar("*** Cabina ***", IP);
            //            AutoItX.Send("f");//salir de menu
            //            AutoItX.Send("{ENTER}");
            //            do
            //            {
            //                resultado = copiar(IP);
            //                listAux = resultado.FindAll(
            //                    delegate (string current)
            //                    {
            //                        return current.Contains(">");
            //                    }
            //                    );
            //            } while (listAux.Count != 2);

            //            AutoItX.Send("m");//confirmar salida de menu
            //            AutoItX.Send("{ENTER}");

            //            validar("3. Siniestros", IP);
            //            AutoItX.Send("3");//opcion 3 siniestros
            //            AutoItX.Send("{ENTER}");
            //            validar("** S I N I E S T R O S **", IP);
            //            AutoItX.Send("2");//opcion 2  administracion de siniestros 
            //            AutoItX.Send("{ENTER}");
            //            #endregion

            //            //revision de siniestro
            //            foreach (var item in siniestros)
            //            {
            //                objNu4.ReportarLog(LOG, "Trabajando con siniestro " + item);
            //                if (item.Length == 11)
            //                {
            //                    //se concidera que el siniestro viene en tipo string
            //                    siniestro = item;
            //                    ramo = siniestro;
            //                    ramo = ramo.Remove(2);
            //                    ejercicio = siniestro;
            //                    ejercicio = ejercicio.Remove(0, 2);
            //                    ejercicio = ejercicio.Remove(2);
            //                    siniestro = siniestro.Remove(0, 4);
            //                    pantallas(siniestro, ramo, ejercicio, IP, oficina);
            //                }
            //                else if (item.Length == 10)
            //                {
            //                    //se concidera que el siniestro viene en tipo numero en el excel
            //                    siniestro = item;
            //                    ramo = siniestro;
            //                    ramo = "0" + ramo.Remove(1);
            //                    ejercicio = siniestro;
            //                    ejercicio = ejercicio.Remove(0, 1);
            //                    ejercicio = ejercicio.Remove(2);
            //                    siniestro = siniestro.Remove(0, 3);
            //                    pantallas(siniestro, ramo, ejercicio, IP, oficina);
            //                }
            //                else if (item.Length < 7)
            //                {
            //                    tools.MessageShowOK_2("El valor de siniestro " + item + " es menor del requerido", "ERROR");
            //                    objNu4.ReportarLog(LOG, "El valor de siniestro " + item + " es menor del requerido");
            //                }


            //            }
            //            Thread.Sleep(TimeSpan.FromSeconds(1));
            //            AutoItX.WinActivate(IP + " - PuTTY", "");
            //            AutoItX.WinClose(handle);
            //            try
            //            {
            //                AutoItX.WinKill(IP + " - PuTTY", "");
            //            }
            //            catch (Exception ex)
            //            {

            //                objNu4.ReportarLog(LOG, ex.ToString());
            //            }
            //            objNu4.ReportarLog(LOG, "Sise proceso finalizado con exito");
            //            tools.MessageShowOK_1("PROCESO FINALIZADO CON EXITO");
            //        }
            //        else
            //        {
            //            objNu4.ReportarLog(LOG, "la ventanana " + IP + " - PuTTY no se activo");
            //            exito = false;
            //        }
            //    }
            //    else
            //    {
            //        objNu4.ReportarLog(LOG, "la ventana PuTTY configuration no se activo");
            //        exito = false;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    objNu4.ReportarLog(LOG, "excepcion " + ex.ToString());
            //    exito = false;
            //}
            //tools.MessageShowOK("PROCESO FINALIZADO CON EXITO", "ALERTA");
            //tools.MessageShowOK("PROCESO FINALIZADO CON EXITO", "OK");

            // informacionTablaSise();
           


           
            PQ.ContentNavigate(LOG);//descomentar para prueba piloto
            Procesos.Reporte objPR = new Procesos.Reporte();
            objPR.generaReporte(LOG);
            return exito;
        }

        //valida la aparicion de cambio en la pantalla de putty para saber que podemos ingresar un dato
        private void validar(string cambio, string IP)
        {
            string resultadoAux;
            List<string> resultado = new List<string>();
            do
            {
                resultado = copiar(IP);
                resultadoAux = HerramientasGral.UbicadoList(resultado, cambio);
            } while (resultadoAux == null);
            objNu4.ReportarLog(LOG, resultadoAux);

        }

        //valida que los datos de la consulta han aparecido
        private List<string> validarConsulta(string cambio, string IP)
        {
            string resultadoAux;
            List<string> resultado = new List<string>();
            do
            {
                resultado = copiar(IP);
                resultadoAux = HerramientasGral.UbicadoList(resultado, cambio);
                resultadoAux = resultadoAux.Replace(cambio, "");
            } while (string.IsNullOrEmpty(resultadoAux) || string.IsNullOrWhiteSpace(resultadoAux));
            objNu4.ReportarLog(LOG, resultadoAux);
            return resultado;
        }

        //valida que los datos de la consulta han aparecido
        private List<string> validarConsulta(string cambio, string fin, string IP)
        {
            string resultadoAux;
            List<string> resultado = new List<string>();
            do
            {
                resultado = copiar(IP);
                resultadoAux = HerramientasGral.UbicadoList(resultado, cambio);
                resultadoAux = objNuFox.StrExtract(resultadoAux, cambio, fin);
            } while (string.IsNullOrEmpty(resultadoAux) || string.IsNullOrWhiteSpace(resultadoAux));
            objNu4.ReportarLog(LOG, resultadoAux);
            return resultado;
        }

        //envia los comandos a putty y obtiene las pantallas
        private void pantallas(string siniestro, string ramo, string ejercicio, string IP, string oficina)
        {
            List<string> resultado = new List<string>();
            //en esta parte reinicia

            validar("8. Consulta de Siniestros", IP);
            AutoItX.WinActivate(IP + " - PuTTY", "");
            AutoIt.AutoItX.Send("8");//consulta siniestros
            AutoItX.Send("{ENTER}");
            objNu4.ReportarLog(LOG, "ingresando 8 consulta de siniestros");
            validar("9. Consulta de Reclamos", IP);
            AutoItX.WinActivate(IP + " - PuTTY", "");
            AutoIt.AutoItX.Send("9");//consulta de reclamos
            AutoItX.Send("{ENTER}");
            objNu4.ReportarLog(LOG, "ingresando 9 consulta de reclamos");

            //entregamos los valores del siniestro para la consulta
            validar("*** SISTEMA DE SINIESTROS ***", IP);
            Thread.Sleep(500);
            AutoItX.Send(oficina);
            AutoItX.Send(ramo);
            AutoItX.Send(ejercicio);
            AutoItX.Send(siniestro);
            SINIESTRO = siniestro;
            objNu4.ReportarLog(LOG, "ingresando oficina " + oficina + " ramo " + ramo + " ejercicio " + ejercicio + " sinietro " + siniestro);
            //esta copia recupera los datos del siniestro
            resultado = validarConsulta("Numero de Inciso      :", IP);
            AutoIt.AutoItX.Send("r");//opcion R reporte
            AutoItX.Send("{ENTER}");
            objNu4.ReportarLog(LOG, "ingresando r reporte");
            List<string> reporte = validarConsulta("9.Reporto  :", IP);
            AutoIt.AutoItX.Send("n");// opcion N continua reporte
            AutoItX.Send("{ENTER}");
            objNu4.ReportarLog(LOG, "ingresando n continua reporte");
            List<string> continuaReporte = validarConsulta("D.M.  :", "R.T.    :", IP);
            AutoIt.AutoItX.Send("s");//opcion S comentario de cabina
            AutoItX.Send("{ENTER}");
            objNu4.ReportarLog(LOG, "ingresando s comentario de cabina");
            List<string> servicio = validarConsulta("1.Ajustador:", "2.Pasado   :", IP);

            infoAdmiSiniestro(resultado, reporte, continuaReporte, servicio);

            //salir de esa consulta
            AutoItX.Send("n");
            AutoItX.Send("{ENTER}");
            Thread.Sleep(500);
            AutoIt.AutoItX.Send("a");
            AutoItX.Send("{ENTER}");
            AutoIt.AutoItX.Send("f");
            AutoItX.Send("{ENTER}");
            Thread.Sleep(500);
            AutoIt.AutoItX.Send("a");
            AutoItX.Send("{ENTER}");
            AutoItX.Send("f");
            AutoItX.Send("{ENTER}");
            Thread.Sleep(1000);
            AutoIt.AutoItX.Send("f");
            AutoItX.Send("{ENTER}");
            //comandos de consulta poliza
            validar("10. Consulta de Polizas/Endosos", IP);
            AutoIt.AutoItX.Send("10");//opcion 10 Consulta de Polizas/Endosos
            AutoItX.Send("{ENTER}");
            objNu4.ReportarLog(LOG, "ingresando 10 Consulta de Polizas/Endosos ");
            validar("10. Consultas los Movimientos de un Inciso", IP);
            AutoIt.AutoItX.Send("10");//10.Consultas los Movimientos de un Inciso
            AutoItX.Send("{ENTER}");
            objNu4.ReportarLog(LOG, "ingresando 10.Consultas los Movimientos de un Inciso");
            validar("Cod.Ramo : .. (F=Fin)", IP);
            Thread.Sleep(500);

            AutoIt.AutoItX.Send(ramo);
            Thread.Sleep(500);
            AutoItX.Send(POLIZA);
            Thread.Sleep(500);
            AutoItX.Send(INCISO);
            Thread.Sleep(1000);

            AutoItX.Send("f");
            AutoItX.Send("{ENTER}");

            //consulta de operaciones
            Thread.Sleep(500);

            AutoItX.Send("f");
            AutoItX.Send("{ENTER}");
            validar(" 1. Consulta de Operaciones", IP);
            AutoItX.Send("1");//opcion 1 consulta de operaciones
            AutoItX.Send("{ENTER}");
            objNu4.ReportarLog(LOG, "ingresando 1. Consulta de Operaciones");
            AutoItX.Send(ramo);
            AutoItX.Send(POLIZA);
            AutoItX.Send(ENDOSO);
            AutoItX.Send("{ENTER}");
            objNu4.ReportarLog(LOG, "ingresando ramo " + ramo + " poliza " + POLIZA + " endoso " + ENDOSO);
            resultado = validarConsulta("Riesgo C.:", "Cob.:", IP);
            infoPoliza(resultado);
            infoOperaciones(resultado);
            AutoItX.Send("b");
            AutoItX.Send("{ENTER}");

            objNu4.ReportarLog(LOG, "Ingresando b coberturas");
            resultado = validarConsulta("Suma Aseg. :", IP);
            infoOperaciones2(resultado);

            AutoItX.Send("{ENTER}");
            objNu4.ReportarLog(LOG, "enter");

            Thread.Sleep(1500);
            AutoItX.Send("{ENTER}");
            objNu4.ReportarLog(LOG, "enter");
            AutoItX.Send("{ENTER}");
            objNu4.ReportarLog(LOG, "enter");
            //resultado = validarConsulta("Pma. D.M. : ", "Ajuste Aut: ", IP);
            //consulta de comberturas de Poliza/Endoso
            //infoCoberturas(resultado);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            for (int i = 0; i < 9; i++)
            {
                AutoItX.Send("f");
                Thread.Sleep(500);
            }
            AutoItX.Send("f");
            AutoItX.Send("{ENTER}");
            Thread.Sleep(1000);
            AutoItX.Send("f");
            AutoItX.Send("{ENTER}");
            Thread.Sleep(1000);
            AutoItX.Send("f");
            AutoItX.Send("{ENTER}");
            Thread.Sleep(1000);

            AutoItX.Send("f");
            AutoItX.Send("{ENTER}");
            Thread.Sleep(1000);
            objNu4.ReportarLog(LOG, "regresando al menu SINIESTROS ");
        }

        //copia el contenido de la pantalla de putty
        private List<string> copiar(string IP)
        {
            List<string> resultado = new List<string>();

            List<string> listAux = new List<string>();
            string resultadoAux = "";
            var winPosicion = new System.Drawing.Rectangle();
            char[] splitAux = { '\r', '\n' };
            int X, Y;
            Thread.Sleep(500);
            AutoItX.WinActivate(IP + " - PuTTY", "");

            winPosicion = AutoItX.WinGetPos();
            X = winPosicion.X;
            Y = winPosicion.Y;
            AutoItX.MouseClick("right", X + 40, Y + 15, 1);
            Thread.Sleep(500);
            AutoItX.Send("{DOWN 13}");
            Thread.Sleep(500);
            AutoItX.Send("{ENTER}");

            Thread.Sleep(500);

            resultadoAux = objNu4.clipboardObtenerTexto();
            listAux = resultadoAux.Split(splitAux).ToList<string>();
            listAux = listAux.Where(x => !string.IsNullOrEmpty(x)).ToList<string>();

            resultado = listAux;

            //reune la lista resultante en una cadena para poder mostrarla y guardarla
            resultadoAux = string.Join("\r\n", resultado.ToArray());
            // System.Windows.Forms.MessageBox.Show(resultadoAux); depuracion
            objNu4.ReportarLog(LOG, resultadoAux);

            return resultado;
        }
    }
}
