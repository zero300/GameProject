using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour , IInteract
{

    public UpdateType type;
    public float increaseNumber;
    public void Interact()
    {
        // TODO : �i�H�t�X�Y�U���ʵe�A�άO UI
        UpdateEvent newEvent = new UpdateEvent() { updateType = type  , increaseNum = increaseNumber };
        EventManager.Broadcast(newEvent);
        Destroy(gameObject);
    }
}
