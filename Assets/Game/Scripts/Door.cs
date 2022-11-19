using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour ,  IInteract
{
    [Tooltip("開關速度")]
    public float openSpeed;
    public LayerMask playerLayer;
    private float scaleOfDoor;
    private float currentPos;

    
    private Transform rightDoor;
    private Transform leftDoor;
    private bool isMove;
    [SerializeField] private int keyCode = 0;
    [SerializeField] private bool isLock;

    private void Awake()
    {
        rightDoor = transform.Find("Right");
        leftDoor = transform.Find("Left");
        scaleOfDoor = rightDoor.localScale.x;
        currentPos = scaleOfDoor;
        isMove = false;

        if (isLock) EventManager.AddEvents<UnlockEvent>(Unlock);

        
    }
    
    /// <summary>
    /// 解鎖
    /// </summary>
    /// <param name="evt"></param>
    private void Unlock(GameEvent evt)
    {
        if (evt is not UnlockEvent) return;


        if(isLock && keyCode == (evt as UnlockEvent).KeyCode)
        {

            isLock = false;
        }
    }

    //界面
    public void Interact()
    {
        if (isMove || isLock) return;

        StartCoroutine(OpenAndCloseDoor());
    }
    /// <summary>
    /// 開啟 等待 然後關閉
    /// </summary>
    /// <returns></returns>
    IEnumerator OpenAndCloseDoor()
    {
        isMove = true;
        while (currentPos > 0)
        {
            currentPos -= openSpeed;
            rightDoor.position += transform.right * openSpeed;
            leftDoor.position += -transform.right * openSpeed;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        currentPos = 0;
        Debug.Log("開門結束");
        // 確保不要馬上剛開完 就馬上關門。
        yield return new WaitForSecondsRealtime(2f);
        Debug.Log("等待結束");
        // 等到底下沒有人的時候 就關門 ，再走進來 還是直接關
        yield return new WaitUntil( () => !Physics.CheckBox(transform.position + Vector3.up * 2.0f, new Vector3(scaleOfDoor, 2.0f, 1.0f), Quaternion.identity, playerLayer));
        // 關門
        Debug.Log("判斷結束");
        while (currentPos < scaleOfDoor)
        {
            currentPos += openSpeed;
            rightDoor.position += -transform.right * openSpeed;
            leftDoor.position += transform.right * openSpeed;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        Debug.Log("關門結束");
        currentPos = scaleOfDoor;
        isMove = false;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<UnlockEvent>(Unlock);
    }
}
