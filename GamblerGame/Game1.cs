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
        Store,
        GameOver,
        Options,
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
        const int TotNumOfItems = 5;

        // Background Values
        #region
        const int xAxisTiles = 16; // Number of background texture grids along the x axis
        const int yAxisTiles = xAxisTiles / 2 + 1; // Number of background texture grids along the y axis
        const int backgroundTileSize = DesiredWidth / xAxisTiles; // Size of the background tile texture (height and width because it is a square)
        private int backgroundPosition = 0; // The position of the y value (of the tiole that spawns in the top left)

        private Texture2D scanlineTexture;
        private Texture2D backgroundTexture;
        private SpriteFont pixelFont;
        private SpriteFont pixelFontLarge;
        private SpriteFont scoreFont;
        private SpriteFont reqScoreFont;
        private SpriteFont titleFont;

        private State gameState;
        private State previousState;
        #endregion

        // Menu
        #region
        const int menuButtonWidth = DesiredWidth / 4;
        const int menuButtonHeight = DesiredHeight / 8;
        const int menuButtonYPos = (int)(DesiredHeight / 1.55);
        const int playButtonXPos = DesiredWidth / 2 - (int)(menuButtonWidth * 1.25);
        const int quitButtonXPos = DesiredWidth / 2 + (int)(menuButtonWidth * .25);
        private List<Button> menuButtons = new List<Button>();
        #endregion

        // Game
        #region
        const int pauseButtonWidth = (int)(DesiredWidth / 3.9);
        const int pauseButtonHeight = DesiredHeight / 10;
        const int rollButtonYPos = (int)(DesiredHeight * .874);
        const int pauseButtonYPos = (int)(DesiredHeight * .739);
        const int rollButtonXPos = (int)(DesiredWidth / 5.9);
        const int pauseButtonXPos = (int)(DesiredWidth * .675);
        private int rollButtonDelay = 0;
        private int rollingAnimationDelay = 0;
        private List<Button> gameButtons = new List<Button>();
        private List<Button> pauseButtons = new List<Button>();
        private List<Button> optionsButtons = new List<Button>();
        private List<Button> gameOverButtons = new List<Button>();
        private List<Button> storeButtons = new List<Button>();
        private List<Button> roundButtons = new List<Button>();
        private List<Item> allItems = new List<Item>();
        private List<Symbol> symbols = new List<Symbol>();
        private bool[] displaySymbol;
        private int[] rollingNumber;
        private bool inRound;
        private Inventory inventory;
        #endregion

        private int blackBarYPos = 0;
        private int desiredBlackBarYPos = DesiredHeight / 2 - DesiredHeight / 10;
        SlotMachine slotMachine;
        Store store;

        private int r = 255;
        private int g = 255;
        private int b = 255;

        private int desiredR = 255;
        private int desiredG = 255;
        private int desiredB = 255;

        private int prevR = 255;
        private int prevG = 255;
        private int prevB = 255;

        // Options
        private bool scanlineToggle = true;
        private bool rollingAnimationToggle = true;
        private bool backgroundAnimationToggle = true;

        // Scoring
        #region
        private double roundScore = 0;
        private double rollScore = 0;
        private List<double> rollScores;
        private double totalScore;
        private bool paused = false;
        private int numRolls;
        private int totalRolls; // TODO: subject to change if we decide to make rounds shorter/longer
        private int numRound;
        private int totalRounds = 5;
        private int minScore;
        private bool hasWon;
        private bool godMode;
        private int money;
        private List<Item> playerInventory;
        const int InventoryMaximum = 5;
        #endregion

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
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            gameState = State.MainMenu;
            rng = new Random();
            godMode = false;
            rollingNumber = new int[3];
            slotMachine = new SlotMachine(Content);
            playerInventory = new List<Item>();
            symbols = slotMachine.SlotList[0].Symbols;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("Background/Purple2");
            scanlineTexture = Content.Load<Texture2D>("Background/SCANLINE 1");
            pixelFont = Content.Load<SpriteFont>("Fonts/monogram");
            pixelFontLarge = Content.Load<SpriteFont>("Fonts/monogram2");
            titleFont = Content.Load<SpriteFont>("Fonts/Daydream");
            scoreFont = Content.Load<SpriteFont>("Fonts/Daydream2");
            reqScoreFont = Content.Load<SpriteFont>("Fonts/Daydream3");
            List<Texture2D> buttonTextures = new List<Texture2D> { Content.Load<Texture2D>("UI/Menu/ButtonUnpressed"), Content.Load<Texture2D>("UI/Menu/ButtonPressed") };
            List<Texture2D> checkboxTextures = new List<Texture2D> { Content.Load<Texture2D>("UI/Menu/EmptyCheckBox"), Content.Load<Texture2D>("UI/Menu/CheckedCheckBox") };

            ui = new UIManager(GraphicsDevice,
                 new List<SpriteFont> { pixelFont, titleFont, scoreFont },
                 new List<Texture2D> { backgroundTexture, scanlineTexture });


            // ----------- MENU BUTTONS ---------------
            // Play Game
            menuButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(playButtonXPos - menuButtonWidth / 3, menuButtonYPos, menuButtonWidth, menuButtonHeight),
                    "Play",
                    pixelFont,
                    new Color(15, 15, 15),
                    buttonTextures));
            menuButtons[0].OnLeftButtonClick += GameState;

            // Quit
            menuButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(quitButtonXPos + menuButtonWidth / 3, menuButtonYPos, menuButtonWidth, menuButtonHeight),
                    "Quit",
                    pixelFont,
                    new Color(15, 15, 15),
                    buttonTextures));
            menuButtons[1].OnLeftButtonClick += Exit;

            // Back
            menuButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(DesiredWidth / 2 - menuButtonWidth / 2, menuButtonYPos, menuButtonWidth, menuButtonHeight),
                    "Options",
                    pixelFont,
                    new Color(15, 15, 15),
                    buttonTextures));
            menuButtons[2].OnLeftButtonClick += Options;

            // ----------- GAME BUTTONS ---------------
            // Roll (reposition
            gameButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(pauseButtonXPos, rollButtonYPos, pauseButtonWidth, pauseButtonHeight),
                    "Roll",
                    pixelFont,
                    new Color(120, 30, 30),
                    buttonTextures));
            gameButtons[0].OnLeftButtonClick += Roll;

            // Pause button
            gameButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(pauseButtonXPos, pauseButtonYPos, pauseButtonWidth, pauseButtonHeight),
                    "Pause",
                    pixelFont,
                    new Color(30, 30, 50),
                    buttonTextures));
            gameButtons[1].OnLeftButtonClick += Pause;

            // ----------- PAUSE BUTTONS ---------------
            // Resume
            pauseButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(DesiredWidth / 2 - DesiredWidth / 12, (int)(DesiredHeight / 10) + DesiredHeight / 2 - (int)(DesiredHeight / 3), DesiredWidth / 6, DesiredHeight / 12),
                    "Resume",
                    pixelFont,
                    new Color(40, 80, 50),
                    buttonTextures));
            pauseButtons[0].OnLeftButtonClick += Resume;

            // New Run
            pauseButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(DesiredWidth / 2 - DesiredWidth / 12, (int)(DesiredHeight / 10) + DesiredHeight / 2 - (int)(DesiredHeight / 3) + DesiredHeight / 9, DesiredWidth / 6, DesiredHeight / 12),
                    "New Run",
                    pixelFont,
                    new Color(130, 120, 30),
                    buttonTextures));
            pauseButtons[1].OnLeftButtonClick += GameState;

            // Options
            pauseButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(DesiredWidth / 2 - DesiredWidth / 12, (int)(DesiredHeight / 10) + DesiredHeight / 2 - (int)(DesiredHeight / 3) + (2 * (DesiredHeight / 9)), DesiredWidth / 6, DesiredHeight / 12),
                    "Options",
                    pixelFont,
                    new Color(120, 60, 40),
                    buttonTextures));
            pauseButtons[2].OnLeftButtonClick += Options;

            // Quit
            pauseButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(DesiredWidth / 2 - DesiredWidth / 12, (int)(DesiredHeight / 10) + DesiredHeight / 2 - (int)(DesiredHeight / 3) + (3 * (DesiredHeight / 9)), DesiredWidth / 6, DesiredHeight / 12),
                    "Quit",
                    pixelFont,
                    new Color(80, 30, 30),
                    buttonTextures));
            pauseButtons[3].OnLeftButtonClick += Exit;


            // ----------- STORE BUTTONS -------------------
            storeButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(pauseButtonXPos, rollButtonYPos, pauseButtonWidth, pauseButtonHeight),
                    "Exit Shop",
                    pixelFont,
                    new Color(30, 120, 30),
                    buttonTextures));
            storeButtons[0].OnLeftButtonClick += BackToGame;

            // Pause button
            storeButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(pauseButtonXPos, pauseButtonYPos, pauseButtonWidth, pauseButtonHeight),
                    "Pause",
                    pixelFont,
                    new Color(30, 30, 50),
                    buttonTextures));
            storeButtons[1].OnLeftButtonClick += Pause;

            // ------------ ROUND OVER BUTTONS --------------
            roundButtons.Add(new Button(
                _graphics.GraphicsDevice,
                new Rectangle(DesiredWidth / 40 + (int)(DesiredWidth / 1.65) / 2 - (int)(DesiredWidth / 1.85) / 2, DesiredHeight / 2 - (DesiredWidth / 10) + (DesiredHeight / 60) * 5 + (DesiredHeight / 10) + (DesiredHeight / 14) * 3, (int)(DesiredWidth / 1.85), menuButtonHeight), // Yeah good enough for now. TODO: change if you like gabe 
                "Store",
                pixelFontLarge,
                new Color(80, 30, 30),
                buttonTextures));
            roundButtons[0].OnLeftButtonClick += Store;


            // ----------- GAME OVER BUTTONS ---------------
            // Play Game
            gameOverButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(playButtonXPos, menuButtonYPos, menuButtonWidth, menuButtonHeight),
                    "New Run",
                    pixelFont,
                    new Color(15, 15, 15),
                    buttonTextures));
            gameOverButtons[0].OnLeftButtonClick += GameState;

            // Quit
            gameOverButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(quitButtonXPos, menuButtonYPos, menuButtonWidth, menuButtonHeight),
                    "Main Menu",
                    pixelFont,
                    new Color(15, 15, 15),
                    buttonTextures));
            gameOverButtons[1].OnLeftButtonClick += Menu;

            // ----------- OPTIONS MENU BUTTONS ---------------

            // Background Animation
            optionsButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(DesiredWidth / 2 + DesiredHeight / 12, (int)(DesiredHeight / 10) + DesiredHeight / 2 - (int)(DesiredHeight / 3), DesiredHeight / 12, DesiredHeight / 12),
                    "Rolling Animation Toggle",
                    pixelFont,
                    Color.White,
                    checkboxTextures,
                    rollingAnimationToggle));
            optionsButtons[0].OnLeftButtonClick += ToggleAnimation;

            // Rolling Animation Toggle
            optionsButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(DesiredWidth / 2 + DesiredHeight / 12, (int)(DesiredHeight / 10) + DesiredHeight / 2 - (int)(DesiredHeight / 3) + (DesiredHeight / 9)
                    , DesiredHeight / 12, DesiredHeight / 12),
                    "Background Animation Toggle",
                    pixelFont,
                    Color.White,
                    checkboxTextures,
                    backgroundAnimationToggle));
            optionsButtons[1].OnLeftButtonClick += ToggleBackground;

            // Scanline
            optionsButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(DesiredWidth / 2 + DesiredHeight / 12, (int)(DesiredHeight / 10) + DesiredHeight / 2 - (int)(DesiredHeight / 3) + (int)(2 * (DesiredHeight / 9)), DesiredHeight / 12, DesiredHeight / 12),
                    "Scanline Toggle",
                    pixelFont,
                    Color.White,
                    checkboxTextures,
                    scanlineToggle));
            optionsButtons[2].OnLeftButtonClick += ToggleScanline;
            // Scanline
            optionsButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(DesiredWidth / 2 + DesiredHeight / 12, (int)(DesiredHeight / 10) + DesiredHeight / 2 - (int)(DesiredHeight / 3) - (int)((DesiredHeight / 9)), DesiredHeight / 12, DesiredHeight / 12),
                    "God Mode",
                    pixelFont,
                    Color.White,
                    checkboxTextures,
                    godMode));
            optionsButtons[3].OnLeftButtonClick += ToggleGodmode;

            // Back
            optionsButtons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(DesiredWidth / 2 - menuButtonWidth / 2, menuButtonYPos, menuButtonWidth, menuButtonHeight),
                    "Back",
                    pixelFont,
                    new Color(15, 15, 15),
                    buttonTextures));
            optionsButtons[4].OnLeftButtonClick += Back;


            // Adds a list of all existing items.
            for (int i = 0; i < TotNumOfItems; i++)
            {
                // TODO: Change the textures for the item buttons.
                // TODO: Change the second font for the description.
                allItems.Add(new Item(i, _graphics.GraphicsDevice, pixelFont, pixelFont, buttonTextures, Content));
            }

            // Adds events based on the type of item.
            foreach (Item item in allItems)
            {
                switch (item.Type)
                {
                    // Guaranteeing a symbol in a slot.
                    case '#':
                        item.UseItem += slotMachine.RollSpecificSymbol;
                        break;

                    // Increasing the chances of a symbol in a slot.
                    case '^':
                        item.UseItem += slotMachine.IncreaseSymbolChance;
                        break;

                    // Freezes a slot.
                    case '&':
                        item.UseItem += slotMachine.FreezeSlot;
                        break;

                    // Adds a multiplier.
                    case '*':
                        item.UseItem += Multiplier;
                        break;
                    case '$':
                        item.UseItem += AddScore;
                        break;
                }
            }

            inventory = new Inventory();
            store = new Store(allItems, money, inventory, playerInventory);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            UpdateColor();
            MoveBlackBar();

            // TODO: Add your update logic here

            if (gameState != State.Options)
            {
                previousState = gameState;
                prevR = desiredR;
                prevG = desiredG;
                prevB = desiredG;
            }
            switch (gameState)
            {
                case State.MainMenu:
                    foreach (Button button in menuButtons)
                    {
                        button.Update(gameTime);
                    }
                    if (backgroundAnimationToggle)
                    {
                        backgroundPosition += 2;
                    }
                    break;
                case State.Game:
                    if (!paused && inRound)
                    {
                        gameButtons[1].Update(gameTime);
                        for(int i = 0; i < playerInventory.Count; i++)
                        {
                            playerInventory[i].Update(gameTime);
                        }
                        if (rollButtonDelay == 0)
                        {
                            gameButtons[0].Update(gameTime);
                        }
                        else
                        {
                            if (rollButtonDelay > 0)
                            {
                                rollButtonDelay--;
                            }
                            if (rollButtonDelay == 260)
                            {
                                displaySymbol[0] = true;
                            }
                            if (rollButtonDelay == 180)
                            {
                                displaySymbol[1] = true;
                            }
                            if (rollButtonDelay == 80)
                            {
                                displaySymbol[2] = true;
                            }
                            if (rollButtonDelay == 1)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    displaySymbol[i] = false;
                                }

                            }
                        }
                        if (rollingAnimationDelay == 0)
                        {
                            rollingAnimationDelay = 3;
                        }
                        else
                        {
                            if (rollingAnimationDelay > 0)
                            {
                                rollingAnimationDelay--;
                            }
                            if (rollingAnimationDelay == 2)
                            {
                                rollingNumber[0] = rng.Next(symbols.Count);
                            }
                            if (rollingAnimationDelay == 1)
                            {
                                rollingNumber[1] = rng.Next(symbols.Count);
                            }
                            if (rollingAnimationDelay == 0)
                            {
                                rollingNumber[2] = rng.Next(symbols.Count);
                            }
                        }
                        if (rollButtonDelay == 0)
                        {
                            if (roundScore >= minScore)
                            {
                                if (numRolls == totalRolls)
                                {
                                    numRolls++;
                                }
                                RoundOver();
                                numRound += 1;
                                if (numRolls < totalRolls)
                                {
                                    money += 4 + (totalRolls - numRolls);
                                }
                                else
                                {
                                    money += 4;
                                }
                                if (numRound == totalRounds)
                                {
                                    hasWon = true;
                                    gameState = State.GameOver;
                                    desiredR = 75;
                                    desiredG = 200;
                                    desiredB = 75;
                                }
                            }
                            else if (numRolls == totalRolls && roundScore < minScore)
                            {
                                hasWon = false;
                                gameState = State.GameOver;
                                desiredR = 200;
                                desiredG = 75;
                                desiredB = 75;
                            }
                        }
                        if (backgroundAnimationToggle)
                        {
                            backgroundPosition++;
                        }

                    }
                    else if (paused)
                    {
                        foreach (Button button in pauseButtons)
                        {
                            button.Update(gameTime);
                        }
                    }
                    if (!inRound)
                    {
                        foreach (Button button in roundButtons)
                        {
                            button.Update(gameTime);
                        }
                    }
                    break;
                case State.Store:
                    if (!paused)
                    {
                        if (store.StoreItems == null)
                        {
                            store.UpdateStore(money, inventory);
                            store.StoreInteraction(rng, gameTime);
                        }
                        if (backgroundAnimationToggle)
                        {
                            backgroundPosition += 2;
                        }
                        money = store.Money;
                        foreach (Button button in storeButtons)
                        {
                            button.Update(gameTime);
                        }
                        store.Update(gameTime);
                    }
                    else
                    {
                        foreach (Button button in pauseButtons)
                        {
                            button.Update(gameTime);
                        }
                    }
                    break;
                case State.Options:
                    optionsButtons[0].CheckboxUpdate(gameTime, rollingAnimationToggle);
                    optionsButtons[1].CheckboxUpdate(gameTime, backgroundAnimationToggle);
                    optionsButtons[2].CheckboxUpdate(gameTime, scanlineToggle);
                    if (previousState != State.Game)
                    {
                        optionsButtons[3].CheckboxUpdate(gameTime, godMode);
                    }
                    optionsButtons[4].Update(gameTime);
                    if (backgroundAnimationToggle)
                    {
                        backgroundPosition += 2;
                    }
                    break;
                case State.GameOver:
                    if (backgroundAnimationToggle)
                    {
                        backgroundPosition += 2;
                    }
                    if (hasWon)
                    {

                    }
                    else
                    {

                    }
                    foreach (Button button in gameOverButtons)
                    {
                        button.Update(gameTime);
                    }
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
                    ui.DrawGameBar(_spriteBatch);
                    ui.DrawGameSlot(_spriteBatch);
                    _spriteBatch.Begin();

                    foreach (Button button in gameButtons)
                    {
                        button.Draw(_spriteBatch);
                    }

                    _spriteBatch.DrawString(reqScoreFont, $"{minScore}", new Vector2((int)(DesiredWidth * .725) + (reqScoreFont.MeasureString($"Score Req: {minScore}").X) / 2, (int)(DesiredHeight * .03) + DesiredHeight / 8 - (reqScoreFont.MeasureString($"Score Req: {minScore}").Y) / 4), Color.White);
                    // Round score variable displayed
                    _spriteBatch.DrawString(scoreFont, $"{roundScore}", new Vector2((int)(DesiredWidth * .835) - (scoreFont.MeasureString("1").X * roundScore.ToString().Length) / 2, (int)(DesiredHeight * .36)), Color.White);

                    if (slotMachine.SymbolList != null)
                    {
                        for (int i = 0; i < slotMachine.SymbolList.Count; i++)
                        {
                            if (displaySymbol[i] == true)
                            {
                                slotMachine.SymbolList[i].DrawSymbol(_spriteBatch, DesiredWidth / 20 + (int)((DesiredWidth / 5.666) * i), DesiredHeight / 2 - (DesiredWidth / 10) - DesiredHeight / 100, DesiredWidth / 5, DesiredWidth / 5);
                            }
                            else
                            {
                                if (rollingAnimationToggle && !slotMachine.SlotList[i].Frozen)
                                {
                                    symbols[rollingNumber[i]].DrawSymbol(_spriteBatch, DesiredWidth / 20 + (int)((DesiredWidth / 5.666) * i), DesiredHeight / 2 - (DesiredWidth / 10) - DesiredHeight / 100, DesiredWidth / 5, DesiredWidth / 5);
                                }
                                else if (slotMachine.SlotList[i].Frozen)
                                {
                                    slotMachine.SlotList[i].Result.DrawSymbol(_spriteBatch, DesiredWidth / 20 + (int)((DesiredWidth / 5.666) * i), DesiredHeight / 2 - (DesiredWidth / 10) - DesiredHeight / 100, DesiredWidth / 5, DesiredWidth / 5);
                                }
                            }
                        }
                    }
                    int rollCount = numRolls;
                    if (numRolls > totalRolls)
                    {
                        rollCount = totalRolls;
                    }

                    _spriteBatch.DrawString(scoreFont, $"Rolls: {rollCount}/{totalRolls}", new Vector2((int)(DesiredWidth * .690), (int)(DesiredHeight * .55)), Color.White);
                    _spriteBatch.DrawString(scoreFont, $"Rounds: {numRound + 1}/{totalRounds}", new Vector2((int)(DesiredWidth * .690), (int)(DesiredHeight * .60)), Color.White);
                    _spriteBatch.DrawString(scoreFont, $"Money: {money}", new Vector2((int)(DesiredWidth * .690), (int)(DesiredHeight * .65)), Color.White);

                    DisplayScoreList();
                    DrawPlayerInventory();

                    _spriteBatch.End();
                    if (paused)
                    {
                        ui.DrawPaused(_spriteBatch, symbols);

                        _spriteBatch.Begin();
                        foreach (Button button in pauseButtons)
                        {
                            button.Draw(_spriteBatch);
                        }
                        _spriteBatch.End();
                    }
                    if (!inRound)
                    {
                        ui.DrawRoundEnd(_spriteBatch);
                        _spriteBatch.Begin();
                        _spriteBatch.DrawString(reqScoreFont, $"Round {numRound} Over", new Vector2((DesiredWidth / 40 + DesiredWidth / 1.65f) / 2 - reqScoreFont.MeasureString($"Round {numRound} Over").X / 2, DesiredHeight / 2 - titleFont.MeasureString($"Round {numRound - 1}").Y), Color.White);

                        _spriteBatch.DrawString(scoreFont, $"Required Score:", new Vector2((DesiredWidth / 40 + DesiredWidth / 80) + DesiredWidth / 80, DesiredHeight / 2 - (DesiredWidth / 10) + (DesiredHeight / 60) * 2 + (DesiredHeight / 10) + (DesiredHeight / 28) - (scoreFont.MeasureString($"Required Score:").Y) / 2), Color.White);
                        _spriteBatch.DrawString(reqScoreFont, $"{minScore}", new Vector2((DesiredWidth / 40 + DesiredWidth / 80) + DesiredWidth / 80 + (scoreFont.MeasureString($"Required Score: ").X), DesiredHeight / 2 + (DesiredHeight / 60) * 2 + (DesiredHeight / 28) - (DesiredWidth / 10) + (DesiredHeight / 10) - (reqScoreFont.MeasureString($"{minScore}").Y) / 2), Color.White);

                        _spriteBatch.DrawString(scoreFont, $"Score Obtained:", new Vector2((DesiredWidth / 40 + DesiredWidth / 80) + DesiredWidth / 80, DesiredHeight / 2 - (DesiredWidth / 10) + (DesiredHeight / 60) * 3 + (DesiredHeight / 10) + (DesiredHeight / 28) + (DesiredHeight / 14) - (scoreFont.MeasureString($"Score Req: {minScore}").Y) / 2), Color.White);
                        _spriteBatch.DrawString(reqScoreFont, $"{roundScore}", new Vector2((DesiredWidth / 40 + DesiredWidth / 80) + DesiredWidth / 80 + (scoreFont.MeasureString($"Score Obtained: ").X), DesiredHeight / 2 + (DesiredHeight / 60) * 3 + (DesiredHeight / 14) - (DesiredWidth / 10) + (DesiredHeight / 10) + (DesiredHeight / 28) - (reqScoreFont.MeasureString($"{roundScore}").Y) / 2), Color.White);

                        _spriteBatch.DrawString(scoreFont, $"Money: ", new Vector2((DesiredWidth / 40 + DesiredWidth / 80) + DesiredWidth / 80, DesiredHeight / 2 + (DesiredHeight / 10) - (DesiredWidth / 10) + (DesiredHeight / 60) * 4 + (DesiredHeight / 28) + (DesiredHeight / 14) * 2 - (scoreFont.MeasureString($"Score Req: {minScore}").Y) / 2), Color.White);
                        _spriteBatch.DrawString(pixelFont, $"Remaining Rolls ", new Vector2((DesiredWidth / 40 + DesiredWidth / 80) + DesiredWidth / 80 + (scoreFont.MeasureString($"Money: ").X), DesiredHeight / 2 + (DesiredHeight / 10) - (DesiredWidth / 10) + (DesiredHeight / 60) * 4 + (DesiredHeight / 28) + (DesiredHeight / 14) * 2 - (pixelFont.MeasureString($"Remaining Rolls").Y) / 1.5f), Color.White);

                        int remainingRolls = totalRolls - numRolls;
                        if (numRolls > totalRolls)
                        {
                            remainingRolls = 0;
                        }
                        _spriteBatch.DrawString(reqScoreFont, $" {remainingRolls} + ", new Vector2((DesiredWidth / 40 + DesiredWidth / 80) + DesiredWidth / 80 + (scoreFont.MeasureString($"Money: ").X + (pixelFont.MeasureString($"RemainingRolls ").X)), DesiredHeight / 2 - (DesiredWidth / 10) + (DesiredHeight / 60) * 4 + (DesiredHeight / 10) + (DesiredHeight / 14) * 2 + (DesiredHeight / 28) - (reqScoreFont.MeasureString($"{minScore}").Y) / 2), Color.White);
                        _spriteBatch.DrawString(pixelFont, $"Current Money ", new Vector2((DesiredWidth / 40 + DesiredWidth / 80) + DesiredWidth / 80 + (scoreFont.MeasureString($"Money: ").X) + (pixelFont.MeasureString($"RemainingRolls ").X) + (reqScoreFont.MeasureString($" {remainingRolls} + ").X), DesiredHeight / 2 + (DesiredHeight / 10) - (DesiredWidth / 10) + (DesiredHeight / 60) * 4 + (DesiredHeight / 28) + (DesiredHeight / 14) * 2 - (pixelFont.MeasureString($"Current Money").Y) / 1.5f), Color.White);

                        _spriteBatch.DrawString(reqScoreFont, $"{money - remainingRolls - 4} + 4 = ${money}", new Vector2((DesiredWidth / 40 + DesiredWidth / 80) + DesiredWidth / 80 + (scoreFont.MeasureString($"Money: ").X) + (pixelFont.MeasureString($"RemainingRolls ").X) + (reqScoreFont.MeasureString($" {remainingRolls} + ").X) + pixelFont.MeasureString("Current Money ").X, DesiredHeight / 2 - (DesiredWidth / 10) + (DesiredHeight / 60) * 4 + (DesiredHeight / 10) + (DesiredHeight / 14) * 2 + (DesiredHeight / 28) - (reqScoreFont.MeasureString($"{minScore}").Y) / 2), Color.White);

                        //_spriteBatch.DrawString(scoreFont, $"Money:", new Vector2((DesiredWidth / 40 + DesiredWidth / 80) + DesiredWidth / 80, DesiredHeight / 2 - (DesiredWidth / 10) + (DesiredHeight / 60) * 3 + (DesiredHeight / 10) + (DesiredHeight / 14) + (scoreFont.MeasureString($"Score Req: {minScore}").Y) / 2), Color.White);

                        //(DesiredWidth / 40 + DesiredWidth / 80, DesiredHeight / 2 - (DesiredWidth / 10) + (DesiredHeight / 60) * 3 + (DesiredHeight / 10) + (DesiredHeight / 14), DesiredWidth / 1.65f - DesiredWidth / 40, (DesiredHeight / 14), new Color(0, 0, 0));
                        foreach (Button button in roundButtons)
                        {
                            button.Draw(_spriteBatch);
                        }
                        _spriteBatch.End();
                    }
                    break;
                case State.Store:

                    ui.DrawGameBar(_spriteBatch);
                    _spriteBatch.Begin();
                    _spriteBatch.DrawString(scoreFont, $"Rolls: 0", new Vector2((int)(DesiredWidth * .690), (int)(DesiredHeight * .55)), Color.White);
                    _spriteBatch.DrawString(scoreFont, $"Rounds: {numRound + 1}/{totalRounds}", new Vector2((int)(DesiredWidth * .690), (int)(DesiredHeight * .60)), Color.White);
                    _spriteBatch.DrawString(scoreFont, $"Money: {money}", new Vector2((int)(DesiredWidth * .690), (int)(DesiredHeight * .65)), Color.White);
                    foreach (Button button in storeButtons)
                    {
                        button.Draw(_spriteBatch);
                    }
                    store.Draw(_spriteBatch);
                    DrawPlayerInventory();
                    _spriteBatch.End();
                    if (paused)
                    {
                        ui.DrawPaused(_spriteBatch, symbols);
                        _spriteBatch.Begin();
                        foreach (Button button in pauseButtons)
                        {
                            button.Draw(_spriteBatch);
                        }
                        _spriteBatch.End();
                    }

                    break;
                case State.Options:
                    for(int i = 0; i < optionsButtons.Count; i++)
                    {
                        if (previousState == State.Game && i == 3)
                        {
                            continue;
                        }
                        else
                        {
                            optionsButtons[i].Draw(_spriteBatch);
                        }
                    }
                    _spriteBatch.End();
                    break;
                case State.GameOver:
                    if (hasWon)
                    {
                        _spriteBatch.DrawString(titleFont, "You Win", new Vector2(DesiredWidth / 2 - titleFont.MeasureString("You Win").X / 2, DesiredHeight / 2 - titleFont.MeasureString("You Win").Y / 2), Color.White);
                    }
                    else
                    {
                        _spriteBatch.DrawString(titleFont, "You Lose", new Vector2(DesiredWidth / 2 - titleFont.MeasureString("You Lose").X / 2, DesiredHeight / 2 - titleFont.MeasureString("You Lose").Y / 2), Color.White);
                    }
                    foreach (Button button in gameOverButtons)
                    {
                        button.Draw(_spriteBatch);
                    }
                    _spriteBatch.End();
                    break;
                case State.Quit:
                    break;
            }

            ui.DrawBlackBars(blackBarYPos);
            if (scanlineToggle)
            {
                ui.DrawScreenFilters(_spriteBatch);
                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Detects if the background has moved off the screen and teleports it back
        /// </summary>
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
        private void GameState()
        {
            Reset();
            slotMachine.Reset();
            desiredR = 100;
            desiredG = 230;
            desiredB = 175;
            desiredBlackBarYPos = DesiredHeight / 2;
            gameState = State.Game;
            inRound = true;
        }

        /// <summary>
        /// Sets the state to game
        /// </summary>
        private void Options()
        {
            desiredR = 235;
            desiredG = 172;
            desiredB = 41;
            desiredBlackBarYPos = DesiredHeight / 2 - DesiredHeight / 10;
            gameState = State.Options;
        }

        /// <summary>
        /// Sets the state to paused
        /// </summary>
        private void Pause()
        {
            desiredR = 67;
            desiredG = 78;
            desiredB = 78;
            desiredBlackBarYPos = DesiredHeight / 2 - DesiredHeight / 10;
            paused = true;
        }

        /// <summary>
        /// Sets the state to resumed
        /// </summary>
        private void Resume()
        {
            desiredR = 100;
            desiredG = 230;
            desiredB = 175;
            desiredBlackBarYPos = DesiredHeight / 2;
            paused = false;
        }
        /// <summary>
        /// Sets the state to Menu
        /// </summary>
        private void Menu()
        {
            desiredR = 255;
            desiredG = 255;
            desiredB = 255;
            gameState = State.MainMenu;
        }

        private void Store()
        {
            desiredR = 255;
            desiredG = 156;
            desiredB = 204;
            gameState = State.Store;
        }

        private void RoundOver()
        {
            desiredR = 255;
            desiredG = 92;
            desiredB = 171;
            inRound = false;
        }

        /// <summary>
        /// Rolls slots
        /// </summary>
        /// <param name="circle"></param>
        private void Roll()
        {
            if (numRolls < totalRolls)
            {
                rollButtonDelay = 300;
                displaySymbol = new bool[3] { false, false, false };
                slotMachine.ScoreList.Clear();
                slotMachine.Roll(rng);
                rollScores = new List<double>();
                rollScores = slotMachine.ScoreList;
                rollScore = slotMachine.RollTotal;
                roundScore += slotMachine.RollTotal;
                numRolls++;
            }
        }

        private void DisplayScoreList()
        {
            int move = 0;

            _spriteBatch.DrawString(scoreFont, "(", new Vector2(((int)(DesiredWidth * .700) - (scoreFont.MeasureString("1").X * roundScore.ToString().Length) / 2) + move, (int)(DesiredHeight * .500)), Color.White);
            for (int i = 0; i < slotMachine.ScoreList.Count; i++)
            {
                string score = slotMachine.ScoreList[i].ToString();
                if (i != slotMachine.ScoreList.Count - 1)
                {
                    score = score + " + ";
                }
                _spriteBatch.DrawString(scoreFont, score, new Vector2(((int)(DesiredWidth * .715) - (scoreFont.MeasureString("1").X * roundScore.ToString().Length) / 2) + move, (int)(DesiredHeight * .500)), Color.White);
                move += (int)scoreFont.MeasureString(score).X;
            }
            _spriteBatch.DrawString(scoreFont, "  )  x  " + slotMachine.Multiplier.ToString(), new Vector2(((int)(DesiredWidth * .715) - (scoreFont.MeasureString("1").X * roundScore.ToString().Length) / 2) + move, (int)(DesiredHeight * .500)), Color.White);
        }

        /// <summary>
        /// Updates color values r, g, and b for the background
        /// </summary>
        private void UpdateColor()
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

        /// <summary>
        /// Adds a multiplier for the score.
        /// </summary>
        /// <param name="action"></param>
        public void Multiplier(string action)
        {
            slotMachine.IncreaseMultipler(int.Parse(action));
        }
        /// <summary>
        /// Increases the points.
        /// </summary>
        /// <param name="action"></param>
        public void AddScore(string action)
        {
            slotMachine.IncreasePoints(int.Parse(action));
        }

        public void BackToGame()
        {
            desiredR = 100;
            desiredG = 230;
            desiredB = 175;
            roundScore = 0;
            numRolls = 0;
            rollScore = 0;
            rollScores = new List<double>();
            minScore = (int)(minScore * 1.5);
            foreach(Slot slot in slotMachine.SlotList)
            {
                slot.Unfreeze();
            }
            inRound = true;
            gameState = State.Game;
            inventory = store.Inventory;
            store.Reset();
        }

        public void Back()
        {
            gameState = previousState;
            desiredR = prevR;
            desiredG = prevG;
            desiredB = prevB;
        }

        public void Reset()
        {
            if (godMode)
            {
                roundScore = 0;
                numRound = 0;
                rollButtonDelay = 0;
                rollScore = 0;
                rollScores = new List<double>();
                totalScore = 0;
                paused = false;
                numRolls = 0;
                minScore = 10;
                hasWon = false;
                totalRolls = 10000;
                money = 10000;
                inventory.Items = new List<Item>();
                slotMachine.Reset();
                store.Reset();
            }
            else
            {
                roundScore = 0;
                numRound = 0;
                rollButtonDelay = 0;
                rollScore = 0;
                rollScores = new List<double>();
                totalScore = 0;
                paused = false;
                numRolls = 0;
                minScore = 300;
                hasWon = false;
                totalRolls = 5;
                money = 4;
                inventory.Items = new List<Item>();
                slotMachine.Reset();
                store.Reset();
            }
        }

        public void ToggleScanline()
        {
            if (scanlineToggle)
            {
                scanlineToggle = false;
            }
            else
            {
                scanlineToggle = true;
            }
        }

        public void ToggleGodmode()
        {
            if (godMode)
            {
                godMode = false;
            }
            else
            {
                godMode = true;
            }
        }

        public void ToggleAnimation()
        {
            if (rollingAnimationToggle)
            {
                rollingAnimationToggle = false;
            }
            else
            {
                rollingAnimationToggle = true;
            }
        }

        public void ToggleBackground()
        {
            if (backgroundAnimationToggle)
            {
                backgroundAnimationToggle = false;
                backgroundPosition = 0;
            }
            else
            {
                backgroundAnimationToggle = true;
            }
        }

        public void DrawPlayerInventory()
        {
            int pos = DesiredWidth / 20 - DesiredWidth / 60; 
            if (playerInventory != null)
            {
                // this line is for testing if it would display. 
                //inventory.Items.Add(allItems[0]);
                for (int i = playerInventory.Count-1; i >= 0 ; i--)
                {
                    playerInventory[i].PlayerInv = playerInventory;
                    playerInventory[i].Bought = true;
                    playerInventory[i].SetLocations((DesiredWidth / 40) + pos, +DesiredHeight / 21);
                    playerInventory[i].Draw(_spriteBatch);
                    pos += playerInventory[i].ItemTexture.Width + (int)(DesiredWidth / 39.5);
                }
            }
        }
    }
}
