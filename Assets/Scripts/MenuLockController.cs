using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLockController : LockController
{
    [SerializeField] int sceneIdLoaded;
    [SerializeField] float loadWait;
    [SerializeField] Animator doorAnim;
    [SerializeField] bool saveString;
    [SerializeField] bool clearString;

    public override void OnAwake()
    {
        keyPosition = transform.Find("KeyPosition");
        return;
    }
    public override void OnChosen()
    {
        if(clearString)
        {
            GameManager.instance.ClearString();
        }
        else if(saveString)
        {
            GameManager.instance.SaveString();
        }
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene() 
    {
        doorAnim.enabled = true;
        yield return new WaitForSeconds(loadWait);
        SceneLoader.instance.SwitchScene(sceneIdLoaded);
    }
}
