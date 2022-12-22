using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class farEnemy : MonoBehaviour
{
    public Animator ani;
    public NavMeshAgent nav;
    public GameObject Player;
    public GameObject rock;
    public float attackCooldown;

    AnimatorStateInfo state;
    GameObject buffer;
    Rigidbody rb;
    float attackTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        state = ani.GetCurrentAnimatorStateInfo(0);
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        

        if (state.IsName("RunFWD") && ani.GetBool("Attack")){
            if (attackTime < 10)
            {
                attackTime += Time.deltaTime;
            }

            if (attackTime < 0)
            {
                return;
            }
            Vector3 dir = Player.transform.position - transform.position;
            GameObject temp = Instantiate(rock, this.transform.position + new Vector3(0, 2, 0), this.transform.rotation);
            buffer = temp;
            temp.GetComponent<Bullet>().player = Player;
        }

        
        ani.SetBool("Attack", false);
        ani.SetBool("Idle", false);
        ani.SetBool("Walk", false);
        //Debug.Log(Vector3.Distance(Player.transform.position, transform.position));



        if (Vector3.Distance(Player.transform.position, transform.position) < 10)
        {
            nav.SetDestination(transform.position);
            nav.isStopped = true;
            ani.SetBool("Attack", true);
            attackTime = -attackCooldown;  
        }
        else if(Vector3.Distance(Player.transform.position, transform.position) < 20)
        {
            nav.isStopped = false;
            ani.SetBool("Walk", true);
            if(state.IsName("WalkFWD"))
            {
                nav.SetDestination(Player.transform.position);
            }
        }
        else
        {
            nav.isStopped = true;
            ani.SetBool("Idle", true);
        }
    }
}
