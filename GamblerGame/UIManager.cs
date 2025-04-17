using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblerGame
{
    internal class UIManager
    {
        private GraphicsDevice graphicsDevice;

        const int DesiredWidth = 1920;
        const int DesiredHeight = 1080;

        // Background Values
        const int xAxisTiles = 16; // Number of background texture grids along the x axis
        const int yAxisTiles = xAxisTiles / 2 + 1; // Number of background texture grids along the y axis
        const int backgroundTileSize = DesiredWidth / xAxisTiles; // Size of the background tile texture (height and width because it is a square)

        // Textures and Fonts
        private Texture2D scanlineTexture;
        private Texture2D backgroundTexture;
        private SpriteFont pixelFont;
        private SpriteFont scoreFont;
        private SpriteFont titleFont;

        // Menu
        const int menuButtonWidth = DesiredWidth / 4;
        const int menuButtonHeight = DesiredHeight / 8;
        const int menuButtonYPos = (int)(DesiredHeight / 1.55);
        const int playButtonXPos = DesiredWidth / 2 - (int)(menuButtonWidth * 1.25);
        const int quitButtonXPos = DesiredWidth / 2 + (int)(menuButtonWidth * .25);

        private int titleWidth = (int)(DesiredWidth / 3);
        private int titleHeight = (int)(DesiredHeight / 2.25);
        private int titleXPos = DesiredWidth / 2 - (int)(DesiredWidth / (3 * 2));
        private int titleYPos = (int)(DesiredHeight / 10) + DesiredHeight / 2 - (int)(DesiredHeight / 2.25);
        private int blackBarHeight = (int)(DesiredHeight / 2);


        // Game
        const int pauseButtonWidth = (int)(DesiredWidth / 3.9);
        const int pauseButtonHeight = DesiredHeight / 10;
        const int rollButtonYPos = (int)(DesiredHeight * .874);
        const int pauseButtonYPos = (int)(DesiredHeight * .739);
        const int rollButtonXPos = (int)(DesiredWidth / 5.9);
        const int pauseButtonXPos = (int)(DesiredWidth * .675);



        public UIManager(GraphicsDevice graphicsDevice, List<SpriteFont> fonts, List<Texture2D> textures)
        {
            this.graphicsDevice = graphicsDevice;
            pixelFont = fonts[0];
            titleFont = fonts[1];
            scoreFont = fonts[2];
            backgroundTexture = textures[0];
            scanlineTexture = textures[1];
        }
        public void DrawMenu(SpriteBatch sb)
        {
            ShapeBatch.Begin(graphicsDevice);
            // Title Box
            ShapeBatch.Box(
                titleXPos,
                titleYPos,
                titleWidth,
                titleHeight,
                Color.Black);
            // Title Outline
            ShapeBatch.BoxOutline(
                titleXPos,
                titleYPos,
                titleWidth + 1,
                titleHeight,
                Color.White);
            /*
            ShapeBatch.BoxOutline(
                playButtonXPos,
                menuButtonYPos,
                menuButtonWidth,
                menuButtonHeight,
                Color.White);
            // Quit Button Outline
            ShapeBatch.BoxOutline(
                quitButtonXPos,
                menuButtonYPos,
                menuButtonWidth,
                menuButtonHeight,
                Color.White);
            */
            sb.End();
            ShapeBatch.End();

            // Title Text
            sb.Begin();
            sb.DrawString(titleFont, "Rig", new Vector2(DesiredWidth / 2 - titleFont.MeasureString("Rig").X / 2, titleYPos + titleFont.MeasureString("Rig").Y / 4), Color.White);
            sb.DrawString(titleFont, "my", new Vector2(DesiredWidth / 2 - titleFont.MeasureString("my").X / 2, titleYPos + titleFont.MeasureString("Rig").Y * 1.25f), Color.White);
            sb.DrawString(titleFont, "Roll", new Vector2(DesiredWidth / 2 - titleFont.MeasureString("Roll").X / 2, titleYPos + titleFont.MeasureString("Rig").Y * 2.25f), Color.White);
            sb.End();
        }

        public void DrawGame(SpriteBatch sb)
        {

            ShapeBatch.Begin(graphicsDevice);
            // HUD Background
            ShapeBatch.Box(
                (int)(DesiredWidth * .66),
                0,
                (int)(DesiredWidth / 3.5),
                DesiredHeight,
                new Color(12, 7, 15));
            // HUD Background Outline
            ShapeBatch.BoxOutline(
                (int)(DesiredWidth * .66),
                0,
                (int)(DesiredWidth / 3.5) + 1,
                DesiredHeight,
                Color.White);
            // Score requirement box
            ShapeBatch.Box((int)(DesiredWidth * .675), (int)(DesiredHeight * .03), (int)(DesiredWidth / 3.9), DesiredHeight / 4, Color.DarkGray);
            // Round score container box
            ShapeBatch.Box((int)(DesiredWidth * .675), (int)(DesiredHeight * .31), (int)(DesiredWidth / 3.9), DesiredHeight / 8, Color.DarkGray);
            // Round score display box
            ShapeBatch.Box((int)(DesiredWidth * .755), (int)(DesiredHeight * .323), (int)(DesiredWidth / 5.9), DesiredHeight / 10, Color.Black);
            // Current score box
            ShapeBatch.Box((int)(DesiredWidth * .675), (int)(DesiredHeight * .465), (int)(DesiredWidth / 3.9), DesiredHeight / 4, Color.DarkGray);

            sb.End();
            ShapeBatch.End();
            sb.Begin();
            // Round score text
            sb.DrawString(scoreFont, "Round", new Vector2((int)(DesiredWidth * .715) - scoreFont.MeasureString("Round").X / 2, (int)(DesiredHeight * .345)), Color.White);
            sb.DrawString(scoreFont, "Score", new Vector2((int)(DesiredWidth * .715) - scoreFont.MeasureString("Score").X / 2, (int)(DesiredHeight * .375)), Color.White);
        }
        /// <summary>
        /// Draws the background as a grid with an extra row off screen, 
        /// the grid will move down until it reaches the designated height
        /// of the texture and then teleport back to is starting position (giving the illusion of constanly moving down)
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="backgroundPosition"></param>
        public void DrawBackground(SpriteBatch sb, int backgroundPosition, Color color)
        {
            for (int i = 0; i < xAxisTiles; i++) //columns
            {
                for (int j = -1; j < yAxisTiles; j++) //rows
                {
                    sb.Draw(backgroundTexture,                  // Texture
                        new Rectangle(i * backgroundTileSize,             // X position
                        (j * backgroundTileSize) + backgroundPosition,    // Y Position
                        backgroundTileSize, backgroundTileSize),          // Width and Height
                        color);                                     // Color (plan on updating color when the state changes
                }
            }
        }

        /// <summary>
        /// Draws each screen filter
        /// As of now it's just a transparent scanline filter
        /// </summary>
        /// <param name="sb"></param>
        public void DrawScreenFilters(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(scanlineTexture, new Rectangle(0, 0, DesiredWidth, DesiredHeight), new Color(25, 25, 25, 1));
        }

        public void DrawBlackBars(GraphicsDevice graphicsDevice, int blackBarYPosition)
        {
            ShapeBatch.Begin(graphicsDevice);
            // Black Bar
            ShapeBatch.Box(
                0,
                0 - blackBarYPosition,
                DesiredWidth,
                blackBarHeight,
                Color.Black);
            ShapeBatch.Box(
                0,
                DesiredHeight / 2 + blackBarYPosition,
                DesiredWidth,
                blackBarHeight,
                Color.Black);
            ShapeBatch.End();
        }

    }
}
