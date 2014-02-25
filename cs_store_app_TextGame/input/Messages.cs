using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public enum MESSAGE_ENUM
    {
        BASE_STRING,
        PLAYER_GET,
        PLAYER_DROP,
        PLAYER_PUT_IN_PLAYER_CONTAINER,
        PLAYER_PUT_IN_GROUND_CONTAINER,
        PLAYER_EAT,
        PLAYER_DRINK,
        PLAYER_CLOSE,
        PLAYER_OPEN,
        PLAYER_GET_FROM_ROOM_CONTAINER,
        PLAYER_GET_FROM_CONTAINER,
        PLAYER_EQUIP,
        PLAYER_EQUIP_BACKPACK,
        PLAYER_REMOVE,
        PLAYER_EAT_LAST,
        PLAYER_DRINK_LAST,
        PLAYER_EAT_GROUND_ITEM,
        PLAYER_EAT_LAST_GROUND_ITEM,
        PLAYER_DRINK_GROUND_ITEM,
        PLAYER_DRINK_LAST_GROUND_ITEM,
        PLAYER_SELL_ITEM,
        PLAYER_BUY,
        PLAYER_PRICE_ITEM,
        PLAYER_CARRYING_GOLD,
        PLAYER_STAND,
        PLAYER_SIT,
        PLAYER_KNEEL,
        NPC_GET,
        NPC_DROP,
        NPC_PUT_IN_NPC_CONTAINER,
        NPC_PUT_IN_GROUND_CONTAINER,
        NPC_EAT,
        NPC_DRINK,
        NPC_CLOSE,
        NPC_OPEN,
        NPC_GET_FROM_ROOM_CONTAINER,
        NPC_GET_FROM_CONTAINER,
        NPC_EQUIP,
        NPC_EQUIP_BACKPACK,
        NPC_REMOVE,
        NPC_EAT_LAST,
        NPC_DRINK_LAST,
        NPC_EAT_GROUND_ITEM,
        NPC_EAT_LAST_GROUND_ITEM,
        NPC_DRINK_GROUND_ITEM,
        NPC_DRINK_LAST_GROUND_ITEM,
        NPC_SELL_ITEM,
        NPC_BUY,
        NPC_PRICE_ITEM,
        NPC_CARRYING_GOLD,
        NPC_ARRIVES,
        NPC_LEAVES,
        PLAYER_ATTACKS_NPC,
        PLAYER_KILLS_NPC,
        DEBUG_REMOVE,
        NPC_LOOK
    };
    public enum ERROR_MESSAGE_ENUM
    {
        OK = -1,
        BAD_INPUT,
        WRONG_DIRECTION,
        GO_WHERE,
        BAD_ITEM,
        HANDS_ARE_FULL,
        NOT_CARRYING_ITEM,
        CONTAINER_CLOSED,
        CONTAINER_ALREADY_OPEN,
        CONTAINER_ALREADY_CLOSED,
        WHAT,
        CONTAINER_NOT_CLOSABLE,
        ITEM_NOT_EQUIPPABLE,
        ALREADY_EQUIPPED,
        NOT_A_SHOP,
        BAD_SHOP,
        NOT_ENOUGH_GOLD,
        ALREADY_STANDING,
        ALREADY_SITTING,
        ALREADY_KNEELING,
        SITTING,
        KNEELING,
        NOT_A_WEAPON
    };
    public static class Messages
    {
        private static Dictionary<MESSAGE_ENUM, string> MessageDictionary = new Dictionary<MESSAGE_ENUM, string>();
        private static Dictionary<ERROR_MESSAGE_ENUM, string> ErrorMessageDictionary = new Dictionary<ERROR_MESSAGE_ENUM, string>();

        static Messages()
        {
            MessageDictionary.Add(MESSAGE_ENUM.BASE_STRING, "/1");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_GET, "You pick up /an /1.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_DROP, "You drop /an /1.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_EAT, "You take a bite of your /1. You have /2 bite/s left.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_EAT_LAST, "You take a bite of the /1. That was the last of it.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_EAT_GROUND_ITEM, "You take a bite of the /1 that is lying on the ground. There are /2 bite/s left.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_EAT_LAST_GROUND_ITEM, "You take a bite of the /1 that is lying on the ground. That was the last of it.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_DRINK, "You take a sip from your /1. You have /2 sip/s left.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_DRINK_LAST, "You take a sip from the /1. That was the last of it.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_DRINK_GROUND_ITEM, "You take a sip from the /1 that is lying on the ground. There are /2 sip/s left.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_DRINK_LAST_GROUND_ITEM, "You take a sip from the /1 that is lying on the ground. That was the last of it.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_OPEN, "You open the /1.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_CLOSE, "You close the /1.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_GET_FROM_CONTAINER, "You remove /an /1 from the /2.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_GET_FROM_ROOM_CONTAINER, "You remove /an /1 from /an /2 that is lying on the ground.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_PUT_IN_PLAYER_CONTAINER, "You put /an /1 in your /2.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_PUT_IN_GROUND_CONTAINER, "You put /an /1 in /an /2 that is lying on the ground.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_EQUIP, "You equip the /1.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_EQUIP_BACKPACK, "You toss /an /1 over your shoulders.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_REMOVE, "You remove your /1.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_BUY, "You buy /an /1 from the shopkeeper for /2 gold pieces.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_PRICE_ITEM, "The shopkeeper carefully examines the /1. 'I'll offer /2 gold pieces for it,' he says.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_CARRYING_GOLD, "You are carrying /1 gold pieces.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_STAND, "You stand up.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_SIT, "You sit down.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_KNEEL, "You kneel.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_ATTACKS_NPC, "You attack the /1 with your /2 and hit for /3 damage.");
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_KILLS_NPC, "You attack the /1 with your /2 and hit for /3 damage.\nThe /1 dies.");
            MessageDictionary.Add(MESSAGE_ENUM.DEBUG_REMOVE, "DEBUG: Removing /1.");
            
            // TODO: can anything be sold for 1 gold piece?
            MessageDictionary.Add(MESSAGE_ENUM.PLAYER_SELL_ITEM, "You sell the /1 for /2 gold pieces.");

            MessageDictionary.Add(MESSAGE_ENUM.NPC_ARRIVES, "/An /1 appears!");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_LEAVES, "The /1 heads /2.");

            MessageDictionary.Add(MESSAGE_ENUM.NPC_GET, "The /1 picks up /an /2.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_DROP, "The /1 drops /an /2.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_EAT, "The /1 takes a bite of its /2.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_EAT_LAST, "The /1 shoves the last bit of the /1 into its mouth.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_EAT_GROUND_ITEM, "The /1 takes a bite of the /2 that is lying on the ground.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_EAT_LAST_GROUND_ITEM, "The /1 eats the last bit of the /2 that is lying on the ground.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_DRINK, "The /1 takes a sip from its /2.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_DRINK_LAST, "The /1 takes the last sip from its /2.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_DRINK_GROUND_ITEM, "The /1 takes a sip from the /2 that is lying on the ground.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_DRINK_LAST_GROUND_ITEM, "The /1 takes the last sip from the /2 that is lying on the ground.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_OPEN, "The /1 opens /an /2.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_CLOSE, "The /1 closes /an /2.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_GET_FROM_CONTAINER, "The /1 removes /an /2 from /an /3.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_GET_FROM_ROOM_CONTAINER, "The /1 removes /an /2 from /an /3 that is lying on the ground.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_PUT_IN_NPC_CONTAINER, "The /1 puts /an /2 in its /3.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_PUT_IN_GROUND_CONTAINER, "The /1 puts /an /2 in /an /3 that is lying on the ground.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_EQUIP, "The /1 equips /an /2.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_EQUIP_BACKPACK, "The /1 tosses /an /2 over its shoulders.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_REMOVE, "The /1 removes /an /2.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_BUY, "The /1 buys /an /2 from the shopkeeper.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_PRICE_ITEM, "The shopkeeper converses with the /1.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_CARRYING_GOLD, "The /1 checks its pockets for gold.");
            MessageDictionary.Add(MESSAGE_ENUM.NPC_LOOK, "The /1 checks its surroundings.");

            // TODO: can anything be sold for 1 gold piece?
            MessageDictionary.Add(MESSAGE_ENUM.NPC_SELL_ITEM, "The /1 sells /an /2 to the shopkeeper.");

            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.BAD_INPUT, "I don't understand what you've typed.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.WRONG_DIRECTION, "You can't go in that direction.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.GO_WHERE, "Where would you like to go?");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.BAD_ITEM, "I can't find that item.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.HANDS_ARE_FULL, "Your hands are full.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.NOT_CARRYING_ITEM, "You aren't carrying that item.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.CONTAINER_CLOSED, "The /1 is closed.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.CONTAINER_ALREADY_OPEN, "The /1 is already open.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.CONTAINER_ALREADY_CLOSED, "The /1 is already closed.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.WHAT, "/1 what?");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.CONTAINER_NOT_CLOSABLE , "The /1 isn't closable.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.ITEM_NOT_EQUIPPABLE , "You can't equip that.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.ALREADY_EQUIPPED, "You can't equip another item of that type.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.NOT_A_SHOP, "You don't see a shop around here.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.BAD_SHOP, "This store won't buy that type of item.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.NOT_ENOUGH_GOLD, "You can't afford the /1.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.ALREADY_STANDING, "You are already standing.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.ALREADY_SITTING, "You are already sitting.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.ALREADY_KNEELING, "You are already kneeling.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.SITTING, "You can't move while sitting.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.KNEELING, "You can't move while kneeling.");
            ErrorMessageDictionary.Add(ERROR_MESSAGE_ENUM.NOT_A_WEAPON, "You can't attack with /an /1!");
        }

        private static string ProcessMessage(string strMessage, string strParameter1, string strParameter2, string strParameter3)
        {
            string strReturn = strMessage;
            strReturn = strReturn.Replace("/1", strParameter1);
            strReturn = strReturn.Replace("/2", strParameter2);
            strReturn = strReturn.Replace("/3", strParameter3);

            // /an - get first consonant after next space
            int nIndex = strReturn.IndexOf("/an");
            while (nIndex != -1)
            {
                int nNextCharIndex = strReturn.IndexOf(' ', nIndex) + 1;
                strReturn = strReturn.Substring(0, nIndex) + (strReturn[nNextCharIndex].IsVowel() ? "an" : "a") + strReturn.Substring(nIndex + "/an".Length);
                nIndex = strReturn.IndexOf("/an");
            }

            // /An - capitalized version of the above
            nIndex = strReturn.IndexOf("/An");
            while (nIndex != -1)
            {
                int nNextCharIndex = strReturn.IndexOf(' ', nIndex) + 1;
                strReturn = strReturn.Substring(0, nIndex) + (strReturn[nNextCharIndex].IsVowel() ? "An" : "A") + strReturn.Substring(nIndex + "/An".Length);
                nIndex = strReturn.IndexOf("/An");
            }

            // numbers
            // HUGE assumption - assume preceding token has been replaced with a number
            nIndex = strReturn.IndexOf("/s");
            while (nIndex != -1)
            {
                int nLastNumberChar = strReturn.LastIndexOf(' ', nIndex) - 1;
                int nFirstNumberChar = strReturn.LastIndexOf(' ', nLastNumberChar) + 1;

                string strNumber = strReturn.Substring(nFirstNumberChar, nLastNumberChar - nFirstNumberChar + 1);
                strReturn = strReturn.Substring(0, nIndex) + (strNumber == "1" ? "" : "s") + strReturn.Substring(nIndex + "/s".Length);
                nIndex = strReturn.IndexOf("/s");
            }

            return strReturn;
        }
        public static string GetMessage(MESSAGE_ENUM message, string strParameter1 = "", string strParameter2 = "", string strParameter3 = "")
        {
            return ProcessMessage(MessageDictionary[message], strParameter1, strParameter2, strParameter3) + "\n";
        }
        public static string GetErrorMessage(ERROR_MESSAGE_ENUM errorMessage, string strParameter1 = "", string strParameter2 = "", string strParameter3 = "")
        {
            return ProcessMessage(ErrorMessageDictionary[errorMessage], strParameter1, strParameter2, strParameter3) + "\n";
        }
    }
}
