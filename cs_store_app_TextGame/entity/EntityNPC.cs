using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    public class EntityNPC : Entity
    {
        // TODO: complex action
        // - for example, a goblin might be hungry
        // - first, it travels until it finds food
        // - then, it picks up the food, takes a bite, then drops it (or finishes it)
        // FIND FOOD
        // PICK UP FOOD
        // EAT
        // DROP FOOD
        #region Attributes

        public DateTime LastActionTime = DateTime.Now;
        public int ActionPulse { get; set; }
        public List<string> Keywords = new List<string>();
        public override string HandsString
        {
            get
            {
                if (LeftHand == null && RightHand == null)
                {
                    return "The " + Name + " isn't holding anything.";
                }

                string strReturn = "";
                if (RightHand == null) { strReturn += "The " + Name + "'s right hand is empty and "; }
                else
                {
                    strReturn = "The " + Name + " is holding a";
                    if (RightHand.Name[0].IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " " + RightHand.Name;
                    strReturn += " in its right hand";
                }

                if (LeftHand == null) { strReturn += "."; }
                else
                {
                    if (RightHand == null)
                    {
                        strReturn = "The " + Name + " is holding a";
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
                    strReturn += " in its left hand.";
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

                string strReturn = "The " + Name + " is wearing ";

                switch (inventory.Count)
                {
                    case 0:
                        return "The " + Name + " isn't wearing anything special.\n";
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
        public static List<EntityNPCBehavior> Behavior = new List<EntityNPCBehavior>();

        #endregion
        #region Constructors

        public EntityNPC() : base() { }
        public EntityNPC Clone()
        {
            EntityNPC npc = new EntityNPC();
            npc.ActionPulse = 10000 + Statics.r.Next(5000);
            npc.NID = Statics.EntityCount++;
            npc.ID = ID;
            npc.Name = _name;
            npc.Gold = Gold;
            npc.MaximumHealth = MaximumHealth;
            npc.CurrentHealth = CurrentHealth;
            if (RightHand != null) { npc.RightHand = RightHand.DeepClone(); }
            if (LeftHand != null) { npc.LeftHand = LeftHand.DeepClone(); }
            if (ArmorChest != null) { npc.ArmorChest = ArmorChest.DeepClone(); }
            if (ArmorHead != null) { npc.ArmorHead = ArmorHead.DeepClone(); }
            if (ArmorFeet != null) { npc.ArmorFeet = ArmorFeet.DeepClone(); }
            if (Backpack != null) { npc.Backpack = Backpack.DeepClone(); }
            if (Ring1 != null) { npc.Ring1 = Ring1.DeepClone(); }
            if (Ring2 != null) { npc.Ring2 = Ring2.DeepClone(); }
            if (Amulet != null) { npc.Amulet = Amulet.DeepClone(); }

            foreach (string keyword in Keywords)
            {
                npc.Keywords.Add(keyword);
            }

            return npc;
        }

        public EntityNPC(XElement npcNode)
        {
            ID = int.Parse(npcNode.Element("id").Value);
            Name = npcNode.Element("name").Value;
            MaximumHealth = int.Parse(npcNode.Element("maximum-health").Value);
            CurrentHealth = MaximumHealth;

            var keywordNodes = from keywords in npcNode
                                .Elements("keywords")
                                  .Elements("keyword")
                               select keywords;
            foreach (var keywordNode in keywordNodes)
            {
                Keywords.Add(keywordNode.Value);
            }

            // behavior
            var behaviorNode = npcNode.Element("behavior");
            if(behaviorNode != null)
            {
                foreach(var node in behaviorNode.Elements())
                {
                    ACTION_ENUM action = TranslatedInput.StringToAction[node.Name.LocalName];
                    int percentage = int.Parse(node.Value);
                    Behavior.Add(new EntityNPCBehavior(action, percentage));
                }
            }

            // inventory
            var inventoryNode = npcNode.Element("inventory");

            // right hand
            var rightHandNode = inventoryNode.Element("right-hand");
            if (rightHandNode != null)
            {
                var itemTypeNode = rightHandNode.Elements().First();
                int nIndex = int.Parse(itemTypeNode.Value);
                switch (itemTypeNode.Name.LocalName)
                {
                    case "weapon":
                        RightHand = ItemTemplates.ItemsWeapon[nIndex].DeepClone();
                        break;
                    case "armor-shield":
                        RightHand = ItemTemplates.ItemsArmorShield[nIndex].DeepClone();
                        break;
                    case "armor-head":
                        RightHand = ItemTemplates.ItemsArmorHead[nIndex].DeepClone();
                        break;
                    case "armor-feet":
                        RightHand = ItemTemplates.ItemsArmorFeet[nIndex].DeepClone();
                        break;
                    case "armor-chest":
                        RightHand = ItemTemplates.ItemsArmorChest[nIndex].DeepClone();
                        break;
                    case "accessory-ring":
                        RightHand = ItemTemplates.ItemsAccessoryRing[nIndex].DeepClone();
                        break;
                    case "accessory-amulet":
                        RightHand = ItemTemplates.ItemsAccessoryAmulet[nIndex].DeepClone();
                        break;
                    case "drink":
                        RightHand = ItemTemplates.ItemsDrink[nIndex].DeepClone();
                        break;
                    case "food":
                        RightHand = ItemTemplates.ItemsFood[nIndex].DeepClone();
                        break;
                    case "container":
                        RightHand = ItemTemplates.ItemsContainer[nIndex].DeepClone();
                        break;
                    case "junk":
                        RightHand = ItemTemplates.ItemsJunk[nIndex].DeepClone();
                        break;
                }
            }

            // left hand
            var leftHandNode = inventoryNode.Element("left-hand");
            if (leftHandNode != null)
            {
                var itemTypeNode = leftHandNode.Elements().First();
                int nIndex = int.Parse(itemTypeNode.Value);
                switch (itemTypeNode.Name.LocalName)
                {
                    case "weapon":
                        LeftHand = ItemTemplates.ItemsWeapon[nIndex].DeepClone();
                        break;
                    case "armor-shield":
                        LeftHand = ItemTemplates.ItemsArmorShield[nIndex].DeepClone();
                        break;
                    case "armor-head":
                        LeftHand = ItemTemplates.ItemsArmorHead[nIndex].DeepClone();
                        break;
                    case "armor-feet":
                        LeftHand = ItemTemplates.ItemsArmorFeet[nIndex].DeepClone();
                        break;
                    case "armor-chest":
                        LeftHand = ItemTemplates.ItemsArmorChest[nIndex].DeepClone();
                        break;
                    case "accessory-ring":
                        LeftHand = ItemTemplates.ItemsAccessoryRing[nIndex].DeepClone();
                        break;
                    case "accessory-amulet":
                        LeftHand = ItemTemplates.ItemsAccessoryAmulet[nIndex].DeepClone();
                        break;
                    case "drink":
                        LeftHand = ItemTemplates.ItemsDrink[nIndex].DeepClone();
                        break;
                    case "food":
                        LeftHand = ItemTemplates.ItemsFood[nIndex].DeepClone();
                        break;
                    case "container":
                        LeftHand = ItemTemplates.ItemsContainer[nIndex].DeepClone();
                        break;
                    case "junk":
                        LeftHand = ItemTemplates.ItemsJunk[nIndex].DeepClone();
                        break;
                }
            }

            // armor chest
            var armorChestNode = inventoryNode.Element("armor-chest");
            if (armorChestNode != null)
            {
                int nIndex = int.Parse(armorChestNode.Value);
                ArmorChest = ItemTemplates.ItemsArmorChest[nIndex].DeepClone();
            }

            // armor head
            var armorHeadNode = inventoryNode.Element("armor-head");
            if (armorHeadNode != null)
            {
                int nIndex = int.Parse(armorHeadNode.Value);
                ArmorHead = ItemTemplates.ItemsArmorHead[nIndex].DeepClone();
            }

            // armor feet
            var armorFeetNode = inventoryNode.Element("armor-feet");
            if (armorFeetNode != null)
            {
                int nIndex = int.Parse(armorFeetNode.Value);
                ArmorFeet = ItemTemplates.ItemsArmorFeet[nIndex].DeepClone();
            }

            // backpack
            var backpackNode = inventoryNode.Element("backpack");
            if (backpackNode != null)
            {
                int nIndex = int.Parse(backpackNode.Value);
                Backpack = ItemTemplates.ItemsContainer[nIndex].DeepClone();
            }

            // ring 1
            var ring1Node = inventoryNode.Element("ring1");
            if (ring1Node != null)
            {
                int nIndex = int.Parse(ring1Node.Value);
                Ring1 = ItemTemplates.ItemsAccessoryRing[nIndex].DeepClone();
            }

            // ring 2
            var ring2Node = inventoryNode.Element("ring2");
            if (ring2Node != null)
            {
                int nIndex = int.Parse(ring2Node.Value);
                Ring2 = ItemTemplates.ItemsAccessoryRing[nIndex].DeepClone();
            }

            // amulet
            var amuletNode = inventoryNode.Element("amulet");
            if (amuletNode != null)
            {
                int nIndex = int.Parse(amuletNode.Value);
                Amulet = ItemTemplates.ItemsAccessoryAmulet[nIndex].DeepClone();
            }

            // gold
            var goldNode = inventoryNode.Element("gold");
            if (goldNode != null)
            {
                Gold = int.Parse(goldNode.Value);
            }
        }

        #endregion
        #region Action Handlers

        private Handler DoAction()
        {
            if (IsDead) { return Handler.HANDLED; }

            Handler handler = Handler.UNHANDLED;
            TranslatedInput input = null;

            Random r = new Random(DateTime.Now.Millisecond);
            int percentage = r.Next(100);

            foreach(EntityNPCBehavior behavior in Behavior)
            {
                if(percentage < behavior.PercentageChance)
                {
                    // TODO: method for converting ACTION_ENUM directly to Do method
                    // dictionary<action_enum, delegate>? possible?
                    //handler = 
                }
            }

            switch (r.Next(10))
            {
                case 0:
                    return DoLook(input);
                case 1:
                    return DoGet(input);
                case 2:
                    return DoDrop(input);
                case 3:
                    return DoShowItem(input);
                case 4:
                    return DoMoveConnection(input);
                case 5:
                    return DoAttack(input);
                default:
                    return DoMoveBasic(input);
            }
        }
        public override Handler DoMoveBasic(TranslatedInput input)
        {
            Handler handler = Handler.HANDLED;

            ExitWithDirection exit = CurrentRoom.GetRandomExit();

            // npc is leaving player's room
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                handler = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_LEAVES, Name, exit.Direction));
            }

            CurrentRoom.NPCs.Remove(this);
            SetCurrentRoom(exit.Exit.Region, exit.Exit.Subregion, exit.Exit.Room);
            CurrentRoom.NPCs.Add(this);

            // npc has moved to player's room
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                handler = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_ARRIVES, Name));
            }

            return handler;
        }
        public override Handler DoMoveConnection(TranslatedInput input)
        {
            if (Posture == ENTITY_POSTURE.SITTING) { return Handler.UNHANDLED; }
            if (Posture == ENTITY_POSTURE.KNEELING) { return Handler.UNHANDLED; }

            Handler handler = Handler.UNHANDLED;
            Connection connection = CurrentRoom.GetRandomConnection();
            if (connection == null) { return DoMoveBasic(input); }

            // TODO: connections need a special display string for NPC exit (and arrival?)

            CurrentRoom.NPCs.Remove(this);
            if(CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                // npc is leaving player's current room
                handler = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_LEAVES, Name, "through a connection"));
            }
            
            // set current room
            SetCurrentRoom(connection);

            // npc has moved to player's room
            CurrentRoom.NPCs.Add(this);
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                handler = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_ARRIVES, Name));
            }

            return handler;
        }
        public Handler DoShowItem(TranslatedInput input)
        {
            if (RightHand == null && LeftHand == null) { return DoGet(input); }
            if (RightHand != null && CurrentRoom.Equals(Game.Player.CurrentRoom)) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_SHOW_ITEM, Name, RightHand.Name)); }
            else if (LeftHand != null && CurrentRoom.Equals(Game.Player.CurrentRoom)) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_SHOW_ITEM, Name, LeftHand.Name)); }

            return Handler.UNHANDLED;
        }
        public override Handler DoGet(TranslatedInput input)
        {
            // [possible] TODO: when npc picks up an item, add possible actions to some sort of list or flag field
            // - for example, when food is picked up, or when npc enters room with food, the npc can eat
            // duplicates are OK, if lefthand and righthand do similar things
            // room needs to keep track of which item types it has; possibly which actions npcs can do in it
            Item item = CurrentRoom.Items.GetRandomItem();
            if (item == null && CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.NPC_NO_ITEMS_IN_ROOM, Name));
            }

            if (HandsAreFull && CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.NPC_HANDS_ARE_FULL, Name, item.Name));
            }

            // item picked up; remove from room
            if (RightHand == null) { RightHand = item; }
            else if (LeftHand == null) { LeftHand = item; }
            CurrentRoom.Items.Remove(item);
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_GET, Name, item.Name));
            }

            return Handler.HANDLED;
        }
        public override Handler DoDrop(TranslatedInput input)
        {
            // TODO: see DoGet, remove possible actions from flag field

            // nothing to drop? pick up instead! just because
            if (RightHand == null && LeftHand == null) { return DoGet(input); }

            // if we make it here, the npc has SOMETHING to drop
            // randomly decide which hand to try first
            Random r = new Random(DateTime.Now.Millisecond);
            Handler handler = Handler.UNHANDLED;
            switch(r.Next(2))
            {
                case 0:
                    handler = DoDropRightHand();
                    if (handler.ReturnCode == RETURN_CODE.HANDLED) { return handler; }
                    else { handler = DoDropLeftHand(); }
                    break;
                case 1:
                    handler = DoDropLeftHand();
                    if (handler.ReturnCode == RETURN_CODE.HANDLED) { return handler; }
                    else { handler = DoDropRightHand(); }
                    break;
            }

            return handler;
        }
        private Handler DoDropRightHand()
        {
            if (RightHand == null) { return Handler.UNHANDLED; }
            CurrentRoom.Items.Add(RightHand);
            Handler handler = Handler.HANDLED;
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                handler = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_DROP, Name, RightHand.Name));
            }
            RightHand = null;
            return handler;
        }
        private Handler DoDropLeftHand()
        {
            if (LeftHand == null) { return Handler.UNHANDLED; }
            CurrentRoom.Items.Add(LeftHand);
            Handler handler = Handler.HANDLED;
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                handler = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_DROP, Name, LeftHand.Name));
            }
            LeftHand = null;
            return handler;
        }
        public override Handler DoLook(TranslatedInput input)
        {
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_LOOK, Name));
            }

            return Handler.HANDLED;
        }
        public override Handler DoAttack(TranslatedInput input)
        {
            EntityPlayer p = Game.Player;
            if (!CurrentRoom.Equals(p.CurrentRoom)) { return Handler.UNHANDLED; }
            if (p.IsDead) { return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_ATTACKS_DEAD_PLAYER, Name)); }

            // if RightHand is holding a non-weapon
            // TODO: LeftHand?
            ItemWeapon weapon = null;
            if (RightHand != null && RightHand.Type == ITEM_TYPE.WEAPON) { weapon = RightHand as ItemWeapon; }
            else if (LeftHand != null && LeftHand.Type == ITEM_TYPE.WEAPON) { weapon = LeftHand as ItemWeapon; }

            if(weapon == null && !HandsAreEmpty)
            {
                return new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.NPC_NOT_A_WEAPON, Name, RightHand.Name)); 
            }

            string strWeapon = weapon == null ? "fist" : weapon.Name;

            // calculate some damage
            MESSAGE_ENUM message = MESSAGE_ENUM.NPC_ATTACKS_PLAYER;
            int damage = 5;
            p.CurrentHealth -= damage;
            if (p.CurrentHealth <= 0) { message = MESSAGE_ENUM.NPC_KILLS_PLAYER; }

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(message, Name, strWeapon, damage.ToString()));
        }
        public override Handler DoEquip(TranslatedInput input)
        {
            return Handler.UNHANDLED;
        }
        public override Handler DoRemove(TranslatedInput input)
        {
            return Handler.UNHANDLED;
        }
        public override Handler DoRemove(ITEM_SLOT item)
        {
            return Handler.UNHANDLED;
        }

        #endregion

        public bool IsKeyword(string strWord)
        {
            foreach(string keyword in Keywords)
            {
                if (keyword == strWord) { return true; }
            }

            return false;
        }
        public Handler Update()
        {
            Handler handler = null;
            if(IsDead && Searched)
            {
                CurrentRoom.NPCs.Remove(this);
                return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.DEBUG_REMOVE, Name));
            }

            DateTime now = DateTime.Now;
            TimeSpan delta = now - LastActionTime;
            if(delta.TotalMilliseconds >= ActionPulse)
            {
                // check behavior
                LastActionTime = now;
                handler = DoAction();
            }

            return handler;
        }
    }
}