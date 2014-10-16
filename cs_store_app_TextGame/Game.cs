using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public static class Game
    {
        public static EntityPlayer Player;
        public static void Initialize()
        {
            Player = new EntityPlayer();
            Player.Level = 1;
        }
    }
}
