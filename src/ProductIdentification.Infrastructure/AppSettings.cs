using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductIdentification.Infrastructure
{
    public class AppSettings
    {
        public string CustomVisionTrainingKey { get; set; }
        public string CustomVisionPredictionKey { get; set; }
        public string CustomVisionProjectId { get; set; }
        public string CustomVisionPredictionId { get; set; }
    }
}