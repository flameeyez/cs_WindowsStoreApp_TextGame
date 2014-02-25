using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "ItemAccessory", Namespace = "cs_store_app_TextGame")]
    public abstract class ItemAccessory : Item
    {
        public ItemAccessory(XElement itemNode) : base(itemNode) { }
    }
}
