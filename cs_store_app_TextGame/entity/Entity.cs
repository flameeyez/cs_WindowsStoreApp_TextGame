using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

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
        #region Attributes
        public int ID { get; set; }
        public int NID { get; set; }
        public EntityAttributes Attributes = new EntityAttributes();
        public ENTITY_POSTURE Posture { get; set; }
        protected string _name { get; set; }
        public virtual string Name
        {
            get
            {
                return (IsDead ? "dead " : "") + _name + " (" + NID.ToString() + ") (" + CurrentHealth.ToString() + ":" + MaximumHealth.ToString() + ")";
            }
            set
            {
                _name = value;
            }
        }
        public virtual string NameBase
        {
            get
            {
                return _name + " (" + NID.ToString() + ") (" + CurrentHealth.ToString() + ":" + MaximumHealth.ToString() + ")";
            }
        }
        public virtual Run NameAsRun
        {
            get
            {
                return new Run { Foreground = new SolidColorBrush(Colors.Yellow), Text = Name };
            }
        }
        public virtual Paragraph NameAsParagraph
        {
            get
            {
                Paragraph p = new Paragraph();
                p.Inlines.Add(NameAsRun);
                return p;
            }
        }
        public virtual Paragraph NameBaseAsParagraph
        {
            get
            {
                Paragraph p = new Paragraph();
                p.Inlines.Add(new Run { Foreground = new SolidColorBrush(Colors.Yellow), Text = NameBase });
                return p;
            }
        }
        public virtual string NameIndefiniteArticle(bool bCapitalize)
        {
            return _name.IndefiniteArticle(bCapitalize);
        }
        public Paragraph NameWithIndefiniteArticle(bool bCapitalize = false)
        {
            Paragraph p = new Paragraph();

            p.Inlines.Add((NameIndefiniteArticle(bCapitalize)).ToRun());
            p.Inlines.Add(NameAsRun);

            return p;
        }
        public int MaximumHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int MaximumMagic { get; set; }
        public int CurrentMagic { get; set; }
        public int Gold { get; set; }
        public int DefensePower { get; set; }
        public int AttackPower
        {
            get
            {
                int nAttackPower = Attributes.Strength;

                if (RightHand != null && RightHand.Type == ITEM_TYPE.WEAPON)
                {
                    ItemWeapon RightHandWeapon = RightHand as ItemWeapon;
                    nAttackPower += RightHandWeapon.AttackPower;
                }
                if (LeftHand != null && LeftHand.Type == ITEM_TYPE.WEAPON)
                {
                    ItemWeapon LeftHandWeapon = LeftHand as ItemWeapon;
                    nAttackPower += LeftHandWeapon.AttackPower;
                }

                return nAttackPower;
            }
        }
        public bool IsDead
        {
            get
            {
                return CurrentHealth <= 0;
            }
        }
        public bool HandsAreFull
        {
            get
            {
                return RightHand != null && LeftHand != null;
            }
        }
        public bool HandsAreEmpty
        {
            get
            {
                return RightHand == null && LeftHand == null;
            }
        }

        #endregion
        public bool Searched { get; set; }
        #region Constructor
        public Entity() 
        {
            Searched = false;
            Posture = ENTITY_POSTURE.STANDING;
        }
        #endregion
        #region Display Strings
        public string HealthString
        {
            get
            {
                return CurrentHealth.ToString() + "/" + MaximumHealth.ToString();
            }
        }
        public string MagicString
        {
            get
            {
                return CurrentMagic.ToString() + "/" + MaximumMagic.ToString();
            }
        }
        public virtual string HandsString { get { return ""; } }
        public virtual string InventoryString { get { return ""; } }
        public virtual string DisplayString 
        {
            get 
            {
                if(IsDead)
                {
                    return "The " + NameBase + " is dead.";
                }

                return HandsString + "\n" + InventoryString; 
            }
        }
        #endregion
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
        #region World Coordinates
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
        #endregion
        #region Methods
        public void SetCurrentRoom(int nRegion, int nSubregion, int nRoom)
        {
            CurrentRegionIndex = nRegion;
            CurrentRoomIndex = nRoom;

            _currentRegion = World.Regions[nRegion];
            _currentSubregion = CurrentRegion.Subregions[nSubregion];
            _currentRoom = CurrentSubregion.Rooms[nRoom];
        }
        public void SetCurrentRoom(Connection connection)
        {
            SetCurrentRoom(connection.DestinationRegion, connection.DestinationSubregion, connection.DestinationRoom);
        }
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
        public Item GetItemFromEquipment(string strKeyword, bool bRemoveFromHand)
        {
            Item item = null;
            if(ArmorHead != null && ArmorHead.IsKeyword(strKeyword))
            {
                item = ArmorHead;
                if (bRemoveFromHand) { ArmorHead = null; }
            }
            else if(ArmorChest != null && ArmorChest.IsKeyword(strKeyword))
            {
                item = ArmorChest;
                if (bRemoveFromHand) { ArmorChest = null; }
            }
            else if(ArmorFeet != null && ArmorFeet.IsKeyword(strKeyword))
            {
                item = ArmorFeet;
                if (bRemoveFromHand) { ArmorFeet = null; }
            }
            else if(Ring1 != null && Ring1.IsKeyword(strKeyword))
            {
                item = Ring1;
                if (bRemoveFromHand) { Ring1 = null; }
            }
            else if(Ring2 != null && Ring2.IsKeyword(strKeyword))
            {
                item = Ring2;
                if (bRemoveFromHand) { Ring2 = null; }
            }
            else if(Amulet != null && Amulet.IsKeyword(strKeyword))
            {
                item = Amulet;
                if (bRemoveFromHand) { Amulet = null; }
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
        #endregion
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
                case ACTION_ENUM.ATTACK:
                    return DoAttack(input);
                case ACTION_ENUM.SHOW_HEALTH:
                    return DoShowHealth(input);
                case ACTION_ENUM.SEARCH:
                    return DoSearch(input);
                default:
                    return Handler.UNHANDLED;
            }
        }

        public virtual Handler DoAttack(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoMoveBasic(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoLook(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoLook(string strWord) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoLook(string strWord1, string strWord2) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoLook(string strWord1, string strWord2, string strWord3) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoLookHands(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoLookInContainer(string strKeyword, int ordinal = 0) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoEat(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoDrink(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoOpen(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoClose(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoMoveConnection(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoPut(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoEquip(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoRemove(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoRemove(ITEM_SLOT item) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoShowInventory(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoGetExtended(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoGet(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoDrop(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoBuy(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoGold(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoPrice(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoSell(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoStand(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoKneel(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoSit(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoShowHealth(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }
        public virtual Handler DoSearch(TranslatedInput input) { return Handler.Default(MESSAGE_ENUM.NO_MESSAGE); }

        #endregion

        public virtual void BeSearched()
        {
            Searched = true;

            // add inventory
            if (ArmorHead != null) 
            {
                CurrentRoom.Items.Add(ArmorHead);
                ArmorHead = null;
            }
            if(ArmorChest != null)
            {
                CurrentRoom.Items.Add(ArmorChest);
                ArmorChest = null;
            }
            if(ArmorFeet!=null)
            {
                CurrentRoom.Items.Add(ArmorFeet);
                ArmorFeet = null;
            }
            if (RightHand != null)
            {
                CurrentRoom.Items.Add(RightHand);
                RightHand = null;
            }
            if (LeftHand != null)
            {
                CurrentRoom.Items.Add(LeftHand);
                LeftHand = null;
            }
            if (Backpack != null)
            {
                CurrentRoom.Items.Add(Backpack);
                Backpack = null;
            }
            if (Amulet != null)
            {
                CurrentRoom.Items.Add(Amulet);
                Amulet = null;
            }
            if (Ring1 != null)
            {
                CurrentRoom.Items.Add(Ring1);
                Ring1 = null;
            }
            if (Ring2 != null)
            {
                CurrentRoom.Items.Add(Ring2);
                Ring2 = null;
            }
        }
    }
}