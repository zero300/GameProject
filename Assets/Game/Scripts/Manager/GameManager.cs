using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager :ManagerBase
{
    public GameManager(GameFacade facade)
    {
        this.facade = facade;
    }
    private Vector3 SavePoint;

    public override void InitManager()
    {
        Debug.Log("Initial GameManager");
    }


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

    
    public override void UpdateManager()
    {
    }

    public override void DestroyManager()
    {
        
    }
}
