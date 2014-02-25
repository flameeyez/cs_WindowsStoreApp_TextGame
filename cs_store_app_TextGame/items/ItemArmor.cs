﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "ItemArmor", Namespace = "cs_store_app_TextGame")]
    public abstract class ItemArmor : Item 
    {
        [DataMember]
        public int ArmorFactor { get; set; }

        public ItemArmor(XElement itemNode) : base(itemNode)
        {
            ArmorFactor = int.Parse(itemNode.Element("armor-factor").Value);
        }
    }
}
