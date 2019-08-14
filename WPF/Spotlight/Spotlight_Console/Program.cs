using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace access1
{
    class Program
    {

        public static List<string> SearchFile(string root)
        {
            var files = new List<string>();
			
			//add only files to the list by enumerating
            foreach (var file in Directory.EnumerateFiles(root))
            {
                files.Add(file);
            }
            //enumaerate sub dir
            foreach (var subDir in Directory.EnumerateDirectories(root))
            {
                try
                {
                	//addrange appends the whole collection to the list
                	//recursive call to get files from sub dirs
                    files.AddRange(SearchFile(subDir));
                }
                catch (UnauthorizedAccessException ex)
                {
                }
            }

            return files;
        }

        public static void Display( string drive, string fname)
        {
            Console.WriteLine("in drive: "+ drive);
            //to avoid path err
            StringBuilder Path = new StringBuilder(drive);
            StringBuilder file = new StringBuilder();
            file.Append(@"\");
            file.Append(fname);
            Path.Append(@"\");
            try
            {
                var files = SearchFile(Path.ToString());

                //check if entered file name exits in all the present files
                foreach (string e in files)
                {
                    //Console.WriteLine(e);
                    if (e.Contains(file.ToString()))
                    {
                        Console.WriteLine("file found " + e);
                    }

                }
            }
            catch(Exception e)
            {
                
            }

        }

		//entry pt
        static void Main(string[] args)
        {
        	//get ip
            Console.WriteLine("enter file name");
            string str = Console.ReadLine();
            
            /* allDrive - array of type driveinfo representing local drives on computer
            *rturned by getdrives
            */
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                //Console.WriteLine("Drive {0}", d.Name);
                /*
                * create a seprate thread for each drive present on the system
                * use delegate for passing multiple params to the method run by the thread
                * can use lambda in c# 3 () = > inside threadstart
                */
                Thread t = new Thread(new ThreadStart(delegate { Display(d.Name, str); }));
                t.Start();
                    
            }
        }
    }
}

