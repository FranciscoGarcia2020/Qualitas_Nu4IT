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
    /// <summary>
    /// BY: JORGE NÚUÑEZ
    /// 17-NOV-17
    /// </summary>
    class Metodos_Charts
    {

        public ObservableCollection<PieDataItem> DatosGrafica(DataTable Datos)
        {
            ObservableCollection<PieDataItem> data = new ObservableCollection<PieDataItem>();
            //data.Add(new PieDataItem() { Title = "", Value = 0 });
            //data.Add(new PieDataItem() { Title = "", Value = 0 });
            //data.Add(new PieDataItem() { Title = "", Value = 0 });
            //data.Add(new PieDataItem() { Title = "", Value = 0 });
            //data.Add(new PieDataItem() { Title = "", Value = 0 });
            //data.Add(new PieDataItem() { Title = "", Value = 0 });
            foreach (DataRow item in Datos.Rows)
            {
                string Titulo = item.Field<string>(0);
                int Valor = item.Field<int>(1);
                data.Add(new PieDataItem() { Title = Titulo, Value = Valor });
            }
            return data;
        }

        public class PieDataItem : INotifyPropertyChanged
        {
            public string Title { get; set; }
            private double _value;
            public double Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    _value = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                    }
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
