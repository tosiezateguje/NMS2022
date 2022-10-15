using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Collider))]
public class InteractableEntity : NetworkEntity 
{

    public virtual void OnMouseDown() { }
    public virtual void OnMouseOver() { }

}
