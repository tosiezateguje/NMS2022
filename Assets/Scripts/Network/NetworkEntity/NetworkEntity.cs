using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System.Threading.Tasks;

[RequireComponent(typeof(NetworkObject))]
public class NetworkEntity : NetworkBehaviour
{
    private NetworkVariable<EntityData> entityData = new(writePerm: NetworkVariableWritePermission.Owner);
    private Vector3 velocity;

    [SerializeField] private float cheapInterpolationTime = 0.1f;
    [SerializeField] private int packetDelay = 100;
    [SerializeField] private bool isInterpolating = true;
    [SerializeField] private bool isStationary = false;
    public int PacketDelay => packetDelay;
    public bool IsInterpolating => isInterpolating;
    public bool IsStationary => isStationary;
    private bool holdPacket = false;

    internal virtual void Start() { }

    internal virtual void FixedUpdate()
    {
        if (isStationary)
            return;

        if (IsOwner)
        {
            SendEntityData();
        }

        if (isInterpolating)
            Interpolate();

        else
        {
            transform.position = entityData.Value.Position;
            transform.rotation = Quaternion.Euler(entityData.Value.Rotation);
        }

    }

    internal virtual void Interpolate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, entityData.Value.Position, ref velocity, cheapInterpolationTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(entityData.Value.Rotation), cheapInterpolationTime);
    }

    internal virtual async void SendEntityData()
    {
        if (holdPacket)
            return;

        holdPacket = true;
        entityData.Value = new EntityData()
        {
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles
        };
        await Task.Delay(this.packetDelay);
        holdPacket = false;
    }

}

