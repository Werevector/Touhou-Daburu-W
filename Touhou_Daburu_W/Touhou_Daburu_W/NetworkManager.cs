using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Lidgren.Network;
namespace Touhou_Daburu_W
{
    class NetworkManager
    {
        bool mIsServer;
        bool mIsConnected;
        int mPort;
        NetPeerConfiguration mConfiguration;
        NetServer mServer;
        NetClient mClient;
        PlayerManager mPlayerManager;

        public NetworkManager()
        {
            mIsServer = false;
            mIsConnected = false;
        }

        public void InitAsServer(int port)
        {
            mIsServer = true;
            mConfiguration = new NetPeerConfiguration("daburu") { Port = port };
            mServer = new NetServer(mConfiguration);
            mServer.Start();
        }

        public void InitAsClient()
        {
            mConfiguration = new NetPeerConfiguration("daburu");
            mClient = new NetClient(mConfiguration);
            mClient.Start();
        }

        public void Connect(string ip, int port)
        {
            if (mClient != null)
            {
                mClient.Connect(ip, port); 
            }
        }

        public void Update()
        {
            if (mIsServer)
                UpdateServer();
            else
                UpdateClient();
        }

        private void UpdateClient()
        {
            NetIncomingMessage message;
            while ((message = mClient.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        float X = message.ReadFloat();
                        float Y = message.ReadFloat();
                        mPlayerManager.SetPlayerOnePosition(new Vector2(X, Y));
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        switch (message.SenderConnection.Status)
                        {
                            case NetConnectionStatus.Connected:
                                mIsConnected = true;
                                break;
                            case NetConnectionStatus.Disconnected:
                                mIsConnected = false;
                                break;
                        }
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)
                        Console.WriteLine(message.ReadString());
                        break;

                    /* .. */
                    default:
                        Console.WriteLine("unhandled message with type: "
                            + message.MessageType);
                        break;
                }
            }
            SendPlayerPosition();
        }

        private void UpdateServer()
        {
            NetIncomingMessage message;
            while ((message = mServer.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        float X = message.ReadFloat();
                        float Y = message.ReadFloat();
                        mPlayerManager.SetPlayerTwoPosition(new Vector2(X, Y));
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        switch (message.SenderConnection.Status)
                        {
                            case NetConnectionStatus.Connected:
                                mIsConnected = true;
                                break;
                            case NetConnectionStatus.Disconnected:
                                mIsConnected = false;
                                break;
                        }
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)
                        Console.WriteLine(message.ReadString());
                        break;

                    /* .. */
                    default:
                        Console.WriteLine("unhandled message with type: "
                            + message.MessageType);
                        break;
                }
            }
            SendPlayerPosition();
        }

        public bool IsConnected() { return mIsConnected; }
        public bool IsServer() { return mIsServer; }
        public void SetPlayerManager(PlayerManager manager) { mPlayerManager = manager; }
        private void SendPlayerPosition()
        {
            if(mPlayerManager != null)
            {
                if (mIsServer && mIsConnected)
                {
                    var message = mServer.CreateMessage();
                    message.Write(mPlayerManager.GetPlayerOnePosition().X);
                    message.Write(mPlayerManager.GetPlayerOnePosition().Y);
                    mServer.SendMessage(message, mServer.Connections[0], NetDeliveryMethod.ReliableOrdered);
                }else if (mIsConnected)
                {
                    var message = mClient.CreateMessage();
                    message.Write(mPlayerManager.GetPlayerTwoPosition().X);
                    message.Write(mPlayerManager.GetPlayerTwoPosition().Y);
                    mClient.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
                }
            }
        }

    }
}
