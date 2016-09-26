using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public abstract class ItemArmor : Item {
        public int ArmorFactor { get; set; }

        protected ItemArmor(ItemArmor template) : base(template) {
            ArmorFactor = template.ArmorFactor;
        }
        public ItemArmor(XElement itemNode) : base(itemNode) {
            ArmorFactor = int.Parse(itemNode.Element("armor-factor").Value);
        }
    }
}
