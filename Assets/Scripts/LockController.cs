using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    BridgeController parentBridge;
    IslandController parentIsland;
    [SerializeField] IslandController targetIsland;
    [SerializeField] string acceptingSymbol;
    [SerializeField] string stackRequirement;
    [SerializeField] string[] stackReplacement;
    [Tooltip("List of objects corresponding to each symbol in stackReplacement")]
    [SerializeField] GameObject[] notchObjects;

    public bool lockEnabled = true;

    // Index of the lock in the parent BridgeController's lock list
    private int lockIndex;

    private void Awake()
    {
        parentBridge = transform.parent.GetComponent<BridgeController>();
        if (parentBridge != null)
        {
            parentIsland = transform.parent.parent.GetComponent<IslandController>();
        }
        else
        {
            parentIsland = transform.parent.GetComponent<IslandController>();
        }
        parentIsland.AddLock(this);
    }
    public void OnChosen()
    {
        // Tell parent bridge that this lock was chosen
        parentIsland.OnLockChosen();
        if (parentBridge != null)
        {
            parentBridge.LowerBridge();
            parentIsland.ChangeAllLocksOnIsland(false);
            targetIsland.SetOriginIsland(parentIsland);
        }
    }
    public string GetAcceptingSymbol()
    {
        return acceptingSymbol;
    }
    public string GetStackRequirement()
    {
        return stackRequirement;
    }
    public string[] GetStackReplacement()
    {
        return stackReplacement;
    }
    public GameObject[] GetNotches()
    {
        return notchObjects;
    }
    public void SetLockIndex(int newIndex)
    {
        lockIndex = newIndex;
    }
    public void SetLockState(bool newState)
    {
        lockEnabled = newState;
    }
}
