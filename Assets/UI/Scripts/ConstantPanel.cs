using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 不繼承的Panel
/// </summary>
public class ConstantPanel : MonoBehaviour
{
    public LayerMask InteractLayer;
    private RawImage center;
    IInteract interactObj;
    bool canInteract;

    private void Awake()
    {
        center = GetComponent<RawImage>();
        EventManager.AddEvents<ArchieveEndPointEvent>(LoseOrWinGame);
    }
    void Update()
    {
        CheckIneteract();
        CheckMousePosFindInteractObj();
    }
    /// <summary>
    /// 使用滑鼠位置，射線檢測可交互物品
    /// </summary>
    private void CheckMousePosFindInteractObj()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;
        if (!canInteract) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 3f, InteractLayer, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject.TryGetComponent<IInteract>(out IInteract interact)) interact.Interact();
            }
        }   
    }
    private void CheckIneteract()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, 3f, InteractLayer, QueryTriggerInteraction.Ignore))
        {
            canInteract = true;
            ChangeCenterColor(Color.red);
        }
        else
        {
            canInteract = false;
            ChangeCenterColor(Color.white);
        }
            
    }

    private void ChangeCenterColor(Color color)
    {
        center.color = color;
    }

    private void LoseOrWinGame(ArchieveEndPointEvent evt) => center.gameObject.SetActive(false);

    private void OnDestroy()
    {
        EventManager.RemoveListener<ArchieveEndPointEvent>(LoseOrWinGame);
    }
}
