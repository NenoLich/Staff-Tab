using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff_Tab
{
    public class Department : IComparable
    {
        List<Employee> employees=new List<Employee>();

        public string Title { get; private set; }

        public Department(string name)
        {
            Title = name;
        }

        public void Hire(Employee employee)
        {
            employees.Add(employee);
            employee.Department = this;
        }

        public int CompareTo(object obj)
        {
            return Title.CompareTo(obj);
        }
    }
}
