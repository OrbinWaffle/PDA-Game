using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    GameObject player;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;
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
        GameManager.instance.UpdateTime();
        player.transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
        player.transform.rotation = GameObject.FindGameObjectWithTag("SpawnPoint").transform.rotation;
    }

    public void ReloadScene()
    {
        player.transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
