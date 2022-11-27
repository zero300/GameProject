using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameFacade : MonoBehaviour
{
    //單例
    private static GameFacade _instance;
    public static GameFacade Instance{ get { return _instance; } }
    
    private AudioManager _audioManager;
    private GameManager _gameManager;
    private UIManager _uimanager;
    private TheSceneManager _sceneManager;

    // 這裡控制單例
    private void Awake()
    {
        if(_instance != null)
            Destroy(_instance);
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    { 
        InitManagers();
    } 
    
    void Update()
    {
        //UpdateManagers();
    }

    private void OnDestroy()
    {
        DestroyManagers();
    }
    /// <summary>
    /// 初始化Manager
    /// </summary>
    private void InitManagers()
    {
        _audioManager = new AudioManager(this);
        _gameManager = new GameManager(this);
        _uimanager = new UIManager(this);
        _sceneManager = new TheSceneManager(this);


        _audioManager.InitManager();
        _gameManager.InitManager();
        _uimanager.InitManager();
        _sceneManager.InitManager();
    }
    /// <summary>
    /// 每幀更新Managers
    /// </summary>
    private void UpdateManagers()
    {
        _audioManager.UpdateManager();
        _gameManager.UpdateManager();
        _uimanager.UpdateManager();
        _sceneManager.UpdateManager();
    }
    /// <summary>
    /// 摧毀所有Managers
    /// </summary>
    private void DestroyManagers()
    {
        _audioManager.DestroyManager();
        _gameManager.DestroyManager();
        _uimanager.DestroyManager();
        _sceneManager.DestroyManager();
    }


    #region 傳遞給其底下的Manager

    #region TheSceneManager
    /// <summary>
    /// 載入場景
    /// </summary>
    /// <param name="index"></param>
    public void LoadScene(SceneIndex index)
    {
        _uimanager.SceneChangedClearUI();
        _sceneManager.LoadScene(index);
    }
    /// <summary>
    /// 增加載入場景後的action
    /// </summary>
    /// <param name="action"></param>
    public void AddActionAfterSceneLoad(Action action)
    {
        _sceneManager.AddActionAfterSceneLoad(action);
    }
    /// <summary>
    /// 刪除載入場景後的action
    /// </summary>
    /// <param name="action"></param>
    public void RemoveActionAfterSceneLoad(Action action)
    {
        _sceneManager.RemoveActionAfterSceneLoad(action);
    }
    #endregion

    #endregion
}
