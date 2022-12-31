using System;
using System.Collections.Generic;
using System.IO;

namespace HPIConverter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] arguments)
        {
            //string[] arguments = Environment.GetCommandLineArgs();


            if (arguments == null || arguments.Length <= 1 || arguments[1] == "-h")
            {
                Console.WriteLine("Usage: inputfile outputfile");
            }
            else
            {
                string inputpath = arguments[0];
                string outputpath = arguments[1];

                if (File.Exists(inputpath))
                {
                    var inputfile = new FileInfo(inputpath);
                    var outputfile = new FileInfo(outputpath);
                    if (inputfile.Extension == ".hpi")
                    {
                        var extention = outputfile.Extension; //extention for output file
                        switch (extention)
                        {
                            case ".png":
                                HPIHelper.ConvertToPng(inputfile.FullName, outputpath);
                                break;
                            case ".jpg":
                                HPIHelper.ConvertToJpeg(inputfile.FullName, outputpath);
                                break;
                            case ".jpeg":
                                HPIHelper.ConvertToJpeg(inputfile.FullName, outputpath);
                                break;
                            default:
                                Console.WriteLine("Invalid file extention. only supports PNG/JPG");
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input path");
                }
            }
        }
    }
}