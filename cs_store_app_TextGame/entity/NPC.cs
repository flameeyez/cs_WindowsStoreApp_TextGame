using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    public class NPC : Entity
    {
        #region Attributes

        public DateTime LastActionTime = DateTime.Now;
        public int ActionPulse { get; set; }
        public override string Name
        {
            get
            {
                return base.Name + " (" + CurrentHealth.ToString() + ":" + MaximumHealth.ToString() + ")";
            }
        }
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

        #endregion
        #region Constructors

        public NPC() : base() { }
        public NPC Clone()
        {
            NPC npc = new NPC();
            npc.ActionPulse = 10000 + StaticMethods.r.Next(5000);
            npc.NID = StaticMethods.EntityCount++;
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

        public NPC(XElement npcNode)
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

        private Handler DoRandomAction()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            TranslatedInput input = null;

            switch (r.Next(2))
            {
                case 0:
                    return DoMoveBasic(input);
                case 1:
                    return DoLook(input);
            }

            return Handler.UNHANDLED;
        }

        public override Handler DoLook(TranslatedInput input)
        {
            if (CurrentRoom.Equals(Game.Player.CurrentRoom))
            {
                return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.NPC_LOOK, Name));
            }

            return Handler.HANDLED;
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
            if (CurrentHealth <= 0) 
            {
                CurrentRoom.NPCs.Remove(this);
                return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.DEBUG_REMOVE, Name));
            }

            DateTime now = DateTime.Now;
            TimeSpan delta = now - LastActionTime;
            if(delta.TotalMilliseconds >= ActionPulse)
            {
                LastActionTime = now;
                handler = DoRandomAction();
            }

            return handler;
        }
    }
}