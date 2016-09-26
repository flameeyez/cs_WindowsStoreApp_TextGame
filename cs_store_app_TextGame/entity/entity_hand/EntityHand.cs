using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityHand", Namespace = "cs_store_app_TextGame")]
    public class EntityHand
    {
        [DataMember]
        public Item Item { get; set; }
        [DataMember]
        public BODY_PART_CONDITION Condition { get; set; }

        // TODO: DoGet, base on Condition

        //<hand>
        //  <weapon>0</weapon>
        //</hand>
        //<hand>
        //  <armor-shield>0</armor-shield>
        //</hand>
        public EntityHand() { }
        public EntityHand(XElement handElement)
        {
            if (handElement.Elements().Count() == 0) { return; }

            XElement itemElement = handElement.Elements().First();
            int nItemTemplateIndex = int.Parse(itemElement.Value);
            switch (itemElement.Name.LocalName)
            {
                case "weapon":
                    Item = ItemTemplates.ItemWeaponTemplates[nItemTemplateIndex].Clone();
                    break;
                case "armor-shield":
                    Item = ItemTemplates.ItemArmorShieldTemplates[nItemTemplateIndex].Clone();
                    break;
                case "armor-head":
                    Item = ItemTemplates.ItemArmorHeadTemplates[nItemTemplateIndex].Clone();
                    break;
                case "armor-feet":
                    Item = ItemTemplates.ItemArmorFeetTemplates[nItemTemplateIndex].Clone();
                    break;
                case "armor-chest":
                    Item = ItemTemplates.ItemArmorChestTemplates[nItemTemplateIndex].Clone();
                    break;
                case "armor-finger":
                    Item = ItemTemplates.ItemArmorFingerTemplates[nItemTemplateIndex].Clone();
                    break;
                case "armor-neck":
                    Item = ItemTemplates.ItemArmorNeckTemplates[nItemTemplateIndex].Clone();
                    break;
                case "drink":
                    Item = ItemTemplates.ItemDrinkTemplates[nItemTemplateIndex].Clone();
                    break;
                case "food":
                    Item = ItemTemplates.ItemFoodTemplates[nItemTemplateIndex].Clone();
                    break;
                case "container-backpack":
                    Item = ItemTemplates.ItemContainerBackpackTemplates[nItemTemplateIndex].Clone();
                    break;
                case "container-pouch":
                    Item = ItemTemplates.ItemContainerPouchTemplates[nItemTemplateIndex].Clone();
                    break;
                case "junk":
                    Item = ItemTemplates.ItemJunkTemplates[nItemTemplateIndex].Clone();
                    break;
                default:
                    throw new Exception("Error in EntityHand constructor. Bad item type.");
            }
        }
    }
}
