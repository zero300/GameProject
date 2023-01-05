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
    /// �����d��
    /// </summary>
    public float maxScanRange = 10;
    /// <summary>
    /// �i���d�����ɶ�
    /// </summary>
    public float scanConstantTime = 2.0f;
    /// <summary>
    /// �����t��
    /// </summary>
    public float scanSpeed = 1.0f;
    /// <summary>
    /// �����d���Y��t��
    /// </summary>
    public float fadeSpeed = 0.05f;
    /// <summary>
    /// �����C��A�a��H��
    /// </summary>
    public Color HeadColor = new Color(37,121,204);
    /// <summary>
    /// �����C��A�̻��Z��
    /// </summary>
    public Color TrailColor = new Color(0,0,0);
}
