using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BradyTechnicalTask.ReferenceData
{
    public static class Reference
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Dictionary<string, decimal> ValueFactor = new Dictionary<string, decimal> {
            { "High", 0.946m },
            { "Medium", 0.696m },
             { "Low",0.265m },
        };
        /// <summary>
        /// 
        /// </summary>
        public static readonly Dictionary<string, decimal> EmissionsFactor = new Dictionary<string, decimal> {
            { "High", 0.812m },
            { "Medium", 0.562m },
             { "Low",0.312m  },
        };
        /// <summary>
        /// 
        /// </summary>
        public static readonly Dictionary<string, string> GeneratorValueFactor = new Dictionary<string, string> {
            { "Offshore", "Low"},
            { "Onshore","High" },
             { "Gas","Medium"  },
             { "Coal","Medium"  },

        };
        /// <summary>
        /// 
        /// </summary>
        public static readonly Dictionary<string, string> EmissionrValueFactor = new Dictionary<string, string> {
            { "Offshore ", "N/A"},
            { "Onshore","N/A" },
             { "Gas","Medium"  },
             { "Coal","High"  },

        };
    };



}
