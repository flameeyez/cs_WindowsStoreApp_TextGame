using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    public enum RETURN_CODE
    {
        HANDLED,
        UNHANDLED
    }

    public class Handler
    {
        public RETURN_CODE ReturnCode { get; set; }
        public MESSAGE_ENUM MessageCode { get; set; }
        public string StringToAppend { get; set; }
        public Paragraph ParagraphToAppend { get; set; }

        #region Constructor
        //public Handler(RETURN_CODE returnCode, string stringToAppend = "", Paragraph paragraphToAppend = null)
        //{
        //    ReturnCode = returnCode;
        //    StringToAppend = stringToAppend;
        //    ParagraphToAppend = paragraphToAppend;
        //}

        public Handler(RETURN_CODE returnCode, MESSAGE_ENUM messageCode, Paragraph p1 = null, Paragraph p2 = null, Paragraph p3 = null)
        {
            ReturnCode = returnCode;
            MessageCode = messageCode;
            ParagraphToAppend = Messages.GetMessageAsParagraph(messageCode, p1, p2, p3);
        }
        #endregion
        #region Statics
        public static Handler Default(MESSAGE_ENUM message)
        {
            return new Handler(RETURN_CODE.HANDLED, message);
        }
        public static Handler UNHANDLED = new Handler(RETURN_CODE.UNHANDLED, MESSAGE_ENUM.NO_MESSAGE);
        #endregion
        #region Equals
        public override bool Equals(object obj)
        {
            Handler handler = obj as Handler;
            if (obj == null) { return false; }

            return handler.ReturnCode == this.ReturnCode && handler.MessageCode == this.MessageCode; // handler.StringToAppend == this.StringToAppend;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}