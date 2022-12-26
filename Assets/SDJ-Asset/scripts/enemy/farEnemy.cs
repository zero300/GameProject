using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class farEnemy : MonoBehaviour
{
    public float alertRange;
    public LayerMask groundLayer;
    public Vector3 initialPos;

    public Animator ani;
    public NavMeshAgent nav;
    public GameObject Player;
    public GameObject rock;
    public float attackCooldown;

    /// <summary>
    /// ĵ�ٵ{�� , 3������
    /// </summary>
    [SerializeField]private int _wakeUpCount = 0;

    private Vector3 VoiceTarget;

    AnimatorStateInfo state;
    Rigidbody rb;
    float attackTime = 0;

    /// <summary>
    /// �O�_�B��l�v���a��
    /// </summary>
    [SerializeField] private bool isTracing;
    [SerializeField] private bool isTraceVoice;

    void Start()
    {
        EventManager.AddEvents<MakeSoundEvent>(VoiceDistanceAndAtk);

        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        initialPos = transform.position;
    }


    void Update()
    {
        if (isTracing) StartTracingPlayer();
        else
        {
            if (_wakeUpCount <= 2) return;
            if(Vector3.Distance(Player.transform.position, transform.position) < alertRange)
            {
                FindThePlayer();
            }
        }
    }

    /// <summary>
    /// �q�L���쪺�n���A�P�_�쩳�n���n�l�W�h�C
    /// </summary>
    private void VoiceDistanceAndAtk(MakeSoundEvent evt)
    {
        // �w�g�b�l�v���a  �ҥH�L���n��
        if (isTracing) return;

        float alert = evt.volumn <= SoundVolumn.Medium ? alertRange : alertRange * 2;

        // �Z���~
        float distance = Vector3.Distance(transform.position, evt.MakeSoundPos);
        if (distance > alert)
        {
            Debug.Log("�Z���~");
            return;
        }

        // �Z����  �ҥH�վ㬰 �n���o�X���a�I
        VoiceTarget = evt.MakeSoundPos;
        nav.destination = evt.MakeSoundPos;
        nav.isStopped = true;
        _wakeUpCount++;

        // ���P�_�������S����
        if (!nav.hasPath)
        {
            Debug.Log("�S�����i�H��F");
            return;
        }

        // �p�G�w�g�b�l���n���F �N���ΦA�ҰʤF
        if (!isTraceVoice) StartCoroutine(GoToVoicePos());

        isTraceVoice = true;
        // �i�H��F�N�L�h
        nav.isStopped = false;
    }
    /// <summary>
    /// �e���n���ӷ��B���L�{
    /// </summary>
    /// <returns></returns>
    IEnumerator GoToVoicePos()
    {
        // �����Ǫ���F�n����m
        yield return new WaitUntil( ()=> transform.position == VoiceTarget);
        // ��F�w�I���X�� �A�^�h�쥻����m
        yield return new WaitForSecondsRealtime(3f);
        nav.destination = initialPos;
        isTraceVoice = false;
    }
    /// <summary>
    /// ��o�{���a��
    /// </summary>
    private void FindThePlayer()
    {
        // ��쪱�a�e �A�e���n���B �h ����Coroutine
        if (isTraceVoice) StopCoroutine(GoToVoicePos());
        isTraceVoice = false;
        isTracing = true;

        nav.destination = Player.transform.position;
        nav.isStopped = false;
    }
    /// <summary>
    /// �}�l�l�ܪ��a
    /// </summary>
    private void StartTracingPlayer()
    {

        state = ani.GetCurrentAnimatorStateInfo(0);
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (state.IsName("RunFWD") && ani.GetBool("Attack"))
        {
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

        if (Vector3.Distance(Player.transform.position, transform.position) < alertRange)
        {
            nav.SetDestination(transform.position);
            nav.isStopped = true;
            ani.SetBool("Attack", true);
            attackTime = -attackCooldown;
        }
        else if (Vector3.Distance(Player.transform.position, transform.position) < 2 * alertRange)
        {
            if(!nav.hasPath) LosePlayerPos();
            nav.isStopped = false;
            ani.SetBool("Walk", true);
            if (state.IsName("WalkFWD"))
            {
                nav.SetDestination(Player.transform.position);
            }
        }
        else
        {
            LosePlayerPos();
        }
    }
    /// <summary>
    /// �ᥢ���a����, �^����
    /// </summary>
    private void LosePlayerPos()
    {
        isTracing = false;
        nav.destination = initialPos;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<MakeSoundEvent>(VoiceDistanceAndAtk);
    }
}
