using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace ppada.Models
{
    public enum repeattype
    {
        daily ,
        weekly,
        monthly
    }
    public class routine : INotifyPropertyChanged
    {
        private int _routine_id;
        [SQLite.PrimaryKey , SQLite.AutoIncrement]
        public int routine_id
        {
            get
            {
                return _routine_id;
            }
            set
            {
                if (value != _routine_id)
                {
                    _routine_id = value;
                    NotifyPropertyChanged("routine_id");
                }
            }
        }

        private string _routine_name;
        public string routine_name
        {
            get
            {
                return _routine_name;
            }
            set
            {
                if (value != _routine_name)
                {
                    _routine_name = value;
                    NotifyPropertyChanged("routine_name");
                }
            }
        }

        private string _routine_details;
        public string routine_details
        {
            get
            {
                return _routine_details;
            }
            set
            {
                if (value != _routine_details)
                {
                    _routine_details = value;
                    NotifyPropertyChanged("routine_details");
                }
            }
        }
        
        private DateTime _routine_deadline;
        public DateTime routine_deadline
        {
            get
            {                
                return _routine_deadline;
            }
            set
            {
                if (value != _routine_deadline)
                {
                    _routine_deadline = value;
                    NotifyPropertyChanged("routine_deadline");
                }
            }
        }        

        private bool _routine_status;
        public bool routine_status
        {
            get
            {
                return _routine_status;
            }
            set
            {
                if (value != _routine_status)
                {
                    _routine_status = value;                    
                    NotifyPropertyChanged("routine_status");                    
                }
            }
        }       
       
        private int _routine_timesdone;
        public int routine_timesdone
        {
            get
            {
                return _routine_timesdone;
            }
            set
            {
                _routine_timesdone = value;
                NotifyPropertyChanged("routine_timesdone");
            }
        }

        private int _routine_timesmissed; 
        public int routine_timesmissed
        {
            get
            {
                return _routine_timesmissed;
            }
            set
            {
                _routine_timesmissed = value;
                NotifyPropertyChanged("routine_timesmissed");
            }
        }

        public int _routine_repeattype;
        public int routine_repeattype
        {
            get
            {
                return _routine_repeattype; 
            }
            set
            {
                _routine_repeattype = value;
                NotifyPropertyChanged("routine_repeattype");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));                
            }
        }

        private void StatusChangedHandler()
        {
            try
            {
                switch (this.routine_repeattype)
                {
                    case 1:
                        //daily move deadline to next day
                        this.routine_deadline.AddDays(1);
                        break;
                    case 2:
                        this.routine_deadline.AddDays(7);
                        break;
                    case 3:
                        this.routine_deadline.AddMonths(1);                        
                        break;
                    default:
                        break;
                }
            }
            catch
            {                
            }
        }

    }
}
