using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ppada.Models;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;


namespace ppada.Views
{
    public class DesignTimeViewModel : INotifyPropertyChanged
    {
        public DesignTimeViewModel()
        {
            
            List<task> nt = new List<task>();
            task NewTask = new task();
            NewTask.task_name = "Task one";
            NewTask.task_details = "Something";
            NewTask.task_deadline = DateTime.Now.Date;
            NewTask.task_priority = 5;
            NewTask.folder_id = 1;
            //set other properites
            AllTasks.Add(NewTask);
            AllTasks.Add(NewTask);
            AllTasks.Add(NewTask);
            AllTasks.Add(NewTask);
            AllTasks.Add(NewTask);
            AllTasks.Add(NewTask);
            NotifyPropertyChanged("AllTasks");
            AllTasks.Add(NewTask);


            List<folder> nf = new List<folder>();
            folder f = new folder();
            f.folder_id = 1;
            f.folder_color = 0xFFFFA500;
            f.folder_name = "some folder";
            AllFolders.Add(f);
            AllFolders.Add(f);
            AllFolders.Add(f);
            AllFolders.Add(f);
            AllFolders.Add(f);
            AllFolders.Add(f);
            NotifyPropertyChanged("AllFolders");
        }
        public ObservableCollection<routine> _AllRoutines;
        public ObservableCollection<routine> AllRoutines
        {
            get
            {
                routine r = new routine();
                r.routine_name = "Routine one";
                r.routine_details = "something";
                r.routine_status = false;
                r.routine_repeattype = 1;
                r.routine_timesdone = 0;
                r.routine_timesmissed = 0;
                _AllRoutines.Add(r);
                _AllRoutines.Add(r);
                _AllRoutines.Add(r);
                _AllRoutines.Add(r);
                _AllRoutines.Add(r);
                NotifyPropertyChanged("AllRoutines");
                return _AllRoutines;
            }
            set
            {
                _AllRoutines = value;
                NotifyPropertyChanged("AllRoutines");
            }
        }
        public ObservableCollection<task> AllTasks { get; set; }
        public ObservableCollection<folder> AllFolders { get; set; }
        public folder CurrentFolder = null;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class CheckBoxToEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            CheckBox check = (CheckBox)value;
            return check.IsChecked;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class ColorItemToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                return new SolidColorBrush((Color)value);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
        private Color ConvertColor(uint uintCol)
        {
            byte A = (byte)((uintCol & 0xFF000000) >> 24);
            byte R = (byte)((uintCol & 0x00FF0000) >> 16);
            byte G = (byte)((uintCol & 0x0000FF00) >> 8);
            byte B = (byte)((uintCol & 0x000000FF) >> 0);

            return Color.FromArgb(A, R, G, B); ;
        }
    }
    public class FolderToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            uint color = (uint)value;
            if (value != null)
            {
                return new SolidColorBrush(ConvertColor(color));
            }
            return new SolidColorBrush(Windows.UI.Colors.Orange);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
        private Color ConvertColor(uint uintCol)
        {
            byte A = (byte)((uintCol & 0xFF000000) >> 24);
            byte R = (byte)((uintCol & 0x00FF0000) >> 16);
            byte G = (byte)((uintCol & 0x0000FF00) >> 8);
            byte B = (byte)((uintCol & 0x000000FF) >> 0);

            return Color.FromArgb(A, R, G, B); ;
        }
    }
    public class BooleanToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool status = (bool)value;
            if (status == true)
            {
                return new SymbolIcon(Symbol.Accept);
            }
            else
            {
                return new Symbol();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class FolderToHeader : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                //set heder to all 
                return "All Tasks";
            }
            else
            {
                try
                {
                    return ((folder)value).folder_name;
                }
                catch
                {
                    return "All Tasks";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class SelectedFolderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ObservableCollection<folder> folder = (ObservableCollection<folder>)value;
            if (App.vm.CurrentFolder == null)
            {
                return null;
            }
            else
            {
                folder f = folder.FirstOrDefault(x => x.folder_id == App.vm.CurrentFolder.folder_id);
                return f;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class BoolToCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.Equals(true))
            {
                return true;
            }
            else return false;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.Equals(true))
            {
                return true;
            }
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value == true)
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if ((bool)value == true)
                return true;
            else
                return false;
        }
    }
    public class DateToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //
            try
            {
                //convert the timespan to days                
                DateTime Deadline = (DateTime)value;
                TimeSpan TimeLeft = Deadline.Subtract(DateTime.Now);
                if (TimeLeft.Days == 0)
                {
                    //is due today gett the hours left
                    if (TimeLeft.Hours > 0)
                    {
                        return TimeLeft.Hours + " hours left";
                    }
                    else
                    {
                        return "Minutes left !";
                    }
                }
                else if (TimeLeft.Days > 0)
                {
                    // some days left
                    if (TimeLeft.Days == 1)
                    {
                        return "due tommorow";
                    }
                    else if (TimeLeft.Days == 7)
                    {
                        return "1 week left";
                    }
                    else if (TimeLeft.Days == 14)
                    {
                        return "2 weeks left";
                    }
                    else
                    {
                        return TimeLeft.Days + " days left";
                    }
                }
                else
                {
                    // is over due
                    return "Overdue !";
                }
            }
            catch
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class DateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //
            try
            {
                //convert the timespan to days
                DateTime Deadline = (DateTime)value;
                TimeSpan TimeLeft = Deadline.Subtract(DateTime.Now);
                if (TimeLeft.Days == 0)
                {
                    //is due today get the hours left
                    return new SolidColorBrush(Windows.UI.Colors.Orange);
                }
                else if (TimeLeft.Days > 0)
                {
                    //some days left
                    return new SolidColorBrush();
                }
                else
                {
                    // is over due
                    return new SolidColorBrush(Windows.UI.Colors.Red);
                }
            }
            catch
            {
                return new SolidColorBrush(Windows.UI.Colors.Red);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class ListToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var x = (ObservableCollection<task>)value;
            if (x.Count == 0)
            {
                return Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                return Windows.UI.Xaml.Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class ListToInvertedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var x = (ObservableCollection<task>)value;
            if (x.Count == 0)
            {
                return Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                return Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class RoutinesListToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var x = (ObservableCollection<routine>)value;
            if (x.Count == 0)
            {
                return Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                return Windows.UI.Xaml.Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class RoutinesListToInvertedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var x = (ObservableCollection<routine>)value;
            if (x.Count == 0)
            {
                return Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                return Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class ListToPendingItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                ObservableCollection<task> AllTasks = new ObservableCollection<task>((ObservableCollection<task>)value);
                return (new ObservableCollection<task>((from item in AllTasks where item.task_status == false select item).ToList()));
            }
            catch { return new ObservableCollection<task>(); }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class ListToCompletedItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                ObservableCollection<task> AllTasks = new ObservableCollection<task>((ObservableCollection<task>)value);
                return (new ObservableCollection<task>((from item in AllTasks where item.task_status == true select item).ToList()));
            }
            catch { return new ObservableCollection<task>(); }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class ListToRoutineItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                ObservableCollection<routine> AllRoutines = new ObservableCollection<routine>((ObservableCollection<routine>)value);
                return (new ObservableCollection<routine>((from item in AllRoutines where item.routine_status == false select item).ToList()));
            }
            catch { return new ObservableCollection<task>(); }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    class ValueConverters
    {
    }
}
