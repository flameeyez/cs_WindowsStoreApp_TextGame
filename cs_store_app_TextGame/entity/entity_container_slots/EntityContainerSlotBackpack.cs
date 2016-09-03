using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityContainerSlotBackpack", Namespace = "cs_store_app_TextGame")]
    public class EntityContainerSlotBackpack : EntityContainerSlot
    {
        public override ITEM_TYPE Type
        {
            get { return ITEM_TYPE.CONTAINER_BACK; }
        }
    }
}
