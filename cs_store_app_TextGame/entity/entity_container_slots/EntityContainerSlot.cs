using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityContainerSlot", Namespace = "cs_store_app_TextGame")]
    [KnownType(typeof(EntityContainerSlotBackpack))]
    [KnownType(typeof(EntityContainerSlotPouch))]
    public abstract class EntityContainerSlot
    {
        [DataMember]
        public ItemContainer Container { get; set; }
        public abstract ITEM_TYPE Type { get; }

        public EQUIP_RESULT DoEquip(Item item)
        {
            if (Type != item.Type) { return EQUIP_RESULT.NOT_EQUIPPABLE; } 
            if (Container != null) { return EQUIP_RESULT.ITEM_ALREADY_EQUIPPED; }

            Container = item as ItemContainer;
            return EQUIP_RESULT.EQUIPPED;
        }
    }
}
