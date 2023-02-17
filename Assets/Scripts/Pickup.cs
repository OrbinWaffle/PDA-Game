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
    void Awake()
    {
        InputManager.instance.playerActions.XRIRightHand.MouseClick.performed += RightHandGrab;
        InputManager.instance.playerActions.XRIRightHand.MouseClick.canceled += RightHandGrab;
        InputManager.instance.playerActions.XRILeftHand.MouseClick.performed += LeftHandGrab;
        InputManager.instance.playerActions.XRILeftHand.MouseClick.canceled += LeftHandGrab;

        playerCollider = GetComponent<Collider>();
    }
    void RightHandGrab(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            OnRightPickup(true);
        }
        else if (value.canceled)
        {
            OnRightPickup(false);
        }
    }
    void LeftHandGrab(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            OnLeftPickup(true);
        }
        else if (value.canceled)
        {
            OnLeftPickup(false);
        }
    }
    void OnRightPickup(bool pickingUp)
    {
        OnPickup(pickingUp, rightHandTransform, ref objInRightHand, ref orgRightTransform);
    }
    void OnLeftPickup(bool pickingUp)
    {
        OnPickup(pickingUp, leftHandTransform, ref objInLeftHand, ref orgLeftTransform);
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
                // Find the closest rigidbody
                Rigidbody closestRB = null;
                float shortestDist = Mathf.Infinity;
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
