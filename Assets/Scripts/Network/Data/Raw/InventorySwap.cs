using Unity.Netcode;

public struct InventorySwap : INetworkSerializable
{
    public int SlotIndex1;
    public int SlotIndex2;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref SlotIndex1);
        serializer.SerializeValue(ref SlotIndex2);
    }
}