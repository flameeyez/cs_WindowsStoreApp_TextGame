using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityBodyPartFeet", Namespace = "cs_store_app_TextGame")]
    public class EntityBodyPartFeet : EntityBodyPart
    {
        public EntityBodyPartFeet() { }
        public EntityBodyPartFeet(XElement bodyPartElement)
        {
            int nItemTemplateIndex = -1;
            if (!int.TryParse(bodyPartElement.Value, out nItemTemplateIndex)) { return; }

            Item = ItemTemplates.ItemsArmorFeet[nItemTemplateIndex].DeepClone(0);
        }
        public override ITEM_TYPE Type
        {
            get
            {
                return ITEM_TYPE.ARMOR_FEET;
            }
        }
    }
}
