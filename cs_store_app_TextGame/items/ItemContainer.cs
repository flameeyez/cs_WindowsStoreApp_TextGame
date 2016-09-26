using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame {
    public abstract class ItemContainer : Item {
        #region Attributes
        public bool Closed { get; set; }
        public bool Closable { get; set; }
        public int MaximumWeight { get; set; }
        public ItemCollection Items = new ItemCollection();
        #endregion

        #region Constructor
        protected ItemContainer(ItemContainer template) : base(template) {
            Closed = template.Closed;
            Closable = template.Closable;
            MaximumWeight = template.MaximumWeight;
        }
        public ItemContainer(XElement itemNode) : base(itemNode) {
            Closed = bool.Parse(itemNode.Element("closed").Value);
            Closable = bool.Parse(itemNode.Element("closable").Value);
            MaximumWeight = int.Parse(itemNode.Element("maximum-weight").Value);
        }
        #endregion

        #region Display
        public Paragraph ItemsParagraph {
            get {
                return Items.ContainerDisplayParagraph(NameAsParagraph);
            }
        }
        #endregion
    }
}