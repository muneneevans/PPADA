using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace toodo_wp.Models
{
    public class task : INotifyPropertyChanged
    {
        private int _task_id;
        [SQLite.PrimaryKey , SQLite.AutoIncrement]
        public int task_id
        {
            get
            {
                return _task_id;
            }
            set
            {
                if (value != _task_id)
                {
                    _task_id = value;
                    NotifyPropertyChanged("task_id");
                }
            }
        }

        private string _task_name;
        public string task_name
        {
            get
            {
                return _task_name;
            }
            set
            {
                if (value != _task_name)
                {
                    _task_name = value;
                    NotifyPropertyChanged("task_name");
                }
            }
        }

        private string _task_details;
        public string task_details
        {
            get
            {
                return _task_details;
            }
            set
            {
                if (value != _task_details)
                {
                    _task_details = value;
                    NotifyPropertyChanged("task_details");
                }
            }
        }

        private int _task_priority;
        public int task_priority
        {
            get
            {
                return _task_priority;
            }
            set
            {
                if (value != _task_priority)
                {
                    _task_priority = value;
                    NotifyPropertyChanged("task_priority");
                }
            }
        }

        private DateTime _task_deadline;
        public  DateTime task_deadline
        {
            get
            {
                return _task_deadline;
            }
            set
            {
                if (value != _task_deadline)
                {
                    _task_deadline = value;
                    NotifyPropertyChanged("task_deadline");
                }
            }
        }       

        private bool _task_status;
        public bool task_status
        {
            get
            {
                return _task_status;
            }
            set
            {
                if (value != _task_status)
                {
                    _task_status = value;
                    NotifyPropertyChanged("task_status");
                }
            }
        }
        

        private int _folder_id;
        public int folder_id
        {
            get
            {
                return _folder_id;
            }
            set
            {
                _folder_id = value;
                NotifyPropertyChanged("folder_id");
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
    }
}
