using System.Windows.Controls;

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
