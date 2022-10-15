#if UNITY_SEVER || UNITY_EDITOR //- SERVER CODE
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


[System.Serializable]
public struct Entity
{
    public GameObject EntityObject;
    public NetworkObject NetworkObject;
}

#endif //- SERVER END