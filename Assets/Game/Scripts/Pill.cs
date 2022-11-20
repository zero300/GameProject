using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour , IInteract
{

    public UpdateType type;
    public float increaseNumber;
    public void Interact()
    {
        // TODO : 可以配合吃下的動畫，或是 UI
        UpdateEvent newEvent = new UpdateEvent() { updateType = type  , increaseNum = increaseNumber };
        EventManager.Broadcast(newEvent);
        Destroy(gameObject);
    }
}
