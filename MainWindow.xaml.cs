using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AnkiEditor.Scripts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace AnkiEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string selectedFile = "";
        private NoteModel noteModel;
        private readonly List<NoteField> fields = new List<NoteField>();
        private dynamic json;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnOpenDeck_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";

            if (fileDialog.ShowDialog() == true)
            {
                selectedFile = fileDialog.FileName;
                LoadJson();
            }
        }

        private void LoadJson()
        {
            string jsonString = File.ReadAllText(selectedFile);
            dynamic data = json = JsonConvert.DeserializeObject(jsonString);

            dynamic nm = data.note_models.First;

            noteModel = new NoteModel((string)nm.crowdanki_uuid) { NoteCount = (int)data.notes.Count };
            foreach (var field in nm.flds)
            {
                noteModel.fields.Add((string)field.name);
            }

            noteModel.fields.Add("_tags_");

            UpdateTextboxes();
            UpdateNoteCount();
        }

        private void UpdateTextboxes()
        {
            dpWithBoxes.Children.Clear();
            fields.Clear();

            foreach (var field in noteModel.fields)
            {

                var noteField = new NoteField() { FieldName = field };

                DockPanel.SetDock(noteField, Dock.Top);
                dpWithBoxes.Children.Add(noteField);
                fields.Add(noteField);
            }

            var languages = InputLanguageManager.Current.AvailableInputLanguages ?? new List<object>();
            var languageList = new List<string>();
            var currentLang = InputLanguageManager.Current.CurrentInputLanguage.ToString();

            foreach (var l in languages)
            {
                languageList.Add(l.ToString());
            }

            foreach (var field in fields)
            {
                var fieldList = new List<NoteField> { new NoteField() { FieldName = "<None>" } };
                fieldList.AddRange(fields);
                fieldList.Remove(field);
                field.FieldMirrorItems = fieldList;
                field.FieldMirrorIndex = 0;

                field.FieldLanguageItems = languageList;
                field.FieldLanguageIndex = languageList.FindIndex(x => x == currentLang);

                field.FieldScriptItems = new List<EScripts>(Enum.GetValues(typeof(EScripts)).Cast<EScripts>());
                field.FieldScriptIndex = 0;

            }

        }

        private void UpdateNoteCount()
        {
            statBarNoteCount.Content = "#notes: " + noteModel.NoteCount;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            dynamic newNote = new JObject();
            newNote.__type__ = "Note";
            newNote.data = "";

            var tagArray = new JArray();

            var fieldData = new JArray();
            foreach (var field in fields)
            {
                if (field.FieldName != "_tags_")
                {
                    fieldData.Add(field.FieldText);
                }
                else
                {
                    foreach (var tag in field.FieldText.Trim().Split(' '))
                    {
                        if (tag != "")
                            tagArray.Add(tag);
                    }
                }
            }

            newNote.fields = fieldData;
            newNote.flags = 0;
            newNote.guid = Utils.Guid64();
            newNote.note_model_uuid = noteModel.uuid;
            newNote.tags = tagArray;


            using (StreamWriter file = new StreamWriter(selectedFile))
            {
                json.notes.Add(newNote);
                JsonSerializer serializer = new JsonSerializer() { Formatting = Formatting.Indented };
                serializer.Serialize(file, json);
            }

            foreach (var field in fields)
            {
                field.Reset();
            }

            noteModel.NoteCount++;
            UpdateNoteCount();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //aaa.Text = (new FuriganaScript(null)).NihongoderaQuery("市役所の 電話番号を 知っていますか。 はい、知っています。 いいえ、知りません。");

        }
    }
}
