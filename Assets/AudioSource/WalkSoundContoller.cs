using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSoundContoller : MonoBehaviour
{
     public GameObject footstep;
     public GameObject runningfootstep;

    // Start is called before the first frame update
    void Start()
    {
        footstep.SetActive(false);
        runningfootstep.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        walk();
        run();
    }

    public void run()
    {
        if(Input.GetKey("w") && Input.GetKey(KeyCode.LeftShift))
        {
            runningfootsteps();
        }
        if(Input.GetKey("s") && Input.GetKey(KeyCode.LeftShift))
        {
            runningfootsteps();
        }
        if(Input.GetKey("a") && Input.GetKey(KeyCode.LeftShift))
        {
            runningfootsteps();
        }
        if(Input.GetKey("d") && Input.GetKey(KeyCode.LeftShift))
        {
            runningfootsteps();
        }

        if(Input.GetKeyUp("w") || !Input.GetKey(KeyCode.LeftShift))
        {
            StopRunning();
        }
        if(Input.GetKeyUp("s") || !Input.GetKey(KeyCode.LeftShift))
        {
            StopRunning();
        }
        if(Input.GetKeyUp("a") || !Input.GetKey(KeyCode.LeftShift))
        {
            StopRunning();
        }
        if(Input.GetKeyUp("d") || !Input.GetKey(KeyCode.LeftShift))
        {
            StopRunning();
        }
    }
    public void walk()
    {
        if(Input.GetKey("w") && !Input.GetKey(KeyCode.LeftShift))
        {
            footsteps();
        }
        if(Input.GetKey("s") && !Input.GetKey(KeyCode.LeftShift))
        {
            footsteps();
        }
        if(Input.GetKey("a") && !Input.GetKey(KeyCode.LeftShift))
        {
            footsteps();
        }
        if(Input.GetKey("d") && !Input.GetKey(KeyCode.LeftShift))
        {
            footsteps();
        }

        if(Input.GetKeyUp("w") || Input.GetKey(KeyCode.LeftShift))
        {
            StopFootsteps();
        }
        if(Input.GetKeyUp("s") || Input.GetKey(KeyCode.LeftShift))
        {
            StopFootsteps();
        }
        if(Input.GetKeyUp("a") || Input.GetKey(KeyCode.LeftShift))
        {
            StopFootsteps();
        }
        if(Input.GetKeyUp("d") || Input.GetKey(KeyCode.LeftShift))
        {
            StopFootsteps();
        }
    }

    void footsteps()
    {
        footstep.SetActive(true);
    }

    void StopFootsteps()
    {
        footstep.SetActive(false);
    }
    void runningfootsteps()
    {
        runningfootstep.SetActive(true);
    }
    void StopRunning()
    {
        runningfootstep.SetActive(false);
    }
}
