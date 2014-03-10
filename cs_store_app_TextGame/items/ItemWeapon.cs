using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "ItemWeapon", Namespace = "cs_store_app_TextGame")]
    public class ItemWeapon : Item
    {
        [DataMember]
        public int AttackPower { get; set; }
        public override ITEM_TYPE Type { get { return ITEM_TYPE.WEAPON; } }
        public ItemWeapon(XElement itemNode) : base(itemNode)
        {
            AttackPower = int.Parse(itemNode.Element("attack-power").Value);
        }
    }
}
