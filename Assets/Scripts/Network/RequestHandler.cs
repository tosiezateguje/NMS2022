using Unity.Netcode;
using Unity.Collections;
using System;


namespace nmms
{
    public class RequestHandler<T>
        where T : INetworkSerializable, new()
    {
        internal CustomMessagingManager manager;
        public string CustomMessage;
        public Action<T> OnRequestReceived;
        public Action<ulong, T> OnRequestReceivedWithSenderId;
        public Action<T> ServerRequest;
        public Action<ulong, T> ClientRequest;

        public RequestHandler() => Initialize(typeof(T).Name);
        public RequestHandler(string customMessage) => Initialize(customMessage);

        public virtual void Initialize(string customMessage)
        {
            CustomMessage = customMessage;
            manager = NetworkManager.Singleton.CustomMessagingManager;
            manager.RegisterNamedMessageHandler(CustomMessage, OnMessage);
            ServerRequest += SendToServer;
            ClientRequest += SendToClient;
        }

        ~RequestHandler()
        {
            manager.UnregisterNamedMessageHandler(CustomMessage);
            ServerRequest -= SendToServer;
            ClientRequest -= SendToClient;
        }

        public virtual void OnMessage(ulong senderId, FastBufferReader message)
        {
            message.ReadValueSafe(out T data);
            OnRequestReceived?.Invoke(data);
        }

        public virtual void SendToServer(T data)
        {
            using (var writer = new FastBufferWriter(1100, Allocator.Temp))
            {
                writer.WriteValueSafe(data);
                manager.SendNamedMessage(CustomMessage, NetworkManager.ServerClientId, writer);
            }
        }

        public virtual void SendToClient(ulong clientId, T data)
        {
            using (var writer = new FastBufferWriter(1100, Allocator.Temp))
            {
                writer.WriteValueSafe(data);
                manager.SendNamedMessage(CustomMessage, clientId, writer);
            }
        }

        public virtual void SendToAllClient(T data)
        {
            using (var writer = new FastBufferWriter(1100, Allocator.Temp))
            {
                writer.WriteValueSafe(data);
                manager.SendNamedMessage(CustomMessage, NetworkManager.Singleton.ConnectedClientsIds, writer);
            }
        }
    }
}



