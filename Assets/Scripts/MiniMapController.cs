using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{

    [SerializeField] float zoomedInCamSize;

    Vector3 fullscreenPos;
    float zoomedOutCamSize;
    Camera camera;


    // Start is called before the first frame update
    void Start()
    {
        fullscreenPos = transform.position;
        camera = GetComponent<Camera>();
        zoomedOutCamSize = camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ZoomInMiniMap() {
        Transform player = SceneLoader.manager.player.transform;
        transform.parent = player;
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        camera.orthographicSize = zoomedInCamSize;
    }

    public void ZoomOutMiniMap() {
        transform.parent = null;
        transform.position = fullscreenPos;
        camera.orthographicSize = zoomedOutCamSize;
    }
}
