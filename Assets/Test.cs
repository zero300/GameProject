using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine( A() );   
    }
    IEnumerator A()
    {
        var b = StartCoroutine(B());
        var c = StartCoroutine(C());

        Debug.Log("B Start");
        yield return b;
        Debug.Log("B End");
        Debug.Log("C Start");
        yield return c;
        Debug.Log("C End");
    }
    IEnumerator B()
    {
        for(int i = 0; i < 5; i++)
        {
            yield return new WaitForSecondsRealtime(1);
            Debug.Log($"B°õ¦æ , {i}");
        }
    }
    IEnumerator C()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSecondsRealtime(1);
            Debug.Log($"C°õ¦æ , {i}");
        }
    }
}
