using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Property" , menuName = "PlayerProperty")]
public class PlayerProperty : ScriptableObject
{
    public float MaxHp = 50;
    public float Hp = 0;
    public float MaxEnergy = 20;
    public float Energy = 0;


    /// <summary>
    /// 探測範圍
    /// </summary>
    public float maxScanRange = 10;
    /// <summary>
    /// 可見範圍持續時間
    /// </summary>
    public float scanConstantTime = 2.0f;
    /// <summary>
    /// 探測速度
    /// </summary>
    public float scanSpeed = 1.0f;
    /// <summary>
    /// 探測範圍的縮減速度
    /// </summary>
    public float fadeSpeed = 0.05f;
    /// <summary>
    /// 探測顏色，靠近人物
    /// </summary>
    public Color HeadColor = new Color(37,121,204);
    /// <summary>
    /// 探測顏色，最遠距離
    /// </summary>
    public Color TrailColor = new Color(0,0,0);
}
