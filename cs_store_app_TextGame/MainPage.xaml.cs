using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Compression;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;

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
        // TODO: create encapsulating class for these variables?
        string strLastInput = "";
        Timer t;
        Queue<string> InputQueue = new Queue<string>();

        #region Page Handlers
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ItemTemplates.Load();
            await World.Load();
            //await World.LoadCompressed();
            await EntityNPCTemplates.Load();

            // world update timer
            t = new Timer(Update, null, 1000, 50);

            // DEBUG
            AddDebug();
            // END DEBUG

            // initialize player
            Game.Initialize();
            TranslatedInput input = null;
            AppendText(Game.Player.DoLook(input).StringToAppend);

            txtInput.Focus(FocusState.Programmatic);
        }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            string input = txtInput.Text.Trim();
            ClearInput();
            if (input.Length > 0) { InputQueue.Enqueue(input); }
        }
        private void btnDirection_Click(object sender, RoutedEventArgs e)
        {
            // button stores text info in Tag (nw, n, ne, etc.)
            ClearInput();
            InputQueue.Enqueue(((Button)sender).Tag.ToString());
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
            Handler handler = Game.Player.ProcessInput(input);
            // TODO: HANDLED vs UNHANDLED?
            AppendText(handler.StringToAppend);
        }
        #endregion
        // TODO: figure out if state should be used
        public async void Update(object state)
        {
            DateTime start = DateTime.Now;

            // interaction with UI thread
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                // handle input queue
                while (InputQueue.Count > 0)
                {
                    string input = InputQueue.Dequeue();
                    HandleInput(input);
                }

                // update world
                List<Handler> handlers = World.Update();
                foreach (Handler handler in handlers)
                {
                    AppendText(handler.StringToAppend);
                }
            });

            DateTime end = DateTime.Now;
            TimeSpan delta = end - start;

            //await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //{
            //    AppendText(delta.TotalMilliseconds.ToString());
            //});
        }
        #region Debug
        public void AddDebug()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int nItemPasses = 10;
            int nNPCCount = 50;

            // DEBUG ITEMS
            for (int i = 0; i < nItemPasses; i++)
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

            // DEBUG NPCS
            for (int i = 0; i < nNPCCount; i++)
            {
                int region = 0;
                int subregion = 0;
                int room = r.Next(9);
                EntityNPC npc = EntityNPCTemplates.NPCs[r.Next(EntityNPCTemplates.NPCs.Count)].Clone();
                npc.SetCurrentRoom(region, subregion, room);
                World.Regions[region].Subregions[subregion].Rooms[room].AddNPC(npc);
            }
        }
        #endregion
    }
}