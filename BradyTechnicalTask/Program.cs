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
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = path;
                watcher.Filter = "*.xml";
                watcher.Created += OnNewXmlFileAdded;
                //watcher.Changed += OnNewXmlFileAdded;
                };
                watcher.EnableRaisingEvents = true;
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Input folder does not exist: {path}");
            }

        }
        private static void OnNewXmlFileAdded(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File added: {e.Name}");
            var xmlGeneration = new XmlGeneration();
            xmlGeneration.GenerateXml(e.Name, ConfigurationManager.AppSettings["InputFolder"]);
        }
    }
}
