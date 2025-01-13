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
                var files = Directory.GetFiles(inputFolderPath, "*.xml");
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        var xmlGeneration = new XmlGeneration();
                        xmlGeneration.GenerateXml(file, inputFolderPath);
                    }
                }
            }
            else
            {
                Console.WriteLine($"Input folder does not exist: {inputFolderPath}");
            }

        }
    }
}
