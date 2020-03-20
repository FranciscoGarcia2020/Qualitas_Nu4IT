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

#endregion

namespace QPil.Resources.Clases
{
    class Metodos_Acces
    {

        //VARIABLES GLOABLES
        public static OleDbConnection conexion;
        public static OleDbDataAdapter adaptador;

        //REVISAR LA CONEXION A LA BDD
        public Boolean ConexionBDD(String archivoBaseAccess)
        {
            if (File.Exists(archivoBaseAccess))
            {
                try
                {
                    conexion = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source =" + archivoBaseAccess + ";");
                    conexion.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Contains("proveedor"))
                    {

                    }
                    return false;
                }
            }
            else
            {
                MessageBox.Show("No existe o se perdio la conexión con la Base de Datos");
                return false;
            }
        }

        //CONSULTA GENERAL
        public DataTable ConsultaBDD(string RutaBaseDatos, String sentencia)
        {
            DataTable dtDatosAcces = new DataTable();
            if (ConexionBDD(RutaBaseDatos))
            {
                dtDatosAcces = consultar(sentencia);
                conexion.Close();
            }
            else
            {
                MessageBox.Show("No se pudó acceder a la base de datos.");
                return null;
            }
            return dtDatosAcces;
        }

        //QUERY
        public DataTable consultar(String sentencia)
        {
            DataTable dt = new DataTable();
            try
            {
                adaptador = new OleDbDataAdapter(sentencia, conexion);
                dt.Clear();
                adaptador.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return dt;
        }


    }
}
