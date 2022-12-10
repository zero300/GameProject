using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour , IInteract
{
    public void Interact()
    {
        Debug.Log("SavePoint Broadcast");
        EventManager.Broadcast(new CheckpointEvent() { checkpoint = transform.position } );
        Destroy(gameObject);
    }
}
