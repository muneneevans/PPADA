using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using toodo_wp.Models;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace toodo_wp.tile
{
    public static class TileManager
    {
        public static async Task DefaultTile(List<task> AllTasks)
        {
            if (AllTasks.Count == 0)
            {
                return;
            }
            var WideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text01);
            var SquareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text03);
            

            var WideTileText = WideTileXml.GetElementsByTagName("text");
            var SquareTileText = SquareTileXml.GetElementsByTagName("text");

            

            (WideTileText[0] as XmlElement).InnerText = "peding tasks";
            (SquareTileText[0] as XmlElement).InnerText = "peding tasks";

            
            
            int count = 1;
            foreach (task t in AllTasks)
            {
                if (!(count >= 4))
                {
                    (SquareTileText[count] as XmlElement).InnerText = t.task_name;
                }
                if ((count >= 5))
                {
                    continue;
                }
                (WideTileText[count] as XmlElement).InnerText = t.task_name;
                
                count++;
            }
            var WideTileNotification = new TileNotification(WideTileXml);
            var SquareTileNotification = new TileNotification(SquareTileXml);


            TileUpdateManager.CreateTileUpdaterForApplication().Update(WideTileNotification);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(SquareTileNotification);    

            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueueForSquare150x150(true);
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueueForWide310x150(true);
            
        }
        public static async Task DefaultTile(List<task> AllTasks, string FolderName)
        {
            if (AllTasks.Count == 0)
            {
                return;
            }
            var WideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text01);
            var SquareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text03);


            var WideTileText = WideTileXml.GetElementsByTagName("text");
            var SquareTileText = SquareTileXml.GetElementsByTagName("text");


            (WideTileText[0] as XmlElement).InnerText = FolderName + " : ";
            (SquareTileText[0] as XmlElement).InnerText = FolderName + " : ";



            int count = 1;
            foreach (task t in AllTasks)
            {
                if (!(count >= 4))
                {
                    (SquareTileText[count] as XmlElement).InnerText = t.task_name;
                }
                if ((count >= 5))
                {
                    continue;
                }
                (WideTileText[count] as XmlElement).InnerText = t.task_name;

                count++;
            }
            var WideTileNotification = new TileNotification(WideTileXml);
            WideTileNotification.Tag = FolderName + "Wide";
            
            var SquareTileNotification = new TileNotification(SquareTileXml);
            SquareTileNotification.Tag = FolderName + "Square";


            var x = TileUpdateManager.CreateTileUpdaterForApplication().GetScheduledTileNotifications();
            foreach (var x1 in x)
            {
                Debug.WriteLine(x1.Tag);
            }
            TileUpdateManager.CreateTileUpdaterForApplication().Update(WideTileNotification);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(SquareTileNotification);

            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueueForSquare150x150(true);
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueueForWide310x150(true);

        }
    }
}
