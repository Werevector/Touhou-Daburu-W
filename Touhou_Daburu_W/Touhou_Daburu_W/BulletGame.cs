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
        NetworkManager mNetworkManager;

        InfoPrinter mInfoPrinter;

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
            mEnemyManager.SetBulletManager(mBulletManager);
            mPlayerManager.SetBulletManager(mBulletManager);

            mInfoPrinter = new InfoPrinter();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mPlayerManager.LoadContent(this.Content);
            mEnemyManager.LoadContent(this.Content);
            mBulletManager.LoadContent(this.Content);
            mInfoPrinter.LoadContent(this.Content);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        const int port = 8090;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.I))
                CreateNetServer(port);
            if (keyboard.IsKeyDown(Keys.O))
                CreateNetClient();
            if (keyboard.IsKeyDown(Keys.P))
                ConnectToServer("25.102.234.66", port);

            
            mStageManager.Update(gameTime);
            mPlayerManager.Update(gameTime);
            mEnemyManager.Update(gameTime);
            mBulletManager.Update(gameTime);
            if (mNetworkManager != null)
            {
                mNetworkManager.Update();
            }

            base.Update(gameTime);
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            mBulletManager.Draw(spriteBatch);
            mEnemyManager.Draw(spriteBatch);
            mPlayerManager.Draw(spriteBatch);
            mInfoPrinter.DrawFrameTiming(spriteBatch, gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void CreateNetServer(int port)
        {
            if (mNetworkManager == null)
            {
                mNetworkManager = new NetworkManager();
                mNetworkManager.InitAsServer(port); 
                mInfoPrinter.mNetworkManager = mNetworkManager;
                mPlayerManager.InitAsMaster();
                mNetworkManager.SetPlayerManager(mPlayerManager);
            }
        }

        private void CreateNetClient()
        {
            if (mNetworkManager == null)
            {
                mNetworkManager = new NetworkManager();
                mNetworkManager.InitAsClient();
                mInfoPrinter.mNetworkManager = mNetworkManager;
                mPlayerManager.InitAsSlave();
                mNetworkManager.SetPlayerManager(mPlayerManager);
            }
        }

        private void ConnectToServer(string ip, int port)
        {
            if (mNetworkManager != null)
                mNetworkManager.Connect(ip, port);
        }
    }
}
