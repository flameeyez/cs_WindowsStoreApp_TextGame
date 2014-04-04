using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace cs_store_app_TextGame
{
    public static class EntityNPCTemplates
    {
        public static List<EntityNPCTemplate> NPCTemplates = new List<EntityNPCTemplate>();

        public async static Task Load()
        {
            await LoadNPCs();
        }
        private async static Task LoadNPCs()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\npc");
                var file = await folder.GetFileAsync("npc-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument npcsDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var npcNodes = from npcTemplates in npcsDocument
                                      .Elements("npc-templates")
                                        .Elements("npc-template")
                               select npcTemplates;
                foreach (var npcNode in npcNodes)
                {
                    NPCTemplates.Add(new EntityNPCTemplate(npcNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
