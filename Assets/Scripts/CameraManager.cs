using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private Transform target;
    public Vector3 ScrollVector = new Vector3(0, 10f, -6f);
    [SerializeField] GameObject cameraObject;
    private Vector3 scrollVelocity;



    public void SetTarget(Transform t) => target = t;

    public void SetPlayerAsTarget()
    {
        if (Server.GetObjectById(GameManager.Instance.CharacterData.NetworkObjectId, out NetworkObject networkObject))
        {
            target = networkObject.gameObject.transform;
        }
    }

    void Update()
    {
        if (target == null) return;
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        cameraObject.transform.position = transform.position + ScrollVector;
    }
}
