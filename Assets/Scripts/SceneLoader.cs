using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.XR;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader manager;

    private GameObject player;
    [HideInInspector]
    public Transform headTransform;

    private void Awake()
    {
        manager = this;
        player = GameObject.FindGameObjectWithTag("Player");
        headTransform = player.GetComponentInChildren<TrackedPoseDriver>().transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchScene(int sceneIndex)
    {
        StartCoroutine(SwitchSceneCoroutine(sceneIndex));
    }
    IEnumerator SwitchSceneCoroutine(int sceneIndex)
    {
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(sceneIndex);
        player.transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
        player.transform.rotation = GameObject.FindGameObjectWithTag("SpawnPoint").transform.rotation;
    }

    public void ReloadScene()
    {
        player.transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
