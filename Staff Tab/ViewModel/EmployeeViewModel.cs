﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Staff_Tab
{
    /// <summary>
    /// Модель представления сотрудника с поддержкой полиморфизма
    /// </summary>
    class EmployeeViewModel: INotifyPropertyChanged
    {
        private Employee employee;
        private SalaryEmployee salaryEmployee;
        private HourlyEmployee hourlyEmployee;
        
        public EmployeeViewModel(Employee employee)
        {
            this.employee = employee;
            switch (employee)
            {
                case SalaryEmployee salaryEmployee:
                    this.salaryEmployee = salaryEmployee;
                    break;
                case HourlyEmployee hourlyEmployee:
                    this.hourlyEmployee = hourlyEmployee;
                    break;
            }
        }
        #region Properties

        public ObservableCollection<Department> Departments
        {
            get => Employee.departments;
            set
            {
                Employee.departments = value;
                OnPropertyChanged();
            }
        }

        public PayFrequency PayFrequency
        {
            get { return employee.PayFrequency; }
            set
            {
                employee.PayFrequency = value;
                OnPropertyChanged();
            }
        }

        public string SecondName
        {
            get { return employee.SecondName; }
            set
            {
                employee.SecondName = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return employee.FirstName; }
            set
            {
                employee.FirstName = value;
                OnPropertyChanged();
            }
        }

        public Department Department
        {
            get { return employee.Department; }
            set
            {
                employee.Department = value;
                OnPropertyChanged();
            }
        }

        public Uri ImageSource
        {
            get { return employee.ImageSource ?? new Uri("pack://application:,,,/Data/DefaultEmployee.jpg"); }
            set
            {
                employee.ImageSource = value;
                OnPropertyChanged();
            }
        }

        public string JobTitles
        {
            get { return employee.JobTitles; }
            set
            {
                employee.JobTitles = value;
                OnPropertyChanged();
            }
        }

        public JobStatus JobStatus
        {
            get { return employee.JobStatus; }
            set
            {
                employee.JobStatus = value;
                OnPropertyChanged();
            }
        }

        public double? AnnualSalary
        {
            get { return salaryEmployee != null ? salaryEmployee.AnnualSalary : null; }

            set
            {
                if (salaryEmployee != null)
                {
                    salaryEmployee.AnnualSalary = value;
                }

                OnPropertyChanged();
            }
        }

        public int? TypicalHours
        {
            get { return hourlyEmployee != null ? hourlyEmployee.TypicalHours : null; }

            set
            {
                if (hourlyEmployee != null)
                {
                    hourlyEmployee.TypicalHours = value;
                }

                OnPropertyChanged();
            }
        }

        public double? HourlyRate
        {
            get { return hourlyEmployee != null ? hourlyEmployee.HourlyRate : null; }

            set
            {
                if (hourlyEmployee != null)
                {
                    hourlyEmployee.HourlyRate = value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        private RelayCommand applyCommand;
        private RelayCommand abortCommand;
        private RelayCommand departmentSelectionChangedCommand;

        public RelayCommand ApplyCommand
        {
            get
            {
                return applyCommand ??
                (applyCommand = new RelayCommand(obj =>
                {
                    DialogCloser.SetDialogResult(obj as Window, true);
                }));

            }
        }

        public RelayCommand AbortCommand
        {
            get
            {
                return abortCommand ??
                (abortCommand = new RelayCommand(obj =>
                {
                    DialogCloser.SetDialogResult(obj as Window, false);
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
                                Department?.Fire(employee);
                                Department = employee.GetDepartment((obj as Department).Title);
                                Department.Hire(employee);
                            }
                            ,obj => obj != null));
            }
        }

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
