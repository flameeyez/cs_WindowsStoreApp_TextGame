using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "Inventory", Namespace = "cs_store_app_TextGame")]
    public class Inventory
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
        public Item Get(string strKeyword, ITEM_TYPE itemType = ITEM_TYPE.ANY, int nRequestedOccurrence = 0)
        {
            if (Items.Count == 0) { return null; }
            if (strKeyword == "") { return null; }

            int nOccurrences = -1;
            foreach(Item item in Items)
            {
                if (item.Type == itemType || itemType == ITEM_TYPE.ANY)
                {
                    if(item.IsKeyword(strKeyword))
                    {
                        nOccurrences++;
                        if (nOccurrences == nRequestedOccurrence)
                        {
                            return item;
                        }
                    }
                }
            }

            return null;
        }
        public List<Item> GetItemsOfType(ITEM_TYPE itemType)
        {
            List<Item> returnList = new List<Item>();
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
        public string DisplayString(bool bAppendPeriod = true)
        {
            string strReturn = "";

            if (Items.Count > 2)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    strReturn += "a";
                    if((Items[i].Name[0]).IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " ";
                    strReturn += Items[i].Name;
                    if (i == Items.Count - 2)
                    {
                        strReturn += ", and ";
                    }
                    else if (i < Items.Count - 1)
                    {
                        strReturn += ", ";
                    }
                }
            }
            else if (Items.Count == 2)
            {
                strReturn += "a";
                if ((Items[0].Name[0]).IsVowel())
                {
                    strReturn += "n";
                }
                strReturn += " ";
                strReturn += Items[0].Name;
                strReturn += " and a";
                if ((Items[1].Name[0]).IsVowel())
                {
                    strReturn += "n";
                }
                strReturn += " ";
                strReturn += Items[1].Name;
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

            if (bAppendPeriod)
            {
                strReturn += ".";
            }

            return strReturn;
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);
        }
    }
}
