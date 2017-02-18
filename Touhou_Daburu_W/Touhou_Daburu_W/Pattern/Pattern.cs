using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Touhou_Daburu_W
{
    class Pattern
    {
        public int mArrayNumber;
        public int mBulletsPerArray;
        public float mInternalArraySpread;
        public float mArraySpread;
        public double mPatternAngle;

        public float mSpeed;
        public float mAcceleration;

        public double mRotation;
        public double mRotationSpeed;
        public double mRotationSpeedDelta;
        public double mMaxRotationSpeed;

        public bool mBoundedRotation;
        public double mMaxRotation;
        public double mMinRotation;
        public bool mFlipRotation;

        public int mFireRate;
        public double mTick;

        public string mBulletType;
        public int mBulletColor;
        public bool mDirectional;

        public Rectangle mHitBox;

        private BulletManager mBulletManager;

        public Pattern()
        {
            mArrayNumber = 1;
            mBulletsPerArray = 1;
            mInternalArraySpread = 0;
            mArraySpread = 0;

            mPatternAngle = 0;

            mRotation = 0;
            mRotationSpeed = 0;
            mRotationSpeedDelta = 0;
            mMaxRotationSpeed = 0;

            mBoundedRotation = false;
            mMaxRotation = 360;
            mMinRotation = 0;
            mFlipRotation = false;

            mFireRate = 5;
            mHitBox = new Rectangle(0, 0, 2, 2);

            mDirectional = false;
            mAcceleration = 0.0f;

            mTick = 0.0;
        }

        public void Update(GameTime gameTime, Enemy owner)
        {
            mTick += gameTime.ElapsedGameTime.TotalSeconds;

            mRotationSpeed += mRotationSpeedDelta;
            mRotation += mRotationSpeed;

            if (mBoundedRotation)
            {
                if (mRotation > mMaxRotation)
                {
                    mRotation = mMaxRotation;
                    if (mFlipRotation) { mRotationSpeed = -mRotationSpeed; }
                }
                if (mRotation < mMinRotation)
                {
                    mRotation = mMinRotation;
                    if (mFlipRotation) { mRotationSpeed = -mRotationSpeed; }
                }
            }

            double t = 1 / (double)mFireRate;
            if (mTick > t)
            {
                Fire(owner);
                mTick = 0.0;
            }
        }

        public void Fire(Enemy owner)
        {
            for (int i = 0; i < mArrayNumber; i++)
            {
                for (int j = 0; j < mBulletsPerArray; j++)
                {
                    double angle = mPatternAngle + mRotation + j * mInternalArraySpread + i * mArraySpread;
                    angle = angle * Math.PI / 180;

                    Vector2 vel = new Vector2();
                    vel.X = (float)Math.Cos(angle);
                    vel.Y = (float)Math.Sin(angle);

                    Vector2 acc = new Vector2();
                    acc.X = vel.X * mAcceleration;
                    acc.Y = vel.Y * mAcceleration;

                    vel.X *= mSpeed;
                    vel.Y *= mSpeed;
                    mBulletManager.SpawnEnemyBullet(mBulletType, mBulletColor, owner.GetPosition(), vel, acc, mHitBox, mDirectional);
                }
            }
        }

        public void SetBulletManager(BulletManager spawner) { mBulletManager = spawner; }

    }
}
