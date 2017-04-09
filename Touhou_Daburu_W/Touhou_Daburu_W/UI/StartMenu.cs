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

        public delegate void SwitchMenu();
        public event SwitchMenu SwitchToConnect;
        public event SwitchMenu SwitchToHost;

        public StartMenu(SpriteFont font, MenuManager manager)
        {
            mMenuChoices = new ItemListSelectable(null, new Vector2(50, 50), ItemListType.Vertical,
                new List<IMenuItem> {
                    new ButtonItem(font, "Connect to Game", Connect),
                    new ButtonItem(font, "Host Game", Host),
                    new ButtonItem(font, "Options")
                });
        }

        public void HandleInput(object sender, TextInputEventArgs e)
        {
            mMenuChoices.HandleInput(sender, e);
        }

        public void Draw(SpriteBatch sb, SpriteFont font, Color color)
        {
            mMenuChoices.Render(sb, font, color);
        }

        private void Connect()
        {
            SwitchToConnect?.Invoke();
        }

        private void Host()
        {
            SwitchToHost?.Invoke();
        }

    }
}
