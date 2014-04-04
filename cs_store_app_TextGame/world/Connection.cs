using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    public class Connection
    {
        public int DestinationRegion { get; set; }
        public int DestinationSubregion { get; set; }
        public int DestinationRoom { get; set; }
        public string ActionVerb { get; set; }
        public string ActionNoun { get; set; }

        // The goblin heads <NPCDisplayString>.
        // The goblin heads <through the gates>.
        public string NPCDisplayString { get; set; }

        // END NEW

        public Connection(XElement connectionNode)
        {
            DestinationRegion = int.Parse(connectionNode.Element("region").Value);
            DestinationSubregion = int.Parse(connectionNode.Element("subregion").Value);
            DestinationRoom = int.Parse(connectionNode.Element("room").Value);
            ActionVerb = connectionNode.Element("action-verb").Value;
            ActionNoun = connectionNode.Element("action-noun").Value;
            NPCDisplayString = connectionNode.Element("npc-display").Value;
        }
        public bool Match(string actionVerb, string actionNoun)
        {
            return (actionVerb == ActionVerb && actionNoun == ActionNoun);
        }
    }
}
