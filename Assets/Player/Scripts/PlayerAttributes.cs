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

    //TODO:���˵{��
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
    /// ����ˮ`
    /// </summary>
    /// <param name="damage">�ˮ`�ƭ�</param>
    public void GetHurt(float damage)
    {
        currentHp = Math.Clamp(currentHp - damage , 0 , playerProperty.MaxHp);
        if(currentHp == 0)
        {
            // ��C��
            GameFacade.Instance.PushPanel(UIPanelType.LosePanel);
        }
    }
    /// <summary>
    /// ������O
    /// </summary>
    /// <param name="energy">���Ӫ���O</param>
    /// <returns>�O�_�i�H����o���ʧ@</returns>
    public bool UseEnergy(float energy)
    {
        if (currentEnergy < energy) return false;

        currentEnergy -= energy;
        return true;
    }
}
