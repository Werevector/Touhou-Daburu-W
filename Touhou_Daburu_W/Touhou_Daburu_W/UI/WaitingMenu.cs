using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Touhou_Daburu_W.UI
{
    class WaitingMenu
    {
        SpriteFont font;
        ButtonItem mWaitingLabel;
        ButtonItem mDotCycle;
        float mTimer;

        int mDotCount;
        public WaitingMenu(SpriteFont font, MenuManager manager)
        {
            mDotCount = 0;
            mTimer = 0.0f;
            mWaitingLabel = new ButtonItem(font, "Waiting for connection");
            Vector2 wb = manager.GetWindowSize();
            mWaitingLabel.SetPosition(wb.X/2-mWaitingLabel.GetItemSpan().X/2, wb.Y/2-mWaitingLabel.GetItemSpan().Y/2);
            mDotCycle = new ButtonItem(font, "");
            mDotCycle.SetPosition(wb.X / 2 - 30, wb.Y / 2 + 10);
        }
        public void Draw(SpriteBatch sb, SpriteFont font, Color color)
        {
            mWaitingLabel.Render(sb, font, color);
            String dots = "";
            for (int i = 0; i < mDotCount; i++)
                dots += ".";
            mDotCycle.SetText(font, dots);
            mDotCycle.Render(sb, font, color);
        }

        public void Update(GameTime gameTime)
        {
            mTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(mTimer > 0.5)
            {
                mTimer = 0.0f;
                mDotCount++;
                if (mDotCount > 3) mDotCount = 0;
            }
        }
    }
}
