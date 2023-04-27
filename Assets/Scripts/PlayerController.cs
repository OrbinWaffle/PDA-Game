using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static GameObject playerInstance;
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
    }
}
