using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public class Exit
    {
        public int Region { get; set; }
        public int Subregion { get; set; }
        public int Room { get; set; }

        public Exit(int nRegion, int nSubregion, int nRoom)
        {
            Region = nRegion;
            Subregion = nSubregion;
            Room = nRoom;
        }
    }
}
