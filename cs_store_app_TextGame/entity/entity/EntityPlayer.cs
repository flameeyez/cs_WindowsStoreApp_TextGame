using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    public class EntityPlayer : EntityBase
    {
        public int Experience { get; set; }
        public EntityPlayer() : base() 
        {
            Hands.Add(new EntityHand());
            Hands.Add(new EntityHand());

            Body.BodyParts.Add(new EntityBodyPartChest());
            Body.BodyParts.Add(new EntityBodyPartHead());
            Body.BodyParts.Add(new EntityBodyPartFeet());
            Body.BodyParts.Add(new EntityBodyPartNeck());
            Body.BodyParts.Add(new EntityBodyPartFinger());
            Body.BodyParts.Add(new EntityBodyPartFinger());

            ContainerSlots.ContainerSlots.Add(new EntityContainerSlotBackpack());
            ContainerSlots.ContainerSlots.Add(new EntityContainerSlotPouch());
        }

        #region Display Runs, Paragraphs
        public override Paragraph InventoryParagraph
        {
            get
            {
                Paragraph p = new Paragraph();
                List<Run> inventory = new List<Run>();

                foreach (EntityBodyPart bodyPart in Body.BodyParts)
                {
                    if (bodyPart.Item != null)
                    {
                        inventory.Add(bodyPart.Item.NameIndefiniteArticle.ToRun());
                        inventory.Add(bodyPart.Item.NameAsRun);
                    }
                }

                foreach(EntityContainerSlot slot in ContainerSlots.ContainerSlots)
                {
                    if (slot.Container != null)
                    {
                        inventory.Add(slot.Container.NameIndefiniteArticle.ToRun());
                        inventory.Add(slot.Container.NameAsRun);
                    }
                }

                p.Inlines.Add(("You are wearing ").ToRun());

                switch (inventory.Count / 2)
                {
                    case 0:
                        p.Inlines.Clear();
                        p.Inlines.Add(("You aren't wearing anything!").ToRun());
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
        #endregion
        #region Input Handling
        public override Handler DoAttack(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_WHAT, input.Words[0].ToSentenceCase().ToParagraph()); }
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }

            string strNPCName = "";
            int ordinal = 0;
            if (input.Words.Length == 2) { strNPCName = input.Words[1]; }
            else if (input.Words.Length == 3)
            {
                if (!Statics.OrdinalStringToInt.TryGetValue(input.Words[1], out ordinal)) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
                strNPCName = input.Words[2];
            }

            EntityNPCBase npc = CurrentRoom.NPCs.FindLiving(strNPCName, ordinal);
            if (npc == null) { npc = CurrentRoom.NPCs.Find(strNPCName, ordinal); }
            if (npc == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            if (npc.IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_NPC_ALREADY_DEAD, npc.NameBaseAsParagraph); }

            Item weapon = Hands.GetAnyItem(ITEM_TYPE.WEAPON);
            // TODO: fix this
            if (weapon == null)
            {
                if (Hands.Hands[0].Item != null)
                {
                    return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_ATTACKS_BAD_WEAPON, Hands.Hands[0].Item.NameAsParagraph);
                }
                else if(Hands.Hands[1].Item != null)
                {
                    return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_ATTACKS_BAD_WEAPON, Hands.Hands[1].Item.NameAsParagraph);
                }
            }

            Paragraph pWeapon = weapon == null ? "fist".ToParagraph() : weapon.NameAsParagraph;

            // calculate damage
            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_ATTACKS_NPC;
            int damage = AttackPower - npc.Body.DefensePower;
            npc.Attributes.CurrentHealth -= damage;
            if (npc.IsDead) 
            {
                Paragraph xpPara = Game.Player.ProcessExperience(npc);
                return Handler.HANDLED(MESSAGE_ENUM.PLAYER_KILLS_NPC, npc.NameBaseAsParagraph, pWeapon, damage.ToString().ToParagraph(), xpPara);
            }

            return Handler.HANDLED(message, npc.NameBaseAsParagraph, pWeapon, damage.ToString().ToParagraph());
        }

        public Paragraph ProcessExperience(EntityNPCBase npc)
        {
            int experience = Statics.LevelDeltaToExperience(npc.Level);
            Game.Player.Experience += experience;

            Paragraph p = new Paragraph();
            
            string strExperience = "You have gained " + experience.ToString() + " experience.";
            p.Inlines.Add(strExperience.ToRun());

            // TODO: replace hard-coded 1000?
            if(Game.Player.Experience > Game.Player.Level * 1000)
            {
                Game.Player.GainLevel();
                string strLevelUp = "You have gained a level! You are now level " + Game.Player.Level.ToString();
                p.Inlines.Add(strLevelUp.ToRun());
            }

            return p;
        }

        public void GainLevel()
        {
            // TODO: update with attribute gains
            Level++;
        }
        public override Handler DoMoveBasic(TranslatedInput input)
        {
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }
            if (Posture == ENTITY_POSTURE.SITTING) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_SITTING); }
            if (Posture == ENTITY_POSTURE.KNEELING) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_KNEELING); }

            // basic direction
            // if "go <direction>"
            int nDirection;
            if (input.Words[0] == "go" || input.Words[0] == "move") { nDirection = Statics.DirectionToInt(input.Words[1]); }
            // see TranslatedInput constructor; hack job replaces first word with integer direction
            else { nDirection = int.Parse(input.Words[0]); }

            Exit exit = CurrentRoom.Exits.Get(nDirection);
            if (exit.Region == -1)
            {
                return Handler.HANDLED(MESSAGE_ENUM.ERROR_WRONG_DIRECTION);
            }

            Coordinates.Set(exit.Region, exit.Subregion, exit.Room);

            return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, Coordinates.CurrentRoomDisplayParagraph);
        }
        public override Handler DoMoveConnection(TranslatedInput input)
        {
            // TODO: since a Connection can have ANY action verb, consider trying to process any fall-through input
            if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_GO_WHERE); }
            if (input.Words.Length > 2) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }

            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }
            if (Posture == ENTITY_POSTURE.SITTING) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_SITTING); }
            if (Posture == ENTITY_POSTURE.KNEELING) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_KNEELING); }

            // go <direction>
            if (Statics.DirectionToInt(input.Words[1]) != -1) { return DoMoveBasic(input); }

            // go <connection>
            Connection connection = CurrentRoom.Connections.Find(input.Words[0], input.Words[1]);
            if (connection != null)
            {
                Coordinates.Set(connection);
                input = null;
                return DoLook(input);
            }

            return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT);
        }
        public override Handler DoLook(TranslatedInput input)
        {
            // "look"
            if (input == null || input.Words.Length < 2) { return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, Coordinates.CurrentRoomDisplayParagraph); }
            // look <item|npc>
            else if (input.Words.Length == 2) { return DoLook(input.Words[1]); }
            // look <at|in|my|ordinal> <item|npc>
            else if (input.Words.Length == 3) { return DoLook(input.Words[1], input.Words[2]); }
            // look <at|in> <my|ordinal> <item|npc>
            else if (input.Words.Length == 4) { return DoLook(input.Words[1], input.Words[2], input.Words[3]); }
            // handling a five-word phrase seems impractical
            return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT);
        }
        protected override Handler DoLook(string strWord)
        {
            // look hands - special case
            if (strWord == "hands") { return DoLookHands(null); }

            // TODO: consider more specific strings for held or equipped items
            Item item = CurrentRoom.Items.Find(strWord);
            if (item != null) { return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, (item.Description).ToParagraph()); }

            item = Hands.GetItem(strWord, false);
            if (item != null) { return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, (item.Description).ToParagraph()); }

            item = Body.GetItem(strWord, false);
            if (item != null) { return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, (item.Description).ToParagraph()); }

            item = ContainerSlots.GetContainer(strWord); //GetItem(strWord, false);
            if (item != null) { return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, (item.Description).ToParagraph()); }

            EntityNPCBase npc = CurrentRoom.NPCs.Find(strWord);
            if (npc != null) { return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, npc.LookParagraph); }

            return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT);
        }
        protected override Handler DoLook(string strWord1, string strWord2)
        {
            if (strWord1 == "at") { return DoLook(strWord2); }
            if (strWord1 == "in") { return DoLookInContainer(strWord2); }

            // TODO: handle "my"
            // - look [at] my backpack, "at" is implied--not "in"
            // can't simply call strWord1 version; needs to be specific for "my"
            if (strWord1 == "my") { return Handler.HANDLED(MESSAGE_ENUM.ERROR_NEED_TO_IMPLEMENT); }

            // if we make it here, we expect "look <ordinal> <item||npc>"
            // TODO: support "look at (the?) second goblin"
            
            int ordinal;
            if (!Statics.OrdinalStringToInt.TryGetValue(strWord1, out ordinal)) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }

            Item item = CurrentRoom.Items.Find(strWord2, ITEM_TYPE.ANY, ordinal);
            if (item != null) { return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, (item.Description).ToParagraph()); }

            EntityNPCBase npc = CurrentRoom.NPCs.Find(strWord2, ordinal);
            if (npc != null) { return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, npc.LookParagraph); }

            return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT);
        }
        protected override Handler DoLook(string strWord1, string strWord2, string strWord3)
        {
            // TODO: strWord2 could be ordinal OR "my"
            if (strWord1 == "at") { return DoLook(strWord2, strWord3); }
            if (strWord1 == "in")
            {
                int ordinal;
                if (Statics.OrdinalStringToInt.TryGetValue(strWord2, out ordinal)) { return DoLookInContainer(strWord3, ordinal); }
                else if (strWord2 == "my")
                {
                    // TODO: fix GetItem to look for BOTH types of container here
                    EntityHand hand = Hands.GetHandWithItem(strWord3);
                    if (hand.Item != null) 
                    {
                        if ((hand.Item.Type & ITEM_TYPE.CONTAINER_ANY) != hand.Item.Type) 
                        {
                            // matched input with a hand, but hand is not holding a container
                            return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); 
                        }

                        // hand is holding requested container
                        ItemContainer handContainer = hand.Item as ItemContainer;
                        if (handContainer.Closed) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_CONTAINER_CLOSED, handContainer.NameAsParagraph); }

                        return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, handContainer.ItemsParagraph);
                    }

                    // hands didn't work; check container slots
                    ItemContainer equippedContainer = ContainerSlots.GetContainer(strWord3);
                    if(equippedContainer != null)
                    {
                        if (equippedContainer.Closed) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_CONTAINER_CLOSED, equippedContainer.NameAsParagraph); }
                        return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, equippedContainer.ItemsParagraph);
                    }

                    return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT);
                }
            }
            return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT);
        }
        public override Handler DoLookHands(TranslatedInput input)
        {
            if (input != null && input.Words.Length > 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, Hands.PlayerDisplayParagraph);
        }
        public override Handler DoLookInContainer(string strKeyword, int ordinal = 0)
        {
            // priority: room, hand, equipped backpack
            // TODO: ensure consistent priority throughout
            // TODO: "my"
            //ItemContainer container = CurrentRoom.Items.Find(strKeyword, ITEM_TYPE.CONTAINER, ordinal) as ItemContainer;
            //if (container == null) { container = GetItemFromHand(strKeyword, false, ITEM_TYPE.CONTAINER) as ItemContainer; }
            //if (container == null && Backpack != null && Backpack.IsKeyword(strKeyword)) { container = Backpack; }
            //if (container == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }

            //// container found
            //if (container.Closed) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_CONTAINER_CLOSED, container.NameAsParagraph); }

            //return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, container.ItemsParagraph);

            // TODO: fix this
            throw new NotImplementedException();
        }
        public override Handler DoEat(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_WHAT, "Eat".ToParagraph()); }
            else if (input.Words.Length > 2) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }

            EntityHand hand = Hands.GetHandWithItem(input.Words[1], ITEM_TYPE.FOOD);
            if (hand != null)
            {
                MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_EAT;

                // hand is holding food
                ItemFood food = hand.Item as ItemFood;
                if (food.NumberOfBites == 1)
                {
                    message = MESSAGE_ENUM.PLAYER_EAT_LAST;
                    hand.Item = null;
                }
                else
                {
                    food.NumberOfBites--;
                }

                return Handler.HANDLED(message, food.NameAsParagraph, food.HealthPerBite.ToString().ToParagraph(), food.NumberOfBites.ToString().ToParagraph());
            }
            else
            {
                // not holding food; check room
                ItemFood food = CurrentRoom.Items.Find(input.Words[1], ITEM_TYPE.FOOD) as ItemFood;
                if(food != null)
                {
                    MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_EAT_GROUND_ITEM;

                    if (food.NumberOfBites == 1)
                    {
                        message = MESSAGE_ENUM.PLAYER_EAT_LAST_GROUND_ITEM;
                        hand.Item = null;
                    }
                    else
                    {
                        food.NumberOfBites--;
                    }

                    return Handler.HANDLED(message, food.NameAsParagraph, food.HealthPerBite.ToString().ToParagraph(), food.NumberOfBites.ToString().ToParagraph());
                }
                else
                {
                    return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT);
                }
            }            
        }
        public override Handler DoDrink(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_WHAT, "Drink".ToParagraph()); }
            else if (input.Words.Length > 2) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }

            EntityHand hand = Hands.GetHandWithItem(input.Words[1], ITEM_TYPE.DRINK);
            if (hand != null)
            {
                MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_DRINK;

                // hand is holding a drink
                ItemDrink drink = hand.Item as ItemDrink;
                if (drink.NumberOfDrinks == 1)
                {
                    message = MESSAGE_ENUM.PLAYER_DRINK_LAST;
                    hand.Item = null;
                }
                else
                {
                    drink.NumberOfDrinks--;
                }

                return Handler.HANDLED(message, drink.NameAsParagraph, drink.MagicPerDrink.ToString().ToParagraph(), drink.NumberOfDrinks.ToString().ToParagraph());
            }
            else
            {
                // not holding a drink; check room
                ItemDrink drink = CurrentRoom.Items.Find(input.Words[1], ITEM_TYPE.DRINK) as ItemDrink;
                if (drink != null)
                {
                    MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_DRINK_GROUND_ITEM;

                    if (drink.NumberOfDrinks == 1)
                    {
                        message = MESSAGE_ENUM.PLAYER_DRINK_LAST_GROUND_ITEM;
                        CurrentRoom.Items.Remove(drink);
                    }
                    else
                    {
                        drink.NumberOfDrinks--;
                    }

                    return Handler.HANDLED(message, drink.NameAsParagraph, drink.MagicPerDrink.ToString().ToParagraph(), drink.NumberOfDrinks.ToString().ToParagraph());
                }
                else
                {
                    return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT);
                }
            }
        }
        public override Handler DoOpen(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_WHAT, "Open".ToParagraph()); }
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }

            ItemContainer container = ContainerSlots.GetContainer(input.Words[1]);
            if (container == null) { container = Hands.GetItem(input.Words[1], false, ITEM_TYPE.CONTAINER_ANY) as ItemContainer; }
            if (container == null) { container = CurrentRoom.Items.Find(input.Words[1], ITEM_TYPE.CONTAINER_ANY) as ItemContainer; }
            if (container == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_ITEM); }

            if (!container.Closed) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_CONTAINER_ALREADY_OPEN, container.NameAsParagraph); }

            container.Closed = false;
            return Handler.HANDLED(MESSAGE_ENUM.PLAYER_OPEN, container.NameAsParagraph);
        }
        public override Handler DoClose(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_WHAT, "Close".ToParagraph()); }
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }
           
            // TODO: close MY <container>
            //if (input.Words.Length == 3 && input.Words[1] == "my") { return Handler.HANDLED(MESSAGE_ENUM.ERROR_NEED_TO_IMPLEMENT); }

            ItemContainer container = ContainerSlots.GetContainer(input.Words[1]);
            if (container == null) { container = Hands.GetItem(input.Words[1], false, ITEM_TYPE.CONTAINER_ANY) as ItemContainer; }
            if (container == null) { container = CurrentRoom.Items.Find(input.Words[1], ITEM_TYPE.CONTAINER_ANY) as ItemContainer; }
            if (container == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_ITEM); }
            
            if (container.Closed) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_CONTAINER_ALREADY_CLOSED, container.NameAsParagraph); }
            if (!container.Closable) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_CONTAINER_NOT_CLOSABLE, container.NameAsParagraph); }

            container.Closed = true;
            return Handler.HANDLED(MESSAGE_ENUM.PLAYER_CLOSE, container.NameAsParagraph);
        }
        public override Handler DoPut(TranslatedInput input)
        {
            // TODO: put <item> on <surface>
            //// put <item> in <container>
            
            if (input.Words.Length < 4) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            if (input.Words[2] != "in") { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }

            // TODO: put <item> in MY <container>
            // if (input.Words.Length == 5 && input.Words[3] == "my") { return Handler.HANDLED(MESSAGE_ENUM.ERROR_NEED_TO_IMPLEMENT); }

            // must be holding item
            // don't remove item here
            Item item = Hands.GetItem(input.Words[1], false);
            if (item == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_NOT_CARRYING_ITEM); }

            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_PUT_IN_PLAYER_CONTAINER;
            ItemContainer container = ContainerSlots.GetContainer(input.Words[3]);
            if (container == null) { container = Hands.GetItem(input.Words[3], false, ITEM_TYPE.CONTAINER_ANY) as ItemContainer; }
            if (container == null || container.Equals(item)) 
            {
                message = MESSAGE_ENUM.PLAYER_PUT_IN_GROUND_CONTAINER;
                container = CurrentRoom.Items.Find(input.Words[3], ITEM_TYPE.CONTAINER_ANY) as ItemContainer; 
            }
            if (container == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }

            if (container.Closed) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_CONTAINER_CLOSED, container.NameAsParagraph); }

            item = Hands.GetItem(input.Words[1], true);
            container.Items.Add(item);
            return Handler.HANDLED(message, item.NameAsParagraph, container.NameAsParagraph);
        }
        public override Handler DoEquip(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_WHAT, "Equip".ToParagraph()); }
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }

            string strItemToEquip = input.Words[1];
            // equip my <item>; strip 'my'
            if (input.Words.Length == 3 && input.Words[1] == "my") { strItemToEquip = input.Words[2]; }

            EntityHand hand = Hands.GetHandWithItem(strItemToEquip);
            if (hand == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            if ((hand.Item.Type & ITEM_TYPE.ARMOR_ANY) == hand.Item.Type) { return Body.DoEquip(hand); }
            if ((hand.Item.Type & ITEM_TYPE.CONTAINER_ANY) == hand.Item.Type) { return ContainerSlots.DoEquip(hand); }

            return Handler.HANDLED(MESSAGE_ENUM.ERROR_ITEM_NOT_EQUIPPABLE);
        }
        public override Handler DoRemove(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_WHAT, "Remove".ToParagraph()); }
            if (input.Words.Length > 2) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }

            EntityHand hand = Hands.GetEmptyHand();
            if (hand == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_HANDS_ARE_FULL); }

            REMOVE_RESULT result = Body.DoRemove(input.Words[1], hand);
            if (result == REMOVE_RESULT.NOT_REMOVED) { result = ContainerSlots.DoRemove(input.Words[1], hand); }
            if (result == REMOVE_RESULT.NOT_REMOVED) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }

            MESSAGE_ENUM message = Statics.ItemTypeToRemoveMessage[hand.Item.Type];
            return Handler.HANDLED(message, hand.Item.NameAsParagraph);
        }
        //protected override Handler DoRemove(ITEM_SLOT itemSlot)
        //{
        //    if (Hands.Full) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_HANDS_ARE_FULL); }
        //    ItemArmor item = Body.GetItem(itemSlot, true);

        //    Paragraph pRemovedItem = null;
        //    MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_REMOVE;

        //    switch (itemSlot)
        //    {
        //        case ITEM_SLOT.AMULET:
        //            if (RightHand == null) { RightHand = Amulet; }
        //            else if (LeftHand == null) { LeftHand = Amulet; }
        //            message = MESSAGE_ENUM.PLAYER_REMOVE_ARMOR_NECK;
        //            pRemovedItem = Amulet.NameAsParagraph;
        //            Amulet = null;
        //            break;
        //        case ITEM_SLOT.ARMOR_CHEST:
        //            if (RightHand == null) { RightHand = ArmorChest; }
        //            else if (LeftHand == null) { LeftHand = ArmorChest; }
        //            message = MESSAGE_ENUM.PLAYER_REMOVE_ARMOR_CHEST;
        //            DefensePower -= ArmorChest.ArmorFactor;
        //            pRemovedItem = ArmorChest.NameAsParagraph;
        //            ArmorChest = null;
        //            break;
        //        case ITEM_SLOT.ARMOR_FEET:
        //            if (RightHand == null) { RightHand = ArmorFeet; }
        //            else if (LeftHand == null) { LeftHand = ArmorFeet; }
        //            message = MESSAGE_ENUM.PLAYER_REMOVE_ARMOR_FEET;
        //            DefensePower -= ArmorFeet.ArmorFactor;
        //            pRemovedItem = ArmorFeet.NameAsParagraph;
        //            ArmorFeet = null;
        //            break;
        //        case ITEM_SLOT.ARMOR_HEAD:
        //            if (RightHand == null) { RightHand = ArmorHead; }
        //            else if (LeftHand == null) { LeftHand = ArmorHead; }
        //            message = MESSAGE_ENUM.PLAYER_REMOVE_ARMOR_HEAD;
        //            DefensePower -= ArmorHead.ArmorFactor;
        //            pRemovedItem = ArmorHead.NameAsParagraph;
        //            ArmorHead = null;
        //            break;
        //        case ITEM_SLOT.BACKPACK:
        //            if (RightHand == null) { RightHand = Backpack; }
        //            else if (LeftHand == null) { LeftHand = Backpack; }
        //            message = MESSAGE_ENUM.PLAYER_REMOVE_BACKPACK;
        //            pRemovedItem = Backpack.NameAsParagraph;
        //            Backpack = null;
        //            break;
        //        case ITEM_SLOT.RING_1:
        //            if (RightHand == null) { RightHand = Ring1; }
        //            else if (LeftHand == null) { LeftHand = Ring1; }
        //            message = MESSAGE_ENUM.PLAYER_REMOVE_ARMOR_FINGER;
        //            pRemovedItem = Ring1.NameAsParagraph;
        //            Ring1 = null;
        //            break;
        //        case ITEM_SLOT.RING_2:
        //            if (RightHand == null) { RightHand = Ring2; }
        //            else if (LeftHand == null) { LeftHand = Ring2; }
        //            message = MESSAGE_ENUM.PLAYER_REMOVE_ARMOR_FINGER;
        //            pRemovedItem = Ring2.NameAsParagraph;
        //            Ring2 = null;
        //            break;
        //    }

        //    return Handler.HANDLED(message, pRemovedItem);
        //}
        public override Handler DoShowInventory(TranslatedInput input)
        {
            if (input.Words.Length > 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, InventoryParagraph);
        }
        public override Handler DoGetExtended(TranslatedInput input)
        {
            //// take <item> from <container>
            //if (input.Words[2] != "from") { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }

            //ItemContainer container = null;
            //MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_GET_FROM_CONTAINER;

            //// holding container?
            //if (LeftHand != null && LeftHand.IsKeyword(input.Words[3]) && LeftHand.Type == ITEM_TYPE.CONTAINER) { container = LeftHand as ItemContainer; }
            //else if (RightHand != null && RightHand.IsKeyword(input.Words[3]) && RightHand.Type == ITEM_TYPE.CONTAINER) { container = RightHand as ItemContainer; }
            //// equipped container?
            //else if (Backpack != null && Backpack.IsKeyword(input.Words[2])) { container = Backpack; }
            //// container on ground?
            //else
            //{
            //    container = CurrentRoom.Items.Find(input.Words[3], ITEM_TYPE.CONTAINER) as ItemContainer;
            //    message = MESSAGE_ENUM.PLAYER_GET_FROM_ROOM_CONTAINER;
            //}

            //if (container == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }

            //// valid container; find item
            //Item item = container.Items.Find(input.Words[1]);
            //if (item == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_ITEM); }

            //// item found; attempt to put in hands
            //if (HandsAreFull) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_HANDS_ARE_FULL); }

            //PutItemInHand(item);
            //container.Items.RemoveItem(item);
            //return Handler.HANDLED(message, item.NameAsParagraph, container.NameAsParagraph);

            // TODO: fix this
            throw new NotImplementedException();
        }
        public override Handler DoGet(TranslatedInput input)
        {
            // take <item> from <container>
            if (input.Words.Length == 4) { return DoGetExtended(input); }

            // take <item>
            if (input.Words.Length == 1)
            {
                return Handler.HANDLED(MESSAGE_ENUM.ERROR_WHAT, input.Words[0].ToSentenceCase().ToParagraph());
            }

            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }

            ItemContainer container = null;
            MESSAGE_ENUM message = MESSAGE_ENUM.PLAYER_GET;

            // find item in current room
            Item item = CurrentRoom.Items.Find(input.Words[1]);
            if (item == null)
            {
                // find item in a room container
                List<Item> containers = CurrentRoom.Items.GetItemsOfType(ITEM_TYPE.CONTAINER_ANY);
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
            }
            if (item == null)
            {
                message = MESSAGE_ENUM.PLAYER_GET_FROM_CONTAINER;

                foreach(EntityHand hand in Hands.Hands)
                {
                    if (hand.Item == null) { continue; }
                    if (hand.Item.IsType(ITEM_TYPE.CONTAINER_ANY))
                    {
                        item = container.Items.Find(input.Words[1]);
                        if (item != null) { break; }
                    }
                }
            }
            if (item == null)
            {
                item = ContainerSlots.FindItem(input.Words[1]);
            }
            if (item == null)
            {
                // item not found
                return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_ITEM);
            }

            // item found; attempt to put in hands
            if (Hands.Full) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_HANDS_ARE_FULL); }

            EntityHand emptyHand = Hands.GetEmptyHand();
            emptyHand.Item = item;

            // if we found in a container, remove from container
            if (container != null) { container.Items.RemoveItem(item); }
            // otherwise, remove from room
            else { CurrentRoom.Items.Remove(item); }

            return Handler.HANDLED(message, item.NameWithIndefiniteArticle, container == null ? null : container.NameAsParagraph);
        }
        public override Handler DoDrop(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_WHAT, "Drop".ToParagraph()); }
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }

            Item item = Hands.GetItem(input.Words[1], true);
            if (item == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_NOT_CARRYING_ITEM); }

            CurrentRoom.Items.Add(item);
            return Handler.HANDLED(MESSAGE_ENUM.PLAYER_DROP, item.NameAsParagraph);
        }
        public override Handler DoBuy(TranslatedInput input)
        {
            // if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_WHAT, "Buy".ToParagraph()); }
            if (input.Words.Length > 2) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }

            RoomShop shop = CurrentRoom as RoomShop;
            if (shop == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_NOT_A_SHOP); }

            // "buy" lists items for sale
            if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.BASE_STRING, shop.SoldItemsParagraph); }

            // scenario 1: buy <number>
            // <number> corresponds with the list obtained through SoldItemsString
            // attempt to parse second word as number
            int itemIndex;
            Item boughtItem = null;
            int nPrice = -1;

            if (int.TryParse(input.Words[1], out itemIndex))
            {
                if (itemIndex < 1 || itemIndex > shop.SoldItems.Count) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }

                nPrice = (int)(shop.SoldItems[itemIndex - 1].Value * shop.SellsAt);
                if (Gold < nPrice)
                {
                    return Handler.HANDLED(MESSAGE_ENUM.ERROR_NOT_ENOUGH_GOLD, shop.SoldItems[itemIndex - 1].NameAsParagraph);
                }

                boughtItem = shop.SoldItems[itemIndex - 1].DeepClone();
            }

            // TODO: scenario 2: parse second word as item keyword
            // TODO: find item with merged input
            // MergeInput(int nStartingIndex)
            // FindItem(string, bool bMultiWord = false)
            // - if true, match string to full item name

            if (boughtItem == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            if (Hands.Full) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_HANDS_ARE_FULL); }

            EntityHand hand = Hands.GetEmptyHand();
            hand.Item = boughtItem;

            Gold -= nPrice;
            return Handler.HANDLED(MESSAGE_ENUM.PLAYER_BUY, boughtItem.NameWithIndefiniteArticle, nPrice.ToString().ToParagraph());
        }
        public override Handler DoGold(TranslatedInput input)
        {
            if (input.Words.Length > 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            if (Gold > 0)
            {
                return Handler.HANDLED(MESSAGE_ENUM.PLAYER_CARRYING_GOLD, Gold.ToString().ToParagraph());
            }

            return Handler.HANDLED(MESSAGE_ENUM.PLAYER_CARRYING_NO_GOLD);
        }
        public override Handler DoPrice(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_WHAT, "Price".ToParagraph()); }
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }

            // verify that we're in a shop
            RoomShop shop = CurrentRoom as RoomShop;
            if (shop == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_NOT_A_SHOP); }

            Handler handler = new Handler(RETURN_CODE.HANDLED, MESSAGE_ENUM.ERROR_BAD_ITEM);
            foreach (EntityHand hand in Hands.Hands)
            {
                handler = shop.DoPriceItem(hand.Item, input.Words[1]);
                if (handler.MessageCode != MESSAGE_ENUM.ERROR_BAD_ITEM)
                {
                    break;
                }
            }

            return handler;
        }
        public override Handler DoSell(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_WHAT, "Sell".ToParagraph()); }
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }

            // verify that we're in a shop
            RoomShop shop = CurrentRoom as RoomShop;
            if (shop == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_NOT_A_SHOP); }

            // attempt to sell item
            // TODO: find player item first, THEN attempt to sell to shop
            return shop.DoBuyFromEntity(this, input.Words[1]);
        }
        public override Handler DoStand(TranslatedInput input)
        {
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }
            if (Posture == ENTITY_POSTURE.STANDING) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_ALREADY_STANDING); }
            Posture = ENTITY_POSTURE.STANDING;
            return Handler.HANDLED(MESSAGE_ENUM.PLAYER_STAND);
        }
        public override Handler DoKneel(TranslatedInput input)
        {
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }
            if (Posture == ENTITY_POSTURE.KNEELING) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_ALREADY_KNEELING); }
            Posture = ENTITY_POSTURE.KNEELING;
            return Handler.HANDLED(MESSAGE_ENUM.PLAYER_KNEEL);
        }
        public override Handler DoSit(TranslatedInput input)
        {
            if (IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD); }
            if (Posture == ENTITY_POSTURE.SITTING) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_ALREADY_SITTING); }
            Posture = ENTITY_POSTURE.SITTING;
            return Handler.HANDLED(MESSAGE_ENUM.PLAYER_SIT);
        }
        public override Handler DoShowHealth(TranslatedInput input)
        {
            if (input.Words.Length > 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            return Handler.HANDLED(MESSAGE_ENUM.PLAYER_SHOW_HEALTH, Attributes.HealthString.ToParagraph(), Attributes.MagicString.ToParagraph());
        }
        public override Handler DoSearch(TranslatedInput input)
        {
            if (input.Words.Length == 1) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_WHAT, "Search".ToParagraph()); }

            int nOrdinal = 0;
            string strNPC = "";

            switch (input.Words.Length)
            {
                case 2:
                    strNPC = input.Words[1];
                    break;
                case 3:
                    if (!Statics.OrdinalStringToInt.ContainsKey(input.Words[1])) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
                    nOrdinal = Statics.OrdinalStringToInt[input.Words[1]];
                    strNPC = input.Words[2];
                    break;
                default:
                    return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT);
            }

            EntityNPCBase npc = CurrentRoom.NPCs.FindDead(strNPC, nOrdinal);
            if (npc == null) { npc = CurrentRoom.NPCs.Find(strNPC, nOrdinal); }
            if (npc == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT); }
            if (!npc.IsDead) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_NPC_NOT_DEAD, npc.NameAsParagraph); }

            // add npc inventory to room
            // TODO: messaging?
            // You search the goblin and remove its equipment. You find <x> gold.
            Gold += npc.Gold;
            npc.BeSearched();

            if (npc.Gold > 0)
            {
                return Handler.HANDLED(MESSAGE_ENUM.NPC_SEARCH_WITH_GOLD, npc.NameAsParagraph, npc.Gold.ToString().ToParagraph());
            }
            else
            {
                return Handler.HANDLED(MESSAGE_ENUM.NPC_SEARCH_NO_GOLD, npc.NameAsParagraph);
            }
        }
        #endregion
    }
}