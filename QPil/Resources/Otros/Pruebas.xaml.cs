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
using Word = Microsoft.Office.Interop.Word;
using System.Data.OleDb;

namespace QPil.Resources
{
    /// <summary>
    /// Interaction logic for Pruebas.xaml
    /// </summary>
    public partial class Pruebas : Window
    {
        public Pruebas()
        {
            InitializeComponent();
            ///
            string archivoBaseAccess = @"\\10.164.246.35\Transporte_Centralizado\BESTCOLLECT\Convenios\DB_COVE.mdb";
            OleDbConnection conexion;
            conexion = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source =" + archivoBaseAccess + ";");
            conexion.Open();
            ///
            int conta = 1;
            string mnsj = "", NOs = "";
            string[] Firmados = Directory.GetFiles(@"\\10.164.246.35\Transporte_Centralizado\BESTCOLLECT\Convenios\Firmados");
            foreach (string item in Firmados)
            {
                string Folio = System.IO.Path.GetFileNameWithoutExtension(item);
                try
                {
                    string sentencia = "UPDATE SolicitudesCOVE SET STATUS = 'FIRMADO', RutaConvenioFinal = '" + item + "' WHERE Folio='" + Folio + "'";
                    OleDbCommand actualizarlFolio = new OleDbCommand(sentencia, conexion);
                    if (conexion.State != ConnectionState.Open)
                        conexion.Open();
                    int result = actualizarlFolio.ExecuteNonQuery();
                    if (result > 0)
                    {

                        conta++;
                    }
                    else
                    {
                        NOs += Folio + Environment.NewLine;
                    }
                }
                catch (Exception ex)
                {
                    mnsj += Folio + " - " + ex.Message.ToString() + Environment.NewLine;
                }
            }
            MessageBox.Show("Con SI: " + conta);
            MessageBox.Show("Con NO: " + NOs);
            MessageBox.Show("Con ERROR: " + mnsj);

        }
    }
}
