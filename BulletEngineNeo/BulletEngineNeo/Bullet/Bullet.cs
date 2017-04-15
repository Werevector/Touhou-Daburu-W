using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BulletEngineNeo.Graphics;

namespace BulletEngineNeo.Bullet
{
    /// <summary>
    /// Basic bullet class, that utilizes composition to define behaviour
    /// </summary>
    class GameBullet
    {
        private SpriteAtlas mAtlas;

        string mSpriteName;
        int mBulletColor;
        Vector2 mPosition;
        Vector2 mVelocity;
        Vector2 mAcceleration;

        List<BulletEffect> mOnCollisionEffects;
        List<BulletEffect> mOnUpdateEffects;
        List<BulletEffect> mOnConditionEffects;

        public string SpriteName
        {
            get
            {
                return mSpriteName;
            }

            set
            {
                mSpriteName = value;
            }
        }
        public int BulletColor
        {
            get
            {
                return mBulletColor;
            }

            set
            {
                mBulletColor = value;
            }
        }
        public Vector2 Position
        {
            get
            {
                return mPosition;
            }

            set
            {
                mPosition = value;
            }
        }
        public Vector2 Velocity
        {
            get
            {
                return mVelocity;
            }

            set
            {
                mVelocity = value;
            }
        }
        public Vector2 Acceleration
        {
            get
            {
                return mAcceleration;
            }

            set
            {
                mAcceleration = value;
            }
        }

        public GameBullet()
        {
            SpriteName = "Small1";
            mOnCollisionEffects = new List<BulletEffect>();
            mOnUpdateEffects = new List<BulletEffect>();
            mOnConditionEffects = new List<BulletEffect>();
        }

        public void Initialize(SpriteAtlas a)
        {
            mAtlas = a;
        }

        public void Destroy()
        {

        }

        public void Update(GameTime gameTime, MainGameLogic game)
        {
            foreach (var effect in mOnUpdateEffects)
                effect.Force(gameTime, game, this);

            foreach (var effect in mOnConditionEffects)
                effect.Check(gameTime, game, this);

            Velocity += Acceleration;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float angle = (float)Math.Atan2(mVelocity.Y, mVelocity.X);
            mAtlas.Draw(spriteBatch, mSpriteName, mBulletColor, mPosition, angle, 1.0f, SpriteEffects.None, 1.0f, 1.0f);
        }

        public void OnCollideEnter(GameTime gameTime, MainGameLogic game)
        {
            foreach (var effect in mOnCollisionEffects)
                effect.Force(gameTime, game, this);
        }

        public void AddOnUpdateEffect(BulletEffect e)
        {
            mOnUpdateEffects.Add(e);
        }

        public void AddOnCollisionEffect(BulletEffect e)
        {
            mOnCollisionEffects.Add(e);
        }

        public void AddOnConditionEffect(BulletEffect e)
        {
            mOnConditionEffects.Add(e);
        }

        
    }
}
