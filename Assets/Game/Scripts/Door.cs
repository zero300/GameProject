using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鎖住的種類
/// </summary>
public enum LockType
{
    KeyLock, // 需要觸碰到鑰匙
    NotLock  // 沒有鎖住
}
/// <summary>
/// 門的總類
/// </summary>
public enum DoorType
{
    OneWay,//單向
    TwoWay,//雙向
    PointDoor, // 存檔點門 , 無法回頭。 開啟後會馬上關閉
}


public class Door : MonoBehaviour ,  IInteract
{
    #region 參數
    
    [Header("門")]
    [Tooltip("門的種類")]
    public DoorType doortype;
    [Tooltip("是否鎖住，以及鎖住種類")]
    public LockType lockType;
    public GameObject SoundEffect; //Drag SoundPrefab here
    
    
    [Header("其餘參數")]
    [Tooltip("開關速度")]
    public float openSpeed;
    public LayerMask playerLayer;
    private float scaleOfDoor;
    private float currentPos;

    private new GameObject light;
    private Transform rightDoor;
    private Transform leftDoor;
    private new BoxCollider collider;
    private bool isMove;
    [SerializeField] private bool isLock = false;
    private bool canSave = false;
    [SerializeField] private int keyCode = 0;
    
    #endregion

    private void Awake()
    {
        light = transform.Find("light").gameObject;
        light.SetActive(false);
        rightDoor = transform.Find("Right");
        leftDoor = transform.Find("Left");
        collider = GetComponent<BoxCollider>();
        scaleOfDoor = rightDoor.localScale.x;
        currentPos = scaleOfDoor;
        isMove = false;

        // 是否鎖住
        switch (lockType) { // 除了沒鎖 就算有其他模式的鎖 應該都算鎖住了
            case LockType.NotLock:
                isLock = false;
                break;
            default:
                isLock = true;
                EventManager.AddEvents<UnlockEvent>(Unlock);
                break;
            
        }
        // 根據門的種類 做調整
        switch (doortype)
        {
            case DoorType.TwoWay:
                collider.size = new Vector3(collider.size.x , collider.size.y , 2);
                break;
            case DoorType.OneWay:
                collider.size = new Vector3(collider.size.x, collider.size.y, 1);
                collider.center = new Vector3(collider.center.x, collider.center.y , 0.5f);
                break;
            case DoorType.PointDoor:
                collider.size = new Vector3(collider.size.x, collider.size.y, 1);
                collider.center = new Vector3(collider.center.x, collider.center.y, 0.5f);
                canSave = true;
                break;
        }
        
    }
    
    /// <summary>
    /// 解鎖
    /// </summary>
    /// <param name="evt"></param>
    private void Unlock(GameEvent evt)
    {
        if (evt is not UnlockEvent) return;

        if(isLock && keyCode == (evt as UnlockEvent).KeyCode)
            isLock = false;
    }

    // 交互Interface
    public void Interact()
    {
        if (isMove || isLock) return;
        if (canSave) Debug.Log("TODO : 記得做存檔ˊˇˋ");
        Instantiate(SoundEffect, this.transform.position, this.transform.rotation); //Create sound prefab
        StartCoroutine(OpenAndCloseDoor());
    }
    /// <summary>
    /// 開啟 等待 然後關閉
    /// </summary>
    /// <returns></returns>
    IEnumerator OpenAndCloseDoor()
    {
        isMove = true;
        light.SetActive(true);
        
        while (currentPos > 0)
        {
            currentPos -= openSpeed;
            rightDoor.position += transform.right * openSpeed;
            leftDoor.position += -transform.right * openSpeed;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        currentPos = 0;
        // 確保不要馬上剛開完 就馬上關門。
        light.SetActive(false);
        yield return new WaitForSecondsRealtime(2f);
        // 等到底下沒有人的時候 就關門 ，再走進來 還是直接關
        yield return new WaitUntil( () => !Physics.CheckBox(transform.position + Vector3.up * 2.0f, new Vector3(scaleOfDoor, 2.0f, 1.0f), Quaternion.identity, playerLayer));
        // 關門
        light.SetActive(true);
        while (currentPos < scaleOfDoor)
        {
            currentPos += openSpeed;
            rightDoor.position += -transform.right * openSpeed;
            leftDoor.position += transform.right * openSpeed;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        currentPos = scaleOfDoor;
        isMove = false;
        light.SetActive(false);
    }

    private void OnDestroy()
    {
        if(lockType != LockType.NotLock )
            EventManager.RemoveListener<UnlockEvent>(Unlock);
    }
}
