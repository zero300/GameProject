using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameFacade : MonoBehaviour
{
    //���
    private static GameFacade _instance;
    public static GameFacade Instance{  get  { return _instance; } }
    
    private AudioManager _audioManager;
    private GameManager _gameManager;
    private UIManager _uimanager;
    private TheSceneManager _sceneManager;

    // �o�̱�����
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
    /// ��l��Manager
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
    /// �C�V��sManagers
    /// </summary>
    private void UpdateManagers()
    {
        _audioManager.UpdateManager();
        _gameManager.UpdateManager();
        _uimanager.UpdateManager();
        _sceneManager.UpdateManager();
    }
    /// <summary>
    /// �R���Ҧ�Managers
    /// </summary>
    private void DestroyManagers()
    {
        _audioManager.DestroyManager();
        _gameManager.DestroyManager();
        _uimanager.DestroyManager();
        _sceneManager.DestroyManager();
    }


    #region �ǻ����䩳�U��Manager

    #region TheSceneManager
    public SceneIndex GetCurrentScene() => _sceneManager.CurrentScene;
    /// <summary>
    /// ���J����
    /// </summary>
    /// <param name="index"></param>
    public void LoadScene(SceneIndex index)
    {
        _uimanager.SceneChangedClearUI();
        _sceneManager.LoadScene(index);
    }
    /// <summary>
    /// �W�[���J�����᪺action
    /// </summary>
    /// <param name="action"></param>
    public void AddActionAfterSceneLoad(Action action)
    {
        _sceneManager.AddActionAfterSceneLoad(action);
    }
    /// <summary>
    /// �R�����J�����᪺action
    /// </summary>
    /// <param name="action"></param>
    public void RemoveActionAfterSceneLoad(Action action)
    {
        _sceneManager.RemoveActionAfterSceneLoad(action);
    }
    #endregion

    #region GameManager
    /// <summary>
    /// �Ȱ��C��
    /// </summary>
    public void PauseGame()
    {
        _gameManager.PauseGame();
    }
    /// <summary>
    /// �~��C��
    /// </summary>
    /// </summary>
    public void ContinueGame()
    {
        _gameManager.ContinueGame();
    }
    /// <summary>
    /// ���ܼҦ�
    /// </summary>
    /// <param name="isEasy"></param>
    public void ChangeLight(bool isEasy)
    {
        _gameManager.ChangeGameMode(isEasy);
    }
    /// <summary>
    /// ���s�}�l�C�� , �ھڦs���I
    /// </summary>
    /// <param name="index"></param>
    public void RestartGame(RestartMode mode, SceneIndex index)
    {
        _gameManager.RestartGame(mode , index);
    }
    /// <summary>
    /// ���PlayerProperty
    /// </summary>
    /// <returns></returns>
    public PlayerProperty GetPlayerProperty() => _gameManager.GetPlayerProperty();

    public bool GetisEasy() => _gameManager.GetisEasy();
    #endregion

    #region UIManager
    /// <summary>
    /// ���Xpanel
    /// </summary>
    /// <param name="type"></param>
    public void PushPanel(UIPanelType type)
    {
        _uimanager.PushPanel(type);
    }
    #endregion

    #endregion
}
