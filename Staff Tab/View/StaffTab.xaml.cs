using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Staff_Tab
{
    /// <summary>
    /// Логика взаимодействия для StaffTab.xaml
    /// </summary>
    public partial class StaffTab : Window
    {
        public StaffTab()
        {
            InitializeComponent();

            IFileService JsonFileService = new JsonFileService();
            IDialogService defaultDialogService = new DefaultDialogService();
            DataContext = new StaffTabViewModel(JsonFileService, defaultDialogService);
        }
    }
}
