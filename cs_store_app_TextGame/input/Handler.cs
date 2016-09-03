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
        public Paragraph Message { get; set; }

        #region Constructor
        public Handler(RETURN_CODE returnCode, MESSAGE_ENUM messageCode, Paragraph p1 = null, Paragraph p2 = null, Paragraph p3 = null, Paragraph p4 = null)
        {
            ReturnCode = returnCode;
            MessageCode = messageCode;
            Message = Messages.Get(messageCode, p1, p2, p3, p4);
        }
        #endregion
        #region Statics
        public static Handler HANDLED(MESSAGE_ENUM message, Paragraph p1 = null, Paragraph p2 = null, Paragraph p3 = null, Paragraph p4 = null)
        {
            return new Handler(RETURN_CODE.HANDLED, message, p1, p2, p3, p4);
        }
        public static Handler UNHANDLED(MESSAGE_ENUM message = MESSAGE_ENUM.NO_MESSAGE, Paragraph p1 = null, Paragraph p2 = null, Paragraph p3 = null, Paragraph p4 = null)
        {
            return new Handler(RETURN_CODE.UNHANDLED, message, p1, p2, p3, p4);
        }
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