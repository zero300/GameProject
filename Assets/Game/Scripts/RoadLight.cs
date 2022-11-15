using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLight : MonoBehaviour , IInteract
{

    private new Light light;
    private void Awake()
    {
        light = transform.Find("Light").GetComponent<Light>();
    }

    public void Interact()
    {
        light.enabled = !light.enabled;
    }
}
