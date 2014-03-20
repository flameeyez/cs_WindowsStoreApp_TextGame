using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    public static class RichTextManager
    {
        public static LinkedList<Paragraph> Paragraphs = new LinkedList<Paragraph>();
        public static int InlineThreshold = 200;
        private static int _InlineCount = 0;

        public static void AddParagraph(Paragraph p)
        {
            Paragraphs.AddLast(p);
            
            _InlineCount += p.Inlines.Count;
            
            while(_InlineCount > InlineThreshold)
            {
                _InlineCount -= Paragraphs.First.Value.Inlines.Count;
                Paragraphs.RemoveFirst();
            }
        }
    }
}
