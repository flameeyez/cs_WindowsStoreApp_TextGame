using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public class ItemArmorHead : ItemArmor {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.ARMOR_HEAD; } }
        protected ItemArmorHead(ItemArmorHead template) : base(template) { }
        public ItemArmorHead(XElement itemNode) : base(itemNode) { }
        public override Item Clone() {
            return new ItemArmorHead(this);
        }
    }
}
