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
        private int totalItems;

        // public int Money { get; set; }
        public List<Item> StoreItems { get; private set; }

        /// <summary>
        /// Intializes Store with a list of items
        /// sets temp items and StoreItems to a new list
        /// </summary>
        /// <param name="items"></param>
        public Store(List<Item> items, int totalItems)
        {
            this.items = items;
            this.tempItems = items;
            StoreItems = new List<Item>();
            this.totalItems = totalItems;
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
            }
        }

        /// <summary>
        /// Randomly stocks the store with items
        /// </summary>
        /// <param name="rng"></param>
        private void StockStore(Random rng)
        {
            // TODO: theres a runtime error caused by this 
            for (int i = 0; i < 4; i++)
            {
                int j;
                if (tempItems.Count > 1)
                {
                    j = rng.Next(tempItems.Count - 1);
                }
                else
                {
                    j = rng.Next(tempItems.Count);
                }
                StoreItems.Add(tempItems[j]);
                tempItems.Remove(tempItems[j]);
            }
        }
    }
}
