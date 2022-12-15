using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Property" , menuName = "PlayerProperty")]
public class PlayerProperty : ScriptableObject
{
    public float MaxHp = 20;
    public float Hp = 0;
    public float MaxEnergy = 20;
    public float Energy = 0;
    /// <summary>
    /// �����d��
    /// </summary>
    public float maxScanRange = 20;
    /// <summary>
    /// ����Ring���e��
    /// </summary>
    public float scanWidth = 5;
    /// <summary>
    /// �����t��
    /// </summary>
    public float scanSpeed = 0.05f;
}
