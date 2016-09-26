using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public class ItemArmorFeet : ItemArmor {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.ARMOR_FEET; } }
        protected ItemArmorFeet(ItemArmorFeet template) : base(template) { }
        public ItemArmorFeet(XElement itemNode) : base(itemNode) { }
        public override Item Clone() {
            return new ItemArmorFeet(this);
        }
    }
}
