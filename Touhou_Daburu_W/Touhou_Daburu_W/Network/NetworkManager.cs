using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using Touhou_Daburu_W.UI.Events;
namespace Touhou_Daburu_W
{
    class NetworkManager
    {
        enum NetGameMessageType
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
        bool mStarted;
        int mPort;
        NetPeerConfiguration mConfiguration;
        NetServer mHost;
        NetClient mClient;
        PlayerManager mPlayerManager;

        public delegate void PlayerConnected(object sender);
        public event PlayerConnected PlayerHasConnected;

        public NetworkManager()
        {
            mIsHost = false;
            mIsConnected = false;
            mStarted = false;
        }

        private void InitAsServer(int port)
        {
            mIsHost = true;
            mConfiguration = new NetPeerConfiguration("daburu") { Port = port };
            mHost = new NetServer(mConfiguration);
            mPort = port;
            mHost.Start();
            mStarted = true;
        }

        public void HandleHostRequest(object sender, HostRequestedArgs a)
        {
            InitAsServer(int.Parse(a.Port));
        }

        public void HandleClientRequest(object sender, ConnectRequestArgs a)
        {
            InitAsClient();
            Connect(a.Ip, int.Parse(a.Port));
        }

        public void InitAsClient()
        {
            mConfiguration = new NetPeerConfiguration("daburu");
            mClient = new NetClient(mConfiguration);
            mClient.Start();
            mStarted = true;
        }

        public void Connect(string ip, int port)
        {
            if (mClient != null && mStarted)
                mClient.Connect(ip, port);

            PlayerHasConnected?.Invoke(this);
        }

        public void Update()
        {
            if (mStarted)
            {
                ProccessMessages();
                SendPlayerDataMessage();
            }
        }

        private void SendDataToPlayerObject(Player player, NetIncomingMessage message)
        {
            player.SetPosition(message.ReadInt32(), message.ReadInt32());
            player.SetFiringState(message.ReadBoolean());

        }

        private NetOutgoingMessage CreatePlayerDataMessage(Player player)
        {
            NetOutgoingMessage message;
            if (mIsHost)
                message = mHost.CreateMessage();
            else
                message = mClient.CreateMessage();
            message.Write((int)NetGameMessageType.PLAYERDATA);
            message.Write((int)player.mPosition.X);
            message.Write((int)player.mPosition.Y);
            message.Write(player.mFiring);
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

        private Player GetHostPlayer()
        {
            Player player;
            if (mIsHost)
                player = mPlayerManager.GetPlayerOne();
            else
                player = mPlayerManager.GetPlayerTwo();
            return player;
        }

        private Player GetConnectedPlayer()
        {
            Player player;
            if (mIsHost)
                player = mPlayerManager.GetPlayerTwo();
            else
                player = mPlayerManager.GetPlayerOne();
            return player;
        }

        private void ProccessMessages()
        {
            NetIncomingMessage message;
            NetPeer peer;
            if (mIsHost)
                peer = mHost;
            else
                peer = mClient;

            while ((message = peer.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        switch ((NetGameMessageType)message.ReadInt32())
                        {
                            case NetGameMessageType.EVENT:
                                break;
                            case NetGameMessageType.PLAYERDATA:
                                SendDataToPlayerObject(GetConnectedPlayer(), message);
                                break;
                            default:
                                break;
                        }
                        
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        switch (message.SenderConnection.Status)
                        {
                            case NetConnectionStatus.Connected:
                                mIsConnected = true;
                                mPlayerManager.InitConnectedPlayer();
                                PlayerHasConnected?.Invoke(this);
                                break;
                            case NetConnectionStatus.Disconnected:
                                mIsConnected = false;
                                break;
                        }
                        break;

                    case NetIncomingMessageType.DebugMessage:
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
