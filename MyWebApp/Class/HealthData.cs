using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Class
{
    public class HealthData
    {
        public string ProjectID { get; set; }
        public DateTime UTCTIme { get; set; } = DateTime.UtcNow;
        public double EnergyInput { get; set; }
        public int Steps { get; set; }
        public double BodyWeight { get; set; }
        public double BMI { get; set; }
    }
}
