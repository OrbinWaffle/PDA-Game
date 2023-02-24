using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    IslandController parentIsland;
    Animator bridgeAnimator;
    private void Awake()
    {
        parentIsland = transform.parent.GetComponent<IslandController>();
        parentIsland.AddBridge(this);
    }

    void Start()
    {
        bridgeAnimator = GetComponent<Animator>();
    }
    public void LowerBridge()
    {
        bridgeAnimator.SetBool("isLowered", true);
    }
    public void RaiseBridge()
    {
        bridgeAnimator.SetBool("isLowered", false);
    }
}
