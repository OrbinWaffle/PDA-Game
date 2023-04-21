using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    Transform headTransform;

    private void Start()
    {
        headTransform = GameManager.instance.headTransform;
    }
    private void Update()
    {
        transform.position = new Vector3(headTransform.position.x, transform.position.y, headTransform.position.z);
        transform.rotation = Quaternion.Euler(90, headTransform.eulerAngles.y, 0);
    }
}
