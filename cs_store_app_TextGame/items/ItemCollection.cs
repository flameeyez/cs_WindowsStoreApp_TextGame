using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame {
    public class ItemCollection {
        // TODO: maximum weight?

        #region Attributes
        private List<Item> Items = new List<Item>();
        private double _weight;
        public double Weight { get { return _weight; } }
        public int Count { get { return Items.Count; } }
        #endregion

        #region Methods
        public void Add(Item itemToAdd) {
            Items.Add(itemToAdd);
            _weight += itemToAdd.Weight;
        }
        public void Remove(Item itemToRemove) {
            if (itemToRemove == null) { return; }
            if (!Items.Contains(itemToRemove)) { return; }

            _weight -= itemToRemove.Weight;
            Items.Remove(itemToRemove);
        }
        public void Remove(int index) {
            if (index < 0) { return; }
            if (index >= Items.Count) { return; }

            _weight -= Items[index].Weight;
            Items.RemoveAt(index);
        }

        // does NOT remove the item
        public Item Find(string strKeyword, ITEM_TYPE itemType = ITEM_TYPE.ANY, int nRequestedOccurrence = 0) {
            if (Items.Count == 0) { return null; }
            if (strKeyword == "") { return null; }

            int nOccurrences = -1;
            for (int i = Items.Count() - 1; i >= 0; i--) {
                if (Items[i].Type == itemType || itemType == ITEM_TYPE.ANY) {
                    if (Items[i].IsKeyword(strKeyword)) {
                        nOccurrences++;
                        if (nOccurrences == nRequestedOccurrence) {
                            return Items[i];
                        }
                    }
                }
            }

            return null;
        }
        public List<Item> GetItemsOfType(ITEM_TYPE itemType) {
            List<Item> returnList = new List<Item>();
            foreach (Item item in Items) {
                if ((item.Type & itemType) == item.Type) {
                    returnList.Add(item);
                }
            }

            return returnList;
        }
        public Item GetRandomItem(ITEM_TYPE itemType = ITEM_TYPE.ANY) {
            if (Items.Count == 0) { return null; }

            Random r = new Random(DateTime.Now.Millisecond);

            if (itemType == ITEM_TYPE.ANY) {
                Item item = Items[r.Next(Items.Count)];
                return item;
            }

            List<Item> items = GetItemsOfType(itemType);
            if (items.Count == 0) { return null; }
            return items[r.Next(items.Count)];
        }
        public void RemoveItem(Item item) {
            Items.Remove(item);
        }
        #endregion

        #region Display
        // You also see...
        public Paragraph RoomDisplayParagraph {
            get {
                if (Items.Count == 0) { return null; }

                Paragraph p = new Paragraph();
                string str = "You also see ";

                if (Items.Count > 2) {
                    for (int i = Items.Count() - 1; i >= 0; i--) {
                        str += (Items[i].Name[0]).IsVowel() ? "an " : "a ";
                        p.Inlines.Add(str.ToRun());
                        p.Inlines.Add(Items[i].Name.ToRun(Statics.ItemBrushColor));
                        if (i == 1) { str = ", and "; }
                        else if (i > 0) { str = ", "; }
                    }
                }
                else if (Items.Count == 2) {
                    str += (Items[1].Name[0]).IsVowel() ? "an " : "a ";
                    p.Inlines.Add(str.ToRun());
                    p.Inlines.Add(Items[1].Name.ToRun(Statics.ItemBrushColor));

                    str = (Items[0].Name[0]).IsVowel() ? " and an " : " and a ";
                    p.Inlines.Add(str.ToRun());
                    p.Inlines.Add(Items[0].Name.ToRun(Statics.ItemBrushColor));
                }
                else if (Items.Count == 1) {
                    str += (Items[0].Name[0]).IsVowel() ? "an " : "a ";
                    p.Inlines.Add(str.ToRun());
                    p.Inlines.Add(Items[0].Name.ToRun(Statics.ItemBrushColor));
                }

                p.Inlines.Add((".\n").ToRun());
                return p;
            }
        }
        // In the <container>, you see...
        public Paragraph ContainerDisplayParagraph(Paragraph NameAsParagraph) {
            Paragraph p = new Paragraph();

            if (Items.Count == 0) {
                p.Inlines.Add("The ".ToRun());
                p.Merge(NameAsParagraph);
                p.Inlines.Add(" is empty.".ToRun());
                return p;
            }

            p.Inlines.Add("In the ".ToRun());
            p.Merge(NameAsParagraph);

            string str = ", you see ";

            if (Items.Count > 2) {
                for (int i = Items.Count() - 1; i >= 0; i--) {
                    str += (Items[i].Name[0]).IsVowel() ? "an " : "a ";
                    p.Inlines.Add(str.ToRun());
                    p.Inlines.Add(Items[i].Name.ToRun(Statics.ItemBrushColor));

                    if (i == 1) { str = ", and "; }
                    else if (i > 0) { str = ", "; }
                }
            }
            else if (Items.Count == 2) {
                str += (Items[1].Name[0]).IsVowel() ? "an " : "a ";
                p.Inlines.Add(str.ToRun());
                p.Inlines.Add(Items[1].Name.ToRun(Statics.ItemBrushColor));

                str = (Items[0].Name[0]).IsVowel() ? " and an " : " and a ";
                p.Inlines.Add(str.ToRun());
                p.Inlines.Add(Items[0].Name.ToRun(Statics.ItemBrushColor));
            }
            else if (Items.Count == 1) {
                str += (Items[0].Name[0]).IsVowel() ? "an " : "a ";
                p.Inlines.Add(str.ToRun());
                p.Inlines.Add(Items[0].Name.ToRun(Statics.ItemBrushColor));
            }

            p.Inlines.Add((".\n").ToRun());
            return p;
        }
        #endregion

        public void Clear() { Items.Clear(); }
        public void Cleanup(int nThreshold = 5) { if (Items.Count > nThreshold) { Items.Clear(); } }
        public ItemCollection Clone() {
            ItemCollection copy = new ItemCollection();
            foreach(Item item in Items) {
                copy.Add(item.Clone());
            }
            return copy;
        }
    }
}
