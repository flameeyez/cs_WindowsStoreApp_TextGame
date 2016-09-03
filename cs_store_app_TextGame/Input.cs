using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame {
    public static class Input {
        private static Queue<ParsedInput> Queue = new Queue<ParsedInput>();
        public static object Lock = new object();

        public static int Count { get { return Queue.Count; } }
        public static void Enqueue(ParsedInput p) { Queue.Enqueue(p); }
        public static ParsedInput Dequeue() {
            ParsedInput p = Queue.Dequeue();
            History.Add(p.String);
            return p;
        }

        public static class History {
            private static List<string> Strings = new List<string>();
            private static int Index = 0;

            public static int Count { get { return Strings.Count; } }

            public static string Previous() {
                if (Count == 0) { return string.Empty; }
                if (Index > 0) { Index--; }
                return Strings[Index];
            }

            public static string Next() {
                if (Count == 0) { return string.Empty; }
                Index++;
                if (Index > Strings.Count - 1) { Index = Strings.Count - 1; }
                return Strings[Index];
            }

            public static void Add(string s) {
                Strings.Add(s);
                Index = Strings.Count;
            }
        }
    }
}
