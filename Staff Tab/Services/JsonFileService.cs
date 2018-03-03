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
    public class JsonFileService : IFileService
    {
        public ObservableCollection<Employee> Open(string filename)
        {
            return JsonConvert.DeserializeObject< ObservableCollection < Employee >> (File.ReadAllText(filename), 
                new EmployeeConverter());
        }

        public void Save(string filename, ObservableCollection<Employee> employees)
        {
            File.WriteAllText(filename, JsonConvert.SerializeObject(employees, Formatting.Indented, new EmployeeConverter()));
        }
    }
}
