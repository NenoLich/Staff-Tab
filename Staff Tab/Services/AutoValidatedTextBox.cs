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
                regex = new Regex(@""+value);
            }
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (regex != null && !regex.IsMatch(Text+e.Text))
            {
                e.Handled = true;
            }
            base.OnPreviewTextInput(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    e.Handled=IsMatch(Text + " ");
                    break;
                case Key.Decimal:
                    e.Handled = IsMatch(Text + ".");
                    break;
            }

            base.OnPreviewKeyDown(e);
        }

        private bool IsMatch(string text)
        {
            return regex != null && !regex.IsMatch(text);
        }
    }
}
