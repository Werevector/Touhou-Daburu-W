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
            spriteBatch.DrawString(font, "Create Server: I", new Vector2(650, 0), Color.White);
            spriteBatch.DrawString(font, "Create Client: O", new Vector2(650, 15), Color.White);
            spriteBatch.DrawString(font, "Connect To Server: P", new Vector2(630, 30), Color.White);
            if (mNetworkManager != null)
            {
                if (mNetworkManager.IsHost())
                {
                    spriteBatch.DrawString(font, "Client Connected: " + mNetworkManager.IsConnected(), new Vector2(2, 30), Color.DarkGoldenrod);
                    
                }
                else
                {
                    spriteBatch.DrawString(font, "Server Connected: " + mNetworkManager.IsConnected(), new Vector2(2, 30), Color.DarkGoldenrod);
                }
                


            }
        }
         
    }
}
