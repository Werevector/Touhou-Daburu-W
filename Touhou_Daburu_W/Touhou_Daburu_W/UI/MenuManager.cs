
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Touhou_Daburu_W.UI;
using Touhou_Daburu_W.UI.Events;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Touhou_Daburu_W
{
    class MenuManager
    {
        enum MainMenuState
        {
            Main,
            Host,
            Connect,
            Options,
            Waiting,
            Exit
        }
        SpriteFont font;
        StartMenu mStart;
        HostGameMenu mHost;
        ConnectGameMenu mConnect;
        WaitingMenu mWait;
        GameWindow mWindow;
        MainMenuState mMenuState;

        public delegate void PlayerRequestedHost(object sender, HostRequestedArgs host);
        public event PlayerRequestedHost RequestedHost;

        public delegate void PlayerRequestedConnect(object sender, ConnectRequestArgs connection);
        public event PlayerRequestedConnect RequestedConnect;

        public MenuManager(GameWindow window)
        {
            mMenuState = MainMenuState.Main;
            mWindow = window;
            mWindow.TextInput += HandleInput;

        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("YuyukoFont");
            //font = content.Load<SpriteFont>("ReimuFont");
            mHost = new HostGameMenu(font, this);
            mStart = new StartMenu(font, this);
            mConnect = new ConnectGameMenu(font, this);
            mWait = new WaitingMenu(font, this);

            mStart.SwitchToHost     += SwitchToHost;
            mStart.SwitchToConnect  += SwitchToConnect;
            mHost.PortIsReady       += HostIsReady;
            mHost.switchToMain      += SwitchToMain;
            mHost.waitingForConnection += SwitchToWaiting;
            mConnect.switchToMain   += SwitchToMain;
            mConnect.ConnectIsReady += ConnectIsReady;
        }

        public void Update(GameTime gameTime)
        {
            if(mMenuState == MainMenuState.Waiting)
            {
                mWait.Update(gameTime);
            }
        }

        public void HandleInput(object sender, TextInputEventArgs e)
        {
            switch (mMenuState)
            {
                case MainMenuState.Main:
                    mStart.HandleInput(sender, e);
                    break;
                case MainMenuState.Host:
                    mHost.HandleInput(sender, e); 
                    break;
                case MainMenuState.Connect:
                    mConnect.HandleInput(sender, e);
                    break;
                case MainMenuState.Options:
                    break;
                case MainMenuState.Waiting:
                    break;
                case MainMenuState.Exit:
                    break;
                default:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (mMenuState)
            {
                case MainMenuState.Main:
                    mStart.Draw(spriteBatch, font, Color.White);
                    break;
                case MainMenuState.Host:
                    mHost.Draw(spriteBatch, font, Color.White);
                    break;
                case MainMenuState.Connect:
                    mConnect.Draw(spriteBatch, font, Color.White);
                    break;
                case MainMenuState.Waiting:
                    mWait.Draw(spriteBatch, font, Color.White);
                    break;
                case MainMenuState.Options:
                    break;
                default:
                    break;
            }
        }

        public Vector2 GetWindowSize()
        {
            return new Vector2(mWindow.ClientBounds.Width, mWindow.ClientBounds.Height);
        }

        private void SwitchToHost()
        {
            mMenuState = MainMenuState.Host;
        }

        private void SwitchToMain()
        {
            mMenuState = MainMenuState.Main;
        }

        private void SwitchToConnect()
        {
            mMenuState = MainMenuState.Connect;
        }

        private void SwitchToWaiting()
        {
            mMenuState = MainMenuState.Waiting;
        }

        private void SwitchState(MainMenuState state)
        {
            mMenuState = state;
        }

        private void HostIsReady(string port)
        {
            mMenuState = MainMenuState.Exit;
            RequestedHost?.Invoke(this, new HostRequestedArgs(port));
        }

        private void ConnectIsReady(string ip, string port)
        {
            mMenuState = MainMenuState.Exit;
            RequestedConnect?.Invoke(this, new ConnectRequestArgs(ip, port));

        }
    }
}
