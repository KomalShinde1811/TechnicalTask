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
            var inputFolderPath = ConfigurationManager.AppSettings["InputFolder"];
            if (Directory.Exists(inputFolderPath))
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = inputFolderPath;

                // Filter to watch only .xml files
                watcher.Filter = "*.xml";

                // Subscribe to the Created event (when a new file is added)
                watcher.Created += OnNewXmlFileAdded;

                // Start watching
                watcher.EnableRaisingEvents = true;

                //var files = Directory.GetFiles(inputFolderPath, "*.xml");
                //if (files.Length > 0)
                //{
                //    foreach (var file in files)
                //    {
                //        var xmlGeneration = new XmlGeneration();
                //        xmlGeneration.GenerateXml(file, inputFolderPath);
                //    }
                //}
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Input folder does not exist: {inputFolderPath}");
            }

        }
        private static void OnNewXmlFileAdded(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"New XML file added: {e.Name}");
            var xmlGeneration = new XmlGeneration();
            xmlGeneration.GenerateXml(e.Name, ConfigurationManager.AppSettings["InputFolder"]);
        }
    }
}
