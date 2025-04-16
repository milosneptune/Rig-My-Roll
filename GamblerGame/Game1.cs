using System;
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
        const int DesiredWidth = 1920;
        const int DesiredHeight = 1080;

        // Background Values
        const int xAxisTiles = 16; // Number of background texture grids along the x axis
        const int yAxisTiles = xAxisTiles/2+1; // Number of background texture grids along the y axis
        const int backgroundTileSize = DesiredWidth / xAxisTiles; // Size of the background tile texture (height and width because it is a square)
        private int backgroundPosition = 0; // The position of the y value (of the tiole that spawns in the top left)

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D backgroundTexture;
        private State gameState;
        

        // Fields

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
            gameState = State.MainMenu;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("Purple2");
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
                    break;
                case State.Game:
                    break;
                case State.Pause:
                    break;
                case State.GameOver:
                    break;
                case State.Quit:
                    break;
            }
            backgroundPosition+= 2; // moves the position of every tile down each frame
            BackgroundScreenWrap(); // if the position has moved to the size of the tile, it resets its position (appearing to be constantly moving
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            
            // Prints the background as a grid with an extra row off screen
            for(int i = 0; i < xAxisTiles; i++) //columns
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
                    break;
                case State.Game:
                    break;
                case State.Pause:
                    break;
                case State.GameOver:
                    break;
                case State.Quit:
                    break;
            }
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
    }
}
