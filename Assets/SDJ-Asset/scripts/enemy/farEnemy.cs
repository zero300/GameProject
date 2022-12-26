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
    /// 警戒程度 , 3為滿值
    /// </summary>
    [SerializeField]private int _wakeUpCount = 0;

    private Vector3 VoiceTarget;

    AnimatorStateInfo state;
    Rigidbody rb;
    float attackTime = 0;

    /// <summary>
    /// 是否處於追逐玩家中
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
    /// 通過收到的聲音，判斷到底要不要追上去。
    /// </summary>
    private void VoiceDistanceAndAtk(MakeSoundEvent evt)
    {
        // 已經在追逐玩家  所以無視聲音
        if (isTracing) return;

        float alert = evt.volumn <= SoundVolumn.Medium ? alertRange : alertRange * 2;

        // 距離外
        float distance = Vector3.Distance(transform.position, evt.MakeSoundPos);
        if (distance > alert)
        {
            Debug.Log("距離外");
            return;
        }

        // 距離內  所以調整為 聲音發出的地點
        VoiceTarget = evt.MakeSoundPos;
        nav.destination = evt.MakeSoundPos;
        nav.isStopped = true;
        _wakeUpCount++;

        // 先判斷中間有沒有路
        if (!nav.hasPath)
        {
            Debug.Log("沒有路可以到達");
            return;
        }

        // 如果已經在追蹤聲音了 就不用再啟動了
        if (!isTraceVoice) StartCoroutine(GoToVoicePos());

        isTraceVoice = true;
        // 可以到達就過去
        nav.isStopped = false;
    }
    /// <summary>
    /// 前往聲音來源處的過程
    /// </summary>
    /// <returns></returns>
    IEnumerator GoToVoicePos()
    {
        // 先等怪物到達聲音位置
        yield return new WaitUntil( ()=> transform.position == VoiceTarget);
        // 到達定點等幾秒 再回去原本的位置
        yield return new WaitForSecondsRealtime(3f);
        nav.destination = initialPos;
        isTraceVoice = false;
    }
    /// <summary>
    /// 當發現玩家後
    /// </summary>
    private void FindThePlayer()
    {
        // 找到玩家前 再前往聲音處 則 關閉Coroutine
        if (isTraceVoice) StopCoroutine(GoToVoicePos());
        isTraceVoice = false;
        isTracing = true;

        nav.destination = Player.transform.position;
        nav.isStopped = false;
    }
    /// <summary>
    /// 開始追蹤玩家
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
    /// 丟失玩家視野, 回到原位
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
