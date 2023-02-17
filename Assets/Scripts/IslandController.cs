using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandController : MonoBehaviour
{
    List<BridgeController> bridgeControllers = new List<BridgeController>();
    IslandController originIsland;
    private void Start()
    {
        // Assign indexes to each bridgeController in the bridgeController list
        for (int i = 0; i < bridgeControllers.Count; ++i)
        {
            bridgeControllers[i].SetBridgeIndex(i);
        }
    }
    void OnBridgeChosen()
    {

    }
    public void AddBridge(BridgeController newBridgeController)
    {
        bridgeControllers.Add(newBridgeController);
    }
    public void OnBridgeChosen(BridgeController chosenBridge)
    {
        ChangeAllLocksOnIsland(false);
        originIsland.OnPlayerLeave();
    }
    public void ChangeAllLocksOnIsland(bool state)
    {
        foreach (BridgeController bridge in bridgeControllers)
        {
            bridge.ChangeLocksOnBridge(state);
        }
    }
    public void OnPlayerLeave()
    {
        ChangeAllLocksOnIsland(true);
        foreach (BridgeController bridge in bridgeControllers)
        {
            bridge.RaiseBridge();
        }
    }
    public void SetOriginIsland(IslandController newOriginIsland)
    {
        originIsland = newOriginIsland;
    }
}
