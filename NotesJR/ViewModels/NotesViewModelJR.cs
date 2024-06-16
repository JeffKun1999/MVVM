using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace NotesJR.ViewModels
{
    internal class NotesViewModelJR : IQueryAttributable
    {
        public ObservableCollection<ViewModels.NoteViewModelJR> AllNotesPageJR { get; }
        public ICommand NewCommand { get; }
        public ICommand SelectNoteCommand { get; }
        public NotesViewModelJR()
        {
            AllNotesPageJR = new ObservableCollection<ViewModels.NoteViewModelJR>(Models.NoteJR.LoadAll().Select(n => new NoteViewModelJR(n)));
            NewCommand = new AsyncRelayCommand(NewNoteAsync);
            SelectNoteCommand = new AsyncRelayCommand<ViewModels.NoteViewModelJR>(SelectNoteAsync);
        }
        private async Task NewNoteAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.NotePageJR));
        }

        private async Task SelectNoteAsync(ViewModels.NoteViewModelJR note)
        {
            if (note != null)
                await Shell.Current.GoToAsync($"{nameof(Views.NotePageJR)}?load={note.Identifier}");
        }
        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("deleted"))
            {
                string noteId = query["deleted"].ToString();
                NoteViewModelJR matchedNote = AllNotesPageJR.Where((n) => n.Identifier == noteId).FirstOrDefault();

                // If note exists, delete it
                if (matchedNote != null)
                    AllNotesPageJR.Remove(matchedNote);
            }
            else if (query.ContainsKey("saved"))
            {
                string noteId = query["saved"].ToString();
                NoteViewModelJR matchedNote = AllNotesPageJR.Where((n) => n.Identifier == noteId).FirstOrDefault();

                // If note is found, update it
                if (matchedNote != null) {
                    matchedNote.Reload();
                    AllNotesPageJR.Move(AllNotesPageJR.IndexOf(matchedNote), 0);
                }
                

                // If note isn't found, it's new; add it.
                else
                    AllNotesPageJR.Insert(0, new NoteViewModelJR(Models.NoteJR.Load(noteId)));
            }
        }

    }
}
