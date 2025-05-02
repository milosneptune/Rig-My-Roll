using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamblerGame
{
    /// <summary>
    /// If the client wants to be notified when a button is clicked, it must
    /// implement a method matching this delegate and then tie that method to
    /// the button's "OnButtonClick" event.
    /// </summary>
    /// <param name="clickedButton">The delegate will be called with a reference to the clicked button</param>
    public delegate void OnButtonClickDelegate();

    
    public class Button
    {
        // Button specific fields
        protected SpriteFont font;
        protected GraphicsDevice device;
        protected MouseState prevMState;
        protected bool enabled = true;
        protected string text;
        protected Rectangle position; // Button position and size
        protected Vector2 textLoc;
        protected Vector2 textLocPressed;
        protected Vector2 textLocUnpressed;
        protected Texture2D buttonImg;
        protected Texture2D buttonUnpressedImg;
        protected Texture2D buttonPressedImg;
        private Color textColor;
        private Color buttonColor;
        private Color hoverColor;
        private Color normalColor;

        public virtual Rectangle Position
        {
            get { return position; }
            set { position = value; }  
        }

        /// <summary>
        /// If the client wants to be notified when a button is clicked, it must
        /// implement a method matching OnButtonClickDelegate and then tie that method to
        /// the button's "OnButtonClick" event.
        /// 
        /// The delegate will be called with a reference to the clicked button.
        /// </summary>
        public event OnButtonClickDelegate OnLeftButtonClick;

        /// <summary>
        /// Create a new custom button
        /// </summary>
        /// <param name="position">Where to draw the button's top left corner</param>
        /// <param name="text">The text to draw on the button</param>
        /// <param name="font">The font to use when drawing the button text.</param>
        /// <param name="color">The color to tint the button.</param>
        /// <param name="device">The game's graphic device</param>
        /// <param name="textures">The list of textures for if a button is pressed or not</param>
        public Button(GraphicsDevice device, Rectangle position, String text, SpriteFont font, Color color, List<Texture2D> textures)
        {
            // Save copies/references to the info we'll need later
            this.font = font;
            this.device = device;
            this.position = position;
            this.text = text;
            normalColor = color;
            buttonColor = normalColor;

            // Sets the textures of each state of the button
            buttonUnpressedImg = textures[0];
            buttonPressedImg = textures[1];
            buttonImg = buttonUnpressedImg;
        }

        /// <summary>
        /// Create a new custom checkbox button
        /// </summary>
        /// <param name="position">Where to draw the button's top left corner</param>
        /// <param name="text">The text to draw on the button</param>
        /// <param name="font">The font to use when drawing the button text.</param>
        /// <param name="color">The color to tint the button.</param>
        /// <param name="device">The game's graphic device</param>
        /// <param name="textures">The list of textures for if a button is checked or not</param>
        public Button(GraphicsDevice device, Rectangle position, String text, SpriteFont font, Color color, List<Texture2D> textures, bool status)
        {
            // Save copies/references to the info we'll need later
            this.font = font;
            this.device = device;
            this.position = position;
            this.text = text;
            normalColor = color;
            buttonColor = normalColor;

            // Sets the location of the text to the left of the button
            Vector2 textSize = font.MeasureString(text);
            textLocUnpressed = new Vector2(
                (position.X - textSize.X),
                (position.Y - textSize.Y / 2 + position.Height / 2)
            );

            textLoc = textLocUnpressed;

            // Sets the textures of each state of the button
            buttonUnpressedImg = textures[0];
            buttonPressedImg = textures[1];
            buttonImg = buttonUnpressedImg;
        }

        /// <summary>
        /// Each frame, update its status if it's been clicked.
        /// </summary>
        /// <param name="gameTime">Unused, but required to implement abstract class</param>
        public virtual void Update(GameTime gameTime)
        {
            // Check/capture the mouse state regardless of whether this button
            // if active so that it's up to date next time!
            MouseState mState = Mouse.GetState();

            ChangeTextLoc();
            buttonColor = normalColor;
            if (this.position.Contains(mState.Position))
            {
                // If it is pressed
                if (mState.LeftButton == ButtonState.Released &&
                prevMState.LeftButton == ButtonState.Pressed)
                {
                    if (OnLeftButtonClick != null)
                    {
                        OnLeftButtonClick();
                    }
                }

                // If it is only hovering over the button
                else
                {
                    Hover();
                }
            }

            if (mState.LeftButton  == ButtonState.Pressed &&
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

        /// <summary>
        /// Each frame, update its status if it's been clicked.
        /// </summary>
        /// <param name="gameTime">Unused, but required to implement abstract class</param>
        public virtual void CheckboxUpdate(GameTime gameTime, bool status)
        {
            // Check/capture the mouse state regardless of whether this button
            // if active so that it's up to date next time!
            MouseState mState = Mouse.GetState();

            buttonColor = normalColor;
            if (this.position.Contains(mState.Position))
            {
                // If it is pressed
                if (mState.LeftButton == ButtonState.Released &&
                prevMState.LeftButton == ButtonState.Pressed)
                {
                    if (OnLeftButtonClick != null)
                    {
                        OnLeftButtonClick();
                    }
                }

                // If it is only hovering over the button, change color
                else
                {
                    Hover();
                }
            }
            //Changes button image based on status
            if (status)
            {
                buttonImg = buttonPressedImg;
            }
            else
            {
                buttonImg = buttonUnpressedImg;
            }

            prevMState = mState;
        }

        /// <summary>
        /// Override the GameObject Draw() to draw the button and then
        /// overlay it with text.
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch on which to draw this button. The button 
        /// assumes that Begin() has already been called and End() will be called later.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Draw the button itself
            spriteBatch.Draw(buttonImg, position, buttonColor);

            // Draw button text over the button
            spriteBatch.DrawString(font, text, textLoc, Color.White);
        }

        /// <summary>
        /// Runs when the mouse is hovered over the button. Updates color
        /// </summary>
        public virtual void Hover()
        {
            hoverColor = new Color (normalColor.R + 15, normalColor.G + 15, normalColor.B + 15);
            buttonColor = hoverColor;
        }
        /// <summary>
        /// Figure out where on the button to draw the text based on its position.
        /// </summary>
        public void ChangeTextLoc()
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
    }
}
