using System;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public PlayerProperty playerProperty;
    public float currentHp;
    public float currentEnergy;
    private void Awake()
    {
        currentHp = playerProperty.MaxHp;
        currentEnergy = playerProperty.MaxEnergy;
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
            // TODO : ��C��
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
