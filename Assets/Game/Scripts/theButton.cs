using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theButton : MonoBehaviour , IInteract
{
    private IInteract InteractObj;

    void Start()
    {
        InteractObj = transform.GetChild(0).GetComponent<IInteract>();
    }

    public void Interact()
    {
        InteractObj.Interact();
    }
}
