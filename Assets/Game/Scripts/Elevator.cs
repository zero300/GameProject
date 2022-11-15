using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour , IInteract
{
    public float moveSmooth;
    private Vector3[] Pos;
    private Transform elevatorBase;
    private int nextPos;
    private bool isMove;
    private void Awake()
    {
        isMove = false;
        elevatorBase = transform.Find("ElevatorBase");
        Transform AllPos = transform.Find("MovePos");
        Pos = new Vector3[AllPos.childCount];
        for (int i = 0; i < AllPos.childCount; i++)
        {
            Pos[i] = AllPos.GetChild(i).position;
        }
        nextPos = 1;
    }

    public void Interact()
    {
        if (isMove) return;

        StartCoroutine(ElevatorMove());

    }
    IEnumerator ElevatorMove()
    {
        isMove = true;
        Vector3 next = Pos[nextPos];
        // �n���U�����V�W �Ӥ��Τ����A �i��b���ӹq��Ұʪ��n��
        yield return new WaitForSecondsRealtime(1.0f);
        while (elevatorBase.position != next)
        {
            elevatorBase.position = Vector3.MoveTowards(elevatorBase.position , next , moveSmooth);
            yield return new WaitForSecondsRealtime(.05f);
        }
        nextPos++;
        if (nextPos >= Pos.Length)
            nextPos = 0;
        isMove = false;
    }
}
