using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Vector3 SavePoint;


    /// <summary>
    /// �Ȱ��C��
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    /// <summary>
    /// �~��C��
    /// </summary>
    public void ContinueGame()
    {
        Time.timeScale = 1;
    }
}
