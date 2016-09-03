using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame {
    public static class MessageQueue {
        // any displayed text is routed through this queue; UI thread checks this queue during each update
        private static Queue<Paragraph> _queue = new Queue<Paragraph>();

        public static void Enqueue(Paragraph p) { _queue.Enqueue(p); }
        public static Paragraph Dequeue() { return _queue.Dequeue(); }
        public static int Count { get { return _queue.Count; } }

        public static object Lock = new object();
    }
}
