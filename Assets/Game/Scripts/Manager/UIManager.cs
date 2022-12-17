using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase
{
    public UIManager(GameFacade facade)
    {
        this.facade = facade;
        canvas = facade.gameObject;
    }
    private Dictionary<UIPanelType, BasePanel> panelDict = new Dictionary<UIPanelType, BasePanel>();
    // �]�����O�|���u����  �n�������ӱq�̤W�����i������
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();


    private GameObject canvas;
    #region Override BaseManager

    
    public override void InitManager()
    {
        PushPanel(UIPanelType.MessagePanel);
        PushPanel(UIPanelType.TitlePanel);
        EventManager.AddEvents<ArchieveEndPointEvent>(LevelWin);
    }
    public override void UpdateManager()
    {
    }

    #endregion
    /// <summary>
    /// �b�����W��ܸ�Panel 
    /// �P��Ĳ�o�쥻Panel��OnPause
    /// </summary>
    /// <param name="type"></param>
    public void PushPanel(UIPanelType type)
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();
        if (panelStack.Count > 0)
        {
            BasePanel toppanel = panelStack.Peek();
            toppanel.OnPause();
        }
        BasePanel panel = GetPanel(type);
        panel.OnEnter();
        panelStack.Push(panel);
    }
    /// <summary>
    /// ������e�̤W�h��Panel
    /// </summary>
    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        // �p�G��e�S������Panel ������^
        if (panelStack.Count <= 0)
            return;

        //���̤W����Panel 
        BasePanel toppanel = panelStack.Pop();
        toppanel.OnExit();
        //�p�G�S��L�����{  ��^
        if (panelStack.Count <= 0)
            return;

        // ����ĤG�ӭ���
        BasePanel secondPanel = panelStack.Peek();
        secondPanel.OnResume();
    }
    /// <summary>
    /// ��o���O
    /// </summary>
    /// <param name="panelType"></param>
    /// <returns></returns>
    public BasePanel GetPanel(UIPanelType panelType)
    {
        // �p�G�r�夣�s�b 
        if (panelDict == null)
            panelDict = new Dictionary<UIPanelType, BasePanel>();

        bool success = panelDict.TryGetValue(panelType, out BasePanel panel);
        if (!success)
        {
            // ����prefab�����|
            // �٨S�����|������
            // TODO : path���r��
            string path;
            switch (panelType)
            {
                case UIPanelType.TitlePanel:
                    path = "UIPanel/" + "TitlePanel";
                    break;
                case UIPanelType.ContinuePanel:
                    path = "UIPanel/" + "ContinuePanel";
                    break;
                case UIPanelType.PausePanel:
                    path = "UIPanel/" + "PausePanel";
                    break;
                case UIPanelType.LosePanel:
                    path = "UIPanel/" + "LosePanel";
                    break;
                case UIPanelType.MessagePanel:
                    path = "UIPanel/" + "MessagePanel";
                    break;
                default:
                    return null;
            }
            GameObject obj = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            // TODO : Canvas����m
            obj.transform.SetParent(canvas.transform, false);
            obj.GetComponent<BasePanel>().UIManager = this;
            obj.GetComponent<BasePanel>().GameFacade = facade;
            panelDict.Add(panelType, obj.GetComponent<BasePanel>());
            return obj.GetComponent<BasePanel>();
        }
        else
        {
            return panel;
        }
    }
    /// <summary>
    /// �M����e�Ҧ�UI
    /// </summary>
    public void SceneChangedClearUI()
    {
        while(panelStack.Count > 0)
        {
            PopPanel();
        }
    }

    public void LevelWin(ArchieveEndPointEvent evt)
    {
        PushPanel(UIPanelType.ContinuePanel);
    }


    public override void DestroyManager()
    {
        Debug.Log("�R��UIManager");
        EventManager.RemoveListener<ArchieveEndPointEvent>(LevelWin);
    }
}
