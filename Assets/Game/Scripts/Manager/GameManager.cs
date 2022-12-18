using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RestartMode
{
    /// <summary>
    /// 從頭開始
    /// </summary>
    FromScratch,
    /// <summary>
    /// 從存檔點開始
    /// </summary>
    FromCheckPoint
}
public class GameManager :ManagerBase
{
    
    public GameManager(GameFacade facade)
    {
        this.facade = facade;
    }
    // Player 
    private Transform PlayerTransform;
    private Vector3 SavePoint;
    private PlayerProperty playerProperty;
    
    private bool _gameModeIsEasy = false;
    private Light _directionalLight;

    /// <summary>
    /// 獲取PlayerProperty
    /// </summary>
    /// <returns></returns>
    public PlayerProperty GetPlayerProperty() => playerProperty;
    /// <summary>
    /// 獲取關卡模式
    /// </summary>
    /// <returns></returns>
    public bool GetisEasy() => _gameModeIsEasy;

    public override void InitManager()
    {
        playerProperty = ScriptableObject.CreateInstance<PlayerProperty>();
        EventManager.AddEvents<CheckpointEvent>(SetSavePoint);
    }

    /// <summary>
    /// 暫停遊戲
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    /// <summary>
    /// 繼續遊戲
    /// </summary>
    public void ContinueGame()
    {
        Time.timeScale = 1;
    }
    /// <summary>
    /// 更改遊戲難度(簡單、困難)
    /// </summary>
    public void ChangeGameMode(bool isEasy)
    {
        _gameModeIsEasy = isEasy;
        ChangeGlobalLight();
    }
    /// <summary>
    /// 更改全局光照
    /// </summary>
    private void ChangeGlobalLight()
    {
        if (_directionalLight == null)
        {
            _directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
            if (_directionalLight == null)
            {
                GameObject lightobj = new GameObject("Direction Light");
                _directionalLight = lightobj.AddComponent<Light>();
                _directionalLight.enabled = true;
                _directionalLight.type = LightType.Directional;
                _directionalLight.shadows = LightShadows.Hard;
                _directionalLight.color = Color.black;
            }
        }
        if (_gameModeIsEasy) _directionalLight.color = new Color(0.1f, 0.1f, 0.1f);
        else _directionalLight.color = Color.black;
    }
    /// <summary>
    /// 重新開始遊戲 但會根據存檔點位置復活
    /// </summary>
    public void RestartGame(RestartMode mode , SceneIndex index)
    {
        if(mode == RestartMode.FromCheckPoint) facade.AddActionAfterSceneLoad(InitPlayerPosWithSavePoint);

        facade.AddActionAfterSceneLoad(ChangeGlobalLight);
        facade.LoadScene(index);
    }
    /// <summary>
    /// 根據存檔點 初始化角色位置
    /// </summary>
    private void InitPlayerPosWithSavePoint()
    {
        if (PlayerTransform == null) PlayerTransform = GameObject.Find("Player").transform;
        PlayerTransform.position = SavePoint;
    }
    /// <summary>
    /// 設置存檔點
    /// </summary>
    /// <param name="point"></param>
    public void SetSavePoint(CheckpointEvent evt)
    {
        SavePoint = evt.checkpoint;
    }

    public override void UpdateManager()
    {
    }

    public override void DestroyManager()
    {
        EventManager.RemoveListener<CheckpointEvent>(SetSavePoint);
    }
}
