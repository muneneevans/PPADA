using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ppada.Models
{
    public class News : INotifyPropertyChanged
    {
        private int _newsId;
        [SQLite.PrimaryKey]
        public int NewsId
        {
            get
            {
                return _newsId;
            }
            set
            {
                if (value != _newsId)
                {
                    _newsId = value;
                    NotifyPropertyChanged("NewsId");
                }
            }
        }

        private string _topic;
        public string Topic
        {
            get
            {
                return _topic;
            }
            set
            {
                if (value != _topic)
                {
                    _topic = value;
                    NotifyPropertyChanged("Topic");
                }
            }
        }

        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (value != _content)
                {
                    _content = value;
                    NotifyPropertyChanged("Content");
                }
            }
        }


        private DateTime _DatePosted;
        public DateTime DatePosted
        {
            get
            {
                return _DatePosted;
            }
            set
            {
                if (value != _DatePosted)
                {
                    _DatePosted = value;
                    NotifyPropertyChanged("DatePosted");
                }
            }
        }

        private Uri _NewsImageUrl;
        public Uri NewsImageUrl
        {
            get
            {
                return _NewsImageUrl;
            }
            set
            {
                if (value != _NewsImageUrl)
                {
                    _NewsImageUrl = value;
                    NotifyPropertyChanged("NewsImageUrl");
                }
            }
        }


        private String _data;
        public String data
        {
            get
            {
                return _data;
            }
            set
            {
                if (value != _data)
                {
                    _data = value;
                    NotifyPropertyChanged("data");
                }
            }
        }

        private String _present;
        public String Present
        {
            get
            {
                return _present;
            }
            set
            {
                if (value != _present)
                {
                    _present = value;
                    NotifyPropertyChanged("Present");
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
