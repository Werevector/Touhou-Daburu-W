using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BulletEngineNeo.Bullet
{
    delegate bool EffectCondition();
    delegate void EffectFunction(GameBullet bullet, MainGameLogic game);
    class BulletEffect
    {
        EffectCondition mEffectCondition;
        EffectFunction mBulletEffect;

        public EffectCondition BulletEffectCondition
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
        public EffectFunction BulletEffectFunction
        {
            get
            {
                return mBulletEffect;
            }

            set
            {
                mBulletEffect = value;
            }
        }

        public BulletEffect() { }

        /// <summary>
        /// Checks the internal condition.
        /// If true trigger effect.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="game"></param>
        public void Check(GameTime gameTime, MainGameLogic game, GameBullet bullet)
        {
            if ((bool)mEffectCondition?.Invoke())
            {
                mBulletEffect?.Invoke(bullet, game);
            }
        }

        /// <summary>
        /// Forces a trigger of the effect, ignoring condition.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="game"></param>
        public void Force(GameTime gameTime, MainGameLogic game, GameBullet bullet)
        {
            mBulletEffect?.Invoke(bullet, game);
        }
    }
}
