using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BulletEngineNeo.Graphics;
namespace BulletEngineNeo
{
    class Enemy
    {
        SpriteAtlas mAtlas;
        SpriteAnimationManager mAnimationManager;

        string mSpriteName;
        Vector2 mPosition;

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

        public Enemy()
        {
            mSpriteName = "RedFairy";
        }

        public void Initialize(SpriteAtlas a, Dictionary<string, SpriteSequenceData> sequences)
        {
            mAtlas = a;
            mAnimationManager = new SpriteAnimationManager(sequences);
        }

        public void Destroy()
        {

        }
        
        public void Update(GameTime gameTime)
        {
            mAnimationManager?.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            mAtlas?.Draw(sb, mSpriteName, mAnimationManager.GetCurrentSequenceKey(), mPosition);
        }
    }
}
