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
        
        public ObservableCollection<routine> AllRoutines { get; set; }       
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public ViewModel()
        {
            //initialize tasks and routines           
            //initializeLists(false);
            //SetCurrentFolder(null);
            //FillNotifications();
            //SetFolders1();
        }

        #region Notification Handlers               
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

        


    }
}
