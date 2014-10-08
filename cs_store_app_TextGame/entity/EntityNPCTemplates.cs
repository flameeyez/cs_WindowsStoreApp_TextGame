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
        public static List<EntityNPCBase> NPCTemplates = new List<EntityNPCBase>();

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

                var npcElements = from npcTemplates in npcsDocument
                                      .Elements("npc-templates")
                                        .Elements("npc-template")
                               select npcTemplates;
                foreach (var npcElement in npcElements)
                {
                    NPCTemplates.Add(new EntityNPCBase(npcElement));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
