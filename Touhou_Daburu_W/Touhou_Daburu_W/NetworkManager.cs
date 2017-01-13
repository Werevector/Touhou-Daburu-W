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
        enum NetMessageType
        {
            PLAYERDATA,
            EVENT
        }

        enum NetEventType
        {
            DAMAGE,
            DEAD,
            BOMB
        }

        bool mIsHost;
        bool mIsConnected;
        int mPort;
        NetPeerConfiguration mConfiguration;
        NetServer mHost;
        NetClient mClient;
        PlayerManager mPlayerManager;

        public NetworkManager()
        {
            mIsHost = false;
            mIsConnected = false;
        }

        public void InitAsServer(int port)
        {
            mIsHost = true;
            mConfiguration = new NetPeerConfiguration("daburu") { Port = port };
            mHost = new NetServer(mConfiguration);
            mPort = port;
            mHost.Start();
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
            ProccessMessages();
            SendPlayerDataMessage();
        }

        private void SendDataToPlayerObject(Player player, NetIncomingMessage message)
        {
            player.SetPosition(message.ReadInt32(), message.ReadInt32());
            player.SetFiring(message.ReadBoolean());

        }

        private NetOutgoingMessage CreatePlayerDataMessage(Player player)
        {
            NetOutgoingMessage message;
            if (mIsHost)
                message = mHost.CreateMessage();
            else
                message = mClient.CreateMessage();
            message.Write((int)mPlayerManager.GetPlayerOnePosition().X);
            message.Write((int)mPlayerManager.GetPlayerOnePosition().Y);
            message.Write(mPlayerManager.GetPlayerOne().IsFiring());
            return message;
        }

        private void SendPlayerDataMessage()
        {
            if (mPlayerManager != null && mIsConnected)
            {
                if (mIsHost)
                    mHost.SendMessage(CreatePlayerDataMessage(mPlayerManager.GetPlayerOne()), mHost.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                else
                    mClient.SendMessage(CreatePlayerDataMessage(mPlayerManager.GetPlayerTwo()), NetDeliveryMethod.ReliableOrdered);
            }
        }

        private Player GetPlayer()
        {
            Player player;
            if (mIsHost)
                player = mPlayerManager.GetPlayerOne();
            else
                player = mPlayerManager.GetPlayerTwo();
            return player;
        }

        private void ProccessMessages()
        {
            NetIncomingMessage message;
            while ((message = mClient.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        SendDataToPlayerObject(GetPlayer(), message);
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
        }
        
        public bool IsConnected() { return mIsConnected; }
        public bool IsHost() { return mIsHost; }
        public void SetPlayerManager(PlayerManager manager) { mPlayerManager = manager; }
        

    }
}
