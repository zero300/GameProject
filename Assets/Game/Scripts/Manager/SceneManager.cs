using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex
{
    Title,
    Demo,
    Level1,
}
public class TheSceneManager : ManagerBase
{
    private SceneIndex _currentScene;
    public SceneIndex CurrentScene { get { return _currentScene; } }

    private Action aftersceneload;
    // GameFacade facade
    public TheSceneManager(GameFacade facade)
    {
        this.facade = facade;
    }
    /// <summary>
    /// ���J����
    /// </summary>
    /// <param name="index"></param>
    public void LoadScene(SceneIndex index)
    {
        _currentScene = index;
        SceneManager.LoadScene( (int)index );
    }
    /// <summary>
    /// ��
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void SceneLoaded(Scene scene , LoadSceneMode mode)
    {
        aftersceneload?.Invoke();
        aftersceneload = null;
    }
    /// <summary>
    /// �W�[action ���J������
    /// </summary>
    /// <param name="action"></param>
    public void AddActionAfterSceneLoad(Action action)
    {
        aftersceneload += action;
    }
    /// <summary>
    /// ����Action �A���J������
    /// Ĳ�o�N�|��null�A�����e�����b�ϥ�
    /// </summary>
    /// <param name="action"></param>
    public void RemoveActionAfterSceneLoad(Action action)
    {
        aftersceneload -= action;
    }



    public override void InitManager()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        Debug.Log("Initial SceneManager");
    }
    public override void UpdateManager()
    {
    }
    public override void DestroyManager()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        Debug.Log("Destroy SceneManager");
    }
}
