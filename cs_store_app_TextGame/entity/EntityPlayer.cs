using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    public class EntityPlayer : Entity
    {
        public EntityPlayer() : base() 
        {
            CurrentHealth = 100;
            MaximumHealth = 100;
            CurrentMagic = 20;
            MaximumMagic = 20;
            SetCurrentRoom(0, 0, 0); 
        }
        
        #region Display Strings
        
        public string CurrentRegionDisplayString
        {
            get
            {
                return "[" + CurrentRegion.Name + " - " + CurrentSubregion.Name + "]\n";
            }
        }
        public string CurrentRoomDisplayString
        {
            get
            {
                return CurrentRegionDisplayString + CurrentRoom.FullDisplayString;
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
        
        #endregion

        #region Display Runs, Paragraphs

        public Paragraph CurrentRoomDisplayParagraph
        {
            get
            {
                Paragraph p = new Paragraph();

                p.Inlines.Add(CurrentRegionDisplayString.ToRun(Colors.Gray));

                foreach (Inline i in CurrentRoomDisplayParagraph.Inlines)
                {
                    p.Inlines.Add(i);
                }

                return p;
            }
        }
        public Paragraph HandsParagraph
        {
            get
            {
                Paragraph p = new Paragraph();

                if (LeftHand == null && RightHand == null)
                {
                    p.Inlines.Add(("Your hands are empty.").ToRun(Colors.Gray));
                    return p;
                }

                string strReturn = "";

                // guaranteed to be holding something here

                // RIGHT HAND
                if (RightHand == null) { strReturn += "Your right hand is empty and "; }
                else
                {
                    strReturn = "You are holding a";
                    if (RightHand.Name[0].IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " ";

                    p.Inlines.Add(strReturn.ToRun(Colors.Gray));
                    p.Inlines.Add(RightHand.Name.ToRun(Colors.LightGreen));

                    strReturn = " in your right hand";
                }

                // LEFT HAND
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

                    strReturn += " ";
                    
                    p.Inlines.Add(strReturn.ToRun(Colors.Gray));
                    p.Inlines.Add(LeftHand.Name.ToRun(Colors.LightGreen));
                    p.Inlines.Add((" in your left hand.").ToRun(Colors.Gray));
                }

                return p;
            }
        }
        // TODO: optimize to minimize run count
        public Paragraph InventoryParagraph
        {
            get
            {
                Paragraph p = new Paragraph();
                List<Run> inventory = new List<Run>();

                if (Backpack != null) 
                { 
                    inventory.Add(Backpack.NameIndefiniteArticle.ToRun(Colors.Gray));
                    inventory.Add(Backpack.NameAsRun); 
                }
                if (ArmorChest != null) 
                {
                    inventory.Add(ArmorChest.NameIndefiniteArticle.ToRun(Colors.Gray));
                    inventory.Add(ArmorChest.NameAsRun);
                }
                if (ArmorFeet != null)
                {
                    inventory.Add(ArmorFeet.NameIndefiniteArticle.ToRun(Colors.Gray));
                    inventory.Add(ArmorFeet.NameAsRun);
                }
                if (ArmorHead != null)
                {
                    inventory.Add(ArmorHead.NameIndefiniteArticle.ToRun(Colors.Gray));
                    inventory.Add(ArmorHead.NameAsRun);
                }
                if (Ring1 != null) 
                {
                    inventory.Add(Ring1.NameIndefiniteArticle.ToRun(Colors.Gray));
                    inventory.Add(Ring1.NameAsRun);
                }
                if (Ring2 != null) 
                {
                    inventory.Add(Ring2.NameIndefiniteArticle.ToRun(Colors.Gray));
                    inventory.Add(Ring2.NameAsRun);
                }
                if (Amulet != null) 
                {
                    inventory.Add(Amulet.NameIndefiniteArticle.ToRun(Colors.Gray));
                    inventory.Add(Amulet.NameAsRun);
                }

                p.Inlines.Add(("You are wearing ").ToRun(Colors.Gray));

                switch (inventory.Count)
                {
                    case 0:
                        p.Inlines.Add(("You aren't wearing anything!\n").ToRun(Colors.Gray));
                        return p;
                    case 1:
                        p.Inlines.Add(inventory[0]);
                        break;
                    case 2:
                        p.Inlines.Add(inventory[0]);
                        p.Inlines.Add((" and ").ToRun(Colors.Gray));
                        p.Inlines.Add(inventory[1]);
                        break;
                    default:
                        for (int i = 0; i < inventory.Count - 1; i++)
                        {
                            p.Inlines.Add(inventory[i]);
                            p.Inlines.Add((", ").ToRun(Colors.Gray));
                        }

                        p.Inlines.Add(("and ").ToRun(Colors.Gray));
                        p.Inlines.Add(inventory[inventory.Count - 1]);
                        break;
                }

                p.Inlines.Add((".\n").ToRun(Colors.Gray));
                return p;
            }
        }
        #endregion

        #region Input Handling

        public override Handler DoMoveBasic(TranslatedInput input)
        {
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }
            if (Posture == ENTITY_POSTURE.SITTING) { return Handler.ERROR_SITTING; }
            if (Posture == ENTITY_POSTURE.KNEELING) { return Handler.ERROR_KNEELING; }

            // basic direction
            // if "go <direction>"
            int nDirection;
            if (input.Words[0] == "go" || input.Words[0] == "move") { nDirection = Statics.DirectionToInt(input.Words[1]); }
            // see TranslatedInput constructor; hack job replaces first word with integer direction
            else { nDirection = int.Parse(input.Words[0]); }

            Exit exit = CurrentRoom.Exits.Get(nDirection);
            if (exit.Region == -1) { return Handler.ERROR_WRONG_DIRECTION; }

            SetCurrentRoom(exit.Region, exit.Subregion, exit.Room);
            return new Handler(RETURN_CODE.HANDLED, CurrentRoomDisplayString);
        }
        public override Handler DoMoveConnection(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return Handler.ERROR_GO_WHERE; }

            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }
            if (Posture == ENTITY_POSTURE.SITTING) { return Handler.ERROR_SITTING; }
            if (Posture == ENTITY_POSTURE.KNEELING) { return Handler.ERROR_KNEELING; }

            // go <direction>
            if (Statics.DirectionToInt(input.Words[1]) != -1) { return DoMoveBasic(input); }

            // go <connection>
            Connection connection = CurrentRoom.Connections.Find(input.Words[0], input.Words[1]);
            if (connection != null)
            {
                SetCurrentRoom(connection.DestinationRegion, connection.DestinationSubregion, connection.DestinationRoom);
                input = null;
                return DoLook(input);
            }

            return Handler.ERROR_BAD_INPUT;
        }
        public override Handler DoLook(TranslatedInput input)
        {
            // "look"
            if (input == null || input.Words.Length < 2) { return new Handler(RETURN_CODE.HANDLED, CurrentRoomDisplayString); }
            // look <item|npc>
            else if (input.Words.Length == 2) { return DoLook(input.Words[1]); }
            // look <at|in|my|ordinal> <item|npc>
            else if (input.Words.Length == 3) { return DoLook(input.Words[1], input.Words[2]); }
            // look <at|in> <my|ordinal> <item|npc>
            else if (input.Words.Length == 4) { return DoLook(input.Words[1], input.Words[2], input.Words[3]); }
            // handling a five-word phrase seems impractical
            return Handler.ERROR_BAD_INPUT;
        }
        public override Handler DoLook(string strWord)
        {
            // look hands - special case
            if (strWord == "hands") { return DoLookHands(null); }

            // TODO: consider more specific strings for held or equipped items
            
            Item item = CurrentRoom.Items.Find(strWord);
            if (item != null) { return new Handler(RETURN_CODE.HANDLED, item.Description + "\n"); }

            item = GetItemFromHand(strWord, false);
            if (item != null) { return new Handler(RETURN_CODE.HANDLED, item.Description + "\n"); }

            item = GetItemFromEquipment(strWord, false);
            if (item != null) { return new Handler(RETURN_CODE.HANDLED, item.Description + "\n"); }

            EntityNPC npc = CurrentRoom.NPCs.Find(strWord);
            if (npc != null) { return new Handler(RETURN_CODE.HANDLED, npc.DisplayString); }

            return Handler.ERROR_BAD_INPUT;
        }
        public override Handler DoLook(string strWord1, string strWord2)
        {
            if (strWord1 == "at") { return DoLook(strWord2); }
            if (strWord1 == "in") { return DoLookInContainer(strWord2); }
            
            // TODO: handle "my"
            // - look [at] my backpack, "at" is implied--not "in"
            // can't simply call strWord1 version; needs to be specific for "my"
            if (strWord1 == "my") { return Handler.ERROR_NEED_TO_IMPLEMENT; }

            int ordinal;
            if (!Statics.OrdinalStringToInt.TryGetValue(strWord1, out ordinal)) { return Handler.ERROR_BAD_INPUT; }

            Item item = CurrentRoom.Items.Find(strWord2, ITEM_TYPE.ANY, ordinal);
            if (item != null) { return new Handler(RETURN_CODE.HANDLED, item.Description + "\n"); }

            EntityNPC npc = CurrentRoom.NPCs.Find(strWord2, ordinal);
            if (npc != null) { return new Handler(RETURN_CODE.HANDLED, npc.DisplayString); }

            return Handler.ERROR_BAD_INPUT;
        }
        public override Handler DoLook(string strWord1, string strWord2, string strWord3)
        {
            // TODO: strWord2 could be ordinal OR "my"
            if (strWord1 == "at") { return DoLook(strWord2, strWord3); }
            if (strWord1 == "in")
            {
                int ordinal;
                if (Statics.OrdinalStringToInt.TryGetValue(strWord2, out ordinal)) { return DoLookInContainer(strWord3, ordinal); }
                else if(strWord2 == "my")
                {
                    // look in my <container>
                    if (RightHand != null && RightHand.Type == ITEM_TYPE.CONTAINER) 
                    {
                        ItemContainer container = RightHand as ItemContainer;
                        return new Handler(RETURN_CODE.HANDLED, container.ItemsString + "\n");
                    }
                    if (LeftHand != null && RightHand.Type == ITEM_TYPE.CONTAINER)
                    {
                        ItemContainer container = LeftHand as ItemContainer;
                        return new Handler(RETURN_CODE.HANDLED, container.ItemsString + "\n");
                    }
                    if (Backpack != null) 
                    {
                        return new Handler(RETURN_CODE.HANDLED, Backpack.ItemsString + "\n");                        
                    }

                    return Handler.ERROR_BAD_INPUT; 
                }
            }
            return Handler.ERROR_BAD_INPUT;
        }
        public override Handler DoLookHands(TranslatedInput input)
        {
            return new Handler(RETURN_CODE.HANDLED, HandsString + "\n");
        }
        public override Handler DoLookInContainer(string strKeyword, int ordinal = 0)
        {
            // priority: room, hand, equipped backpack
            // TODO: ensure consistent priority throughout
            // TODO: "my"
            ItemContainer container = CurrentRoom.Items.Find(strKeyword, ITEM_TYPE.CONTAINER, ordinal) as ItemContainer;
            if (container == null) { container = GetItemFromHand(strKeyword, false, ITEM_TYPE.CONTAINER) as ItemContainer; }
            if (container == null && Backpack != null && Backpack.IsKeyword(strKeyword)) { container = Backpack; }
            if (container == null) { return Handler.ERROR_BAD_INPUT; }

            // container found
            if (container.Closed) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_CONTAINER_CLOSED, container.Name)); }
            return new Handler(RETURN_CODE.HANDLED, container.ItemsString + "\n");
        }
        public override Handler DoEat(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, "Eat")); }
            else if (input.Words.Length > 2) { return Handler.ERROR_BAD_INPUT; }
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }

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
                itemToEat = CurrentRoom.Items.Find(input.Words[1], ITEM_TYPE.FOOD) as ItemFood;
                message = MESSAGE_ENUM.PLAYER_EAT_GROUND_ITEM;
            }

            if (itemToEat == null) { return Handler.ERROR_BAD_INPUT; }

            CurrentHealth += itemToEat.HealthPerBite;
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
                else { CurrentRoom.Items.Remove(itemToEat); }
            }

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, itemToEat.Name, itemToEat.HealthPerBite.ToString(), itemToEat.NumberOfBites.ToString()));
        }
        public override Handler DoDrink(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, "Drink")); }
            else if (input.Words.Length > 2) { return Handler.ERROR_BAD_INPUT; }
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }

            ItemDrink itemToDrink = null;
            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_DRINK;

            if (RightHand != null && RightHand.Type == ITEM_TYPE.DRINK && RightHand.IsKeyword(input.Words[1])) { itemToDrink = RightHand as ItemDrink; }
            else if (LeftHand != null && LeftHand.Type == ITEM_TYPE.DRINK && LeftHand.IsKeyword(input.Words[1])) { itemToDrink = LeftHand as ItemDrink; }
            else
            {
                itemToDrink = CurrentRoom.Items.Find(input.Words[1], ITEM_TYPE.DRINK) as ItemDrink;
                message = MESSAGE_ENUM.PLAYER_DRINK_GROUND_ITEM;
            }

            if (itemToDrink == null) { return Handler.ERROR_BAD_INPUT; }

            CurrentMagic += itemToDrink.MagicPerDrink;
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
                else { CurrentRoom.Items.Remove(itemToDrink); }
            }

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, itemToDrink.Name, itemToDrink.MagicPerDrink.ToString(), itemToDrink.NumberOfDrinks.ToString()));
        }
        public override Handler DoOpen(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, "Open")); }
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }

            ItemContainer container = null;

            if (Backpack != null) { container = Backpack; }
            else if (RightHand != null && RightHand.Type == ITEM_TYPE.CONTAINER) { container = RightHand as ItemContainer; }
            else if (LeftHand != null && LeftHand.Type == ITEM_TYPE.CONTAINER) { container = LeftHand as ItemContainer; }
            else { container = CurrentRoom.Items.Find(input.Words[1], ITEM_TYPE.CONTAINER) as ItemContainer; }

            if (container == null) { return Handler.ERROR_BAD_ITEM; }

            if (!container.Closed) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_CONTAINER_ALREADY_OPEN, container.Name)); }

            container.Closed = false;
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_OPEN, container.Name));
        }
        public override Handler DoClose(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, "Close")); }
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }

            ItemContainer container = null;
            // close MY <container>
            // hands, equipped backpack
            if (input.Words.Length == 3 && input.Words[1] == "my") { return Handler.ERROR_NEED_TO_IMPLEMENT; }

            if (Backpack != null) { container = Backpack; }
            else if (RightHand != null && RightHand.Type == ITEM_TYPE.CONTAINER) { container = RightHand as ItemContainer; }
            else if (LeftHand != null && LeftHand.Type == ITEM_TYPE.CONTAINER) { container = LeftHand as ItemContainer; }
            else { container = CurrentRoom.Items.Find(input.Words[1], ITEM_TYPE.CONTAINER) as ItemContainer; }

            if (container == null) { return Handler.ERROR_BAD_ITEM; }

            if (container.Closed) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_CONTAINER_ALREADY_CLOSED, container.Name)); }
            if (!container.Closable) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_CONTAINER_NOT_CLOSABLE, container.Name)); }

            container.Closed = true;
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_CLOSE, container.Name));
        }
        public override Handler DoPut(TranslatedInput input)
        {
            // put <item> in <container> - TODO: <on surface>
            if (input.Words.Length < 4) { return Handler.ERROR_BAD_INPUT; }
            if (input.Words[2] != "in") { return Handler.ERROR_BAD_INPUT; }
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }

            // put <item> in MY <container>
            if (input.Words.Length == 5 && input.Words[3] == "my") { return Handler.ERROR_NEED_TO_IMPLEMENT; }

            // must be holding item
            // don't remove item here
            Item item = GetItemFromHand(input.Words[1], false);
            if (item == null) { return Handler.ERROR_NOT_CARRYING_ITEM; }

            ItemContainer container = null;
            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_PUT_IN_PLAYER_CONTAINER;

            // make sure that we don't accidentally try to put a container into itself
            if (Backpack != null && Backpack.IsKeyword(input.Words[3]) && !(Backpack.Equals(item))) { container = Backpack; }
            else if (RightHand != null && RightHand.IsKeyword(input.Words[3]) && !(RightHand.Equals(item))) { container = RightHand as ItemContainer; }
            else if (LeftHand != null && LeftHand.IsKeyword(input.Words[3]) && !(LeftHand.Equals(item))) { container = LeftHand as ItemContainer; }
            else
            {
                container = CurrentRoom.Items.Find(input.Words[3], ITEM_TYPE.CONTAINER) as ItemContainer;
                if (container != null && container.Equals(item)) { container = null; }
                else { message = MESSAGE_ENUM.PLAYER_PUT_IN_GROUND_CONTAINER; }
            }

            if (container == null) { return Handler.ERROR_BAD_INPUT; }
            if (container.Closed) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_CONTAINER_CLOSED, container.Name)); }

            item = GetItemFromHand(input.Words[1], true);
            container.Items.Add(item);

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, item.Name, container.Name));
        }
        public override Handler DoEquip(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, "Equip")); }            
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }

            string strItemToEquip = input.Words[1];
            if (input.Words.Length == 3 && input.Words[1] == "my") { strItemToEquip = input.Words[2]; }

            Item itemToEquip = null;
            if (RightHand != null && RightHand.IsKeyword(strItemToEquip)) { itemToEquip = RightHand; }
            else if (LeftHand != null && LeftHand.IsKeyword(strItemToEquip)) { itemToEquip = LeftHand; }

            if (itemToEquip == null) { return Handler.ERROR_BAD_INPUT; }

            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_EQUIP;

            switch (itemToEquip.Type)
            {
                case ITEM_TYPE.ARMOR_CHEST:
                    if (ArmorChest != null) { return Handler.ERROR_ALREADY_EQUIPPED; }
                    ArmorChest = itemToEquip as ItemArmorChest;
                    DefensePower += ArmorChest.ArmorFactor;
                    break;
                case ITEM_TYPE.ARMOR_FEET:
                    if (ArmorFeet != null) { return Handler.ERROR_ALREADY_EQUIPPED; }
                    ArmorFeet = itemToEquip as ItemArmorFeet;
                    DefensePower += ArmorFeet.ArmorFactor;
                    break;
                case ITEM_TYPE.ARMOR_HEAD:
                    if (ArmorHead != null) { return Handler.ERROR_ALREADY_EQUIPPED; }
                    ArmorHead = itemToEquip as ItemArmorHead;
                    DefensePower += ArmorHead.ArmorFactor;
                    break;
                case ITEM_TYPE.CONTAINER:
                    if (Backpack != null) { return Handler.ERROR_ALREADY_EQUIPPED; }
                    Backpack = itemToEquip as ItemContainer;
                    message = MESSAGE_ENUM.PLAYER_EQUIP_BACKPACK;
                    break;
                case ITEM_TYPE.ACCESSORY_AMULET:
                    if (Amulet != null) { return Handler.ERROR_ALREADY_EQUIPPED; }
                    Amulet = itemToEquip as ItemAccessoryAmulet;
                    break;
                case ITEM_TYPE.ACCESSORY_RING:
                    if (Ring1 != null && Ring2 != null) { return Handler.ERROR_ALREADY_EQUIPPED; }
                    if (Ring1 == null) { Ring1 = itemToEquip as ItemAccessoryRing; }
                    else if (Ring2 == null) { Ring2 = itemToEquip as ItemAccessoryRing; }
                    break;
                default:
                    return Handler.ERROR_ITEM_NOT_EQUIPPABLE;
            }

            // TODO: ITEM_SOURCE?
            if (itemToEquip.Equals(RightHand)) { RightHand = null; }
            else if (itemToEquip.Equals(LeftHand)) { LeftHand = null; }

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, itemToEquip.Name));
        }
        public override Handler DoRemove(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, "Remove")); }
            if (input.Words.Length > 2) { return Handler.ERROR_BAD_INPUT; }
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }

            Handler handler = Handler.ERROR_BAD_INPUT;
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
            if (HandsAreFull) { return Handler.ERROR_HANDS_ARE_FULL; }

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
            if (input.Words[2] != "from") { return Handler.ERROR_BAD_INPUT; }

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
                container = CurrentRoom.Items.Find(input.Words[3], ITEM_TYPE.CONTAINER) as ItemContainer;
                message = MESSAGE_ENUM.PLAYER_GET_FROM_ROOM_CONTAINER;
            }

            if (container == null) { return Handler.ERROR_BAD_INPUT; }

            // valid container; find item
            Item item = container.Items.Find(input.Words[1]);
            if (item == null) { return Handler.ERROR_BAD_ITEM; }

            // item found; attempt to put in hands
            if (HandsAreFull) { return Handler.ERROR_HANDS_ARE_FULL; }

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
                        return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, "Get"));
                    case "take":
                        return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, "Take"));
                    default:
                        return Handler.ERROR_BAD_INPUT;
                }
            }

            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }

            ItemContainer container = null;
            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_GET;

            // find item in current room
            Item item = CurrentRoom.Items.Find(input.Words[1]);
            if (item == null)
            {
                // find item in a room container
                List<Item> containers = CurrentRoom.Items.GetItemsOfType(ITEM_TYPE.CONTAINER);
                for (int i = containers.Count() - 1; i >= 0; i--)
                {
                    container = containers[i] as ItemContainer;
                    item = container.Items.Find(input.Words[1]);
                    if (item != null)
                    {
                        message = MESSAGE_ENUM.PLAYER_GET_FROM_ROOM_CONTAINER;
                        break;
                    }
                }
                // changed: reversed list traversal
                //foreach (Item i in containers)
                //{
                //}
            }
            if (item == null)
            {
                message = MESSAGE_ENUM.PLAYER_GET_FROM_CONTAINER;

                if (LeftHand != null && LeftHand.Type == ITEM_TYPE.CONTAINER)
                {
                    container = LeftHand as ItemContainer;
                    item = container.Items.Find(input.Words[1]);
                }
            }
            if (item == null)
            {
                if (RightHand != null && RightHand.Type == ITEM_TYPE.CONTAINER)
                {
                    container = RightHand as ItemContainer;
                    item = container.Items.Find(input.Words[1]);
                }
            }
            if (item == null)
            {
                if (Backpack != null)
                {
                    container = Backpack;
                    item = container.Items.Find(input.Words[1]);
                }
            }
            if (item == null)
            {
                // item not found
                return Handler.ERROR_BAD_ITEM;
            }

            // item found; attempt to put in hands
            if (HandsAreFull) { return Handler.ERROR_HANDS_ARE_FULL; }
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
                CurrentRoom.Items.Remove(item);
                // AppendText(Messages.Message(MESSAGE_ENUM.PLAYER_GET, item.Name));
            }

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, item.Name, container == null ? "" : container.Name));
        }
        public override Handler DoDrop(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, "Drop")); }
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }

            Item item = GetItemFromHand(input.Words[1], true);
            if (item == null) { return Handler.ERROR_NOT_CARRYING_ITEM; }

            CurrentRoom.Items.Add(item);
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_DROP, item.Name));
        }
        public override Handler DoBuy(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, "Buy")); }
            if (input.Words.Length > 2) { return Handler.ERROR_BAD_INPUT; }
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }

            RoomShop shop = CurrentRoom as RoomShop;
            if (shop == null) { return Handler.ERROR_NOT_A_SHOP; }

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
                if (itemIndex < 1 || itemIndex > shop.SoldItems.Count) { return Handler.ERROR_BAD_INPUT; }

                nPrice = (int)(shop.SoldItems[itemIndex - 1].Value * shop.SellsAt);
                if (Gold < nPrice)
                {
                    return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_NOT_ENOUGH_GOLD, shop.SoldItems[itemIndex - 1].Name));
                }

                boughtItem = shop.SoldItems[itemIndex - 1].DeepClone();
            }

            // TODO: scenario 2: parse second word as item keyword
            // TODO: find item with merged input
            // MergeInput(int nStartingIndex)
            // FindItem(string, bool bMultiWord = false)
            // - if true, match string to full item name

            if (boughtItem == null) { return Handler.ERROR_BAD_INPUT; }
            if (HandsAreFull) { return Handler.ERROR_HANDS_ARE_FULL; }

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
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, "Price")); }
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }

            // verify that we're in a shop
            RoomShop shop = CurrentRoom as RoomShop;
            if (shop == null) { return Handler.ERROR_NOT_A_SHOP; }

            // attempt to price right hand item
            Item item = RightHand;
            Handler handler = shop.DoPriceItem(item, input.Words[1]);

            // if not handled, attempt to price left hand item
            if (handler.Equals(Handler.ERROR_BAD_ITEM))
            {
                item = LeftHand;
                handler = shop.DoPriceItem(item, input.Words[1]);
            }

            return handler;
        }
        public override Handler DoSell(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, "Sell")); }
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }

            // verify that we're in a shop
            RoomShop shop = CurrentRoom as RoomShop;
            if (shop == null) { return Handler.ERROR_NOT_A_SHOP; }

            // attempt to sell item
            // TODO: find player item first, THEN attempt to sell to shop
            return shop.DoBuyFromEntity(this, input.Words[1]);
        }
        public override Handler DoStand(TranslatedInput input) 
        {
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }
            if (Posture == ENTITY_POSTURE.STANDING) { return Handler.ERROR_ALREADY_STANDING; }
            Posture = ENTITY_POSTURE.STANDING;
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_STAND));
        }
        public override Handler DoKneel(TranslatedInput input) 
        {
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }
            if (Posture == ENTITY_POSTURE.KNEELING) { return Handler.ERROR_ALREADY_KNEELING; }
            Posture = ENTITY_POSTURE.KNEELING;
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_KNEEL));
        }
        public override Handler DoSit(TranslatedInput input) 
        {
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }
            if (Posture == ENTITY_POSTURE.SITTING) { return Handler.ERROR_ALREADY_SITTING; }
            Posture = ENTITY_POSTURE.SITTING;
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_SIT));
        }
        public override Handler DoAttack(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, input.Words[0].ToSentenceCase())); }
            if (IsDead) { return Handler.ERROR_PLAYER_IS_DEAD; }

            string NPCName = "";
            int ordinal = 0;
            if (input.Words.Length == 2) { NPCName = input.Words[1]; }
            else if (input.Words.Length == 3) 
            {
                if (!Statics.OrdinalStringToInt.TryGetValue(input.Words[1], out ordinal)) { return Handler.ERROR_BAD_INPUT; }
                NPCName = input.Words[2]; 
            }

            EntityNPC npc = CurrentRoom.NPCs.Find(NPCName, ordinal);
            if (npc == null) { return Handler.ERROR_BAD_INPUT; }
            if (npc.IsDead) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_NPC_ALREADY_DEAD, npc.NameBase)); }

            if (RightHand != null && RightHand.Type != ITEM_TYPE.WEAPON) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_NOT_A_WEAPON, RightHand.Name)); }
            string weapon = RightHand == null ? "fist" : RightHand.Name;

            // calculate damage
            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_ATTACKS_NPC;
            int damage = AttackPower - npc.DefensePower;
            npc.CurrentHealth -= damage;
            if (npc.CurrentHealth <= 0) { message = MESSAGE_ENUM.PLAYER_KILLS_NPC; }

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, npc.NameBase, weapon, damage.ToString()));
        }
        public override Handler DoShowHealth(TranslatedInput input)
        {
            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_SHOW_HEALTH, HealthString, MagicString));
        }
        public override Handler DoSearch(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WHAT, "Search")); }
            if (input.Words.Length > 2) { return Handler.ERROR_BAD_INPUT; }

            EntityNPC npc = CurrentRoom.NPCs.Find(input.Words[1]);
            if (npc == null) { return Handler.ERROR_BAD_INPUT; }
            if (!npc.IsDead) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_NPC_NOT_DEAD, npc.Name)); }

            // add npc inventory to room
            // TODO: messaging?
            // You search the goblin and remove its equipment. You find <x> gold.
            Gold += npc.Gold;
            npc.BeSearched();

            if (npc.Gold > 0)
            {
                return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_SEARCH_WITH_GOLD, npc.Name, npc.Gold.ToString()));
            }
            else
            {
                return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_SEARCH_NO_GOLD, npc.Name));
            }
        }

        #endregion
    }
}