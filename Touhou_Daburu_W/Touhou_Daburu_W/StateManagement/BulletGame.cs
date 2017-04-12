using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Touhou_Daburu_W.UI.Events;

/*
 * TODO
 *  - Refactor managers to only use events and delegates
 *
 */

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
        MenuManager mMenuManager;

        InfoPrinter mInfoPrinter;
        GameState mGameState;

        enum GameState
        {
            Playing,
            Paused,
            MainMenu,
            ScoreScreen
        }

        public BulletGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            mGameState = GameState.MainMenu;
        }
        
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            InitManagers();

            base.Initialize();
        }

        private void InitManagers()
        {
            mNetworkManager = new NetworkManager();
            mPlayerManager = new PlayerManager();
            mEnemyManager = new EnemyManager();
            mBulletManager = new BulletManager();
            mStageManager = new StageManager();
            mInfoPrinter = new InfoPrinter();
            mMenuManager = new MenuManager(Window);
            
            mStageManager.Init(mEnemyManager, mBulletManager);
            mEnemyManager.SetBulletManager(mBulletManager);
            mPlayerManager.SetBulletManager(mBulletManager);
            mNetworkManager.SetPlayerManager(mPlayerManager);

            mInfoPrinter.mNetworkManager = mNetworkManager;

            mMenuManager.RequestedHost += mNetworkManager.HandleHostRequest;
            mMenuManager.RequestedHost += mPlayerManager.HandleHostRequest;
            mMenuManager.RequestedConnect += mNetworkManager.HandleClientRequest;
            mMenuManager.RequestedConnect += mPlayerManager.HandleClientRequest;
            mNetworkManager.PlayerHasConnected += mStageManager.HandlePlayerConnected;
            mNetworkManager.PlayerHasConnected += HandlePlayerConnected;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mPlayerManager.LoadContent(this.Content);
            mEnemyManager.LoadContent(this.Content);
            mBulletManager.LoadContent(this.Content);
            mInfoPrinter.LoadContent(this.Content);
            mMenuManager.LoadContent(this.Content);
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

            mStageManager.Update(gameTime);
            mPlayerManager.Update(gameTime);
            mEnemyManager.Update(gameTime);
            mBulletManager.Update(gameTime);
            mNetworkManager.Update();

            mMenuManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            mInfoPrinter.Update(gameTime);

            spriteBatch.Begin();
            

            switch (mGameState)
            {
                case GameState.Playing:
                    mBulletManager.Draw(spriteBatch);
                    mEnemyManager.Draw(spriteBatch);
                    mPlayerManager.Draw(spriteBatch);
                    mInfoPrinter.DrawFrameTiming(spriteBatch, gameTime);
                    mInfoPrinter.DrawConnectionInfo(spriteBatch);
                    break;
                case GameState.Paused:
                    break;
                case GameState.MainMenu:
                    mMenuManager.Draw(spriteBatch);
                    break;
                case GameState.ScoreScreen:
                    break;
                default:
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void HandlePlayerConnected(object sender)
        {
            mGameState = GameState.Playing;
        }
    }
}
