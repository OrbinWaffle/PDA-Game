using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader manager;

    public GameObject player;

    private void Awake()
    {
        manager = this;
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
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(sceneIndex);
    }

    public void ReloadScene()
    {
        player.transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
