using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff_Tab
{
    /// <summary>
    /// Сериализация и десериализаци в JSon
    /// </summary>
    public class JsonFileService : IFileService
    {
        public Employee GetAfterEdit(string filename)
        {
            return JsonConvert.DeserializeObject<Employee>(File.ReadAllText(filename),
                new EmployeeConverter());
        }

        public ObservableCollection<Employee> Open(string filename)
        {
            return JsonConvert.DeserializeObject< ObservableCollection < Employee >> (File.ReadAllText(filename), 
                new EmployeeConverter());
        }

        public void Save(string filename, ObservableCollection<Employee> employees)
        {
            File.WriteAllText(filename, JsonConvert.SerializeObject(employees, Formatting.Indented, new EmployeeConverter()));
        }

        public void SaveBeforeEdit(string filename, Employee employee)
        {
            File.WriteAllText(filename, JsonConvert.SerializeObject(employee, Formatting.Indented, new EmployeeConverter()));
        }
    }
}
