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
using System.Windows;

namespace Staff_Tab
{
    class StaffTabViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> employees;
        private ObservableCollection<Employee> selectedEmployees;
        private ObservableCollection<Department> departments;
        private ObservableCollection<Department> selectedDepartments;
        private bool updated = false;
        private string lastFilePath = null;

        IFileService fileService;
        IDialogService dialogService;

        public StaffTabViewModel(IFileService fileService, IDialogService dialogService)
        {
            employees = new ObservableCollection<Employee>();
            selectedDepartments = new ObservableCollection<Department>();

            this.dialogService = dialogService;
            this.fileService = fileService;
        }

        public bool Updated
        {
            get { return updated; }
            set
            {
                updated = value;
                OnPropertyChanged();
            }
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

        public ObservableCollection<Employee> SelectedEmployees
        {
            get => selectedEmployees;
            set
            {
                selectedEmployees = value;
                OnPropertyChanged();
            }
        }

        #region Commands

        private RelayCommand newCommand;
        private RelayCommand saveCommand;
        private RelayCommand saveAsCommand;
        private RelayCommand openCommand;
        private RelayCommand departmentSelectionChangedCommand;
        private RelayCommand closingCommand;

        public RelayCommand NewCommand
        {
            get
            {
                return newCommand ??
                (newCommand = new RelayCommand(obj =>
                {
                    CheckUnsavedData();
                    Reset();
                }));

            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      if (lastFilePath is null) SaveFile();
                      else SaveLastFile();
                  }));
            }
        }

        public RelayCommand SaveAsCommand
        {
            get
            {
                return saveAsCommand ??
                    (saveAsCommand = new RelayCommand(obj => SaveFile()));
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
                              employees.Clear();
                              foreach (Employee employee in fileService.Open(dialogService.FilePath))
                                  employees.Add(employee);
                              lastFilePath = dialogService.FilePath;
                              updated = false;
                              dialogService.ShowMessage("Файл открыт");
                          }

                          SelectedEmployees = new ObservableCollection<Employee>(employees.Distinct());
                          Departments = new ObservableCollection<Department>(employees.Select(x => x.Department).Distinct());
                      }
                      catch (Exception ex)
                      {
                          dialogService.ShowMessage(ex.Message);
                      }
                  }));
            }
        }

        public RelayCommand ClosingCommand
        {
            get
            {
                return closingCommand ??
                  (closingCommand = new RelayCommand(obj =>
                  {
                      CheckUnsavedData();
                      Environment.Exit(0);
                  }));

            }
        }

        public RelayCommand DepartmentSelectionChangedCommand
        {
            get
            {
                return departmentSelectionChangedCommand ??
                            (departmentSelectionChangedCommand = new RelayCommand(obj =>
                            {
                                SelectedEmployees = new ObservableCollection<Employee>(employees.Join(SelectedDepartments, x => x.Department, y => y, (x, y) => x));
                            }));
            }
        }

        #endregion

        private void CheckUnsavedData()
        {
            if (updated)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить изменения?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
                //else e.Cancel = true;//Отменяем действие
            }
        }

        private void SaveFile()
        {
            try
            {
                if (dialogService.SaveFileDialog() == true)
                {
                    lastFilePath = dialogService.FilePath;
                    SaveLastFile();
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        private void SaveLastFile()
        {
            try
            {
                fileService.Save(lastFilePath, employees);
                dialogService.ShowMessage("Файл сохранен");
                updated = false;
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        private void Reset()
        {
            Departments.Clear();
            employees.Clear();
            updated = false;
            lastFilePath = null;
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
