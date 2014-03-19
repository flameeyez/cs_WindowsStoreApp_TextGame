using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    public class ConnectionCollection
    {
        private List<Connection> Connections = new List<Connection>();
        public Connection Find(string strActionVerb, string strActionNoun)
        {
            foreach (Connection c in Connections)
            {
                if (c.Match(strActionVerb, strActionNoun))
                {
                    return c;
                }
            }

            return null;
        }
        public Connection Random()
        {
            if (Connections.Count == 0) { return null; }
            Random r = new Random(DateTime.Now.Millisecond);
            return Connections[r.Next(Connections.Count)];
        }
        public void Add(Connection c)
        {
            Connections.Add(c);
        }
    }
}
