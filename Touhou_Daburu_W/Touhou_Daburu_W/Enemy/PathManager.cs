using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Touhou_Daburu_W
{
    class PathManager
    {
        private List<Vector2> mPathPoints;
        private int mPathIndex;
        private float mRadius;
        private Vector2 mResultVector;

        public PathManager()
        {
            mPathPoints = new List<Vector2>();
            mPathIndex = 0;
            mRadius = 3.0f;
            mResultVector = new Vector2(0, 0);
        }

        public PathManager(List<Vector2> points)
        {
            mPathPoints = points;
            mPathIndex = 0;
            mRadius = 3.0f;
            mResultVector = new Vector2(0, 0);
        }

        public void Update(Vector2 position)
        {
            if (mPathPoints.Count != 0 && mPathIndex != mPathPoints.Count - 1)
            {
                if ( (position - mPathPoints[mPathIndex]).Length() < mRadius )
                    mPathIndex++;

                double angle = 
                    Math.Atan2( mPathPoints[mPathIndex].Y - position.Y, 
                                mPathPoints[mPathIndex].X - position.X);

                mResultVector.X = (float)Math.Cos(angle);
                mResultVector.Y = (float)Math.Sin(angle);
            }
        }

        public Vector2 GetUnitVector() { return mResultVector; }

        public void SetPathPoints(List<Vector2> points) { mPathPoints = points; }
    }
}
