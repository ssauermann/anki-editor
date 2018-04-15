using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
