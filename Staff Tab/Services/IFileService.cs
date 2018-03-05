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
        ObservableCollection<Employee> Open(string filename);
        void Save(string filename, ObservableCollection<Employee> employees);
    }
}
