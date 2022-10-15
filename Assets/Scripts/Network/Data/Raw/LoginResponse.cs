using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public struct LoginResponse : INetworkSerializable
{
    public int ResponseCode;
    public string ResponseMessage;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ResponseCode);
        if(this.ResponseMessage != null)
            serializer.SerializeValue(ref ResponseMessage);

    }
}
