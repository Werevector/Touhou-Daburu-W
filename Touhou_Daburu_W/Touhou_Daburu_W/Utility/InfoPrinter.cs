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
        public NetworkManager mNetworkManager;
        private FrameCounter frameCounter;

        private double mFrameTime;
        private bool mIsRunningSlow;

        public InfoPrinter()
        {
            frameCounter = new FrameCounter();
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("infoFont");   
        }

        public void Update(GameTime gameTime)
        {
            mFrameTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            mIsRunningSlow = gameTime.IsRunningSlowly;
            frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

        }

        public void DrawFrameTiming(SpriteBatch spriteBatch, GameTime gameTime)
        {
            mFrameTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            spriteBatch.DrawString(font, "FrameTime: " + mFrameTime, new Vector2(2, 0), Color.White);
            spriteBatch.DrawString(font, "Slow: " + mIsRunningSlow, new Vector2(2, 17), mIsRunningSlow ? Color.Red : Color.White);
            spriteBatch.DrawString(font, "FPS: " + frameCounter.AverageFramesPerSecond, new Vector2(2, 36), Color.Red);
        }

        public void DrawConnectionInfo(SpriteBatch spriteBatch)
        {
            if (mNetworkManager != null)
            {
                if (mNetworkManager.IsHost())
                    spriteBatch.DrawString(font, "Player Connected: " + mNetworkManager.IsConnected(), new Vector2(615, 10), Color.DarkGoldenrod);
                else
                    spriteBatch.DrawString(font, "Host Connected: " + mNetworkManager.IsConnected(), new Vector2(615, 10), Color.DarkGoldenrod);
            }
        }
         
    }
}
