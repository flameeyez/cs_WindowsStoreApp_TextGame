﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame {
    public enum INPUT_SOURCE {
        PLAYER,
        NPC,
        ROOM
    }
    public enum ACTION_ENUM {
        NONE,
        LOOK,
        MOVE_BASIC,
        MOVE_CONNECTION,
        GET_ITEM,
        DROP_ITEM,
        SHOW_HANDS,
        PUT_ITEM,
        OPEN_CONTAINER,
        CLOSE_CONTAINER,
        EQUIP_ITEM,
        REMOVE_EQUIPMENT,
        SHOW_INVENTORY,
        EAT,
        DRINK,
        BUY_ITEM,
        SELL_ITEM,
        PRICE_ITEM,
        SHOW_GOLD,
        SIT,
        STAND,
        KNEEL,
        ATTACK,
        SHOW_HEALTH,
        SEARCH
    }

    public class ParsedInput {
        public ACTION_ENUM Action { get; set; }
        public string String { get; set; }
        public string[] Words { get; set; }
        public INPUT_SOURCE Source { get; set; }

        public static Dictionary<string, ACTION_ENUM> StringToAction;

        static ParsedInput() {
            StringToAction = new Dictionary<string, ACTION_ENUM>();

            StringToAction.Add("look", ACTION_ENUM.LOOK);
            StringToAction.Add("go", ACTION_ENUM.MOVE_CONNECTION);
            StringToAction.Add("move", ACTION_ENUM.MOVE_CONNECTION);
            StringToAction.Add("get", ACTION_ENUM.GET_ITEM);
            StringToAction.Add("take", ACTION_ENUM.GET_ITEM);
            StringToAction.Add("drop", ACTION_ENUM.DROP_ITEM);
            StringToAction.Add("hands", ACTION_ENUM.SHOW_HANDS);
            StringToAction.Add("put", ACTION_ENUM.PUT_ITEM);
            StringToAction.Add("close", ACTION_ENUM.CLOSE_CONTAINER);
            StringToAction.Add("open", ACTION_ENUM.OPEN_CONTAINER);
            StringToAction.Add("equip", ACTION_ENUM.EQUIP_ITEM);
            StringToAction.Add("wear", ACTION_ENUM.EQUIP_ITEM);
            StringToAction.Add("remove", ACTION_ENUM.REMOVE_EQUIPMENT);
            StringToAction.Add("inventory", ACTION_ENUM.SHOW_INVENTORY);
            StringToAction.Add("eat", ACTION_ENUM.EAT);
            StringToAction.Add("drink", ACTION_ENUM.DRINK);
            StringToAction.Add("sell", ACTION_ENUM.SELL_ITEM);
            StringToAction.Add("price", ACTION_ENUM.PRICE_ITEM);
            StringToAction.Add("buy", ACTION_ENUM.BUY_ITEM);
            StringToAction.Add("health", ACTION_ENUM.SHOW_HEALTH);
            StringToAction.Add("gold", ACTION_ENUM.SHOW_GOLD);
            StringToAction.Add("sit", ACTION_ENUM.SIT);
            StringToAction.Add("stand", ACTION_ENUM.STAND);
            StringToAction.Add("kneel", ACTION_ENUM.KNEEL);
            StringToAction.Add("attack", ACTION_ENUM.ATTACK);
            StringToAction.Add("kill", ACTION_ENUM.ATTACK);
            StringToAction.Add("search", ACTION_ENUM.SEARCH);
        }

        public ParsedInput(INPUT_SOURCE source, string unparsedInput) {
            Source = source;
            Action = ACTION_ENUM.NONE;
            String = unparsedInput;

            char[] delimiters = { ' ' };
            string strInput = unparsedInput.ToLower();
            Words = strInput.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (Words.Length == 0) { return; }

            int nDirection = Statics.DirectionToInt(Words[0]);
            if (nDirection != -1) {
                // hack - replace first word with integer direction
                Words[0] = nDirection.ToString();
                Action = ACTION_ENUM.MOVE_BASIC;
            }
            else {
                ACTION_ENUM action;
                if (StringToAction.TryGetValue(Words[0], out action)) {
                    Action = action;
                }
            }
        }
    }
}
