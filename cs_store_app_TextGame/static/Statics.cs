using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Windows.UI.Xaml.Documents;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace cs_store_app_TextGame
{
    public static class Statics
    {
        // DEBUG
        public static int DebugNPCCount = 5;
        public static int DebugItemPasses = 2;

        public static int RunningInlineCount = 0;
        public static int ItemCount = 0;
        public static int EntityCount = 0;
        // END DEBUG

        public static Random r = new Random(DateTime.Now.Millisecond);
        public static Dictionary<string, int> OrdinalStringToInt = new Dictionary<string, int>();

        #region Static Constructor
        static Statics()
        {
            OrdinalStringToInt.Add("first", 0);
            OrdinalStringToInt.Add("second", 1);
            OrdinalStringToInt.Add("third", 2);
            OrdinalStringToInt.Add("fourth", 3);
            OrdinalStringToInt.Add("fifth", 4);
            OrdinalStringToInt.Add("sixth", 5);
            OrdinalStringToInt.Add("seventh", 6);
            OrdinalStringToInt.Add("eighth", 7);
            OrdinalStringToInt.Add("ninth", 8);
            OrdinalStringToInt.Add("tenth", 9);
        }
        #endregion

        #region String Operations
        public static string ToSentenceCase(this string str)
        {
            if (str.Length == 0) { return str; }
            if (str.Length == 1) { return char.ToUpper(str[0]).ToString(); }
            return char.ToUpper(str[0]) + str.Substring(1);
        }
        public static Run ToRun(this string s)
        {
            return new Run { Foreground = new SolidColorBrush(Colors.Orange), Text = s };
        }
        public static Run ToRun(this string s, Color c)
        {
            return new Run { Foreground = new SolidColorBrush(c), Text = s };
        }
        public static Paragraph ToParagraph(this string s)
        {
            Paragraph p = new Paragraph();
            p.Inlines.Add(s.ToRun());
            return p;
        }
        public static string IndefiniteArticle(this string s, bool bCapitalize = false)
        {
            if (s.Length == 0) { return ""; }
            return (bCapitalize ? "A" : "a") + (s[0].IsVowel() ? "n" : "") + " ";
        }
        #endregion

        #region Run Operations
        public static void Merge(this Run r1, Run r2)
        {
            if (r2 == null) { return; }
            r1.Text += " " + r2.Text;
        }
        #endregion

        #region Paragraph Operations
        public static void Compress(this Paragraph p)
        {
            if (p.Inlines.Count <= 1) { return; }

            for (int i = p.Inlines.Count - 1; i > 0; i--)
            {
                Run current = p.Inlines[i - 1] as Run;
                Run next = p.Inlines[i] as Run;

                SolidColorBrush currentBrush = current.Foreground as SolidColorBrush;
                SolidColorBrush nextBrush = next.Foreground as SolidColorBrush;

                if (currentBrush.Color.Equals(nextBrush.Color))
                {
                    current.Text += next.Text;
                    p.Inlines.Remove(next);
                }
            }
        }
        public static Paragraph Clone(this Paragraph p)
        {
            Paragraph clone = new Paragraph();
            foreach (Inline i in p.Inlines)
            {
                Run r = i as Run;
                Run cloneRun = new Run();
                cloneRun.Text = r.Text;
                cloneRun.Foreground = r.Foreground;
                clone.Inlines.Add(cloneRun);
            }
            return clone;
        }
        public static void Merge(this Paragraph p1, Paragraph p2)
        {
            if (p2 == null) { return; }

            while (p2.Inlines.Count > 0)
            {
                Inline i = p2.Inlines[0];
                p2.Inlines.Remove(i);
                p1.Inlines.Add(i);
            }
        }
        #endregion

        #region Char Operations
        public static bool IsVowel(this char character)
        {
            return new[] { 'a', 'e', 'i', 'o', 'u' }.Contains(char.ToLower(character));
        }
        #endregion



        public static string ExitIntegerToStringAbbreviated(int nDirection)
        {
            switch(nDirection)
            {
                case 0:
                    return "nw";
                case 1:
                    return "n";
                case 2:
                    return "ne";
                case 3:
                    return "w";
                case 4:
                    return "out";
                case 5:
                    return "e";
                case 6:
                    return "sw";
                case 7:
                    return "s";
                case 8:
                    return "se";
                default:
                    return "";
            }
        }
        public static string ExitIntegerToStringFull(int nDirection)
        {
            switch (nDirection)
            {
                case 0:
                    return "northwest";
                case 1:
                    return "north";
                case 2:
                    return "northeast";
                case 3:
                    return "west";
                case 4:
                    return "out";
                case 5:
                    return "east";
                case 6:
                    return "southwest";
                case 7:
                    return "south";
                case 8:
                    return "southeast";
                default:
                    return "";
            }
        }
        public static int DirectionToInt(string strInput)
        {
	        int nReturn = -1;

	        if(strInput == "northwest" || strInput == "nw")			{ nReturn = 0; }
	        else if(strInput == "north" || strInput == "n")			{ nReturn = 1; }
	        else if(strInput == "northeast" || strInput == "ne")	{ nReturn = 2; }
	        else if(strInput == "west" || strInput == "w")			{ nReturn = 3; }
	        else if(strInput == "out" || strInput == "o")			{ nReturn = 4; }
	        else if(strInput == "east" || strInput == "e")			{ nReturn = 5; }
	        else if(strInput == "southwest" || strInput == "sw")	{ nReturn = 6; }
	        else if(strInput == "south" || strInput == "s")			{ nReturn = 7; }
	        else if(strInput == "southeast" || strInput == "se")	{ nReturn = 8; }

	        return nReturn;
        }
        public static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(a.GetType());
                serializer.WriteObject(stream, a);
                stream.Position = 0;
                T returnObject = (T)serializer.ReadObject(stream);

                Item item = returnObject as Item;
                if (item != null)
                {
                    //item.UID = Guid.NewGuid();
                    item.NID = Statics.ItemCount++;
                }

                return returnObject;
            }
        }

        // TODO: implement entity levels, experience (npcs gain experience?)
        public static Color LevelToColor(int level)
        {
            int delta = 10 - r.Next(20); // level - Game.Player.Level;
            if (delta > 5) { return Colors.Purple; }
            else if (delta > 3) { return Colors.Red; }
            else if (delta > 1) { return Colors.Orange; }
            else if (delta == 0) { return Colors.Yellow; }
            else if (delta > -2) { return Colors.Blue; }
            else if (delta > -3) { return Colors.LightGreen; }
            else { return Colors.Gray; }
        }
    }
}
