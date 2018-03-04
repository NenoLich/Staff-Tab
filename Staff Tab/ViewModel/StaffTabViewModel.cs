using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Collections;

namespace Staff_Tab
{
    class StaffTabViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> employees;
        private ObservableCollection<Department> departments;
        private ObservableCollection<Department> selectedDepartments;
        
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
            get => departments;
            set
            {
                departments = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Department> SelectedDepartments
        {
            get => selectedDepartments;
            set
            {
                selectedDepartments = value;
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
        private RelayCommand departmentsExpanded;
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

                          Departments = new ObservableCollection<Department>(Employees.Select(x => x.Department).Distinct());
                      }
                      catch (Exception ex)
                      {
                          dialogService.ShowMessage(ex.Message);
                      }
                  }));
            }
        }

        public RelayCommand DepartmentsExpanded
        {
            get
            {
                return departmentsExpanded ??
                  (departmentsExpanded = new RelayCommand(obj =>
                  {
                      Expander expander = obj as Expander;

                      foreach (Department department in Departments)
                      {
                          

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
