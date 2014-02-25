using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Handler(RETURN_CODE returnCode, string stringToAppend = "")
        {
            ReturnCode = returnCode;
            StringToAppend = stringToAppend;
        }

        public static Handler HANDLED = new Handler(RETURN_CODE.HANDLED);
        public static Handler UNHANDLED = new Handler(RETURN_CODE.UNHANDLED);
        public static Handler WRONG_DIRECTION = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.WRONG_DIRECTION));
        public static Handler BAD_INPUT = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.BAD_INPUT));
        public static Handler BAD_ITEM = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.BAD_ITEM));
        public static Handler NOT_CARRYING_ITEM = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.NOT_CARRYING_ITEM));
        public static Handler HANDS_ARE_FULL = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.HANDS_ARE_FULL));
        public static Handler GO_WHERE = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.GO_WHERE));
        public static Handler ITEM_NOT_EQUIPPABLE = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.ITEM_NOT_EQUIPPABLE));
        public static Handler ALREADY_EQUIPPED = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.ALREADY_EQUIPPED));
        public static Handler NOT_A_SHOP = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.NOT_A_SHOP));
        public static Handler BAD_SHOP = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.BAD_SHOP));
        public static Handler NOT_ENOUGH_GOLD = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.NOT_ENOUGH_GOLD));
        public static Handler ALREADY_STANDING = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.ALREADY_STANDING));
        public static Handler ALREADY_SITTING = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.ALREADY_SITTING));
        public static Handler ALREADY_KNEELING = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.ALREADY_KNEELING));
        public static Handler SITTING = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.SITTING));
        public static Handler KNEELING = new Handler(RETURN_CODE.HANDLED, Messages.GetErrorMessage(ERROR_MESSAGE_ENUM.KNEELING));

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
