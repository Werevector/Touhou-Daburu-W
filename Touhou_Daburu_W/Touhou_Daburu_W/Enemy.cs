using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Touhou_Daburu
{
    class Enemy
    {
        TextureAtlas mEnemyAtlas;
        List<Pattern> mFirePatterns;

        private string mSpriteName;

        public Rectangle mHitBox;

        public Vector2 mPosition;
        public Vector2 mVelocity;
        public Vector2 mAcceleration;
        public float mMoveSpeed;

        public List<Vector2> mPathPoints;

        public int mHealth;

        private int pathIndex;

        private int mCurrentTick;
        private int mSequenceIndex;

        public Dictionary<string, SpriteSequence> mSequenceMap;
        public SpriteSequence mCurrentSequence;

        private Texture2D debugTexture;

        public Enemy()
        {
            mPosition = new Vector2(0, 0);
            mVelocity = new Vector2(0, 0);
            mAcceleration = new Vector2(0, 0);
            mMoveSpeed = 0f;

            mFirePatterns = new List<Pattern>();
            mHitBox = new Rectangle(0, 0, 2, 2);

            mPathPoints = new List<Vector2>();

            mHealth = 10;
            pathIndex = 0;

            mCurrentTick = 0;
            mSequenceIndex = 0;

        }

        public Enemy(TextureAtlas atlas, Vector2 pos, Vector2 vel, Vector2 acc, Rectangle hitbox = new Rectangle())
        {
            mEnemyAtlas = atlas;
            mPosition = pos;
            mVelocity = vel;
            mAcceleration = acc;
            mHitBox = hitbox;
        }

        public void Update(GameTime gameTime)
        {
            UpdateAnimation();

            mVelocity.X += mAcceleration.X;
            mVelocity.Y += mAcceleration.Y;

            mPosition.X += mVelocity.X;
            mPosition.Y += mVelocity.Y;

            mHitBox.X = (int)mPosition.X - mHitBox.Width / 2;
            mHitBox.Y = (int)mPosition.Y - mHitBox.Height / 2;

            if (mPathPoints.Count != 0 && pathIndex != mPathPoints.Count-1)
            {
                if ((mPosition - mPathPoints[pathIndex]).Length() < 5)
                {
                    pathIndex++;
                }

                double angle = Math.Atan2(mPathPoints[pathIndex].Y - mPosition.Y, mPathPoints[pathIndex].X - mPosition.X);
                Vector2 pathVel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                mVelocity = pathVel * mMoveSpeed;
                
            }

            foreach (var pattern in mFirePatterns)
            {
                pattern.Update(gameTime, this);
            }
            
        }

        public void UpdateAnimation()
        {
            mCurrentTick++;
            if (mCurrentTick == mCurrentSequence.mUpdateDelay)
            {
                mCurrentTick = 0;
                mSequenceIndex++;

                if (mSequenceIndex == mCurrentSequence.mSequence.Count)
                {
                    if (mCurrentSequence.Looping)
                        mSequenceIndex = mCurrentSequence.mSubLoop;
                    else
                        mSequenceIndex = mCurrentSequence.mSequence.Count - 1;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle r = new Rectangle(0, 0, 5, 5);
            for (int i = 0; i < mPathPoints.Count - 2; i++)
            {
                Utility.DrawLine(sb, debugTexture, mPathPoints[i], mPathPoints[i + 1], Color.Gold);
                //Utility.DrawRectangle(sb, debugTexture, mPathPoints[i], r, Color.Yellow);
            }
            mEnemyAtlas.Draw(sb, mSpriteName, mCurrentSequence.GetKeyAt(mSequenceIndex), mPosition);
        }

        public Vector2 GetPosition() { return mPosition; }
        public Rectangle GetHitbox() { return mHitBox; }
        public void SetTextureAtlas(TextureAtlas atlas) { mEnemyAtlas = atlas; debugTexture = mEnemyAtlas.GenerateDebugTexture(Color.White); }
        public void AddPattern(Pattern p) { mFirePatterns.Add(p); }
        public void SetSpriteName(string type) { mSpriteName = type; }


        public void SetSequenceMap(Dictionary<string, SpriteSequence> map)
        {
            mSequenceMap = map;
            mCurrentSequence = mSequenceMap["Idle"];
        }
    }
}
