using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Touhou_Daburu_W.UI
{
    public delegate void ItemActivateDelegate();
    interface IMenuItem
    {
        void Activate();
        void Link(ItemActivateDelegate activation);
        void SetParent(IMenuItem parent);
        void Render(SpriteBatch sb, SpriteFont font, Color color);
        void SetPosition(float x, float y);
        void SetPosition(Vector2 nPos);
        Vector2 GetPosition();
        Vector2 GetItemSpan();
    }
}
