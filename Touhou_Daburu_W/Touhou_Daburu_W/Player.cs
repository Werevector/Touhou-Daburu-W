using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Touhou_Daburu_W
{
    class Player
    {
        public Vector2 mPosition;
        private Vector2 mVelocity;
        private Rectangle mHitBox;
        private float mMovementSpeed;
        private float mMovementSpeedFocused;
        private int mLives;
        private int mBombs;
        private int mPowerLevel;
        private string mSpriteName;
        private SpriteAtlas mAtlas;

        private double mTick;
        private double mRespawnTimer;
        private double mInvulnTimer;
        private double mRespawnTime;
        private double mInvulnTime;

        SpriteAnimationManager mAnimationManager;

        public BulletManager mBulletManager;

        public bool mFiring;
        public bool mFocusing;
        public bool mBombing;
        public bool mDead;
        public bool mDamaged;
        public bool mInvuln;
        public bool mLeft;
        public bool mRight;
        private bool mIsGhostObject;

        public Player()
        {
            mPosition = new Vector2();
            mVelocity = new Vector2();
            mHitBox = new Rectangle(0, 0, 4, 4);
            mLives = 3;
            mBombs = 3;
            mPowerLevel = 1;
            mMovementSpeed = 200;
            mSpriteName = "pl00";
            mAtlas = null;
            mFiring = false;
            mDead = false;
            mFocusing = false;
            mBombing = false;
            mIsGhostObject = false;
            mRespawnTime = 2;
            mInvulnTime = 2;
        }

        public void Init(SpriteAtlas atlas, string spritename, Dictionary<string, SpriteSequenceData> sequences)
        {
            mAtlas = atlas;
            mSpriteName = spritename;
            mRespawnTime = 2;
            mInvulnTime = 2;
            mDead = false;
            mAnimationManager = new SpriteAnimationManager(sequences);
        }

        public void Update(GameTime gameTime)
        {
            mTick += gameTime.ElapsedGameTime.TotalSeconds;

            if (!mDamaged)
                UpdatePlaying(gameTime);
            else
                UpdateRespawning(gameTime);
            
        }

        private void UpdateRespawning(GameTime gameTime)
        {
            mRespawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(!mDead && mRespawnTimer > mRespawnTime)
            {
                Respawn();
            }
        }

        private void UpdatePlaying(GameTime gameTime)
        {
            if (mInvuln)
            {
                mInvulnTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (mInvulnTimer > mInvulnTime)
                    mInvuln = false;
                    
            }
            mVelocity.X = 0; mVelocity.Y = 0;
            if (!mIsGhostObject)
            {
                ResetControlFlags();
                CheckControls();
            }
            UpdateMovement(gameTime);
            StateCheck(gameTime);
            UpdateAnimation(gameTime);
        }

        private void CheckControls()
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Left))
                mVelocity.X -= 1;
            if (keyboard.IsKeyDown(Keys.Right))
                mVelocity.X += 1;
            if (keyboard.IsKeyDown(Keys.Up))
                mVelocity.Y -= 1;
            if (keyboard.IsKeyDown(Keys.Down))
                mVelocity.Y += 1;

            mFiring = keyboard.IsKeyDown(Keys.Z);
        }

        private void Fire()
        {
            int gap = 10;
            Vector2 vel = new Vector2(0, -2000);
            Vector2 acc = new Vector2();
            Rectangle hitBox = new Rectangle(0, 0, 30, 40);
            for (int i = 0; i < 2; i++)
            {
                Vector2 pos = new Vector2(mPosition.X - gap + (gap*2*i), mPosition.Y);
                if(mBulletManager != null)
                    mBulletManager.SpawnPlayerBullet(mAtlas, "Shot", pos, vel, acc, hitBox, true);
            }
        }

        public void TakeDamage()
        {
            mLives--;
            if (mLives <= 0)
                mDead = true;
            mDamaged = true;
            SetPosition(999, 999);
        }

        public void OnCollide()
        {
            if(!mDamaged && !mInvuln)
            {

            }
        }

        private void Respawn()
        {
            SetPosition(300, 300);
            mDamaged = false;
            mRespawnTimer = 0.0;
            mInvuln = true;
            mInvulnTimer = 0.0;
        }

        private void ResetControlFlags()
        {
            //mShooting = false;
            mFocusing = false;
            mLeft = false;
            mRight = false;
        }

        private void StateCheck(GameTime gameTime)
        {
            if(mVelocity.X < 0)
            {
                mLeft = true;
            }
            if(mVelocity.X > 0)
            {
                mRight = true;
            }
            if (mRight && mLeft)
            {
                mRight = false;
                mLeft = false;
            }
            if (mFiring)
            {
                double t = 1 / (double)30;
                if (mTick > t)
                {
                    Fire();
                    mTick = 0.0;
                }
            }    
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            if(!mLeft && !mRight)
            {
                mAnimationManager.ChangeSequence("Idle");
            }else
            {
                if (mLeft)
                    mAnimationManager.ChangeSequence("MoveLeft");
                if (mRight)
                    mAnimationManager.ChangeSequence("MoveRight");
            }
            mAnimationManager.Update(gameTime);
            
        }

        private void UpdateMovement(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (mVelocity.X != 0 && mVelocity.Y != 0)
            {
                mVelocity.Normalize();
            }
            mPosition += (mVelocity * mMovementSpeed * delta);
            UpdateHitBox();

        }
        private void UpdateHitBox()
        {
            mHitBox.X = (int)mPosition.X - mHitBox.Width / 2;
            mHitBox.Y = (int)mPosition.Y - mHitBox.Height / 2;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!mDamaged)
            {
                mAtlas.Draw(spriteBatch, mSpriteName, mAnimationManager.GetCurrentSequenceKey(), mPosition, mInvuln ? 0.3f : 1.0f);
            }   
        }

        public void SetMoveSpeed(float speed)
        {
            mMovementSpeed = speed;
        }
        public void SetPosition(float x, float y)
        {
            mPosition.X = x; mPosition.Y = y;
            mHitBox.X = (int)x; mHitBox.Y = (int)y;
        }
        public void SetPosition(Vector2 pos)
        {
            mPosition = pos;
        }
        public void MakeGhostObject()
        {
            mIsGhostObject = true;
        }
        public bool IsFiring()
        {
            return mFiring;
        }
        public void SetFiringState(bool state) { mFiring = state; }
        public Rectangle GetHitBox()
        {
            return mHitBox;
        }

    }
}
