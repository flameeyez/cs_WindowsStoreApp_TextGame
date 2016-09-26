using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public class ItemArmorChest : ItemArmor {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.ARMOR_CHEST; } }
        protected ItemArmorChest(ItemArmorChest template) : base(template) { }
        public ItemArmorChest(XElement itemNode) : base(itemNode) { }
        public override Item Clone() {
            return new ItemArmorChest(this);
        }
    }
}
