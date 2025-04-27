using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GamblerGame
{
    internal class Store
    {
        private List<Item> items; // list of all items
        private List<Item> tempItems; // temporary list of items 
        private List<Item> inventory;
        private int money;

        // public int Money { get; set; }
        public List<Item> StoreItems { get; private set; }
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
            this.tempItems = items;
            StoreItems = new List<Item>();
            this.inventory = inventory;
            this.money = money;
        }
        
        /// <summary>
        /// Updates each item in StoreItems
        /// </summary>
        /// <param name="rng"></param>
        /// <param name="gametime"></param>
        public void StoreInteraction(Random rng, GameTime gametime)
        {
            StockStore(rng);
            foreach (Item item in StoreItems)
            {
                item.Update(gametime);
                item.BuyButton.OnLeftButtonClick += BuyItem;
            }
        }

        /// <summary>
        /// Randomly stocks the store with items
        /// </summary>
        /// <param name="rng"></param>
        private void StockStore(Random rng)
        {
            // TODO: theres a runtime error caused by this 
            for (int i = 0; i < 5; i++)
            {
                rng.Next(tempItems.Count);
                StoreItems.Add(tempItems[i]);
            }
        }

        /// <summary>
        /// Buys the item.
        /// </summary>
        public void BuyItem()
        {
            foreach (Item item in StoreItems)
            {
                if (item.Bought)
                {
                    inventory.Add(item);
                    money -= item.Price;
                    StoreItems.Remove(item);
                }
            }
        }
    }
}
