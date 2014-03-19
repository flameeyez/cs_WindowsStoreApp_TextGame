using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "Inventory", Namespace = "cs_store_app_TextGame")]
    public class ItemCollection
    {
        [DataMember]
        public List<Item> Items = new List<Item>();
        [DataMember]
        private double _weight;
        public double Weight 
        {
            get
            { 
                return _weight; 
            }
        }
        public int Count { get { return Items.Count; } }

        // TODO: maximum weight?

        public void Add(Item itemToAdd)
        {
            Items.Add(itemToAdd);
            _weight += itemToAdd.Weight;
        }
        public void Remove(Item itemToRemove)
        {
            if (itemToRemove == null) { return; }
            if (!Items.Contains(itemToRemove)) { return; }

            _weight -= itemToRemove.Weight;
            Items.Remove(itemToRemove);
        }
        public void Remove(int index)
        {
            if (index < 0) { return; }
            if (index >= Items.Count) { return; }

            _weight -= Items[index].Weight;
            Items.RemoveAt(index);
        }
        
        // does NOT remove the item
        public Item Find(string strKeyword, ITEM_TYPE itemType = ITEM_TYPE.ANY, int nRequestedOccurrence = 0)
        {
            if (Items.Count == 0) { return null; }
            if (strKeyword == "") { return null; }

            int nOccurrences = -1;
            for (int i = Items.Count() - 1; i >= 0; i--)
            {
                if (Items[i].Type == itemType || itemType == ITEM_TYPE.ANY)
                {
                    if (Items[i].IsKeyword(strKeyword))
                    {
                        nOccurrences++;
                        if (nOccurrences == nRequestedOccurrence)
                        {
                            return Items[i];
                        }
                    }
                }                
            }

            return null;
        }
        public List<Item> GetItemsOfType(ITEM_TYPE itemType)
        {
            List<Item> returnList = new List<Item>();
            //for (int i = Items.Count() - 1; i >= 0; i--)
            //{
            //    if(Items[i].Type == itemType)
            //    {
            //        returnList.Add(Items[i]);
            //    }
            //}
            foreach(Item item in Items)
            {
                if(item.Type == itemType)
                {
                    returnList.Add(item);
                }
            }

            return returnList;
        }
        public Item GetRandomItem(ITEM_TYPE itemType = ITEM_TYPE.ANY)
        {
            if (Items.Count == 0) { return null; }

            Random r = new Random(DateTime.Now.Millisecond);

            if (itemType == ITEM_TYPE.ANY) 
            {
                Item item = Items[r.Next(Items.Count)];
                return item;
            }

            List<Item> items = GetItemsOfType(itemType);
            if (items.Count == 0) { return null; }
            return items[r.Next(items.Count)];
        }
        public string BaseDisplayString
        {
            get
            {
                if (Items.Count == 0) { return ""; }

                string strReturn = "";
                if (Items.Count > 2)
                {
                    for (int i = Items.Count() - 1; i >= 0; i--)
                    {
                        strReturn += "a";
                        if ((Items[i].Name[0]).IsVowel())
                        {
                            strReturn += "n";
                        }
                        strReturn += " ";
                        strReturn += Items[i].Name;
                        if (i == 1)
                        {
                            strReturn += ", and ";
                        }
                        else if (i > 0)
                        {
                            strReturn += ", ";
                        }
                    }
                }
                else if (Items.Count == 2)
                {
                    strReturn += "a";
                    if ((Items[1].Name[0]).IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " ";
                    strReturn += Items[1].Name;
                    strReturn += " and a";
                    if ((Items[0].Name[0]).IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " ";
                    strReturn += Items[0].Name;
                }
                else if (Items.Count == 1)
                {
                    strReturn += "a";
                    if ((Items[0].Name[0]).IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " ";
                    strReturn += Items[0].Name;
                }

                return strReturn;
            }
        }
        public string RoomDisplayString
        {
            get
            {
                if (Items.Count == 0) { return ""; }
                return "You also see " + BaseDisplayString + ".\n";
            }
        }
        public Paragraph RoomDisplayParagraph
        {
            get
            {
                if (Items.Count == 0) { return null; }

                // TODO: color individual items
                Paragraph p = new Paragraph();
                p.Inlines.Add(RoomDisplayString.ToRun());
                return p;
            }
        }
        public void RemoveItem(Item item)
        {
            Items.Remove(item);
        }
    }
}
