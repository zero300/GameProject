using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static UnlockEvent unlockEvent = new UnlockEvent();
    public static MakeSoundEvent makeSoundEvent = new MakeSoundEvent();
}

public class UnlockEvent : GameEvent {
    public int KeyCode;
}
/// <summary>
/// 強化的種類
/// </summary>
public enum UpdateType
{
    ScanRange
}
public class UpdateEvent : GameEvent
{
    public UpdateType updateType;
    public float increaseNum;
}
/// <summary>
/// 聲音的大小
/// </summary>
public enum SoundVolumn { 
    VeryLittle = 0,
    Little = 1,
    Medium = 2,
    Large = 3,
    VeryLarge = 4
}
public class MakeSoundEvent : GameEvent 
{
    public SoundVolumn volumn;
    public Vector3 MakeSoundPos; 
}



