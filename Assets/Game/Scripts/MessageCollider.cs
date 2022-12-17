using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCollider : MonoBehaviour ,IInteract
{
    [Tooltip("想要說的話")]
    public string msg;
    public void Interact()
    {
        DisplayMessageEvent evt = new DisplayMessageEvent() { msg = this.msg};
        EventManager.Broadcast(evt);
        Destroy(gameObject);
    }
}
