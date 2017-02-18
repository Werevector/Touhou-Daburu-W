using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;


namespace Touhou_Daburu_W
{
    class EnemyGenerator
    {
        int mAmount;
        float mTime;
        float mInterval;
        EnemyInfo mEnemyInfo;
        int mCurrentAmount;
        float mCurrentInterval;
        bool mRunning;
        bool mFinished;

        List<Vector2> mPathPoints;

        EnemyManager mEnemyManager;
        BulletManager mBulletManager;

        public EnemyGenerator()
        {
            mAmount = 0;
            mTime = 0;
            mInterval = 0;

            mCurrentAmount = 0;
            mCurrentInterval = 0;
            mRunning = false;
            mFinished = false;
            mPathPoints = new List<Vector2>();
        }

        public void Init(GeneratorInfo gInfo, EnemyManager eManager, BulletManager bManager)
        {
            Load(gInfo);
            mEnemyManager = eManager;
            mBulletManager = bManager;
        }

        public void Load(GeneratorInfo gInfo)
        {
            mAmount = gInfo.Amount;
            mTime = gInfo.Time;
            mInterval = gInfo.Interval;
            mEnemyInfo = gInfo.Enemy;

            List<Vector2> controlPoints = new List<Vector2>();
            for (int i = 0; i < gInfo.PathControlPoints.Length; i++)
            {
                controlPoints.Add(new Vector2(gInfo.PathControlPoints[i][0], gInfo.PathControlPoints[i][1]));
            }
            mPathPoints = Utility.CreateSplinePoints(controlPoints, 20);

            mCurrentInterval = mInterval;
        }

        public void Update(GameTime gameTime)
        {
            if (!mFinished && mRunning)
            {
                mCurrentInterval += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (mCurrentInterval > mInterval && mCurrentAmount < mAmount)
                {
                    Generate();
                    mCurrentInterval = 0;
                    if (CheckFinished()) { Stop(); }
                }
            }
        }

        private void Generate()
        {
            Enemy enemy = new Enemy(mEnemyInfo.SpriteType);
            enemy.mHitBox = new Rectangle(0, 0, mEnemyInfo.HitBox[0], mEnemyInfo.HitBox[1]);
            enemy.mPosition = new Vector2(mEnemyInfo.Position[0], mEnemyInfo.Position[1]);
            enemy.mMoveSpeed = mEnemyInfo.MoveSpeed;
            enemy.SetPath(mPathPoints);
            foreach (var pInfo in mEnemyInfo.FirePatterns)
            {
                enemy.AddPattern(GeneratePatternFromStruct(pInfo));
            }

            mEnemyManager.SpawnEnemy(enemy);
            mCurrentAmount++;
        }

        public Pattern GeneratePatternFromStruct(PatternInfo pInfo)
        {
            Pattern pattern = new Pattern();
            pattern.mAcceleration = pInfo.Acceleration;
            pattern.mSpeed = pInfo.Speed;

            pattern.mArrayNumber = pInfo.ArrayNumber;
            pattern.mBulletsPerArray = pInfo.BulletsPerArray;

            pattern.mPatternAngle = pInfo.PatternAngle;
            pattern.mFireRate = pInfo.FireRate;
            pattern.mHitBox = 
                new Rectangle(0, 0, pInfo.HitBox[0], pInfo.HitBox[1]);

            pattern.mInternalArraySpread = pInfo.InternalArraySpread;
            pattern.mArraySpread = pInfo.ArraySpread;

            pattern.mRotationSpeed = pInfo.RotationSpeed;
            pattern.mRotationSpeedDelta = pInfo.RotationSpeedDelta;
            
            pattern.mBoundedRotation = pInfo.BoundedRotation;
            pattern.mMaxRotation = pInfo.MaxRotation;
            pattern.mMinRotation = pInfo.MinRotation;
            pattern.mFlipRotation = pInfo.FlipRotation;

            pattern.SetBulletManager(mBulletManager);

            pattern.mBulletType = pInfo.SpriteType;
            pattern.mBulletColor = pInfo.ColorIndex;
            pattern.mDirectional = pInfo.Directional;

            return pattern;
        }

        public void Start()
        {
            mRunning = true;
        }
        private void Stop()
        {
            mRunning = false;
            mFinished = true;
        }
        private bool CheckFinished()
        {
            return mCurrentAmount == mAmount;
        }
        public bool IsFinished() { return mFinished; }
        public float GetActivationTime() { return mTime; }
    }
}
