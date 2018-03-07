using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Staff_Tab
{
    /// <summary>
    /// Текстбокс с возможностью проверки вводимых данных
    /// </summary>
    class AutoValidatedTextBox: TextBox
    {
        private Regex regex;

        public string Regex
        {
            get=> regex.ToString();
            set
            {
                regex = new Regex(value);
            }
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (regex != null || !regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
            base.OnPreviewTextInput(e);
        }
    }
}
