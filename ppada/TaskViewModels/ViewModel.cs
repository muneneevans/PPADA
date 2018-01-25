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
        #endregion

        #region Annotation handlers
        public void createNewAnnotation(Annotation newAnnotation) {
            dbh.createAnnotation(newAnnotation);
            fetchAnnotations();
        }

        public void AddBookmark(Note updatingNote) {
            dbh.updateNote(updatingNote);
            fetchBookmarks();

        }
        #endregion
    }
}
