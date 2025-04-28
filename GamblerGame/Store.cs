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
        private List<Item> items; // list of all items
        private List<Item> inventory;
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
        public List<Item> Inventory
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
        public Store(List<Item> items, int money, List<Item> inventory)
        {
            this.items = items;
            storeItems = new List<Item>();
            this.inventory = inventory;
            this.money = money;

            boxOne = new Vector2(100, 50);
            boxTwo = new Vector2(800, 50);
            boxThree = new Vector2(100, 600);
            boxFour = new Vector2(800, 600);
        }
        public void Update(GameTime gameTime)
        {
            if (storeItems != null)
            {
                foreach (Item item in StoreItems)
                {
                    item.Update(gameTime);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (StoreItems != null)
            {
                foreach (Item item in StoreItems)
                {
                    item.Draw(spriteBatch);
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
            storeItems = new List<Item>();
            for (int i = 0; i < 4; i++)
            {
                Item item = items[rng.Next(0, items.Count)];
                storeItems.Add(item);
            }
        }

        /// <summary>
        /// Buys the item.
        /// </summary>
        public void BuyItem()
        {
            foreach (Item item in storeItems)
            {
                if (item.Bought)
                {
                    inventory.Add(item);
                    money -= item.Price;
                    storeItems.Remove(item);
                    break;
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
    }
}
