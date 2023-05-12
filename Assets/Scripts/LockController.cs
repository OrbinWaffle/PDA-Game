using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockController : MonoBehaviour
{
    BridgeController parentBridge;
    IslandController parentIsland;
    [SerializeField] IslandController targetIsland;
    [SerializeField] string acceptingSymbol;
    [SerializeField] string stackRequirement;
    [SerializeField] string[] stackReplacement;
    [SerializeField] bool autoWrite = true;
    TextMeshProUGUI lockText;
    protected Transform keyPosition;

    public bool lockEnabled = true;

    // Index of the lock in the parent BridgeController's lock list
    private int lockIndex;

    private void Awake()
    {
        OnAwake();
    }
    public virtual void OnAwake()
    {
        parentBridge = transform.parent.GetComponent<BridgeController>();
        if (parentBridge != null)
        {
            parentIsland = transform.parent.parent.GetComponent<IslandController>();
            if(targetIsland == null)
            {
                Debug.LogError("BRIDGE LOCK DOES NOT HAVE A TARGET ISLAND");
            }
        }
        else
        {
            parentIsland = transform.parent.GetComponent<IslandController>();
        }
        parentIsland.AddLock(this);
        keyPosition = transform.Find("KeyPosition");
        if(autoWrite)
        {
            string replacementString = "";
            foreach (string symbol in stackReplacement)
            {
                string symbolToAdd = symbol;
                if (symbol == "epsilon")
                {
                    symbolToAdd = "x";
                }
                replacementString += symbolToAdd;
            }
            string symbolToPrint = acceptingSymbol;
            if(acceptingSymbol == "epsilon")
            {
                symbolToPrint = "x";
            }
            lockText = transform.Find("Model/Canvas/Symbols").GetComponent<TextMeshProUGUI>();

            lockText.text = symbolToPrint + "\n\n" + stackRequirement + "\n\n" + replacementString;
        }
    }
    public virtual void OnChosen()
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
    public void SetLockIndex(int newIndex)
    {
        lockIndex = newIndex;
    }
    public void SetLockState(bool newState)
    {
        lockEnabled = newState;
    }
    public Transform GetKeyPos()
    {
        return keyPosition;
    }
}
