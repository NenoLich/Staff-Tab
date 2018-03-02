using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff_Tab
{
    class Department: IComparable
    {
        List<Employee> employees=new List<Employee>();

        public string Name { get; private set; }

        public Department(string name)
        {
            Name = name;
        }

        public void Hire(Employee employee)
        {
            employees.Add(employee);
            employee.Department = this;
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(object obj)
        {
            return Name.CompareTo(obj);
        }
    }
}
