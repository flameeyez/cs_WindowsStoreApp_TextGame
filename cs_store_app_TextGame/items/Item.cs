using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    [Flags]
    public enum ITEM_TYPE
    {
        BASE = 0x0,
        DRINK = 0x1,
        FOOD = 0x2,
        JUNK = 0x4,
        // something = 0x8,
        WEAPON = 0x10,
        ARMOR_CHEST = 0x20,
        ARMOR_FEET = 0x40,
        ARMOR_HEAD = 0x80,
        ARMOR_NECK = 0x100,
        ARMOR_FINGER = 0x200,
        ARMOR_SHIELD = 0x400,
        // TODO: shields don't count right now (for DoEquip)
        ARMOR_ANY = ARMOR_CHEST | ARMOR_FEET | ARMOR_HEAD | ARMOR_NECK | ARMOR_FINGER, // | ARMOR_SHIELD
        CONTAINER_POUCH = 0x1000,
        CONTAINER_BACKPACK = 0x2000,
        CONTAINER_ANY = CONTAINER_POUCH | CONTAINER_BACKPACK,
        ANY = 0xFFFF,
    };

    public enum ITEM_MATCH_CODE
    {
        MATCH,
        BAD_KEYWORD,
        BAD_TYPE
    }

    [DataContract(Name = "Item", Namespace = "cs_store_app_TextGame")]
    [KnownType(typeof(ItemWeapon))]
    [KnownType(typeof(ItemArmorChest))]
    [KnownType(typeof(ItemArmorFeet))]
    [KnownType(typeof(ItemArmorFinger))]
    [KnownType(typeof(ItemArmorHead))]
    [KnownType(typeof(ItemArmorNeck))]
    [KnownType(typeof(ItemArmorShield))]
    [KnownType(typeof(ItemContainerBackpack))]
    [KnownType(typeof(ItemContainerPouch))]
    [KnownType(typeof(ItemFood))]
    [KnownType(typeof(ItemDrink))]
    [KnownType(typeof(ItemJunk))]
    public abstract class Item : GameObject
    {
        public static Dictionary<string, ITEM_TYPE> StringToEnum = new Dictionary<string, ITEM_TYPE>();
        static Item()
        {
            StringToEnum.Add("any", ITEM_TYPE.ANY);
            StringToEnum.Add("weapon", ITEM_TYPE.WEAPON);
            StringToEnum.Add("armor-chest", ITEM_TYPE.ARMOR_CHEST);
            StringToEnum.Add("armor-feet", ITEM_TYPE.ARMOR_FEET);
            StringToEnum.Add("armor-head", ITEM_TYPE.ARMOR_HEAD);
            StringToEnum.Add("armor-shield", ITEM_TYPE.ARMOR_SHIELD);
            StringToEnum.Add("container-pouch", ITEM_TYPE.CONTAINER_POUCH);
            StringToEnum.Add("container-backpack", ITEM_TYPE.CONTAINER_BACKPACK);
            StringToEnum.Add("food", ITEM_TYPE.FOOD);
            StringToEnum.Add("drink", ITEM_TYPE.DRINK);
            StringToEnum.Add("armor-neck", ITEM_TYPE.ARMOR_NECK);
            StringToEnum.Add("armor-finger", ITEM_TYPE.ARMOR_FINGER);
            StringToEnum.Add("junk", ITEM_TYPE.JUNK);
        }

        //[DataMember]
        //public Guid UID { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        private string _name;
        public string Name 
        {
            get
            {
                return _name + " (" + NID.ToString() + ")";// +" {" + UID.ToString() + "}";
            }
            set
            {
                _name = value;
            }
        }
        public Run NameAsRun
        {
            get
            {
                return Name.ToRun(Colors.LightGreen);
            }
        }
        public Paragraph NameAsParagraph
        {
            get
            {
                Paragraph p = new Paragraph();
                p.Inlines.Add(NameAsRun);
                return p;
            }
        }
        public string NameIndefiniteArticle
        {
            get
            {
                return _name.IndefiniteArticle();
            }
        }
        public Paragraph NameWithIndefiniteArticle
        {
            get
            {
                Paragraph p = new Paragraph();

                p.Inlines.Add((NameIndefiniteArticle).ToRun());
                p.Inlines.Add(NameAsRun);

                return p;
            }
        }
        [DataMember]
        private string _description;
        public string Description 
        {
            get
            {
                return _description; // Name + "\n" + _description;
            }
            set
            {
                _description = value;
            }
        }
        [DataMember]
        public double Weight { get; set; }
        [DataMember]
        public int Value{get;set;}
        // no underlying stored value, so no [DataMember] attribute
        public abstract ITEM_TYPE Type { get; }
        [DataMember]
        public List<string> Keywords = new List<string>();
        
        public Item() { }
        public Item(XElement itemNode) : this()
        {
            ID = int.Parse(itemNode.Element("id").Value);
            Name = itemNode.Element("name").Value;
            Description = itemNode.Element("description").Value;
            Weight = double.Parse(itemNode.Element("weight").Value);
            Value = int.Parse(itemNode.Element("value").Value);

            // item.keywords
            var keywordNodes = from keywords in itemNode
                                .Elements("keywords")
                                  .Elements("keyword")
                            select keywords;
            foreach(var keywordNode in keywordNodes)
            {
                Keywords.Add(keywordNode.Value);
            }
        }
        public bool IsKeyword(string strKeyword)
        {
            foreach(string keyword in Keywords)
            {
                if(keyword == strKeyword)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            Item item = obj as Item;
            if (item == null) { return false; }

            return (this.Name == item.Name) && (this.Type == item.Type);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Name.GetHashCode();
            hash = (hash * 7) + Type.GetHashCode();
            return hash;
        }

        public ITEM_MATCH_CODE Match(string strKeyword, ITEM_TYPE type = ITEM_TYPE.ANY)
        {
            if (!this.IsKeyword(strKeyword)) { return ITEM_MATCH_CODE.BAD_KEYWORD; }
            if (!(this.Type == type) && type != ITEM_TYPE.ANY) { return ITEM_MATCH_CODE.BAD_TYPE; }
            return ITEM_MATCH_CODE.MATCH;
        }

        public bool IsType(ITEM_TYPE type)
        {
            return (type & Type) == Type;
        }
    }
}
