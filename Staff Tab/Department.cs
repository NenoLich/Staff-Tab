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

        public string DepartmentName { get; private set; }

        public Department(string departmentName)
        {
            DepartmentName = departmentName;
        }

        public void Hire(Employee employee)
        {
            employees.Add(employee);
        }

        public override string ToString()
        {
            return DepartmentName;
        }

        public int CompareTo(object obj)
        {
            return DepartmentName.CompareTo(obj);
        }
    }
}
