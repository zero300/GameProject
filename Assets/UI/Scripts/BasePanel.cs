using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    protected UIManager uimanager;
    public UIManager UIManager
    {
        set { uimanager = value; }
    }
    protected GameFacade facade;
    public GameFacade GameFacade
    {
        set { facade = value; }
    }
    /// <summary>
    /// 界面顯示出來
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// 介面暫停
    /// </summary>
    public abstract void OnPause();

    /// <summary>
    /// 介面繼續
    /// </summary>
    public abstract void OnResume();

    /// <summary>
    /// 介面退出 , 介面不顯示
    /// </summary>
    public abstract void OnExit();
}
