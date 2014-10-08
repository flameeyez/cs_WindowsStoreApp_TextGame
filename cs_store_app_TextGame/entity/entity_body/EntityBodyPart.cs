using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public enum BODY_PART_CONDITION
    {
        MISSING = 0,
        DISABLED = 1,
        INJURED = 2,
        NORMAL = 3
    }

    [DataContract(Name = "EntityBodyPart", Namespace = "cs_store_app_TextGame")]
    [KnownType(typeof(EntityBodyPartChest))]
    [KnownType(typeof(EntityBodyPartFeet))]
    [KnownType(typeof(EntityBodyPartFinger))]
    [KnownType(typeof(EntityBodyPartHead))]
    [KnownType(typeof(EntityBodyPartNeck))]
    public abstract class EntityBodyPart
    {
        [DataMember]
        public BODY_PART_CONDITION Condition { get; set; }
        [DataMember]
        public ItemArmor Item { get; set; }
        public abstract ITEM_TYPE Type { get; }
        [DataMember]
        public EntityBase Owner { get; set; }

        // Body.DoEquip takes the best (highest value) result from BodyParts.DoEquip
        public EQUIP_RESULT DoEquip(ItemArmor itemToEquip)
        {
            if (Condition == BODY_PART_CONDITION.MISSING) { return EQUIP_RESULT.BODY_PART_MISSING; }
            if (itemToEquip.Type != this.Type) { return EQUIP_RESULT.NOT_EQUIPPABLE; }
            if (this.Item != null) { return EQUIP_RESULT.ITEM_ALREADY_EQUIPPED; }

            this.Item = itemToEquip;
            return EQUIP_RESULT.EQUIPPED;
        }

        public EntityBodyPart()
        {
            Condition = BODY_PART_CONDITION.NORMAL;
        }
    }
}

        //public void Injure() 
        //{
        //    if (Condition == BODY_PART_CONDITION.MISSING) { return; }
            
        //    Condition--;

        //    if (Condition == BODY_PART_CONDITION.MISSING) 
        //    {
        //        Owner.CurrentRoom.Items.Add(this.Item);
                
        //        // TODO: DISPLAY MESSAGE
        //        // TODO: CONSIDER ABSORBING ITEMSLOT CLASSES
        //        return;
        //    }
        //}