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

        const int ItemWidth = 160;
        const int ItemHeight = 160;
        const int InventoryMax = 5;

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
                titleYPos-1,
                titleWidth + 1,
                titleHeight + 1,
                Color.White);
            sb.End();
            ShapeBatch.End();

            // Title Text
            sb.Begin();
            sb.DrawString(titleFont, "Rig", new Vector2(DesiredWidth / 2 - titleFont.MeasureString("Rig").X / 2, titleYPos + titleFont.MeasureString("Rig").Y / 4), Color.White);
            sb.DrawString(titleFont, "my", new Vector2(DesiredWidth / 2 - titleFont.MeasureString("my").X / 2, titleYPos + titleFont.MeasureString("Rig").Y * 1.25f), Color.White);
            sb.DrawString(titleFont, "Roll", new Vector2(DesiredWidth / 2 - titleFont.MeasureString("Roll").X / 2, titleYPos + titleFont.MeasureString("Rig").Y * 2.25f), Color.White);
            sb.End();
        }
        public void DrawPaused(SpriteBatch sb, List<Symbol> symbols)
        {
            ShapeBatch.Begin(graphicsDevice);
            ShapeBatch.Box(0, 0, DesiredWidth, DesiredHeight, new Color (0, 0, 0, 200));
            // Scores Background
            ShapeBatch.Box(
                (int)(DesiredWidth * .66)+1,
                DesiredHeight/10+1,
                (int)(DesiredWidth / 3.5),
                DesiredHeight - DesiredHeight / 5-3,
                Color.Black);
            // Scores Background Outline
            ShapeBatch.BoxOutline(
                (int)(DesiredWidth * .66)+1,
                DesiredHeight / 10+1,
                (int)(DesiredWidth / 3.5) + 1,
                DesiredHeight - DesiredHeight / 5-2,
                new Color(255,255,255));
            ShapeBatch.End();
            sb.Begin();
            sb.DrawString(pixelFont, "Cherry - Scores 10 Points", new Vector2((int)(DesiredWidth * .66) + DesiredWidth / 80 + DesiredHeight / 15, DesiredHeight - DesiredHeight / 5 - (0 * (DesiredHeight / 15)) + pixelFont.MeasureString("Cherry - Scores 10 Points").Y / 2), Color.White);
            sb.DrawString(pixelFont, "Seven - Scores 100 Points", new Vector2((int)(DesiredWidth * .66) + DesiredWidth / 80 + DesiredHeight / 15, DesiredHeight - DesiredHeight / 5 - (1 * (DesiredHeight / 15)) + pixelFont.MeasureString("Seven - Scores 100 Points").Y / 2), Color.White);
            sb.DrawString(pixelFont, "Lemon - Scores 35 Points", new Vector2((int)(DesiredWidth * .66) + DesiredWidth / 80 + DesiredHeight / 15, DesiredHeight - DesiredHeight / 5 - (2 * (DesiredHeight / 15)) + pixelFont.MeasureString("Lemon - Scores 35 Points").Y / 2), Color.White);
            sb.DrawString(pixelFont, "Lime - Scores 55 Points", new Vector2((int)(DesiredWidth * .66) + DesiredWidth / 80 + DesiredHeight / 15, DesiredHeight - DesiredHeight / 5 - (3 * (DesiredHeight / 15)) + pixelFont.MeasureString("Lime - Scores 55 Points").Y / 2), Color.White);
            sb.DrawString(pixelFont, "P.apple - Scores 60 Points", new Vector2((int)(DesiredWidth * .66) + DesiredWidth / 80 + DesiredHeight / 15, DesiredHeight - DesiredHeight / 5 - (4 * (DesiredHeight / 15)) + pixelFont.MeasureString("P.apple - Scores 60 Points").Y / 2), Color.White);
            sb.DrawString(pixelFont, "Melon - Scores 25 Points", new Vector2((int)(DesiredWidth * .66) + DesiredWidth / 80 + DesiredHeight / 15, DesiredHeight - DesiredHeight / 5 - (5 * (DesiredHeight / 15)) + pixelFont.MeasureString("Melon - Scores 25 Points").Y / 2), Color.White);
            sb.DrawString(pixelFont, "Orange - Scores 65 Points", new Vector2((int)(DesiredWidth * .66) + DesiredWidth / 80 + DesiredHeight / 15, DesiredHeight - DesiredHeight / 5 - (6 * (DesiredHeight / 15)) + pixelFont.MeasureString("Orange - Scores 65 Points").Y / 2), Color.White);
            sb.DrawString(pixelFont, "Kiwi - Scores 50 Points", new Vector2((int)(DesiredWidth * .66) + DesiredWidth / 80 + DesiredHeight / 15, DesiredHeight - DesiredHeight / 5 - (7 * (DesiredHeight / 15)) + pixelFont.MeasureString("Kiwi - Scores 50 Points").Y / 2), Color.White);
            sb.DrawString(pixelFont, "Apple - Scores 15 Points", new Vector2((int)(DesiredWidth * .66) + DesiredWidth / 80 + DesiredHeight / 15, DesiredHeight - DesiredHeight / 5 - (8 * (DesiredHeight / 15)) + pixelFont.MeasureString("Apple - Scores 15 Points").Y / 2), Color.White);
            sb.DrawString(pixelFont, "S.berry - Scores 20 Points", new Vector2((int)(DesiredWidth * .66) + DesiredWidth / 80 + DesiredHeight / 15, DesiredHeight - DesiredHeight / 5 - (9 * (DesiredHeight / 15)) + pixelFont.MeasureString("S.berry - Scores 20 Points").Y / 2), Color.White);
            sb.DrawString(pixelFont, "Banana - Scores 30 Points", new Vector2((int)(DesiredWidth * .66) + DesiredWidth / 80 + DesiredHeight / 15, DesiredHeight - DesiredHeight / 5 - (10 * (DesiredHeight / 15)) + pixelFont.MeasureString("Banana - Scores 30 Points").Y / 2), Color.White);
            
            for (int i = 0; i < symbols.Count; i++)
            {
                symbols[i].DrawSymbol(sb, (int)(DesiredWidth * .66) + DesiredWidth / 80, DesiredHeight - DesiredHeight / 5 - (i * (DesiredHeight / 15)), DesiredHeight / 15, DesiredHeight / 15);

            }
            sb.End();
        }

        public void DrawRoundEnd(SpriteBatch sb)
        {
            ShapeBatch.Begin(graphicsDevice);
            ShapeBatch.Box(0, 0, DesiredWidth, DesiredHeight, new Color(0, 0, 0, 100));
            ShapeBatch.Box(DesiredWidth / 40, DesiredHeight / 2 - (DesiredWidth / 10), DesiredWidth / 1.65f, DesiredHeight - (DesiredHeight / 2 - (DesiredWidth / 10)), new Color(20, 20, 20));
            ShapeBatch.Box(DesiredWidth / 40 + DesiredWidth/80, DesiredHeight / 2 - (DesiredWidth / 10) + DesiredHeight / 60, DesiredWidth / 1.65f - DesiredWidth / 40,  (DesiredHeight / 10), new Color(0, 0, 0));
            ShapeBatch.Box(DesiredWidth / 40 + DesiredWidth / 80, DesiredHeight / 2 - (DesiredWidth / 10) + (DesiredHeight / 60)*2 + (DesiredHeight / 10), DesiredWidth / 1.65f - DesiredWidth / 40, (DesiredHeight / 14), new Color(0, 0, 0));
            ShapeBatch.Box(DesiredWidth / 40 + DesiredWidth / 80, DesiredHeight / 2 - (DesiredWidth / 10) + (DesiredHeight / 60) * 3 + (DesiredHeight / 10) + (DesiredHeight / 14), DesiredWidth / 1.65f - DesiredWidth / 40, (DesiredHeight / 14), new Color(0, 0, 0));
            ShapeBatch.Box(DesiredWidth / 40 + DesiredWidth / 80, DesiredHeight / 2 - (DesiredWidth / 10) + (DesiredHeight / 60) * 4 + (DesiredHeight / 10) + (DesiredHeight / 14) * 2, DesiredWidth / 1.65f - DesiredWidth / 40, (DesiredHeight / 14), new Color(0, 0, 0));
            ShapeBatch.End();
            ShapeBatch.Begin(graphicsDevice);
            ShapeBatch.BoxOutline(DesiredWidth / 40 -1, DesiredHeight / 2 - (DesiredWidth / 10) - 1, DesiredWidth / 1.65f + 1, DesiredHeight - (DesiredHeight / 2 - (DesiredWidth / 10)) + 1, Color.White);
            ShapeBatch.BoxOutline(DesiredWidth / 40 + DesiredWidth / 80 - 1, DesiredHeight / 2 - (DesiredWidth / 10) + DesiredHeight / 60+ 1, DesiredWidth / 1.65f - DesiredWidth / 40, (DesiredHeight / 10) + 1, new Color(100, 100, 100));
            ShapeBatch.BoxOutline(DesiredWidth / 40 + DesiredWidth / 80 - 1, DesiredHeight / 2 - (DesiredWidth / 10) + (DesiredHeight / 60) * 2 + (DesiredHeight / 10) + 1, DesiredWidth / 1.65f - DesiredWidth / 40 + 1, (DesiredHeight / 14) + 1, new Color(100, 100, 100));
            ShapeBatch.BoxOutline(DesiredWidth / 40 + DesiredWidth / 80 - 1, DesiredHeight / 2 - (DesiredWidth / 10) + (DesiredHeight / 60) * 3 + (DesiredHeight / 10) + (DesiredHeight / 14) + 1, DesiredWidth / 1.65f - DesiredWidth / 40 + 1, (DesiredHeight / 14) + 1, new Color(100, 100, 100));
            ShapeBatch.BoxOutline(DesiredWidth / 40 + DesiredWidth / 80 - 1, DesiredHeight / 2 - (DesiredWidth / 10) + (DesiredHeight / 60) * 4 + (DesiredHeight / 10) + (DesiredHeight / 14) * 2 + 1, DesiredWidth / 1.65f - DesiredWidth / 40 + 1, (DesiredHeight / 14) + 1, new Color(100, 100, 100));
            ShapeBatch.End();
        }
        public void DrawGameSlot(SpriteBatch sb)
        {

            ShapeBatch.Begin(graphicsDevice);
            
            // Slot box
            ShapeBatch.Box(DesiredWidth / 20 - DesiredWidth/60, DesiredHeight / 2 - (DesiredWidth / 9), DesiredWidth / 20 + ((DesiredWidth / 6) * 3.2f), DesiredWidth / 4.5f, new Color(12, 7, 15));

            //ShapeBatch.Box(DesiredWidth / 20, DesiredHeight / 2 - (DesiredWidth / 10)-1, DesiredWidth / 20 + ((DesiredWidth / 6) * 3)+1, DesiredWidth / 5+1, Color.Black);
            ShapeBatch.Box(DesiredWidth / 20 + DesiredWidth / 80, DesiredHeight / 2 - (DesiredWidth / 11.5f) , DesiredWidth / 5.75f , DesiredWidth / 6, Color.DarkGray);
            ShapeBatch.Box(DesiredWidth / 20 + (DesiredWidth / 5.75f) + DesiredWidth/68, DesiredHeight / 2 - (DesiredWidth / 11.5f) , DesiredWidth / 5.75f, DesiredWidth / 6 , Color.DarkGray);
            ShapeBatch.Box(DesiredWidth / 20 + ((DesiredWidth / 5.75f) * 2) + DesiredWidth / 60, DesiredHeight / 2 - (DesiredWidth / 11.5f), DesiredWidth / 5.75f, DesiredWidth / 6, Color.DarkGray);
            ShapeBatch.End();
            ShapeBatch.Begin(graphicsDevice);
            ShapeBatch.BoxOutline(DesiredWidth / 20 + DesiredWidth / 80, DesiredHeight / 2 - (DesiredWidth / 11.5f), DesiredWidth / 5.75f, DesiredWidth / 6, Color.DarkGray);
            ShapeBatch.BoxOutline(DesiredWidth / 20 + (DesiredWidth / 5.75f) + DesiredWidth / 68, DesiredHeight / 2 - (DesiredWidth / 11.5f), DesiredWidth / 5.75f, DesiredWidth / 6, Color.DarkGray);
            ShapeBatch.BoxOutline(DesiredWidth / 20 + ((DesiredWidth / 5.75f) * 2) + DesiredWidth / 60, DesiredHeight / 2 - (DesiredWidth / 11.5f), DesiredWidth / 5.75f, DesiredWidth / 6, Color.DarkGray);
            ShapeBatch.End();
        }

        public void DrawGameBar(SpriteBatch sb)
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
            ShapeBatch.Box((int)(DesiredWidth * .675), (int)(DesiredHeight * .03), (int)(DesiredWidth / 3.9), DesiredHeight / 4, new Color(50, 50, 50));

            // Score requirement display box
            ShapeBatch.Box((int)(DesiredWidth * .785), (int)(DesiredHeight * .03) + DesiredHeight / 16, (DesiredWidth / 7.25f), DesiredHeight / 8, Color.Black);

            // Round score container box
            ShapeBatch.Box((int)(DesiredWidth * .675), (int)(DesiredHeight * .31), (int)(DesiredWidth / 3.9), DesiredHeight / 8, new Color(50, 50, 50));
            // Round score display box
            ShapeBatch.Box((int)(DesiredWidth * .755), (int)(DesiredHeight * .323), (int)(DesiredWidth / 5.9), DesiredHeight / 10, Color.Black);
            // Current score box
            ShapeBatch.Box((int)(DesiredWidth * .675), (int)(DesiredHeight * .465), (int)(DesiredWidth / 3.9), DesiredHeight / 4, new Color(50, 50, 50));

            // Item box
            //ShapeBatch.Box(DesiredWidth / 40, (DesiredHeight / 20), DesiredWidth / 1.65f, DesiredHeight / 8.25f, new Color(12, 7, 15, 200));
            int xpos = DesiredWidth / 20 - DesiredWidth / 60;
            for (int i = 0; i < InventoryMax; i++)
            {
                ShapeBatch.Box((DesiredWidth / 40) + xpos, (DesiredHeight / 20), ItemWidth, ItemHeight, new Color(12, 7, 15, 200));
                ShapeBatch.BoxOutline((DesiredWidth / 40) + xpos, (DesiredHeight / 20) - 1, ItemWidth + 1, ItemHeight + 1, Color.White);
                xpos += ItemWidth + 50;
            }
            sb.End();
            ShapeBatch.End();
            ShapeBatch.Begin(graphicsDevice);
            ShapeBatch.BoxOutline(DesiredWidth / 40, (DesiredHeight / 20) - 1, DesiredWidth / 1.65f + 1, (DesiredHeight / 8.25f) + 1, Color.White);
            ShapeBatch.BoxOutline((int)(DesiredWidth * .675), (int)(DesiredHeight * .03), (int)(DesiredWidth / 3.9), DesiredHeight / 4, new Color(150, 150, 150));

            // Score requirement display box outline
            ShapeBatch.BoxOutline((int)(DesiredWidth * .785), (int)(DesiredHeight * .03) + DesiredHeight / 16, (DesiredWidth / 7.25f), DesiredHeight / 8, new Color(150, 150, 150));

            // Round score container box outline
            ShapeBatch.BoxOutline((int)(DesiredWidth * .675), (int)(DesiredHeight * .31), (int)(DesiredWidth / 3.9), DesiredHeight / 8, new Color(150, 150, 150));
            // Round score display box outline
            ShapeBatch.BoxOutline((int)(DesiredWidth * .755), (int)(DesiredHeight * .323), (int)(DesiredWidth / 5.9), DesiredHeight / 10, new Color(150, 150, 150));
            // Current score box outline
            ShapeBatch.BoxOutline((int)(DesiredWidth * .675), (int)(DesiredHeight * .465), (int)(DesiredWidth / 3.9), DesiredHeight / 4, new Color(150, 150, 150));
            
            ShapeBatch.End();
            sb.Begin();
            // Round score text

            sb.DrawString(scoreFont, "Score", new Vector2((int)(DesiredWidth * .735) - scoreFont.MeasureString("Score").X / 2, ((DesiredHeight * .03f) + DesiredHeight / 8) - (scoreFont.MeasureString("Score").Y / 2)), Color.White);
            sb.DrawString(scoreFont, "Required", new Vector2((int)(DesiredWidth * .735) - scoreFont.MeasureString("Required").X / 2, ((DesiredHeight * .03f) + DesiredHeight / 8) + (scoreFont.MeasureString("Required").Y / 2)), Color.White);
            sb.DrawString(scoreFont, "Round", new Vector2((int)(DesiredWidth * .715) - scoreFont.MeasureString("Round").X / 2, (int)(DesiredHeight * .345)), Color.White);
            sb.DrawString(scoreFont, "Score", new Vector2((int)(DesiredWidth * .715) - scoreFont.MeasureString("Score").X / 2, (int)(DesiredHeight * .375)), Color.White);
            sb.End();
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
            sb.Draw(scanlineTexture, new Rectangle(0, 0, DesiredWidth, DesiredHeight), new Color(12, 12, 12, 1));
        }

        public void DrawBlackBars(int blackBarYPosition)
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
