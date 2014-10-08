using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    public class Region
    {
        #region Attributes
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Subregion> Subregions = new List<Subregion>();
        #endregion

        #region Constructor
        public Region(XElement regionNode)
        {
            ID = int.Parse(regionNode.Element("id").Value);
            Name = regionNode.Element("name").Value;

            // region.subregions
            var subregionNodes = from subregions in regionNode
                                .Elements("subregions")
                                  .Elements("subregion") select subregions;
            foreach (var subregionNode in subregionNodes)
            {
                Subregions.Add(new Subregion(subregionNode));
            }
        }
        #endregion

        public void Update()
        {
            foreach(Subregion subregion in Subregions)
            {
                subregion.Update();
            }
        }
    }
}
