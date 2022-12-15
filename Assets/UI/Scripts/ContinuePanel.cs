using System;
using UnityEngine;
using UnityEngine.UI;

public class ContinuePanel : BasePanel
{
    private Button ContinueButton;
    private Button RestartButton;
    private Button ExitButton;
    void Awake()
    {
        ContinueButton = transform.Find("ContinueButton").GetComponent<Button>();
        RestartButton = transform.Find("RestartButton").GetComponent<Button>();
        ExitButton = transform.Find("ExitButton").GetComponent<Button>();

        ContinueButton.onClick.AddListener(ContinueButtonCallback);
        RestartButton.onClick.AddListener(RestartButtonCallback);
        ExitButton.onClick.AddListener(ExitButtonCallback);
    }

    // NextStage
    private void ContinueButtonCallback()
    {
        //facade.AddActionAfterSceneLoad(PushLevel1NeedPanel); 
        facade.RestartGame(RestartMode.FromScratch, facade.GetCurrentScene() + 1);       
    }
    // is win so init
    private void RestartButtonCallback()
    {
        facade.RestartGame(RestartMode.FromScratch, facade.GetCurrentScene());
    }
    private void ExitButtonCallback()
    {
        facade.LoadScene(SceneIndex.Title);
    }
    private void PushLevel1NeedPanel()
    {
        Debug.Log("Level1 Need");
    }
    public override void OnEnter()
    {
        if (Enum.GetName(typeof(SceneIndex), facade.GetCurrentScene() + 1) == null) ContinueButton.enabled = false;
        else ContinueButton.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.SetActive(true);
    }
    public override void OnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
    }
    public override void OnResume()
    {
        if (Enum.GetName(typeof(SceneIndex), facade.GetCurrentScene() + 1) == null) ContinueButton.enabled = false;
        else ContinueButton.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.SetActive(true);
    }
    public override void OnExit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
    }
}
