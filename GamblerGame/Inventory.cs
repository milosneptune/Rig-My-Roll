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
    internal class Inventory
    {
        private Vector2 boxOne;
        private Vector2 boxTwo;
        private Vector2 boxThree;
        private Vector2 boxFour;
        private Vector2 boxFive;
        private List<Item> items;

        public List<Item> Items { get; private set; }

        public Inventory()
        {

        }

        /// <summary>
        /// Adds item to the item list.
        /// </summary>
        /// <param name="item"></param>
        public void Add(Item item)
        {
            items.Add(item);
        }
        /// <summary>
        /// Removes item from list.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(Item item)
        {
            items.Remove(item);
        }
        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
