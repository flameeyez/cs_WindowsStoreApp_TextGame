using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public class ItemDrink : Item {
        public int NumberOfDrinks { get; set; }
        public int MagicPerDrink { get; set; }
        public override ITEM_TYPE Type { get { return ITEM_TYPE.DRINK; } }
        protected ItemDrink(ItemDrink template) : base(template) {
            NumberOfDrinks = template.NumberOfDrinks;
            MagicPerDrink = template.MagicPerDrink;
        }
        public ItemDrink(XElement itemNode) : base(itemNode) {
            NumberOfDrinks = int.Parse(itemNode.Element("number-of-drinks").Value);
            MagicPerDrink = int.Parse(itemNode.Element("magic-per-drink").Value);
        }
        public override Item Clone() {
            return new ItemDrink(this);
        }
    }
}
