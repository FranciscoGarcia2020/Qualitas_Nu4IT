using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.ComponentModel;
using nu4itFox;
using System.Threading;

namespace QPil.Procesos
{
    class DatosLog
    {
        public static DataTable DATOS = new DataTable();
        private string LOG;
        nufox objNuFox = new nufox();
        Procesos.QContent PQ = new Procesos.QContent();

        //crea un hilo para trabajar
        public void HiloDeEjecucion(string log)
        {
            LOG = log;
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += FuncionPrincipalHILO;
            if (worker.IsBusy != true)
                worker.RunWorkerAsync();
            do
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));

            } while (worker.IsBusy==false);
        }

        //la funcion principal llamada por el hilo
        public void FuncionPrincipalHILO(object sender, DoWorkEventArgs e)
        {
            bool res = true;

            res = creacionTabla();
        }

        private bool creacionTabla()
        {
            bool exito = true;
            char[] splitAux = { '\r', '\n' };
            List<string> archivoLog = new List<string>();
            List<string> siniestros = new List<string>();
            string direccion = Directory.GetCurrentDirectory() + "\\" + "BITQPilProcSise20190205152653.log";
            StreamReader sr = new StreamReader(direccion);
            string resultado = sr.ReadToEnd();
            int row = 0, index;
            string aux;
            sr.Close();
            archivoLog = (resultado.Split(splitAux).ToList<string>());
            archivoLog = archivoLog.Where(x => !string.IsNullOrEmpty(x)).ToList<string>();
            TablaSise();
            DATOS.Rows.Add();
            index = archivoLog.FindIndex(
            delegate (string current)
            {
                return current.Contains("infoAdmiSiniestro");
            }
            );
            index++;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "sinietro ");
            DATOS.Rows[row]["Siniestro"] = aux;
            //objNu4.ReportarLog(LOG, "sinietro " + SINIESTRO);

            //reporte
            index++;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "reporte ");
            DATOS.Rows[row]["Reporte"] = aux;
            //objNu4.ReportarLog(LOG, "reporte " + aux);

            index++;
            //poliza
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "poliza");
            DATOS.Rows[row]["Poliza"] = aux;

            index++;
            //fecha de registro
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "fecha de registro ");
            DATOS.Rows[row]["Fecha de registro"] = aux;
            //objNu4.ReportarLog(LOG, "fecha de registro " + aux);
            index++;
            //endoso
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "endoso ");
            DATOS.Rows[row]["Endoso"] = aux;

            index++;
            //inciso
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "inciso ");

            DATOS.Rows[row]["Inciso"] = aux;

            index++;
            //modelo
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "modelo ");
            DATOS.Rows[row]["Modelo"] = aux;
            //objNu4.ReportarLog(LOG, "modelo " + modelo);

            index++;
            //motor
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "motor ");
            DATOS.Rows[row]["Motor"] = aux;
            //objNu4.ReportarLog(LOG, "motor " + aux);

            index++;
            //vigencia
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "vigencia ");
            DATOS.Rows[row]["Vigencia"] = aux;
            //objNu4.ReportarLog(LOG, "vigencia " + aux);

            //fecha ocurrido
            index++;
            aux = archivoLog[index];

            aux = objNuFox.StrExtract(aux, "fecha ocurrido ");
            DATOS.Rows[row]["Fecha Ocurrido"] = aux;
            //objNu4.ReportarLog(LOG, "fecha ocurrido " + aux);


            index += 2;
            //inicia reporte

            //asegurado                                          
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "asegurado ");

            DATOS.Rows[row]["Asegurado"] = aux;
            //objNu4.ReportarLog(LOG, "asegurado " + aux);

            index++;
            //serie
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "serie ");
            DATOS.Rows[row]["Serie"] = aux;
            //objNu4.ReportarLog(LOG, "serie " + aux);
            index++;
            //placas
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "placas ");
            DATOS.Rows[row]["Placas"] = aux;
            //objNu4.ReportarLog(LOG, "placas " + aux);

            //c.ocurrio
            index++;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "como ocurrio ");
            DATOS.Rows[row]["Como ocurrio"] = aux;
            // objNu4.ReportarLog(LOG, "como ocurrio " + aux);

            //marca
            index++;
            aux = archivoLog[index];

            aux = objNuFox.StrExtract(aux, "marca ");
            DATOS.Rows[row]["Marca"] = aux;
            //objNu4.ReportarLog(LOG, "marca " + aux);
            //continua reporte

            //reporte sac
            index += 2;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "reporte sac ");
            DATOS.Rows[row]["Reporte SAC"] = aux;
            //objNu4.ReportarLog(LOG, "reporte sac " + aux);

            //cobertura
            index++;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "cobertura ");
            DATOS.Rows[row]["Cobertura"] = aux;
            //objNu4.ReportarLog(LOG, "cobertura " + aux);

            //deducible DM
            index++;
            aux = archivoLog[index];

            aux = objNuFox.StrExtract(aux, "DM ");
            DATOS.Rows[row]["Deducible D.M."] = aux;
            //objNu4.ReportarLog(LOG, "DM " + aux);

            index++;
            //deducible RT
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "RT ");
            DATOS.Rows[row]["Deducible R.T."] = aux;
            //objNu4.ReportarLog(LOG, "RT " + aux);

            try
            {

                //comentario
                index++;
                aux = archivoLog[index];
                aux = objNuFox.StrExtract(aux, "comentario ");
                DATOS.Rows[row]["Comentario"] = aux;
                // objNu4.ReportarLog(LOG, "comentario " + aux);
            }
            catch (Exception)
            {


            }

            index++;
            //estatus de la poliza
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "Estatus de la Poliza: ");

            DATOS.Rows[row]["Estatus Poliza"] = aux;
            // objNu4.ReportarLog(LOG, "Estatus de la Poliza: Poliza Vencida ");

            //emision
            index = archivoLog.FindIndex(
           delegate (string current)
           {
               return current.Contains("infoPoliza");
           }
           );
            index++;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "Fecha Emision ");

            DATOS.Rows[row]["Fecha Emision"] = aux;

            index += 2;
            //primas pendientes
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "Primas Pendientes: ");
            DATOS.Rows[row]["Primas pendientes"] = aux;

            //suma asegurada
            index = archivoLog.FindIndex(
          delegate (string current)
          {
              return current.Contains("ingresa a infoOperaciones2");
          }
          );
            index++;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "Suma asegurada ");

            DATOS.Rows[row]["Suma asegurada"] = aux;

            //coberturas
            index = archivoLog.FindIndex(
         delegate (string current)
         {
             return current.Contains("ingresa a infoCoberturas");
         }
         );
            index++;
            //suma asegurada Cobertura
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "Suma.Aseg Cober ");
            DATOS.Rows[row]["Suma.Aseg Cober"] = aux;
            // objNu4.ReportarLog(LOG, "Suma.Aseg Cober " + aux);

            //suma RC 
            index++;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "Suma R.C. ");
            DATOS.Rows[row]["Suma R.C."] = aux;


            //suma GM
            index++;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "Suma G.M. ");
            DATOS.Rows[row]["Suma G.M."] = aux;


            //suma EE 
            index++;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "Suma E.E. ");
            DATOS.Rows[row]["Suma E.E."] = aux;


            //suma ADAP
            index++;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "Suma ADAP  ");
            DATOS.Rows[row]["Suma ADAP"] = aux;


            //Suma Aj.Au.
            index++;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "Suma Aj.Au. ");
            DATOS.Rows[row]["Suma Aj.Au."] = aux;


            //Suma G.L. 
            index++;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "Suma G.L. ");
            DATOS.Rows[row]["Suma G.L."] = aux;


            //Suma AV. 
            index++;
            aux = archivoLog[index];
            aux = objNuFox.StrExtract(aux, "Suma A.V. ");
            DATOS.Rows[row]["Suma A.V."] = aux;

            DATOS.Rows.Add("2635987", "1429039", "5700182165", "11/09/2018", "0000", "0001", "2016", "HECHO EN M",
                "CHEVROLET SONIC LT PAQ D 1.6L 115HP", "N74AFN", "3G1J85CC5GS543081", "Poliza Vigente", "181304556", "5.00%",
                "10.00%","186585"," Adicional",	"06/04/2018","31/05/18 al 31/05/23","33,315.15","39,109.57",
                "4,476,060.01",	"11/09/2018","ROSAS RODRIGUEZ JORGE ALBERTO","11/09/2018 04:26","3RO PEGA A NA	AMPLIA	09/11/2018 08:24 (DM14168): Q VS Q NA RES"
                );
            //PQ.ContentNavigate(LOG);
            new Procesos.Reporte().generaReporte(LOG);

            return exito;
        }
        //creacion de la estructura de la tabla para sise
        private void TablaSise()
        {

            List<string> titulos = new List<string>();
            //validar si es vacio

            //Reclamacion
            
            titulos.Add("Siniestro");//*

            titulos.Add("Reporte");//*
            titulos.Add("Poliza");//*
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
                                             //  titulos.Add("***CONSULTA DE Coberturas de POLIZA/ENDOSO***");

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
    }
}
