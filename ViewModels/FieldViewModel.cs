using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;

namespace AnkiEditor.ViewModels
{
    public class FieldViewModel: PropertyChangedBase
    {

        public FieldViewModel(string name)
        {
            Name = name;
        }

        private string _value;
        private string _selectedLanguage;

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                NotifyOfPropertyChange(() => Value);
            }
        }

        public string Name { get; }

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                NotifyOfPropertyChange(() => SelectedLanguage);
            }
        }

        public void TextChanged(RichTextBox sender)
        {

        }
    }
}
