using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    public class Room
    {
        #region Attributes
        public int ID { get; set; }

        private string _description { get; set; }
        public string Description
        {
            get
            {
                return _description + "\n";
            }
            set
            {
                _description = value;
            }
        }

        public ExitCollection Exits = new ExitCollection();
        public ItemCollection Items = new ItemCollection();
        public ConnectionCollection Connections = new ConnectionCollection();
        public EntityNPCCollection NPCs = new EntityNPCCollection();

        public DateTime EmptyRoomTimer = DateTime.Now;

        #endregion

        #region Constructors
        public Room()
        {
            for (int i = 0; i < ExitCollection.NUMBER_OF_EXITS; i++)
            {
                Exits.Set(i, new Exit(-1, -1, -1));
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
                int nRegion = int.Parse(northwestNode.Element("region").Value);
                int nSubregion = int.Parse(northwestNode.Element("subregion").Value);
                int nRoom = int.Parse(northwestNode.Element("room").Value);

                Exits.Set(0, new Exit(nRegion, nSubregion, nRoom));
            }
            var northNode = exitNode.Element("north");
            if (northNode != null)
            {
                int nRegion = int.Parse(northNode.Element("region").Value);
                int nSubregion = int.Parse(northNode.Element("subregion").Value);
                int nRoom = int.Parse(northNode.Element("room").Value);

                Exits.Set(1, new Exit(nRegion, nSubregion, nRoom));
            }
            var northeastNode = exitNode.Element("northeast");
            if (northeastNode != null)
            {
                int nRegion = int.Parse(northeastNode.Element("region").Value);
                int nSubregion = int.Parse(northeastNode.Element("subregion").Value);
                int nRoom = int.Parse(northeastNode.Element("room").Value);

                Exits.Set(2, new Exit(nRegion, nSubregion, nRoom));
            }
            var westNode = exitNode.Element("west");
            if (westNode != null)
            {
                int nRegion = int.Parse(westNode.Element("region").Value);
                int nSubregion = int.Parse(westNode.Element("subregion").Value);
                int nRoom = int.Parse(westNode.Element("room").Value);

                Exits.Set(3, new Exit(nRegion, nSubregion, nRoom));
            }
            var outNode = exitNode.Element("out");
            if (outNode != null)
            {
                int nRegion = int.Parse(outNode.Element("region").Value);
                int nSubregion = int.Parse(outNode.Element("subregion").Value);
                int nRoom = int.Parse(outNode.Element("room").Value);

                Exits.Set(4, new Exit(nRegion, nSubregion, nRoom));
            }
            var eastNode = exitNode.Element("east");
            if (eastNode != null)
            {
                int nRegion = int.Parse(eastNode.Element("region").Value);
                int nSubregion = int.Parse(eastNode.Element("subregion").Value);
                int nRoom = int.Parse(eastNode.Element("room").Value);

                Exits.Set(5, new Exit(nRegion, nSubregion, nRoom));
            }
            var southwestNode = exitNode.Element("southwest");
            if (southwestNode != null)
            {
                int nRegion = int.Parse(southwestNode.Element("region").Value);
                int nSubregion = int.Parse(southwestNode.Element("subregion").Value);
                int nRoom = int.Parse(southwestNode.Element("room").Value);

                Exits.Set(6, new Exit(nRegion, nSubregion, nRoom));
            }
            var southNode = exitNode.Element("south");
            if (southNode != null)
            {
                int nRegion = int.Parse(southNode.Element("region").Value);
                int nSubregion = int.Parse(southNode.Element("subregion").Value);
                int nRoom = int.Parse(southNode.Element("room").Value);

                Exits.Set(7, new Exit(nRegion, nSubregion, nRoom));
            }
            var southeastNode = exitNode.Element("southeast");
            if (southeastNode != null)
            {
                int nRegion = int.Parse(southeastNode.Element("region").Value);
                int nSubregion = int.Parse(southeastNode.Element("subregion").Value);
                int nRoom = int.Parse(southeastNode.Element("room").Value);

                Exits.Set(8, new Exit(nRegion, nSubregion, nRoom));
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
        #endregion

        #region Display
        public Paragraph FullDisplayParagraph
        {
            get
            {
                Paragraph p = new Paragraph();

                p.Inlines.Add(Description.ToRun());

                p.Merge(Items.RoomDisplayParagraph);
                p.Merge(NPCs.RoomDisplayParagraph);
                p.Merge(Exits.RoomDisplayParagraph);
                
                return p;
            }
        }
        #endregion

        public List<Handler> Update()
        {
            List<Handler> handlers = new List<Handler>();

            if (!this.Equals(Game.Player.CurrentRoom))
            {
                DateTime now = DateTime.Now;
                TimeSpan delta = now - EmptyRoomTimer;
                if (delta.TotalMilliseconds > Statics.EmptyRoomCleanupThreshold)
                {
                    EmptyRoomTimer = now;
                    Cleanup();
                    // handlers.Add(new Handler(RETURN_CODE.HANDLED, MESSAGE_ENUM.DEBUG_ROOM_CLEANUP, ID.ToString().ToParagraph()));
                }
            }
                        
            handlers.AddRange(NPCs.Update());
            return handlers;
        }

        public EntityNPC GetRandomHostile(EntityNPC source, bool bMustBeAlive = false)
        {
            return NPCs.GetRandomHostile(source, bMustBeAlive);
        }

        public void Cleanup()
        {
            // TODO: stagger? is cleanup on all rooms OK?
            // TODO: dynamic time threshold based on item, npc count?
            Items.Cleanup();
            NPCs.Cleanup();
        }

        public void ResetEmptyRoomTimer()
        {
            EmptyRoomTimer = DateTime.Now;
        }
    }
}