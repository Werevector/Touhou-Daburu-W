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

        public void HandleEvents()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Down))
                SelectNewItem(mSelected + 1);
            if (state.IsKeyDown(Keys.Up))
                SelectNewItem(mSelected - 1);
        }

        public override void Render(SpriteBatch sb, SpriteFont font, Color color)
        {
            for (int i = 0; i < mItems.Count; i++)
            {
                mItems[i].Render(sb, font, (i == mSelected ? color : Color.Red) );
            }
        }
    }
}
