using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using toodo_wp.Models;

namespace toodo_wp.Tiles
{
    public class TileManager
    {
        public void DefaultTile(List<task> AllTasks )
        {
            var TileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150BlockAndText01);
            var tileText = TileXml.GetElementsByTagName("text");
            int count = 0;
            foreach (task t in AllTasks)
            {
                (tileText[count] as XmlElement).InnerText = t.task_name;
                count++;
            }
            var TileNotification = new TileNotification(TileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(TileNotification);
        }
    }
}
