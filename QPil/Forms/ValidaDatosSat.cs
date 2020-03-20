using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Threading;
using OpenQA.Selenium;

namespace QPil.Forms
{
    public partial class ValidaDatosSat : Form
    {
        string rutaLogSAT, rutaLogSAT2, rutaLogREPUVE, documento, ID;
        string RFC, SERIE, ANIO, NOAPROB, FOLIO, CERTIFICADO, RUTAPDF, RFCORIGINAL, FOLIOFISCAL, RFCRECEPTOR, SERIEAUTO;
        IWebDriver driverInicial;
        public ValidaDatosSat(string rutaLogSat, string rutaPdf, string rfc, string serie, string folio, string noAprobacion, string anio, string certificado, string id, IWebDriver driver)//SAT
        {
            InitializeComponent();
            textBoxRfc.Text = rfc;
            RFCORIGINAL = rfc;
            textBoxSerie.Text = serie;
            textBoxFolio.Text = folio;
            textBoxNoAprob.Text = noAprobacion;
            textBoxAnio.Text = anio;
            textBoxCertificado.Text = certificado;

            rutaLogSAT = rutaLogSat;
            RUTAPDF = rutaPdf;
            pnlRepube.Hide();
            pnlSat.Show();
            pnlSat2.Hide();
            btnEditarRepuve.Hide();
            btnEditarSat2.Hide();
            btnEditar.Show();
            this.ID = id;
            driverInicial = driver;
            webBrowserPDF.Navigate(RUTAPDF);

        }
        public ValidaDatosSat(string rutaLogSat2, string rutaPdf, string folioFiscal, string rfcEmisor, string rfcReceptor, string id, IWebDriver driver)//SAT2
        {
            InitializeComponent();
            txtFolioFiscal.Text = folioFiscal;
            txtRFCEmisor.Text = rfcEmisor;
            txtRFCReceptor.Text = rfcReceptor;

            rutaLogSAT2 = rutaLogSat2;
            RUTAPDF = rutaPdf;
            pnlRepube.Hide();
            pnlSat.Hide();
            pnlSat2.Show();
            btnEditarRepuve.Hide();
            btnEditar.Hide();
            btnEditarSat2.Show();
            this.ID = id;
            driverInicial = driver;
            webBrowserPDF.Navigate(RUTAPDF);
        }
        public ValidaDatosSat(string rutaLogRepuve, string rutaPdf, string serieAuto, string id, IWebDriver driver)//REPUVE
        {
            InitializeComponent();

            txtSerie.Text = serieAuto;
            rutaLogREPUVE = rutaLogRepuve;
            RUTAPDF = rutaPdf;
            pnlRepube.Show();
            pnlSat.Hide();
            pnlSat2.Hide();
            btnEditarSat2.Hide();
            btnEditar.Hide();
            btnEditarRepuve.Show();
            this.ID = id;
            driverInicial = driver;
            webBrowserPDF.Navigate(RUTAPDF);
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
            //  Procesos.CaptchaSAT objPMD = new Procesos.CaptchaSAT(rutaLogSAT);
            bool cancelar = true;
            Procesos.CaptchaSAT.ValidacionBoton = cancelar;
            Procesos.CaptchaRepuve.ValidacionBoton = cancelar;
            Procesos.CaptchaSAT2.ValidacionBoton = cancelar;

        }

        private void btnEditar_Click(object sender, EventArgs e) //SAT
        {
            bool cancelar = false;
            Procesos.CaptchaSAT.ValidacionBoton = cancelar;
            DataRow[] row = Procesos.ManejoDeDocumentos.DT_DATOS_SAT.Select("ID ='" + ID + "'");
            for (int i = 0; i < row.Length; i++)
            {
                row[i]["RFC EMISOR"] = textBoxRfc.Text;
                row[i]["SERIE"] = textBoxSerie.Text;
                row[i]["FOLIO"] = textBoxFolio.Text;
                row[i]["NO APROBACION"] = textBoxNoAprob.Text;
                row[i]["AÑO"] = textBoxAnio.Text;
                row[i]["CERTIFICADO"] = textBoxCertificado.Text;
                row[i].AcceptChanges();
            }
            // this.Close();
            this.Dispose();
            //Procesos.CaptchaSAT.DT_DATOS_SAT = DT_DATOS_SAT;
            Procesos.CaptchaSAT sat = new Procesos.CaptchaSAT(rutaLogSAT);
            DataTable fila = row.CopyToDataTable();
            sat.recibeDatatableSAT(fila.Rows[0]);
        }

        private void btnEditarSat2_Click(object sender, EventArgs e) //SAT2
        {
            bool cancelar = false;           
            Procesos.CaptchaSAT2.ValidacionBoton = cancelar;
            DataRow[] row = Procesos.ManejoDeDocumentos.DT_DATOS_SAT2.Select("ID ='" + ID + "'");
            for (int i = 0; i < row.Length; i++)
            {
                row[i]["RFC EMISOR"] = txtRFCEmisor.Text;
                row[i]["FOLIOFISCAL"] = txtFolioFiscal.Text;
                row[i]["RFCRECEPTOR"] = txtRFCReceptor.Text;
                row[i].AcceptChanges();
            }
            // this.Close();
            this.Dispose();
            //Procesos.CaptchaSAT2.DT_DATOS_SAT2 = DT_DATOS_SAT2;
            DataTable fila = row.CopyToDataTable();
            Procesos.CaptchaSAT2 sat2 = new Procesos.CaptchaSAT2(rutaLogSAT2);
            sat2.recibeDatatableSAT2(fila.Rows[0], driverInicial);

        }
        private void btnEditarRepuve_Click(object sender, EventArgs e) //REPUVE
        {
            bool cancelar = false;
            Procesos.CaptchaRepuve.ValidacionBoton = cancelar;
            DataRow[] row = Procesos.ManejoDeDocumentos.DT_DATOS_REPUVE.Select("ID ='" + ID + "'");
            for (int i = 0; i < row.Length; i++)
            {
                row[i]["SERIE"] = txtSerie.Text;
                row[i].AcceptChanges();
            }

            //this.Dispose();
            DataTable fila = row.CopyToDataTable();
            this.Dispose();
            //Procesos.CaptchaRepuve.DT_DATOS_REPUVE = DT_DATOS_REPUVE;
            Procesos.CaptchaRepuve repuve = new Procesos.CaptchaRepuve(rutaLogREPUVE);
            repuve.recibeDatatableRepuve(fila.Rows[0], driverInicial);

        }
        public void recibeDatos()
        {
            RFC = textBoxRfc.Text;
            SERIE = textBoxSerie.Text;
            FOLIO = textBoxFolio.Text;
            NOAPROB = textBoxNoAprob.Text;
            ANIO = textBoxAnio.Text;
            CERTIFICADO = textBoxCertificado.Text;
            DataRow[] t = Procesos.ManejoDeDocumentos.DT_DATOS_SAT.Select("RFC='" + RFCORIGINAL + "'");
            for (int i = 0; i < 1; i++)
            {
                t[i]["RFC"] = RFC;
                t[i]["SERIE"] = SERIE;
                t[i]["FOLIO"] = FOLIO;
                t[i]["NO.APROBACION"] = NOAPROB;
                t[i]["AÑO"] = ANIO;
                t[i]["CERTIFICADO"] = CERTIFICADO;
                t[i]["RUTA PDF"] = RUTAPDF;
                t[i].AcceptChanges();
            }

        }

    }
}
