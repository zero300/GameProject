using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : BasePanel
{
    private ColorChoosePanel colorChoosePanel;
    private PlayerProperty playerProperty;

    Button CloseButton;
    Button AcceptButton;
    Button HeadColorButton;
    Button EndColorButton;

    RawImage HeadColor;
    RawImage EndColor;

    void Awake()
    {
        CloseButton = transform.Find("CloseButton").GetComponent<Button>();
        AcceptButton = transform.Find("AcceptButton").GetComponent<Button>();
        HeadColorButton = transform.Find("HeadColorButton").GetComponent<Button>();
        EndColorButton = transform.Find("EndColorButton").GetComponent<Button>();

        HeadColor = HeadColorButton.GetComponent<RawImage>();
        EndColor = EndColorButton.GetComponent<RawImage>();

        HeadColorButton.onClick.AddListener(HeadColorButtonCallback);
        EndColorButton.onClick.AddListener(EndColorButtonCallback);
        CloseButton.onClick.AddListener(CloseButtonCallback);
        AcceptButton.onClick.AddListener(AcceptButtonCallback);
    }
    #region UI CallBack
   
    private void HeadColorButtonCallback()
    {
        if (colorChoosePanel == null)
        {
            colorChoosePanel = uimanager.GetPanel(UIPanelType.ColorChoosePanel).gameObject.GetComponent<ColorChoosePanel>();
            colorChoosePanel.gameObject.SetActive(false);
        }

        colorChoosePanel.InjectChangeData(HeadColor, HeadColor.color);
        uimanager.PushPanel(UIPanelType.ColorChoosePanel);
    }
    private void EndColorButtonCallback()
    {
        if (colorChoosePanel == null)
        {
            colorChoosePanel = uimanager.GetPanel(UIPanelType.ColorChoosePanel).gameObject.GetComponent<ColorChoosePanel>();
            colorChoosePanel.gameObject.SetActive(false);
        }

        colorChoosePanel.InjectChangeData(EndColor, EndColor.color);
        uimanager.PushPanel(UIPanelType.ColorChoosePanel);
    }
    private void AcceptButtonCallback()
    {
        playerProperty.HeadColor = HeadColor.color;
        playerProperty.TrailColor = EndColor.color;
        uimanager.PopPanel();
    }
    private void CloseButtonCallback()
    {
        uimanager.PopPanel();
    }
    #endregion

    public override void OnEnter()
    {
        if(playerProperty == null) playerProperty = facade.GetPlayerProperty();
        gameObject.SetActive(true);
        HeadColor.color = playerProperty.HeadColor;
        EndColor.color = playerProperty.TrailColor;
    }
    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
    public override void OnPause()
    {
        gameObject.SetActive(false);
    }
    public override void OnResume()
    {
        gameObject.SetActive(true);
    }
}
