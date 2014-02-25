using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    public class Region
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Subregion> Subregions = new List<Subregion>();

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

        public List<Handler> Update()
        {
            List<Handler> handlers = new List<Handler>();

            foreach(Subregion subregion in Subregions)
            {
                List<Handler> subregionHandlers = subregion.Update();
                if (subregionHandlers.Count > 0) { handlers.AddRange(subregionHandlers); }
            }

            return handlers;
        }
    }
}
