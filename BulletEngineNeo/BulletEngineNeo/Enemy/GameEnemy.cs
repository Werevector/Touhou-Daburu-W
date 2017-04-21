using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using System.IO;
using BulletEngineNeo.Graphics;
using BulletEngineNeo.ScriptEffects;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using MoonSharp.Interpreter.Serialization;



namespace BulletEngineNeo.Enemy
{
    class GameEnemy
    {
        private int ID;

        public string SpriteName;

        public Vector2 Position;
        public Vector2 Velocity;

        SpriteAtlas Atlas;
        SpriteAnimationManager AnimationManager;

        public float MovementSpeed;

        [MoonSharpHidden]
        private List<ObjectEffect> mOnCollisionEffects;
        [MoonSharpHidden]
        private List<ObjectEffect> mOnGameAreaExitEffects;
        [MoonSharpHidden]
        private List<ObjectEffect> mOnUpdateEffects;
        [MoonSharpHidden]
        private List<ObjectEffect> mOnConditionEffects;
        [MoonSharpHidden]
        private List<ObjectEffect> mOnSpawnEffects;

        public GameEnemy()
        {

        }

        public void Initialize(SpriteAtlas a, int id)
        {
            Atlas = a;
            ID = id;
        }

        public void Destroy(MainGameLogic game)
        {
            
        }

        [MoonSharpHidden]
        public void Update(GameTime gameTime, MainGameLogic game)
        {
            AnimationManager?.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Atlas?.Draw(spriteBatch, SpriteName, AnimationManager.GetCurrentSequenceKey(), Position);
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
        public void OnSpawn(GameTime gameTime, MainGameLogic game)
        {
            DynValue bul = UserData.Create(this);
            foreach (var effect in mOnSpawnEffects)
                effect.Force(gameTime, game, bul);
        }

        [MoonSharpHidden]
        public void AddOnUpdateEffect(ObjectEffect e)
        {
            mOnUpdateEffects.Add(e);
        }

        [MoonSharpHidden]
        public void AddOnCollisionEffect(ObjectEffect e)
        {
            mOnCollisionEffects.Add(e);
        }

        [MoonSharpHidden]
        public void AddOnConditionEffect(ObjectEffect e)
        {
            mOnConditionEffects.Add(e);
        }

        [MoonSharpHidden]
        public void AddOnGameAreaExitEffect(ObjectEffect e)
        {
            mOnGameAreaExitEffects.Add(e);
        }

        [MoonSharpHidden]
        public void AddOnSpawnEffect(ObjectEffect e)
        {
            mOnSpawnEffects.Add(e);
        }

    }
}
