
using UnityEngine;
using Unity.Netcode;

public struct EntityData : INetworkSerializable
{
    public Vector3 Position;
    public Vector3 Rotation;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Position);
        serializer.SerializeValue(ref Rotation);
    }
}