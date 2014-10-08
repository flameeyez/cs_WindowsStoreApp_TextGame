//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;

//namespace cs_store_app_TextGame
//{
//    public class EntityNPCTemplate
//    {
//        public int ID { get; set; }
//        public int NID { get; set; }
//        public EntityAttributes Attributes = new EntityAttributes();
//        public string Name { get; set; }
//        public int Gold { get; set; }
//        public int DefensePower { get; set; }
//        public FACTION Faction { get; set; }
//        public List<string> Keywords = new List<string>();
//        public EntityBehavior Behavior { get; set; }

//        #region Body
//        EntityBody Body = new EntityBody();
//        EntityHands Hands = new EntityHands();
//        #endregion

//        public EntityNPCTemplate(XElement npcNode)
//        {
//            ID = int.Parse(npcNode.Element("id").Value);
//            Name = npcNode.Element("name").Value;
//            Faction = (FACTION)Enum.Parse(typeof(FACTION), npcNode.Element("faction").Value);
            
//            // attributes
//            XElement attributesElement = npcNode.Element("attributes");
//            Attributes.Strength = int.Parse(attributesElement.Element("strength").Value);
//            Attributes.Intelligence = int.Parse(attributesElement.Element("intelligence").Value);
//            Attributes.Vitality = int.Parse(attributesElement.Element("vitality").Value);
//            Attributes.MaximumHealth = int.Parse(attributesElement.Element("maximum-health").Value);
//            Attributes.CurrentHealth = Attributes.MaximumHealth;
//            Attributes.MaximumMagic = int.Parse(attributesElement.Element("maximum-magic").Value);
//            Attributes.CurrentMagic = Attributes.MaximumMagic;            

//            // keywords
//            foreach(XElement keywordElement in npcNode.Element("keywords").Elements("keyword"))
//            {
//                Keywords.Add(keywordElement.Value);
//            }

//            // behavior
//            var behaviorNode = npcNode.Element("behavior");
//            if (behaviorNode != null)
//            {
//                foreach (var element in behaviorNode.Elements())
//                {
//                    ACTION_ENUM action = TranslatedInput.StringToAction[element.Name.LocalName];
//                    int percentage = int.Parse(element.Value);
//                    Behavior.PossibleActions.Add(new EntityBehaviorAction(action, percentage));
//                }
//            }

//            // inventory
//            var inventoryNode = npcNode.Element("inventory");

//            // right hand
//            var rightHandNode = inventoryNode.Element("right-hand");
//            if (rightHandNode != null)
//            {
//                var itemTypeNode = rightHandNode.Elements().First();
//                int nIndex = int.Parse(itemTypeNode.Value);
//                switch (itemTypeNode.Name.LocalName)
//                {
//                    case "weapon":
//                        RightHand = ItemTemplates.ItemsWeapon[nIndex].DeepClone();
//                        break;
//                    case "armor-shield":
//                        RightHand = ItemTemplates.ItemsArmorShield[nIndex].DeepClone();
//                        break;
//                    case "armor-head":
//                        RightHand = ItemTemplates.ItemsArmorHead[nIndex].DeepClone();
//                        break;
//                    case "armor-feet":
//                        RightHand = ItemTemplates.ItemsArmorFeet[nIndex].DeepClone();
//                        break;
//                    case "armor-chest":
//                        RightHand = ItemTemplates.ItemsArmorChest[nIndex].DeepClone();
//                        break;
//                    case "armor-finger":
//                        RightHand = ItemTemplates.ItemsArmorFinger[nIndex].DeepClone();
//                        break;
//                    case "armor-neck":
//                        RightHand = ItemTemplates.ItemsArmorNeck[nIndex].DeepClone();
//                        break;
//                    case "drink":
//                        RightHand = ItemTemplates.ItemsDrink[nIndex].DeepClone();
//                        break;
//                    case "food":
//                        RightHand = ItemTemplates.ItemsFood[nIndex].DeepClone();
//                        break;
//                    case "container":
//                        RightHand = ItemTemplates.ItemsContainer[nIndex].DeepClone();
//                        break;
//                    case "junk":
//                        RightHand = ItemTemplates.ItemsJunk[nIndex].DeepClone();
//                        break;
//                }
//            }

//            // left hand
//            var leftHandNode = inventoryNode.Element("left-hand");
//            if (leftHandNode != null)
//            {
//                var itemTypeNode = leftHandNode.Elements().First();
//                int nIndex = int.Parse(itemTypeNode.Value);
//                switch (itemTypeNode.Name.LocalName)
//                {
//                    case "weapon":
//                        LeftHand = ItemTemplates.ItemsWeapon[nIndex].DeepClone();
//                        break;
//                    case "armor-shield":
//                        LeftHand = ItemTemplates.ItemsArmorShield[nIndex].DeepClone();
//                        break;
//                    case "armor-head":
//                        LeftHand = ItemTemplates.ItemsArmorHead[nIndex].DeepClone();
//                        break;
//                    case "armor-feet":
//                        LeftHand = ItemTemplates.ItemsArmorFeet[nIndex].DeepClone();
//                        break;
//                    case "armor-chest":
//                        LeftHand = ItemTemplates.ItemsArmorChest[nIndex].DeepClone();
//                        break;
//                    case "armor-finger":
//                        LeftHand = ItemTemplates.ItemsArmorFinger[nIndex].DeepClone();
//                        break;
//                    case "armor-neck":
//                        LeftHand = ItemTemplates.ItemsArmorNeck[nIndex].DeepClone();
//                        break;
//                    case "drink":
//                        LeftHand = ItemTemplates.ItemsDrink[nIndex].DeepClone();
//                        break;
//                    case "food":
//                        LeftHand = ItemTemplates.ItemsFood[nIndex].DeepClone();
//                        break;
//                    case "container":
//                        LeftHand = ItemTemplates.ItemsContainer[nIndex].DeepClone();
//                        break;
//                    case "junk":
//                        LeftHand = ItemTemplates.ItemsJunk[nIndex].DeepClone();
//                        break;
//                }
//            }

//            // armor chest
//            var armorChestNode = inventoryNode.Element("armor-chest");
//            if (armorChestNode != null)
//            {
//                int nIndex = int.Parse(armorChestNode.Value);
//                ArmorChest = ItemTemplates.ItemsArmorChest[nIndex].DeepClone();
//            }

//            // armor head
//            var armorHeadNode = inventoryNode.Element("armor-head");
//            if (armorHeadNode != null)
//            {
//                int nIndex = int.Parse(armorHeadNode.Value);
//                ArmorHead = ItemTemplates.ItemsArmorHead[nIndex].DeepClone();
//            }

//            // armor feet
//            var armorFeetNode = inventoryNode.Element("armor-feet");
//            if (armorFeetNode != null)
//            {
//                int nIndex = int.Parse(armorFeetNode.Value);
//                ArmorFeet = ItemTemplates.ItemsArmorFeet[nIndex].DeepClone();
//            }

//            // backpack
//            var backpackNode = inventoryNode.Element("backpack");
//            if (backpackNode != null)
//            {
//                int nIndex = int.Parse(backpackNode.Value);
//                Backpack = ItemTemplates.ItemsContainer[nIndex].DeepClone();
//            }

//            // ring 1
//            var ring1Node = inventoryNode.Element("ring1");
//            if (ring1Node != null)
//            {
//                int nIndex = int.Parse(ring1Node.Value);
//                Ring1 = ItemTemplates.ItemsArmorFinger[nIndex].DeepClone();
//            }

//            // ring 2
//            var ring2Node = inventoryNode.Element("ring2");
//            if (ring2Node != null)
//            {
//                int nIndex = int.Parse(ring2Node.Value);
//                Ring2 = ItemTemplates.ItemsArmorFinger[nIndex].DeepClone();
//            }

//            // amulet
//            var amuletNode = inventoryNode.Element("amulet");
//            if (amuletNode != null)
//            {
//                int nIndex = int.Parse(amuletNode.Value);
//                Amulet = ItemTemplates.ItemsArmorNeck[nIndex].DeepClone();
//            }

//            // gold
//            var goldNode = inventoryNode.Element("gold");
//            if (goldNode != null)
//            {
//                Gold = int.Parse(goldNode.Value);
//            }
//        }

//        public EntityNPC Clone()
//        {
//            return new EntityNPC(this);
//        }
//    }
//}