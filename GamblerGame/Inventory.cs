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
        const int DesiredWidth = 1920;
        const int DesiredHeight = 1080;
        private Vector2 boxOne;
        private Vector2 boxTwo;
        private Vector2 boxThree;
        private Vector2 boxFour;
        private Vector2 boxFive;
        private List<Item> items;

        public List<Item> Items
        {
            get { return items; }
            set { items = value; }
        }

        public Inventory()
        {
            boxOne = new Vector2(DesiredWidth / 40 + (DesiredHeight * .31f) / 2, (DesiredHeight / 5));
            boxTwo = new Vector2(DesiredWidth / 40 + DesiredWidth / 1.65f - (DesiredHeight * .31f) * 1.5f, (DesiredHeight / 5));
            boxThree = new Vector2(DesiredWidth / 40 + (DesiredHeight * .31f) / 2, (DesiredHeight * .57f));
            boxFour = new Vector2(DesiredWidth / 40 + DesiredWidth / 1.65f - (DesiredHeight * .31f) * 1.5f, (DesiredHeight * .57f));
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
