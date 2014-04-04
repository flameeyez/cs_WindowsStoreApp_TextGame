using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    public class EntityNPCTemplate
    {
        public int ID { get; set; }
        public int NID { get; set; }
        public EntityAttributes Attributes = new EntityAttributes();
        public string Name { get; set; }
        public int MaximumHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int MaximumMagic { get; set; }
        public int CurrentMagic { get; set; }
        public int Gold { get; set; }
        public int DefensePower { get; set; }
        public ENTITY_TYPE Type { get; set; }
        public List<string> Keywords = new List<string>();
        
        // TODO: implement behavior
        public static List<EntityNPCBehavior> Behavior = new List<EntityNPCBehavior>();

        #region Equipment
        public Item RightHand { get; set; }
        public Item LeftHand { get; set; }
        public ItemContainer Backpack { get; set; }
        public ItemArmorChest ArmorChest { get; set; }
        public ItemArmorFeet ArmorFeet { get; set; }
        public ItemArmorHead ArmorHead { get; set; }
        public ItemAccessoryAmulet Amulet { get; set; }
        public ItemAccessoryRing Ring1 { get; set; }
        public ItemAccessoryRing Ring2 { get; set; }
        #endregion

        public EntityNPCTemplate(XElement npcNode)
        {
            ID = int.Parse(npcNode.Element("id").Value);
            Name = npcNode.Element("name").Value;
            Type = (ENTITY_TYPE)Enum.Parse(typeof(ENTITY_TYPE), npcNode.Element("type").Value);
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
            if (behaviorNode != null)
            {
                foreach (var node in behaviorNode.Elements())
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

        public EntityNPC Clone()
        {
            EntityNPC npc = new EntityNPC();
            npc.ActionPulse = 8000 + Statics.r.Next(15000);
            npc.NID = Statics.EntityCount++;
            npc.ID = ID;
            npc.Type = Type;
            npc.Name = Name;
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
    }
}