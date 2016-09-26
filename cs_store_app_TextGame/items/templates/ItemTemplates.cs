using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public static class ItemTemplates {
        public static List<ItemGem> ItemGemTemplates = new List<ItemGem>();
        public static List<ItemJunk> ItemJunkTemplates = new List<ItemJunk>();
        public static List<ItemWeapon> ItemWeaponTemplates = new List<ItemWeapon>();
        public static List<ItemFood> ItemFoodTemplates = new List<ItemFood>();
        public static List<ItemDrink> ItemDrinkTemplates = new List<ItemDrink>();
        public static List<ItemContainerBackpack> ItemContainerBackpackTemplates = new List<ItemContainerBackpack>();
        public static List<ItemContainerPouch> ItemContainerPouchTemplates = new List<ItemContainerPouch>();
        public static List<ItemArmorChest> ItemArmorChestTemplates = new List<ItemArmorChest>();
        public static List<ItemArmorFeet> ItemArmorFeetTemplates = new List<ItemArmorFeet>();
        public static List<ItemArmorHead> ItemArmorHeadTemplates = new List<ItemArmorHead>();
        public static List<ItemArmorNeck> ItemArmorNeckTemplates = new List<ItemArmorNeck>();
        public static List<ItemArmorFinger> ItemArmorFingerTemplates = new List<ItemArmorFinger>();
        public static List<ItemArmorShield> ItemArmorShieldTemplates = new List<ItemArmorShield>();

        public async static Task Load() {
            await LoadItemTemplateGems();
            await LoadItemsContainerBackpack();
            await LoadItemsContainerPouch();
            await LoadItemsDrink();
            await LoadItemsFood();
            await LoadItemsJunk();
            await LoadItemsWeapon();
            await LoadItemsArmorChest();
            await LoadItemsArmorFeet();
            await LoadItemsArmorHead();
            await LoadItemsArmorNeck();
            await LoadItemsArmorFinger();
            await LoadItemsArmorShield();
        }

        private async static Task LoadItemTemplateGems() {
            try {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-gem-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsDrinkDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsDrinkDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes) {
                    ItemGemTemplates.Add(new ItemGem(itemNode));
                }
            }
            catch (Exception e) {
                throw e;
            }
        }

        private async static Task LoadItemsDrink() {
            try {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-drink-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsDrinkDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsDrinkDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes) {
                    ItemDrinkTemplates.Add(new ItemDrink(itemNode));
                }
            }
            catch (Exception e) {
                throw e;
            }
        }
        private async static Task LoadItemsFood() {
            try {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-food-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsFoodDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsFoodDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes) {
                    ItemFoodTemplates.Add(new ItemFood(itemNode));
                }
            }
            catch (Exception e) {
                throw e;
            }
        }
        private async static Task LoadItemsJunk() {
            try {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-junk-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsJunkDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsJunkDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes) {
                    ItemJunkTemplates.Add(new ItemJunk(itemNode));
                }
            }
            catch (Exception e) {
                throw e;
            }
        }
        private async static Task LoadItemsWeapon() {
            try {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-weapon-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsWeaponDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsWeaponDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes) {
                    ItemWeaponTemplates.Add(new ItemWeapon(itemNode));
                }
            }
            catch (Exception e) {
                throw e;
            }
        }
        private async static Task LoadItemsContainerBackpack() {
            try {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-container-backpack-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsContainerDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsContainerDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes) {
                    ItemContainerBackpackTemplates.Add(new ItemContainerBackpack(itemNode));
                }
            }
            catch (Exception e) {
                throw e;
            }
        }
        private async static Task LoadItemsContainerPouch() {
            try {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-container-pouch-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsContainerDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsContainerDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes) {
                    ItemContainerPouchTemplates.Add(new ItemContainerPouch(itemNode));
                }
            }
            catch (Exception e) {
                throw e;
            }
        }
        private async static Task LoadItemsArmorChest() {
            try {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-armor-chest-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsArmorChestDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsArmorChestDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes) {
                    ItemArmorChestTemplates.Add(new ItemArmorChest(itemNode));
                }
            }
            catch (Exception e) {
                throw e;
            }
        }

        internal static Item CloneRandom(ITEM_TYPE itemType) {
            switch (itemType) {
                case ITEM_TYPE.ARMOR_CHEST:
                    return ItemArmorChestTemplates.RandomListItem().Clone();
                case ITEM_TYPE.ARMOR_FEET:
                    return ItemArmorFeetTemplates.RandomListItem().Clone();
                case ITEM_TYPE.ARMOR_FINGER:
                    return ItemArmorFingerTemplates.RandomListItem().Clone();
                case ITEM_TYPE.ARMOR_HEAD:
                    return ItemArmorHeadTemplates.RandomListItem().Clone();
                case ITEM_TYPE.ARMOR_NECK:
                    return ItemArmorNeckTemplates.RandomListItem().Clone();
                case ITEM_TYPE.ARMOR_SHIELD:
                    return ItemArmorShieldTemplates.RandomListItem().Clone();
                case ITEM_TYPE.CONTAINER_BACK:
                    return ItemContainerBackpackTemplates.RandomListItem().Clone();
                case ITEM_TYPE.CONTAINER_WAIST:
                    return ItemContainerPouchTemplates.RandomListItem().Clone();
                case ITEM_TYPE.DRINK:
                    return ItemDrinkTemplates.RandomListItem().Clone();
                case ITEM_TYPE.FOOD:
                    return ItemFoodTemplates.RandomListItem().Clone();
                case ITEM_TYPE.GEM:
                    return ItemGemTemplates.RandomListItem().Clone();
                case ITEM_TYPE.JUNK:
                    return ItemJunkTemplates.RandomListItem().Clone();
                case ITEM_TYPE.WEAPON:
                    return ItemWeaponTemplates.RandomListItem().Clone();
                default:
                    throw new Exception();
                    //return null;
            }
        }

        private async static Task LoadItemsArmorFeet() {
            try {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-armor-feet-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsArmorFeetDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsArmorFeetDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes) {
                    ItemArmorFeetTemplates.Add(new ItemArmorFeet(itemNode));
                }
            }
            catch (Exception e) {
                throw e;
            }
        }
        private async static Task LoadItemsArmorHead() {
            try {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-armor-head-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsArmorHeadDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsArmorHeadDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes) {
                    ItemArmorHeadTemplates.Add(new ItemArmorHead(itemNode));
                }
            }
            catch (Exception e) {
                throw e;
            }
        }
        private async static Task LoadItemsArmorNeck() {
            try {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-armor-neck-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsArmorNeckDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsArmorNeckDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes) {
                    ItemArmorNeckTemplates.Add(new ItemArmorNeck(itemNode));
                }
            }
            catch (Exception e) {
                throw e;
            }
        }
        private async static Task LoadItemsArmorFinger() {
            try {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-armor-finger-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsArmorFingerDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsArmorFingerDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes) {
                    ItemArmorFingerTemplates.Add(new ItemArmorFinger(itemNode));
                }
            }
            catch (Exception e) {
                throw e;
            }
        }
        private async static Task LoadItemsArmorShield() {
            try {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-armor-shield-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsArmorShieldDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsArmorShieldDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes) {
                    ItemArmorShieldTemplates.Add(new ItemArmorShield(itemNode));
                }
            }
            catch (Exception e) {
                throw e;
            }
        }
    }
}