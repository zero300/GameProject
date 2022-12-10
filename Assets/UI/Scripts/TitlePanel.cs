using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : BasePanel
{
    private Button startButton;
    private Button exitButton;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        startButton = transform.Find("StartButton").GetComponent<Button>();
        exitButton = transform.Find("ExitButton").GetComponent<Button>();

        startButton.onClick.AddListener(StartButtonCallback);
        exitButton.onClick.AddListener(ExitButtonCallback);
    }
    private void StartButtonCallback()
    {
        //facade.AddActionAfterSceneLoad(PushDemoNeedPanel);
        facade.LoadScene(SceneIndex.Demo);
    }
    private void ExitButtonCallback()
    {
        Application.Quit();
    }

    private void PushDemoNeedPanel()
    {
        uimanager.PushPanel(UIPanelType.ContinuePanel);
    }


    public override void OnEnter()
    {
        gameObject.SetActive(true);
    }
    public override void OnPause()
    {
        gameObject.SetActive(false);
    }
    public override void OnResume()
    {
        gameObject.SetActive(true);
    }
    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
}
