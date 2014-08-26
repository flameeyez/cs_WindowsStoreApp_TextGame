using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame.entity
{
    public class EntityHumanoid : EntityBase
    {
        public EntityHumanoid()
        {
            Body.BodyParts.Add(new EntityBodyPartHead());
        }
    }
}
