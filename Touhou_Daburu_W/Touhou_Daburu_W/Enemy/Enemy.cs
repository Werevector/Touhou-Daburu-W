using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Touhou_Daburu_W
{
    class Enemy
    {
        SpriteAtlas mEnemyAtlas;
        private string mSpriteName;
        private Texture2D debugTexture;
        private List<Pattern> mFirePatterns;
        public Rectangle mHitBox;
        public Vector2 mPosition;
        public Vector2 mVelocity;
        public Vector2 mAcceleration;
        private PathManager mPathManager;
        private SpriteAnimationManager mAnimationManager;
        public float mMoveSpeed;
        private int mHealth;

        enum MoveState
        {
            IDLE,
            LEFT,
            RIGHT
        }
        private MoveState mMoveState;

        public Enemy(SpriteAtlas atlas, string name)
        {
            mEnemyAtlas = atlas;
            mSpriteName = name;
            mHitBox = new Rectangle(0,0,50,50);

            mPosition = new Vector2(0, 0);
            mVelocity = new Vector2(0, 0);
            mAcceleration = new Vector2(0, 0);
            mMoveSpeed = 3f;

            mFirePatterns = new List<Pattern>();

            mPathManager = new PathManager();
            mHealth = 10;
            mMoveState = MoveState.IDLE;
        }

        public Enemy(string name)
        {
            mSpriteName = name;
            mHitBox = new Rectangle(0, 0, 50, 50);

            mPosition = new Vector2(0, 0);
            mVelocity = new Vector2(0, 0);
            mAcceleration = new Vector2(0, 0);
            mMoveSpeed = 3f;

            mFirePatterns = new List<Pattern>();

            mPathManager = new PathManager();
            mHealth = 10;
            mMoveState = MoveState.IDLE;
        }

        public void Draw(SpriteBatch sb)
        {
            SpriteEffects effect = mMoveState == MoveState.RIGHT ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            mEnemyAtlas.Draw(sb, mSpriteName, mAnimationManager.GetCurrentSequenceKey(), mPosition, effect);
        }

        public void Update(GameTime gameTime)
        {
            mPathManager.Update(mPosition);
            UpdateMovement(gameTime);
            CheckMoveState();
            UpdateAnimation(gameTime);
            UpdateFirePatterns(gameTime);
        }

        public void TakeDamage(int amount)
        {
            mHealth -= amount;
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            switch (mMoveState)
            {
                case MoveState.IDLE:
                    mAnimationManager.ChangeSequence("Idle");
                    break;
                case MoveState.LEFT:
                    mAnimationManager.ChangeSequence("SideMove");
                    break;
                case MoveState.RIGHT:
                    mAnimationManager.ChangeSequence("SideMove");
                    break;
                default:
                    break;
            }
            mAnimationManager.Update(gameTime);
        }

        private void CheckMoveState()
        {
            if (mVelocity.X < 0)
            {
                mMoveState = MoveState.LEFT;
            }
            if (mVelocity.X > 0)
            {
                mMoveState = MoveState.RIGHT;
            }
        }

        private void UpdateMovement(GameTime gameTime)
        {
            double delta = gameTime.ElapsedGameTime.TotalSeconds;
            mVelocity.X = 0; mVelocity.Y = 0;
            mVelocity = mPathManager.GetUnitVector();
            mVelocity *= mMoveSpeed;
            
            //mVelocity.X += mAcceleration.X;
            //mVelocity.Y += mAcceleration.Y;

            mPosition.X += mVelocity.X * (float)delta;
            mPosition.Y += mVelocity.Y * (float)delta;

            mHitBox.X = (int)mPosition.X - mHitBox.Width / 2;
            mHitBox.Y = (int)mPosition.Y - mHitBox.Height / 2;
        }

        private void UpdateFirePatterns(GameTime gameTime)
        {
            foreach (var pattern in mFirePatterns)
            {
                pattern.Update(gameTime, this);
            }
        }

        public string GetSpriteName()
        {
            return mSpriteName;
        }
        public Vector2 GetPosition()
        {
            return mPosition;
        }
        public Rectangle GetHitbox()
        {
            return mHitBox;
        }
        public int GetHealth()
        {
            return mHealth;
        }
        public void SetAtlas(SpriteAtlas atlas)
        {
            mEnemyAtlas = atlas;
        }
        public void SetAnimationSequences( Dictionary<string, SpriteSequenceData> sequences)
        {
            mAnimationManager = new SpriteAnimationManager(sequences);
        }
        public void SetPosition(Vector2 position)
        {
            mPosition = position;
        }
        public void SetVelocity(Vector2 velocity)
        {
            mVelocity = velocity;
        }
        public void SetAcceleration(Vector2 acceleration)
        {
            mAcceleration = acceleration;
        }
        public void SetPatterns(List<Pattern> patterns)
        {
            mFirePatterns = patterns;
        }
        public void AddPattern(Pattern p)
        {
            mFirePatterns.Add(p);
        }
        public void SetPath(List<Vector2> points)
        {
            mPathManager.SetPathPoints(points);
        }
    }
}
