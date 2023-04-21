using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class MiniMapController : MonoBehaviour
{

    [SerializeField] float zoomedInCamSize;

    Vector3 fullscreenPos;
    float zoomedOutCamSize;
    Quaternion orgRotation;
    Vector3 orgPosition;
    Camera camera;
    bool isZoomed;
    Transform headTransform;

    // Start is called before the first frame update
    void Start()
    {
        InputManager.instance.playerActions.DefaultControls.Map.started += ToggleZoom;
        headTransform = GameManager.instance.headTransform;

        fullscreenPos = transform.position;
        camera = GetComponent<Camera>();
        zoomedOutCamSize = camera.orthographicSize;
        orgRotation = transform.rotation;
        orgPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isZoomed)
        {
            transform.rotation = Quaternion.Euler(90, headTransform.eulerAngles.y, 0);
        }
    }
    public void ToggleZoom(InputAction.CallbackContext value)
    {
        Debug.Log("lskjdfa;jhfjhsf");
        if (isZoomed)
        {
            ZoomOutMiniMap();
        }
        else
        {
            ZoomInMiniMap();
        }
    }

    public void ZoomInMiniMap()
    {
        isZoomed = true;
        transform.position = new Vector3(headTransform.position.x, transform.position.y, headTransform.position.z);
        camera.orthographicSize = zoomedInCamSize;
    }

    public void ZoomOutMiniMap() {
        isZoomed = false;
        transform.rotation = Quaternion.Euler(90, 0, 0);
        camera.orthographicSize = zoomedOutCamSize;
        transform.rotation = orgRotation;
        transform.position = orgPosition;
    }
}
