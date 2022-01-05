using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HeartBitSimulation
{
    public class HBSimGame : Game
    {
        public const int WIDTH = 1280;
        public const int HEIGHT = 720;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Color bColor;
        static internal Graphics graphics;
        static internal Monitor monitor;
        static internal Heart heart;

        // font
        SpriteFont font;

        public HBSimGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = WIDTH;
            _graphics.PreferredBackBufferHeight = HEIGHT;
        }

        protected override void Initialize()
        {
            bColor = new Color(0, 25, 0);
            graphics = new Graphics();
            graphics.Init(_graphics);

            monitor = new Monitor();
            monitor.Init();

            heart = new Heart(monitor);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var ks = Keyboard.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bColor);

            graphics.Draw();

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, heart.heartRate.ToString() + " /BPM", new Vector2(0, 0), Color.Green);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
