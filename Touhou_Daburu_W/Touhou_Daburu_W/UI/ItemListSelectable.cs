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
    class ItemListSelectable : ItemList
    {
        private int mSelected;
        public ItemListSelectable(IMenuItem parent, Vector2 pos, ItemListType type, List<IMenuItem> list = null) : base(parent, pos, type, list)
        {
            mSelected = 0;

        }

        public void SelectNewItem(int index)
        {
            if (index >= 0 && index < mItems.Count)
                mSelected = index; 
        }

        public override void Render(SpriteBatch sb, SpriteFont font, Color color)
        {
            for (int i = 0; i < mItems.Count; i++)
            {
                mItems[i].Render(sb, font, (i == mSelected ? Color.Red : color) );
            }
        }

        virtual public void HandleInput(object sender, TextInputEventArgs e)
        {
            char charEntered = e.Character;
            if (charEntered == 'w')
                SelectNewItem(mSelected - 1);
            if (charEntered == 's')
                SelectNewItem(mSelected + 1);
            if (charEntered == '\r')
                mItems[mSelected].Activate();
        }
    }
}
