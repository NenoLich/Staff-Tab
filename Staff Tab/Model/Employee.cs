using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Staff_Tab
{
    public abstract class Employee
    {
        [JsonIgnore]
        protected static ObservableCollection<Department> departments = new ObservableCollection<Department>();

        public PayFrequency PayFrequency { get; set; }
        public string SecondName { get; set; }
        public string FirstName { get; set; }
        public Uri ImageSource { get; set; }

        /// <summary>
        /// Title of employee
        /// </summary>
        public string JobTitles { get; set; }

        public Department Department { get; set; }
        public JobStatus JobStatus { get; set; }

        [JsonConstructor]
        protected Employee(string secondName, string firstName, string jobTitles, string departmentName, JobStatus jobStatus)
        {
            SecondName = secondName;
            FirstName = firstName;
            JobTitles = jobTitles;

            Department department = departments.FirstOrDefault(x => x.Title == departmentName);
            Department = department is null ? new Department(departmentName) : department;
            department?.Hire(this);

            JobStatus = jobStatus;
        }

        public override bool Equals(object other)
        {
            return SecondName == (other as Employee)?.SecondName && FirstName == (other as Employee)?.FirstName;
        }

        public override int GetHashCode()
        {
            return SecondName.GetHashCode();
        }
    }
}
