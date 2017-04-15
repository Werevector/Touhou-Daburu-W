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

namespace BulletEngineNeo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGameLogic : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Dictionary<string, SpriteAtlas> mEnemyNameToAtlasMap;
        private Dictionary<string, Dictionary<string, SpriteSequenceData>> mEnemyNameToSequencesetMap;

        private Dictionary<string, SpriteAtlas> mBulletNameToAtlasMap;

        List<GameBullet> mBullets;

        Enemy mTestEnemy;

        double mTick;
        public MainGameLogic()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.graphics.PreferredBackBufferHeight = 1080;
            this.Window.Position = new Point(0, 0);
            this.Window.IsBorderless = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            mTick = 0.0;   
            mEnemyNameToAtlasMap = new Dictionary<string, SpriteAtlas>();
            mEnemyNameToSequencesetMap = new Dictionary<string, Dictionary<string, SpriteSequenceData>>();
            mBulletNameToAtlasMap = new Dictionary<string, SpriteAtlas>();
            mBullets = new List<GameBullet>();
            base.Initialize();
        }

        /// <summary>
        /// Initialize function running after the content has loaded.
        /// Functions as a post init.
        /// </summary>
        private void PostLoadContentInitialize()
        {
            mTestEnemy = new Enemy();
            mTestEnemy.Position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
            mTestEnemy.SpriteName = "RedFairy";
            mTestEnemy.Initialize(mEnemyNameToAtlasMap[mTestEnemy.SpriteName], mEnemyNameToSequencesetMap[mTestEnemy.SpriteName]);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadEnemyContent();
            LoadBulletContent();
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

            mTestEnemy.Update(gameTime);
            mTick += gameTime.ElapsedGameTime.TotalSeconds;
            double t = 1 / (double)1;
            if(mTick > t)
            {
                GameBullet b = new GameBullet();
                b.SpriteName = "Small7";
                b.Initialize(mBulletNameToAtlasMap[b.SpriteName]);
                b.Position = mTestEnemy.Position;
                b.Velocity = new Vector2(100,0);
                EffectFunction effectF = (GameBullet bullet, MainGameLogic game) =>
                {
                    bullet.BulletColor++;
                    if (bullet.BulletColor > 15)
                        bullet.BulletColor = 0;
                };
                BulletEffect bEffect = new BulletEffect();

                EffectFunction effectF2 = (GameBullet bullet, MainGameLogic game) =>
                {
                    bullet.Velocity += new Vector2(-2f, 2);
                };
                BulletEffect bEffect2 = new BulletEffect();

                bEffect.BulletEffectFunction = effectF;
                bEffect2.BulletEffectFunction = effectF2;

                b.AddOnUpdateEffect(bEffect);
                b.AddOnUpdateEffect(bEffect2);
                mBullets.Add(b);
                mTick = 0.0;
            }

            foreach (var bullet in mBullets)
            {
                bullet.Update(gameTime, this);
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
            mTestEnemy.Draw(gameTime, spriteBatch);
            foreach (var bullet in mBullets)
            {
                bullet.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
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

        private void LoadBulletContent()
        {
            string[] descriptors = System.IO.Directory.GetFiles("Descriptors/Images/Bullet/", "*.json", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var descriptor in descriptors)
            {
                ContentManager content = this.Content;
                string aJson = File.ReadAllText(descriptor);
                AtlasInfo aInfo = JsonConvert.DeserializeObject<AtlasInfo>(aJson);
                SpriteAtlas spriteAtlas = new SpriteAtlas();
                Texture2D image = content.Load<Texture2D>("Images/Bullet/" + aInfo.Image);
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
                    //originMap.Add(clipset.Key, clipset.OriginAngle);
                    originMap.Add(clipset.Key, 90);
                    mBulletNameToAtlasMap.Add(clipset.Key, spriteAtlas);
                }
                spriteAtlas.SetImage(image);
                spriteAtlas.SetClipMap(clipMap, originMap);
            }
        }
    }
}
