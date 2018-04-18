﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AnkiEditor.Models;
using Caliburn.Micro;

namespace AnkiEditor.ViewModels
{
    public class DeckViewModel : PropertyChangedBase
    {
        private readonly Deck _deckModel;
        private readonly Dictionary<string, Models.NoteModel> _noteModels = new Dictionary<string, Models.NoteModel>();

        public DeckViewModel(Models.Deck deckModel)
        {
            _deckModel = deckModel;

            // Create dictionary of noteModels
            foreach (var noteModel in deckModel.note_models)
            {
                _noteModels.Add(noteModel.crowdanki_uuid, noteModel);
                NoteModels.Add(noteModel);
            }

            SelectedNoteModel = NoteModels.First();
            
            foreach (var note in deckModel.notes)
            {
                // Add notes where notemodel exists
                if (_noteModels.TryGetValue(note.note_model_uuid, out var noteModel))
                {
                    NoteViewModels.Add(new NoteViewModel(note, noteModel, this));
                }
            }

            foreach (var lang in InputLanguageManager.Current.AvailableInputLanguages ?? new List<object>())
            {
                Languages.Add(lang.ToString());
            }

            var defaultLang = InputLanguageManager.Current.CurrentInputLanguage.Name;

            foreach (var noteModel in deckModel.note_models)
            {
                var prefix = noteModel.crowdanki_uuid;

                foreach (var fld in noteModel.flds)
                {
                    FieldSettings.Add($"{prefix}_{fld.name}", new FieldSettings(){Language = defaultLang});
                }
            }
        }

        public string Name => _deckModel.name;
        public string Desc => _deckModel.desc;

        public int NoteCount => NoteViewModels.Count;


        public bool CanAddNote => SelectedNoteModel != null;

        public void AddNote()
        {
            var newNote = new NoteViewModel(SelectedNoteModel, this);
            NoteViewModels.Add(newNote);
            ScrollToSelected = true;
            SelectedNoteViewModel = newNote;
        }

        public bool ScrollToSelected
        {
            get => _scrollToSelected;
            set
            {
                _scrollToSelected = value;
                NotifyOfPropertyChange(() => ScrollToSelected);
            }
        }

        public bool CanDeleteNote => SelectedNoteViewModel != null;

        public void DeleteNote()
        {
            NoteViewModels.Remove(SelectedNoteViewModel);
        }


        public ObservableCollection<NoteViewModel> NoteViewModels { get; } = new ObservableCollection<NoteViewModel>();
        private NoteViewModel _selectedNoteViewModel;
        private Models.NoteModel _selectedNoteModel;
        private bool _scrollToSelected;

        public NoteViewModel SelectedNoteViewModel
        {
            get => _selectedNoteViewModel;
            set
            {
                _selectedNoteViewModel = value;
                NotifyOfPropertyChange(() => SelectedNoteViewModel);
                NotifyOfPropertyChange(() => CanDeleteNote);
                NotifyOfPropertyChange(() => SettingsForField);

            }
        }

        public ObservableCollection<Models.NoteModel> NoteModels { get; } = new ObservableCollection<Models.NoteModel>();

        public Models.NoteModel SelectedNoteModel
        {
            get => _selectedNoteModel;
            set
            {
                _selectedNoteModel = value;
                NotifyOfPropertyChange(() => SelectedNoteModel);
            }
        }

        #region FieldSettings


        public ObservableCollection<string> Languages { get; } = new ObservableCollection<string>();

        public Dictionary<string, FieldSettings> FieldSettings = new Dictionary<string, FieldSettings>();
        private string _selectedField;

        public string SelectedField
        {
            get => _selectedField;
            set
            {
                _selectedField = value;
                NotifyOfPropertyChange(() => SelectedField);
                NotifyOfPropertyChange(() => SettingsForField);
            }
        }

        public FieldSettings SettingsForField => FieldSettings.GetValueOrDefault($"{SelectedNoteViewModel.Uuid}_{SelectedField}");

        #endregion

    }
}
