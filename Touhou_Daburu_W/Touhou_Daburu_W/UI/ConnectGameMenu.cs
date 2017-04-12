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
    class ConnectGameMenu
    {
        ButtonItem mPortLabel;
        ButtonItem mIPLabel;
        TextInputItem mPortTextField;
        TextInputItem mIPTextField;
        ItemList mItemList;
        ItemList mItemListV1;
        ItemList mItemListV2;

        ItemActivateDelegate mClientReady;

        private bool ip = true;

        public delegate void ConnectInfoReady(string ip, string port);
        public delegate void SwitchToMain();
        public event ConnectInfoReady ConnectIsReady;
        public event SwitchToMain switchToMain;
        public event SwitchToMain waitingForConnection;

        public ConnectGameMenu(SpriteFont font, MenuManager manager)
        {

            mIPLabel = new ButtonItem(font, "IP: ");
            mIPTextField = new TextInputItem(font, "localhost");
            mIPTextField.Link(() => { ip = false; });

            mPortLabel = new ButtonItem(font, "Port: ");
            mPortTextField = new TextInputItem(font, "8090", ReadyConnect);

            mItemListV1 = new ItemListSelectable(null, new Vector2(0,0), ItemListType.Horizontal);
            mItemListV2 = new ItemListSelectable(null, new Vector2(0,40), ItemListType.Horizontal);

            //mItemList = new ItemListSelectable(null, new Vector2(50, 50), ItemListType.Vertical,
            //    new List<IMenuItem> {
            //        mIPLabel, mIPTextField, mPortLabel, mPortTextField
            //    });
            //mItemList = new ItemListSelectable(null, new Vector2(50, 50), ItemListType.Vertical);
           // mItemList.AddItem(mItemListV1);
            //mItemList.AddItem(mItemListV2);
            mItemListV1.AddItem(mIPLabel);
            mItemListV1.AddItem(mIPTextField);
            mItemListV2.AddItem(mPortLabel);
            mItemListV2.AddItem(mPortTextField);
        }

        public void HandleInput(object sender, TextInputEventArgs e)
        {
            if (e.Character == (char)27)
            {
                switchToMain?.Invoke();
                mIPTextField.ClearField();
                mPortTextField.ClearField();
            }
            else
            {
                if (ip)
                    mIPTextField.HandleInput(sender, e);
                else
                    mPortTextField.HandleInput(sender, e);
            }
                
        }

        public void Draw(SpriteBatch sb, SpriteFont font, Color color)
        {
            //mIPLabel.Render(sb, font, Color.DarkViolet);
            //mIPTextField.Render(sb, font, color);

            //mPortLabel.Render(sb, font, Color.Cyan);
            //mPortTextField.Render(sb, font, color);
            mItemListV1.Render(sb, font, color);
            mItemListV2.Render(sb, font, color);
        }

        private void ReadyConnect()
        {
            ConnectIsReady?.Invoke(mIPTextField.GetText(), mPortTextField.GetText());
            waitingForConnection?.Invoke();
        }
    }
}
