using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "ItemContainer", Namespace = "cs_store_app_TextGame")]
    public class ItemContainer : Item
    {
        [DataMember]
        public bool Closed { get; set; }
        [DataMember]
        public bool Closable { get; set; }
        [DataMember]
        public int MaximumWeight { get; set; }
        public override ITEM_TYPE Type { get { return ITEM_TYPE.CONTAINER; } }
        [DataMember]
        public Inventory Items = new Inventory();
        public ItemContainer(XElement itemNode) : base(itemNode)
        {
            Closed = bool.Parse(itemNode.Element("closed").Value);
            Closable = bool.Parse(itemNode.Element("closable").Value);
            MaximumWeight = int.Parse(itemNode.Element("maximum-weight").Value);
        }

        public string ItemsString
        {
            get
            {
                if(Items.Items.Count == 0)
                {
                    return "The " + Name + " is empty.";
                }

                string strItemsString = "In the " + Name + ", you see ";
                strItemsString += Items.DisplayString();
                return strItemsString;
            }
        }
    }
}