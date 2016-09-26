using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public class ItemContainerPouch : ItemContainer {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.CONTAINER_WAIST; } }
        protected ItemContainerPouch(ItemContainerPouch template) : base(template) { }
        public ItemContainerPouch(XElement itemNode) : base(itemNode) { }
        public override Item Clone() {
            return new ItemContainerPouch(this);
        }
    }
}
