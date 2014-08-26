using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Xaml.Documents;

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
        public ENTITY_RELATIONSHIP_GROUP RelationshipGroup { get; set; }
        public override Paragraph HandsParagraph
        {
            get
            {
                Paragraph p = new Paragraph();

                if (LeftHand == null && RightHand == null)
                {
                    p.Inlines.Add("The ".ToRun());
                    p.Merge(NameAsParagraph);
                    p.Inlines.Add(" isn't holding anything.".ToRun());
                    return p;
                }

                if (RightHand == null) 
                {
                    p.Inlines.Add("The ".ToRun());
                    p.Merge(NameAsParagraph);
                    p.Inlines.Add("'s right hand is empty and ".ToRun()); 
                }
                else
                {
                    p.Inlines.Add("The ".ToRun());
                    p.Merge(NameAsParagraph);
                    p.Inlines.Add(" is holding ".ToRun());
                    p.Merge(RightHand.NameWithIndefiniteArticle);
                    p.Inlines.Add(" in its right hand".ToRun());
                }

                if (LeftHand == null) { p.Inlines.Add(".".ToRun()); }
                else
                {
                    if (RightHand == null)
                    {
                        p.Inlines.Add("The ".ToRun());
                        p.Merge(NameAsParagraph);
                        p.Inlines.Add(" is holding ".ToRun());
                    }
                    else
                    {
                        p.Inlines.Add(" and ".ToRun());
                    }

                    p.Merge(LeftHand.NameWithIndefiniteArticle);
                    p.Inlines.Add(" in its left hand.".ToRun());
                }

                return p;
            }
        }
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
                        return "The " + Name + " isn't wearing anything special.";
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

                strReturn += ".";
                return strReturn;
            }
        }
        public override Paragraph InventoryParagraph
        {
            get
            {
                List<Run> inventory = new List<Run>();

                if (Backpack != null)
                {
                    inventory.Add(Backpack.NameIndefiniteArticle.ToRun());
                    inventory.Add(Backpack.NameAsRun);
                }
                if (ArmorChest != null)
                {
                    inventory.Add(ArmorChest.NameIndefiniteArticle.ToRun());
                    inventory.Add(ArmorChest.NameAsRun);
                }
                if (ArmorFeet != null)
                {
                    inventory.Add(ArmorFeet.NameIndefiniteArticle.ToRun());
                    inventory.Add(ArmorFeet.NameAsRun);
                }
                if (ArmorHead != null)
                {
                    inventory.Add(ArmorHead.NameIndefiniteArticle.ToRun());
                    inventory.Add(ArmorHead.NameAsRun);
                }
                if (Ring1 != null)
                {
                    inventory.Add(Ring1.NameIndefiniteArticle.ToRun());
                    inventory.Add(Ring1.NameAsRun);
                }
                if (Ring2 != null)
                {
                    inventory.Add(Ring2.NameIndefiniteArticle.ToRun());
                    inventory.Add(Ring2.NameAsRun);
                }
                if (Amulet != null)
                {
                    inventory.Add(Amulet.NameIndefiniteArticle.ToRun());
                    inventory.Add(Amulet.NameAsRun);
                }

                Paragraph p = new Paragraph();
                p.Inlines.Add("The ".ToRun());
                p.Merge(NameAsParagraph);
                p.Inlines.Add(" is wearing ".ToRun());

                switch (inventory.Count / 2)
                {
                    case 0:
                        p.Inlines.Add("The ".ToRun());
                        p.Merge(NameAsParagraph);
                        p.Inlines.Add(" isn't wearing anything special.".ToRun());
                        return p;
                    case 1:
                        p.Inlines.Add(inventory[0]);
                        p.Inlines.Add(inventory[1]);
                        break;
                    case 2:
                        p.Inlines.Add(inventory[0]);
                        p.Inlines.Add(inventory[1]);
                        p.Inlines.Add((" and ").ToRun());
                        p.Inlines.Add(inventory[2]);
                        p.Inlines.Add(inventory[3]);
                        break;
                    default:
                        for (int i = 0; i < inventory.Count - 2; i += 2)
                        {
                            p.Inlines.Add(inventory[i]);
                            p.Inlines.Add(inventory[i + 1]);
                            p.Inlines.Add((", ").ToRun());
                        }

                        p.Inlines.Add(("and ").ToRun());
                        p.Inlines.Add(inventory[inventory.Count - 2]);
                        p.Inlines.Add(inventory[inventory.Count - 1]);
                        break;
                }

                p.Inlines.Add((".").ToRun());
                return p;
            }
        }
        public static List<EntityNPCBehavior> Behavior = new List<EntityNPCBehavior>();

        public override string Name
        {
            get
            {
                return base.Name + " (" + NID.ToString() + ") (" + CurrentHealth.ToString() + ":" + MaximumHealth.ToString() + ") (" + RelationshipGroup.ToString() + ")";
            }
            set
            {
                base.Name = value;
            }
        }
        public override string NameBase
        {
            get
            {
                return base.NameBase + " (" + NID.ToString() + ") (" + CurrentHealth.ToString() + ":" + MaximumHealth.ToString() + ")";
            }
        }

        #endregion
        #region Constructors

        public EntityNPC() : base() { }
        public EntityNPC(EntityNPCTemplate template) : base() 
        {
            // TODO: move ActionPulse to .xml
            ActionPulse = 8000 + Statics.r.Next(15000);
            NID = Statics.EntityCount++;
            ID = template.ID;
            RelationshipGroup = template.RelationshipGroup;
            Name = template.Name;
            Gold = template.Gold;
            MaximumHealth = template.MaximumHealth;
            CurrentHealth = template.CurrentHealth;
            if (template.RightHand != null) { RightHand = template.RightHand.DeepClone(); }
            if (template.LeftHand != null) { LeftHand = template.LeftHand.DeepClone(); }
            if (template.ArmorChest != null) { ArmorChest = template.ArmorChest.DeepClone(); }
            if (template.ArmorHead != null) { ArmorHead = template.ArmorHead.DeepClone(); }
            if (template.ArmorFeet != null) { ArmorFeet = template.ArmorFeet.DeepClone(); }
            if (template.Backpack != null) { Backpack = template.Backpack.DeepClone(); }
            if (template.Ring1 != null) { Ring1 = template.Ring1.DeepClone(); }
            if (template.Ring2 != null) { Ring2 = template.Ring2.DeepClone(); }
            if (template.Amulet != null) { Amulet = template.Amulet.DeepClone(); }

            foreach (string keyword in template.Keywords)
            {
                Keywords.Add(keyword);
            }
        }

        #endregion
        #region Action Handlers

        private Handler DoAction()
        {
            // DEBUG
            // return Handler.HANDLED(MESSAGE_ENUM.NO_MESSAGE);
            // END DEBUG

            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.NO_MESSAGE); }

            Handler handler = Handler.UNHANDLED();
            TranslatedInput input = null;

            Random r = new Random(DateTime.Now.Millisecond);
            int percentage = r.Next(100);

            //foreach(EntityNPCBehavior behavior in Behavior)
            //{
            //    if(percentage < behavior.PercentageChance)
            //    {
            //        // TODO: method for converting ACTION_ENUM directly to Do method
            //        // dictionary<action_enum, delegate>? possible?
            //        //handler = 
            //    }
            //}

            //while (handler.ReturnCode == RETURN_CODE.UNHANDLED)
            //{
            //    switch(r.Next(2))
            //    {
            //        case 0:
            //            handler = DoGet(input);
            //            break;
            //        case 1:
            //            handler = DoEquip(input);
            //            break;
            //    }
            //}

            //return handler;

            // UNHANDLED means that the action couldn't be taken
            // actions can be HANDLED with no visible action
            while (handler.ReturnCode == RETURN_CODE.UNHANDLED)
            {
                switch (r.Next(10))
                {
                    case 0: handler = DoLook(input); break;
                    case 1: handler = DoGet(input); break;
                    case 2: handler = DoDrop(input); break;
                    case 3: handler = DoShowItem(input); break;
                    case 4: handler = DoMoveConnection(input); break;
                    case 5: handler = DoAttack(input); break;
                    case 6: handler = DoEquip(input); break;
                    default: handler = DoMoveBasic(input); break;
                }
            }

            return handler;
        }
        public override Handler DoMoveBasic(TranslatedInput input)
        {
            Handler handler = Handler.HANDLED(MESSAGE_ENUM.NO_MESSAGE);

            ExitWithDirection exit = CurrentRoom.Exits.RandomWithDirection();

            // npc is leaving player's room
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                handler = Handler.HANDLED(MESSAGE_ENUM.NPC_LEAVES, NameAsParagraph, exit.Direction.ToParagraph());
            }

            CurrentRoom.NPCs.Remove(this);
            SetCurrentRoom(exit.Exit.Region, exit.Exit.Subregion, exit.Exit.Room);
            CurrentRoom.NPCs.Add(this);

            // npc has moved to player's room
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                handler = Handler.HANDLED(MESSAGE_ENUM.NPC_ARRIVES, NameWithIndefiniteArticle(true));
            }

            return handler;
        }
        public override Handler DoMoveConnection(TranslatedInput input)
        {
            if (Posture == ENTITY_POSTURE.SITTING) { return Handler.UNHANDLED(); }
            if (Posture == ENTITY_POSTURE.KNEELING) { return Handler.UNHANDLED(); }

            Handler handler = Handler.UNHANDLED();
            Connection connection = CurrentRoom.Connections.Random();
            if (connection == null) { return DoMoveBasic(input); }

            // TODO: connections need a special display string for NPC exit (and arrival?)

            CurrentRoom.NPCs.Remove(this);
            if(CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                // npc is leaving player's current room
                handler = Handler.HANDLED(MESSAGE_ENUM.NPC_LEAVES_CONNECTION, NameAsParagraph, connection.NPCDisplayString.ToParagraph());
            }
            
            // set current room
            SetCurrentRoom(connection);

            // npc has moved to player's room
            CurrentRoom.NPCs.Add(this);
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                handler = Handler.HANDLED(MESSAGE_ENUM.NPC_ARRIVES, NameWithIndefiniteArticle(true));
            }

            return handler;
        }
        public Handler DoShowItem(TranslatedInput input)
        {
            if (RightHand == null && LeftHand == null) { return DoGet(input); }
            if (RightHand != null && CurrentRoom.Equals(Game.Player.CurrentRoom)) 
            {
                return Handler.HANDLED(MESSAGE_ENUM.NPC_SHOW_ITEM, NameAsParagraph, RightHand.NameAsParagraph);
            }
            else if (LeftHand != null && CurrentRoom.Equals(Game.Player.CurrentRoom)) 
            {
                return Handler.HANDLED(MESSAGE_ENUM.NPC_SHOW_ITEM, NameAsParagraph, LeftHand.NameAsParagraph);
            }

            return Handler.UNHANDLED();
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
                return Handler.HANDLED(MESSAGE_ENUM.ERROR_NPC_NO_ITEMS_IN_ROOM, NameAsParagraph);
            }

            if (HandsAreFull && CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                return Handler.HANDLED(MESSAGE_ENUM.ERROR_NPC_HANDS_ARE_FULL, NameAsParagraph, item.NameWithIndefiniteArticle);
            }

            // item picked up; remove from room
            if (RightHand == null) { RightHand = item; }
            else if (LeftHand == null) { LeftHand = item; }
            CurrentRoom.Items.Remove(item);
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                return Handler.HANDLED(MESSAGE_ENUM.NPC_GET, NameAsParagraph, item.NameWithIndefiniteArticle);
            }

            return Handler.HANDLED(MESSAGE_ENUM.NO_MESSAGE);
        }
        public override Handler DoDrop(TranslatedInput input)
        {
            // TODO: see DoGet, remove possible actions from flag field

            // nothing to drop? pick up instead! just because
            if (RightHand == null && LeftHand == null) { return DoGet(input); }

            // if we make it here, the npc has SOMETHING to drop
            // randomly decide which hand to try first
            Random r = new Random(DateTime.Now.Millisecond);
            Handler handler = null;
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
            if (RightHand == null) { return Handler.UNHANDLED(); }
            CurrentRoom.Items.Add(RightHand);
            Handler handler = Handler.HANDLED(MESSAGE_ENUM.NO_MESSAGE);
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                handler = Handler.HANDLED(MESSAGE_ENUM.NPC_DROP, NameAsParagraph, RightHand.NameWithIndefiniteArticle);
            }
            RightHand = null;
            return handler;
        }
        private Handler DoDropLeftHand()
        {
            if (LeftHand == null) { return Handler.UNHANDLED(); }
            CurrentRoom.Items.Add(LeftHand);
            Handler handler = Handler.HANDLED(MESSAGE_ENUM.NO_MESSAGE);
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                handler = Handler.HANDLED(MESSAGE_ENUM.NPC_DROP, NameAsParagraph, LeftHand.NameWithIndefiniteArticle);
            }
            LeftHand = null;
            return handler;
        }
        public override Handler DoLook(TranslatedInput input)
        {
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                return Handler.HANDLED(MESSAGE_ENUM.NPC_LOOK, NameAsParagraph);
            }

            return Handler.HANDLED(MESSAGE_ENUM.NO_MESSAGE);
        }

        public override Handler DoAttack(TranslatedInput input)
        {
            Handler handler = DoAttackPlayer(input);
            if (handler.ReturnCode == RETURN_CODE.UNHANDLED) { handler = DoAttackNPC(input); }
            if (handler.ReturnCode == RETURN_CODE.UNHANDLED) { return DoMoveBasic(input); }
            return handler;
        }
        public Handler DoAttackNPC(TranslatedInput input)
        {
            // TODO: allow for retrieval of dead hostiles?
            // if so, need different action
            EntityNPC hostile = CurrentRoom.GetRandomHostile(this, true);
            if (hostile == null) { return Handler.UNHANDLED(); }

            if (hostile.IsDead && CurrentRoom.Equals(Game.Player.CurrentRoom)) { return Handler.HANDLED(MESSAGE_ENUM.NPC_ATTACKS_DEAD_NPC, this.NameAsParagraph, hostile.NameAsParagraph); }

            // if RightHand is holding a non-weapon
            // TODO: LeftHand?
            ItemWeapon weapon = null;
            if (RightHand != null && RightHand.Type == ITEM_TYPE.WEAPON) { weapon = RightHand as ItemWeapon; }
            else if (LeftHand != null && LeftHand.Type == ITEM_TYPE.WEAPON) { weapon = LeftHand as ItemWeapon; }

            if (weapon == null && RightHand != null)
            {
                return Handler.HANDLED(MESSAGE_ENUM.ERROR_NPC_ATTACKS_NPC_BAD_WEAPON, NameAsParagraph, hostile.NameAsParagraph, RightHand.NameWithIndefiniteArticle);
            }

            Paragraph pWeapon = weapon == null ? "fist".ToParagraph() : weapon.NameAsParagraph;

            // calculate some damage
            MESSAGE_ENUM message = MESSAGE_ENUM.NPC_ATTACKS_NPC;
            int damage = AttackPower - hostile.DefensePower; // 2;
            hostile.CurrentHealth -= damage;
            if (hostile.CurrentHealth <= 0) { message = MESSAGE_ENUM.NPC_KILLS_NPC; }

            if (!CurrentRoom.Equals(Game.Player.CurrentRoom)) { return Handler.HANDLED(MESSAGE_ENUM.NO_MESSAGE); }
            return Handler.HANDLED(message, NameAsParagraph, hostile.NameBaseAsParagraph, pWeapon, damage.ToString().ToParagraph());
        }
        public Handler DoAttackPlayer(TranslatedInput input)
        {
            EntityPlayer p = Game.Player;
            if (!CurrentRoom.Equals(p.CurrentRoom)) { return Handler.UNHANDLED(); }

            if (p.IsDead) 
            {
                return Handler.HANDLED(MESSAGE_ENUM.NPC_ATTACKS_DEAD_PLAYER, NameAsParagraph);
            }

            // if RightHand is holding a non-weapon
            // TODO: LeftHand?
            ItemWeapon weapon = null;
            if (RightHand != null && RightHand.Type == ITEM_TYPE.WEAPON) { weapon = RightHand as ItemWeapon; }
            else if (LeftHand != null && LeftHand.Type == ITEM_TYPE.WEAPON) { weapon = LeftHand as ItemWeapon; }

            if(weapon == null && RightHand != null)
            {
                return Handler.HANDLED(MESSAGE_ENUM.ERROR_NPC_ATTACKS_PLAYER_BAD_WEAPON, NameAsParagraph, RightHand.NameWithIndefiniteArticle);
            }

            Paragraph pWeapon = weapon == null ? "fist".ToParagraph() : weapon.NameAsParagraph;

            // calculate some damage
            MESSAGE_ENUM message = MESSAGE_ENUM.NPC_ATTACKS_PLAYER;
            int damage = 5;
            p.CurrentHealth -= damage;
            if (p.CurrentHealth <= 0) { message = MESSAGE_ENUM.NPC_KILLS_PLAYER; }

            // TODO: remove "" for Messages.GetMessage on cleanup
            return Handler.HANDLED(message, NameAsParagraph, pWeapon, damage.ToString().ToParagraph());
        }
        public override Handler DoEquip(TranslatedInput input)
        {
            Handler handler = DoEquipRightHand();
            if (handler.ReturnCode == RETURN_CODE.UNHANDLED) { handler = DoEquipLeftHand(); }
            return handler;
        }
        public Handler DoEquipRightHand()
        {
            if (RightHand == null) { return Handler.UNHANDLED(); }

            switch(RightHand.Type)
            {
                case ITEM_TYPE.ACCESSORY_AMULET:
                    if (Amulet != null) { return Handler.UNHANDLED(); }
                    Amulet = RightHand as ItemAccessoryAmulet;
                    break;
                case ITEM_TYPE.ACCESSORY_RING:
                    if (Ring1 == null) { Ring1 = RightHand as ItemAccessoryRing; }
                    else if (Ring2 == null) { Ring2 = RightHand as ItemAccessoryRing; }
                    else { return Handler.UNHANDLED(); }
                    break;
                case ITEM_TYPE.ARMOR_CHEST:
                    if (ArmorChest != null) { return Handler.UNHANDLED(); }
                    ArmorChest = RightHand as ItemArmorChest;
                    break;
                case ITEM_TYPE.ARMOR_FEET:
                    if (ArmorFeet != null) { return Handler.UNHANDLED(); }
                    ArmorFeet = RightHand as ItemArmorFeet;
                    break;
                case ITEM_TYPE.ARMOR_HEAD:
                    if (ArmorHead != null) { return Handler.UNHANDLED(); }
                    ArmorHead = RightHand as ItemArmorHead;
                    break;
                case ITEM_TYPE.CONTAINER:
                    if (Backpack != null) { return Handler.UNHANDLED(); }
                    Backpack = RightHand as ItemContainer;
                    break;
                default:
                    return Handler.UNHANDLED();
            }

            Handler handler = Handler.HANDLED(MESSAGE_ENUM.NO_MESSAGE);

            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                handler = Handler.HANDLED(MESSAGE_ENUM.NPC_EQUIP, NameAsParagraph, RightHand.NameWithIndefiniteArticle);
            }

            RightHand = null;
            return handler;
        }
        public Handler DoEquipLeftHand()
        {
            if (LeftHand == null) { return Handler.UNHANDLED(); }

            switch (LeftHand.Type)
            {
                case ITEM_TYPE.ACCESSORY_AMULET:
                    if (Amulet != null) { return Handler.UNHANDLED(); }
                    Amulet = LeftHand as ItemAccessoryAmulet;
                    break;
                case ITEM_TYPE.ACCESSORY_RING:
                    if (Ring1 == null) { Ring1 = LeftHand as ItemAccessoryRing; }
                    else if (Ring2 == null) { Ring2 = LeftHand as ItemAccessoryRing; }
                    else { return Handler.UNHANDLED(); }
                    break;
                case ITEM_TYPE.ARMOR_CHEST:
                    if (ArmorChest != null) { return Handler.UNHANDLED(); }
                    ArmorChest = LeftHand as ItemArmorChest;
                    break;
                case ITEM_TYPE.ARMOR_FEET:
                    if (ArmorFeet != null) { return Handler.UNHANDLED(); }
                    ArmorFeet = LeftHand as ItemArmorFeet;
                    break;
                case ITEM_TYPE.ARMOR_HEAD:
                    if (ArmorHead != null) { return Handler.UNHANDLED(); }
                    ArmorHead = LeftHand as ItemArmorHead;
                    break;
                case ITEM_TYPE.CONTAINER:
                    if (Backpack != null) { return Handler.UNHANDLED(); }
                    Backpack = LeftHand as ItemContainer;
                    break;
                default:
                    return Handler.UNHANDLED();
            }

            Handler handler = CurrentRoom.Equals(Game.Player.CurrentRoom) ? 
                Handler.HANDLED(MESSAGE_ENUM.NPC_EQUIP, NameAsParagraph, LeftHand.NameWithIndefiniteArticle) : 
                Handler.HANDLED(MESSAGE_ENUM.NO_MESSAGE);

            //Handler handler = Handler.HANDLED(MESSAGE_ENUM.NO_MESSAGE);

            //if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            //{
            //    handler = Handler.HANDLED(MESSAGE_ENUM.NPC_EQUIP, NameAsParagraph, LeftHand.NameWithIndefiniteArticle);
            //}

            LeftHand = null;
            return handler;
        }
        public override Handler DoRemove(TranslatedInput input)
        {
            return Handler.UNHANDLED();
        }
        public override Handler DoRemove(ITEM_SLOT item)
        {
            return Handler.UNHANDLED();
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
                return Handler.HANDLED(MESSAGE_ENUM.DEBUG_REMOVE, NameAsParagraph);
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