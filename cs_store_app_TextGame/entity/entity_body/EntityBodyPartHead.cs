using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public class EntityBodyPartHead : EntityBodyPart 
    {
        public override ITEM_TYPE Type 
        {
            get
            {
                return ITEM_TYPE.ARMOR_HEAD; 
            }
        }
    }
}
