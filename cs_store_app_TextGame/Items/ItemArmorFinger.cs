using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "ItemArmorFinger", Namespace = "cs_store_app_TextGame")]
    public class ItemArmorFinger : ItemArmor
    {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.ARMOR_FINGER; } }

        public ItemArmorFinger(XElement itemNode) : base(itemNode) { }
    }
}
