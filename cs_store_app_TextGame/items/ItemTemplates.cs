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
        public static List<ItemContainer> ItemsContainer = new List<ItemContainer>();
        public static List<ItemArmorChest> ItemsArmorChest = new List<ItemArmorChest>();
        public static List<ItemArmorFeet> ItemsArmorFeet = new List<ItemArmorFeet>();
        public static List<ItemArmorHead> ItemsArmorHead = new List<ItemArmorHead>();
        public static List<ItemAccessoryAmulet> ItemsAccessoryAmulet = new List<ItemAccessoryAmulet>();
        public static List<ItemAccessoryRing> ItemsAccessoryRing = new List<ItemAccessoryRing>();
        public static List<ItemArmorShield> ItemsArmorShield = new List<ItemArmorShield>();

        public async static Task Load()
        {
            await LoadItemsContainer();
            await LoadItemsDrink();
            await LoadItemsFood();
            await LoadItemsJunk();
            await LoadItemsWeapon();
            await LoadItemsArmorChest();
            await LoadItemsArmorFeet();
            await LoadItemsArmorHead();
            await LoadItemsAccessoryAmulet();
            await LoadItemsAccessoryRing();
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
        private async static Task LoadItemsContainer()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-container-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsContainerDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsContainerDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsContainer.Add(new ItemContainer(itemNode));
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
        private async static Task LoadItemsAccessoryAmulet()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-accessory-amulet-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsAccessoryAmuletDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsAccessoryAmuletDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsAccessoryAmulet.Add(new ItemAccessoryAmulet(itemNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async static Task LoadItemsAccessoryRing()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\templates");
                var file = await folder.GetFileAsync("item-accessory-ring-templates.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument itemsAccessoryRingDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var itemNodes = from itemTemplates in itemsAccessoryRingDocument
                                      .Elements("item-templates")
                                        .Elements("item-template")
                                select itemTemplates;
                foreach (var itemNode in itemNodes)
                {
                    ItemsAccessoryRing.Add(new ItemAccessoryRing(itemNode));
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