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
        public string CustomVisionEndpoint { get; set; }
        public string Storage { get; set; }
        public string EmailFrom { get; set; }
        public string EmailPassword { get; set; }
        public string EmailSmtpHostPassword { get; set; }
    }
}