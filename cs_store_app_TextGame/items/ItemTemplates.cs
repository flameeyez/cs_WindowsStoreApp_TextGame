using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    public static class ItemTemplates
    {
        public static List<ItemJunk> ItemsJunk = new List<ItemJunk>();
        public static List<ItemWeapon> ItemsWeapon = new List<ItemWeapon>();
        public static List<ItemFood> ItemsFood = new List<ItemFood>();
        public static List<ItemDrink> ItemsDrink = new List<ItemDrink>();
        public static List<ItemContainerBackpack> ItemsContainerBackpack = new List<ItemContainerBackpack>();
        public static List<ItemContainerPouch> ItemsContainerPouch = new List<ItemContainerPouch>();
        public static List<ItemArmorChest> ItemsArmorChest = new List<ItemArmorChest>();
        public static List<ItemArmorFeet> ItemsArmorFeet = new List<ItemArmorFeet>();
        public static List<ItemArmorHead> ItemsArmorHead = new List<ItemArmorHead>();
        public static List<ItemArmorNeck> ItemsArmorNeck = new List<ItemArmorNeck>();
        public static List<ItemArmorFinger> ItemsArmorFinger = new List<ItemArmorFinger>();
        public static List<ItemArmorShield> ItemsArmorShield = new List<ItemArmorShield>();

        public async static Task Load()
        {
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
        private async static Task LoadItemsDrink()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-drink-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsDrinkDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsDrinkDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsDrink.Add(new ItemDrink(itemNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async static Task LoadItemsFood()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-food-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsFoodDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsFoodDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsFood.Add(new ItemFood(itemNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async static Task LoadItemsJunk()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-junk-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsJunkDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsJunkDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                  select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsJunk.Add(new ItemJunk(itemNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async static Task LoadItemsWeapon()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-weapon-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsWeaponDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsWeaponDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsWeapon.Add(new ItemWeapon(itemNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async static Task LoadItemsContainerBackpack()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-container-backpack-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsContainerDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsContainerDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsContainerBackpack.Add(new ItemContainerBackpack(itemNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async static Task LoadItemsContainerPouch()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-container-pouch-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsContainerDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsContainerDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsContainerPouch.Add(new ItemContainerPouch(itemNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async static Task LoadItemsArmorChest()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-armor-chest-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsArmorChestDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsArmorChestDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsArmorChest.Add(new ItemArmorChest(itemNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async static Task LoadItemsArmorFeet()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-armor-feet-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsArmorFeetDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsArmorFeetDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsArmorFeet.Add(new ItemArmorFeet(itemNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async static Task LoadItemsArmorHead()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-armor-head-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsArmorHeadDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsArmorHeadDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsArmorHead.Add(new ItemArmorHead(itemNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async static Task LoadItemsArmorNeck()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-armor-neck-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsArmorNeckDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsArmorNeckDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsArmorNeck.Add(new ItemArmorNeck(itemNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async static Task LoadItemsArmorFinger()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-armor-finger-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsArmorFingerDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsArmorFingerDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsArmorFinger.Add(new ItemArmorFinger(itemNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async static Task LoadItemsArmorShield()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-armor-shield-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsArmorShieldDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsArmorShieldDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsArmorShield.Add(new ItemArmorShield(itemNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}