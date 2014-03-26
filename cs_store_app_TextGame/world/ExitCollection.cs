using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    public class ExitCollection
    {
        #region Attributes
        public static int NUMBER_OF_EXITS = 9;
        private Exit[] Exits = new Exit[NUMBER_OF_EXITS];
        private string RoomDisplayString
        {
            get
            {
                string strReturn = "";
                for (int i = 0; i < NUMBER_OF_EXITS; i++)
                {
                    if (Exits[i].Region == -1) { continue; }

                    strReturn += Statics.ExitIntegerToStringAbbreviated(i);
                    strReturn += ", ";
                }

                if (strReturn.Length > 0)
                {
                    strReturn = strReturn.Substring(0, strReturn.Length - 2);
                    strReturn = "Obvious exits: " + strReturn;
                }
                else
                {
                    strReturn = "Obvious exits: none";
                }

                return strReturn;
            }
        }
        public Paragraph RoomDisplayParagraph
        {
            get
            {
                return RoomDisplayString.ToParagraph();
            }
        }
        #endregion

        #region Methods
        public Exit Get(int nDirection)
        {
            if (nDirection < 0 || nDirection >= NUMBER_OF_EXITS) { return null; }
            return Exits[nDirection];
        }
        public void Set(int nDirection, Exit exit)
        {
            if (nDirection < 0 || nDirection >= NUMBER_OF_EXITS) { return; }
            Exits[nDirection] = exit;
        }
        public Exit Random()
        {
            Random r = new Random(DateTime.Now.Millisecond);

            int nDirection = r.Next(9);
            Exit exit = Get(nDirection);
            while (exit.Region == -1)
            {
                nDirection = r.Next(9);
                exit = Get(nDirection);
            }

            return exit;
        }
        public ExitWithDirection RandomWithDirection()
        {
            Random r = new Random(DateTime.Now.Millisecond);

            int nDirection = r.Next(9);
            Exit exit = Get(nDirection);
            while (exit.Region == -1)
            {
                nDirection = r.Next(9);
                exit = Get(nDirection);
            }

            string direction = Statics.ExitIntegerToStringFull(nDirection);

            return new ExitWithDirection(exit, direction);
        }
        #endregion
    }
}
