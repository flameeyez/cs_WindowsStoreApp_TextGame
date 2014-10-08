using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityBehaviorAction", Namespace = "cs_store_app_TextGame")]
    public class EntityBehaviorAction
    {
        [DataMember]
        public ACTION_ENUM Action { get; set; }
        [DataMember]
        public int PercentageChance { get; set; }

        public EntityBehaviorAction(ACTION_ENUM action, int percentageChance)
        {
            Action = action;
            PercentageChance = percentageChance;
        }
    }
}
