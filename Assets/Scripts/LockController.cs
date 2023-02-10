using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    [SerializeField] string acceptingSymbol;
    [SerializeField] string stackRequirement;
    [SerializeField] string[] stackReplacement;
    [SerializeField] GameObject bridge;
    
    public void EnableBridge()
    {
        bridge.SetActive(true);
    }
    public void DisableBridge()
    {
        bridge.SetActive(false);
    }
    public string GetAcceptingSymbol()
    {
        return acceptingSymbol;
    }
    public string getStackRequirement()
    {
        return stackRequirement;
    }
    public string[] getStackReplacement()
    {
        return stackReplacement;
    }
}
