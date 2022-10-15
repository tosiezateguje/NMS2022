using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[System.Serializable]
public struct CharacterLocation : INetworkSerializable
{
    public int MapId;
    public Vector3 Position;
    public float RotationY;
    public Vector3 Rotation {
        get {
            return new Vector3(0, RotationY, 0);
        }
        set {
            RotationY = value.y;
        }
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref MapId);
        serializer.SerializeValue(ref Position);
        serializer.SerializeValue(ref RotationY);
    }
}
