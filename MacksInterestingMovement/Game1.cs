using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace MacksInterestingMovement
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public float TIME;

        public int RightBound = 700;
        public int LeftBound = 20;

        Player player;
        Player player2;

        

        OneButtonPlayer OneButtonPlayer;

        FallingObject obj1;

        FPSComponent fps;

        fallingObjectManager gameManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            /*player = new Player(this, 0, "MushroomGuy");
            player2 = new Player(this, 1, "MushroomGuyPurple");
            
            this.Components.Add(player);
            Components.Add(player2);*/
            /*obj1 = new FallingObject(this, "MushroomGuyPurple");
            this.Components.Add(obj1);
            obj1.pos = new Vector2(40, 0);*/

            OneButtonPlayer = new OneButtonPlayer(this, "MushroomGuy");
            this.Components.Add(OneButtonPlayer);

            gameManager = new fallingObjectManager(this, this.Components, this, OneButtonPlayer);
            Components.Add(gameManager);

            fps = new FPSComponent(this, true, true);
            this.Components.Add(fps);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //player.texture = Content.Load<Texture2D>(player);
            /*player.LoadPlayerContent();
            player2.LoadPlayerContent();*/
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            TIME = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            /*player.PlayerUpdate(TIME);
            player2.PlayerUpdate(TIME);*/
            /*if (player.Intersects(player2))
            {
                Debug.WriteLine("Rectangle");
                if (player.PerPixelCollision(player2))
                {
                    Debug.WriteLine("PIXELLSSS");
                }
            }*/

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

           
            base.Draw(gameTime);
            Matrix transformMatrix = Matrix.CreateScale(2);
            //turning alpha blend off is 400% increse in performance bc it doesnt read alpha bits
            //deffered lets the vid card decide, usually back to front
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            //_spriteBatch.Begin();
            /*player.Draw(_spriteBatch);
            player2.Draw(_spriteBatch);*/
            _spriteBatch.End();

        }
    }
}