using Unity.Netcode;


public class ChatMessage : INetworkSerializable
{
    public string Owner;
    public string Message;



    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (Owner == null)
        {
            Owner = "";
        }
        serializer.SerializeValue(ref Owner);
        serializer.SerializeValue(ref Message);
    }
}