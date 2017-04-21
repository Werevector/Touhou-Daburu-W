using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.IO;
using BulletEngineNeo.Graphics;
using BulletEngineNeo.Bullet;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using MoonSharp.Interpreter.Serialization;

namespace BulletEngineNeo
{
    /// <summary>
    /// This is the main type for your game.
    /// 
    /// </summary>
    public class MainGameLogic : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Dictionary<string, SpriteAtlas> mEnemyNameToAtlasMap;
        private Dictionary<string, Dictionary<string, SpriteSequenceData>> mEnemyNameToSequencesetMap;
        
        BulletStorage mBullets;
        
        SpriteFont font;

        public Rectangle mBulletArea;
        double mTick;

        public MainGameLogic()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.graphics.PreferredBackBufferHeight = 1080;
            this.graphics.PreferredBackBufferWidth = 1920 / 2;
            this.Window.Position = new Point(0, 0);
            this.Window.IsBorderless = true;
            int padding = 70;
            mBulletArea.Width = this.graphics.PreferredBackBufferWidth + padding*2;
            mBulletArea.Height = this.graphics.PreferredBackBufferHeight + padding*2;
            mBulletArea.X = 0 - padding;
            mBulletArea.Y = 0 - padding;

            mBullets = new BulletStorage();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            UserData.RegisterType<GameBullet>();
            UserData.RegisterType<Vec2>();

            mTick = 0.0;   
            mEnemyNameToAtlasMap = new Dictionary<string, SpriteAtlas>();
            mEnemyNameToSequencesetMap = new Dictionary<string, Dictionary<string, SpriteSequenceData>>();

            mBullets.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// Initialize function running after the content has loaded.
        /// Functions as a post init.
        /// </summary>
        private void PostLoadContentInitialize()
        {
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("YuyukoFont");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mBullets.LoadContent(this.Content);
            
            PostLoadContentInitialize();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            mBullets.Update(gameTime, this);
            
            mTick += gameTime.ElapsedGameTime.TotalSeconds;
            double t = 1 / (double)15;
            
            if (mTick > t)
            {
                GameBullet b = new GameBullet();
                b.mSpriteName = "Small4";
                b.mBulletColor = 5;
                b.mPosition.x = 200;
                b.mPosition.y = 100;
                b.mVelocity.x = 200;
                b.mVelocity.y = 0;

                mBullets.Add(b, gameTime, this);
                mTick = 0.0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            
            mBullets.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(font, mBullets.getCount().ToString(), new Vector2(0,0), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DestroyBullet(int id)
        {
            mBullets.QueueDestroy(id);
        }

        private void LoadEnemyContent()
        {
            ContentManager content = this.Content;
            string[] descriptors = System.IO.Directory.GetFiles("Descriptors/Images/Enemy/", "*.json", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var descriptor in descriptors)
            {
                string aJson = File.ReadAllText(descriptor);
                EnemyAtlasInfo aInfo = JsonConvert.DeserializeObject<EnemyAtlasInfo>(aJson);
                Texture2D image = content.Load<Texture2D>("Images/Enemy/" + aInfo.Image);
                SpriteAtlas enemyAtlas = new SpriteAtlas();
                Dictionary<string, List<Rectangle>> clipMap = new Dictionary<string, List<Rectangle>>();
                Dictionary<string, int> originMap = new Dictionary<string, int>();
                foreach (var clipset in aInfo.ClipSets)
                {
                    List<Rectangle> clips = new List<Rectangle>();
                    foreach (var clip in clipset.Set)
                    {
                        clips.Add(new Rectangle(clip[0], clip[1], clip[2], clip[3]));
                    }
                    clipMap.Add(clipset.Key, clips);
                    originMap.Add(clipset.Key, 0);
                    mEnemyNameToAtlasMap.Add(clipset.Key, enemyAtlas);
                }
                enemyAtlas.SetImage(image);
                enemyAtlas.SetClipMap(clipMap, originMap);

                foreach (var enemy in aInfo.EnemySequences)
                {
                    Dictionary<string, SpriteSequenceData> sequenceMap = new Dictionary<string, SpriteSequenceData>();
                    foreach (var sequence in enemy.Sequences)
                    {
                        SpriteSequenceData s = new SpriteSequenceData();
                        s.mSequence = sequence.Seq;
                        s.mLooping = sequence.Looping;
                        s.mSubLoop = sequence.SubLoop;
                        sequenceMap.Add(sequence.Key, s);
                    }
                    mEnemyNameToSequencesetMap.Add(enemy.Key, sequenceMap);
                }
            }
        }
    }
}
