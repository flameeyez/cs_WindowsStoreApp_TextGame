using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;
using System.IO;
using System.Xml.Linq;
using Windows.UI.Popups;

namespace cs_store_app_TextGame
{
    public enum MESSAGE_ENUM
    {
        DEBUG_TODO,


        NO_MESSAGE,
        BASE_STRING,

        // PLAYER
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
        PLAYER_EQUIP_ARMOR_CHEST,
        PLAYER_EQUIP_ARMOR_HEAD,
        PLAYER_EQUIP_ARMOR_FEET,
        PLAYER_EQUIP_ARMOR_FINGER,
        PLAYER_EQUIP_ARMOR_NECK,
        PLAYER_EQUIP_CONTAINER_BACKPACK,
        PLAYER_EQUIP_CONTAINER_POUCH,
        PLAYER_REMOVE,
        PLAYER_REMOVE_ARMOR_CHEST,
        PLAYER_REMOVE_ARMOR_HEAD,
        PLAYER_REMOVE_ARMOR_FEET,
        PLAYER_REMOVE_ARMOR_FINGER,
        PLAYER_REMOVE_ARMOR_NECK,
        PLAYER_REMOVE_CONTAINER_BACKPACK,
        PLAYER_REMOVE_CONTAINER_POUCH,
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
        PLAYER_ATTACKS_NPC,
        PLAYER_KILLS_NPC,
        PLAYER_SHOW_HEALTH,


        // NPC
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
        NPC_LEAVES_CONNECTION,
        NPC_LOOK,
        NPC_SHOW_ITEM,
        NPC_ATTACKS_PLAYER,
        NPC_KILLS_PLAYER,
        NPC_ATTACKS_NPC,
        NPC_KILLS_NPC,
        NPC_ATTACKS_DEAD_NPC,
        NPC_ATTACKS_DEAD_PLAYER,
        NPC_SEARCH_WITH_GOLD,
        NPC_SEARCH_NO_GOLD,


        // ERROR MESSAGES
        ERROR_PLAYER_IS_DEAD,
        ERROR_PLAYER_CONTAINER_CLOSED,
        ERROR_BAD_INPUT,
        ERROR_WRONG_DIRECTION,
        ERROR_GO_WHERE,
        ERROR_BAD_ITEM,
        ERROR_HANDS_ARE_FULL,
        ERROR_NOT_CARRYING_ITEM,
        ERROR_CONTAINER_CLOSED,
        ERROR_CONTAINER_ALREADY_OPEN,
        ERROR_CONTAINER_ALREADY_CLOSED,
        ERROR_WHAT,
        ERROR_CONTAINER_NOT_CLOSABLE,
        ERROR_ITEM_NOT_EQUIPPABLE,
        ERROR_ALREADY_EQUIPPED,
        ERROR_NOT_A_SHOP,
        ERROR_BAD_SHOP,
        ERROR_NOT_ENOUGH_GOLD,
        ERROR_ALREADY_STANDING,
        ERROR_ALREADY_SITTING,
        ERROR_ALREADY_KNEELING,
        ERROR_SITTING,
        ERROR_KNEELING,
        ERROR_NPC_HANDS_ARE_FULL,
        ERROR_NPC_NO_ITEMS_IN_ROOM,
        ERROR_NPC_NOT_DEAD,
        ERROR_NPC_ALREADY_DEAD,
        ERROR_NEED_TO_IMPLEMENT,

        ERROR_NPC_ATTACKS_NPC_BAD_WEAPON,
        ERROR_NPC_ATTACKS_PLAYER_BAD_WEAPON,
        ERROR_PLAYER_ATTACKS_BAD_WEAPON,

        ERROR_BODY_PART_MISSING,

        // DEBUG
        DEBUG_REMOVE,
        PLAYER_CARRYING_NO_GOLD,
        DEBUG_ROOM_CLEANUP
    };
    public static class Messages
    {
        private static Dictionary<MESSAGE_ENUM, List<string>> MessageDictionary = new Dictionary<MESSAGE_ENUM, List<string>>();
        private static Dictionary<string, MESSAGE_ENUM> StringToMessageEnum = new Dictionary<string, MESSAGE_ENUM>();

        public static async Task Load()
        {
            await LoadMessagesFromXML();
        }

        private static async Task LoadMessagesFromXML()
        {
            foreach (MESSAGE_ENUM message in Enum.GetValues(typeof(MESSAGE_ENUM)))
            {
                MessageDictionary.Add(message, new List<string>());
            }

            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\messages");
            var file = await folder.GetFileAsync("messages.xml");
            var stream = await file.OpenStreamForReadAsync();

            XDocument messagesDocument = XDocument.Load(stream);
            await stream.FlushAsync();

            var messageNodes = from messages in messagesDocument
                                    .Elements("messages")
                                    .Elements("message")
                                select messages;
            foreach (var messageNode in messageNodes)
            {
                MESSAGE_ENUM message = (MESSAGE_ENUM)(Enum.Parse(typeof(MESSAGE_ENUM), messageNode.Element("id").Value));
                List<string> messageList = MessageDictionary[message];

                var stringNodes = from strings in messageNode
                                        .Elements("strings")
                                        .Elements("string")
                                    select strings;

                foreach (var stringNode in stringNodes)
                {
                    // XML parsing treats \n as literal text
                    messageList.Add(stringNode.Value.Replace("\\n", "\n"));
                }
            }

            // DEBUG
            MessageDictionary[MESSAGE_ENUM.DEBUG_TODO].Add("DEBUG: On the TODO list!");
            // END DEBUG
        }
        private static Paragraph Process(string strMessage, Paragraph p1 = null, Paragraph p2 = null, Paragraph p3 = null, Paragraph p4 = null)
        {
            strMessage += "\n";
            Paragraph p = new Paragraph();

            // TODO: somewhat arbitrary number of parameters (up to 9?)
            int currentParameterIndex = strMessage.IndexOfAny("1234".ToCharArray());

            if (currentParameterIndex == -1) { return strMessage.ToParagraph(); }
            while (currentParameterIndex != -1)
            {
                if (currentParameterIndex > 0) { p.Inlines.Add(strMessage.Substring(0, currentParameterIndex).ToRun()); }

                switch (strMessage[currentParameterIndex])
                {
                    case '1': p.Merge(p1.Clone()); break;
                    case '2': p.Merge(p2.Clone()); break;
                    case '3': p.Merge(p3.Clone()); break;
                    case '4': p.Merge(p4.Clone()); break;
                    default: break;
                }

                strMessage = strMessage.Substring(currentParameterIndex + 1);
                currentParameterIndex = strMessage.IndexOfAny("1234".ToCharArray());
            }

            p.Inlines.Add(strMessage.ToRun());

            return p;
        }
        public static Paragraph Get(MESSAGE_ENUM message, Paragraph p1 = null, Paragraph p2 = null, Paragraph p3 = null, Paragraph p4 = null)
        {
            if (message == MESSAGE_ENUM.NO_MESSAGE) { return null; }
            return Process(MessageDictionary[message].RandomListItem(), p1, p2, p3, p4);
        }

        public static void Display(MESSAGE_ENUM message, Paragraph p1 = null, Paragraph p2 = null, Paragraph p3 = null, Paragraph p4 = null)
        {
            MessageQueue.Enqueue(Messages.Get(message, p1, p2, p3, p4));
        }
    }
}
