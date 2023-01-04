using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour
{
    public PlayerProperty playerProperty;
    public RawImage HurtRed;
    public float MaxHp;
    public float currentHp;
    public float currentEnergy;

    //TODO:受傷程度
    private void Awake()
    {
        currentHp = playerProperty.MaxHp;
        MaxHp = currentHp;
        currentEnergy = playerProperty.MaxEnergy;
    }

    private void Update()
    {
        float percetage = 1 - currentHp / MaxHp;
        HurtRed.color = new Color(1, 1, 1, percetage);
    }

    /// <summary>
    /// 受到傷害
    /// </summary>
    /// <param name="damage">傷害數值</param>
    public void GetHurt(float damage)
    {
        currentHp = Math.Clamp(currentHp - damage , 0 , playerProperty.MaxHp);
        if(currentHp == 0)
        {
            // 輸遊戲
            GameFacade.Instance.PushPanel(UIPanelType.LosePanel);
        }
    }
    /// <summary>
    /// 消耗體力
    /// </summary>
    /// <param name="energy">消耗的體力</param>
    /// <returns>是否可以執行這次動作</returns>
    public bool UseEnergy(float energy)
    {
        if (currentEnergy < energy) return false;

        currentEnergy -= energy;
        return true;
    }
}
