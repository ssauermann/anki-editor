using System.Windows.Controls;
using AnkiEditor.PropertyExtensions;

namespace AnkiEditor.Views
{
    /// <summary>
    /// Interaktionslogik für DeckView.xaml
    /// </summary>
    public partial class DeckView : UserControl
    {
        public DeckView()
        {
            InitializeComponent();
        }

        //TODO Scroll when Tag is updated, not only on selection changed
        private void NoteViewModels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NoteViewModels.Tag as bool? != true) return;
            NoteViewModels.ScrollToCenterOfView(NoteViewModels.SelectedItem);
            NoteViewModels.Tag = false;
        }
    }
}
