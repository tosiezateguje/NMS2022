using Unity.Netcode;
public struct LoginRequest : INetworkSerializable
{
    public string Username;
    public string Password;
    public string Email;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Username);
        serializer.SerializeValue(ref Password);
        if(this.Email != null)
        {
            serializer.SerializeValue(ref Email);
        }
    }
}

