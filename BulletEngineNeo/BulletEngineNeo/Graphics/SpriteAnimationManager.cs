using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BulletEngineNeo.Graphics
{
    class SpriteAnimationManager
    {
        private int mSequenceIndex;
        private double mCurrentTick;
        private string mCurrentSequenceName;

        private Dictionary<string, SpriteSequenceData> mSequenceData;
        private SpriteSequenceData mCurrentSequence;

        public SpriteAnimationManager(Dictionary<string, SpriteSequenceData> sequenceData)
        {
            mSequenceIndex = 0;
            mCurrentTick = 0.0;
            mSequenceData = sequenceData;
            ChangeSequence("Idle");
            mCurrentSequenceName = "Idle";
        }

        public void Update(GameTime gameTime)
        {
            mCurrentTick += gameTime.ElapsedGameTime.TotalSeconds;
            double t = 1 / (double)mCurrentSequence.mFramesPerSecond;
            if (mCurrentTick > t)
            {
                mCurrentTick = 0.0;
                mSequenceIndex++;

                if (mSequenceIndex == mCurrentSequence.mSequence.Count)
                {
                    if (mCurrentSequence.mLooping)
                        mSequenceIndex = mCurrentSequence.mSubLoop;
                    else
                        mSequenceIndex = mCurrentSequence.mSequence.Count - 1;
                }
            }
        }

        public void ChangeSequence(string sequenceName)
        {
            if (mCurrentSequenceName != sequenceName)
            {
                mCurrentSequence = mSequenceData[sequenceName];
                mSequenceIndex = 0;
                mCurrentTick = 0.0;
                mCurrentSequenceName = sequenceName;
            }
        }

        public int GetCurrentSequenceKey()
        {
            return mCurrentSequence.mSequence[mSequenceIndex];
        }
    }
}
