#if UNITY_SERVER || UNITY_EDITOR  //- SERVER CODE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public static class SpawningManager
{
    private static EntityList entityList = Resources.Load<EntityList>("EntityList");
    public static void SpawnEntity()
    {
        if(entityList.GetEntityByIndex(0, out GameObject entity))
        {
            NetworkObject networkObject = GameObject.Instantiate(entity).GetComponent<NetworkObject>();
            networkObject.Spawn();
        }
    }

    public static Entity SpawnPlayer()
    {
        if (entityList.GetEntityByIndex(0, out GameObject entity))
        {
            NetworkObject networkObject = GameObject.Instantiate(entity).GetComponent<NetworkObject>();
            networkObject.Spawn();
            return new Entity { EntityObject = entity, NetworkObject = networkObject };
        }
        return new Entity();
    }

}
#endif //- SERVER END