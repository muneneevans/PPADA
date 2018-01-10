using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ppada.Models
{
    public class Note : INotifyPropertyChanged
    {
        private int _noteId;
        [SQLite.PrimaryKey]
        public int id {
            get {
                return _noteId;
            }
            set {
                if (value != _noteId) {
                    _noteId = value;
                    NotifyPropertyChanged("noteId");
                }
            }
        }

        private string _title;
        public string title {
            get
            {
                return _title;
            }
            set
            {
                if(value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged("title");
                }
            }
        }

        private string _content;
        public string content {
            get
            {
                return _content;
            }
            set
            {
                if(value != _content)
                {
                    _content = value;
                    NotifyPropertyChanged("content");
                }
            }
        }

        private int _topicId;
        [SQLite.PrimaryKey]
        public int topicId
        {
            get
            {
                return _topicId;
            }
            set
            {
                if (value != _topicId)
                {
                    _topicId = value;
                    NotifyPropertyChanged("topicId");
                }
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
