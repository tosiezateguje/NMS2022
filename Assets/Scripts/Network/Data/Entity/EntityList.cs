using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[CreateAssetMenu(fileName = "New Entity List", menuName = "Entity List")]
public class EntityList : ScriptableObject
{
    [SerializeField] List<GameObject> entities;
    public List<GameObject> Entities => entities;



    public bool GetEntityByIndex(int index, out GameObject entity)
    {
        entity = entities[index];
        if (entity != null)
            return true;
        return false;
    }
}

