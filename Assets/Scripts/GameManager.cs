using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public GameObject player;
    [HideInInspector] public Transform headTransform;
    public string savedString;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        headTransform = player.GetComponentInChildren<TrackedPoseDriver>().transform;
    }
    public void UpdateTime()
    {
        LevelTimer.instance.StartTimer(FindObjectOfType<SceneInfo>().time);
    }
    public void SaveString()
    {
        savedString = KeyController.instance.GetFinalString();
    }
    public void ClearString()
    {
        savedString = "";
    }
    public void LoadString()
    {
        KeyController.instance.SetString(savedString);
    }
}
