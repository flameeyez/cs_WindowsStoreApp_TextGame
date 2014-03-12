using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public class ExitWithDirection
    {
        public Exit Exit { get; set; }
        public string Direction { get; set; }

        public ExitWithDirection(Exit exit, string direction)
        {
            Exit = exit;
            Direction = direction;
        }
    }
}
