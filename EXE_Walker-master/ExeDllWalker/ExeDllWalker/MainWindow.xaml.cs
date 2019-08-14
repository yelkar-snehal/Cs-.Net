using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExeDllWalker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetInfo_Click(object sender, RoutedEventArgs e)
        {
            string ret = "";
            ProcessInfo(proc.Text);
            title1.Content = proc.Text + " Dlls in use:";
            ret = File.ReadAllText("logs.txt");
            
            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Width = 800;
            Application.Current.MainWindow.Height = 500;
            Spanel.Height = 250;
            var textBox = new TextBox
            {
                Width = Spanel.Width,
                Height = 250,
                
                Text = ret,
                FontSize = 18,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Visible,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible
            };

            //this.Content = textBox;
            Spanel.Children.Add(textBox);
            textBox.IsReadOnly = true;
        }

        public void ProcessInfo(string str)
        {
            Process[] procName = Process.GetProcessesByName(str);
            ProcessModule pm;
            ProcessModuleCollection m = null;
            using (TextWriter sw = new StreamWriter("logs.txt"))
            {
                foreach (var proc in procName)
                {
                    sw.WriteLine("////////////////////////////////////////////////////////////////////");
                    m = proc.Modules;
                    sw.WriteLine("Processs Name: " + proc.ProcessName);
                    sw.WriteLine("Thread cnt: "+ proc.Threads.Count);
                    for (int i = 0; i < m.Count; i++)
                    {
                        pm = m[i];
                        sw.WriteLine("The moduleName is " + pm.ModuleName);
                        sw.WriteLine("The " + pm.ModuleName + "'s File name is: "
                           + pm.FileName);
                    }

                    sw.WriteLine("End of one proc\n");
                }
            }
        }

        private void DLLInfo_Click(object sender, RoutedEventArgs e)
        {
            string ret = "";
            DLLUseInfo(dllname.Text);
            title1.Content = "Procs using "+dllname.Text+" : ";
            ret = File.ReadAllText("logs2.txt");

            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Width = 800;
            Application.Current.MainWindow.Height = 500;
            Spanel.Height = 250;
            var textBox2 = new TextBox
            {
                Width = Spanel.Width,
                Height = 250,

                Text = ret,
                FontSize = 18,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Visible,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible
            };

            //thils.Content = textBox;
            Spanel.Children.Clear();
            Spanel.Children.Add(textBox2);
            textBox2.IsReadOnly = true;
        }

        public void DLLUseInfo(string str)
        {
            Process[] localAll = Process.GetProcesses();
            ProcessModule pmx;
            ProcessModuleCollection mx = null;

            using (TextWriter swx = new StreamWriter("logs2.txt"))
            {
                foreach (var procx in localAll)
                {
                    
                    try
                    {
                        mx = procx.Modules;
                        for (int i = 0; i < mx.Count; i++)
                        {
                            pmx = mx[i];
                            if (pmx.ModuleName.Contains(str))
                            {
                                swx.WriteLine("processs: " + procx.ProcessName);
                            }
                        }
                    }
                    #pragma warning disable 0168
                    catch (System.ComponentModel.Win32Exception)
                    {

                    }

                }
            }
        }
    }
}

/*
 
     */
