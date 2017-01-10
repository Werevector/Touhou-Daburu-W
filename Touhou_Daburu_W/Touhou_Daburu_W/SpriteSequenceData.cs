using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touhou_Daburu_W
{
    class SpriteSequenceData
    {
        public List<int> mSequence { get; set; }
        public bool mLooping { get; set; }
        public int mFramesPerSecond { get; set; }
        public int mSubLoop = 0;

        public SpriteSequenceData()
        {
            mSequence = new List<int> { 0 };
            mLooping = true;
            mFramesPerSecond = 10;
        }
    }
}
