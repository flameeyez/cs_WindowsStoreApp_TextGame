using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    public class WorldCoordinates
    {
        public WorldCoordinates() { Set(0, 0, 0); }



        public int RegionIndex { get; set; }
        public int SubregionIndex { get; set; }
        public int RoomIndex { get; set; }
        


        private Room _currentroom;
        public Room CurrentRoom
        {
            get
            {
                return _currentroom;
            }
        }
        private Subregion _currentsubregion;
        public Subregion CurrentSubregion
        {
            get
            {
                return _currentsubregion;
            }
        }
        private Region _currentregion;
        public Region CurrentRegion
        {
            get
            {
                return _currentregion;
            }
        }



        public void Set(int region, int subregion, int room)
        {
            RegionIndex = region;
            SubregionIndex = subregion;
            RoomIndex = room;

            _currentregion = World.Regions[RegionIndex];
            _currentsubregion = CurrentRegion.Subregions[SubregionIndex];
            _currentroom = CurrentSubregion.Rooms[RoomIndex];
        }
        public void Set(Connection connection)
        {
            Set(connection.DestinationRegion, connection.DestinationSubregion, connection.DestinationRoom);
        }

        

        public Paragraph CurrentRegionParagraph
        {
            get
            {
                Paragraph p = new Paragraph();
                p.Inlines.Add(("[" + CurrentRegion.Name + " - " + CurrentSubregion.Name + "]\n").ToRun());
                return p;
            }
        }
        public Paragraph CurrentRoomDisplayParagraph
        {
            get
            {
                Paragraph p = CurrentRegionParagraph;
                p.Merge(CurrentRoom.FullDisplayParagraph);
                return p;
            }
        }
    }
}
