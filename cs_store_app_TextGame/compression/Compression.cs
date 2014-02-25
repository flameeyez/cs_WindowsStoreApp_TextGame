using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Compression;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using System.IO;

namespace cs_store_app_TextGame
{
    public static class Compression
    {
        public static void Compress(string strFileName, string strFolderName = "xml")
        {

        }
        public static async Task Decompress(string strFileName, string strFolderName = "xml")
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(strFolderName);
                var file = await folder.GetFileAsync(strFileName);
                var stream = await file.OpenStreamForReadAsync();

                var decompressedFilename = strFileName + ".decompressed";
                var decompressedFile = await folder.CreateFileAsync(decompressedFilename, CreationCollisionOption.ReplaceExisting);

                using (var compressedInput = await file.OpenSequentialReadAsync())
                using (var decompressor = new Decompressor(compressedInput))
                using (var decompressedOutput = await decompressedFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var bytesDecompressed = await RandomAccessStream.CopyAsync(decompressor, decompressedOutput);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
