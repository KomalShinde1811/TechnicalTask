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
                watcher.Filter = ReferenceData.Reference.XmlExtension;
                watcher.Created += OnNewXmlFileAdded;

                // Start watching
                watcher.EnableRaisingEvents = true;
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Input folder does not exist: {inputFolderPath}");
            }

        }
        private static void OnNewXmlFileAdded(object sender, FileSystemEventArgs e)
        {
            var xmlGeneration = new XmlGeneration();
            xmlGeneration.GenerateXml(e.Name, ConfigurationManager.AppSettings["InputFolder"]);
        }
    }
}
