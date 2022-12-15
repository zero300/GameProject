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
    /// ±´´ú½d³ò
    /// </summary>
    public float maxScanRange = 20;
    /// <summary>
    /// ±´´úRingªº¼e«×
    /// </summary>
    public float scanWidth = 5;
    /// <summary>
    /// ±´´ú³t«×
    /// </summary>
    public float scanSpeed = 0.05f;
}
