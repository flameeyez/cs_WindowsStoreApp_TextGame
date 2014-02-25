using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "ItemArmorFeet", Namespace = "cs_store_app_TextGame")]
    public class ItemArmorFeet : ItemArmor
    {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.ARMOR_FEET; } }

        public ItemArmorFeet(XElement itemNode) : base(itemNode) { }
    }
}
