using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "ItemContainer", Namespace = "cs_store_app_TextGame")]
    public abstract class ItemContainer : Item
    {
        #region Attributes
        [DataMember]
        public bool Closed { get; set; }
        [DataMember]
        public bool Closable { get; set; }
        [DataMember]
        public int MaximumWeight { get; set; }
        [DataMember]
        public ItemCollection Items = new ItemCollection();
        #endregion

        #region Constructor
        public ItemContainer(XElement itemNode) : base(itemNode)
        {
            Closed = bool.Parse(itemNode.Element("closed").Value);
            Closable = bool.Parse(itemNode.Element("closable").Value);
            MaximumWeight = int.Parse(itemNode.Element("maximum-weight").Value);
        }
        #endregion

        #region Display
        public Paragraph ItemsParagraph
        {
            get
            {
                return Items.ContainerDisplayParagraph(NameAsParagraph);
            }
        }
        #endregion
    }
}