using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BulletEngineNeo.ScriptEffects;
using BulletEngineNeo.Graphics;
using MoonSharp.Interpreter;

namespace BulletEngineNeo.Bullet
{
    /// <summary>
    /// Basic bullet class, that utilizes composition to define behaviour
    /// </summary>
    public class GameBullet
    {
        [MoonSharpHidden]
        private int ID;
        [MoonSharpHidden]
        private SpriteAtlas mAtlas;

        public string mSpriteName;
        public int mBulletColor;
        public Vec2 mPosition;
        public Vec2 mVelocity;
        public Vec2 mAcceleration;

        [MoonSharpHidden]
        private List<ObjectEffect> mOnCollisionEffects;
        [MoonSharpHidden]
        private List<ObjectEffect> mOnGameAreaExitEffects;
        [MoonSharpHidden]
        private List<Effect> mOnUpdateEffects;
        [MoonSharpHidden]
        private List<Effect> mOnConditionEffects;
        [MoonSharpHidden]
        private List<Effect> mOnSpawnEffects;

        public int GetId() { return ID; }

        [MoonSharpHidden]
        public GameBullet()
        {
            mSpriteName = "Small1";
            mOnCollisionEffects = new List<Effect>();
            mOnUpdateEffects = new List<Effect>();
            mOnConditionEffects = new List<Effect>();
            mOnGameAreaExitEffects = new List<Effect>();
            mOnSpawnEffects = new List<Effect>();
            mVelocity = new Vec2();
            mPosition = new Vec2();
            mAcceleration = new Vec2();
        }

        [MoonSharpHidden]
        public void Initialize(SpriteAtlas a, int id)
        {
            mAtlas = a;
            ID = id;
        }

        public void Destroy(MainGameLogic game)
        {
            game.DestroyBullet(ID);
        }

        [MoonSharpHidden]
        public void Update(GameTime gameTime, MainGameLogic game)
        {
            if (mOnUpdateEffects.Count != 0 || mOnConditionEffects.Count != 0)
            {
                DynValue bul = UserData.Create(this);
                foreach (var effect in mOnUpdateEffects)
                    effect.Force(gameTime, game, bul);
                foreach (var effect in mOnConditionEffects)
                    effect.Check(gameTime, game, bul);
            } 

            mVelocity.x += mAcceleration.x;
            mVelocity.y += mAcceleration.y;
            mPosition.x += mVelocity.x * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mPosition.y += mVelocity.y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(Utility.IsOutsideRect(game.mBulletArea, mPosition.ToMono()))
            {
                OnBulletAreaExit(gameTime, game);
            }

            if(Utility.IsOutsideRect(game.Window.ClientBounds, mPosition.ToMono()))
            {
                OnGameAreaExit(gameTime, game);
            }
        }

        [MoonSharpHidden]
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float angle = (float)Math.Atan2(mVelocity.y, mVelocity.x);
            mAtlas.Draw(spriteBatch, mSpriteName, mBulletColor, mPosition.ToMono(), angle, 1.0f, SpriteEffects.None, 1.0f, 1.0f);
        }

        [MoonSharpHidden]
        public void OnCollideEnter(GameTime gameTime, MainGameLogic game)
        {
            DynValue bul = UserData.Create(this);
            foreach (var effect in mOnCollisionEffects)
                effect.Force(gameTime, game, bul);
        }

        [MoonSharpHidden]
        public void OnBulletAreaExit(GameTime gameTime, MainGameLogic game)
        {
            Destroy(game);
        }

        [MoonSharpHidden]
        public void OnGameAreaExit(GameTime gameTime, MainGameLogic game)
        {
            DynValue bul = UserData.Create(this);
            foreach (var effect in mOnGameAreaExitEffects)
                effect.Force(gameTime, game, bul);
        }

        [MoonSharpHidden]
        public void OnSpawn(GameTime gameTime,MainGameLogic game)
        {
            DynValue bul = UserData.Create(this);
            foreach (var effect in mOnSpawnEffects)
                effect.Force(gameTime, game, bul);
        }

        [MoonSharpHidden]
        public void AddOnUpdateEffect(Effect e)
        {
            mOnUpdateEffects.Add(e);
        }

        [MoonSharpHidden]
        public void AddOnCollisionEffect(Effect e)
        {
            mOnCollisionEffects.Add(e);
        }

        [MoonSharpHidden]
        public void AddOnConditionEffect(Effect e)
        {
            mOnConditionEffects.Add(e);
        }

        [MoonSharpHidden]
        public void AddOnGameAreaExitEffect(Effect e)
        {
            mOnGameAreaExitEffects.Add(e);
        }

        [MoonSharpHidden]
        public void AddOnSpawnEffect(Effect e)
        {
            mOnSpawnEffects.Add(e);
        }

        
    }
}
