using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorChoosePanel : BasePanel
{
    private Color currentChooseColor = Color.black;
    private Object needChange;

    Image colorImage;

    Scrollbar redScrollbar;
    Scrollbar blueScrollbar;
    Scrollbar greenScrollbar;

    InputField redInputField;
    InputField blueInputField;
    InputField greenInputField;

    Button ConfirmButton;
    Button CancelButton;

    // Start is called before the first frame update
    void Awake()
    {
        colorImage = transform.Find("LastColor").GetComponent<Image>();

        redScrollbar = transform.Find("RedChooser").GetComponent<Scrollbar>();
        greenScrollbar = transform.Find("GreenChooser").GetComponent<Scrollbar>();
        blueScrollbar = transform.Find("BlueChooser").GetComponent<Scrollbar>();
        

        redInputField = transform.Find("RedInputField").GetComponent<InputField>();
        greenInputField = transform.Find("GreenInputField").GetComponent<InputField>();
        blueInputField = transform.Find("BlueInputField").GetComponent<InputField>();
        

        ConfirmButton = transform.Find("ConfirmButton").GetComponent<Button>();
        CancelButton = transform.Find("CancelButton").GetComponent<Button>();

        redScrollbar.value = currentChooseColor.r;
        greenScrollbar.value = currentChooseColor.g;
        blueScrollbar.value = currentChooseColor.b;
        
        redInputField.text = ((currentChooseColor.r) * 255).ToString();
        greenInputField.text = ((currentChooseColor.g) * 255).ToString();
        blueInputField.text = ((currentChooseColor.b) * 255).ToString();
        
        ConfirmButton.onClick.AddListener(ConfirmButtonCallback);
        CancelButton.onClick.AddListener(CancelButtonCallback);

        redScrollbar.onValueChanged.AddListener(RedScrollbarChange);
        greenScrollbar.onValueChanged.AddListener(GreenScrollbarChange);
        blueScrollbar.onValueChanged.AddListener(BlueScrollbarChange);
        
        redInputField.onValueChanged.AddListener(RedInputFieldChange);
        greenInputField.onValueChanged.AddListener(GreenInputFieldChange);
        blueInputField.onValueChanged.AddListener(BlueInputFieldChange);
        

        colorImage.color = currentChooseColor;
    }

    #region UI CallBack

    private void RedScrollbarChange(float value)
    {
        redInputField.onValueChanged.RemoveListener(RedInputFieldChange);
        redInputField.text = ((int)(value * 255)).ToString();
        currentChooseColor = new Color(value, currentChooseColor.g, currentChooseColor.b);
        colorImage.color = currentChooseColor;
        redInputField.onSubmit.AddListener(RedInputFieldChange);
    }
    private void RedInputFieldChange(string str)
    {
        redScrollbar.onValueChanged.RemoveListener(RedScrollbarChange);
        int value = int.Parse(str);
        redScrollbar.value = (float)value / 255;
        currentChooseColor = new Color(redScrollbar.value, currentChooseColor.g, currentChooseColor.b);
        colorImage.color = currentChooseColor;
        redScrollbar.onValueChanged.AddListener(RedScrollbarChange);
    }
    private void BlueScrollbarChange(float value)
    {
        blueInputField.onValueChanged.RemoveListener(BlueInputFieldChange);
        blueInputField.text = ((int)(value * 255)).ToString();
        currentChooseColor = new Color(currentChooseColor.r, currentChooseColor.g, value);
        colorImage.color = currentChooseColor;
        blueInputField.onSubmit.AddListener(BlueInputFieldChange);
    }
    private void BlueInputFieldChange(string str)
    {
        blueScrollbar.onValueChanged.RemoveListener(BlueScrollbarChange);
        int value = int.Parse(str);
        blueScrollbar.value = (float)value / 255;
        currentChooseColor = new Color(currentChooseColor.r, currentChooseColor.g, blueScrollbar.value);
        colorImage.color = currentChooseColor;
        blueScrollbar.onValueChanged.AddListener(BlueScrollbarChange);
    }
    private void GreenScrollbarChange(float value)
    {
        greenInputField.onValueChanged.RemoveListener(GreenInputFieldChange);
        greenInputField.text = ((int)(value * 255)).ToString();
        currentChooseColor = new Color(currentChooseColor.r, value, currentChooseColor.b);
        colorImage.color = currentChooseColor;
        greenInputField.onSubmit.AddListener(GreenInputFieldChange);
    }
    private void GreenInputFieldChange(string str)
    {
        greenScrollbar.onValueChanged.RemoveListener(GreenScrollbarChange);
        int value = int.Parse(str);
        greenScrollbar.value = (float)value / 255;
        currentChooseColor = new Color(currentChooseColor.r, greenScrollbar.value, currentChooseColor.b);
        colorImage.color = currentChooseColor;
        greenScrollbar.onValueChanged.AddListener(GreenScrollbarChange);
    }

    private void ConfirmButtonCallback()
    {
        if(needChange.GetType() == typeof(Image))
        {
            (needChange as Image).color = currentChooseColor;
        }else if (needChange.GetType() == typeof(RawImage))
        {
            (needChange as RawImage).color = currentChooseColor;
        }
        uimanager.PopPanel();
    }
    private void CancelButtonCallback()
    {
        uimanager.PopPanel();
    }
    #endregion

    public override void OnEnter()
    {
        gameObject.SetActive(true);

        redScrollbar.value = currentChooseColor.r;
        blueScrollbar.value = currentChooseColor.b;
        greenScrollbar.value = currentChooseColor.g;
        redInputField.text = ((currentChooseColor.r) * 255).ToString();
        blueInputField.text = ((currentChooseColor.b) * 255).ToString();
        greenInputField.text = ((currentChooseColor.g) * 255).ToString();

        colorImage.color = currentChooseColor;
    }
    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
    public override void OnPause()
    {

    }
    public override void OnResume()
    {

    }

    /// <summary>
    /// 注入顏色
    /// </summary>
    /// <param name="color"></param>
    public void InjectCurrentColor(Color color) => currentChooseColor = color;
    /// <summary>
    /// 注入需要改變的資料
    /// </summary>
    /// <param name="obj"></param>
    public void InjectChangeData(Object obj ,Color color ){
        needChange = obj;
        currentChooseColor = color;
    }

}
