using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    public class Room
    {
        private static int NUMBER_OF_EXITS = 9;
        public int ID { get; set; }
        public string Description { get; set; }
        public Inventory Items = new Inventory();
        public Exit[] Exits = new Exit[NUMBER_OF_EXITS];
        public List<Connection> Connections = new List<Connection>();

        // friendly, unfriendly, and everything in-between
        public List<EntityNPC> NPCs = new List<EntityNPC>();

        public Room()
        {
            for(int i = 0; i < NUMBER_OF_EXITS; i++)
            {
                Exits[i] = new Exit(-1, -1, -1);
            }
        }

        public Room(XElement roomNode) : this()
        {
            ID = int.Parse(roomNode.Element("id").Value);
            Description = roomNode.Element("description").Value;

            // room.exits
            var exitNode = roomNode.Element("exits");
            var northwestNode = exitNode.Element("northwest");
            if (northwestNode != null)
            {
                Exits[0].Region = int.Parse(northwestNode.Element("region").Value);
                Exits[0].Subregion = int.Parse(northwestNode.Element("subregion").Value);
                Exits[0].Room = int.Parse(northwestNode.Element("room").Value);
            }
            var northNode = exitNode.Element("north");
            if (northNode != null)
            {
                Exits[1].Region = int.Parse(northNode.Element("region").Value);
                Exits[1].Subregion = int.Parse(northNode.Element("subregion").Value);
                Exits[1].Room = int.Parse(northNode.Element("room").Value);
            }
            var northeastNode = exitNode.Element("northeast");
            if (northeastNode != null)
            {
                Exits[2].Region = int.Parse(northeastNode.Element("region").Value);
                Exits[2].Subregion = int.Parse(northeastNode.Element("subregion").Value);
                Exits[2].Room = int.Parse(northeastNode.Element("room").Value);
            }
            var westNode = exitNode.Element("west");
            if (westNode != null)
            {
                Exits[3].Region = int.Parse(westNode.Element("region").Value);
                Exits[3].Subregion = int.Parse(westNode.Element("subregion").Value);
                Exits[3].Room = int.Parse(westNode.Element("room").Value);
            }
            var outNode = exitNode.Element("out");
            if (outNode != null)
            {
                Exits[4].Region = int.Parse(outNode.Element("region").Value);
                Exits[4].Subregion = int.Parse(outNode.Element("subregion").Value);
                Exits[4].Room = int.Parse(outNode.Element("room").Value);
            }
            var eastNode = exitNode.Element("east");
            if (eastNode != null)
            {
                Exits[5].Region = int.Parse(eastNode.Element("region").Value);
                Exits[5].Subregion = int.Parse(eastNode.Element("subregion").Value);
                Exits[5].Room = int.Parse(eastNode.Element("room").Value);
            }
            var southwestNode = exitNode.Element("southwest");
            if (southwestNode != null)
            {
                Exits[6].Region = int.Parse(southwestNode.Element("region").Value);
                Exits[6].Subregion = int.Parse(southwestNode.Element("subregion").Value);
                Exits[6].Room = int.Parse(southwestNode.Element("room").Value);
            }
            var southNode = exitNode.Element("south");
            if (southNode != null)
            {
                Exits[7].Region = int.Parse(southNode.Element("region").Value);
                Exits[7].Subregion = int.Parse(southNode.Element("subregion").Value);
                Exits[7].Room = int.Parse(southNode.Element("room").Value);
            }
            var southeastNode = exitNode.Element("southeast");
            if (southeastNode != null)
            {
                Exits[8].Region = int.Parse(southeastNode.Element("region").Value);
                Exits[8].Subregion = int.Parse(southeastNode.Element("subregion").Value);
                Exits[8].Room = int.Parse(southeastNode.Element("room").Value);
            }

            // room.connections
            var connectionNodes = from connections in roomNode
                            .Elements("connections")
                              .Elements("connection")
                                  select connections;
            foreach (var connectionNode in connectionNodes)
            {
                Connections.Add(new Connection(connectionNode));
            }

            // TODO:room.inventory
        }
        public void AddItem(Item itemToAdd)
        {
            Items.Add(itemToAdd);
        }
        public void RemoveItem(Item itemToRemove)
        {
            Items.Remove(itemToRemove);
        }

        public string NPCString
        {
            get
            {
                if (NPCs.Count == 0) { return ""; }

                string strReturn = "You also see ";

                if (NPCs.Count > 2)
                {
                    for (int i = 0; i < NPCs.Count; i++)
                    {
                        strReturn += "a";
                        if ((NPCs[i].Name[0]).IsVowel())
                        {
                            strReturn += "n";
                        }
                        strReturn += " ";
                        strReturn += NPCs[i].Name;
                        if (i == NPCs.Count - 2)
                        {
                            strReturn += ", and ";
                        }
                        else if (i < NPCs.Count - 1)
                        {
                            strReturn += ", ";
                        }
                    }
                }
                else if (NPCs.Count == 2)
                {
                    strReturn += "a";
                    if ((NPCs[0].Name[0]).IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " ";
                    strReturn += NPCs[0].Name;
                    strReturn += " and a";
                    if ((NPCs[1].Name[0]).IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " ";
                    strReturn += NPCs[1].Name;
                }
                else if (NPCs.Count == 1)
                {
                    strReturn += "a";
                    if ((NPCs[0].Name[0]).IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " ";
                    strReturn += NPCs[0].Name;
                }

                strReturn += ".\n";

                return strReturn;
            }
        }

        public string ItemsString
        {
            get
            {
                if (Items.Items.Count == 0) { return ""; }
               	
        		string strReturn = "You also see " + Items.DisplayString() + "\n";
	            return strReturn;
            }
        }

        public string ExitsString
        {
            get
            {
                string strReturn = "";
                for (int i = 0; i < NUMBER_OF_EXITS; i++)
                {
                    if (Exits[i].Region == -1) { continue; }

                    strReturn += StaticMethods.ExitIntegerToStringAbbreviated(i);
                    strReturn += ", ";
                }

                if(strReturn.Length > 0)
                {
                    strReturn = strReturn.Substring(0, strReturn.Length - 2);
                    strReturn = "Obvious exits: " + strReturn + "\n";
                }
                else
                {
                    strReturn = "Obvious exits: none" + "\n";
                }

                return strReturn;
            }
        }

        public string FullDisplayString
        {
            get
            {
                return Description + "\n" + ItemsString + NPCString + ExitsString;
            }
        }

        public Connection GetConnection(string strActionVerb, string strActionNoun)
        {
            foreach(Connection c in Connections)
            {
                if(c.Match(strActionVerb, strActionNoun))
                {
                    return c;
                }
            }

            return null;
        }

        public Item FindItem(string strKeyword, ITEM_TYPE itemType = ITEM_TYPE.ANY)
        {
            return Items.Get(strKeyword, itemType);
        }

        public void AddNPC(EntityNPC npc)
        {
            NPCs.Add(npc);
        }

        public EntityNPC FindNPC(string strWord)
        {
            foreach (EntityNPC npc in NPCs)
            {
                if (npc.IsKeyword(strWord)) { return npc; }
            }

            return null;
        }

        public List<Handler> Update()
        {
            List<Handler> handlers = new List<Handler>();

            for (int i = NPCs.Count - 1; i >= 0; i--)
            {
                Handler handler = NPCs[i].Update();
                if (handler != null) { handlers.Add(handler); }
            }

            return handlers;
        }

        public ExitWithDirection GetRandomExit()
        {
            Random r = new Random(DateTime.Now.Millisecond);

            int nDirection = r.Next(9);
            Exit exit = Exits[nDirection];
            while ( exit.Region == -1) 
            {
                nDirection = r.Next(9);
                exit = Exits[nDirection]; 
            }

            string direction = StaticMethods.ExitIntegerToStringFull(nDirection);

            return new ExitWithDirection(exit, direction);
        }

        public Connection GetRandomConnection()
        {
            if (Connections.Count == 0) { return null; }
            Random r = new Random(DateTime.Now.Millisecond);
            return Connections[r.Next(Connections.Count)];
        }
    }
}
