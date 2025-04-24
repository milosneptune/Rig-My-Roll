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
        private bool showDescription;
        private SpriteFont descriptionFont;
        private Vector2 descriptionLoc;
        private Vector2 descriptionLocUnpressed;
        private Vector2 descriptionLocPressed;

        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public int Price { get { return price; } }
        public char Type { get { return type; } }
        public string Action { get { return action; } }
        public bool ShowDescription
        {
            get { return showDescription; }
            set { showDescription = value; }
        }
        public Rectangle Position
        {
            get { return position; }
            set
            {
                position = value;

                // Figure out where on the button to draw the text after setting the position.
                // Runs every time the location changes.
                SetTextLocations();
            }
        }
        public event UseItemDelegate UseItem;

        public Item(int itemIndex, GraphicsDevice device, SpriteFont font, SpriteFont descriptionFont, List<Texture2D> textures)
            : base(device, new Rectangle(0, 0, 0, 0), "name_here", font, Color.Black, textures)
        {
            name = itemsFile.GetName(itemIndex);
            description = itemsFile.GetDescription(name);
            price = itemsFile.GetPrice(name);
            type = itemsFile.FindItemType(name);
            action = itemsFile.GetItemAction(name);
            showDescription = false;
            this.descriptionFont = descriptionFont;

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
            
            if (this.position.Contains(mState.Position))
            {
                // If it is pressed
                if (mState.LeftButton == ButtonState.Released &&
                prevMState.LeftButton == ButtonState.Pressed)
                {
                    // If it isn't store, then the item will be used.
                    if (UseItem != null && !showDescription)
                    {
                        UseItem(action);
                    }
                    else
                    {
                        BuyItemChoice();
                    }
                }

                // If it is only hovering over the button and there is no description.
                else if (!showDescription)
                {
                    Hover();
                }
            }

            if (mState.LeftButton == ButtonState.Pressed &&
                this.position.Contains(mState.Position))
            {
                buttonImg = buttonPressedImg;
                textLoc = textLocPressed;
                descriptionLoc = descriptionLocPressed;
            }
            else
            {
                buttonImg = buttonUnpressedImg;
                textLoc = textLocUnpressed;
                descriptionLoc = descriptionLocUnpressed;
            }

            prevMState = mState;
        }
        /// <summary>
        /// Override the Button Draw() to draw the item and then
        /// overlay it with text with or without the description.
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch on which to draw this button. The button 
        /// assumes that Begin() has already been called and End() will be called later.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the button itself
            spriteBatch.Draw(buttonImg, position, new Color(15, 15, 15));

            // Draw button text over the button
            spriteBatch.DrawString(font, text, textLoc, Color.White);

            if (showDescription)
            {
                // Draw description below the name
                spriteBatch.DrawString(descriptionFont, description, descriptionLoc, Color.White);
            }
        }
        /// <summary>
        /// When the mouse is hovering over the item, the description is shown.
        /// </summary>
        public override void Hover()
        {
            // Changes color
            base.Hover();

            // TODO: Print description.
        }

        /// <summary>
        /// Finds the text locations.
        /// </summary>
        public void SetTextLocations()
        {
            // If there is only the name
            if (!showDescription)
            {
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
            else
            {
                Vector2 nameSize = font.MeasureString(text);
                Vector2 descriptionSize = descriptionFont.MeasureString(description);

                textLocPressed = new Vector2(
                (position.X + position.Width / 2) - nameSize.X / 2,
                (position.Y + position.Height / 2) - (nameSize.Y + descriptionSize.Y) / 4
                );
                textLocUnpressed = new Vector2(
                    (position.X + position.Width / 2) - nameSize.X / 2,
                    (position.Y + position.Height / 2) - (nameSize.Y + descriptionSize.Y) / 2
                );

                descriptionLocPressed = new Vector2(
                    (position.X + position.Width / 2) - descriptionSize.X / 2,
                    (position.Y + position.Height / 2) - descriptionSize.Y / 4
                );
                descriptionLocUnpressed = new Vector2(
                    (position.X + position.Width / 2) - descriptionSize.X / 2,
                    (position.Y + position.Height / 2) - descriptionSize.Y / 2
                );
                textLoc = textLocUnpressed;
                descriptionLoc = descriptionLocUnpressed;
            }
        }

        /// <summary>
        /// Displays the choice of buy or cancel.
        /// </summary>
        public void BuyItemChoice()
        {
            // TODO: Add choice of buying or cancelling. There should be one button to buy, and once that button is pressed,
            // the delegate allows it to return the purchased item.
        }
    }
}
