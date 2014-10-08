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
                    case "container-backpack":
                        SoldItems.Add(ItemTemplates.ItemsContainerBackpack[value]);
                        break;
                    case "container-pouch":
                        SoldItems.Add(ItemTemplates.ItemsContainerPouch[value]);
                        break;
                    case "armor-finger":
                        SoldItems.Add(ItemTemplates.ItemsArmorFinger[value]);
                        break;
                    case "armor-neck":
                        SoldItems.Add(ItemTemplates.ItemsArmorNeck[value]);
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
                // TODO: move these strings to Messages class?
                if (SoldItems.Count == 0) { return "This shop doesn't sell any items."; }
                
                string strItemsString = "This shop sells the following items:\n";
                for (int i = 0; i < SoldItems.Count; i++)
                {
                    int nPrice = (int)(SoldItems[i].Value * SellsAt);
                    strItemsString += "   " + (i + 1).ToString() + ". " + SoldItems[i].Name + " - " + nPrice.ToString() + " gold pieces\n";
                }

                return strItemsString;
            }
        }
        public Paragraph SoldItemsParagraph
        {
            get
            {
                return SoldItemsString.ToParagraph();
            }
        }
        #endregion

        #region Handlers
        public Handler DoPriceItem(Item item, string strKeyword)
        {
            if (item == null) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_ITEM); }
            if (!item.IsKeyword(strKeyword)) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_ITEM); }
            if (!ShopItemTypes.HasFlag(item.Type)) { return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_SHOP); }

            // shop will buy this item type
            int nPrice = (int)(item.Value * BuysAt);

            return Handler.HANDLED(MESSAGE_ENUM.PLAYER_PRICE_ITEM, item.NameAsParagraph, nPrice.ToString().ToParagraph());
        }
        public Handler DoBuyFromEntity(EntityBase entity, string strKeyword)
        {
            EntityHand hand = entity.Hands.GetHandWithItem(strKeyword);
            if (hand == null)
            { 
                // no hand is holding the requested item
                return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_ITEM);
            }
            else
            {
                // hand is holding the requested item
                if(ShopItemTypes.HasFlag(hand.Item.Type))
                {
                    // shop WILL buy this item type
                    int nPrice = (int)(hand.Item.Value * BuysAt);

                    Paragraph itemNameAsParagraph = hand.Item.NameAsParagraph;
                    entity.Gold += nPrice;
                    hand.Item = null;
                    return Handler.HANDLED(MESSAGE_ENUM.PLAYER_SELL_ITEM, itemNameAsParagraph, nPrice.ToString().ToParagraph());
                }
                else
                {
                    // shop WILL NOT buy this item type
                    return Handler.HANDLED(MESSAGE_ENUM.ERROR_BAD_SHOP);
                }
            }
        }
        #endregion
    }
}
