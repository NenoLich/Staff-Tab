using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff_Tab
{
    class SalaryEmployee : Employee
    {
        public new PayFrequency PayFrequency { get; }

        /// <summary>
        /// Annual salary rates. Only applies for employees whose pay frequency is "Salary"
        /// </summary>
        public double AnnualSalary { get; protected set; }

        [JsonConstructor]
        public SalaryEmployee(string secondName, string firstName, string jobTitles, string department, JobStatus jobStatus, int? typicalHours, double? annualSalary, double? hourlyRate) :
            base(secondName, firstName, jobTitles, department, jobStatus)
        {
            PayFrequency = PayFrequency.Salary;
            AnnualSalary = annualSalary ?? 0;
        }
    }
}
