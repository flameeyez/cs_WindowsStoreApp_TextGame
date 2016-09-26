using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public class ItemFood : Item {
        public int NumberOfBites { get; set; }
        public int HealthPerBite { get; set; }
        public override ITEM_TYPE Type { get { return ITEM_TYPE.FOOD; } }
        protected ItemFood(ItemFood template) : base(template) {
            NumberOfBites = template.NumberOfBites;
            HealthPerBite = template.HealthPerBite;
        }
        public ItemFood(XElement itemNode) : base(itemNode) {
            NumberOfBites = int.Parse(itemNode.Element("number-of-bites").Value);
            HealthPerBite = int.Parse(itemNode.Element("health-per-bite").Value);
        }
        public override Item Clone() {
            return new ItemFood(this);
        }
    }
}
