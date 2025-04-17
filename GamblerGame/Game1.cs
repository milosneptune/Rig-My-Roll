using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamblerGame
{
    public enum State
    {
        MainMenu,
        Game,
        Pause,
        GameOver,
        Quit
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        const int DesiredWidth = 1920;
        const int DesiredHeight = 1080;

        // Background Values
        const int xAxisTiles = 16; // Number of background texture grids along the x axis
        const int yAxisTiles = xAxisTiles / 2 + 1; // Number of background texture grids along the y axis
        const int backgroundTileSize = DesiredWidth / xAxisTiles; // Size of the background tile texture (height and width because it is a square)
        private int backgroundPosition = 0; // The position of the y value (of the tiole that spawns in the top left)
        private Texture2D scanlineTexture;
        private Texture2D backgroundTexture;
        private SpriteFont pixelFont;
        private SpriteFont scoreFont;
        private SpriteFont titleFont;

        private State gameState;

        // Menu
        const int menuButtonWidth = DesiredWidth / 4;
        const int menuButtonHeight = DesiredHeight / 8;
        const int menuButtonYPos = (int)(DesiredHeight / 1.55);
        const int playButtonXPos = DesiredWidth / 2 - (int)(menuButtonWidth * 1.25);
        const int quitButtonXPos = DesiredWidth / 2 + (int)(menuButtonWidth * .25);
        private List<Button> menuButtons = new List<Button>();

        // Game
        const int pauseButtonWidth = (int)(DesiredWidth / 3.9);
        const int pauseButtonHeight = DesiredHeight / 10;
        const int rollButtonYPos = (int)(DesiredHeight * .874);
        const int pauseButtonYPos = (int)(DesiredHeight * .739);
        const int rollButtonXPos = (int)(DesiredWidth / 5.9);
        const int pauseButtonXPos = (int)(DesiredWidth * .675);
        private List<Button> gameButtons = new List<Button>();

        // Symbols
        private Texture2D sevenTexture;

        // Scoring
        private double roundScore = 0;
        private double rollScore;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = DesiredWidth;
            _graphics.PreferredBackBufferHeight = DesiredHeight;
            _graphics.ApplyChanges();
            gameState = State.Game;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("Background/Purple2");
            scanlineTexture = Content.Load<Texture2D>("Background/SCANLINE 1");
            sevenTexture = Content.Load<Texture2D>("UI/Symbols/seven");
            pixelFont = Content.Load<SpriteFont>("Fonts/monogram");
            titleFont = Content.Load<SpriteFont>("Fonts/Daydream");
            scoreFont = Content.Load<SpriteFont>("Fonts/Daydream2");

            // Play Game
            menuButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(playButtonXPos, menuButtonYPos, menuButtonWidth, menuButtonHeight),
                    "Play",
                    pixelFont,
                    Color.Black));
            menuButtons[0].OnLeftButtonClick += GameState;

            // Quit
            menuButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(quitButtonXPos, menuButtonYPos, menuButtonWidth, menuButtonHeight),
                    "Quit",
                    pixelFont,
                    Color.Black));
            menuButtons[1].OnLeftButtonClick += Exit;

            gameButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(pauseButtonXPos, rollButtonYPos, pauseButtonWidth, pauseButtonHeight),
                    "Roll",
                    pixelFont,
                    Color.Black));
            gameButtons[0].OnLeftButtonClick += Roll;

            // Quit
            gameButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(pauseButtonXPos, pauseButtonYPos, pauseButtonWidth, pauseButtonHeight),
                    "Pause",
                    pixelFont,
                    Color.Black));
            gameButtons[1].OnLeftButtonClick += Pause;
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch (gameState)
            {
                case State.MainMenu:
                    foreach (Button button in menuButtons)
                    {
                        button.Update(gameTime);
                    }
                    backgroundPosition += 2; // moves the position of every tile down each frame
                    break;
                case State.Game:
                    foreach (Button button in gameButtons)
                    {
                        button.Update(gameTime);
                    }
                    roundScore += 10;
                    backgroundPosition++; // moves the position of every tile down each frame
                    break;
                case State.Pause:
                    break;
                case State.GameOver:
                    break;
                case State.Quit:
                    break;
            }

            BackgroundScreenWrap(); // if the position has moved to the size of the tile, it resets its position (appearing to be constantly moving
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            // Prints the background as a grid with an extra row off screen
            for (int i = 0; i < xAxisTiles; i++) //columns
            {
                for (int j = -1; j < yAxisTiles; j++) //rows
                {
                    _spriteBatch.Draw(backgroundTexture,                  // Texture
                        new Rectangle(i * backgroundTileSize,             // X position
                        (j * backgroundTileSize) + backgroundPosition,    // Y Position
                        backgroundTileSize, backgroundTileSize),          // Width and Height
                        Color.White);                                     // Color (plan on updating color when the state changes
                }
            }

            switch (gameState)
            {
                case State.MainMenu:
                    foreach (Button button in menuButtons)
                    {
                        button.Draw(_spriteBatch);
                    }
                    ShapeBatch.Begin(GraphicsDevice);
                    ShapeBatch.Box(DesiredWidth / 2 - (int)(DesiredWidth / (1.75 * 2)), (int)(DesiredHeight / 8.25), (int)(DesiredWidth / 1.75), (int)(DesiredHeight / 2.25), Color.Black);
                    ShapeBatch.BoxOutline(DesiredWidth / 2 - (int)(DesiredWidth / (1.75 * 2)), (int)(DesiredHeight / 8.25), (int)(DesiredWidth / 1.75) + 1, (int)(DesiredHeight / 2.25), Color.White);
                    ShapeBatch.BoxOutline(playButtonXPos, menuButtonYPos, menuButtonWidth, menuButtonHeight, Color.White);
                    ShapeBatch.BoxOutline(quitButtonXPos, menuButtonYPos, menuButtonWidth, menuButtonHeight, Color.White);
                    _spriteBatch.End();
                    ShapeBatch.End();

                    _spriteBatch.Begin();
                    _spriteBatch.DrawString(titleFont, "Rig", new Vector2(DesiredWidth / 2 - titleFont.MeasureString("Rig").X / 2, (int)(DesiredHeight / 6.25)), Color.White);
                    _spriteBatch.DrawString(titleFont, "my", new Vector2(DesiredWidth / 2 - titleFont.MeasureString("my").X / 2, (int)(DesiredHeight / 3.6)), Color.White);
                    _spriteBatch.DrawString(titleFont, "Roll", new Vector2(DesiredWidth / 2 - titleFont.MeasureString("Roll").X / 2, (int)(DesiredHeight / 2.50)), Color.White);

                    break;
                case State.Game:
                    
                    ShapeBatch.Begin(GraphicsDevice);
                    ShapeBatch.Box((int)(DesiredWidth * .66), 0, (int)(DesiredWidth / 3.5), DesiredHeight, new Color(12, 7, 15));
                    ShapeBatch.BoxOutline((int)(DesiredWidth * .66), 0, (int)(DesiredWidth / 3.5) + 1, DesiredHeight, Color.White);
                    ShapeBatch.Box((int)(DesiredWidth * .675), (int)(DesiredHeight * .03), (int)(DesiredWidth / 3.9), DesiredHeight / 4, Color.DarkGray);
                    ShapeBatch.Box((int)(DesiredWidth * .675), (int)(DesiredHeight * .31), (int)(DesiredWidth / 3.9), DesiredHeight / 8, Color.DarkGray);
                    ShapeBatch.Box((int)(DesiredWidth * .755), (int)(DesiredHeight * .323), (int)(DesiredWidth / 5.9), DesiredHeight / 10, Color.Black);
                    ShapeBatch.Box((int)(DesiredWidth * .675), (int)(DesiredHeight * .465), (int)(DesiredWidth / 3.9), DesiredHeight / 4, Color.DarkGray);
                    _spriteBatch.End();
                    ShapeBatch.End();
                    _spriteBatch.Begin();

                    foreach (Button button in gameButtons)
                    {
                        button.Draw(_spriteBatch);
                    }
                    _spriteBatch.DrawString(scoreFont, "Round", new Vector2((int)(DesiredWidth * .715) - scoreFont.MeasureString("Round").X / 2, (int)(DesiredHeight * .345)), Color.White);
                    _spriteBatch.DrawString(scoreFont, "Score", new Vector2((int)(DesiredWidth * .715) - scoreFont.MeasureString("Score").X / 2, (int)(DesiredHeight * .375)), Color.White);
                    _spriteBatch.DrawString(scoreFont, $"{roundScore}", new Vector2((int)(DesiredWidth * .835) - (scoreFont.MeasureString("1").X * roundScore.ToString().Length )/ 2 , (int)(DesiredHeight * .36)), Color.White);
                    /*
                    _spriteBatch.Draw(sevenTexture, new Rectangle((int)(DesiredWidth * .765), (int)(DesiredHeight * .345), (int)(DesiredWidth / 32), (int)(DesiredWidth / 32)), Color.White);
                    _spriteBatch.Draw(sevenTexture, new Rectangle((int)(DesiredWidth * .783), (int)(DesiredHeight * .345), (int)(DesiredWidth / 32), (int)(DesiredWidth / 32)), Color.White);
                    _spriteBatch.Draw(sevenTexture, new Rectangle((int)(DesiredWidth * .802), (int)(DesiredHeight * .345), (int)(DesiredWidth / 32), (int)(DesiredWidth / 32)), Color.White);
                    */
                    break;
                case State.Pause:
                    break;
                case State.GameOver:
                    break;
                case State.Quit:
                    break;

            }
            _spriteBatch.Draw(scanlineTexture, new Rectangle(0, 0, DesiredWidth, DesiredHeight), new Color(25, 25, 25, 1));
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Detects if the background has moved off the screen and teleports it back
        /// </summary>
        /// <param name="circle"></param>
        private void BackgroundScreenWrap()
        {
            if (backgroundPosition > backgroundTileSize)
            {
                backgroundPosition = 0;
            }
        }

        /// <summary>
        /// Sets the state to game
        /// </summary>
        /// <param name="circle"></param>
        private void GameState()
        {
            gameState = State.Game;
        }

        /// <summary>
        /// Sets the state to paused
        /// </summary>
        /// <param name="circle"></param>
        private void Pause()
        {
            gameState = State.Pause;
        }

        /// <summary>
        /// Rolls slots
        /// </summary>
        /// <param name="circle"></param>
        private void Roll()
        {
            // call roll logic ig maybe
        }
    }
}
