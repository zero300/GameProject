using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Property" , menuName = "PlayerProperty")]
public class PlayerProperty : ScriptableObject
{
    public float MaxHp;
    public float Hp;
    public float MaxEnergy;
    public float Energy;
}
