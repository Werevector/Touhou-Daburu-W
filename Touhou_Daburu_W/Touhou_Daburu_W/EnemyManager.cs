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
    class EnemyManager
    {
        private Dictionary<string, SpriteAtlas> mNameToAtlasMap;
        Dictionary<string, Dictionary<string, SpriteSequenceData>> mNameToSequencesetMap;

        List<Enemy> mEnemies;
        List<int> mEnemyDestroyQueue;

        public EnemyManager()
        {
            mNameToAtlasMap = new Dictionary<string, SpriteAtlas>();
            mNameToSequencesetMap = new Dictionary<string, Dictionary<string, SpriteSequenceData>>();
            mEnemies = new List<Enemy>();
            mEnemyDestroyQueue = new List<int>();
        }

        public void LoadContent(ContentManager content)
        {
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
                    mNameToAtlasMap.Add(clipset.Key, enemyAtlas);
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
                    mNameToSequencesetMap.Add(enemy.Key, sequenceMap);
                }
            }
        }

        public void SpawnEnemy(Enemy enemy)
        {
            enemy.SetAtlas(mNameToAtlasMap[enemy.GetSpriteName()]);
            enemy.SetAnimationSequences(mNameToSequencesetMap[enemy.GetSpriteName()]);
            mEnemies.Add(enemy);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemy in mEnemies)
            {
                enemy.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var enemy in mEnemies)
            {
                enemy.Update(gameTime);
            }

        }

        private void CheckEnemyBounds()
        {
            for (int i = 0; i < mEnemies.Count; i++)
            {
                if (Utility.IsWithinScreenBounds(100, mEnemies[i].GetHitbox()))
                    mEnemyDestroyQueue.Add(i);
            }
        }

        private void ProccessDestroyQueues()
        {
            mEnemyDestroyQueue.Sort();
            mEnemyDestroyQueue.Reverse();
            foreach (var index in mEnemyDestroyQueue)
            {
                mEnemies.RemoveAt(index);
            }
            mEnemyDestroyQueue.Clear();
        }

        public void PrepareForNextStage()
        {
            mEnemies.Clear();
            mEnemyDestroyQueue.Clear();
        }
    }
}
