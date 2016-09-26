using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityBodyPartHead", Namespace = "cs_store_app_TextGame")]
    public class EntityBodyPartHead : EntityBodyPart 
    {
        public EntityBodyPartHead() { }
        public EntityBodyPartHead(XElement bodyPartElement)
        {
            int nItemTemplateIndex = -1;
            if (!int.TryParse(bodyPartElement.Value, out nItemTemplateIndex)) { return; }

            Item = (ItemArmor)ItemTemplates.ItemArmorHeadTemplates[nItemTemplateIndex].Clone();
        }
        public override ITEM_TYPE Type 
        {
            get
            {
                return ITEM_TYPE.ARMOR_HEAD; 
            }
        }
    }
}
