using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;


namespace Touhou_Daburu_W
{
    class StageManager
    {
        List<EnemyGenerator> mGenerators;
        List<int> mDestroyGeneratorQueue;
        EnemyManager mEnemyManager;
        BulletManager mBulletManager;
        string[] mStageDescriptions;

        double mStageTime;
        int mCurrentStage;

        public StageManager()
        {
            mStageTime = 0;
            mCurrentStage = 0;
        }

        public void HandlePlayerConnected(object sender)
        {
            Start();
        }

        public void Init(EnemyManager eManager, BulletManager bManager)
        {
            mGenerators = new List<EnemyGenerator>();
            mDestroyGeneratorQueue = new List<int>();
            SetEnemyManager(eManager);
            SetBulletManager(bManager);
            ReadStages();
        }

        public void Update(GameTime gameTime)
        {
            mStageTime += gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < mGenerators.Count; i++)
            {
                if ((float)mStageTime >= mGenerators[i].GetActivationTime())
                {
                    mGenerators[i].Start();
                }

                mGenerators[i].Update(gameTime);

                if (mGenerators[i].IsFinished())
                {
                    mDestroyGeneratorQueue.Add(i);
                }
            }

            ProccessDestroyQueue();
        }

        public void Start()
        {
            mCurrentStage = 0;
            LoadStage(mCurrentStage);
        }

        private void ProccessDestroyQueue()
        {
            mDestroyGeneratorQueue.Sort();
            mDestroyGeneratorQueue.Reverse();
            foreach (var index in mDestroyGeneratorQueue)
            {
                mGenerators.RemoveAt(index);
            }
            mDestroyGeneratorQueue.Clear();
        }

        private void ReadStages()
        {
              mStageDescriptions = 
                System.IO.Directory.GetFiles("Stages/", "*.json", System.IO.SearchOption.TopDirectoryOnly);
        }

        private void LoadStage(int index)
        {
            if(index < mStageDescriptions.Length)
            {
                mGenerators.Clear();
                mDestroyGeneratorQueue.Clear();
                mEnemyManager.PrepareForNextStage();

                string sJson = File.ReadAllText(mStageDescriptions[index]);
                StageInfo sInfo = JsonConvert.DeserializeObject<StageInfo>(sJson);
                foreach (var generatorInfo in sInfo.Generators)
                {
                    EnemyGenerator generator = new EnemyGenerator();
                    generator.Init(generatorInfo, mEnemyManager, mBulletManager);
                    mGenerators.Add(generator);
                }
            }
        }

        private void LoadNextStage()
        {
            if(mCurrentStage < mStageDescriptions.Length-1)
            {
                mCurrentStage++;
                LoadStage(mCurrentStage);
            }
        }

        public void SetEnemyManager(EnemyManager manager)
        {
            mEnemyManager = manager;
        }
        public void SetBulletManager(BulletManager manager)
        {
            mBulletManager = manager;
        }
    }
}
