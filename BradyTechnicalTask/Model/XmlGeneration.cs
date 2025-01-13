using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using BradyTechnicalTask.ReferenceData;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BradyTechnicalTask.Model
{
    public class XmlGeneration
    {
        public void GenerateXml(string filename,string inputFolderPath)
        {
            var data = MapXmlToGenerationReport(filename);
            var outputData = new GenerationOutput()
            {
                Totals = TotalGenerationsCalculation(data.Generators.ToList()),
                MaxEmissionGenerators = DailyEmissionCalculation(data.Generators.Where(x => x.EmissionsRating != 0).ToList()),
                ActualHeatRates = ActualHeatRateCalculation(data.Generators.Where(x => x.ActualNetGeneration != 0).ToList())
            };
            GenerateOutPutFile(outputData, filename, inputFolderPath);
        }
        private GenerationReport MapXmlToGenerationReport(string filePath)
        {
            try
            {
                var report = new GenerationReport();
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

        private List<Totals> TotalGenerationsCalculation(List<Generator> generators)
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

        private List<MaxEmissionGenerators> DailyEmissionCalculation(List<Generator> generators)
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

        private List<ActualHeatRates> ActualHeatRateCalculation(List<Generator> generators)
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

        private void GenerateOutPutFile(GenerationOutput generationOutput, string filename, string inputFolderPath)
        {
            string outpuytFilename = filename.Replace(inputFolderPath, string.Empty);
            var filePath = ConfigurationManager.AppSettings["OutputFolder"] + outpuytFilename.Replace(Reference.XmlExtension, string.Empty) + "-Result.xml";
            var serializer = new XmlSerializer(typeof(GenerationOutput));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, generationOutput);
            }
        }

        

    }
}
