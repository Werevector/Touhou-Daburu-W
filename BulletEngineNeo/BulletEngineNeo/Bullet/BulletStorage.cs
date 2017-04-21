using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using BulletEngineNeo.Graphics;
using BulletEngineNeo.ScriptEffects;
using BulletEngineNeo.Bullet;
using MoonSharp.Interpreter;

namespace BulletEngineNeo.Bullet
{
    class BulletStorage
    {
        private List<GameBullet> mBullets;
        private List<int> mDestroyQueue;

        private EffectFactory mEffectFactory;
        private Dictionary<string, SpriteAtlas> mBulletNameToAtlasMap;

        private int mBulletIDCounter;

        public BulletStorage()
        {
            mBullets = new List<GameBullet>();
            mDestroyQueue = new List<int>();
            mEffectFactory = new EffectFactory();
            mBulletNameToAtlasMap = new Dictionary<string, SpriteAtlas>();
        }

        public void Initialize()
        {
            mEffectFactory.Initialize();
        }

        public void LoadContent(ContentManager content)
        {
            string[] descriptors = System.IO.Directory.GetFiles("Descriptors/Images/Bullet/", "*.json", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var descriptor in descriptors)
            {                
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

        public void Add(GameBullet b, GameTime gameTime, MainGameLogic game)
        {
            b.Initialize(mBulletNameToAtlasMap[b.mSpriteName], mBulletIDCounter);
            mBulletIDCounter++;
            b.AddOnSpawnEffect(mEffectFactory.CreateEffect(null, "randomcolor"));
            b.OnSpawn(gameTime, game);
            mBullets.Add(b);
        }

        public void QueueDestroy(int id)
        {
            for (int i = 0; i < mBullets.Count; i++)
            {
                if (mBullets[i].GetId() == id) { mDestroyQueue.Add(i); break; }
            }
        }

        public void DestroyNow(int id)
        {
            for (int i = 0; i < mBullets.Count; i++)
            {
                if (mBullets[i].GetId() == id) { mBullets.RemoveAt(i); break; }
            }
        }

        public void Update(GameTime gameTime, MainGameLogic game)
        {
            mEffectFactory.Update();
            foreach (var bullet in mBullets)
            {
                bullet.Update(gameTime, game);
            }
            ProccessDestroyQueue();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var bullet in mBullets)
            {
                bullet.Draw(gameTime, spriteBatch);
            }
        }

        public int getCount() { return mBullets.Count; }

        private void ProccessDestroyQueue()
        {
            mDestroyQueue.Sort();
            mDestroyQueue.Reverse();
            foreach (var index in mDestroyQueue)
            {
                mBullets.RemoveAt(index);
            }
            mDestroyQueue.Clear();
        }

        public List<GameBullet> List() { return mBullets; }
        
    }
}
