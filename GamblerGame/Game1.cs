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

        private State gameState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            gameState = State.MainMenu;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
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
            base.Draw(gameTime);
        }
    }
}
