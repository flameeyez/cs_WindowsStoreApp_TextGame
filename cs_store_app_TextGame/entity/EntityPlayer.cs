using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public class EntityPlayer : Entity
    {
        public EntityPlayer() : base() { SetCurrentRoom(0, 0, 0); }

        public string CurrentRoomDisplayString
        {
            get
            {
                string strReturn = "[" + CurrentRegion.Name + " - " + CurrentSubregion.Name + "]\n";
                strReturn += CurrentRoom.FullDisplayString;
                return strReturn;
            }
        }
        public override string HandsString
        {
            get
            {
                if (LeftHand == null && RightHand == null)
                {
                    return "Your hands are empty.";
                }

                string strReturn = "";
                if (RightHand == null) { strReturn += "Your right hand is empty and "; }
                else
                {
                    strReturn = "You are holding a";
                    if (RightHand.Name[0].IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " " + RightHand.Name;
                    strReturn += " in your right hand";
                }

                if (LeftHand == null) { strReturn += "."; }
                else
                {
                    if (RightHand == null)
                    {
                        strReturn = "You are holding a";
                    }
                    else
                    {
                        strReturn += " and a";
                    }

                    if (LeftHand.Name[0].IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " " + LeftHand.Name;
                    strReturn += " in your left hand.";
                }

                return strReturn;
            }
        }
        public override string InventoryString
        {
            get
            {
                List<string> inventory = new List<string>();

                if (Backpack != null) { inventory.Add("a" + (Backpack.Name[0].IsVowel() ? "n" : "") + " " + Backpack.Name); }
                if (ArmorChest != null) { inventory.Add("a" + (ArmorChest.Name[0].IsVowel() ? "n" : "") + " " + ArmorChest.Name); }
                if (ArmorFeet != null) { inventory.Add("a" + (ArmorFeet.Name[0].IsVowel() ? "n" : "") + " " + ArmorFeet.Name); }
                if (ArmorHead != null) { inventory.Add("a" + (ArmorHead.Name[0].IsVowel() ? "n" : "") + " " + ArmorHead.Name); }
                if (Ring1 != null) { inventory.Add("a" + (Ring1.Name[0].IsVowel() ? "n" : "") + " " + Ring1.Name); }
                if (Ring2 != null) { inventory.Add("a" + (Ring2.Name[0].IsVowel() ? "n" : "") + " " + Ring2.Name); }
                if (Amulet != null) { inventory.Add("a" + (Amulet.Name[0].IsVowel() ? "n" : "") + " " + Amulet.Name); }

                string strReturn = "You are wearing ";

                switch (inventory.Count)
                {
                    case 0:
                        return "You aren't wearing anything!\n";
                    case 1:
                        strReturn += inventory[0];
                        break;
                    case 2:
                        strReturn += inventory[0] + " and " + inventory[1];
                        break;
                    default:
                        for (int i = 0; i < inventory.Count - 1; i++)
                        {
                            strReturn += inventory[i] + ", ";
                        }

                        strReturn += "and " + inventory[inventory.Count - 1];
                        break;
                }

                strReturn += ".\n";
                return strReturn;
            }
        }

        #region Input Handling

        public override Handler DoMoveBasic(TranslatedInput input)
        {
            if (Posture == ENTITY_POSTURE.SITTING) { return Handler.SITTING; }
            if (Posture == ENTITY_POSTURE.KNEELING) { return Handler.KNEELING; }

            // basic direction
            // if "go <direction>"
            int nDirection;
            if (input.Words[0] == "go" || input.Words[0] == "move") { nDirection = StaticMethods.DirectionToInt(input.Words[1]); }
            // see TranslatedInput constructor; hack job replaces first word with integer direction
            else { nDirection = int.Parse(input.Words[0]); }

            Exit exit = CurrentRoom.Exits[nDirection];
            if (exit.Region == -1) { return Handler.WRONG_DIRECTION; }

            SetCurrentRoom(exit.Region, exit.Subregion, exit.Room);
            return new Handler(RETURN_CODE.HANDLED, CurrentRoomDisplayString);
        }
        public override Handler DoMoveConnection(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return Handler.GO_WHERE; }

            if (Posture == ENTITY_POSTURE.SITTING) { return Handler.SITTING; }
            if (Posture == ENTITY_POSTURE.KNEELING) { return Handler.KNEELING; }

            // go <direction>
            if (StaticMethods.DirectionToInt(input.Words[1]) != -1) { return DoMoveBasic(input); }

            // go <connection>
            Connection connection = CurrentRoom.GetConnection(input.Words[0], input.Words[1]);
            if (connection != null)
            {
                SetCurrentRoom(connection.DestinationRegion, connection.DestinationSubregion, connection.DestinationRoom);
                input = null;
                return DoLook(input);
            }

            return Handler.BAD_INPUT;
        }
        public override Handler DoLook(TranslatedInput input)
        {
            if (input == null || input.Words.Length < 2) { return new Handler(RETURN_CODE.HANDLED, CurrentRoomDisplayString); }
            else if (input.Words.Length == 2) { return DoLook(input.Words[1]); }
            else if (input.Words.Length == 3) { return DoLook(input.Words[1], input.Words[2]); }
            return Handler.BAD_INPUT;
        }
        public override Handler DoLook(string strWord)
        {
            if (strWord == "hands") { return DoLookHands(null); }
                
            Item item = GetItemFromHand(strWord, false);
            if (item != null) { return new Handler(RETURN_CODE.HANDLED, item.Description + "\n"); }

            item = CurrentRoom.Items.Get(strWord);
            if (item != null) { return new Handler(RETURN_CODE.HANDLED, item.Description + "\n"); }

            EntityNPC npc = CurrentRoom.FindNPC(strWord);
            if (npc != null) { return new Handler(RETURN_CODE.HANDLED, npc.HandsString + "\n" + npc.InventoryString); }

            return Handler.BAD_INPUT;
        }
        public override Handler DoLook(string strWord1, string strWord2)
        {
            if (strWord1 == "at") { return DoLook(strWord2); }
            else if (strWord1 == "in") { return DoLookInContainer(strWord2); }
            else { return Handler.BAD_INPUT; }
        }
        public override Handler DoLookHands(TranslatedInput input)
        {
            return new Handler(RETURN_CODE.HANDLED, HandsString + "\n");
        }
        public override Handler DoLookInContainer(string strKeyword)
        {
            ItemContainer container = null;

            if (Backpack != null && Backpack.IsKeyword(strKeyword)) { container = Backpack; }
            else { container = GetItemFromHand(strKeyword, false, ITEM_TYPE.CONTAINER) as ItemContainer; }
            if (container == null) { container = CurrentRoom.Items.Get(strKeyword, ITEM_TYPE.CONTAINER) as ItemContainer; }
            if (container == null) { return Handler.BAD_INPUT; }

            return new Handler(RETURN_CODE.HANDLED, container.ItemsString + "\n");
        }
        public override Handler DoEat(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.WHAT, "Eat")); }
            else if (input.Words.Length > 2) { return Handler.BAD_INPUT; }

            ItemFood itemToEat = null;
            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_EAT;

            // TODO: rework priority return codes
            // get and store return code from right hand
            // if not a clean match, get and store return code from left hand
            // if still not a clean match, get and store return code from room
            // if still not a clean match, get the CLOSEST match return code
            // - if we matched keyword but not item type, we want that (can't eat)
            // - otherwise, we have a bad item (don't care about matched type if we didn't match keyword)

            if (RightHand != null && RightHand.Type == ITEM_TYPE.FOOD && RightHand.IsKeyword(input.Words[1])) { itemToEat = RightHand as ItemFood; }
            else if (LeftHand != null && LeftHand.Type == ITEM_TYPE.FOOD && LeftHand.IsKeyword(input.Words[1])) { itemToEat = LeftHand as ItemFood; }
            else
            {
                itemToEat = CurrentRoom.FindItem(input.Words[1], ITEM_TYPE.FOOD) as ItemFood;
                message = MESSAGE_ENUM.PLAYER_EAT_GROUND_ITEM;
            }

            if (itemToEat == null) { return Handler.BAD_INPUT; }

            itemToEat.NumberOfBites--;
            if (itemToEat.NumberOfBites == 0)
            {
                switch (message)
                {
                    case MESSAGE_ENUM.PLAYER_EAT:
                        message = MESSAGE_ENUM.PLAYER_EAT_LAST;
                        break;
                    case MESSAGE_ENUM.PLAYER_EAT_GROUND_ITEM:
                        message = MESSAGE_ENUM.PLAYER_EAT_LAST_GROUND_ITEM;
                        break;
                }
            }

            if (itemToEat.NumberOfBites == 0)
            {
                if (itemToEat.Equals(RightHand)) { RightHand = null; }
                else if (itemToEat.Equals(LeftHand)) { LeftHand = null; }
                else { CurrentRoom.RemoveItem(itemToEat); }
            }

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, itemToEat.Name, itemToEat.NumberOfBites.ToString()));
        }
        public override Handler DoDrink(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.WHAT, "Drink")); }
            else if (input.Words.Length > 2) { return Handler.BAD_INPUT; }

            ItemDrink itemToDrink = null;
            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_DRINK;

            if (RightHand != null && RightHand.Type == ITEM_TYPE.DRINK && RightHand.IsKeyword(input.Words[1])) { itemToDrink = RightHand as ItemDrink; }
            else if (LeftHand != null && LeftHand.Type == ITEM_TYPE.DRINK && LeftHand.IsKeyword(input.Words[1])) { itemToDrink = LeftHand as ItemDrink; }
            else
            {
                itemToDrink = CurrentRoom.FindItem(input.Words[1], ITEM_TYPE.DRINK) as ItemDrink;
                message = MESSAGE_ENUM.PLAYER_DRINK_GROUND_ITEM;
            }

            if (itemToDrink == null) { return Handler.BAD_INPUT; }

            itemToDrink.NumberOfDrinks--;
            if (itemToDrink.NumberOfDrinks == 0)
            {
                switch (message)
                {
                    case MESSAGE_ENUM.PLAYER_DRINK:
                        message = MESSAGE_ENUM.PLAYER_DRINK_LAST;
                        break;
                    case MESSAGE_ENUM.PLAYER_DRINK_GROUND_ITEM:
                        message = MESSAGE_ENUM.PLAYER_DRINK_LAST_GROUND_ITEM;
                        break;
                }
            }

            if (itemToDrink.NumberOfDrinks == 0)
            {
                if (itemToDrink.Equals(RightHand)) { RightHand = null; }
                else if (itemToDrink.Equals(LeftHand)) { LeftHand = null; }
                else { CurrentRoom.RemoveItem(itemToDrink); }
            }

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, itemToDrink.Name, itemToDrink.NumberOfDrinks.ToString()));
        }
        public override Handler DoOpen(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.WHAT, "Open")); }

            ItemContainer container = null;

            if (Backpack != null) { container = Backpack; }
            else if (RightHand.Type == ITEM_TYPE.CONTAINER) { container = RightHand as ItemContainer; }
            else if (LeftHand.Type == ITEM_TYPE.CONTAINER) { container = LeftHand as ItemContainer; }
            else { container = CurrentRoom.FindItem(input.Words[1], ITEM_TYPE.CONTAINER) as ItemContainer; }

            if (container == null) { return Handler.BAD_ITEM; }

            if (!container.Closed) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.CONTAINER_ALREADY_OPEN, container.Name)); }

            container.Closed = false;
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_OPEN, container.Name));
        }
        public override Handler DoClose(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.WHAT, "Close")); }

            ItemContainer container = null;

            if (Backpack != null) { container = Backpack; }
            else if (RightHand.Type == ITEM_TYPE.CONTAINER) { container = RightHand as ItemContainer; }
            else if (LeftHand.Type == ITEM_TYPE.CONTAINER) { container = LeftHand as ItemContainer; }
            else { container = CurrentRoom.FindItem(input.Words[1], ITEM_TYPE.CONTAINER) as ItemContainer; }

            if (container == null) { return Handler.BAD_ITEM; }

            if (container.Closed) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.CONTAINER_ALREADY_CLOSED, container.Name)); }
            if (!container.Closable) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.CONTAINER_NOT_CLOSABLE, container.Name)); }

            container.Closed = true;
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_CLOSE, container.Name));
        }
        public override Handler DoPut(TranslatedInput input)
        {
            // put <item> in <container> - TODO: <on surface>
            if (input.Words.Length != 4) { return Handler.BAD_INPUT; }
            if (input.Words[2] != "in") { return Handler.BAD_INPUT; }

            // must be holding item
            // don't remove item here
            Item item = GetItemFromHand(input.Words[1], false);
            if (item == null) { return Handler.NOT_CARRYING_ITEM; }

            ItemContainer container = null;
            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_PUT_IN_PLAYER_CONTAINER;

            // make sure that we don't accidentally try to put a container into itself
            if (Backpack != null && Backpack.IsKeyword(input.Words[3]) && !(Backpack.Equals(item))) { container = Backpack; }
            else if (RightHand != null && RightHand.IsKeyword(input.Words[3]) && !(RightHand.Equals(item))) { container = RightHand as ItemContainer; }
            else if (LeftHand != null && LeftHand.IsKeyword(input.Words[3]) && !(LeftHand.Equals(item))) { container = LeftHand as ItemContainer; }
            else
            {
                container = CurrentRoom.FindItem(input.Words[3], ITEM_TYPE.CONTAINER) as ItemContainer;
                if (container != null && container.Equals(item)) { container = null; }
                else { message = MESSAGE_ENUM.PLAYER_PUT_IN_GROUND_CONTAINER; }
            }

            if (container == null) { return Handler.BAD_INPUT; }
            if (container.Closed) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.CONTAINER_CLOSED, container.Name)); }

            item = GetItemFromHand(input.Words[1], true);
            container.Items.Add(item);

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, item.Name, container.Name));
        }
        public override Handler DoEquip(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.WHAT, "Equip")); }
            if (input.Words.Length != 2) { return Handler.BAD_INPUT; }

            Item itemToEquip = null;

            if (RightHand != null && RightHand.IsKeyword(input.Words[1])) { itemToEquip = RightHand; }
            else if (LeftHand != null && LeftHand.IsKeyword(input.Words[1])) { itemToEquip = LeftHand; }

            if (itemToEquip == null) { return Handler.BAD_INPUT; }

            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_EQUIP;

            switch (itemToEquip.Type)
            {
                case ITEM_TYPE.ARMOR_CHEST:
                    if (ArmorChest != null) { return Handler.ALREADY_EQUIPPED; }
                    ArmorChest = itemToEquip as ItemArmorChest;
                    DefensePower += ArmorChest.ArmorFactor;
                    break;
                case ITEM_TYPE.ARMOR_FEET:
                    if (ArmorFeet != null) { return Handler.ALREADY_EQUIPPED; }
                    ArmorFeet = itemToEquip as ItemArmorFeet;
                    DefensePower += ArmorFeet.ArmorFactor;
                    break;
                case ITEM_TYPE.ARMOR_HEAD:
                    if (ArmorHead != null) { return Handler.ALREADY_EQUIPPED; }
                    ArmorHead = itemToEquip as ItemArmorHead;
                    DefensePower += ArmorHead.ArmorFactor;
                    break;
                case ITEM_TYPE.CONTAINER:
                    if (Backpack != null) { return Handler.ALREADY_EQUIPPED; }
                    Backpack = itemToEquip as ItemContainer;
                    message = MESSAGE_ENUM.PLAYER_EQUIP_BACKPACK;
                    break;
                case ITEM_TYPE.ACCESSORY_AMULET:
                    if (Amulet != null) { return Handler.ALREADY_EQUIPPED; }
                    Amulet = itemToEquip as ItemAccessoryAmulet;
                    break;
                case ITEM_TYPE.ACCESSORY_RING:
                    if (Ring1 != null && Ring2 != null) { return Handler.ALREADY_EQUIPPED; }
                    if (Ring1 == null) { Ring1 = itemToEquip as ItemAccessoryRing; }
                    else if (Ring2 == null) { Ring2 = itemToEquip as ItemAccessoryRing; }
                    break;
                default:
                    return Handler.ITEM_NOT_EQUIPPABLE;
            }

            if (itemToEquip.Equals(RightHand)) { RightHand = null; }
            else if (itemToEquip.Equals(LeftHand)) { LeftHand = null; }

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, itemToEquip.Name));
        }
        public override Handler DoRemove(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.WHAT, "Remove")); }
            if (input.Words.Length > 2) { return Handler.BAD_INPUT; }

            Handler handler = Handler.BAD_INPUT;
            Item removedItem = null;

            if (Amulet != null && Amulet.IsKeyword(input.Words[1]))
            {
                removedItem = Amulet;
                handler = DoRemove(ITEM_SLOT.AMULET);
            }
            else if (ArmorChest != null && ArmorChest.IsKeyword(input.Words[1]))
            {
                removedItem = ArmorChest;
                handler = DoRemove(ITEM_SLOT.ARMOR_CHEST);
            }
            else if (ArmorFeet != null && ArmorFeet.IsKeyword(input.Words[1]))
            {
                removedItem = ArmorFeet;
                handler = DoRemove(ITEM_SLOT.ARMOR_FEET);
            }
            else if (ArmorHead != null && ArmorHead.IsKeyword(input.Words[1]))
            {
                removedItem = ArmorHead;
                handler = DoRemove(ITEM_SLOT.ARMOR_HEAD);
            }
            else if (Backpack != null && Backpack.IsKeyword(input.Words[1]))
            {
                removedItem = Backpack;
                handler = DoRemove(ITEM_SLOT.BACKPACK);
            }
            else if (Ring1 != null && Ring1.IsKeyword(input.Words[1]))
            {
                removedItem = Ring1;
                handler = DoRemove(ITEM_SLOT.RING_1);
            }
            else if (Ring2 != null && Ring2.IsKeyword(input.Words[1]))
            {
                removedItem = Ring2;
                handler = DoRemove(ITEM_SLOT.RING_2);
            }

            return handler;
        }
        public override Handler DoRemove(ITEM_SLOT itemSlot)
        {
            if (HandsAreFull) { return Handler.HANDS_ARE_FULL; }

            string removedItem = "";

            switch (itemSlot)
            {
                case ITEM_SLOT.AMULET:
                    if (RightHand == null) { RightHand = Amulet; }
                    else if (LeftHand == null) { LeftHand = Amulet; }
                    removedItem = Amulet.Name;
                    Amulet = null;
                    break;
                case ITEM_SLOT.ARMOR_CHEST:
                    if (RightHand == null) { RightHand = ArmorChest; }
                    else if (LeftHand == null) { LeftHand = ArmorChest; }
                    DefensePower -= ArmorChest.ArmorFactor;
                    removedItem = ArmorChest.Name;
                    ArmorChest = null;
                    break;
                case ITEM_SLOT.ARMOR_FEET:
                    if (RightHand == null) { RightHand = ArmorFeet; }
                    else if (LeftHand == null) { LeftHand = ArmorFeet; }
                    DefensePower -= ArmorFeet.ArmorFactor;
                    removedItem = ArmorFeet.Name;
                    ArmorFeet = null;
                    break;
                case ITEM_SLOT.ARMOR_HEAD:
                    if (RightHand == null) { RightHand = ArmorHead; }
                    else if (LeftHand == null) { LeftHand = ArmorHead; }
                    DefensePower -= ArmorHead.ArmorFactor;
                    removedItem = ArmorHead.Name;
                    ArmorHead = null;
                    break;
                case ITEM_SLOT.BACKPACK:
                    if (RightHand == null) { RightHand = Backpack; }
                    else if (LeftHand == null) { LeftHand = Backpack; }
                    removedItem = Backpack.Name;
                    Backpack = null;
                    break;
                case ITEM_SLOT.RING_1:
                    if (RightHand == null) { RightHand = Ring1; }
                    else if (LeftHand == null) { LeftHand = Ring1; }
                    removedItem = Ring1.Name;
                    Ring1 = null;
                    break;
                case ITEM_SLOT.RING_2:
                    if (RightHand == null) { RightHand = Ring2; }
                    else if (LeftHand == null) { LeftHand = Ring2; }
                    removedItem = Ring2.Name;
                    Ring2 = null;
                    break;
            }

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_REMOVE, removedItem));
        }
        public override Handler DoShowInventory(TranslatedInput input)
        {
            return new Handler(RETURN_CODE.HANDLED, InventoryString);
        }
        public override Handler DoGetExtended(TranslatedInput input)
        {
            // take <item> from <container>
            if (input.Words[2] != "from") { return Handler.BAD_INPUT; }

            ItemContainer container = null;
            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_GET_FROM_CONTAINER;

            // holding container?
            if (LeftHand != null && LeftHand.IsKeyword(input.Words[3]) && LeftHand.Type == ITEM_TYPE.CONTAINER) { container = LeftHand as ItemContainer; }
            else if (RightHand != null && RightHand.IsKeyword(input.Words[3]) && RightHand.Type == ITEM_TYPE.CONTAINER) { container = RightHand as ItemContainer; }
            // equipped container?
            else if (Backpack != null && Backpack.IsKeyword(input.Words[2])) { container = Backpack; }
            // container on ground?
            else
            {
                container = CurrentRoom.FindItem(input.Words[3], ITEM_TYPE.CONTAINER) as ItemContainer;
                message = MESSAGE_ENUM.PLAYER_GET_FROM_ROOM_CONTAINER;
            }

            if (container == null) { return Handler.BAD_INPUT; }

            // valid container; find item
            Item item = container.Items.Get(input.Words[1]);
            if (item == null) { return Handler.BAD_ITEM; }

            // item found; attempt to put in hands
            if (HandsAreFull) { return Handler.HANDS_ARE_FULL; }

            PutItemInHand(item);
            container.Items.RemoveItem(item);
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, item.Name, container.Name));
        }
        public override Handler DoGet(TranslatedInput input)
        {
            // take <item> from <container>
            if (input.Words.Length == 4) { return DoGetExtended(input); }

            // take <item>
            if (input.Words.Length == 1)
            {
                switch (input.Words[0])
                {
                    case "get":
                        return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.WHAT, "Get"));
                    case "take":
                        return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.WHAT, "Take"));
                    default:
                        return Handler.BAD_INPUT;
                }
            }

            ItemContainer container = null;
            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_GET;

            // find item in current room
            Item item = CurrentRoom.FindItem(input.Words[1]);
            if (item == null)
            {
                // find item in a room container
                List<Item> containers = CurrentRoom.Items.GetItemsOfType(ITEM_TYPE.CONTAINER);
                foreach (Item i in containers)
                {
                    container = i as ItemContainer;
                    item = container.Items.Get(input.Words[1]);
                    if (item != null)
                    {
                        message = MESSAGE_ENUM.PLAYER_GET_FROM_ROOM_CONTAINER;
                        break;
                    }
                }
            }
            if (item == null)
            {
                message = MESSAGE_ENUM.PLAYER_GET_FROM_CONTAINER;

                if (LeftHand != null && LeftHand.Type == ITEM_TYPE.CONTAINER)
                {
                    container = LeftHand as ItemContainer;
                    item = container.Items.Get(input.Words[1]);
                }
            }
            if (item == null)
            {
                if (RightHand != null && RightHand.Type == ITEM_TYPE.CONTAINER)
                {
                    container = RightHand as ItemContainer;
                    item = container.Items.Get(input.Words[1]);
                }
            }
            if (item == null)
            {
                if (Backpack != null)
                {
                    container = Backpack;
                    item = container.Items.Get(input.Words[1]);
                }
            }
            if (item == null)
            {
                // item not found
                return Handler.BAD_ITEM;
            }

            // item found; attempt to put in hands
            if (HandsAreFull) { return Handler.HANDS_ARE_FULL; }
            PutItemInHand(item);

            // if we found in a container, remove from container
            if (container != null)
            {
                container.Items.RemoveItem(item);
                // AppendText(Messages.Message(MESSAGE_ENUM.PLAYER_GET_FROM_ROOM_CONTAINER, item.Name, container.Name));
            }
            // otherwise, remove from room
            else
            {
                CurrentRoom.RemoveItem(item);
                // AppendText(Messages.Message(MESSAGE_ENUM.PLAYER_GET, item.Name));
            }

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, item.Name, container == null ? "" : container.Name));
        }
        public override Handler DoDrop(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.WHAT, "Drop")); }

            Item item = GetItemFromHand(input.Words[1], true);
            if (item == null) { return Handler.NOT_CARRYING_ITEM; }

            CurrentRoom.AddItem(item);
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_DROP, item.Name));
        }
        public override Handler DoBuy(TranslatedInput input)
        {
            if (input.Words.Length > 2) { return Handler.BAD_INPUT; }

            RoomShop shop = CurrentRoom as RoomShop;
            if (shop == null) { return Handler.NOT_A_SHOP; }

            // "buy" lists items for sale
            if (input.Words.Length == 1)
            {
                return new Handler(RETURN_CODE.HANDLED, shop.SoldItemsString);
            }

            // scenario 1: buy <number>
            // <number> corresponds with the list obtained through SoldItemsString
            // attempt to parse second word as number
            int itemIndex;
            Item boughtItem = null;
            int nPrice = -1;

            if (int.TryParse(input.Words[1], out itemIndex))
            {
                if (itemIndex < 1 || itemIndex > shop.SoldItems.Count) { return Handler.BAD_INPUT; }

                nPrice = (int)(shop.SoldItems[itemIndex - 1].Value * shop.SellsAt);
                if (Gold < nPrice)
                {
                    return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.NOT_ENOUGH_GOLD, shop.SoldItems[itemIndex - 1].Name));
                }

                boughtItem = shop.SoldItems[itemIndex - 1].DeepClone();
            }

            // TODO: scenario 2: parse second word as item keyword
            // TODO: find item with merged input
            // MergeInput(int nStartingIndex)
            // FindItem(string, bool bMultiWord = false)
            // - if true, match string to full item name

            if (boughtItem == null) { return Handler.BAD_INPUT; }
            if (HandsAreFull) { return Handler.HANDS_ARE_FULL; }

            if (RightHand == null)
            {
                RightHand = boughtItem;
            }
            else if (LeftHand == null)
            {
                LeftHand = boughtItem;
            }

            Gold -= nPrice;
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_BUY, boughtItem.Name, nPrice.ToString()));
        }
        public override Handler DoGold(TranslatedInput input)
        {
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_CARRYING_GOLD, Gold.ToString()));
        }
        public override Handler DoPrice(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.WHAT, "Price")); }

            // verify that we're in a shop
            RoomShop shop = CurrentRoom as RoomShop;
            if (shop == null) { return Handler.NOT_A_SHOP; }

            // attempt to price right hand item
            Item item = RightHand;
            Handler handler = shop.PriceItem(item, input.Words[1]);

            // if not handled, attempt to price left hand item
            if (handler.Equals(Handler.BAD_ITEM))
            {
                item = LeftHand;
                handler = shop.PriceItem(item, input.Words[1]);
            }

            return handler;
        }
        public override Handler DoSell(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.WHAT, "Sell")); }

            // verify that we're in a shop
            RoomShop shop = CurrentRoom as RoomShop;
            if (shop == null) { return Handler.NOT_A_SHOP; }

            // attempt to sell item
            // TODO: find player item first, THEN attempt to sell to shop
            return shop.DoBuyFromEntity(this, input.Words[1]);
        }
        public override Handler DoStand(TranslatedInput input) 
        {
            if (Posture == ENTITY_POSTURE.STANDING) { return Handler.ALREADY_STANDING; }
            Posture = ENTITY_POSTURE.STANDING;
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_STAND));
        }
        public override Handler DoKneel(TranslatedInput input) 
        {
            if (Posture == ENTITY_POSTURE.KNEELING) { return Handler.ALREADY_KNEELING; }
            Posture = ENTITY_POSTURE.KNEELING;
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_KNEEL));
        }
        public override Handler DoSit(TranslatedInput input) 
        {
            if (Posture == ENTITY_POSTURE.SITTING) { return Handler.ALREADY_SITTING; }
            Posture = ENTITY_POSTURE.SITTING;
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_SIT));
        }
        public override Handler DoAttack(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.WHAT, input.Words[0].ToSentenceCase())); }

            string NPCString = "";
            if (input.Words.Length == 2) { NPCString = input.Words[1]; }
            else if (input.Words.Length == 3) { NPCString = input.Words[2]; }
            
            EntityNPC npc = CurrentRoom.FindNPC(NPCString);
            if (npc == null) { return Handler.BAD_INPUT; }

            if (RightHand != null && RightHand.Type != ITEM_TYPE.WEAPON) { return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.NOT_A_WEAPON, RightHand.Name)); }
            string weapon = RightHand == null ? "fist" : RightHand.Name;

            // calculate some damage
            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_ATTACKS_NPC;
            int damage = 5;
            npc.CurrentHealth -= damage;
            if (npc.CurrentHealth <= 0) { message = MESSAGE_ENUM.PLAYER_KILLS_NPC; }

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, npc.Name, weapon, damage.ToString()));
        }

        #endregion
    }
}
