using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNInterface.Controls;
using XNInterface.Input;

namespace XNInterfaceTest
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch sb;
        Window win;
#if WINDOWS_PHONE
        WP7Touch touchInput;
#else
        MouseInput mouseInput;
#endif

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(GraphicsDevice);

            win = Content.Load<Window>("test");
            win.Initialise(null);
            win.LoadGraphics(GraphicsDevice, Content);

            win.NamedChildren["testBtn"].Triggered += (b) =>
                {
                    Exit();
                };

#if WINDOWS_PHONE
            touchInput = new WP7Touch(win);
            touchInput.ClearGestures();
            touchInput.EnableTap();
            touchInput.EnableDoubleTap();
#else
            mouseInput = new MouseInput(win);
#endif

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back))
                Exit();

            win.PerformLayout(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

#if WINDOWS_PHONE
            touchInput.HandleGestures();
#else
            mouseInput.HandleMouse(Mouse.GetState());
#endif

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            sb.Begin();
            win.Draw(GraphicsDevice, sb, gameTime.ElapsedGameTime.TotalSeconds);
            sb.End();
            base.Draw(gameTime);
        }
    }
}
