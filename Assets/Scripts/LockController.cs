using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    [SerializeField] BridgeController parentBridge;
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
        parentBridge.AddLock(this);
    }
    public void OnChosen()
    {
        // Tell parent bridge that this lock was chosen
        parentBridge.OnLockChosen(lockIndex);
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
}
