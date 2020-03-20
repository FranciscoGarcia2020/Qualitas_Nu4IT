#region LIBRERIAS
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
using System.Windows.Shapes;
//Librerias agregadas
using System.ComponentModel;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
using Excel = Microsoft.Office.Interop.Excel;
using Outlook = Microsoft.Office.Interop.Outlook;
using Word = Microsoft.Office.Interop.Word;
using System.Data.OleDb;
#endregion

namespace NubotBKHL
{
    class Metodos_Selenium
    {
        public bool DarClik(IWebDriver driver, string Tipo, string Tag, string Caption)
        {
            ReadOnlyCollection<IWebElement> Collection;
            try
            {
                switch (Tipo)
                {
                    case "TagName":
                        Collection = driver.FindElements(By.TagName(Tag));
                        for (int i = 0; i < Collection.Count; i++)
                        {
                            string inner = Collection[i].Text;
                            if (inner.Contains(Caption))
                            {
                                Collection[i].Click();
                                return true;
                            }
                        }
                        break;
                    default: break;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}
