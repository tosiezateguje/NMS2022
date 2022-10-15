using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : NetworkBehaviour
{
    private ParticleManager particleManager;
    private NavMeshAgent agent;
    [SerializeField] private Vector3 targetPosition;

    private void Start()
    {
        particleManager = GameManager.Instance.ParticleManager;
        agent = GetComponent<NavMeshAgent>();
        if(IsOwner)
            GameManager.Instance.CameraManager.SetTarget(transform);
        

    }

    // void Update()
    // {
    //     if (agent == null)
    //         return;

    //     if(Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
    //         return;              
        
    // }

}
