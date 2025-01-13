using System;
using System.Configuration;
using System.IO;
using BradyTechnicalTask.Model;
using System.Linq;

namespace BradyTechnicalTask
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var path = ConfigurationManager.AppSettings["InputFolder"];
            if (Directory.Exists(path))
            {
                Console.WriteLine($"Input folder exists: {path}");

                var files = Directory.GetFiles(path, "*.xml");
                if (files.Length > 0)
                {
                    Console.WriteLine("XML files found:");
                    foreach (var file in files)
                    {
                        var xmlGeneration = new XmlGeneration();
                        xmlGeneration.GenerateXml(file, path);
                    }
                }
                else
                {
                    Console.WriteLine("No XML files found in the folder.");
                }
            }
            else
            {
                Console.WriteLine($"Input folder does not exist: {path}");
            }
            //var inputFolderPath = ConfigurationManager.AppSettings["InputFolder"];
            //Console.WriteLine($"Input folder exist: {inputFolderPath}");
            //if (Directory.Exists(inputFolderPath))
            //{
            //    FileSystemWatcher watcher = new FileSystemWatcher();
            //    watcher.Path = inputFolderPath;
            //    watcher.Filter =".xml";
            //    watcher.Created += OnNewXmlFileAdded;
            //    watcher.EnableRaisingEvents = true;
            //    Console.ReadLine();
            //}
            //else
            //{
            //    Console.WriteLine($"Input folder does not exist: {inputFolderPath}");
            //}

        }
        private static void OnNewXmlFileAdded(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File added: {e.Name}");
            var xmlGeneration = new XmlGeneration();
            xmlGeneration.GenerateXml(e.Name, ConfigurationManager.AppSettings["InputFolder"]);
        }
    }
}
