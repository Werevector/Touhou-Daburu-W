using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Touhou_Daburu_W.UI.Events;


namespace Touhou_Daburu_W
{
    class PlayerManager
    {
        Player mPlayerOne;
        Player mPlayerTwo;

        private Dictionary<string, SpriteAtlas> mNameToAtlas;
        Dictionary<string, Dictionary<string, SpriteSequenceData>> mNameToSequenceset;

        BulletManager mBulletManager;

        bool mIsSlaveManager;

        public PlayerManager()
        {
            mIsSlaveManager = false;
            mNameToAtlas = new Dictionary<string, SpriteAtlas>();
            mNameToSequenceset = new Dictionary<string, Dictionary<string, SpriteSequenceData>>();
        }

        public void HandleHostRequest(object sender, HostRequestedArgs a)
        {
            InitAsMaster();
        }

        public void HandleClientRequest(object sender, ConnectRequestArgs a)
        {
            InitAsSlave();
        }

        private void InitAsMaster()
        {
            mPlayerOne = new Player();
            mPlayerOne.Init(mNameToAtlas["pl00"], "pl00", mNameToSequenceset["pl00"]);
            mPlayerOne.mBulletManager = mBulletManager;
            mPlayerOne.SetPosition(400, 200);

        }

        private void InitAsSlave()
        {
            mIsSlaveManager = true;
            mPlayerTwo = new Player();
            mPlayerTwo.Init(mNameToAtlas["pl01"], "pl01", mNameToSequenceset["pl01"]);
            mPlayerTwo.mBulletManager = mBulletManager;
            mPlayerTwo.SetPosition(200, 200);
        }

        public void InitConnectedPlayer()
        {
            if (mIsSlaveManager)
            {
                mPlayerOne = new Player();
                mPlayerOne.Init(mNameToAtlas["pl00"], "pl00", mNameToSequenceset["pl00"]);
                mPlayerOne.mBulletManager = mBulletManager;
                mPlayerOne.SetPosition(400, 200);
                mPlayerOne.MakeGhostObject();
            }
            else
            {
                mPlayerTwo = new Player();
                mPlayerTwo.Init(mNameToAtlas["pl01"], "pl01", mNameToSequenceset["pl01"]);
                mPlayerTwo.mBulletManager = mBulletManager;
                mPlayerTwo.SetPosition(200, 200);
                mPlayerTwo.MakeGhostObject();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (mPlayerOne != null)
                mPlayerOne.Update(gameTime); 
            if(mPlayerTwo != null)
                mPlayerTwo.Update(gameTime);

            CheckPlayerCollisions();
        }
        
        public void CheckPlayerCollisions()
        {
            if (mPlayerOne != null && mBulletManager.CheckCollisionEnemy(mPlayerOne.GetHitBox()))
                mPlayerOne.TakeDamage();

            if (mPlayerTwo != null && mBulletManager.CheckCollisionEnemy(mPlayerTwo.GetHitBox()))
                mPlayerTwo.TakeDamage();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(mPlayerOne != null)
                mPlayerOne.Draw(spriteBatch);
            if(mPlayerTwo != null)
                mPlayerTwo.Draw(spriteBatch);
        }

        public void LoadContent(ContentManager content)
        {
            string[] descriptors = System.IO.Directory.GetFiles("Descriptors/Images/Player/", "*.json", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var descriptor in descriptors)
            {
                string aJson = File.ReadAllText(descriptor);
                PlayerAtlasInfo aInfo = JsonConvert.DeserializeObject<PlayerAtlasInfo>(aJson);
                Texture2D image = content.Load<Texture2D>("Images/Player/" + aInfo.Image);
                SpriteAtlas playerAtlas = new SpriteAtlas();
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
                    
                }
                playerAtlas.SetImage(image);
                playerAtlas.SetClipMap(clipMap, originMap);
                mNameToAtlas.Add(aInfo.Image, playerAtlas);

                Dictionary<string, SpriteSequenceData> sequenceMap = new Dictionary<string, SpriteSequenceData>();
                foreach (var sequence in aInfo.Sequences)
                {
                    SpriteSequenceData s = new SpriteSequenceData();
                    s.mSequence = sequence.Seq;
                    s.mLooping = sequence.Looping;
                    s.mSubLoop = sequence.SubLoop;
                    sequenceMap.Add(sequence.Key, s);
                }
                mNameToSequenceset.Add(aInfo.Image, sequenceMap);
            }
            //mPlayerOne.Init(mNameToAtlas["pl00"], "pl00", mNameToSequenceset["pl00"]);
            //mPlayerTwo.Init(mNameToAtlas["pl01"], "pl01", mNameToSequenceset["pl01"]);

        }

        public void SetBulletManager(BulletManager manager)
        {
            mBulletManager = manager;
        }

        public void SetAsSlave() { mIsSlaveManager = true; }

        public void SetPlayerOnePosition(Vector2 newpos) { mPlayerOne.SetPosition(newpos); }
        public void SetPlayerTwoPosition(Vector2 newpos) { mPlayerTwo.SetPosition(newpos); }
        public Player GetPlayerOne()
        {
            return mPlayerOne;
        }
        public Player GetPlayerTwo()
        {
            return mPlayerTwo;
        }
    }
}
