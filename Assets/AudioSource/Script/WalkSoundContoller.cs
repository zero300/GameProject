using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSoundContoller : MonoBehaviour
{
     public GameObject footstep;
     public GameObject runningfootstep;
     public GameObject jumpsound;
     public GameObject fallsound;
     public GameObject player;
     [SerializeField] bool isGround;

    // Start is called before the first frame update
    void Start()
    {
        footstep.SetActive(false);
        runningfootstep.SetActive(false);
        isGround = true;
    }

    // Update is called once per frame
    void Update()
    {
        jump();
        walk();
        run();
        
    }

    public void jump()
    {
        if(player.GetComponent<PlayerControl>().Ground() && isGround == false)
        {
            Instantiate(fallsound, this.transform.position, this.transform.rotation);
            isGround = true;
        }
        else if( !player.GetComponent<PlayerControl>().Ground() && isGround == true)
        {
            Instantiate(jumpsound, this.transform.position, this.transform.rotation);   
            isGround = false;
        }
    }

    public void run()
    {
        if(isGround)
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
        }
        else
        {
            StopRunning();
        }
        

        if(Input.GetKeyUp("w") || !Input.GetKey(KeyCode.LeftShift))
        {
            
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
        if(isGround)
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
        }
        else
        {
            StopFootsteps();
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
