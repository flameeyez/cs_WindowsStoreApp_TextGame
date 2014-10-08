using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityContainerSlots", Namespace = "cs_store_app_TextGame")]
    public class EntityContainerSlots
    {
        [DataMember]
        public List<EntityContainerSlot> ContainerSlots = new List<EntityContainerSlot>();

        public Handler DoEquip(EntityHand hand)
        {
            ItemContainer container = hand.Item as ItemContainer;
            EQUIP_RESULT bestResult = EQUIP_RESULT.NOT_EQUIPPABLE;

            foreach (EntityContainerSlot slot in ContainerSlots)
            {
                EQUIP_RESULT currentResult = slot.DoEquip(container);

                if (currentResult == EQUIP_RESULT.EQUIPPED)
                {
                    hand.Item = null;
                    return Handler.HANDLED(Statics.ItemTypeToEquipMessage[container.Type], container.NameAsParagraph);
                }
                else if (currentResult > bestResult)
                {
                    bestResult = currentResult;
                }
            }

            return Handler.HANDLED(Statics.EquipResultToMessage[bestResult]);
        }

        public ItemContainer GetContainer(string strKeyword)
        {
            foreach(EntityContainerSlot slot in ContainerSlots)
            {
                if (slot.Container == null) { continue; }
                if (slot.Container.IsKeyword(strKeyword)) { return slot.Container; }
            }

            return null;
        }

        public Item FindItem(string strKeyword)
        {
            foreach (EntityContainerSlot slot in ContainerSlots)
            {
                if (slot.Container == null) { continue; }
                Item item = slot.Container.Items.Find(strKeyword);
                if (item != null) { return item; }
            }

            return null;
        }
    }
}
