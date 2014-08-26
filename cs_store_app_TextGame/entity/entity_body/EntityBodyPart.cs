using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public abstract class EntityBodyPart
    {
        public enum BODY_PART_CONDITION
        {
            MISSING = 0,
            DISABLED = 1,
            INJURED = 2,
            NORMAL = 3
        }

        public BODY_PART_CONDITION Condition { get; set; }
        public Item Item { get; set; }
        public abstract ITEM_TYPE Type { get; }
        public Entity Owner { get; set; }

        public Status.EQUIP_STATUS DoEquip(Item itemToEquip)
        {
            if (Condition == BODY_PART_CONDITION.MISSING) { return Status.EQUIP_STATUS.BODY_PART_MISSING; }
            if (itemToEquip.Type != this.Type) { return Status.EQUIP_STATUS.NOT_EQUIPPABLE; }
            if (this.Item != null) { return Status.EQUIP_STATUS.ITEM_ALREADY_EQUIPPED; }

            this.Item = itemToEquip;
            return Status.EQUIP_STATUS.EQUIPPED;
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