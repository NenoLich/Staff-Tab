using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

        public string Department
        {
            get { return employee.Department.Title; }
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

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
