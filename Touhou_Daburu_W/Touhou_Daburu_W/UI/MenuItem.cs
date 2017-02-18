using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Touhou_Daburu_W.UI
{
    abstract class MenuItem : IMenuItem
    {
        protected ItemActivateDelegate mItemFunction;
        protected Vector2 mPosition;
        protected Vector2 mItemRectDiag;
        private IMenuItem mParent;

        public MenuItem()
        {
            mPosition = new Vector2();
            mItemRectDiag = new Vector2();
            mParent = null;
            mItemFunction = () => { };
        }

        virtual public void Activate()
        {
            mItemFunction();
        }
        virtual public void Link(ItemActivateDelegate activation)
        {
            mItemFunction = activation; 
        }

        public void SetPosition(float x, float y)
        {
            mPosition = new Vector2(x, y);
        }

        public void SetParent(IMenuItem parent)
        {
            mParent = parent;
        }

        virtual public void Render(SpriteBatch sb, SpriteFont font, Color color)
        {

        }

        public Vector2 GetPosition()
        {
            return mPosition;
        }
        public Vector2 GetItemSpan()
        {
            return mItemRectDiag;
        }

        public void SetPosition(Vector2 nPos)
        {
            mPosition = nPos; 
        }
    }
}
