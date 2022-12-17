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
    // 因為面板會有優先級  要關閉應該從最上面的進行關閉
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
    /// 在頁面上顯示該Panel 
    /// 同時觸發原本Panel的OnPause
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
    /// 移除當前最上層的Panel
    /// </summary>
    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        // 如果當前沒有任何Panel 直接返回
        if (panelStack.Count <= 0)
            return;

        //拿最上面的Panel 
        BasePanel toppanel = panelStack.Pop();
        toppanel.OnExit();
        //如果沒其他頁面ㄌ  返回
        if (panelStack.Count <= 0)
            return;

        // 獲取第二個頁面
        BasePanel secondPanel = panelStack.Peek();
        secondPanel.OnResume();
    }
    /// <summary>
    /// 獲得面板
    /// </summary>
    /// <param name="panelType"></param>
    /// <returns></returns>
    public BasePanel GetPanel(UIPanelType panelType)
    {
        // 如果字典不存在 
        if (panelDict == null)
            panelDict = new Dictionary<UIPanelType, BasePanel>();

        bool success = panelDict.TryGetValue(panelType, out BasePanel panel);
        if (!success)
        {
            // 先找prefab的路徑
            // 還沒做路徑ˊˇˋ
            // TODO : path的字典
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
            // TODO : Canvas的位置
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
    /// 清除當前所有UI
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
        Debug.Log("刪除UIManager");
        EventManager.RemoveListener<ArchieveEndPointEvent>(LevelWin);
    }
}
