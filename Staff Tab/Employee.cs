using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Staff_Tab
{
    abstract class Employee: IComparable
    {
        [JsonIgnore]
        protected static DepartmentCollection departments = (DepartmentCollection)Application.Current.FindResource("Departments");

        public PayFrequency PayFrequency { get; }
        public string SecondName { get; protected set; }
        public string FirstName { get; protected set; }

        /// <summary>
        /// Title of employee
        /// </summary>
        public string JobTitles { get; protected set; }

        public Department Department { get; set; }
        public JobStatus JobStatus { get; protected set; }

        [JsonConstructor]
        protected Employee(string secondName, string firstName, string jobTitles, string departmentName, JobStatus jobStatus)
        {
            SecondName = secondName;
            FirstName = firstName;
            JobTitles = jobTitles;

            Department department = departments.First(x => x.Name == departmentName);
            Department = department is null ? new Department(departmentName) : department;
            department.Hire(this);

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
