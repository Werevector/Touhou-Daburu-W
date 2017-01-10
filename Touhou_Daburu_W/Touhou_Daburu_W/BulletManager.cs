using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Touhou_Daburu_W
{
    class BulletManager
    {

        private Dictionary<string, SpriteAtlas> mNameToAtlasMap;

        private List<Bullet> mPlayerBullets;
        private List<Bullet> mEnemyBullets;

        private List<int> mPBDestroyQ;
        private List<int> mEBDestroyQ;

        public BulletManager()
        {
            mNameToAtlasMap = new Dictionary<string, SpriteAtlas>();
            mPlayerBullets = new List<Bullet>();
            mEnemyBullets = new List<Bullet>();
            mPBDestroyQ = new List<int>();
            mEBDestroyQ = new List<int>();
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
                    mNameToAtlasMap.Add(clipset.Key, spriteAtlas);
                }
                spriteAtlas.SetImage(image);
                spriteAtlas.SetClipMap(clipMap, originMap);
            }
        }

        public void SpawnEnemyBullet(string bulletType, int bulletColor,
                                     Vector2 pos, Vector2 vel, Vector2 acc, Rectangle hitBox,
                                     bool angled)
        {
            Bullet bullet = new Bullet();
            bullet.mBulletAtlas = mNameToAtlasMap[bulletType];
            bullet.mBulletType = bulletType;
            bullet.mBulletColor = bulletColor;
            bullet.mPosition = pos;
            bullet.mVelocity = vel;
            bullet.mAcceleration = acc;
            bullet.mHitBox = hitBox;
            bullet.mDirectional = angled;
            bullet.mAllied = false;
            
            mEnemyBullets.Add(bullet);
        }

        public bool CheckCollisionEnemy(Rectangle hitbox)
        {
            bool result = false;
            for (int i = 0; i < mEnemyBullets.Count; i++)
            {
                if(hitbox.Intersects(mEnemyBullets[i].mHitBox)){
                    mEBDestroyQ.Add(i);
                    result = true;
                }
            }
            return result;
        }

        public bool CheckCollisionPlayer(Rectangle hitbox)
        {
            bool result = false;
            for (int i = 0; i < mPlayerBullets.Count; i++)
            {
                if (hitbox.Intersects(mPlayerBullets[i].mHitBox))
                {
                    mPBDestroyQ.Add(i);
                    result = true;
                }
            }
            return result;
        }

        public void SpawnPlayerBullet(SpriteAtlas atlas, string bulletType,
                                      Vector2 pos, Vector2 vel, Vector2 acc, Rectangle hitBox,
                                      bool angled)
        {
            Bullet bullet = new Bullet();
            bullet.mBulletAtlas = atlas;
            bullet.mBulletType = bulletType;
            bullet.mPosition = pos;
            bullet.mVelocity = vel;
            bullet.mAcceleration = acc;
            bullet.mHitBox = hitBox;
            bullet.mDirectional = angled;
            bullet.SetAllied();

            mPlayerBullets.Add(bullet);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var bullet in mEnemyBullets)
            {
                bullet.Update(gameTime);
            }
            foreach (var bullet in mPlayerBullets)
            {
                bullet.Update(gameTime);
            }

            CheckBulletBounds();
            ProccessDestroyQueues();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var bullet in mEnemyBullets)
            {
                bullet.Draw(spriteBatch);
            }
            foreach (var bullet in mPlayerBullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        private void CheckBulletBounds()
        {
            int pad = 50;
            Rectangle bound = new Rectangle(-pad, -pad, 1920 + pad * 2, 1080 + pad * 2);
            for (int i = 0; i < mEnemyBullets.Count; i++)
            {
                if (Utility.IsOutsideRect(bound, mEnemyBullets[i].mPosition))
                {
                    mEBDestroyQ.Add(i);
                }
            }

            for (int i = 0; i < mPlayerBullets.Count; i++)
            {
                if (Utility.IsOutsideRect(bound, mPlayerBullets[i].mPosition))
                {
                    mPBDestroyQ.Add(i);
                }
            }
        }

        private void ProccessDestroyQueues()
        {
            mEBDestroyQ.Sort();
            mEBDestroyQ.Reverse();
            foreach (var index in mEBDestroyQ)
            {
                mEnemyBullets.RemoveAt(index);
            }
            mEBDestroyQ.Clear();

            mPBDestroyQ.Sort();
            mPBDestroyQ.Reverse();
            foreach (var index in mPBDestroyQ)
            {
                mPlayerBullets.RemoveAt(index);
            }
            mPBDestroyQ.Clear();
        }
    }
}
