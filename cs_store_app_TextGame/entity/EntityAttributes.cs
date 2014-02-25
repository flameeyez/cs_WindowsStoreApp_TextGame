using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public class EntityAttributes
    {
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Vitality { get; set; }

        public EntityAttributes()
        {
            Strength = 20;
            Intelligence = 20;
            Vitality = 20;
        }
    }
}
