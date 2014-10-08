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
                    Item = ItemTemplates.ItemsWeapon[nItemTemplateIndex].DeepClone(0);
                    break;
                case "armor-shield":
                    Item = ItemTemplates.ItemsArmorShield[nItemTemplateIndex].DeepClone(0);
                    break;
                case "armor-head":
                    Item = ItemTemplates.ItemsArmorHead[nItemTemplateIndex].DeepClone(0);
                    break;
                case "armor-feet":
                    Item = ItemTemplates.ItemsArmorFeet[nItemTemplateIndex].DeepClone(0);
                    break;
                case "armor-chest":
                    Item = ItemTemplates.ItemsArmorChest[nItemTemplateIndex].DeepClone(0);
                    break;
                case "armor-finger":
                    Item = ItemTemplates.ItemsArmorFinger[nItemTemplateIndex].DeepClone(0);
                    break;
                case "armor-neck":
                    Item = ItemTemplates.ItemsArmorNeck[nItemTemplateIndex].DeepClone(0);
                    break;
                case "drink":
                    Item = ItemTemplates.ItemsDrink[nItemTemplateIndex].DeepClone(0);
                    break;
                case "food":
                    Item = ItemTemplates.ItemsFood[nItemTemplateIndex].DeepClone(0);
                    break;
                case "container-backpack":
                    Item = ItemTemplates.ItemsContainerBackpack[nItemTemplateIndex].DeepClone(0);
                    break;
                case "container-pouch":
                    Item = ItemTemplates.ItemsContainerPouch[nItemTemplateIndex].DeepClone(0);
                    break;
                case "junk":
                    Item = ItemTemplates.ItemsJunk[nItemTemplateIndex].DeepClone(0);
                    break;
                default:
                    throw new Exception("Error in EntityHand constructor. Bad item type.");
            }
        }
    }
}
