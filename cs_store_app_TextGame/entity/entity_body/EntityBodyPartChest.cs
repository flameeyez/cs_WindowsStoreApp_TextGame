using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityBodyPartChest", Namespace = "cs_store_app_TextGame")]
    public class EntityBodyPartChest : EntityBodyPart
    {
        public EntityBodyPartChest() { }
        public EntityBodyPartChest(XElement bodyPartElement)
        {
            int nItemTemplateIndex = -1;
            if (!int.TryParse(bodyPartElement.Value, out nItemTemplateIndex)) { return; }

            Item = (ItemArmor)ItemTemplates.ItemArmorChestTemplates[nItemTemplateIndex].Clone();
        }
        public override ITEM_TYPE Type
        {
            get
            {
                return ITEM_TYPE.ARMOR_CHEST;
            }
        }
    }
}
