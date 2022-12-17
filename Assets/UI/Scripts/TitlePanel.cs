using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : BasePanel
{
    private Button startButton;
    private Button exitButton;
    private Button easyModeButton;
    private Button blindModeButton;
    private bool isEasy = false;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        startButton = transform.Find("StartButton").GetComponent<Button>();
        exitButton = transform.Find("ExitButton").GetComponent<Button>();
        easyModeButton = transform.Find("EasyModeButton").GetComponent<Button>();
        blindModeButton = transform.Find("BlindModeButton").GetComponent<Button>();



        startButton.onClick.AddListener(StartButtonCallback);
        exitButton.onClick.AddListener(ExitButtonCallback);
        easyModeButton.onClick.AddListener(EasyModeButtonCallback);
        blindModeButton.onClick.AddListener(BlindModeButtonCallback);
    }
    private void StartButtonCallback()
    {
        //facade.AddActionAfterSceneLoad(PushDemoNeedPanel);
        easyModeButton.gameObject.SetActive(true);
        blindModeButton.gameObject.SetActive(true);
    }
    private void ExitButtonCallback()
    {
        Application.Quit();
    }
    private void EasyModeButtonCallback()
    {
        isEasy = true;
        facade.AddActionAfterSceneLoad(ChooseMode);
        facade.LoadScene(SceneIndex.Level0);
    }
    private void BlindModeButtonCallback()
    {
        isEasy = false;
        facade.AddActionAfterSceneLoad(ChooseMode);
        facade.LoadScene(SceneIndex.Level0);
    }

    private void ChooseMode()
    {
        facade.ChangeLight(isEasy);
    }
    private void PushDemoNeedPanel()
    {
        uimanager.PushPanel(UIPanelType.ContinuePanel);
    }


    public override void OnEnter()
    {
        easyModeButton.gameObject.SetActive(false);
        blindModeButton.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
    public override void OnPause()
    {
        gameObject.SetActive(false);
    }
    public override void OnResume()
    {
        easyModeButton.gameObject.SetActive(false);
        blindModeButton.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
}
