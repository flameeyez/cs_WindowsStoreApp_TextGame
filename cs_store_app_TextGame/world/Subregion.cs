using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    public class Subregion
    {
        #region Attributes
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Room> Rooms = new List<Room>();
        #endregion

        #region Constructor
        public Subregion(XElement regionNode)
        {
            ID = int.Parse(regionNode.Element("id").Value);
            Name = regionNode.Element("name").Value;

            // region.rooms
            var roomNodes = from rooms in regionNode
                                .Elements("rooms")
                                  .Elements("room")
                            select rooms;
            foreach (var roomNode in roomNodes)
            {
                XElement shop = roomNode.Element("shop");
                if (shop == null) { Rooms.Add(new Room(roomNode)); }
                else
                {
                    Rooms.Add(new RoomShop(roomNode));
                }
            }
        }
        #endregion

        public void Update()
        {
            foreach(Room room in Rooms)
            {
                room.Update();
            }
        }
    }
}
