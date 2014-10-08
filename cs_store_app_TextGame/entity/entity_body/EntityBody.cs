using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityBody", Namespace = "cs_store_app_TextGame")]
    public class EntityBody
    {
        [DataMember]
        private int _defensepower;
        public int DefensePower
        {
            get
            {
                return _defensepower;
            }
        }

        [DataMember]
        public List<EntityBodyPart> BodyParts = new List<EntityBodyPart>();

        // result determines message to display

        public Handler DoEquip(EntityHand hand)
        {
            ItemArmor armor = hand.Item as ItemArmor;
            EQUIP_RESULT bestResult = EQUIP_RESULT.NOT_EQUIPPABLE;

            foreach (EntityBodyPart part in BodyParts)
            {
                EQUIP_RESULT currentResult = part.DoEquip(armor);

                if (currentResult == EQUIP_RESULT.EQUIPPED)
                {
                    _defensepower += armor.ArmorFactor;
                    hand.Item = null;
                    return Handler.HANDLED(Statics.ItemTypeToEquipMessage[armor.Type], armor.NameAsParagraph);
                }
                else if (currentResult > bestResult)
                {
                    bestResult = currentResult;
                }
            }

            return Handler.HANDLED(Statics.EquipResultToMessage[bestResult]);
        }
        public EQUIP_RESULT DoEquip(ItemArmor itemToEquip)
        {
            EQUIP_RESULT bestResult = EQUIP_RESULT.NOT_EQUIPPABLE;

            foreach(EntityBodyPart part in BodyParts)
            {
                EQUIP_RESULT currentResult = part.DoEquip(itemToEquip);

                if (currentResult == EQUIP_RESULT.EQUIPPED) 
                {
                    _defensepower += itemToEquip.ArmorFactor;
                    return currentResult; 
                }
                else if (currentResult > bestResult)
                {
                    bestResult = currentResult; 
                }
            }

            return bestResult;
        }

        // TODO: how to determine which message to display?
        // is returning an item ok? return status instead?
        public Handler DoRemove(string strKeyword, EntityHand hand)
        {
            foreach(EntityBodyPart part in BodyParts)
            {
                if (part.Item == null) { continue; }
                if (!part.Item.IsKeyword(strKeyword)) { continue; }

                _defensepower -= part.Item.ArmorFactor;
                hand.Item = part.Item;
                part.Item = null;

                MESSAGE_ENUM message = Statics.ItemTypeToRemoveMessage[hand.Item.Type];
                return Handler.HANDLED(message, hand.Item.NameAsParagraph);
            }

            return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT);
        }

        public Item GetItem(string strKeyword, bool bRemove)
        {
            foreach(EntityBodyPart part in BodyParts)
            {
                if (part.Item == null) { continue; }
                if (!part.Item.IsKeyword(strKeyword)) { continue; }

                Item item = part.Item;
                if (bRemove) { part.Item = null; }
                return item;
            }

            return null;
        }
    }
}
