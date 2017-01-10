using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Touhou_Daburu_W
{
    public class BulletGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PlayerManager mPlayerManager;
        EnemyManager mEnemyManager;
        BulletManager mBulletManager;
        StageManager mStageManager;

        public BulletGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //graphics.PreferredBackBufferWidth = 1600;
            //graphics.PreferredBackBufferHeight = 1200;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            mPlayerManager = new PlayerManager();
            mEnemyManager = new EnemyManager();
            mBulletManager = new BulletManager();
            mStageManager = new StageManager();
            mStageManager.Init(mEnemyManager, mBulletManager);
            mPlayerManager.SetBulletManager(mBulletManager);
            base.Initialize();
            GraphicsDevice.Clear(Color.Black);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mPlayerManager.LoadContent(this.Content);
            mEnemyManager.LoadContent(this.Content);
            mBulletManager.LoadContent(this.Content);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mStageManager.Update(gameTime);
            mPlayerManager.Update(gameTime);
            mEnemyManager.Update(gameTime);
            mBulletManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black*0.5f);

            spriteBatch.Begin();
            mBulletManager.Draw(spriteBatch);
            mEnemyManager.Draw(spriteBatch);
            mPlayerManager.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
