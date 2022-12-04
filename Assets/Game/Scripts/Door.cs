using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������
/// </summary>
public enum LockType
{
    KeyLock, // �ݭnĲ�I���_��
    NotLock  // �S�����
}
/// <summary>
/// �����`��
/// </summary>
public enum DoorType
{
    OneWay,//��V
    TwoWay,//���V
    PointDoor, // �s���I�� , �L�k�^�Y�C �}�ҫ�|���W����
}


public class Door : MonoBehaviour ,  IInteract
{
    #region �Ѽ�
    
    [Header("��")]
    [Tooltip("��������")]
    public DoorType doortype;
    [Tooltip("�O�_���A�H��������")]
    public LockType lockType;
    public GameObject SoundEffect; //Drag SoundPrefab here
    
    
    [Header("��l�Ѽ�")]
    [Tooltip("�}���t��")]
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

        // �O�_���
        switch (lockType) { // ���F�S�� �N�⦳��L�Ҧ����� ���ӳ������F
            case LockType.NotLock:
                isLock = false;
                break;
            default:
                isLock = true;
                EventManager.AddEvents<UnlockEvent>(Unlock);
                break;
            
        }
        // �ھڪ������� ���վ�
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
    /// ����
    /// </summary>
    /// <param name="evt"></param>
    private void Unlock(GameEvent evt)
    {
        if (evt is not UnlockEvent) return;

        if(isLock && keyCode == (evt as UnlockEvent).KeyCode)
            isLock = false;
    }

    // �椬Interface
    public void Interact()
    {
        if (isMove || isLock) return;
        if (canSave) Debug.Log("TODO : �O�o���s�ɣ�����");
        Instantiate(SoundEffect, this.transform.position, this.transform.rotation); //Create sound prefab
        StartCoroutine(OpenAndCloseDoor());
    }
    /// <summary>
    /// �}�� ���� �M������
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
        // �T�O���n���W��}�� �N���W�����C
        light.SetActive(false);
        yield return new WaitForSecondsRealtime(2f);
        // ���쩳�U�S���H���ɭ� �N���� �A�A���i�� �٬O������
        yield return new WaitUntil( () => !Physics.CheckBox(transform.position + Vector3.up * 2.0f, new Vector3(scaleOfDoor, 2.0f, 1.0f), Quaternion.identity, playerLayer));
        // ����
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
