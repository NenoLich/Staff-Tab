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
using System.Windows.Media;
using System.IO;

namespace Staff_Tab
{
    /// <summary>
    /// Основная модель представления
    /// </summary>
    class StaffTabViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> employees;
        private ObservableCollection<Employee> selectedEmployees;
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
            get => Employee.departments;
            set
            {
                Employee.departments = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Департаменты, используемые для отображения
        /// </summary>
        public ObservableCollection<Department> SelectedDepartments
        {
            get => selectedDepartments;
            set
            {
                selectedDepartments = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Сотрудники, используемые для отображения
        /// </summary>
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
        private RelayCommand selectAllCommand;
        private RelayCommand unSelectAllCommand;
        private RelayCommand closingCommand;
        private RelayCommand addDepartment;
        private RelayCommand addSalaryEmployeeCommand;
        private RelayCommand addHourlyEmployeeCommand;
        private RelayCommand editEmployeeCommand;
        private RelayCommand removeEmployeeCommand;

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

                          Departments = new ObservableCollection<Department>(Employee.departments.Distinct());
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

        /// <summary>
        /// Выделение всех Checkbox'ов внутри передаваемого ListBox'а
        /// </summary>
        public RelayCommand SelectAllCommand
        {
            get
            {
                return selectAllCommand ??
                    (selectAllCommand = new RelayCommand(obj =>
                      {
                          ListBox listBox = obj as ListBox;
                          listBox.SelectAll();
                      },
                      obj=>obj is ListBox));
            }
        }

        /// <summary>
        /// Снятие выделения всех Checkbox'ов внутри передаваемого ListBox'а
        /// </summary>
        public RelayCommand UnSelectAllCommand
        {
            get
            {
                return unSelectAllCommand ??
                    (unSelectAllCommand = new RelayCommand(obj =>
                    {
                        ListBox listBox = obj as ListBox;
                        var checkboxes = FindVisualChildren<CheckBox>(listBox);
                        foreach (CheckBox item in checkboxes)
                        {
                            item.IsChecked = false;
                        }
                    },
                      obj => obj is ListBox));
            }
        }

        /// <summary>
        /// Добавление нового подразделения
        /// </summary>
        public RelayCommand AddDepartment
        {
            get
            {
                return addDepartment ??
                    (addDepartment = new RelayCommand(obj =>
                    {
                        TextBox textBox = obj as TextBox;
                        Department.Create(textBox.Text);
                        textBox.Text = string.Empty;
                    }));
            }
        }

        /// <summary>
        /// Фильтрация подразделений для отображения
        /// </summary>
        public RelayCommand DepartmentSelectionChangedCommand
        {
            get
            {
                return departmentSelectionChangedCommand ??
                            (departmentSelectionChangedCommand = new RelayCommand(obj =>
                            {
                                UpdateSelected();
                            }));
            }
          }

        public RelayCommand AddSalaryEmployeeCommand
        {
            get
            {
                return addSalaryEmployeeCommand ??
                            (addSalaryEmployeeCommand = new RelayCommand(obj =>
                            {
                                Employee employee = new SalaryEmployee();
                                TryAddEmployee(employee);
                            }));
            }
        }

        public RelayCommand AddHourlyEmployeeCommand
        {
            get
            {
                return addHourlyEmployeeCommand ??
                            (addHourlyEmployeeCommand = new RelayCommand(obj =>
                            {
                                Employee employee = new HourlyEmployee();
                                TryAddEmployee(employee);
                            }));
            }
        }

        public RelayCommand EditEmployeeCommand
        {
            get
            {
                return editEmployeeCommand ??
                            (editEmployeeCommand = new RelayCommand(obj =>
                            {
                                if (obj is null)
                                {
                                    return;
                                }
                                Employee employee = obj as Employee;

                                fileService.SaveBeforeEdit("Editable.json", employee);
                                EmployeeView employeeView = new EmployeeView(employee);
                                employeeView.ShowDialog();
                                if (employeeView.DialogResult.HasValue && employeeView.DialogResult.Value)
                                {
                                    Updated = true;
                                    UpdateSelected();
                                }
                                else
                                {
                                    employee = fileService.GetAfterEdit("Editable.json");
                                }
                                File.Delete("Editable.json");
                            }));
            }
        }

        public RelayCommand RemoveEmployeeCommand
        {
            get
            {
                return removeEmployeeCommand ??
                            (removeEmployeeCommand = new RelayCommand(obj =>
                            {
                                if (obj is null)
                                {
                                    return;
                                }
                                employees.Remove(obj as Employee);

                                Updated = true;
                                UpdateSelected();
                            }));
            }
        }

        #endregion

        private void UpdateSelected()
        {
            SelectedEmployees = new ObservableCollection<Employee>(employees.Join(SelectedDepartments, x => x.Department, y => y, (x, y) => x));
        }

        /// <summary>
        /// Проверка наличия несохраненных изменений
        /// </summary>
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

        /// <summary>
        /// Сохранение в файл
        /// </summary>
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

        /// <summary>
        /// Сохранение в последний используемый файл
        /// </summary>
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

        /// <summary>
        /// Попытка добавления нового сотрудника с помощью соответствующего окна
        /// </summary>
        /// <param name="employee"></param>
        private void TryAddEmployee(Employee employee)
        {
            EmployeeView employeeView = new EmployeeView(employee);
            employeeView.ShowDialog();
            if (employeeView.DialogResult.HasValue && employeeView.DialogResult.Value)
            {
                employees.Add(employee);
                Updated = true;
                UpdateSelected();
            }
        }

        /// <summary>
        /// Обнуление основных параметров
        /// </summary>
        private void Reset()
        {
            Departments.Clear();
            employees.Clear();
            updated = false;
            lastFilePath = null;
        }

        /// <summary>
        /// Finds the visual child.
        /// </summary>
        /// <typeparam name="childItem">The type of the child item.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                {
                    yield return (T)child;
                }
                else
                {
                    var childOfChild = FindVisualChildren<T>(child);
                    if (childOfChild != null)
                    {
                        foreach (var subchild in childOfChild)
                        {
                            yield return subchild;
                        }
                    }
                }
            }
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
