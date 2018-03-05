using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff_Tab
{
    /// <summary>
    /// Подразделение
    /// </summary>
    public class Department
    {
        List<Employee> employees=new List<Employee>();

        /// <summary>
        /// Название подразделения
        /// </summary>
        public string Title { get; private set; }

        public Department(string name)
        {
            Title = name;
        }

        /// <summary>
        /// Зачисление в штат
        /// </summary>
        /// <param name="employee"></param>
        public void Hire(Employee employee)
        {
            employees.Add(employee);
            employee.Department = this;
        }

        public override bool Equals(object other)
        {
            return Title == (other as Department).Title;
        }

        public override int GetHashCode()
        {
            return this.Title.GetHashCode();
        }
    }
}
