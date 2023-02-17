using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [SerializeField] IslandController parentIsland;
    [SerializeField] IslandController targetIsland;
    List<LockController> lockControllers = new List<LockController>();
    Animator bridgeAnimator;

    // Index of the bridge in the parent IslandController's bridge list
    int bridgeIndex;

    private void Awake()
    {
        parentIsland.AddBridge(this);
    }

    void Start()
    {
        bridgeAnimator = GetComponent<Animator>();

        // Assign indexes to each lockController in the lockController list
        for(int i = 0; i < lockControllers.Count; ++i)
        {
            lockControllers[i].SetLockIndex(i);
        }
    }
    public void AddLock(LockController lockToAdd)
    {
        lockControllers.Add(lockToAdd);
    }
    public void OnLockChosen(int lockIndex)
    {
        parentIsland.OnBridgeChosen(this);
        // Disable all locks
        ChangeLocksOnBridge(false);
        targetIsland.SetOriginIsland(parentIsland);
        bridgeAnimator.SetBool("isLowered", true);
    }
    public void RaiseBridge()
    {
        bridgeAnimator.SetBool("isLowered", false);
    }
    // Enables/Disables all locks within this bridge
    public void ChangeLocksOnBridge(bool state)
    {
        foreach (LockController lockController in lockControllers)
        {
            lockController.lockEnabled = state;
        }
    }
    public void SetBridgeIndex(int newIndex)
    {
        bridgeIndex = newIndex;
    }
}
