using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace BulletEngineNeo.ScriptEffects
{
    public delegate bool EffectCondition();
    public delegate void EffectFunction(DynValue bullet, MainGameLogic game);
    public class ObjectEffect
    {
        public EffectCondition mEffectCondition;
        public EffectFunction mEffectFunction;

        public EffectCondition EffectCondition
        {
            get
            {
                return mEffectCondition;
            }

            set
            {
                mEffectCondition = value;
            }
        }
        public EffectFunction EffectFunction
        {
            get
            {
                return mEffectFunction;
            }

            set
            {
                mEffectFunction = value;
            }
        }

        public ObjectEffect() { }

        /// <summary>
        /// Checks the internal condition.
        /// If true trigger effect.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="game"></param>
        public void Check(GameTime gameTime, MainGameLogic game, DynValue bullet)
        {
            if ((bool)mEffectCondition?.Invoke())
            {
                mEffectFunction?.Invoke(bullet, game);
            }
        }

        /// <summary>
        /// Forces a trigger of the effect, ignoring condition.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="game"></param>
        public void Force(GameTime gameTime, MainGameLogic game, DynValue bullet)
        {
            mEffectFunction?.Invoke(bullet, game);
        }
    }
}
