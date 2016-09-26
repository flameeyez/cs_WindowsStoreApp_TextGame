using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public class ItemArmorShield : ItemArmor {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.ARMOR_SHIELD; } }
        protected ItemArmorShield(ItemArmorShield template) : base(template) { }
        public ItemArmorShield(XElement itemNode) : base(itemNode) { }
        public override Item Clone() {
            return new ItemArmorShield(this);
        }
    }
}
