using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveableEntity : InteractableEntity
{
    [SerializeField] private bool isMoveable = true;
    [SerializeField] Vector3 destination;
    public Vector3 Destination => destination;
    private NavMeshAgent navMeshAgent;

    public Action OnDestinationReached;
    [SerializeField] private float destinationMargin = 0.1f;


    internal override void Start()
    {
        base.Start();
        navMeshAgent = GetComponent<NavMeshAgent>();
        OnDestinationReached += ResetNavMeshAgent;
    }

    public virtual void SetDestination(Vector3 destination)
    {
        if(!isMoveable)
            return;

        if(destination != this.destination)
        {
            this.destination = destination;
            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(destination);
        }
    }

    internal override void FixedUpdate()
    {
        base.FixedUpdate();

        if (navMeshAgent.enabled && navMeshAgent.remainingDistance <= destinationMargin)
        {
            OnDestinationReached?.Invoke();
        }

    }


    void ResetNavMeshAgent()
    {
        navMeshAgent.enabled = false;
      //  navMeshAgent.ResetPath();
    }

}
