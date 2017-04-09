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
    class TextInputItem : MenuItem
    {
        private string mText;
        private IMenuItem mParent;

        private const char ActivateChar = '\r';
        private const char DeleteChar = '\b';

        public bool focus { get; set; }

        public TextInputItem() : base()
        {
            mText = "";
            focus = false;
        }

        public TextInputItem(SpriteFont font, String text)
        {
            mText = text;
            focus = false;
            mItemRectDiag = font.MeasureString(text);
        }

        public TextInputItem(SpriteFont font, String text, ItemActivateDelegate del)
        {
            mText = text;
            focus = false;
            mItemRectDiag = font.MeasureString(text);
            mItemFunction = del;
        }

        public override void Render(SpriteBatch sb, SpriteFont font, Color color)
        {
            Vector2 renderPos = mPosition;
            try
            {
                sb.DrawString(font, mText, mPosition, color);
            }
            catch (ArgumentException)
            {
                Console.Write(mText + " contains unsupported value");
                mText = mText.Remove(mText.Length-1);
                sb.DrawString(font, mText, mPosition, color);
            }
        }
        public void SetText(SpriteFont font, string text)
        {
            mText = text;
            mItemRectDiag = font.MeasureString(text);
        }

        public void ClearField() { mText = ""; }

        public string GetText()
        {
            return mText;
        }

        virtual public void HandleInput(object sender, TextInputEventArgs e)
        {
            switch (e.Character)
            {
                case '\r':
                    Activate();
                    break;
                case '\b':
                    if(mText.Length != 0) mText = mText.Remove(mText.Length - 1);
                    break;
                default:
                    mText += e.Character;
                    break;
            }
        }
    }
}
