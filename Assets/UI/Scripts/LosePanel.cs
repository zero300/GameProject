using UnityEngine;
using UnityEngine.UI;

public class LosePanel : BasePanel
{
    private Button RestartButton;
    private Button ExitButton;

    void Start()
    {
        RestartButton = transform.Find("RestartButton").GetComponent<Button>();
        ExitButton = transform.Find("ExitButton").GetComponent<Button>();

        RestartButton.onClick.AddListener(RestartButtonCallback);
        ExitButton.onClick.AddListener(ExitButtonCallback);
    }

    private void RestartButtonCallback()
    {
        facade.RestartGame( RestartMode.FromCheckPoint , facade.GetCurrentScene() );
    }
    private void ExitButtonCallback()
    {
        facade.LoadScene(SceneIndex.Title);
    }

    public override void OnEnter()
    {
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
