using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Compression;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;

namespace cs_store_app_TextGame {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        Timer WorldUpdateTimer;
        Timer CheckAppendQueueTimer;

        #region Initialization
        public MainPage() {
            this.InitializeComponent();
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            await ItemTemplates.Load();
            await World.Load();
            await EntityNPCTemplates.Load();
            await Messages.Load();
            await EntityRelationshipTable.Load();

            // DEBUG
            // AppendParagraph(EntityRelationshipTable.DisplayString().ToParagraph());
            // END DEBUG

            // world update timer
            WorldUpdateTimer = new Timer(Update, null, 1000, 50);
            CheckAppendQueueTimer = new Timer(CheckAppendQueue, null, 0, 100);

            // DEBUG
            AddDebug();
            // END DEBUG

            // initialize player
            Game.Initialize();
            ParsedInput input = null;
            Handler handler = Game.Player.DoLook(input);
            MessageQueue.Enqueue(handler.Message);
            // Messages.Display(handler.MessageCode, handler.ParagraphToAppend);

            //AppendParagraph(Game.Player.DoLook(input).ParagraphToAppend);
            txtInput.Focus(FocusState.Programmatic);
        }
        private async void CheckAppendQueue(object state) {
            while (MessageQueue.Count > 0) {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    lock (MessageQueue.Lock) {
                        AppendParagraph(MessageQueue.Dequeue());
                        CheckParagraphCount();
                        ScrollToBottom();
                    }
                });
            }
        }
        #endregion
        #region UI
        private void btnOK_Click(object sender, RoutedEventArgs e) {
            string input = txtInput.Text.Trim();
            ClearInput();
            if (input.Length > 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, input)); }
        }
        private void btnDirection_Click(object sender, RoutedEventArgs e) {
            // button stores text info in Tag (nw, n, ne, etc.)
            ClearInput();
            Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, ((Button)sender).Tag.ToString()));
        }
        private void txtInput_KeyUp(object sender, KeyRoutedEventArgs e) {
            switch (e.Key) {
                case Windows.System.VirtualKey.Enter:
                    string input = txtInput.Text.Trim();
                    ClearInput();
                    if (input.Length > 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, input)); }
                    break;
                case Windows.System.VirtualKey.Escape:
                    ClearInput();
                    break;
                case Windows.System.VirtualKey.Up:
                    txtInput.Text = Input.History.Previous();
                    txtInput.Select(txtInput.Text.Length, 0);
                    break;
                case Windows.System.VirtualKey.Down:
                    txtInput.Text = Input.History.Next();
                    txtInput.Select(txtInput.Text.Length, 0);
                    break;
                default:
                    break;
            }
        }
        private void AppendParagraph(Paragraph p) {
            if (p == null) { return; }

            // single point of compression
            // no need to compress anywhere else
            p.Compress();

            // DEBUG
            Statics.RunningInlineCount += p.Inlines.Count;
            // END DEBUG

            this.txtOutput.Blocks.Add(p);
        }
        private void AppendDebugText(string str) {
            if (str.Length > 0) {

                Paragraph p = new Paragraph();
                Run r = new Run();
                r.Text = str;
                p.Inlines.Add(r);
                this.txtDebug.Blocks.Add(p);

                // TODO: modify to work with total text length
                while (txtDebug.Blocks.Count > 50) {
                    txtDebug.Blocks.RemoveAt(0);
                }


                if (txtDebugOutputScroll.ExtentHeight > 0) {
                    txtDebugOutputScroll.ChangeView(null, txtDebugOutputScroll.ExtentHeight, null);
                }
            }
        }
        private void ClearInput(bool bFocus = true) {
            this.txtInput.Text = "";
            if (bFocus) {
                this.txtInput.Focus(FocusState.Programmatic);
            }
        }
        private void CheckParagraphCount() {
            // TODO: modify to work with total text length
            if (Statics.RunningInlineCount > Statics.RunningInlineThreshold) {
                // AppendDebugText("Cleaning up!");
                while (Statics.RunningInlineCount > Statics.RunningInlineCutCount) {
                    Paragraph p = txtOutput.Blocks[0] as Paragraph;
                    Statics.RunningInlineCount -= p.Inlines.Count;
                    txtOutput.Blocks.RemoveAt(0);
                }
            }

            txtDebugBlockCount.Text = Statics.RunningInlineCount.ToString();
        }
        private void ScrollToBottom() {
            if (txtOutputScroll.ExtentHeight > 0) {
                txtOutputScroll.ChangeView(null, txtOutputScroll.ExtentHeight, null, true);
            }
        }
        #endregion
        #region Input Handling
        public void HandleInput() {
            while (Input.Count > 0) {
                ParsedInput input = Input.Dequeue();
                AppendParagraph(("> " + input.String + "\n").ToParagraph());
                Handler handler = Game.ProcessInput(input);
                MessageQueue.Enqueue(handler.Message);
            }
        }
        #endregion
        #region Update
        // TODO: figure out if state should be used
        // TODO: move all logic to Game
        public async void Update(object state) {
            Stopwatch sUpdate = Stopwatch.StartNew();
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                HandleInput();
                World.Update();
                CheckParagraphCount();
                ScrollToBottom();
                sUpdate.Stop();

                #region Debug
                if (sUpdate.ElapsedMilliseconds > 15) { AppendDebugText("Update: " + sUpdate.ElapsedMilliseconds.ToString() + "ms"); }
                #endregion
            });
        }
        #endregion
        #region Debug
        public void AddDebug() {
            Random r = new Random(DateTime.Now.Millisecond);

            // DEBUG ITEMS
            for (int i = 0; i < Statics.DebugItemPasses; i++) {
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsDrink[0]);
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsFood[0]);
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsJunk[0]);
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsWeapon[0]);
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsContainerBackpack[0]);
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsContainerPouch[0]);
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsArmorChest[0]);
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsArmorFeet[0]);
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsArmorHead[0]);
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsArmorNeck[0]);
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsArmorFinger[0]);
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsArmorFinger[1]);
                World.Regions[0].Subregions[0].Rooms[r.Next(9)].AddItem(ItemTemplates.ItemsArmorShield[0]);
            }

            //EntityBodyPartChest bodyPartChest = new EntityBodyPartChest();

            //ItemArmorChest chest = new ItemArmorChest();
            //chest.Name = "Chest Name";
            //chest.Description = "Chest Description";
            //chest.ArmorFactor = 5;
            //ItemArmorChest chestClone = chest.DeepClone();

            //bodyPartChest.Item = chestClone;
            //EntityBodyPartChest bodyPartChestClone = bodyPartChest.DeepClone();

            // DEBUG NPCS
            for (int i = 0; i < Statics.DebugNPCCount; i++) {
                int region = 0;
                int subregion = 0;
                int room = r.Next(9);
                EntityNPCBase npc = EntityNPCTemplates.NPCTemplates.Random().DeepClone();
                npc.Coordinates.Set(region, subregion, room);
                World.Regions[region].Subregions[subregion].Rooms[room].NPCs.Add(npc);
            }
        }
        #endregion

        #region Touch
        Point initialpoint;
        private void Page_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) {
            initialpoint = e.Position;
        }
        private void Page_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e) {
            if (e.IsInertial) {
                e.Complete();

                // int threshold = 20;
                Point currentpoint = e.Position;

                double deltaX = currentpoint.X - initialpoint.X;
                double deltaY = currentpoint.Y - initialpoint.Y;

                AppendDebugText("X: " + deltaX.ToString());
                AppendDebugText("Y: " + deltaY.ToString());

                // threshold percentage
                double dThreshold = 0.5;

                double absX = Math.Abs(deltaX);
                double absY = Math.Abs(deltaY);
                double xyDelta = absX - absY;

                if (xyDelta >= 0) {
                    // x >= y
                    double deltaPercentage = xyDelta / absX;
                    if (deltaPercentage > dThreshold) {
                        // only use x
                        if (deltaX > 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, "e")); }
                        else if (deltaX < 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, "w")); }
                    }
                    else {
                        // use x and y
                        if (deltaX > 0) {
                            if (deltaY > 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, "se")); }
                            else if (deltaY < 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, "ne")); }
                        }
                        else if (deltaX < 0) {
                            if (deltaY > 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, "sw")); }
                            else if (deltaY < 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, "nw")); }
                        }
                    }
                }
                else {
                    // y > x
                    double deltaPercentage = -xyDelta / absY;
                    if (deltaPercentage > dThreshold) {
                        // only use y
                        if (deltaY > 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, "s")); }
                        else if (deltaY < 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, "n")); }
                    }
                    else {
                        // use x and y
                        if (deltaX > 0) {
                            if (deltaY > 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, "se")); }
                            else if (deltaY < 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, "ne")); }
                        }
                        else if (deltaX < 0) {
                            if (deltaY > 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, "sw")); }
                            else if (deltaY < 0) { Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, "nw")); }
                        }
                    }
                }
            }
        }
        private void txtOutput_Tapped(object sender, TappedRoutedEventArgs e) {
            RichTextBlock r = sender as RichTextBlock;
            TextPointer t = r.GetPositionFromPoint(e.GetPosition(r));
            TextElement element = t.Parent as TextElement;

            Run run = element as Run;
            txtCurrentRun.Text = run.Text;
        }
        private void rectSwipe_Tapped(object sender, TappedRoutedEventArgs e) {
            Input.Enqueue(new ParsedInput(INPUT_SOURCE.PLAYER, "out"));
        }
        #endregion
    }
}

// TODO: use compressed world once finalized
//await World.LoadCompressed();

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
