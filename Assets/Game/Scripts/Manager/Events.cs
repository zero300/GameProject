using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static UnlockEvent unlockEvent = new UnlockEvent();
}

public class UnlockEvent : GameEvent {
    public int KeyCode;
}
public enum UpdateType
{
    ScanRange
}
public class UpdateEvent : GameEvent
{
    public UpdateType updateType;
    public float increaseNum;
}


