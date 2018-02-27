using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ppada.Models;

using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ppada.tile;

namespace ppada.TaskViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {

        #region properties
        DBHelper dbh = new DBHelper();
        public ObservableCollection<Topic> AllTopics { get; set; }
        public ObservableCollection<Note> SearchNotes { get; set; }
        public ObservableCollection<Note> AllBookmarks { get; set; }
        public ObservableCollection<Annotation> AllAnnotations { get; set; }
        public ObservableCollection<routine> AllRoutines { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public ViewModel()
        {
            //initialize topics and Notes
            fetchTopics();
            fetchAnnotations();
            fetchBookmarks();
            //initializeLists(false);
            //SetCurrentFolder(null);
            //FillNotifications();
            //SetFolders1();
        }

        public void Refresh()
        {
            fetchTopics();
            fetchAnnotations();
            fetchBookmarks();
        }

        #region Notification Handlers     
        private void fetchTopics()
        {
            AllTopics = new ObservableCollection<Topic>();
            AllTopics = new ObservableCollection<Topic>(dbh.GetAllTopics());
            NotifyPropertyChanged("AllTopics");
        }
        private void fetchAnnotations()
        {
            AllAnnotations = new ObservableCollection<Annotation>();
            AllAnnotations = new ObservableCollection<Annotation>(dbh.GetAllAnnotations());
            NotifyPropertyChanged("AllAnnotations");
        }
        private void fetchBookmarks()
        {
            AllBookmarks = new ObservableCollection<Note>();
            AllBookmarks = new ObservableCollection<Note>(dbh.GetAllBookmarks());
            NotifyPropertyChanged("AllBookmarks");
        }
        #endregion

        #region INotify Handlers
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private void RoutinesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }
        private void RoutinePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {

            }
            catch { }
        }
        #endregion


        #region Topic Handler
        public Note GetTopicNote(int topicId)
        {
            return dbh.GetTopicNote(topicId);
        }

        public void SearchNote(string text)
        {

            string[] searchTerms = text.Split(' ');

            //Using DataContext.Log is handy 
            //if we want to look at Linq2SQL's generated SQL:
            //dc.Log = new System.IO.StringWriter();

            //Prepare to build a "players" query:
            IQueryable<Note> NotesQuery = (IQueryable<Note>) (dbh.GetAllNotes().ToList<Note>().AsQueryable());

            //Refine our query, one search term at a time:
            foreach (var term in searchTerms)
            {
                //Create (and use) a local variable of the search term
                //to avoid the "outer variable trap":
                //http://stackoverflow.com/questions/3416758
                //http://stackoverflow.com/questions/295593
                var currentTerm = term.Trim();
                NotesQuery = NotesQuery.Where(p => ( p.title.ToLower().Contains(currentTerm.ToLower()) || p.content.ToLower().Contains(currentTerm.ToLower())) );
            }

            //Now we have the complete query. Get the results from the database:
            //var FilteredNotes = NotesQuery.Select(p => new
            //{
            //    p.title,
            //    p.content,
            //    p.id,                
            //}).ToList();

            this.SearchNotes = new ObservableCollection<Note>(NotesQuery);
            //See if the generated SQL looked like it was supposed to:
            NotifyPropertyChanged("SearchNotes");

        }
        #endregion

        #region Annotation handlers
        public void createNewAnnotation(Annotation newAnnotation)
        {
            dbh.createAnnotation(newAnnotation);
            fetchAnnotations();
        }

        public void ToggleBookmark(Note updatingNote)
        {
            updatingNote.bookmarked = !updatingNote.bookmarked;
            dbh.updateNote(updatingNote);
            fetchBookmarks();

        }


        public void DeleteAnnotation(Annotation selectedAnnotation)
        {
            dbh.DeleteAnnotation(selectedAnnotation.id);
            fetchAnnotations();
        }
        #endregion
    }
}
