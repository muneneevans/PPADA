using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;

namespace toodo_wp.Models
{
    public class DBHelper
    {
        static string DBPath = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "toodo.sqlite"));//DataBase Name
        SQLiteConnection dbconn = new SQLiteConnection(DBPath);

        public DBHelper()
        {
            CheckTable();
        }

        public void CheckTable()
        {
            using (dbconn = new SQLiteConnection(DBPath))
            {
                dbconn.CreateTable<task>();
                dbconn.CreateTable<routine>();
                dbconn.CreateTable<folder>();
                DefaultFolder();

            }

        }

        public async Task<bool> OnCreate(string DBPath)
        {
            try
            {
                if (!CheckFileExists(DBPath).Result)
                {
                    using (dbconn = new SQLiteConnection(DBPath))
                    {
                        CreateTaskTable();
                        CreateRoutineTable();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> CheckFileExists(string FileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(FileName);
                return true;
            }
            catch
            {
                return false;
            }
        }


        #region task table managers
        public void InsertTask(task t)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                dbconn.RunInTransaction(() =>
                {
                    dbconn.Insert(t);
                });
            }
        }

        public void UpdateTask(task t)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var existingtask = dbconn.Query<task>("select * from task where task_id =" + t.task_id).FirstOrDefault();
                if (existingtask != null)
                {
                    existingtask = t;
                    dbconn.RunInTransaction(() =>
                    {
                        dbconn.Update(existingtask);
                    });
                }
            }
        }

        public task GetTask(int TaskID)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var existingtask = dbconn.Query<task>("select * from task where task_id =" + TaskID).FirstOrDefault();
                return existingtask;
            }
        }

        public ObservableCollection<task> GetAllTasks()
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                List<task> mycollection = dbconn.Table<task>().ToList<task>();
                return new ObservableCollection<task>(mycollection);
            }
        }

        public ObservableCollection<task> GetTasksByStatus(bool status)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                //List<task> mycollection = dbconn.Query<task>("select * from task where task_status =" + status);
                var mycollection = dbconn.Query<task>("select * from task").Where(t => t.task_status == status).ToList();
                return new ObservableCollection<task>(mycollection.ToList());
            }
        }

        public ObservableCollection<task> GetTasksByFolder(folder f)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                //List<task> mycollection = dbconn.Query<task>("select * from task where task_status =" + status);
                var mycollection = dbconn.Query<task>("select * from task").Where(t => t.folder_id == f.folder_id).ToList();
                return new ObservableCollection<task>(mycollection.ToList());
            }
        }
        public ObservableCollection<task> GetPendingTasksByFolder(folder f)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                //List<task> mycollection = dbconn.Query<task>("select * from task where task_status =" + status);
                var mycollection = dbconn.Query<task>("select * from task").Where((t => t.folder_id == f.folder_id && t.task_status == false)).ToList();
                return new ObservableCollection<task>(mycollection.ToList());
            }
        }

        public void DeleteTask(int TaskId)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var existingtask = dbconn.Query<task>("select * from task where task_id =" + TaskId).FirstOrDefault();
                if (existingtask != null)
                {
                    dbconn.RunInTransaction(() =>
                    {
                        dbconn.Delete(existingtask);
                    });
                }
            }
        }

        public void DeleteAllTasks()
        {
            try
            {
                using (dbconn = new SQLiteConnection(DBPath))
                {
                    dbconn.DropTable<task>();
                    dbconn.CreateTable<task>();
                }
            }
            catch { }
        }

        public bool CreateTaskTable()
        {
            try
            {
                using (dbconn = new SQLiteConnection(DBPath))
                {
                    dbconn.CreateTable<task>();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region routine table managers
        public void InsertRoutine(routine r)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                dbconn.RunInTransaction(() =>
                {
                    dbconn.Insert(r);
                });
            }
        }

        public void UpdateRoutine(routine r)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var existingroutine = dbconn.Query<routine>("select * from routine where routine_id =" + r.routine_id).FirstOrDefault();
                if (existingroutine != null)
                {
                    existingroutine = r;
                    dbconn.RunInTransaction(() =>
                    {
                        dbconn.Update(existingroutine);
                    });
                }
            }
        }

        public routine GetRoutine(int RoutineID)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var existingroutine = dbconn.Query<routine>("select * from routine where routine_id =" + RoutineID).FirstOrDefault();
                return existingroutine;
            }
        }

        public ObservableCollection<routine> GetAllRoutines()
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                List<routine> mycollection = dbconn.Table<routine>().ToList<routine>();
                return new ObservableCollection<routine>(mycollection);
            }
        }

        public ObservableCollection<routine> GetRoutinesByStatus(bool status)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                List<routine> mycollection = dbconn.Query<routine>("select * from routine").Where(r => r.routine_status == status).ToList();

                return new ObservableCollection<routine>(mycollection);
            }
        }
        public ObservableCollection<routine> GetPendingRoutinesByStatus(bool status)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                List<routine> mycollection = dbconn.Query<routine>("select * from routine").Where(r => ((r.routine_status == status) || (r.routine_deadline <= DateTime.Now)) || ((r.routine_status == true) && (r.routine_deadline <= DateTime.Now))).ToList();
                return new ObservableCollection<routine>(mycollection);
            }
        }

        public void DeleteRoutine(int RoutineId)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var existingroutine = dbconn.Query<routine>("select * from routine where routine_id =" + RoutineId).FirstOrDefault();
                if (existingroutine != null)
                {
                    dbconn.RunInTransaction(() =>
                    {
                        dbconn.Delete(existingroutine);
                    });
                }
            }
        }

        public void DeleteAllRoutines()
        {
            using (var dbConn = new SQLiteConnection(DBPath))
            {
                dbConn.DropTable<routine>();
                dbConn.CreateTable<routine>();
            }
        }

        public bool CreateRoutineTable()
        {
            try
            {
                using (dbconn = new SQLiteConnection(DBPath))
                {
                    dbconn.CreateTable<routine>();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region folder managers
        public void InsertFolder(folder f)
        {
            try
            {
                using (var dbconn = new SQLiteConnection(DBPath))
                {
                    dbconn.RunInTransaction(() =>
                    {
                        dbconn.Insert(f);
                    });
                }
            }
            catch { }
        }

        public void UpdateFolder(folder f)
        {
            try
            {
                using (var dbconn = new SQLiteConnection(DBPath))
                {
                    var existingfolder = dbconn.Query<folder>("select * from folder where folder_id =" + f.folder_id).FirstOrDefault();
                    if (existingfolder != null)
                    {
                        existingfolder = f;
                        dbconn.RunInTransaction(() =>
                        {
                            dbconn.Update(existingfolder);
                        });
                    }
                }
            }
            catch { }
        }

        public void DefaultFolder()
        {
            folder f = new folder();
            f.folder_id = 1;
            f.folder_name = "regular";
            f.folder_color = 0xFFFFA500;
            try
            {
                using (var dbconn = new SQLiteConnection(DBPath))
                {
                    var existingfolder = dbconn.Query<folder>("select * from folder where folder_id =" + f.folder_id).FirstOrDefault();
                    if (existingfolder == null)
                    {
                        InsertFolder(f);
                    }
                }
            }
            catch
            {
                InsertFolder(f);
            }
        }

        public folder GetFolder(int FolderId)
        {
            try
            {
                using (var dbconn = new SQLiteConnection(DBPath))
                {
                    folder existingfolder = dbconn.Query<folder>("select * from folder where folder_id = " + FolderId).FirstOrDefault();
                    return existingfolder;
                }
            }
            catch
            {
                return null;
            }
        }

        public ObservableCollection<folder> GetAllFolders()
        {
            try
            {
                using (var dbconn = new SQLiteConnection(DBPath))
                {
                    List<folder> FolderCollection = dbconn.Query<folder>("Select * from folder").ToList();
                    return new ObservableCollection<folder>(FolderCollection);
                }
            }
            catch { return null; }
        }

        public void DeleteFolder(folder f)
        {
            try
            {
                using (var dbconn = new SQLiteConnection(DBPath))
                {
                    var existingfolder = dbconn.Query<folder>("select * from folder where folder_id =" + f.folder_id).FirstOrDefault();
                    if (existingfolder != null)
                    {
                        dbconn.RunInTransaction(() =>
                        {
                            dbconn.Delete(existingfolder);
                        });
                    }
                }
            }
            catch { }
        }

        public void DeleteAllFolders()
        {
            using (var dbConn = new SQLiteConnection(DBPath))
            {
                dbConn.DropTable<folder>();
                dbConn.CreateTable<folder>();
                DefaultFolder();
            }
        }

        public bool CreateFolderTable()
        {
            try
            {
                using (dbconn = new SQLiteConnection(DBPath))
                {
                    dbconn.CreateTable<folder>();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        private Color ConvertColor(uint uintCol)
        {
            byte A = (byte)((uintCol & 0xFF000000) >> 24);
            byte R = (byte)((uintCol & 0x00FF0000) >> 16);
            byte G = (byte)((uintCol & 0x0000FF00) >> 8);
            byte B = (byte)((uintCol & 0x000000FF) >> 0);

            return Color.FromArgb(A, R, G, B); ;
        }
    }
}
