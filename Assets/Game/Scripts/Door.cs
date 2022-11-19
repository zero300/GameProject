using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour ,  IInteract
{
    [Tooltip("�}���t��")]
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
    /// ����
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

    //�ɭ�
    public void Interact()
    {
        if (isMove || isLock) return;

        StartCoroutine(OpenAndCloseDoor());
    }
    /// <summary>
    /// �}�� ���� �M������
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
        Debug.Log("�}������");
        // �T�O���n���W��}�� �N���W�����C
        yield return new WaitForSecondsRealtime(2f);
        Debug.Log("���ݵ���");
        // ���쩳�U�S���H���ɭ� �N���� �A�A���i�� �٬O������
        yield return new WaitUntil( () => !Physics.CheckBox(transform.position + Vector3.up * 2.0f, new Vector3(scaleOfDoor, 2.0f, 1.0f), Quaternion.identity, playerLayer));
        // ����
        Debug.Log("�P�_����");
        while (currentPos < scaleOfDoor)
        {
            currentPos += openSpeed;
            rightDoor.position += -transform.right * openSpeed;
            leftDoor.position += transform.right * openSpeed;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        Debug.Log("��������");
        currentPos = scaleOfDoor;
        isMove = false;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<UnlockEvent>(Unlock);
    }
}
