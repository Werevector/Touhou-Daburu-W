using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace BulletEngineNeo.ScriptEffects
{
    public class EffectFactory
    {
        EffectFunctionStorage mFunctionStorage;

        public EffectFactory()
        {
            mFunctionStorage = new EffectFunctionStorage();
        }

        public void Initialize()
        {
            mFunctionStorage.Initialize("EffectScripts/Bullets");
        }

        public void Update()
        {
            mFunctionStorage.Update();
        }

        public ObjectEffect CreateEffect(string condition, string effectName)
        {
            ObjectEffect effect = new ObjectEffect();
            effect.EffectFunction = mFunctionStorage.GetEffectFunction(effectName);
            return effect;
        }
    }
}
