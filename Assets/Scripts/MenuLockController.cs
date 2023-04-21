using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLockController : MonoBehaviour
{
    [SerializeField] int sceneIdLoaded;
    [SerializeField] float loadWait;
    [SerializeField] Animator doorAnim;

    IEnumerator OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Key")) 
        {
            doorAnim.enabled = true;
            yield return new WaitForSeconds(loadWait);
            SceneLoader.instance.SwitchScene(sceneIdLoaded);
        }
    }
}
