using System;
using System.Configuration;
using System.IO;
using BradyTechnicalTask.Model;
using System.Linq;
using System.Threading.Tasks;

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
                watcher.Created += async (sender, e) =>
                {
                    if (e.ChangeType == WatcherChangeTypes.Created)
                    {
                        await OnNewXmlFileAdded(e.FullPath);
                    }
                };
                watcher.EnableRaisingEvents = true;
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Input folder does not exist: {path}");
            }

        }
        private static async Task OnNewXmlFileAdded(string fullPath)
        {
            await Task.Delay(500);
            Console.WriteLine($"File added: {fullPath}");
            var xmlGeneration = new XmlGeneration();
            xmlGeneration.GenerateXml(fullPath, ConfigurationManager.AppSettings["InputFolder"]);
        }
    }
}
