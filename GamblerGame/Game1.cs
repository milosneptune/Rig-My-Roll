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
        private Random rng;

        private UIManager ui;

        const int DesiredWidth = 1920;
        const int DesiredHeight = 1080;

        // Background Values
        const int xAxisTiles = 16; // Number of background texture grids along the x axis
        const int yAxisTiles = xAxisTiles / 2 + 1; // Number of background texture grids along the y axis
        const int backgroundTileSize = DesiredWidth / xAxisTiles; // Size of the background tile texture (height and width because it is a square)
        private int backgroundPosition = 0; // The position of the y value (of the tiole that spawns in the top left)

        private Texture2D scanlineTexture;
        private Texture2D backgroundTexture;
        private Texture2D buttonUnpressed;
        private Texture2D buttonPressed;
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

        private int blackBarYPos = 0;
        private int desiredBlackBarYPos = DesiredHeight/2 - DesiredHeight/10;
        SlotMachine slotMachine;

        private int r = 255;
        private int g = 255;
        private int b = 255;

        private int desiredR = 255;
        private int desiredG = 255;
        private int desiredB = 255;

        // Symbols
        private Texture2D sevenTexture;

        // Scoring
        private double roundScore = 0;
        private double rollScore;
        private double totalScore;

        Symbol tester;
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
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            gameState = State.MainMenu;
            slotMachine = new SlotMachine(Content);
             tester = new Symbol(SymbolName.Seven, Content);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("Background/Purple2");
            buttonUnpressed = Content.Load<Texture2D>("UI/Menu/ButtonUnpressed");
            buttonPressed = Content.Load<Texture2D>("UI/Menu/ButtonPressed");
            scanlineTexture = Content.Load<Texture2D>("Background/SCANLINE 1");
            sevenTexture = Content.Load<Texture2D>("UI/Symbols/seven");
            pixelFont = Content.Load<SpriteFont>("Fonts/monogram");
            titleFont = Content.Load<SpriteFont>("Fonts/Daydream");
            scoreFont = Content.Load<SpriteFont>("Fonts/Daydream2");
            List <Texture2D> buttonTextures = new List<Texture2D> { buttonUnpressed, buttonPressed };

            ui = new UIManager(GraphicsDevice, new List<SpriteFont> { pixelFont, titleFont, scoreFont }, new List<Texture2D> { backgroundTexture, scanlineTexture, sevenTexture });

            // Play Game
            menuButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(playButtonXPos, menuButtonYPos, menuButtonWidth, menuButtonHeight),
                    "Play",
                    pixelFont,
                    Color.Black,
                    buttonTextures));
            menuButtons[0].OnLeftButtonClick += GameState;

            // Quit
            menuButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(quitButtonXPos, menuButtonYPos, menuButtonWidth, menuButtonHeight),
                    "Quit",
                    pixelFont,
                    Color.Black,
                    buttonTextures));
            menuButtons[1].OnLeftButtonClick += Exit;

            // Roll (reposition
            gameButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(pauseButtonXPos, rollButtonYPos, pauseButtonWidth, pauseButtonHeight),
                    "Roll",
                    pixelFont,
                    Color.Black,
                    buttonTextures));
            gameButtons[0].OnLeftButtonClick += Roll;

            // Pause button
            gameButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(pauseButtonXPos, pauseButtonYPos, pauseButtonWidth, pauseButtonHeight),
                    "Pause",
                    pixelFont,
                    Color.Black,
                    buttonTextures));
            gameButtons[1].OnLeftButtonClick += Pause;
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            updateColor();
            MoveBlackBar();

            // TODO: Add your update logic here
            switch (gameState)
            {
                case State.MainMenu:
                    foreach (Button button in menuButtons)
                    {
                        button.Update(gameTime);
                    }
                    backgroundPosition += 2;
                    break;
                case State.Game:
                    foreach (Button button in gameButtons)
                    {
                        button.Update(gameTime);
                    }
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
            if (backgroundPosition != DesiredHeight / 2)
            {
                ui.DrawBackground(_spriteBatch, backgroundPosition, new Color(r, g, b));
            }
            switch (gameState)
            {
                case State.MainMenu:
                    foreach (Button button in menuButtons)
                    {
                        button.Draw(_spriteBatch);
                    }
                    ui.DrawMenu(_spriteBatch);
                    break;
                case State.Game:
                    ui.DrawGame(_spriteBatch);

                    // --- TESTING ---
                    //tester.DrawSymbol(_spriteBatch);


                    foreach (Button button in gameButtons)
                    {
                        button.Draw(_spriteBatch);
                    }
                    // Round score variable displayed
                    _spriteBatch.DrawString(scoreFont, $"{roundScore}", new Vector2((int)(DesiredWidth * .835) - (scoreFont.MeasureString("1").X * roundScore.ToString().Length) / 2, (int)(DesiredHeight * .36)), Color.White);
                    /*
                    _spriteBatch.Draw(sevenTexture, new Rectangle((int)(DesiredWidth * .765), (int)(DesiredHeight * .345), (int)(DesiredWidth / 32), (int)(DesiredWidth / 32)), Color.White);
                    _spriteBatch.Draw(sevenTexture, new Rectangle((int)(DesiredWidth * .783), (int)(DesiredHeight * .345), (int)(DesiredWidth / 32), (int)(DesiredWidth / 32)), Color.White);
                    _spriteBatch.Draw(sevenTexture, new Rectangle((int)(DesiredWidth * .802), (int)(DesiredHeight * .345), (int)(DesiredWidth / 32), (int)(DesiredWidth / 32)), Color.White);
                    */
                    _spriteBatch.End();
                    break;
                case State.Pause:
                    break;
                case State.GameOver:
                    break;
                case State.Quit:
                    break;
            }

            ui.DrawBlackBars(GraphicsDevice, blackBarYPos);
            ui.DrawScreenFilters(_spriteBatch);
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
        /// Detects if the background has moved off the screen and teleports it back
        /// </summary>
        /// <param name="circle"></param>
        private void MoveBlackBar()
        {
            if (blackBarYPos < desiredBlackBarYPos)
            {
                blackBarYPos += 2;
            }
            else if (blackBarYPos > desiredBlackBarYPos)
            {
                blackBarYPos -= 2;
            }
        }

        /// <summary>
        /// Sets the state to game
        /// </summary>
        /// <param name="circle"></param>
        private void GameState()
        {
            desiredR = 100;
            desiredG = 230;
            desiredB = 175;
            desiredBlackBarYPos = DesiredHeight / 2;
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
        /// Sets the state to Menu
        /// </summary>
        /// <param name="circle"></param>
        private void Menu()
        {
            desiredR = 255;
            desiredG = 255;
            desiredB = 255;
            gameState = State.MainMenu;
        }

        /// <summary>
        /// Rolls slots
        /// </summary>
        /// <param name="circle"></param>
        private void Roll()
        {
            slotMachine.Roll(rng);
            rollScore = slotMachine.RollScore;

            roundScore = slotMachine.RoundScore;
            totalScore += slotMachine.RoundScore;
            slotMachine.DrawSymbols(_spriteBatch);

        }

        /// <summary>
        /// Updates color values r, g, and b for the background
        /// </summary>
        private void updateColor()
        {
            if (r > desiredR)
            {
                r--;
            }
            else if (r < desiredR)
            {
                r++;
            }

            if (g > desiredG)
            {
                g--;
            }
            else if (g < desiredG)
            {
                g++;
            }

            if (b > desiredB)
            {
                b--;
            }
            else if (b < desiredB)
            {
                b++;
            }
        }
    }
}
