using UnityEngine;
using Unity.Netcode;
using nms;

namespace example
{
    public class ChatManager : NetworkBehaviour
    {
        RequestHandler<ChatMessage> chatMessageRequestHandler = new RequestHandler<ChatMessage>();

        void OnNetworkStart()
        {
            chatMessageRequestHandler.OnRequestReceived += CreateChatMessage;
        }

        void CreateChatMessage(ChatMessage message)
        {
            //tu powinien wjechac kodzik na np wyswietlanie na ekranie :3
            Debug.Log(message.Owner + ": " + message.Message);
        }

        public void SendChatMessage(string message)
        {
            chatMessageRequestHandler.SendToServer(new ChatMessage
            {
                Owner = "Player",
                Message = message
            });
        }

    }



    //implementujemy interfejs wymagany przez unity
    public struct ChatMessage : INetworkSerializable
    {
        public string Owner;
        public string Message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Owner);
            serializer.SerializeValue(ref Message);
        }
    }
}

