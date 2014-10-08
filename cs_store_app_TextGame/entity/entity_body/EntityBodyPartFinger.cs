using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityBodyPartFinger", Namespace = "cs_store_app_TextGame")]
    public class EntityBodyPartFinger : EntityBodyPart
    {
        public EntityBodyPartFinger() { }
        public EntityBodyPartFinger(XElement bodyPartElement)
        {
            int nItemTemplateIndex = -1;
            if (!int.TryParse(bodyPartElement.Value, out nItemTemplateIndex)) { return; }

            Item = ItemTemplates.ItemsArmorFinger[nItemTemplateIndex].DeepClone(0);
        }
        public override ITEM_TYPE Type
        {
            get
            {
                return ITEM_TYPE.ARMOR_FINGER;
            }
        }
    }
}
