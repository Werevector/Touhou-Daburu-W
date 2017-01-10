using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Touhou_Daburu_W
{
    class InfoPrinter
    {
        SpriteFont font;

        private double mFrameTime;
        private bool mIsRunningSlow;

        public InfoPrinter()
        {
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("infoFont");
            
        }

        public void Update(GameTime gameTime)
        {
            mFrameTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            mIsRunningSlow = gameTime.IsRunningSlowly;
        }

        public void DrawFrameTiming(SpriteBatch spriteBatch, GameTime gameTime)
        {
            mFrameTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            spriteBatch.DrawString(font, "FrameTime: " + mFrameTime, new Vector2(2, 0), Color.Red);
            spriteBatch.DrawString(font, "Slow: " + mIsRunningSlow, new Vector2(2, 15), Color.Red);
        }
    }
}
