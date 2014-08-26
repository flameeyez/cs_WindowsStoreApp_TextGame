using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public static class Status
    {
        // NOT_EQUIPPABLE == no valid slots
        // BODY_PART_MISSING == at least one valid slot, all missing
        // ITEM_ALREADY_EQUIPPED == at least one valid slot, at least one not missing, all non-missing already have items
        // EQUIPPED == valid slot, equipped
        public enum EQUIP_STATUS
        {
            NOT_EQUIPPABLE = 0,
            BODY_PART_MISSING = 1,
            ITEM_ALREADY_EQUIPPED = 2,
            EQUIPPED = 3
        }
    }
}
