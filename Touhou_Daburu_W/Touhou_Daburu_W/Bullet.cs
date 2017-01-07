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
    class Bullet
    {

        public TextureAtlas mBulletAtlas;
        private Texture2D mErrorTexture;

        public string mBulletType;
        public int mBulletColor;

        public Vector2 mPosition;
        public Vector2 mVelocity;
        public Vector2 mAcceleration;

        public Rectangle mHitBox;

        public bool mDirectional;
        public bool mAllied;

        private float mAngle;
        private float mTextureAlpha;

        public Bullet()
        {
            mBulletAtlas = null;
            mBulletType = "";
            mBulletColor = 0;

            mPosition = new Vector2(0, 0);
            mVelocity = new Vector2(0, 0);
            mAcceleration = new Vector2(0, 0);

            mHitBox = new Rectangle(0, 0, 2, 2);
            mDirectional = false;
            mAngle = 0;
            mAllied = false;

            mTextureAlpha = mAllied ? 0.5f : 1.0f;
        }

        public void Init(TextureAtlas atlas, string type, int color)
        {
            mBulletAtlas = atlas;
            mBulletType = type;
            mBulletColor = color;
        }

        public void Update(GameTime gameTime)
        {
            double delta = gameTime.ElapsedGameTime.TotalSeconds;

            mVelocity.X += mAcceleration.X;
            mVelocity.Y += mAcceleration.Y;

            mPosition.X += mVelocity.X * (float)delta;
            mPosition.Y += mVelocity.Y * (float)delta;

            mHitBox.X = (int)mPosition.X - mHitBox.Width / 2;
            mHitBox.Y = (int)mPosition.Y - mHitBox.Height / 2;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (mDirectional)
                mAngle = (float)Math.Atan2(mVelocity.Y, mVelocity.X);

            if (mBulletAtlas != null)
                mBulletAtlas.Draw(spriteBatch, mBulletType, mBulletColor, mPosition, mAngle, 1.0f, SpriteEffects.None, 1.0f, mTextureAlpha);
        }

    }
}
