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
        CONTAINER = 0x8,
        WEAPON = 0x10,
        ARMOR_CHEST = 0x20,
        ARMOR_FEET = 0x40,
        ARMOR_HEAD = 0x80,
        ACCESSORY_AMULET = 0x100,
        ACCESSORY_RING = 0x200,
        ARMOR_SHIELD = 0x400,
        ANY = 0xFFF,
    };

    public enum ITEM_MATCH_CODE
    {
        MATCH,
        BAD_KEYWORD,
        BAD_TYPE
    }

    [DataContract(Name = "Item", Namespace = "cs_store_app_TextGame")]
    public abstract class Item
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
            StringToEnum.Add("container", ITEM_TYPE.CONTAINER);
            StringToEnum.Add("food", ITEM_TYPE.FOOD);
            StringToEnum.Add("drink", ITEM_TYPE.DRINK);
            StringToEnum.Add("accessory-amulet", ITEM_TYPE.ACCESSORY_AMULET);
            StringToEnum.Add("accessory-ring", ITEM_TYPE.ACCESSORY_RING);
            StringToEnum.Add("junk", ITEM_TYPE.JUNK);
        }

        //[DataMember]
        //public Guid UID { get; set; }
        [DataMember]
        public int NID { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        private string _name;
        public string Name 
        {
            get
            {
                return _name +" (" + NID.ToString() + ")";// +" {" + UID.ToString() + "}";
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
                return Name + "\n" + _description;
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
        public Item(XElement itemNode)
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
    }
}
