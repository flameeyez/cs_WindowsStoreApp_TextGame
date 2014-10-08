using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace cs_store_app_TextGame
{
    public enum ENTITY_POSTURE
    {
        STANDING,
        SITTING,
        KNEELING
    }

    [DataContract(Name = "EntityBase", Namespace = "cs_store_app_TextGame")]
    public abstract class EntityBase : GameObject
    {
        [OnDeserialized]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            NID = Statics.NID++;

            foreach(EntityBodyPart part in Body.BodyParts)
            {
                if(part.Item != null)
                {
                    part.Item.NID = Statics.NID++;
                }                
            }
        }

        [DataMember]
        public int ID { get; set; }
        
        [DataMember]
        public WorldCoordinates Coordinates = new WorldCoordinates();
        public Room CurrentRoom { get { return Coordinates.CurrentRoom; } }
        
        // strength, intelligence, vitality, health, magic
        [DataMember]
        public EntityAttributes Attributes = new EntityAttributes();
        public bool IsDead
        {
            get
            {
                return Attributes.CurrentHealth <= 0;
            }
        }
        
        [DataMember]
        public bool HasBeenSearched { get; set; }

        [DataMember]
        public int Gold { get; set; }
        [DataMember]
        public ENTITY_POSTURE Posture { get; set; }
        
        public int AttackPower
        {
            get
            {
                return Hands.AttackPower;
            }
        }
        
        [DataMember]
        public EntityBody Body = new EntityBody();
        [DataMember]
        public EntityHands Hands = new EntityHands();
        [DataMember]
        public EntityContainerSlots ContainerSlots = new EntityContainerSlots();

        public abstract Paragraph InventoryParagraph { get; }        

        #region Action Handlers
        public virtual Handler DoAttack(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoMoveBasic(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoLook(TranslatedInput input) { return Handler.UNHANDLED(); }
            protected virtual Handler DoLook(string strWord) { return Handler.UNHANDLED(); }
            protected virtual Handler DoLook(string strWord1, string strWord2) { return Handler.UNHANDLED(); }
            protected virtual Handler DoLook(string strWord1, string strWord2, string strWord3) { return Handler.UNHANDLED(); }
        public virtual Handler DoLookHands(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoLookInContainer(string strKeyword, int ordinal = 0) { return Handler.UNHANDLED(); }
        public virtual Handler DoEat(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoDrink(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoOpen(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoClose(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoMoveConnection(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoPut(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoEquip(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoRemove(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoShowInventory(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoGetExtended(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoGet(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoDrop(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoBuy(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoGold(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoPrice(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoSell(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoStand(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoKneel(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoSit(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoShowHealth(TranslatedInput input) { return Handler.UNHANDLED(); }
        public virtual Handler DoSearch(TranslatedInput input) { return Handler.UNHANDLED(); }
        public Handler ProcessInput(TranslatedInput input)
        {
            switch (input.Action)
            {
                case ACTION_ENUM.NONE:
                    return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_INPUT);
                case ACTION_ENUM.MOVE_BASIC:
                    return DoMoveBasic(input);
                case ACTION_ENUM.MOVE_CONNECTION:
                    return DoMoveConnection(input);
                case ACTION_ENUM.LOOK:
                    return DoLook(input);
                case ACTION_ENUM.OPEN_CONTAINER:
                    return DoOpen(input);
                case ACTION_ENUM.CLOSE_CONTAINER:
                    return DoClose(input);
                case ACTION_ENUM.BUY_ITEM:
                    return DoBuy(input);
                case ACTION_ENUM.SELL_ITEM:
                    return DoSell(input);
                case ACTION_ENUM.PRICE_ITEM:
                    return DoPrice(input);
                case ACTION_ENUM.SHOW_GOLD:
                    return DoGold(input);
                case ACTION_ENUM.GET_ITEM:
                    return DoGet(input);
                case ACTION_ENUM.PUT_ITEM:
                    return DoPut(input);
                case ACTION_ENUM.REMOVE_EQUIPMENT:
                    return DoRemove(input);
                case ACTION_ENUM.EQUIP_ITEM:
                    return DoEquip(input);
                case ACTION_ENUM.DROP_ITEM:
                    return DoDrop(input);
                case ACTION_ENUM.DRINK:
                    return DoDrink(input);
                case ACTION_ENUM.EAT:
                    return DoEat(input);
                case ACTION_ENUM.SHOW_INVENTORY:
                    return DoShowInventory(input);
                case ACTION_ENUM.SHOW_HANDS:
                    return DoLookHands(input);
                case ACTION_ENUM.SIT:
                    return DoSit(input);
                case ACTION_ENUM.STAND:
                    return DoStand(input);
                case ACTION_ENUM.KNEEL:
                    return DoKneel(input);
                case ACTION_ENUM.ATTACK:
                    return DoAttack(input);
                case ACTION_ENUM.SHOW_HEALTH:
                    return DoShowHealth(input);
                case ACTION_ENUM.SEARCH:
                    return DoSearch(input);
                default:
                    return Handler.UNHANDLED();
            }
        }

        #endregion


        public EntityBase() { HasBeenSearched = false; }
        public EntityBase(XElement entityBaseElement) : this()
        {
            ID = int.Parse(entityBaseElement.Element("id").Value);

            // attributes
            XElement attributesElement = entityBaseElement.Element("attributes");
            Attributes.Strength = int.Parse(attributesElement.Element("strength").Value);
            Attributes.Intelligence = int.Parse(attributesElement.Element("intelligence").Value);
            Attributes.Vitality = int.Parse(attributesElement.Element("vitality").Value);
            Attributes.MaximumHealth = int.Parse(attributesElement.Element("maximum-health").Value);
            Attributes.CurrentHealth = Attributes.MaximumHealth;
            Attributes.MaximumMagic = int.Parse(attributesElement.Element("maximum-magic").Value);
            Attributes.CurrentMagic = Attributes.MaximumMagic;

            //<inventory>
            //  <hands>
            //    <hand />
            //    <hand />
            //  </hands>
            //  <body>
            //    <armor-head />
            //    <armor-feet />
            //    <armor-chest>0</armor-chest>
            //    <ring />
            //    <ring />
            //    <amulet />
            //    <backpack>0</backpack>
            //  </body>
            //  <gold>50</gold>
            //</inventory>
            var inventoryElement = entityBaseElement.Element("inventory");

            // hands
            var handElements = inventoryElement.Elements("hands");
            foreach(var handElement in handElements.Elements("hand"))
            {
                Hands.Add(new EntityHand(handElement));
            }

            // body
            var bodyElement = inventoryElement.Element("body");
            foreach(var bodyPartElement in bodyElement.Elements())
            {
                switch(bodyPartElement.Name.LocalName)
                {
                    case "armor-chest":
                        Body.BodyParts.Add(new EntityBodyPartChest(bodyPartElement));
                        break;
                    case "armor-head":
                        Body.BodyParts.Add(new EntityBodyPartHead(bodyPartElement));
                        break;
                    case "armor-feet":
                        Body.BodyParts.Add(new EntityBodyPartFeet(bodyPartElement));
                        break;
                    case "backpack":
                        // TODO: finish
                        // Body.BodyParts.Add(new EntityBodyPartHead(bodyPartElement));
                        break;
                    case "finger":
                        Body.BodyParts.Add(new EntityBodyPartFinger(bodyPartElement));
                        break;
                    case "neck":
                        Body.BodyParts.Add(new EntityBodyPartNeck(bodyPartElement));
                        break;
                }
            }

            // gold
            var goldNode = inventoryElement.Element("gold");
            if (goldNode != null)
            {
                Gold = int.Parse(goldNode.Value);
            }
        }
        public virtual void BeSearched()
        {
            HasBeenSearched = true;

            foreach(EntityBodyPart part in Body.BodyParts)
            {
                if (part.Item == null) { continue; }
                CurrentRoom.Items.Add(part.Item);
                part.Item = null;
            }

            foreach(EntityHand hand in Hands.Hands)
            {
                if (hand.Item == null) { continue; }
                CurrentRoom.Items.Add(hand.Item);
                hand.Item = null;
            }

            foreach(EntityContainerSlot slot in ContainerSlots.ContainerSlots)
            {
                if (slot.Container == null) { continue; }
                CurrentRoom.Items.Add(slot.Container);
                slot.Container = null;
            }
        }
    }
}
