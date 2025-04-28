using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GamblerGame
{
    internal class Store
    {
        const int DesiredWidth = 1920;
        const int DesiredHeight = 1080;
        private List<Item> items; // list of all items
        private Inventory inventory;
        private List<Item> storeItems;
        private Vector2 boxOne;
        private Vector2 boxTwo;
        private Vector2 boxThree;
        private Vector2 boxFour;
        private int money;

        public List<Item> StoreItems
        {
            get { return storeItems; }
            set { storeItems = value; }
        }
        public Inventory Inventory
        {
            get { return inventory; }
            set { inventory = value; }
        }
        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        /// <summary>
        /// Intializes Store with a list of items
        /// sets temp items and StoreItems to a new list
        /// </summary>
        /// <param name="items"></param>
        public Store(List<Item> items, int money, Inventory inventory)
        {
            this.items = items;
            storeItems = new List<Item>();
            this.inventory = inventory;
            this.money = money;

            boxOne = new Vector2(DesiredWidth / 40 + (DesiredHeight * .31f)/2, (DesiredHeight / 5));
            boxTwo = new Vector2(DesiredWidth / 40 + DesiredWidth / 1.65f - (DesiredHeight * .31f) * 1.5f, (DesiredHeight / 5));
            boxThree = new Vector2(DesiredWidth / 40 + (DesiredHeight * .31f) / 2, (DesiredHeight * .57f));
            boxFour = new Vector2(DesiredWidth / 40 + DesiredWidth / 1.65f - (DesiredHeight * .31f)*1.5f, (DesiredHeight * .57f));
        }
        public void Update(GameTime gameTime)
        {
            if (storeItems != null)
            {
                for (int i = 0; i < storeItems.Count; i++)
                {
                    storeItems[i].Update(gameTime);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (storeItems != null)
            {
                for(int i = storeItems.Count - 1; i >= 0;i--) 
                {
                        storeItems[i].Draw(spriteBatch);
                }
            }
        }
        /// <summary>
        /// Updates each item in StoreItems
        /// </summary>
        /// <param name="rng"></param>
        /// <param name="gametime"></param>
        public void StoreInteraction(Random rng, GameTime gametime)
        {
            StockStore(rng);
            foreach (Item item in storeItems)
            {
                item.Update(gametime);
                item.BuyButton.OnLeftButtonClick += BuyItem;
            }

            storeItems[0].SetLocations((int)boxOne.X, (int)boxOne.Y);
            storeItems[1].SetLocations((int)boxTwo.X, (int)boxTwo.Y);
            storeItems[2].SetLocations((int)boxThree.X, (int)boxThree.Y);
            storeItems[3].SetLocations((int)boxFour.X, (int)boxFour.Y);

        }

        /// <summary>
        /// Randomly stocks the store with items
        /// </summary>
        /// <param name="rng"></param>
        private void StockStore(Random rng)
        {
            List<int> indexes = new List<int>();
            storeItems = new List<Item>();
            for (int i = 0; i < 4; i++)
            {
                int nextIndex = rng.Next(0, items.Count);
                if (!indexes.Contains(nextIndex))
                {
                    Item item = items[nextIndex];
                    storeItems.Add(item);
                    indexes.Add(nextIndex);
                }
                else
                {
                    i--;
                }
            }
        }

        /// <summary>
        /// Buys the item.
        /// </summary>
        public void BuyItem()
        {
            foreach (Item item in storeItems)
            {
                if (item.Bought && money >= item.Price)
                {
                    inventory.Add(item);
                    money -= item.Price;
                    storeItems.Remove(item);
                    break;
                }
                else
                {
                    item.Bought = false;
                }
            }
        }
        public void Reset()
        {
            inventory = null;
            money = 0;
            storeItems = null;
            foreach (Item item in items)
            {
                item.Bought = false;
            }
        }

        public void UpdateStore(int money, Inventory inventory)
        {
            storeItems = new List<Item>();
            this.inventory = inventory;
            this.money = money;
        }
    }
}
