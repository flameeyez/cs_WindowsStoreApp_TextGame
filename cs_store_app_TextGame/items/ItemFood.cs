using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "ItemFood", Namespace = "cs_store_app_TextGame")]
    public class ItemFood : Item
    {
        [DataMember]
        public int NumberOfBites { get; set; }
        [DataMember]
        public int HealthPerBite { get; set; }
        public override ITEM_TYPE Type { get { return ITEM_TYPE.FOOD; } }
        public ItemFood(XElement itemNode) : base(itemNode)
        {
            NumberOfBites = int.Parse(itemNode.Element("number-of-bites").Value);
            HealthPerBite = int.Parse(itemNode.Element("health-per-bite").Value);
        }
    }
}
