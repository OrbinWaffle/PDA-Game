using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    [SerializeField] float pickupRadius = 1f;
    [SerializeField] Transform rightHandTransform;
    // Original parent of object held in right hand
    Transform orgRightTransform;
    [SerializeField] Transform leftHandTransform;
    // Original parent of object held in left hand
    Transform orgLeftTransform;
    Rigidbody objInRightHand;
    Rigidbody objInLeftHand;
    Collider playerCollider;

    private Vector2 mousePosition = Vector2.zero;

    void Awake()
    {
        InputManager.instance.playerActions.DefaultControls.HandGrabRight.started += HandGrab;
        InputManager.instance.playerActions.DefaultControls.HandGrabRight.canceled += HandGrab;
        InputManager.instance.playerActions.DefaultControls.HandGrabLeft.started += HandGrab;
        InputManager.instance.playerActions.DefaultControls.HandGrabLeft.canceled += HandGrab;

        InputManager.instance.playerActions.DefaultControls.MousePoint.started += value => mousePosition = value.ReadValue<Vector2>();
        InputManager.instance.playerActions.DefaultControls.MousePoint.canceled += value => mousePosition = value.ReadValue<Vector2>();

        playerCollider = GetComponent<Collider>();
    }

    public void OnDisable()
    {
        InputManager.instance.playerActions.DefaultControls.HandGrabRight.started -= HandGrab;
        InputManager.instance.playerActions.DefaultControls.HandGrabRight.canceled -= HandGrab;
        InputManager.instance.playerActions.DefaultControls.HandGrabLeft.started -= HandGrab;
        InputManager.instance.playerActions.DefaultControls.HandGrabLeft.canceled -= HandGrab;

        InputManager.instance.playerActions.DefaultControls.MousePoint.started -= value => mousePosition = value.ReadValue<Vector2>();
        InputManager.instance.playerActions.DefaultControls.MousePoint.canceled -= value => mousePosition = value.ReadValue<Vector2>();
    }

    public void OnDestroy()
    {
        if (isActiveAndEnabled)
        {
            InputManager.instance.playerActions.DefaultControls.HandGrabRight.started -= HandGrab;
            InputManager.instance.playerActions.DefaultControls.HandGrabRight.canceled -= HandGrab;
            InputManager.instance.playerActions.DefaultControls.HandGrabLeft.started -= HandGrab;
            InputManager.instance.playerActions.DefaultControls.HandGrabLeft.canceled -= HandGrab;

            InputManager.instance.playerActions.DefaultControls.MousePoint.started -= value => mousePosition = value.ReadValue<Vector2>();
            InputManager.instance.playerActions.DefaultControls.MousePoint.canceled -= value => mousePosition = value.ReadValue<Vector2>();
        }
    }

    void HandGrab(InputAction.CallbackContext value)
    {
        if (value.action.name == "HandGrabRight")
        {
            if (value.started)
            {
                OnPickup(true, rightHandTransform, ref objInRightHand, ref orgRightTransform);
            }
            else if (value.canceled)
            {
                OnPickup(false, rightHandTransform, ref objInRightHand, ref orgRightTransform);
            }
        }
        else if (value.action.name == "HandGrabLeft")
        {
            if (value.started)
            {
                OnPickup(true, leftHandTransform, ref objInLeftHand, ref orgLeftTransform);
            }
            else if (value.canceled)
            {
                OnPickup(false, leftHandTransform, ref objInLeftHand, ref orgLeftTransform);
            }
        }
    }

    /// <summary>
    /// This method will do one of two things:
    /// It can check for objects close to the hand. If it finds an object with a rigidbody, it will parent the rigidbody to the hand.
    /// It can set the held rigidbody's parent to its original parent, causing it to no longer be attached to the hand.
    /// </summary>
    /// <param name="pickingUp">Whether or not we are picking up the item or dropping it</param>
    /// <param name="handTransform">The transform of the hand (either left or right) that we want to work with</param>
    /// <param name="heldRB">A refrence to the variable containing the rigidbody currently held in the left/right hand</param> 
    /// <param name="orgTransform">A refrence to the variable containing the original parent of the held transform</param>
    void OnPickup(bool pickingUp, Transform handTransform, ref Rigidbody heldRB, ref Transform orgTransform)
    {
        // If we're picking up the object
        if (pickingUp)
        {
            Collider[] colliders = Physics.OverlapSphere(handTransform.position, pickupRadius);
            if (colliders.Length > 0)
            {
                Rigidbody closestRB = null;
                float shortestDist = Mathf.Infinity;
                if (InputManager.instance.targetControlScheme == ControlSchemeEnum.VR)
                {
                    // Find the closest rigidbody
                    foreach (Collider col in colliders)
                    {
                        Rigidbody collidedRB = col.GetComponent<Rigidbody>();
                        if (collidedRB != null)
                        {
                            float dist = Vector3.Distance(col.transform.position, handTransform.position);
                            if (dist < shortestDist)
                            {
                                shortestDist = dist;
                                closestRB = collidedRB;
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("SDLFJLJF");
                    Ray ray = new Ray();
                    RaycastHit hit;

                    ray = Camera.main.ScreenPointToRay(mousePosition);
                    if (Physics.SphereCast(ray, 0.3f, out hit, 300, LayerMask.GetMask("Grabbable"))) //Change to spherecast
                    {
                        closestRB = hit.collider.GetComponent<Rigidbody>();
                    }
                }

                // If we found it, disable the held object's physics, teleport it into player's hand, and set its parent to the hand
                if (closestRB != null)
                {
                    closestRB.isKinematic = true;
                    Physics.IgnoreCollision(playerCollider, closestRB.GetComponent<Collider>(), true);
                    orgTransform = closestRB.transform.parent;
                    closestRB.transform.parent = handTransform;
                    heldRB = closestRB;
                }
            }
        }
        // If we're dropping the object
        else
        {
            if (heldRB != null)
            {
                heldRB.isKinematic = false;
                Physics.IgnoreCollision(playerCollider, heldRB.GetComponent<Collider>(), false);
                heldRB.transform.parent = orgTransform;
                heldRB = null;
            }
        }
    }
}
