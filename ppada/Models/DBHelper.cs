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

namespace ppada.Models
{
    public class DBHelper
    {
        static string DBPath = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "ppada.db"));//DataBase Name
        SQLiteConnection dbconn = new SQLiteConnection(DBPath);

        public DBHelper()
        {
            CheckTable();
        }

        public void CheckTable()
        {
            using (dbconn = new SQLiteConnection(DBPath))
            {
                List<Note> Notes = dbconn.Query<Note>("SELECT *  FROM notes");
                //dbconn.CreateTable<task>();
                //dbconn.CreateTable<routine>();
                //dbconn.CreateTable<folder>();
                //DefaultFolder();

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


        #region Note handlers
        public void createNote(Note newNote)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                dbconn.Insert(newNote);
            }
        }

        public void updateNote(Note updatingNote)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var existingNote = dbconn.Query<Note>("SELECT * from notes where id = " + updatingNote.id).FirstOrDefault();
                if (existingNote != null)
                {
                    existingNote = updatingNote;
                    dbconn.Update(existingNote);
                }
            }
        }

        public Note GetNote(int id)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var foundNote = dbconn.Query<Note>("SELECT * FROM notes WHERE id =" + id).FirstOrDefault();
                return foundNote;
            }
        }

        public ObservableCollection<Note> GetAllNotes()
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                return new ObservableCollection<Note>(dbconn.Table<Note>().ToList<Note>());
            }
        }

        public Note GetTopicNote(int topicId)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var foundNote = dbconn.Query<Note>("select * from notes where topicID =" + topicId).FirstOrDefault();
                return foundNote;                
            }
        }
        #endregion

        #region Topic handlers
        public ObservableCollection<Topic> GetAllTopics()
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                return new ObservableCollection<Topic>(dbconn.Table<Topic>().ToList<Topic>());
            }
        }
        #endregion

        #region Annotation handlers
        public void createAnnotation(Annotation newAnnotation)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                dbconn.Insert(newAnnotation);
            }
        }

        public ObservableCollection<Annotation> GetAllAnnotations()
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                return new ObservableCollection<Annotation>(dbconn.Table<Annotation>().ToList<Annotation>());
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

        #region page managers

        #endregion
    }
}
