using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;


namespace Spotlight
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
        bool chk = true;
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchResults.Visibility = Visibility.Visible;
            SearchResults.IsEnabled = true;
            chk = true;
            search();
            
        }
        

        public void search()
        {
            if(chk)
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                string str = fname.Text;

                foreach (DriveInfo d in allDrives)
                {
                    //Console.WriteLine("Drive {0}", d.Name);

                    Thread t = new Thread(new ThreadStart(delegate { Display(d.Name, str); }));
                    t.Start();
                    //SearchResults.AppendText(d.Name);
                    //SearchResults.AppendText("\n");

                }
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            chk = false;
            SearchResults.IsReadOnly = true;
        }

        //////////////////////////////////////////////////////////
        public void Display(string drive, string fname)
        {
            //Console.WriteLine("in drive: " + drive);

            StringBuilder Path = new StringBuilder(drive);
            StringBuilder file = new StringBuilder();
            file.Append(@"\");
            file.Append(fname);
            Path.Append(@"\");

            this.Dispatcher.Invoke((Action)(() =>
            {
               // SearchResults.AppendText(Path.ToString());
               // SearchResults.AppendText("\n");
                try
                {
                    var files = SearchFile(Path.ToString());


                    foreach (string e in files)
                    {
                        //Console.WriteLine(e);
                        if (e.Contains(file.ToString()))
                        {
                            SearchResults.AppendText(e);
                            SearchResults.AppendText("\n");
                        }

                    }
                }
                catch (Exception e)
                {
                    using (StreamWriter w = File.AppendText("Log.txt"))
                       {
                           w.WriteLine(e);
                       }
                }
            }));

        }
        ///////////////////////////////////////////////////////////////////
        public List<string> SearchFile(string root)
        {
            var files = new List<string>();

            foreach (var file in Directory.EnumerateFiles(root))
            {
                files.Add(file);
            }
            foreach (var subDir in Directory.EnumerateDirectories(root))
            {
                try
                {
                    files.AddRange(SearchFile(subDir));
                }
                catch (UnauthorizedAccessException ex)
                {
                    using (StreamWriter w = File.AppendText("Log.txt"))
                     {
                         w.WriteLine(ex);
                     }

                }
            }

            return files;
        }

    }

}
