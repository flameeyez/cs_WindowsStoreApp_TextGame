using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    public class RoomShop : Room
    {
        #region Attributes
        public List<Item> SoldItems = new List<Item>();
        public ITEM_TYPE ShopItemTypes { get; set; }
        public double BuysAt { get; set; }
        public double SellsAt { get; set; }
        #endregion

        #region Constructor
        public RoomShop(XElement roomNode) : base(roomNode) 
        {
            var shopNode = roomNode.Element("shop");

            BuysAt = double.Parse(shopNode.Element("buys-at").Value);
            SellsAt = double.Parse(shopNode.Element("sells-at").Value);

            var itemTypeNodes = from itemTypes in shopNode
                .Elements("item-types")
                  .Elements("item-type")
                                  select itemTypes;
            foreach (var itemTypeNode in itemTypeNodes)
            {
                ShopItemTypes |= Item.StringToEnum[itemTypeNode.Value];
            }

            var soldItemsNode = shopNode.Element("sold-items");
            foreach(var item in soldItemsNode.Elements())
            {
                int value = int.Parse(item.Value);
                switch(item.Name.LocalName)
                {
                    case "weapon":
                        SoldItems.Add(ItemTemplates.ItemsWeapon[value]);
                        break;
                    case "armor-chest":
                        SoldItems.Add(ItemTemplates.ItemsArmorChest[value]);
                        break;
                    case "armor-feet":
                        SoldItems.Add(ItemTemplates.ItemsArmorFeet[value]);
                        break;
                    case "armor-head":
                        SoldItems.Add(ItemTemplates.ItemsArmorHead[value]);
                        break;
                    case "junk":
                        SoldItems.Add(ItemTemplates.ItemsJunk[value]);
                        break;
                    case "container":
                        SoldItems.Add(ItemTemplates.ItemsContainer[value]);
                        break;
                    case "accessory-ring":
                        SoldItems.Add(ItemTemplates.ItemsAccessoryRing[value]);
                        break;
                    case "accessory-amulet":
                        SoldItems.Add(ItemTemplates.ItemsAccessoryAmulet[value]);
                        break;
                    case "food":
                        SoldItems.Add(ItemTemplates.ItemsFood[value]);
                        break;
                    case "drink":
                        SoldItems.Add(ItemTemplates.ItemsDrink[value]);
                        break;
                    default:
                        break;
                }
            }

            var itemNodes = from items in shopNode
                .Elements("sold-items")
                  .Elements("sold-item")
                                select items;
            foreach (var itemTypeNode in itemTypeNodes)
            {
                ShopItemTypes |= Item.StringToEnum[itemTypeNode.Value];
            }
        }
        #endregion

        #region Display
        public string SoldItemsString
        {
            get
            {
                if (SoldItems.Count == 0) { return "This shop doesn't sell any items."; }
                
                string strItemsString = "This shop sells the following items:\n";
                for (int i = 0; i < SoldItems.Count; i++)
                {
                    int nPrice = (int)(SoldItems[i].Value * SellsAt);
                    strItemsString += "   " + (i + 1).ToString() + ". " + SoldItems[i].Name + " - " + nPrice.ToString() + " gold pieces\n";
                }
                strItemsString += "\n";
                return strItemsString;
            }
        }
        public Paragraph SoldItemsParagraph
        {
            get
            {
                Paragraph p = new Paragraph();
                p.Inlines.Add(SoldItemsString.ToRun());
                return p;
            }
        }
        #endregion

        #region Handlers
        public Handler DoPriceItem(Item item, string strKeyword)
        {
            if (item == null) { return Handler.Default(MESSAGE_ENUM.ERROR_BAD_ITEM); }
            if (!item.IsKeyword(strKeyword)) { return Handler.Default(MESSAGE_ENUM.ERROR_BAD_ITEM); }
            if (!ShopItemTypes.HasFlag(item.Type)) { return Handler.Default(MESSAGE_ENUM.ERROR_BAD_SHOP); }

            // shop will buy this item type
            int nPrice = (int)(item.Value * BuysAt);

            return new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_PRICE_ITEM, item.Name, nPrice.ToString()));
        }
        public Handler DoBuyFromEntity(Entity entity, string strKeyword)
        {
            bool bValidItem = false;
            Handler handler = Handler.UNHANDLED;

            if (entity.RightHand != null && entity.RightHand.IsKeyword(strKeyword))
            {
                // valid item; attempt to sell
                bValidItem = true;

                if (ShopItemTypes.HasFlag(entity.RightHand.Type))
                {
                    // shop will buy this item type
                    int nPrice = (int)(entity.RightHand.Value * BuysAt);
                    handler = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_SELL_ITEM, entity.RightHand.Name, nPrice.ToString()));

                    entity.Gold += nPrice;
                    entity.RightHand = null;
                }
            }

            if (handler.ReturnCode == RETURN_CODE.UNHANDLED && entity.LeftHand != null && entity.LeftHand.IsKeyword(strKeyword))
            {
                // valid item; attempt to sell
                bValidItem = true;

                if (ShopItemTypes.HasFlag(entity.LeftHand.Type))
                {
                    // shop will buy this item type
                    int nPrice = (int)(entity.LeftHand.Value * BuysAt);
                    handler = new Handler(RETURN_CODE.HANDLED, Messages.GetMessage(MESSAGE_ENUM.PLAYER_SELL_ITEM, entity.LeftHand.Name, nPrice.ToString()));

                    entity.Gold += nPrice;
                    entity.LeftHand = null;
                }
            }

            if (handler.ReturnCode == RETURN_CODE.UNHANDLED)
            {
                if (bValidItem)
                {
                    // item found, but shop wouldn't buy
                    return Handler.Default(MESSAGE_ENUM.ERROR_BAD_SHOP);
                }
                else
                {
                    // item not found
                    return Handler.Default(MESSAGE_ENUM.ERROR_BAD_ITEM);
                }
            }

            return handler;
        }
        #endregion
    }
}
