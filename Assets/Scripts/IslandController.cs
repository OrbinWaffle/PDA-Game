using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandController : MonoBehaviour
{
    List<LockController> lockControllers = new List<LockController>();
    List<BridgeController> bridgeControllers = new List<BridgeController>();
    IslandController originIsland;
    private void Start()
    {

    }
    public void OnLockChosen()
    {
        if (originIsland != null)
        {
            originIsland.OnPlayerLeave();
        }
    }
    public void ChangeAllLocksOnIsland(bool state)
    {
        foreach (LockController lockCont in lockControllers)
        {
            lockCont.SetLockState(state);
        }
    }
    public void RaiseAllBridges()
    {
        foreach(BridgeController bridge in bridgeControllers)
        {
            bridge.RaiseBridge();
        }
    }
    public void OnPlayerLeave()
    {
        ChangeAllLocksOnIsland(true);
        RaiseAllBridges();
    }
    public void SetOriginIsland(IslandController newOriginIsland)
    {
        originIsland = newOriginIsland;
    }
    public void AddLock(LockController newLock)
    {
        lockControllers.Add(newLock);
    }
    public void AddBridge(BridgeController newBridge)
    {
        bridgeControllers.Add(newBridge);
    }
}
