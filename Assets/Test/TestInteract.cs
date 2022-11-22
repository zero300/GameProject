using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteract : MonoBehaviour
{
    public GameObject interactObj;
    private IInteract _interact;
    private void Awake()
    {
        _interact = interactObj.GetComponent<IInteract>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z)) {
            _interact.Interact();
        }
    }
}
