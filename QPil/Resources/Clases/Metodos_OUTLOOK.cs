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
using Nu4it;
using nu4itExcel;
using nu4itFox;
using System.Reflection;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;
using System.Threading;
using System.Collections;
using System.Data;
using SHDocVw;

using System.Runtime.InteropServices;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Data.OleDb;
#endregion

namespace QPil.Resources.Clases
{
    class Metodos_Outlook : UserControl
    {
        //
        public List<string> CuentasOutlook()
        {
            Outlook.Application app = new Outlook.Application();
            Outlook.NameSpace ns = app.GetNamespace("MAPI");
            List<string> NombreFolders = new List<string>();
            if (ns.Folders.Count != 0)
            {
                int conta = 0;
                for (int k = 1; k <= ns.Folders.Count; k++)
                {
                    NombreFolders.Add(ns.Folders[k].Name);
                    conta++;
                }
            }
            return NombreFolders;
        }

        //
        public List<string> CarpetasOutlook(String cuenta)
        {
            string listaCarpetas = "";
            List<string> Carpetas = new List<string>();

            Outlook.Application app = new Outlook.Application();
            Outlook.NameSpace ns = app.GetNamespace("MAPI");
            if (ns.Folders[cuenta].Folders.Count != 0)
                for (int a = 1; a <= ns.Folders[cuenta].Folders.Count; a++)
                {
                    Dispatcher.Invoke(((Action)(() => listaCarpetas = listaCarpetas + ns.Folders[cuenta].Folders[a].Name + Environment.NewLine)));
                    SubCarpetas(ns.Folders[cuenta].Folders[a]);
                }
            string[] carpetas = listaCarpetas.Split('\n');
            List<string> lisCrp = new List<string>();
            lisCrp = carpetas.ToList();
            lisCrp.Sort();
            carpetas = lisCrp.ToArray();
            for (int x = 0; x < carpetas.Length; x++)
                if (carpetas[x] != "")
                {
                    carpetas[x] = carpetas[x].Replace("\r", "");
                    Dispatcher.Invoke(((Action)(() => Carpetas.Add(carpetas[x]))));
                }
            return Carpetas;
        }

        //
        public void SubCarpetas(Outlook.MAPIFolder folder)
        {
            for (int k = 1; k <= folder.Folders.Count; k++)
                if (folder.Folders[k].Folders.Count > 0)
                    SubCarpetas(folder.Folders[k]);
        }

        //
        public Outlook.MAPIFolder ObtenerMAPI(string carpeta, Outlook.MAPIFolder folder)
        {
            Outlook.MAPIFolder mapa = folder;
            if (folder.Folders.Count > 0)
            {
                try
                {
                    for (int a = 1; a <= folder.Folders.Count; a++)
                    {
                        string comp = "";
                        if (comp != carpeta)
                        {
                            if (folder.Folders[a].Folders.Count > 0)
                            {
                                for (int z = 1; z <= folder.Folders[a].Folders.Count; z++)
                                {
                                    mapa = ObtenerMAPI(carpeta, folder.Folders[a].Folders[z]);
                                    string nombrCarp = mapa.Name;
                                    if (nombrCarp == carpeta)
                                    {
                                        z = folder.Folders[a].Folders.Count;
                                        a = folder.Folders.Count;
                                    }
                                }
                            }
                        }
                        if (comp == carpeta)
                        {
                            mapa = folder.Folders[a];
                            a = folder.Folders.Count;
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            return (mapa);
        }




    }
}
