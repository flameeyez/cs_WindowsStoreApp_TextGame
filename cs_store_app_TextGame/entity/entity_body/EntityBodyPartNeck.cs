using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityBodyPartNeck", Namespace = "cs_store_app_TextGame")]
    public class EntityBodyPartNeck : EntityBodyPart
    {
        public EntityBodyPartNeck() { }
        public EntityBodyPartNeck(XElement bodyPartElement)
        {
            int nItemTemplateIndex = -1;
            if (!int.TryParse(bodyPartElement.Value, out nItemTemplateIndex)) { return; }

            Item = (ItemArmor)ItemTemplates.ItemArmorNeckTemplates[nItemTemplateIndex].Clone();
        }
        public override ITEM_TYPE Type
        {
            get
            {
                return ITEM_TYPE.ARMOR_NECK;
            }
        }
    }
}
