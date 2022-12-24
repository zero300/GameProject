using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class farEnemy : MonoBehaviour
{
    public float alertRange;
    public LayerMask groundLayer;

    public Animator ani;
    public NavMeshAgent nav;
    public GameObject Player;
    public GameObject rock;
    public float attackCooldown;

    AnimatorStateInfo state;
    Rigidbody rb;
    float attackTime = 0;

    /// <summary>
    /// �O�_�B��l�v���a��
    /// </summary>
    private bool isTracing;

    void Start()
    {
        EventManager.AddEvents<MakeSoundEvent>(VoiceDistanceAndAtk);

        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }


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

    /// <summary>
    /// �q�L���쪺�n���A�P�_�쩳�n���n�l�W�h�C
    /// </summary>
    private void VoiceDistanceAndAtk(MakeSoundEvent evt)
    {
        if (isTracing) return;

        float distance = Vector3.Distance(transform.position, evt.MakeSoundPos);
        if (distance > alertRange)
        {
            Debug.Log("�Z���~");
            return;
        }

        //�n���P�Ǫ����������
        if (Physics.Raycast(transform.position , evt.MakeSoundPos , distance
            , groundLayer , QueryTriggerInteraction.Ignore))
        {
            Debug.Log("���������");
            return;
        }


        nav.destination = evt.MakeSoundPos;
        nav.isStopped = false;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<MakeSoundEvent>(VoiceDistanceAndAtk);
    }
}
