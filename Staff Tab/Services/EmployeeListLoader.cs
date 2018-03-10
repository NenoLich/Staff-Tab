using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff_Tab.Services.DataVirtualization;

namespace Staff_Tab
{
    class EmployeeListLoader : IVirtualListLoader<Employee>
    {
        public EmployeeListLoader(List<Employee> source)
        {
            Source = source;
        }

        public List<Employee> Source { get; set; }
        public int ItemCount { get => Source.Count(); }
        public bool CanSort => true;

        public IList<Employee> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
        {
            overallCount = ItemCount;

            SortDescription sortDescription = sortDescriptions == null || sortDescriptions.Count == 0 ? new SortDescription() : sortDescriptions[0];
            ListSortDirection direction = string.IsNullOrEmpty(sortDescription.PropertyName) ? ListSortDirection.Ascending : sortDescription.Direction;

            count = ItemCount-startIndex < count ? ItemCount-startIndex : count;
            Employee[] employees = new Employee[count];
            for (int i = 0; i < count; i++)
            {
                int index;
                if (direction == ListSortDirection.Ascending)
                    index = startIndex + i;
                else
                    index = overallCount - 1 - startIndex - i;

                    employees[i] = Source[index];
            }

            return employees;
        }
    }
}