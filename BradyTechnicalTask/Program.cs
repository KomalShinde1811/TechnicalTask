using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using BradyTechnicalTask.ReferenceData;
using BradyTechnicalTask.Model;
using System.Linq;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using System.Xml.Serialization;
using System.Reflection;

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
                        var data = MapXmlToGenerationReport(file);
                        var outputData = new GenerationOutput()
                        {
                            Totals = TotalGenerationsCalculation(data.Generators.ToList()),
                            MaxEmissionGenerators = DailyEmissionCalculation(data.Generators.Where(x => x.EmissionsRating != 0).ToList()),
                            ActualHeatRates = ActualHeatRateCalculation(data.Generators.Where(x => x.ActualNetGeneration != 0).ToList())
                        };
                        GenerateOutPutFile(outputData);
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

        }

        public static GenerationReport MapXmlToGenerationReport(string filePath)
        {
            try
            {
                var report = new GenerationReport();
                //var serializer = new XmlSerializer(typeof(GenerationReport));

                //using (var reader = new StreamReader(filePath))
                //{
                //    return (GenerationReport)serializer.Deserialize(reader);
                //}
                var names = new List<string>();
                var data = new List<Generator>();
                XDocument xdoc = XDocument.Load(filePath);
                foreach (var childElement in xdoc.Root.Elements())
                {
                    names.Add(childElement.Name.ToString());
                }
                foreach (var element in names)
                {
                    var windGenerators = xdoc.Descendants(element + "Generator");

                    foreach (var windGenerator in windGenerators)
                    {
                        string name = windGenerator.Element("Name")?.Value;
                        string location = windGenerator.Element("Location")?.Value;
                        string emissionsRating = windGenerator.Element("EmissionsRating")?.Value;
                        string heatInput = windGenerator.Element("TotalHeatInput")?.Value;
                        string netGeneration = windGenerator.Element("ActualNetGeneration")?.Value;
                        var generatorData = new Generator()
                        {
                            Name = name,
                            Location = location,
                            EmissionsRating = Convert.ToDecimal(emissionsRating),
                            TotalHeatInput = Convert.ToDecimal(heatInput),
                            ActualNetGeneration = Convert.ToDecimal(netGeneration)
                        };

                        var generationData = windGenerator.Element("Generation")?.Descendants("Day");

                        if (generationData != null)
                        {
                            var generations = new List<Generation>();
                            foreach (var day in generationData)
                            {
                                string date = day.Element("Date")?.Value;
                                string energy = day.Element("Energy")?.Value;
                                string price = day.Element("Price")?.Value;
                                generations.Add(new Generation()
                                {
                                    Date = Convert.ToDateTime(date),
                                    Energy = Convert.ToDecimal(energy),
                                    Price = Convert.ToDecimal(price)
                                });
                            }
                            generatorData.Generation = generations;
                        }
                        data.Add(generatorData);
                    }
                    report.Generators = data;
                }
                return report;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading the XML file: {ex.Message}");
                return null;
            }

        }

        public static List<Totals> TotalGenerationsCalculation(List<Generator> generators)
        {
            var total = new List<Totals>();
            foreach (var generator in generators)
            {
                var valueFactorName = Reference.GeneratorValueFactor.Keys.FirstOrDefault(key => generator.Name.Contains(key));
                var valueFactor = Reference.ValueFactor[Reference.GeneratorValueFactor[valueFactorName]];
                decimal totalGen = 0;
                var t = new Totals() { Name = generator.Name };
                foreach (var generation in generator.Generation)
                {
                    totalGen += (generation.Energy * generation.Price * valueFactor);
                }
                t.Total = totalGen;
                total.Add(t);
            }

            return total;
        }

        public static List<MaxEmissionGenerators> DailyEmissionCalculation(List<Generator> generators)
        {

            var dailyEmission = new List<MaxEmissionGenerators>();
            foreach (var generator in generators)
            {
                var emissionFactorName = Reference.EmissionrValueFactor.Keys.FirstOrDefault(key => generator.Name.Contains(key));
                var emissionFactor = Reference.EmissionsFactor[Reference.EmissionrValueFactor[emissionFactorName]];
                foreach (var generation in generator.Generation)
                {
                    var emission = new MaxEmissionGenerators() { Name = generator.Name, Date = generation.Date };
                    emission.Emission = generation.Energy * emissionFactor * generator.EmissionsRating;
                    dailyEmission.Add(emission);
                }
            }
            return dailyEmission.GroupBy(x => x.Date).Select(y => new MaxEmissionGenerators()
            {
                Name = y.OrderByDescending(z => z.Emission).Select(z => z.Name).FirstOrDefault(),
                Date = y.Key,
                Emission = y.Max(z => z.Emission)
            }).ToList();
        }

        public static List<ActualHeatRates> ActualHeatRateCalculation(List<Generator> generators)
        {

            var heatRates = new List<ActualHeatRates>();
            foreach (var generator in generators)
            {
                var heatRate = new ActualHeatRates() { Name = generator.Name };
                heatRate.HeatRate = generator.TotalHeatInput / generator.ActualNetGeneration;
                heatRates.Add(heatRate);
            }
            return heatRates;
        }

        public static void GenerateOutPutFile(GenerationOutput generationOutput)
        {
            var filePath = ConfigurationManager.AppSettings["OutputFolder"];
            var serializer = new XmlSerializer(typeof(GenerationOutput));
            //using (var writer = new StreamWriter(filePath))
            //{
            //    // Serialize the report object to XML and write it to the file
            //    serializer.Serialize(writer, generationOutput);
            //    Console.WriteLine(writer);
            //}
            using (var stringWriter = new StringWriter())
            {
                // Serialize the object to XML
                serializer.Serialize(stringWriter, generationOutput);
                Console.WriteLine(stringWriter);

            }
        }

    }
}
