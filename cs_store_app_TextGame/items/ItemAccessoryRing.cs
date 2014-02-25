using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "ItemAccessoryRing", Namespace = "cs_store_app_TextGame")]
    public class ItemAccessoryRing : ItemAccessory
    {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.ACCESSORY_RING; } }

        public ItemAccessoryRing(XElement itemNode) : base(itemNode) { }
    }
}
