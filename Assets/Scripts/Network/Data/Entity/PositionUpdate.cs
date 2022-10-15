using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PositionUpdate : INetworkSerializable
{
    public Vector3 Position;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Position);
    }
}
