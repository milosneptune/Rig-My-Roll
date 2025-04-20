using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamblerGame
{
    /// <summary>
    /// A delegate for when the item is selected.
    /// </summary>
    /// <param name="action"></param>
    public delegate void UseItemDelegate(string action);
    internal class Item : Button
    {
        private ScriptManager itemsFile = new ScriptManager("ItemsFile.txt");
        private string name;
        private string description;
        private int price;
        private char type;
        private string action;

        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public int Price { get { return price; } }
        public char Type { get { return type; } }
        public string Action { get { return action; } }
        public Rectangle Position
        {
            get { return position; }
            set
            {
                position = value;

                // Figure out where on the button to draw the text after setting the position.
                // Runs every time the location changes.
                Vector2 textSize = font.MeasureString(text);
                textLocPressed = new Vector2(
                    (position.X + position.Width / 2) - textSize.X / 2,
                    (position.Y + position.Height / 2) - textSize.Y / 4
                );
                textLocUnpressed = new Vector2(
                    (position.X + position.Width / 2) - textSize.X / 2,
                    (position.Y + position.Height / 2) - textSize.Y / 2
                );
                textLoc = textLocUnpressed;
            }
        }
        public event UseItemDelegate UseItem;

        public Item(int itemIndex, GraphicsDevice device, SpriteFont font, List<Texture2D> textures)
            : base(device, new Rectangle(0, 0, 0, 0), "name_here", font, Color.Black, textures)
        {
            name = itemsFile.GetName(itemIndex);
            description = itemsFile.GetDescription(name);
            price = itemsFile.GetPrice(name);
            type = itemsFile.FindItemType(name);
            action = itemsFile.GetItemAction(name);

            // Changes display name
            text = name;
        }
        /// <summary>
        /// Each frame, update its status if it's been clicked.
        /// </summary>
        /// <param name="gameTime">Unused, but required to implement abstract class</param>
        public override void Update(GameTime gameTime)
        {
            // Check/capture the mouse state regardless of whether this button
            // if active so that it's up to date next time!
            MouseState mState = Mouse.GetState();
            if (mState.LeftButton == ButtonState.Released &&
                prevMState.LeftButton == ButtonState.Pressed &&
                this.position.Contains(mState.Position))
            {
                if (UseItem != null)
                {
                    UseItem(action);
                }
            }

            if (mState.LeftButton == ButtonState.Pressed &&
                this.position.Contains(mState.Position))
            {
                buttonImg = buttonPressedImg;
                textLoc = textLocPressed;
            }
            else
            {
                buttonImg = buttonUnpressedImg;
                textLoc = textLocUnpressed;
            }

            prevMState = mState;
        }
    }
}
