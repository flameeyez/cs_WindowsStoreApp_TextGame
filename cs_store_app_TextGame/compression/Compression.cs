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

//private async void CompressionTest()
//{
//    AppendText("Working...", false);

//    var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml");
//    var originalFile = await folder.GetFileAsync("world.xml");
//    var stream = await originalFile.OpenStreamForReadAsync();

//    AppendText("File opened...", false);


//    // lzms
//    var compressedFilename = originalFile.Name + ".compressed";
//    var compressedFile = await folder.CreateFileAsync(compressedFilename, CreationCollisionOption.GenerateUniqueName);

//    AppendText("world.xml.compressed created...", false);

//    // ** DO COMPRESSION **
//    // Following code actually performs compression from original file to the newly created
//    // compressed file. In order to do so it:
//    // 1. Opens input for the original file.
//    // 2. Opens output stream on the file to be compressed and wraps it into Compressor object.
//    // 3. Copies original stream into Compressor wrapper.
//    // 4. Finalizes compressor - it puts termination mark into stream and flushes all intermediate
//    //    buffers.
//    using (var originalInput = await originalFile.OpenReadAsync())
//    using (var compressedOutput = await compressedFile.OpenAsync(FileAccessMode.ReadWrite))
//    using (var compressor = new Compressor(compressedOutput.GetOutputStreamAt(0), CompressAlgorithm.XpressHuff, 0))
//    {
//        AppendText("Inside compression block...", false);
//        var bytesCompressed = await RandomAccessStream.CopyAsync(originalInput, compressor);
//        var finished = await compressor.FinishAsync();
//        AppendText(String.Format("Compressed {0} bytes into {1}\n", bytesCompressed, compressedOutput.Size));
//    }

//    var decompressedFilename = originalFile.Name + ".decompressed";
//    var decompressedFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(decompressedFilename, CreationCollisionOption.GenerateUniqueName);

//    // ** DO DECOMPRESSION **
//    // Following code performs decompression from the just compressed file to the
//    // decompressed file. In order to do so it:
//    // 1. Opens input stream on compressed file and wraps it into Decompressor object.
//    // 2. Opens output stream from the file that will store decompressed data.
//    // 3. Copies data from Decompressor stream into decompressed file stream.
//    using (var compressedInput = await compressedFile.OpenSequentialReadAsync())
//    using (var decompressor = new Decompressor(compressedInput))
//    using (var decompressedOutput = await decompressedFile.OpenAsync(FileAccessMode.ReadWrite))
//    {
//        AppendText("Decompressing...", false);
//        var bytesDecompressed = await RandomAccessStream.CopyAsync(decompressor, decompressedOutput);
//        AppendText(String.Format("Decompressed {0} bytes of data\n", bytesDecompressed));
//    }
//}