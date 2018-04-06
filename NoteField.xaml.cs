using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UserControl = System.Windows.Controls.UserControl;

namespace AnkiEditor
{
    /// <summary>
    /// Interaction logic for NoteField.xaml
    /// </summary>
    public partial class NoteField : UserControl
    {
        public NoteField()
        {
            InitializeComponent();
            FieldMirrorItems = new List<NoteField>();
            FieldScriptItems = new List<string>();
            FieldLanguageItems = new List<string>();
            FieldText = string.Empty;
        }

        private Script CurrentScript;

        #region Properties

        public static readonly DependencyProperty FieldTextProperty =
            DependencyProperty.Register(nameof(FieldText), typeof(string), typeof(NoteField));

        public string FieldText
        {
            get => (string)GetValue(FieldTextProperty);
            set => SetValue(FieldTextProperty, value);
        }

        public static readonly DependencyProperty FieldNameProperty =
            DependencyProperty.Register(nameof(FieldName), typeof(string), typeof(NoteField));

        public string FieldName
        {
            get => (string)GetValue(FieldNameProperty);
            set => SetValue(FieldNameProperty, value);
        }

        public static readonly DependencyProperty FieldKeepProperty =
            DependencyProperty.Register(nameof(FieldKeep), typeof(bool), typeof(NoteField));

        public bool FieldKeep
        {
            get => (bool)GetValue(FieldKeepProperty);
            set => SetValue(FieldKeepProperty, value);
        }

        public static readonly DependencyProperty FieldMirrorIndexProperty =
            DependencyProperty.Register(nameof(FieldMirrorIndex), typeof(int), typeof(NoteField));

        public int FieldMirrorIndex
        {
            get => (int)GetValue(FieldMirrorIndexProperty);
            set => SetValue(FieldMirrorIndexProperty, value);
        }

        public static readonly DependencyProperty FieldMirrorItemsProperty =
            DependencyProperty.Register(nameof(FieldMirrorItems), typeof(IList<NoteField>), typeof(NoteField),
                new PropertyMetadata(new List<NoteField>()));

        public IList<NoteField> FieldMirrorItems
        {
            get => (IList<NoteField>)GetValue(FieldMirrorItemsProperty);
            set => SetValue(FieldMirrorItemsProperty, value);
        }

        public static readonly DependencyProperty FieldScriptIndexProperty =
            DependencyProperty.Register(nameof(FieldScriptIndex), typeof(int), typeof(NoteField));

        public int FieldScriptIndex
        {
            get => (int)GetValue(FieldScriptIndexProperty);
            set => SetValue(FieldScriptIndexProperty, value);
        }

        public static readonly DependencyProperty FieldScriptItemsProperty =
            DependencyProperty.Register(nameof(FieldScriptItems), typeof(IList<string>), typeof(NoteField), new PropertyMetadata(new List<string>()));

        public IList<string> FieldScriptItems
        {
            get => (IList<string>)GetValue(FieldScriptItemsProperty);
            set => SetValue(FieldScriptItemsProperty, value);
        }


        public static readonly DependencyProperty FieldLanguageIndexProperty =
            DependencyProperty.Register(nameof(FieldLanguageIndex), typeof(int), typeof(NoteField));

        public int FieldLanguageIndex
        {
            get => (int)GetValue(FieldLanguageIndexProperty);
            set => SetValue(FieldLanguageIndexProperty, value);
        }

        public static readonly DependencyProperty FieldLanguageItemsProperty =
            DependencyProperty.Register(nameof(FieldLanguageItems), typeof(IList<string>), typeof(NoteField), new PropertyMetadata(new List<string>()));

        public IList<string> FieldLanguageItems
        {
            get => (IList<string>)GetValue(FieldLanguageItemsProperty);
            set => SetValue(FieldLanguageItemsProperty, value);
        }

        #endregion

        #region Events
        /*
        public event MouseButtonEventHandler LeftBlockMouseDown;
        private void LeftBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (LeftBlockMouseDown != null) LeftBlockMouseDown(this, e);
            e.Handled = true;
        }*/

        #endregion

        public void Reset()
        {
            if (!FieldKeep)
            {
                FieldText = string.Empty;
            }
        }

        public override string ToString()
        {
            return FieldName;
        }

        private void CmbLanguage_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedLanguage = (string)CmbLanguage.SelectedItem;
            if (selectedLanguage != null)
                InputLanguageManager.SetInputLanguage(TxtBox, CultureInfo.CreateSpecificCulture(selectedLanguage));
        }

        public event TextChangedEventHandler OnTextChanged;
        private void TxtBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            OnTextChanged?.Invoke(sender, e);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentScript?.Stop();
            var selectedScript = (string)CmbScript.SelectedItem;

            switch (selectedScript)
            {
                case "Furigana":
                    CurrentScript = new FuriganaScript(this, FieldMirrorItems[FieldMirrorIndex]);
                    break;
                case "DictionaryForm":
                    CurrentScript = new DictionaryFormScript(this, FieldMirrorItems[FieldMirrorIndex]);
                    break;
                case "Notes":
                    CurrentScript = new NotesScript(this, FieldMirrorItems[FieldMirrorIndex]);
                    break;
            }

            CurrentScript?.Start();
        }

        public event RoutedEventHandler TextGotFocus;

        private void TxtBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextGotFocus?.Invoke(sender, e);
        }

        public event RoutedEventHandler TextLostFocus;

        private void TxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextLostFocus?.Invoke(sender, e);
        }

    }
}
