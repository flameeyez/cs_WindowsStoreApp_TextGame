using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage.Compression;
using Windows.Storage.Streams;
using Windows.Storage;

namespace cs_store_app_TextGame
{
    public static class World
    {
        public static List<Region> Regions = new List<Region>();
        public static string DisplayString
        {
            get
            {
                string strReturn = "";

                foreach (Region region in Regions)
                {
                    strReturn += "Region " + region.ID.ToString() + ": " + region.Name + "\n";
                    foreach(Subregion subregion in region.Subregions)
                    {
                        strReturn += "Subregion " + subregion.ID.ToString() + ": " + subregion.Name + "\n";
                        foreach (Room room in subregion.Rooms)
                        {
                            strReturn += room.FullDisplayString + "\n";
                        }
                    }
                }

                return strReturn;
            }
        }

        public static async Task Load()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\world");
                var file = await folder.GetFileAsync("world_save.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument worldDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var regionNodes = from regions in worldDocument
                                      .Elements("world")
                                        .Elements("regions")
                                          .Elements("region")
                                  select regions;
                foreach (var regionNode in regionNodes)
                {
                    Regions.Add(new Region(regionNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static async Task LoadCompressed()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml");
                var file = await folder.GetFileAsync("world.compressed");
                var stream = await file.OpenStreamForReadAsync();

                var decompressedFilename = "world.decompressed";
                var decompressedFile = await folder.CreateFileAsync(decompressedFilename, CreationCollisionOption.ReplaceExisting);

                using (var compressedInput = await file.OpenSequentialReadAsync())
                using (var decompressor = new Decompressor(compressedInput))
                using (var decompressedOutput = await decompressedFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var bytesDecompressed = await RandomAccessStream.CopyAsync(decompressor, decompressedOutput);
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            await Load();
        }

        public static List<Handler> Update()
        {
            List<Handler> handlers = new List<Handler>();

            foreach(Region region in Regions)
            {
                List<Handler> regionHandlers = region.Update();
                if (regionHandlers.Count > 0) { handlers.AddRange(regionHandlers); }
            }

            return handlers;
        }
    }
}
