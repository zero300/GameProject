using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour , IInteract
{
    public int keyCode;
    public float description;

    public void Interact()
    {
        Debug.Log("¾ß°_Æ_°Í");
        EventManager.Broadcast(Events.unlockEvent);
        Destroy(gameObject);
    }
}
