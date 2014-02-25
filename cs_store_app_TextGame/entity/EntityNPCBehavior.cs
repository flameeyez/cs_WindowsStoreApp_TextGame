using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public class EntityNPCBehavior
    {
        public ACTION_ENUM Action { get; set; }
        public int PercentageChance { get; set; }

        public EntityNPCBehavior(ACTION_ENUM action, int percentageChance)
        {
            Action = action;
            PercentageChance = percentageChance;
        }
    }
}
