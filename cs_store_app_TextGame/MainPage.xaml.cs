using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Compression;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace cs_store_app_TextGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // TODO: SourceInfo class, two members (ITEM_SOURCE, Item)
        // TODO: public SourceInfo FindItem(string strKeyword)
        // - switch(itemSource):
        // - {
        // -    case ITEM_SOURCE.PLAYER_BACKPACK:
        // -       break;
        // - }
        // TODO: handle full item names (drop, get, put, look in/at, equip, etc.)
        // TODO: real-time action (loop ~4 times per second, append debug text on each tick)
        // TODO: find item with merged input
        // - MergeInput(int nStartingIndex)
        // - FindItem(string, bool bMultiWord = false)
        //   - if true, match string to full item name

        Entity player;
        string strLastInput = "";
        Timer t;
        Queue<string> InputQueue = new Queue<string>();

        #region UI Handling
        public MainPage()
        {
            this.InitializeComponent();
            t = new Timer(Update, null, 1000, 50);
        }

        public async void Update(object state)
        {
            DateTime start = DateTime.Now;

            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                while(InputQueue.Count > 0)
                {
                    string input = InputQueue.Dequeue();
                    HandleInput(input);
                }

                List<Handler> handlers = World.Update();
                foreach(Handler handler in handlers)
                {
                    AppendText(handler.StringToAppend, false);
                }

                if (handlers.Count > 0) { AppendText("\n"); }
            });

            DateTime end = DateTime.Now;
            TimeSpan delta = end - start;

            //await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //{
            //    AppendText(delta.TotalMilliseconds.ToString());
            //});
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //DateTime start = DateTime.Now;
            //AppendText("Hello");
            //DateTime end = DateTime.Now;
            //AppendText(((TimeSpan)(end - start)).TotalMilliseconds.ToString());

            string input = txtInput.Text.Trim();
            ClearInput();
            if (input.Length > 0) { InputQueue.Enqueue(input); }

            //CompressionTest();
        }

        private async void CompressionTest()
        {
            AppendText("Working...", false);

            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml");
            var originalFile = await folder.GetFileAsync("world.xml");
            var stream = await originalFile.OpenStreamForReadAsync();

            AppendText("File opened...", false);


            // lzms
            var compressedFilename = originalFile.Name + ".compressed";
            var compressedFile = await folder.CreateFileAsync(compressedFilename, CreationCollisionOption.GenerateUniqueName);

            AppendText("world.xml.compressed created...", false);

            // ** DO COMPRESSION **
            // Following code actually performs compression from original file to the newly created
            // compressed file. In order to do so it:
            // 1. Opens input for the original file.
            // 2. Opens output stream on the file to be compressed and wraps it into Compressor object.
            // 3. Copies original stream into Compressor wrapper.
            // 4. Finalizes compressor - it puts termination mark into stream and flushes all intermediate
            //    buffers.
            using (var originalInput = await originalFile.OpenReadAsync())
            using (var compressedOutput = await compressedFile.OpenAsync(FileAccessMode.ReadWrite))
            using (var compressor = new Compressor(compressedOutput.GetOutputStreamAt(0), CompressAlgorithm.XpressHuff, 0))
            {
                AppendText("Inside compression block...", false);
                var bytesCompressed = await RandomAccessStream.CopyAsync(originalInput, compressor);
                var finished = await compressor.FinishAsync();
                AppendText(String.Format("Compressed {0} bytes into {1}\n", bytesCompressed, compressedOutput.Size));
            }

            var decompressedFilename = originalFile.Name + ".decompressed";
            var decompressedFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(decompressedFilename, CreationCollisionOption.GenerateUniqueName);

            // ** DO DECOMPRESSION **
            // Following code performs decompression from the just compressed file to the
            // decompressed file. In order to do so it:
            // 1. Opens input stream on compressed file and wraps it into Decompressor object.
            // 2. Opens output stream from the file that will store decompressed data.
            // 3. Copies data from Decompressor stream into decompressed file stream.
            using (var compressedInput = await compressedFile.OpenSequentialReadAsync())
            using (var decompressor = new Decompressor(compressedInput))
            using (var decompressedOutput = await decompressedFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                AppendText("Decompressing...", false);
                var bytesDecompressed = await RandomAccessStream.CopyAsync(decompressor, decompressedOutput);
                AppendText(String.Format("Decompressed {0} bytes of data\n", bytesDecompressed));
            }
        }
        private void txtInput_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch(e.Key)
            {
                case Windows.System.VirtualKey.Enter:
                    string input = txtInput.Text.Trim();
                    ClearInput();
                    if (input.Length > 0) { InputQueue.Enqueue(input); }
                    break;
                case Windows.System.VirtualKey.Up:
                    txtInput.Text = strLastInput;
                    txtInput.Select(txtInput.Text.Length, 0);
                    break;
                case Windows.System.VirtualKey.Down:
                    txtInput.Text = "";
                    break;
                default:
                    break;
            }
        }
        private void AppendText(string str, bool bScroll = true)
        {
            if (str.Length > 0)
            {

                Paragraph p = new Paragraph();
                Run r = new Run();
                r.Text = str;
                p.Inlines.Add(r);
                this.txtOutput.Blocks.Add(p);

                CheckParagraphCount();

                if (bScroll) { ScrollToBottom(); }
            }
        }
        private void ClearInput(bool bFocus = true)
        {
            this.txtInput.Text = "";
            if (bFocus)
            {
                this.txtInput.Focus(FocusState.Programmatic);
            }
        }
        private void CheckParagraphCount()
        {
            // TODO: modify to work with total text length
            if (txtOutput.Blocks.Count > 500)
            {
                while (txtOutput.Blocks.Count > 100)
                {
                    txtOutput.Blocks.RemoveAt(0);
                }
            }

            //txtParagraphCount.Text = txtOutput.Blocks.Count.ToString();
        }
        private void ScrollToBottom()
        {
            if (txtOutputScroll.ExtentHeight > 0)
            {
                txtOutputScroll.ChangeView(null, txtOutputScroll.ExtentHeight, null);
            }
        }
        private void btnDirection_Click(object sender, RoutedEventArgs e)
        {
            // button stores text info in Tag (nw, n, ne, etc.)
            ClearInput();
            InputQueue.Enqueue(((Button)sender).Tag.ToString());
        }
        #endregion

        #region Input Handling
        public void HandleInput(string input)
        {
            strLastInput = input;
            
            // DateTime begin = DateTime.Now;
            AppendText("> " + input + "\n", false); 
            ProcessInput(new TranslatedInput(input));
            // DateTime end = DateTime.Now;
            // TimeSpan delta = end - begin;
            // AppendText(delta.TotalMilliseconds.ToString(), false);
        }
        public void ProcessInput(TranslatedInput input)
        {
            if (input.Words.Length == 0) { return; }
            if (input.Action == ACTION_ENUM.NONE) { AppendText(Handler.BAD_INPUT.StringToAppend); }
            Handler handler = player.ProcessInput(input);
            // TODO: HANDLED vs UNHANDLED?
            AppendText(handler.StringToAppend);
        }
        #endregion

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            player = new Player();

            await ItemTemplates.Load();
            await World.Load();
            await NPCTemplates.Load();

            //await World.LoadCompressed();

            // DEBUG ITEMS
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 2; i++)
            {
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsDrink[0].DeepClone());
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsFood[0].DeepClone());
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsJunk[0].DeepClone());
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsWeapon[0].DeepClone());
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsContainer[0].DeepClone());
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsContainer[1].DeepClone());
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsArmorChest[0].DeepClone());
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsArmorFeet[0].DeepClone());
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsArmorHead[0].DeepClone());
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsAccessoryAmulet[0].DeepClone());
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsAccessoryRing[0].DeepClone());
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsAccessoryRing[1].DeepClone());
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsArmorShield[0].DeepClone());
            }

            for (int i = 0; i < 1; i++)
            {
                int region = 0;
                int subregion = 0;
                int room = r.Next(9);
                NPC npc = NPCTemplates.NPCs[r.Next(2)].Clone();
                npc.SetCurrentRoom(region, subregion, room);
                World.Regions[region].Subregions[subregion].Rooms[room].AddNPC(npc);
            }
                // END DEBUG ITEMS

                // debug size testing
                //long before, after, difference;

                //for (int i = 0; i < 10; i++)
                //{
                //    before = GC.GetTotalMemory(true);
                //    World.Regions[0].Subregions[0].Rooms[0].AddItem(ItemTemplates.ItemsContainer[0].DeepClone());
                //    after = GC.GetTotalMemory(true);
                //    difference = after - before;

                //    AppendText(difference.ToString(), false);
                //}

                player.SetCurrentRoom(0, 0, 0);
            TranslatedInput input = null;
            AppendText(player.DoLook(input).StringToAppend);

            txtInput.Focus(FocusState.Programmatic);
        }
    }
}