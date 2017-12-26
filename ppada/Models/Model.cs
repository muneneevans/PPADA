using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ppada.Models
{            
    public static class Model
    {
        //class dealing with reading and writing to a file
        public static async Task<string> ReadFromTasksFileAsync()
        {
            try
            {
                StorageFile ReadingFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///TaskModels/Tasksfile.txt"));
                string data = await FileIO.ReadTextAsync(ReadingFile);
                Debug.WriteLine(data);
                return data;
            }
            catch
            {
                return null;
            }
        }       
        public static async Task WriteToTasksFileAsync(string Content)
        {
            StorageFile WritingFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///TaskModels/Tasksfile.txt"));
            await FileIO.WriteTextAsync(WritingFile, Content);
            Debug.WriteLine("Successfully written to file");
        }

        public static async Task<string> ReadFromRoutinesFileAsync()
        {
            try
            {
                StorageFile ReadingFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///TaskModels/Routinesfile.txt"));
                string data = await FileIO.ReadTextAsync(ReadingFile);
                Debug.WriteLine(data);
                return data;
            }
            catch
            {
                return null;
            }
        }
        public static async Task WriteToRoutinesFileAsync(string Content)
        {
            StorageFile WritingFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///TaskModels/RoutinesFile.txt"));
            await FileIO.WriteTextAsync(WritingFile, Content);
            Debug.WriteLine("Successfully written to file");
        }
    }
}
