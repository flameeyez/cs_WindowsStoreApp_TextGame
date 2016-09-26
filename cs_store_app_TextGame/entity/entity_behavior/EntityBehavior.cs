using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityBehavior", Namespace = "cs_store_app_TextGame")]
    public class EntityBehavior
    {
        // TODO: consider sorted list
        [DataMember]
        public List<EntityBehaviorAction> PossibleActions = new List<EntityBehaviorAction>();

        public List<EntityBehaviorAction> GetDesiredBehavior()
        {
            // return a lottery-like list of desired actions
            // this method is called each time an npc is eligible for an action
            // TODO: remember to reset action timer
            // TODO: rename this to EntityNPCBehavior once implemented
            List<EntityBehaviorAction> DesiredActions = new List<EntityBehaviorAction>();
            List<EntityBehaviorAction> PossibleActionsCopy = new List<EntityBehaviorAction>(PossibleActions);

            // run - 50
            // attack - 90
            // idle - 100
            // for this behavior, first check is for run (50%), next check (if run roll fails) is attack (90%), final check is guaranteed (idle, 100%)

            // assuming roll of 80, run check would fail, attack check succeeds, so we'd extract attack from this list
            // next roll is 60, run check fails again, so we extract idle
            // now all that's left is run, so that's our lowest priority action

            // final DesiredAction List is attack, idle, run
            // upon returning, we'd attempt an attack
            // if that fails (no hostiles present), we'd idle, which is guaranteed

            // TODO: consider returning once a guaranteed action is added (possibly run, idle?)
            while(PossibleActionsCopy.Count > 0)
            {
                // last item should always have greatest chance
                int random = Statics.Random.Next(PossibleActionsCopy[PossibleActionsCopy.Count - 1].PercentageChance);

                for (int i = 0; i < PossibleActionsCopy.Count; i++)
                {
                    if (random < PossibleActionsCopy[i].PercentageChance)
                    {
                        DesiredActions.Add(PossibleActionsCopy[i]);
                        PossibleActionsCopy.RemoveAt(i);
                        break;
                    }
                }
            }

            return DesiredActions;
        }
    }
}
