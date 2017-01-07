using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Touhou_Daburu
{

    class Utility
    {
        public static List<Vector2> CreateSplinePoints(List<Vector2> points, int resolution)
        {
            List<Vector2> splinePoints = new List<Vector2>();

            for(int i = 0; i < points.Count -3; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    splinePoints.Add( PointOnSpline(points[i], points[i + 1], points[i + 2], points[i + 3], (1f / resolution) * j) );
                }
            }


            return splinePoints;
        }

        public static Vector2 PointOnSpline(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float amount)
        {
            return new Vector2(
                CatmullRom(p1.X, p2.X, p3.X, p4.X, amount),
                CatmullRom(p1.Y, p2.Y, p3.Y, p4.Y, amount));
        }

        public static float CatmullRom(float p1, float p2, float p3, float p4, float amount)
        {
            return MathHelper.CatmullRom(p1, p2, p3, p4, amount);
        }

        public static void DrawLine(SpriteBatch sb, Texture2D debugTexture, Vector2 start, Vector2 end, Color c)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(debugTexture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                c, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }

        public static void DrawRectangle(SpriteBatch sb, Texture2D debugTexture, Vector2 pos, Rectangle rect, Color c)
        {
            sb.Draw(debugTexture, new Rectangle((int)pos.X - rect.Width / 2, (int)pos.Y - rect.Height / 2, rect.Width, rect.Height), null, c, 0, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
