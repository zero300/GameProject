using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllBar : MonoBehaviour
{
    bool status;
    public GameObject SoundEffect;
    // Start is called before the first frame update
    private void Awake() {
        status = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void Rotate()
    {
        Instantiate(SoundEffect, this.transform.position, this.transform.rotation); //Create sound prefab
        if(!status)
        {
            transform.Rotate(new Vector3(-60, 0, 0));
            status = true;
        }
        else
        {
            transform.Rotate(new Vector3(60, 0, 0));
            status = false;
        }
    }
}
