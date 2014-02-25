using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public enum ITEM_SLOT
    {
        BACKPACK,
        ARMOR_CHEST,
        ARMOR_HEAD,
        ARMOR_FEET,
        RING_1,
        RING_2,
        AMULET
    }

    public enum ENTITY_POSTURE
    {
        STANDING,
        SITTING,
        KNEELING
    }

    public abstract class Entity
    {
        public int ID { get; set; }
        public int NID { get; set; }
        // TODO: see if we can avoid storing world references in each entity
        // - how to move player?
        public Entity() 
        {
            Posture = ENTITY_POSTURE.STANDING;
        }

        public EntityAttributes Attributes = new EntityAttributes();
        public ENTITY_POSTURE Posture { get; set; }

        private string _name { get; set; }
        public string Name 
        {
            get
            {
                return _name + " (" + NID.ToString() + ")";
            }
            set
            {
                _name = value;
            }
        }
        public int MaximumHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int MaximumMagic { get; set; }
        public int CurrentMagic { get; set; }
        public int Gold { get; set; }
        public int DefensePower { get; set; }

        public Item RightHand { get; set; }
        public Item LeftHand { get; set; }
        public bool HandsAreFull
        {
            get
            {
                return RightHand != null && LeftHand != null;
            }
        }
        public virtual string HandsString { get { return ""; } }

        public ItemContainer Backpack { get; set; }
        public ItemArmorChest ArmorChest { get; set; }
        public ItemArmorFeet ArmorFeet { get; set; }
        public ItemArmorHead ArmorHead { get; set; }
        public ItemAccessoryAmulet Amulet { get; set; }
        public ItemAccessoryRing Ring1 { get; set; }
        public ItemAccessoryRing Ring2 { get; set; }

        public int CurrentRegionIndex { get; set; }
        public int CurrentSubregionIndex { get; set; }
        public int CurrentRoomIndex { get; set; }

        private Room _currentRoom;
        public Room CurrentRoom
        {
            get
            {
                return _currentRoom;
            }
        }
        private Region _currentRegion;
        public Region CurrentRegion
        {
            get
            {
                return _currentRegion;
            }
        }
        private Subregion _currentSubregion;
        public Subregion CurrentSubregion
        {
            get
            {
                return _currentSubregion;
            }
        }

        public void SetCurrentRoom(int nRegion, int nSubregion, int nRoom)
        {
            CurrentRegionIndex = nRegion;
            CurrentRoomIndex = nRoom;

            _currentRegion = World.Regions[nRegion];
            _currentSubregion = CurrentRegion.Subregions[nSubregion];
            _currentRoom = CurrentSubregion.Rooms[nRoom];
        }
        
        // GetItem(hand, strKeyword)
        // ItemtoHand(item)
        public Item GetItemFromHand(string strKeyword, bool bRemoveFromHand, ITEM_TYPE itemType = ITEM_TYPE.ANY)
        {
            Item item = null;

            if (RightHand != null)
            {
                if (RightHand.Type == itemType || itemType == ITEM_TYPE.ANY)
                {
                    if (RightHand.IsKeyword(strKeyword))
                    {
                        item = RightHand;
                        if (bRemoveFromHand) { RightHand = null; }
                    }
                }
            }
            if (item == null && LeftHand != null)
            {
                if (LeftHand.Type == itemType || itemType == ITEM_TYPE.ANY)
                {
                    if (LeftHand.IsKeyword(strKeyword))
                    {
                        item = LeftHand;
                        if (bRemoveFromHand) { LeftHand = null; }
                    }
                }
            }

            return item;
        }
        public bool PutItemInHand(Item item)
        {
            if (RightHand == null)
            {
                RightHand = item;
                return true;
            }
            else if (LeftHand == null)
            {
                LeftHand = item;
                return true;
            }

            return false;
        }

        public virtual string InventoryString { get { return ""; } }

        //////////////////////////////////////////

        #region Input Handling

        public Handler ProcessInput(TranslatedInput input)
        {
            switch (input.Action)
            {
                case ACTION_ENUM.NONE:
                    return Handler.UNHANDLED;
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
                default:
                    return Handler.UNHANDLED;
            }
        }

        public virtual Handler DoMoveBasic(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoLook(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoLook(string strWord) { return Handler.HANDLED; }
        public virtual Handler DoLook(string strWord1, string strWord2) { return Handler.HANDLED; }
        public virtual Handler DoLookHands(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoLookInContainer(string strKeyword) { return Handler.HANDLED; }
        public virtual Handler DoEat(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoDrink(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoOpen(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoClose(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoMoveConnection(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoPut(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoEquip(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoRemove(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoRemove(ITEM_SLOT item) { return Handler.HANDLED; }
        public virtual Handler DoShowInventory(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoGetExtended(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoGet(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoDrop(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoBuy(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoGold(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoPrice(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoSell(TranslatedInput input) { return Handler.HANDLED; }

        public virtual Handler DoStand(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoKneel(TranslatedInput input) { return Handler.HANDLED; }
        public virtual Handler DoSit(TranslatedInput input) { return Handler.HANDLED; }

        #endregion
    }
}
