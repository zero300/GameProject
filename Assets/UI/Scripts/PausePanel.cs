using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : BasePanel
{

    Button ContinueButton;
    Button ExitButton;
    Toggle GamemodeToggle;

    // Start is called before the first frame update
    void Start()
    {
        Transform ground = transform.Find("Ground");
        ContinueButton = ground.Find("ContinueButton").GetComponent<Button>();
        ExitButton = ground.Find("ExitButton").GetComponent<Button>();
        GamemodeToggle = ground.Find("GameModeCheckbox").GetComponent<Toggle>();

        ContinueButton.onClick.AddListener(ContinueButtonCallback);
        ExitButton.onClick.AddListener(ExitButtonCallback);
        GamemodeToggle.onValueChanged.AddListener(GamemodeToggleCallback);
    }

    private void ContinueButtonCallback()
    {
        uimanager.PopPanel();
    }
    private void ExitButtonCallback()
    {
        // TODO : 轉移到Title 或是直接關閉遊戲
        facade.LoadScene(SceneIndex.Title);
    }
    private void GamemodeToggleCallback(bool value)
    {
        Debug.Log("確認value = " + value);
        facade.ChangeLight(value);
    }


    public override void OnEnter()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        facade.PauseGame();
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        facade.ContinueGame();
        gameObject.SetActive(false);
    }

    public override void OnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        facade.ContinueGame();
        gameObject.SetActive(false);
    }

    public override void OnResume()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        facade.PauseGame();
        gameObject.SetActive(true);
    }
}
