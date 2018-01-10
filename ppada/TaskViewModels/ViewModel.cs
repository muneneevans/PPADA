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
        public ObservableCollection<routine> AllRoutines { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public ViewModel()
        {
            //initialize topics and Notes
            fetchTopics();
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

    }
}
