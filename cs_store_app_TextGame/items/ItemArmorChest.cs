using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "ItemArmorChest", Namespace = "cs_store_app_TextGame")]
    public class ItemArmorChest : ItemArmor
    {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.ARMOR_CHEST; } }

        public ItemArmorChest(XElement itemNode) : base(itemNode) { }
    }
}
