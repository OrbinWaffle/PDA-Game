using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static GameObject playerInstance;
    [SerializeField] Transform hand;
    void Awake()
    {
        if(playerInstance == null)
        {
            playerInstance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
        InputManager.instance.playerActions.DefaultControls.Recall.started += RecallSword;
    }
    void RecallSword(InputAction.CallbackContext value)
    {
        Transform keyInstance = KeyController.instance.transform;
        if(!keyInstance.GetComponent<Rigidbody>().isKinematic)
        {
            keyInstance.position = hand.position;
            keyInstance.rotation = hand.rotation;
        }
    }
}
