using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ppada.Models
{
    public class Topic : INotifyPropertyChanged
    {
        private int _id;
        [SQLite.PrimaryKey]
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("id");
                }
            }
        }

        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("title");
                }
            }
        }


        private int _sectionid;

        public int sectionId
        {
            get
            {
                return _sectionid;
            }
            set
            {
                if (value != _sectionid)
                {
                    _sectionid = value;
                    NotifyPropertyChanged("sectionId");
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
