using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : BasePanel
{
    Text message;
    private void Start()
    {
        message = transform.Find("messageText").GetComponent<Text>();
        EventManager.AddEvents<DisplayMessageEvent>(ShowMessage);
    }
    public void ShowMessage(DisplayMessageEvent evt)
    {
        StopCoroutine(TextFade());
        message.color = Color.white;
        message.text = evt.msg;
        StartCoroutine(TextFade());
    }
    public override void OnEnter()
    {
    }
    public override void OnExit()
    {
    }
    public override void OnPause()
    {
    }
    public override void OnResume()
    {
    }
    IEnumerator TextFade()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        while (message.color.a > 0)
        {
            message.color -= new Color(0 , 0 , 0 , 0.1f);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener<DisplayMessageEvent>(ShowMessage);
    }
}
