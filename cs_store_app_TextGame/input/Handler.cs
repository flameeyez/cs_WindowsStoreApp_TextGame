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
        public string StringToAppend { get; set; }
        public Paragraph ParagraphToAppend { get; set; }

        public Handler(RETURN_CODE returnCode, string stringToAppend = "", Paragraph paragraphToAppend = null)
        {
            ReturnCode = returnCode;
            StringToAppend = stringToAppend;
            ParagraphToAppend = paragraphToAppend;
        }

        public static Handler HANDLED = new Handler(RETURN_CODE.HANDLED);
        public static Handler UNHANDLED = new Handler(RETURN_CODE.UNHANDLED);
        public static Handler ERROR_WRONG_DIRECTION = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_WRONG_DIRECTION), Messages.GetMessageAsParagraph(MESSAGE_ENUM.ERROR_WRONG_DIRECTION));
        public static Handler ERROR_BAD_INPUT = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_BAD_INPUT));
        public static Handler ERROR_BAD_ITEM = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_BAD_ITEM));
        public static Handler ERROR_NOT_CARRYING_ITEM = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_NOT_CARRYING_ITEM));
        public static Handler ERROR_HANDS_ARE_FULL = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_HANDS_ARE_FULL));
        public static Handler ERROR_GO_WHERE = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_GO_WHERE));
        public static Handler ERROR_ITEM_NOT_EQUIPPABLE = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_ITEM_NOT_EQUIPPABLE));
        public static Handler ERROR_ALREADY_EQUIPPED = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_ALREADY_EQUIPPED));
        public static Handler ERROR_NOT_A_SHOP = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_NOT_A_SHOP));
        public static Handler ERROR_BAD_SHOP = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_BAD_SHOP));
        public static Handler ERROR_NOT_ENOUGH_GOLD = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_NOT_ENOUGH_GOLD));
        public static Handler ERROR_ALREADY_STANDING = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_ALREADY_STANDING));
        public static Handler ERROR_ALREADY_SITTING = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_ALREADY_SITTING));
        public static Handler ERROR_ALREADY_KNEELING = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_ALREADY_KNEELING));
        public static Handler ERROR_SITTING = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_SITTING));
        public static Handler ERROR_KNEELING = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_KNEELING));
        public static Handler ERROR_PLAYER_IS_DEAD = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_PLAYER_IS_DEAD));
        public static Handler ERROR_NEED_TO_IMPLEMENT = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.ERROR_NEED_TO_IMPLEMENT));

        public override bool Equals(object obj)
        {
            Handler handler = obj as Handler;
            if (obj == null) { return false; }

            return handler.ReturnCode == this.ReturnCode && handler.StringToAppend == this.StringToAppend;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}