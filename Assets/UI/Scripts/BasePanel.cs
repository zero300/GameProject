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
    /// �ɭ���ܥX��
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// �����Ȱ�
    /// </summary>
    public abstract void OnPause();

    /// <summary>
    /// �����~��
    /// </summary>
    public abstract void OnResume();

    /// <summary>
    /// �����h�X , ���������
    /// </summary>
    public abstract void OnExit();
}
