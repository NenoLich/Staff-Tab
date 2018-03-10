using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff_Tab
{
    /// <summary>
    /// Универсальный интерфейс сериализации
    /// </summary>
    interface IFileService
    {
        List<Employee> Open(string filename);
        void Save(string filename, List<Employee> employees);

        Employee GetAfterEdit(string filename);
        void SaveBeforeEdit(string filename, Employee employee);
    }
}
