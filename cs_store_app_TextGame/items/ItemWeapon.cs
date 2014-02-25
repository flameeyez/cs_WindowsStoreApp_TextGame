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
        public int MinimumDamage { get; set; }
        [DataMember]
        public int MaximumDamage { get; set; }
        public override ITEM_TYPE Type { get { return ITEM_TYPE.WEAPON; } }
        public ItemWeapon(XElement itemNode) : base(itemNode)
        {
            MinimumDamage = int.Parse(itemNode.Element("minimum-damage").Value);
            MaximumDamage = int.Parse(itemNode.Element("maximum-damage").Value);
        }
    }
}
