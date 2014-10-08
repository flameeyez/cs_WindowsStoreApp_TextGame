using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public enum EQUIP_RESULT
    {
        NOT_EQUIPPABLE = 0,
        BODY_PART_MISSING = 1,
        ITEM_ALREADY_EQUIPPED = 2,
        EQUIPPED = 3
    }

    public enum HANDLED_RESULT
    {
        HANDLED,
        UNHANDLED
    }
}
