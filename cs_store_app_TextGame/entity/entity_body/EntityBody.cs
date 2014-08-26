using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public class EntityBody
    {
        public List<EntityBodyPart> BodyParts = new List<EntityBodyPart>();

        public Status.EQUIP_STATUS DoEquip(Item itemToEquip)
        {
            Status.EQUIP_STATUS max = Status.EQUIP_STATUS.NOT_EQUIPPABLE;

            foreach(EntityBodyPart part in BodyParts)
            {
                Status.EQUIP_STATUS current = part.DoEquip(itemToEquip);

                if (current == Status.EQUIP_STATUS.EQUIPPED) { return Status.EQUIP_STATUS.EQUIPPED; }
                else if (current > max) { max = current; }
            }

            return max;
        }
    }
}
