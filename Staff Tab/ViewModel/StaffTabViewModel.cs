using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace Staff_Tab
{
    class StaffTabViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> employees;

        IFileService fileService;
        IDialogService dialogService;

        public StaffTabViewModel(IFileService fileService, IDialogService dialogService)
        {
            Employees = new ObservableCollection<Employee>();

            this.dialogService = dialogService;
            this.fileService = fileService;
        }

        public ObservableCollection<Department> Departments
        {
            get => Employee.departments;
            set
            {
                Employee.departments = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get => employees;
            set
            {
                employees = value;
                OnPropertyChanged();
            }
        }

        #region Commands

        private RelayCommand saveCommand;
        private RelayCommand openCommand;
        private RelayCommand departmentSelectionChangedCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (dialogService.SaveFileDialog() == true)
                          {
                              fileService.Save(dialogService.FilePath, Employees);
                              dialogService.ShowMessage("Файл сохранен");
                          }
                      }
                      catch (Exception ex)
                      {
                          dialogService.ShowMessage(ex.Message);
                      }
                  }));
            }
        }

        public RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??
                  (openCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (dialogService.OpenFileDialog() == true)
                          {
                              Employees.Clear();
                              foreach (Employee employee in fileService.Open(dialogService.FilePath))
                                  Employees.Add(employee);
                              dialogService.ShowMessage("Файл открыт");
                          }
                      }
                      catch (Exception ex)
                      {
                          dialogService.ShowMessage(ex.Message);
                      }
                  }));
            }
        }

        public RelayCommand DepartmentSelectionChangedCommand => departmentSelectionChangedCommand ??
                    (departmentSelectionChangedCommand = new RelayCommand(obj =>
                      Employees = Employees.Where(x => x.Department == obj as Department) as ObservableCollection<Employee>));

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
