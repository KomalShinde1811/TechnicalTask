using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BradyTechnicalTask.Model
{
    public class GenerationOutput
    {
        public List<Totals> Totals { get; set; }
        public List<MaxEmissionGenerators> MaxEmissionGenerators { get; set; }
        public List<ActualHeatRates> ActualHeatRates { get; set; }
    }
}
