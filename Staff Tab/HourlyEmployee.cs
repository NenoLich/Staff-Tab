using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff_Tab
{
    class HourlyEmployee : Employee
    {
        public new PayFrequency PayFrequency { get; }

        /// <summary>
        /// Describes the typical amount of work for hourly employees. This data does not apply to salary employees. 
        /// 40 - Employee paid on an hourly basis; works an 8 hour day; can be either full-time permanent (FT/P) or full-time temporary (FT-T) which is a seasonal employee; 
        /// 35 - Employee paid on an hourly basis; works a 7 hour day; can be either full-time permanent (FT/P) or full-time temporary (FT-T) which is a seasonal employee; 
        /// 20 - Employee paid on a part-time, hourly basis; typically works a 4 hour day, 5 days a week; 
        /// 10 - Employee paid on a part-time, hourly basis; works 10 hours or less in a week.
        /// </summary>
        public int TypicalHours { get; protected set; }

        /// <summary>
        /// The hourly salary rates for individuals whose pay frequency is "hourly"
        /// </summary>
        public double HourlyRate { get; protected set; }

        [JsonConstructor]
        public HourlyEmployee(string secondName, string firstName, string jobTitles, string department, JobStatus jobStatus, int? typicalHours, double? annualSalary, double? hourlyRate) :
            base(secondName, firstName, jobTitles, department, jobStatus)
        {
            PayFrequency = PayFrequency.Hourly;
            TypicalHours = typicalHours ?? 0;
            HourlyRate = hourlyRate ?? 0;
        }
    }
}
