using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ClientSpawningManager : NetworkBehaviour
{
    [SerializeField] private CameraManager cameraManager;

    [ClientRpc]
    public void SetTargetClientRpc(ulong targetId)
    {
        if(IsServer) return;

        if (Server.GetObjectById(3, out NetworkObject target))
        {
            cameraManager.SetTarget(target.gameObject.transform);
        }
    }
}
