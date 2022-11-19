using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour , IInteract
{
    public int keyCode;
    public float description;

    public void Interact()
    {
        UnlockEvent newEvent = new UnlockEvent() { KeyCode = keyCode };
        EventManager.Broadcast(newEvent);
        Destroy(gameObject);
    }
}
