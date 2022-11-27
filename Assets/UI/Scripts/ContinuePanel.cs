using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinuePanel : BasePanel
{
    private Button ContinueButton;
    private Button ExitButton;

    void Start()
    {
        ContinueButton = transform.Find("ContinueButton").GetComponent<Button>();
        ExitButton = transform.Find("ExitButton").GetComponent<Button>();

        ContinueButton.onClick.AddListener(ContinueButtonCallback);
        ExitButton.onClick.AddListener(ExitButtonCallback);
    }


    private void ContinueButtonCallback()
    {
        facade.AddActionAfterSceneLoad(PushLevel1NeedPanel);
        facade.LoadScene(SceneIndex.Level1);
    }
    private void ExitButtonCallback()
    {
        facade.AddActionAfterSceneLoad(PushTitlePanel);
        facade.LoadScene(SceneIndex.Title);
    }

    private void PushTitlePanel()
    {
        uimanager.PushPanel(UIPanelType.TitlePanel);
    }
    private void PushLevel1NeedPanel()
    {
        Debug.Log("Level1 Need");
    }

    public override void OnEnter()
    {
    }
    public override void OnPause()
    {
    }
    public override void OnResume()
    {
    }
    public override void OnExit()
    {
    }
}
