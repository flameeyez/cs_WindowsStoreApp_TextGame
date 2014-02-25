using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "ItemJunk", Namespace = "cs_store_app_TextGame")]
    public class ItemJunk : Item
    {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.JUNK; } }
        public ItemJunk(XElement itemNode) : base(itemNode) { }
    }
}
