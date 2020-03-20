using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//LIBRERIAS AGREGADAS 
using System.IO;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Net.NetworkInformation;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Threading;
using System.Collections;
using System.ComponentModel;
using System.Data;

using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Controls;


namespace NubotBKHL
{
    class Metodos_MiIE
    {
        //DAR CLICK EN ELEMEENTO
        public void DarClickEnElemento(SHDocVw.InternetExplorer MiIE, string Tipo, string Id, string Atributo, string AtribBuscado)
        {
            try
            {
                switch (Tipo)
                {
                    case "Id":
                        var IDS = MiIE.Document.getElemensById(Id);
                        if (IDS != null)
                        {
                            IDS.click();
                        }
                        break;
                    case "TagName":
                        var TAGS = MiIE.Document.getElementsByTagName(Id);
                        for (int i = 0; i < TAGS.Length; i++)
                        {
                            string titulo = TAGS[i].GetAttribute(Atributo);
                            if (titulo.ToUpper().Contains(AtribBuscado.ToUpper()))
                            {
                                TAGS[i].click();
                                do { } while ((Convert.ToInt16(MiIE.ReadyState) != 4) || (MiIE.Busy));
                                Thread.Sleep(500);
                                break;
                            }
                        }
                        break;
                    case "ClassName":
                        break;
                    case "Name":
                        break;
                    case "Inner":
                        var solucion = MiIE.Document.getElementsByTagName(Id);
                        for (int transp = 0; transp < solucion.Length; transp++)
                        {
                            if (solucion[transp].InnerHTML.ToString().Contains(Atributo))
                            {
                                solucion[transp].click();
                                do { } while ((Convert.ToInt16(MiIE.ReadyState) != 4) || (MiIE.Busy));
                                Thread.Sleep(500);
                                break;
                            }
                        }
                        break;
                    case "Attribute":
                        var butons = MiIE.Document.getElementsByTagName(Id);
                        for (int i = 0; i < butons.Length; i++)
                        {
                            string titulo = butons[i].GetAttribute(Atributo);
                            if (titulo.ToUpper().Contains(AtribBuscado.ToUpper()))
                            {
                                butons[i].click();
                                do { } while ((Convert.ToInt16(MiIE.ReadyState) != 4) || (MiIE.Busy));
                                Thread.Sleep(500);
                                break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ///StatusLog("No existe el elemento");
            }
        }
    }
}
