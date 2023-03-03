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
    FixedJoint rightJoint;
    FixedJoint leftJoint;
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
        rightJoint = rightHandTransform.GetComponent<FixedJoint>();
        leftJoint = leftHandTransform.GetComponent<FixedJoint>();
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
                OnPickup(true, true, ref objInRightHand, ref objInLeftHand, ref rightJoint, ref leftJoint);
            }
            else if (value.canceled)
            {
                OnPickup(false, true, ref objInRightHand, ref objInLeftHand, ref rightJoint, ref leftJoint);
            }
        }
        else if (value.action.name == "HandGrabLeft")
        {
            if (value.started)
            {
                OnPickup(true, false, ref objInLeftHand, ref objInRightHand, ref leftJoint, ref rightJoint);
            }
            else if (value.canceled)
            {
                OnPickup(false, false, ref objInLeftHand, ref objInRightHand, ref leftJoint, ref rightJoint);
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
    void OnPickup(bool pickingUp, bool right, ref Rigidbody heldRB, ref Rigidbody otherHandRB, ref FixedJoint handJoint, ref FixedJoint otherHandJoint)
    {
        // If we're picking up the object
        if (pickingUp)
        {
            Collider[] colliders = Physics.OverlapSphere(handJoint.transform.position, pickupRadius);
            if (colliders.Length > 0)
            {
                Rigidbody closestRB = null;
                float shortestDist = Mathf.Infinity;
                if (InputManager.instance.targetControlScheme == ControlSchemeEnum.VR)
                {
                    // Find the closest rigidbody
                    foreach (Collider col in colliders)
                    {
                        if(col.gameObject.layer != 6)
                        {
                            continue;
                        }
                        Rigidbody collidedRB = col.GetComponentInParent<Rigidbody>();
                        if (collidedRB != null)
                        {
                            float dist = Vector3.Distance(col.transform.position, handJoint.transform.position);
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
                    Ray ray = new Ray();
                    RaycastHit hit;

                    ray = Camera.main.ScreenPointToRay(mousePosition);
                    if (Physics.SphereCast(ray, 0.3f, out hit, 300, LayerMask.GetMask("Grabbable"))) //Change to spherecast
                    {
                        closestRB = hit.collider.GetComponentInParent<Rigidbody>();
                    }
                }

                // If we found it, disable the held object's physics, teleport it into player's hand, and set its parent to the hand
                if (closestRB != null)
                {
                    if (closestRB == otherHandRB)
                    {
                        OnPickup(false, !right, ref otherHandRB, ref heldRB, ref otherHandJoint, ref handJoint);
                    }
                    KeyController keyController = closestRB.GetComponent<KeyController>();
                    if (keyController != null)
                    {
                        keyController.OnRelease();
                    }
                    //closestRB.isKinematic = true;
                    Collider[] RBColliders = closestRB.GetComponentsInChildren<Collider>();
                    foreach (Collider col in RBColliders)
                    {
                        Physics.IgnoreCollision(playerCollider, col, true);
                    }
                    //orgTransform = closestRB.transform.parent;
                    //closestRB.transform.parent = handTransform;
                    handJoint.connectedBody = closestRB;
                    heldRB = closestRB;
                }
            }
        }
        // If we're dropping the object
        else
        {
            if (heldRB != null)
            {
                KeyController keyController = heldRB.GetComponent<KeyController>();
                if (keyController != null)
                {
                    keyController.isPickedUp = false;
                }
                Debug.Log("MOGUES");
                //heldRB.isKinematic = false;
                Collider[] RBColliders = heldRB.GetComponentsInChildren<Collider>();
                foreach (Collider col in RBColliders)
                {
                    Physics.IgnoreCollision(playerCollider, col, false);
                }
                //heldRB.transform.parent = orgTransform;
                handJoint.connectedBody = null;
                heldRB = null;
            }
        }
    }
}
