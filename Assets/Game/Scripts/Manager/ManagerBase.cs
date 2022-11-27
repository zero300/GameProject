using UnityEngine;

public abstract class ManagerBase 
{
    protected GameFacade facade;
    public abstract void InitManager();
    public abstract void UpdateManager();
    public abstract void DestroyManager();
}
