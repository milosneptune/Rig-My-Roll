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
        private List<Item> items;
        private List<Item> tempItems;
        public List<Item> storeItems;
        private int money;
        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        public List<Item> StoreItems
        {
            get { return storeItems; }
            private set { storeItems = value; }
        }
        public Store(List<Item> items)
        {
            this.items = items;
            this.tempItems = items;
            storeItems = new List<Item>();
        }
        
        public void StoreInteraction(Random rng, GameTime gametime)
        {
            StockStore(rng);
            foreach (Item item in storeItems)
            {
                item.Update(gametime);
            }
        }

        private void StockStore(Random rng)
        {
            for (int i = 0; i < 4; i++)
            {
                rng.Next(tempItems.Count);
                storeItems.Add(tempItems[i]);
                tempItems.Remove(tempItems[i]);
            }
        }
    }
}
