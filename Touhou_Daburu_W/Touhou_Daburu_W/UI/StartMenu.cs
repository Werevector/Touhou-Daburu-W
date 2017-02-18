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
    class StartMenu
    {
        ItemListSelectable mMenuChoices;
        public StartMenu(SpriteFont font)
        {
            mMenuChoices = new ItemListSelectable(null, new Vector2(50, 50), ItemListType.Vertical,
                new List<IMenuItem> {
                    new ButtonItem(font, "Button 1"),
                    new ButtonItem(font, "Button 2"),
                    new ButtonItem(font, "Button 3"),
                    new ButtonItem(font, "Button 4"),
                    new ButtonItem(font, "Button 5")
                });
        }

        public void HandleEvents()
        {
            mMenuChoices.HandleEvents();
        }

        public void Draw(SpriteBatch sb, SpriteFont font, Color color)
        {
            mMenuChoices.Render(sb, font, color);
        }

    }
}
