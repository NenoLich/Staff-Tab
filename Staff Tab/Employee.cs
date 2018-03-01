using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff_Tab
{
    abstract class Employee: IComparable
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PayFrequency PayFrequency { get; }
        public string SecondName { get; protected set; }
        public string FirstName { get; protected set; }

        /// <summary>
        /// Title of employee
        /// </summary>
        public string JobTitles { get; protected set; }
        public Department Department { get; protected set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public JobStatus JobStatus { get; protected set; }

        [JsonConstructor]
        protected Employee(string secondName, string firstName, string jobTitles, string department, JobStatus jobStatus)
        {
            PayFrequency = PayFrequency.Salary;
            SecondName = secondName;
            FirstName = firstName;
            JobTitles = jobTitles;
            if ()
            {
                Department = department;
            }
            
            JobStatus = jobStatus;
        }

        public override string ToString()
        {
            return $"{ FirstName } { SecondName }";
        }

        public int CompareTo(object obj)
        {
            return SecondName.CompareTo(obj);
        }
    }
}
