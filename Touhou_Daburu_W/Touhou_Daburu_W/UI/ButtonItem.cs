using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Touhou_Daburu_W.UI
{
    class ButtonItem : MenuItem
    {
        private string mText;
        private IMenuItem mParent;

        public ButtonItem() : base()
        {
            mText = "";
        }

        public ButtonItem(SpriteFont font, String text)
        {
            mText = text;
            mItemRectDiag = font.MeasureString(text);
        }

        public override void Activate()
        {
            mItemFunction();
        }
        public override void Link(ItemActivateDelegate activation)
        {
            mItemFunction = activation;
        }

        public override void Render(SpriteBatch sb, SpriteFont font, Color color)
        {
            Vector2 renderPos = mPosition; 
            sb.DrawString(font, mText, mPosition, color);
        }

        public void SetText(SpriteFont font, ref string text)
        {
            mText = text;
            mItemRectDiag = font.MeasureString(text);
        }
    }
}
