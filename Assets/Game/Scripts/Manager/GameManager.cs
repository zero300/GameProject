using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Vector3 SavePoint;


    /// <summary>
    /// ¼È°±¹CÀ¸
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    /// <summary>
    /// Ä~Äò¹CÀ¸
    /// </summary>
    public void ContinueGame()
    {
        Time.timeScale = 1;
    }
}
