using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public class ItemArmorFinger : ItemArmor {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.ARMOR_FINGER; } }
        protected ItemArmorFinger(ItemArmorFinger template) : base(template) { }
        public ItemArmorFinger(XElement itemNode) : base(itemNode) { }
        public override Item Clone() {
            return new ItemArmorFinger(this);
        }
    }
}
