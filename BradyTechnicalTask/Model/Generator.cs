using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BradyTechnicalTask.Model
{

    public class Generator
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public decimal EmissionsRating { get; set; }
        public decimal TotalHeatInput { get; set; }

        public decimal ActualNetGeneration { get; set; }

        public List<Generation> Generation { get; set; }


    }
}
