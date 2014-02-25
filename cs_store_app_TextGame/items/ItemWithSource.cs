using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public enum ITEM_SOURCE
    {
        NOT_FOUND = -1,
        PLAYER_BACKPACK = 0,
        PLAYER_RIGHT_HAND = 1,
        PLAYER_LEFT_HAND = 2,
        ROOM = 3
    }
    public class ItemWithSource
    {
        public ITEM_SOURCE Source { get; set; }
        public Item Item { get; set; }

        public ItemWithSource(ITEM_SOURCE source, Item item)
        {
            Source = source;
            Item = item;
        }
    }
}
