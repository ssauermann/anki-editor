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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnkiEditor.Views
{
    /// <summary>
    /// Interaktionslogik für NoteView.xaml
    /// </summary>
    public partial class NoteView : UserControl
    {
        public NoteView()
        {
            InitializeComponent();

            /*Tokenizer.TokenMatcher = text =>
            {
                if (text.EndsWith(" "))
                {
                    return text.Substring(0, text.Length - 1).Trim();
                }

                return null;
            };*/
        }
    }
}
