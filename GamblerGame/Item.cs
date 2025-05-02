using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        const int DesiredWidth = 1920;
        const int DesiredHeight = 1080;
        private const int ButtonOffset = DesiredHeight / 80;
        private ScriptManager itemsFile = new ScriptManager("ItemsFile.txt");
        private string name;
        private string description;
        private int price;
        private char type;
        private string action;
        private bool hideDescription;
        private bool printDescription;
        private SpriteFont descriptionFont;
        private Vector2 mousePos;
        private Vector2 descriptionLoc;
        private Vector2 descriptionLocUnpressed;
        private Vector2 descriptionLocPressed;
        private Vector2 priceLoc;
        private Vector2 priceLocUnpressed;
        private Vector2 priceLocPressed;
        private Vector2 normalSize;
        private Vector2 purchasedSize;
        private Vector2 withDescriptionSize;
        private Button buyButton;
        private Button cancelButton;
        private Button useItemButton;
        //private Rectangle choiceBox;
        private bool displayUseItem;
        private bool displayBuyBox;
        public List<Item> PlayerInv
        {
            get;
            set;
        }
        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public int Price { get { return price; } }
        public char Type { get { return type; } }
        public string Action { get { return action; } }
        public Texture2D ItemTexture { get; private set; }

        /// <summary>
        /// Returns bool of if the item is bought.
        /// </summary>
        public bool Bought
        {
            get { return hideDescription; }
            set { hideDescription = value; }
        }

        /// <summary>
        /// Returns the position of a button
        /// </summary>
        public override Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Returns the button
        /// </summary>
        public Button BuyButton
        {
            get { return buyButton; }
            set { buyButton = value; }
        }
        public event UseItemDelegate UseItem;

        public Item(int itemIndex, GraphicsDevice device, SpriteFont font, SpriteFont descriptionFont, List<Texture2D> textures, ContentManager ct)
            : base(device, new Rectangle(0, 0, 0, 0), "name_here", font, Color.White, textures) // TODO: Change color
        {
            // Draws item stats from item file
            name = itemsFile.GetName(itemIndex);
            description = itemsFile.GetDescription(name);
            price = itemsFile.GetPrice(name);
            type = itemsFile.FindItemType(name);
            action = itemsFile.GetItemAction(name);

            hideDescription = false;
            printDescription = false;
            this.descriptionFont = descriptionFont;

            // Loads texture from item file
            LoadTextures(ct);
            mousePos = new Vector2();
            buyButton = new Button(device, new Rectangle(0, 0, 0, 0), "Buy", font, Color.Black, textures);
            cancelButton = new Button(device, new Rectangle(0, 0, 0, 0), "Cancel", font, Color.Black, textures);// TODO: Change color
            useItemButton = new Button(device, new Rectangle(0, 0, 0, 0), "Use Item", font, Color.Black, textures);// TODO: Change color
            //choiceBox = new Rectangle(0, 0, 0, 0);

            // Sets the sizes of the buttons
            normalSize = new Vector2((DesiredHeight * .31f), (DesiredHeight * .31f));
            purchasedSize = new Vector2((int)(DesiredHeight / 6.75), (int)(DesiredHeight / 6.75));
            withDescriptionSize = new Vector2((DesiredHeight * .31f), (DesiredHeight * .31f));

            // Changes display name
            text = name;

            displayUseItem = false;
            displayBuyBox = false;
            useItemButton.OnLeftButtonClick += UseItemTrigger;
            buyButton.OnLeftButtonClick += BuyItem;
            cancelButton.OnLeftButtonClick += CancelItem;
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
            mousePos.X = mState.Position.X;
            mousePos.Y = mState.Position.Y - (int)descriptionFont.MeasureString(description).Y;
            if (!Bought)
            {
                buyButton.Update(gameTime);
                cancelButton.Update(gameTime);
            }
            else
            {
                useItemButton.Update(gameTime);
            }
            if (this.position.Contains(mState.Position))
            {
                // If it is pressed
                if (mState.LeftButton == ButtonState.Released &&
                prevMState.LeftButton == ButtonState.Pressed)
                {
                    // If it isn't store, then the choice of using the item will be shown.
                    if (UseItem != null && hideDescription)
                    {
                        DisplayUseItem();
                    }
                    else
                    {
                        BuyItemChoice();
                    }
                }

                // If it is only hovering over the button and there is no description.
                Hover();
            }
            else
            {
                printDescription = false;
            }

            if (mState.LeftButton == ButtonState.Pressed &&
                this.position.Contains(mState.Position))
            {
                buttonImg = buttonUnpressedImg;
                textLoc = textLocUnpressed;
                descriptionLoc = descriptionLocUnpressed;
                priceLoc = priceLocUnpressed;
            }
            else
            {
                buttonImg = buttonUnpressedImg;
                textLoc = textLocUnpressed;
                descriptionLoc = descriptionLocUnpressed;
                priceLoc = priceLocUnpressed;
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
            if (!Bought)
            {
                // Draw the button itself
                spriteBatch.Draw(buttonImg, position, new Color(15, 15, 15));
                // Draws the button's item texture
                spriteBatch.Draw(ItemTexture, position, Color.White);
                // Draw button text over the button
                spriteBatch.DrawString(font, text, textLoc, Color.White);
            }
            
            if (!hideDescription)
            {
                // Draws the price
                spriteBatch.DrawString(descriptionFont, "Price: " + price, priceLoc, Color.White);
            }
            // Draws the use item button if its supposed to be
            if (displayUseItem)
            {
                useItemButton.Draw(spriteBatch);
            }
            // Draws the buy and cancel buttons if they are supposed to be
            if (displayBuyBox && !Bought)
            {
                ShapeBatch.Begin(device);
                //ShapeBatch.Box(choiceBox, Color.Pink);
                ShapeBatch.End();
                buyButton.Draw(spriteBatch);
                cancelButton.Draw(spriteBatch);
            }
            // Prints the description on the mouse
            if (printDescription)
            {
                spriteBatch.End();
                ShapeBatch.Begin(device);
                ShapeBatch.Box(new Rectangle((int)mousePos.X, (int)mousePos.Y, (int)descriptionFont.MeasureString(description).X, (int)descriptionFont.MeasureString(description).Y), new Color(1, 15, 15, 150));
                ShapeBatch.BoxOutline(new Rectangle((int)mousePos.X, (int)mousePos.Y - 1, (int)descriptionFont.MeasureString(description).X + 1, (int)descriptionFont.MeasureString(description).Y + 2), Color.White);
                ShapeBatch.End();
                spriteBatch.Begin();
                spriteBatch.DrawString(descriptionFont, description, mousePos, Color.White);
                spriteBatch.DrawString(descriptionFont, description, mousePos, Color.White);
            }
        }
        /// <summary>
        /// When the mouse is hovering over the item, the description is shown.
        /// </summary>
        public override void Hover()
        {
            // Changes color
            base.Hover();
            // Prints the description
            printDescription = true;
        }

        /// <summary>
        /// Finds the locations.
        /// </summary>
        public void SetLocations(int xLoc, int yLoc)
        {
            position.X = xLoc;
            position.Y = yLoc;

            // If there is only the name
            if (hideDescription)
            {
                Vector2 textSize = font.MeasureString(text);
                Vector2 buttonTextSize = descriptionFont.MeasureString("Use Item");

                position.Width = (int)normalSize.X;
                position.Height = (int)normalSize.Y;
                if (Bought)
                {
                    position.Width = (int)purchasedSize.X;
                    position.Height = (int)purchasedSize.Y;
                }

                textLocPressed = new Vector2(
                    (position.X + position.Width / 2) - textSize.X / 2,
                    (position.Y + position.Height / 2) - textSize.Y / 4
                );
                textLocUnpressed = new Vector2(
                    (position.X + position.Width / 2) - textSize.X / 2,
                    (position.Y + position.Height / 2) - textSize.Y / 2
                );

                // Finds the postion of the use item button. It is the same width as the item.
                // The button is directly under the item.
                useItemButton.Position = new Rectangle(
                    (int)position.X,
                    (int)((position.Y + position.Height)),
                    position.Width,
                    (int)(buttonTextSize.Y * 2)
                );
                textLoc = textLocUnpressed;
            }
            else
            {
                Vector2 nameSize = font.MeasureString(text);
                Vector2 descriptionSize = descriptionFont.MeasureString(description);
                Vector2 cancelSize = descriptionFont.MeasureString("Cancel");
                Vector2 priceSize = descriptionFont.MeasureString("Price: " + price);

                position.Width = (int)withDescriptionSize.X;
                position.Height = (int)withDescriptionSize.Y;

                textLocPressed = new Vector2(
                (position.X + position.Width / 2) - nameSize.X / 2,
                (position.Y + position.Height / 2) - (nameSize.Y + descriptionSize.Y) / 4
                );

                textLocUnpressed = new Vector2(
                    (position.X + position.Width / 2) - nameSize.X / 2,
                    (position.Y) + (nameSize.Y)
                );

                descriptionLocPressed = new Vector2(
                    (position.X + position.Width / 2) - descriptionSize.X / 2,
                    (position.Y + position.Height / 2) - descriptionSize.Y / 4 + ButtonOffset
                );
                descriptionLocUnpressed = new Vector2(
                    (position.X + position.Width / 2) - descriptionSize.X / 2,
                    (position.Y + position.Height) - descriptionSize.Y - ButtonOffset
                );

                priceLocPressed = new Vector2(
                    (position.X + position.Width / 2) - priceSize.X / 2,
                    (descriptionLocPressed.Y) + (int)(2.5 * ButtonOffset)
                );
                priceLocUnpressed = new Vector2(
                    (position.X + position.Width / 2) - priceSize.X / 2,
                    (descriptionLocUnpressed.Y) - (int)(2.5 * ButtonOffset)
                );

                /*choiceBox = new Rectangle(
                    (int)position.X,
                    (int)((position.Y + position.Height)),
                    position.Width,
                    (int)(descriptionSize.Y * 2)
                    );
                */

                // Sets buy button location
                buyButton.Position = new Rectangle(
                    (int)position.X + ButtonOffset,
                    (int)(position.Y + position.Height + ButtonOffset / 2),
                    (position.Width / 2) - 2 * ButtonOffset,
                    (int)(descriptionSize.Y * 2)
                );

                // Sets cancel button location
                cancelButton.Position = new Rectangle(
                    (int)(position.X + position.Width / 2 + ButtonOffset),
                    (int)(position.Y + position.Height + ButtonOffset / 2),
                    (position.Width / 2) - 2 * ButtonOffset,
                    (int)(descriptionSize.Y * 2)
                );
                textLoc = textLocUnpressed;
                descriptionLoc = descriptionLocUnpressed;
                priceLoc = priceLocUnpressed;
            }
        }

        /// <summary>
        /// Displays the choice of buy or cancel.
        /// </summary>
        public void BuyItemChoice()
        {
            if (displayBuyBox)
            {
                displayBuyBox = false;
            }
            else
            {
                displayBuyBox = true;
            }
        }

        /// <summary>
        /// If the use clicks the item, they can choose to use the button. If they click the item again, the button disappears.
        /// Same goes for the buy box.
        /// </summary>
        public void DisplayUseItem()
        {
            if (displayUseItem)
            {
                displayUseItem = false;
            }
            else
            {
                displayUseItem = true;
            }
        }

        /// <summary>
        /// Uses the Use Item button to trigger the use of the item.
        /// </summary>
        public void UseItemTrigger()
        {
            UseItem(action);
            this.Bought = false;
            this.displayUseItem = false;
            this.displayBuyBox = false;
            PlayerInv.Remove(this);

        }

        /// <summary>
        /// The item gets bought
        /// </summary>
        public void BuyItem()
        {
            Bought = true;
        }

        /// <summary>
        /// Cancel the buy box.
        /// </summary>
        public void CancelItem()
        {
            displayBuyBox = false;
        }

        /// <summary>
        /// Loads a texture for the item based on the type of the item
        /// </summary>
        /// <param name="ct"></param>
        private void LoadTextures(ContentManager ct)
        {
            switch (type)
            {
                case '#':
                    ItemTexture = ct.Load<Texture2D>("UI/Items/RollSpecific");
                    break;
                case '^':
                    ItemTexture = ct.Load<Texture2D>("UI/Items/ChanceSymbol");
                    break;
                case '&':
                    ItemTexture = ct.Load<Texture2D>("UI/Items/Freeze");
                    break;
                case '*':
                    ItemTexture = ct.Load<Texture2D>("UI/Items/IncreaseMult");
                    break;
                case '$':
                    ItemTexture = ct.Load<Texture2D>("UI/Items/IncreasePoints");
                    break;
            }
        }
    }
}
