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
            if (NoteViewModelsSorted.Tag as bool? != true) return;
            NoteViewModelsSorted.ScrollToCenterOfView(NoteViewModelsSorted.SelectedItem);
            NoteViewModelsSorted.Tag = false;
        }
    }
}
