using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ppada.Models
{
    public class Page : INotifyPropertyChanged
    {
        private int _pageId;
        [SQLite.PrimaryKey]
        public int pageId {
            get {
                return _pageId;
            }
            set {
                if (value != _pageId) {
                    _pageId = value;
                    NotifyPropertyChanged("pageId");
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
