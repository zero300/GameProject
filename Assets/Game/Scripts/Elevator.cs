using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour , IInteract
{
    public float moveSmooth;
    private Vector3[] Pos;
    private Transform elevatorBase;
    public Light uplight;
    public Light downlight;
    public GameObject SoundEffect;
    private int nextPos;
    private bool isMove;
    private void Awake()
    {
        isMove = false;
        SoundEffect.SetActive(false);
        elevatorBase = transform.Find("ElevatorBase");
        uplight = elevatorBase.transform.Find("uplight").GetComponent<Light>();
        downlight = elevatorBase.transform.Find("downlight").GetComponent<Light>();
        TurnOffLight();
        
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
        SoundEffect.SetActive(true);
        Vector3 next = Pos[nextPos];
        TurnOnLight();
        yield return new WaitForSecondsRealtime(1.0f);
        while (elevatorBase.position != next)
        {
            elevatorBase.position = Vector3.MoveTowards(elevatorBase.position , next , moveSmooth);
            yield return new WaitForSecondsRealtime(.05f);
        }
        nextPos++;
        if (nextPos >= Pos.Length)
            nextPos = 0;
        TurnOffLight();
        isMove = false;
        SoundEffect.SetActive(false);
    }

    private void TurnOnLight()
    {
        uplight.enabled = true;
        downlight.enabled = true;
    }
    private void TurnOffLight()
    {
        uplight.enabled = false;
        downlight.enabled = false;
    }

}
