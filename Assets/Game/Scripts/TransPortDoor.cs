using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransPortDoor : MonoBehaviour , IInteract
{
    public SceneIndex nextScene;
    public void Interact()
    {
        Debug.Log("Broadcast End");
        EventManager.Broadcast(new ArchieveEndPointEvent() );
    }
}
