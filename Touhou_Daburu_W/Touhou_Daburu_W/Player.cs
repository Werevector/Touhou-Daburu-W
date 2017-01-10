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
        private float mMovementSpeed;
        private float mMovementSpeedFocused;
        private int mLives;
        private int mBombs;
        private int mPowerLevel;
        private string mSpriteName;
        private SpriteAtlas mAtlas;
        SpriteAnimationManager mAnimationManager;

        public bool mShooting;
        public bool mFocusing;
        public bool mBombing;
        public bool mDead;
        public bool mLeft;
        public bool mRight;
        private bool mComputer;

        public Player()
        {
            mPosition = new Vector2();
            mVelocity = new Vector2();
            mLives = 3;
            mBombs = 3;
            mPowerLevel = 1;
            mMovementSpeed = 200;
            mSpriteName = "pl00";
            mAtlas = null;
            mShooting = false;
            mFocusing = false;
            mBombing = false;
            mComputer = false;
        }

        public void Init(SpriteAtlas atlas, string spritename, Dictionary<string, SpriteSequenceData> sequences)
        {
            mAtlas = atlas;
            mSpriteName = spritename;
            mAnimationManager = new SpriteAnimationManager(sequences);
        }

        public void Update(GameTime gameTime)
        {
            mVelocity.X = 0; mVelocity.Y = 0;
            if (!mComputer)
            {
                ResetControlFLags();
                CheckControls();
            }
            UpdateMovement(gameTime);
            StateCheck();
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
        }

        private void ResetControlFLags()
        {
            mShooting = false;
            mFocusing = false;
            mLeft = false;
            mRight = false;
        }

        private void StateCheck()
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mAtlas.Draw(spriteBatch, mSpriteName, mAnimationManager.GetCurrentSequenceKey(), mPosition);
        }

        public void SetMoveSpeed(float speed)
        {
            mMovementSpeed = speed;
        }
        public void SetPosition(float x, float y)
        {
            mPosition.X = x; mPosition.Y = y;
        }
        public void SetPosition(Vector2 pos)
        {
            mPosition = pos;
        }
        public void SetComputerControlled(bool controlled)
        {
            mComputer = controlled;
        }

    }
}
