using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{

    private NetworkVariable<PlayerData> playerData = new(writePerm: NetworkVariableWritePermission.Owner);

    private Vector3 velocity;
    private float yRotVelocity;
    [SerializeField] private float cheapInterpolationTime = 0.1f;

    private void Update()
    {
        if (IsOwner)
        {
            playerData.Value = new PlayerData()
            {
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles
            };
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, playerData.Value.Position, ref velocity, cheapInterpolationTime);
            transform.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, playerData.Value.Rotation.y, ref yRotVelocity, cheapInterpolationTime), 0);
        }
    }



}

public struct PlayerData : INetworkSerializable
{
    private float posX, posZ, rotY;

    public Vector3 Position
    {
        get => new Vector3(posX, 0, posZ);
        set
        {
            posX = value.x;
            posZ = value.z;
        }
    }

    public Vector3 Rotation
    {
        get => new Vector3(0, rotY, 0);
        set => rotY = value.y;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref posX);
        serializer.SerializeValue(ref posZ);
        serializer.SerializeValue(ref rotY);
    }
}

