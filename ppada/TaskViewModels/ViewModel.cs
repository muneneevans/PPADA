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
        public ObservableCollection<task> AllTasks { get; set; }
        public ObservableCollection<folder> AllFolders { get; set; }
        public folder CurrentFolder { get; set; }
        public bool InFolderPage = false;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public ViewModel()
        {
            //initialize tasks and routines           
            initializeLists(false);
            SetCurrentFolder(null);
            FillNotifications();
            //SetFolders1();
        }

        #region Notification Handlers
        public async void FillNotifications()
        {
            await TileManager.DefaultTile(AllTasks.ToList());
        }
        public async Task FillNotifications(string FolderName)
        {
            await TileManager.DefaultTile(AllTasks.ToList(), FolderName);
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
        private void TasksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                foreach (task T in AllTasks)
                {
                    T.PropertyChanged += TaskPropertyChanged;
                }

            }
            catch
            { }
        }
        private void TaskPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "task_status")
                {
                    task t = (task)sender;
                    if (!InFolderPage)
                    {
                        AllTasks.Remove(t);
                    }
                    NotifyPropertyChanged("AllTasks");
                    dbh.UpdateTask(t);
                }
            }
            catch { }
        }

        private void RoutinesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                foreach (task T in AllTasks)
                {
                    T.PropertyChanged += RoutinePropertyChanged;
                }
            }
            catch
            { }
        }
        private void RoutinePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "routine_status")
                {
                    routine r = (routine)sender;
                    AllRoutines.Remove(r);
                    NotifyPropertyChanged("AllRoutines");
                    //r.routine_timesdone++;
                    dbh.UpdateRoutine(r);
                }

            }
            catch { }
        }
        #endregion

        #region list handlers   
        private void initializeLists(bool status)
        {
            AllTasks = new ObservableCollection<task>();
            AllRoutines = new ObservableCollection<routine>();
            AllFolders = new ObservableCollection<folder>();            
            AllTasks = new ObservableCollection<task>(dbh.GetTasksByStatus(status).OrderByDescending(order => order.task_priority).ThenBy(order => order.task_deadline));
            AllRoutines = new ObservableCollection<routine>(dbh.GetPendingRoutinesByStatus(status).OrderBy(order => order.routine_deadline).ToList());
            AllFolders = new ObservableCollection<folder>(dbh.GetAllFolders());
            foreach (task t in AllTasks)
            {
                t.PropertyChanged += TaskPropertyChanged;
            }
            foreach (routine r in AllRoutines)
            {
                ValidateRoutine(r);
                r.PropertyChanged += RoutinePropertyChanged;
            }
            NotifyPropertyChanged("AllTasks");
            NotifyPropertyChanged("AllRoutines");
            NotifyPropertyChanged("AllFolders");
        }
        public void RefreshList( )
        {
            try
            {
                                              
            }
            catch { }
        }        
        #endregion

        #region Routine Handlers
        public DateTime ComputeDeadline(routine NewRoutine)
        {
            try
            {
                // get the type of the routine
                DateTime NewDeadline = DateTime.Now;
                var today = NewDeadline;
                var yesterday = NewDeadline.AddDays(-1);
                var thisWeekStart = NewDeadline.AddDays(-(int)NewDeadline.DayOfWeek);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                var lastWeekStart = thisWeekStart.AddDays(-7);
                var lastWeekEnd = thisWeekStart.AddSeconds(-1);
                var thisMonthStart = NewDeadline.AddDays(1 - NewDeadline.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                var lastMonthStart = thisMonthStart.AddMonths(-1);
                var lastMonthEnd = thisMonthStart.AddSeconds(-1);
                var nextMonthStart = thisMonthStart.AddMonths(1);
                var nextMonthEnd = nextMonthStart.AddMonths(1).AddDays(-1);

                DateTime ReturnDate = NewDeadline;

                switch (NewRoutine.routine_repeattype)
                {
                    case 1:
                        //daily move deadline to next day
                        ReturnDate = new DateTime(NewDeadline.Year, NewDeadline.Month, NewDeadline.Day, 23, 59, 30);
                        break;
                    case 2:
                        ReturnDate = new DateTime(thisWeekEnd.Year, thisWeekEnd.Month, thisWeekEnd.Day, 23, 59, 30);
                        break;
                    case 3:
                        ReturnDate = new DateTime(nextMonthEnd.Year, nextMonthEnd.Month, nextMonthEnd.Day, 23, 59, 30);
                        break;
                    default:
                        break;
                }
                return ReturnDate;
            }
            catch { return new DateTime(); }
        }
        public void ValidateRoutine(routine r)
        {
            //check if the deadline has been met
            if (r.routine_deadline <= DateTime.Now)
            {
                if (r.routine_status == true)
                {
                    r.routine_timesdone++;
                    r.routine_status = false;
                }
                else
                {
                    r.routine_timesmissed++;
                }
                r.routine_deadline = ComputeDeadline(r);
                UpdateRoutine(r);
            }
            NotifyPropertyChanged("AllRoutines");
        }
        public void UpdateRoutine(routine NewRoutine)
        {
            try
            {
                var found = AllRoutines.FirstOrDefault(x => x.routine_id == NewRoutine.routine_id);
                found = NewRoutine;
                AllTasks = new ObservableCollection<task>(AllTasks.OrderByDescending(order => order.task_priority).ThenBy(order => order.task_deadline));
                NotifyPropertyChanged("AllRoutines");
                dbh.UpdateRoutine(NewRoutine);
            }
            catch { }
        }
        public void AddRoutine(routine NewRoutine)
        {
            try
            {
                NewRoutine.routine_deadline = ComputeDeadline(NewRoutine);
                AllRoutines.Add(NewRoutine);
                NewRoutine.PropertyChanged += RoutinePropertyChanged;
                AllRoutines = new ObservableCollection<routine>(AllRoutines.OrderBy(order => order.routine_deadline).ToList());
                NotifyPropertyChanged("AllRoutines");
                dbh.InsertRoutine(NewRoutine);

            }
            catch { }
        }
        public void DeleteAllRoutines()
        {
            try
            {
                AllRoutines = new ObservableCollection<routine>();
                NotifyPropertyChanged("AllRoutines");
                dbh.DeleteAllRoutines();
            }
            catch { }
        }
        public void RemoveRoutine(routine RoutineToBeRemoved)
        {
            try
            {
                var found = AllRoutines.FirstOrDefault(x => x.routine_id == RoutineToBeRemoved.routine_id);
                AllRoutines.Remove(found);
                NotifyPropertyChanged("AllRoutines");
                dbh.DeleteRoutine(RoutineToBeRemoved.routine_id);
            }
            catch { }
        }
        #endregion

        #region task Handlers
        public DateTime ComputeDeadline(int DateType)
        {
            try
            {
                // get the type of the routine
                DateTime NewDeadline = DateTime.Now;
                var today = NewDeadline;
                var yesterday = NewDeadline.AddDays(-1);
                var thisWeekStart = NewDeadline.AddDays(-(int)NewDeadline.DayOfWeek);
                var thisWeekEnd = thisWeekStart.AddDays((int)7).AddSeconds((double)-1);
                var lastWeekStart = thisWeekStart.AddDays(-7);
                var lastWeekEnd = thisWeekStart.AddSeconds(-1);
                var thisMonthStart = NewDeadline.AddDays(1 - NewDeadline.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                var lastMonthStart = thisMonthStart.AddMonths(-1);
                var lastMonthEnd = thisMonthStart.AddSeconds(-1);
                var nextMonthStart = thisMonthStart.AddMonths(1);
                var nextMonthEnd = nextMonthStart.AddMonths(1).AddDays(-1);

                DateTime ReturnDate = new DateTime();

                switch (DateType)
                {
                    case 1:
                        //daily move deadline to next day
                        ReturnDate = new DateTime(NewDeadline.Year, NewDeadline.Month, NewDeadline.Day, 23, 59, 30);
                        break;
                    case 2:
                        ReturnDate = new DateTime(thisWeekEnd.Year, thisWeekEnd.Month, thisWeekEnd.Day, 23, 59, 30);
                        break;
                    case 3:
                        ReturnDate = new DateTime(nextMonthEnd.Year, nextMonthEnd.Month, nextMonthEnd.Day, 23, 59, 30);
                        break;
                    default:
                        break;
                }
                return ReturnDate;
            }
            catch { return new DateTime(); }
        }
        public void UpdateTask(task NewTask)
        {
            try
            {
                var found = AllTasks.FirstOrDefault(x => x.task_id == NewTask.task_id);
                found = NewTask;
                AllTasks = new ObservableCollection<task>(AllTasks.OrderByDescending(order => order.task_priority).ThenBy(order => order.task_deadline));
                NotifyPropertyChanged("AllTasks");
                dbh.UpdateTask(NewTask);
                FillNotifications();
                ToastManager.ToastManger.UpdateToast(NewTask);
            }
            catch { }
        }
        public void GetAllPendingTasks()
        {
            initializeLists(false);
        }
        
        public void FilterTasksByFolder(folder SelectedFolder)
        {
            if (SelectedFolder == null)
            {
                //get all Tasks
                initializeLists(false);
            }
            else
            {
                AllTasks = new ObservableCollection<task>(dbh.GetTasksByFolder(SelectedFolder).OrderBy(order => order.task_status));
                foreach (task t in AllTasks)
                {
                    t.PropertyChanged += TaskPropertyChanged;
                }
                NotifyPropertyChanged("AllTasks");
            }
            
        }
        public async void FilterPendingTasksByFolder(folder SelectedFolder)
        {
            AllTasks = new ObservableCollection<task>(dbh.GetPendingTasksByFolder(SelectedFolder).OrderByDescending(order => order.task_priority).ThenBy(order => order.task_deadline));
            foreach (task t in AllTasks)
            {
                t.PropertyChanged += TaskPropertyChanged;
            }
            NotifyPropertyChanged("AllTasks");
            try
            {
                await FillNotifications(SelectedFolder.folder_name);
            }
            catch { }
        }
        public void FilterTasksByToday()
        {
            try
            {
                AllTasks = new ObservableCollection<task>(dbh.GetPendingTasksByFolder(CurrentFolder).Where(x => x.task_deadline.Date == DateTime.Now.Date));
            }
            catch
            {
                AllTasks = new ObservableCollection<task>(dbh.GetTasksByStatus(false).Where(x => x.task_deadline.Date == DateTime.Now.Date));
            }
            foreach (task t in AllTasks)
            {
                t.PropertyChanged += TaskPropertyChanged;
            }
            AllTasks = new ObservableCollection<task>(AllTasks.OrderByDescending(order => order.task_priority).ThenBy(order => order.task_deadline));
            foreach (task t in AllTasks)
            {
                t.PropertyChanged += TaskPropertyChanged;
            }
            NotifyPropertyChanged("AllTasks");
            FillNotifications("Today");
        }
        public void FilterTasksByWeek()
        {
            DateTime weekstart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            DateTime StartOfWeek = new DateTime(weekstart.Year, weekstart.Month, weekstart.Day, 23, 59, 30);
            int daysToOffset = ((int)DateTime.Now.DayOfWeek + 1) * 1;
            DateTime weekend = DateTime.Now.AddDays(daysToOffset);
            //DateTime EndOfWeek = new DateTime(weekend.Year, weekend.Month, weekend.Day, 23, 59, 30);
            DateTime EndOfWeek = StartOfWeek.AddDays(6) ;

            try
            {
                AllTasks = new ObservableCollection<task>(dbh.GetPendingTasksByFolder(CurrentFolder).Where(x => x.task_deadline.Date >= StartOfWeek.Date));
                AllTasks = new ObservableCollection<task>(AllTasks.Where(x => x.task_deadline.Date <= EndOfWeek.Date));
            } catch
            {
                AllTasks = new ObservableCollection<task>(dbh.GetTasksByStatus(false).Where(x => x.task_deadline.Date >= StartOfWeek.Date));
                AllTasks = new ObservableCollection<task>(AllTasks.Where(x => x.task_deadline.Date <= EndOfWeek.Date));
            }
            
            AllTasks = new ObservableCollection<task>(AllTasks.OrderByDescending(order => order.task_priority).ThenBy(order => order.task_deadline));
            foreach (task t in AllTasks)
            {
                t.PropertyChanged += TaskPropertyChanged;
            }
            NotifyPropertyChanged("AllTasks");
        }

        public void AddTask(task NewTask)
        {
            try
            {
                AllTasks.Add(NewTask);
                NewTask.PropertyChanged += TaskPropertyChanged;
                AllTasks = new ObservableCollection<task>(AllTasks.OrderByDescending(order => order.task_priority).ThenBy(order => order.task_deadline));
                NotifyPropertyChanged("AllTasks");
                dbh.InsertTask(NewTask);


                ToastManager.ToastManger.CreateNewToast(NewTask);
                
            }
            catch { }
        }
        public void AddTaskWithReminder(task NewTask , DateTime RemindTime)
        {
            try
            {
                AllTasks.Add(NewTask);
                NewTask.PropertyChanged += TaskPropertyChanged;
                AllTasks = new ObservableCollection<task>(AllTasks.OrderByDescending(order => order.task_priority).ThenBy(order => order.task_deadline));
                NotifyPropertyChanged("AllTasks");
                dbh.InsertTask(NewTask);
                ToastManager.ToastManger.CreateCustomToast(NewTask, RemindTime);
                
            }
            catch { }
        }
        public void DeleteAllTasks()
        {
            try
            {
                AllTasks = new ObservableCollection<task>();
                NotifyPropertyChanged("AllTasks");
                dbh.DeleteAllTasks();                
            }
            catch { }
        }
        public void RemoveTask(task TaskToBeRemoved)
        {
            try
            {
                var found = AllTasks.FirstOrDefault(x => x.task_id == TaskToBeRemoved.task_id);
                AllTasks.Remove(found);
                NotifyPropertyChanged("AllTasks");
                dbh.DeleteTask(TaskToBeRemoved.task_id);
            }
            catch { }
        }
        public async void RemoveAllTasks()
        {
            try
            {
                AllTasks = new ObservableCollection<task>();
                //await Model.WriteToTasksFileAsync(JsonConvert.SerializeObject(AllTasks));
                Debug.WriteLine("task removed");
            }
            catch { }
        }
        #endregion

        #region folder Handlers
        public void SetCurrentFolder(folder NewCurrentFolder)
        {
            CurrentFolder = NewCurrentFolder;
            NotifyPropertyChanged("CurrentFolder");
        }
        public void UnSetCurrentFolder()
        {
            CurrentFolder = null;
            NotifyPropertyChanged("CurrentFolder");
        }
        public void SetCurrentFolder(int NewCurrentFolderId)
        {
            var found = AllFolders.FirstOrDefault(x => x.folder_id == NewCurrentFolderId);
            CurrentFolder = (folder)found;
            NotifyPropertyChanged("CurrentFolder");
        }
        public void UpdateFolder(folder NewFolder)
        {
            try
            {
                var found = AllFolders.FirstOrDefault(x => x.folder_id == NewFolder.folder_id);
                found = NewFolder;
                NotifyPropertyChanged("AllFolders");
                NotifyPropertyChanged("CurrentFolder");
                dbh.UpdateFolder(NewFolder);
            }
            catch
            {
            }

        }
        public void AddFolder(folder Newfolder)
        {
            try
            {
                AllFolders.Add(Newfolder);
                NotifyPropertyChanged("AllFolders");
                dbh.InsertFolder(Newfolder);
            }
            catch { }
        }
        public async Task RemoveFolder(folder FolderToBeRemoved)
        {
            try
            {
                var found = AllFolders.FirstOrDefault(x => x.folder_id == FolderToBeRemoved.folder_id);
                AllFolders.Remove(found);
                NotifyPropertyChanged("AllFolders");
                dbh.DeleteFolder(FolderToBeRemoved);

                ObservableCollection<task> FolderTasks = dbh.GetTasksByFolder(FolderToBeRemoved);
                foreach (task t in FolderTasks)
                {
                    dbh.DeleteTask(t.task_id);
                    AllTasks.Remove(t);
                    NotifyPropertyChanged("AllTasks");
                }
                initializeLists(false);
                SetCurrentFolder(null);

            }
            catch { }
        }
        public void DeleteAllFolders()
        {
            try
            {                
                AllFolders = new ObservableCollection<folder>();
                dbh.DeleteAllFolders();
                AllFolders = dbh.GetAllFolders();
                NotifyPropertyChanged("AllFolders");
            }
            catch { }
        }
        #endregion
    }
}
