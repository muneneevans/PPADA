﻿using SQLite;
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
                List<Note> Notes = dbconn.Query<Note>("SELECT *  FROM note");
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
                var existingNote = dbconn.Query<Note>("SELECT * from note where id = " + updatingNote.id).FirstOrDefault();
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
                var foundNote = dbconn.Query<Note>("SELECT * FROM note WHERE id =" + id).FirstOrDefault();
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

        public ObservableCollection<Note> GetAllBookmarks()
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var foundNotes = new ObservableCollection<Note>(dbconn.Table<Note>().ToList<Note>().Where(x => x.bookmarked == true));
                return foundNotes;
            }
        }

        public Note GetTopicNote(int topicId)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var foundNote = dbconn.Query<Note>("select * from note where topicID =" + topicId).FirstOrDefault();
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

        public ObservableCollection<Topic> GetSectionTopics(int sectionId)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var foundNote = new ObservableCollection<Topic>(dbconn.Query<Topic>("select * from topic where sectionId =" + sectionId));
                return foundNote;
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

        public Annotation GetAnnotation(int id)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var foundNote = dbconn.Query<Annotation>("SELECT * FROM Annotation WHERE id =" + id).FirstOrDefault();
                return foundNote;
            }
        }

        public void DeleteAnnotation(int id)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var foundAnnotation = dbconn.Query<Annotation>("SELECT * FROM Annotation WHERE id =" + id).FirstOrDefault();
                if (foundAnnotation != null)
                {
                    dbconn.Delete(foundAnnotation);
                }
            }
        }
        #endregion


        #region Section handlers
        public void createSection(Section newSection)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                dbconn.Insert(newSection);
            }
        }

        public ObservableCollection<Section> GetAllSections()
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                return new ObservableCollection<Section>(dbconn.Table<Section>().ToList<Section>());
            }
        }

        public Section GetSection(int id)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var foundNote = dbconn.Query<Section>("SELECT * FROM Section WHERE id =" + id).FirstOrDefault();
                return foundNote;
            }
        }

        public void DeleteSection(int id)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var foundSection = dbconn.Query<Section>("SELECT * FROM Section WHERE id =" + id).FirstOrDefault();
                if (foundSection != null)
                {
                    dbconn.Delete(foundSection);
                }
            }
        }
        #endregion

        #region News handlers
        public void createNews(News newNews)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                dbconn.Insert(newNews);
            }
        }

        public ObservableCollection<News> GetAllNews()
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                return new ObservableCollection<News>(dbconn.Table<News>().ToList<News>());
            }
        }

        public News GetNews(int id)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var foundNews = dbconn.Query<News>("SELECT * FROM News WHERE id =" + id).FirstOrDefault();
                return foundNews;
            }
        }

        public void DeleteNews(int id)
        {
            using (var dbconn = new SQLiteConnection(DBPath))
            {
                var foundNews = dbconn.Query<News>("SELECT * FROM News WHERE id =" + id).FirstOrDefault();
                if (foundNews != null)
                {
                    dbconn.Delete(foundNews);
                }
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
