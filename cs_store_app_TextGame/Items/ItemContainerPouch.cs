using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "ItemContainerPouch", Namespace = "cs_store_app_TextGame")]
    public class ItemContainerPouch : ItemContainer
    {
        public ItemContainerPouch(XElement itemNode) : base(itemNode) { }
        public override ITEM_TYPE Type
        {
            get
            {
                return ITEM_TYPE.CONTAINER_POUCH;
            }
        }
    }
}
