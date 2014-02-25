using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "ItemDrink", Namespace = "cs_store_app_TextGame")]
    public class ItemDrink : Item
    {
        [DataMember]
        public int NumberOfDrinks { get; set; }
        [DataMember]
        public int MagicPerDrink { get; set; }
        public override ITEM_TYPE Type { get { return ITEM_TYPE.DRINK; } }
        public ItemDrink(XElement itemNode) : base(itemNode)
        {
            NumberOfDrinks = int.Parse(itemNode.Element("number-of-drinks").Value);
            MagicPerDrink = int.Parse(itemNode.Element("magic-per-drink").Value);
        }

    }
}
