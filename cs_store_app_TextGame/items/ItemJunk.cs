using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public class ItemJunk : Item {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.JUNK; } }
        protected ItemJunk(ItemJunk template) : base(template) { }
        public ItemJunk(XElement itemNode) : base(itemNode) { }
        public override Item Clone() {
            return new ItemJunk(this);
        }
    }
}
