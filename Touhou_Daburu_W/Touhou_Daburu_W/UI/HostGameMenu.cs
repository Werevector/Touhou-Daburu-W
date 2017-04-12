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
    
    class HostGameMenu
    {

        ButtonItem mPortLabel;
        TextInputItem mHostPortTextField;
        ItemActivateDelegate mHostReady;

        public delegate void HostPortReady(string port);
        public delegate void SwitchToMain();
        public event HostPortReady PortIsReady;
        public event SwitchToMain switchToMain;
        public event SwitchToMain waitingForConnection;

        public HostGameMenu(SpriteFont font, MenuManager manager)
        {
            mPortLabel = new ButtonItem(font, "Port: ");
            mHostPortTextField = new TextInputItem(font, "8090", ReadyHost);
            mHostPortTextField.SetPosition(mPortLabel.GetItemSpan().X + 10, 0);
        }

        public void HandleInput(object sender, TextInputEventArgs e)
        {
            if (e.Character == (char)27)
                switchToMain?.Invoke();
            else
                mHostPortTextField.HandleInput(sender, e);
        }

        public void Draw(SpriteBatch sb, SpriteFont font, Color color)
        {
            mPortLabel.Render(sb, font, Color.Cyan);
            mHostPortTextField.Render(sb, font, color);
        }

        private void ReadyHost()
        {
            PortIsReady?.Invoke(mHostPortTextField.GetText());
            waitingForConnection?.Invoke();
        }
    }
}
