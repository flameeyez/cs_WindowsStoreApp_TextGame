using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public class ItemWeapon : Item {
        public int AttackPower { get; set; }
        public override ITEM_TYPE Type { get { return ITEM_TYPE.WEAPON; } }
        protected ItemWeapon(ItemWeapon template) : base(template) {
            AttackPower = template.AttackPower;
        }
        public ItemWeapon(XElement itemNode) : base(itemNode) {
            AttackPower = int.Parse(itemNode.Element("attack-power").Value);
        }
        public override Item Clone() {
            return new ItemWeapon(this);
        }
    }
}
