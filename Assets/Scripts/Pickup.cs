using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    [SerializeField] float pickupRadius = 1f;
    [SerializeField] Transform rightHandTransform;
    Transform orgRightTransform;
    [SerializeField] Transform leftHandTransform;
    Transform orgLeftTransform;
    Rigidbody objInRightHand;
    Rigidbody objInLeftHand;
    void Awake()
    {
        Debug.Log("A OKAY");
        InputManager.instance.playerActions.XRIRightHand.MouseClick.performed += RightHandGrab;
        InputManager.instance.playerActions.XRIRightHand.MouseClick.canceled += RightHandGrab;
        InputManager.instance.playerActions.XRILeftHand.MouseClick.performed += LeftHandGrab;
        InputManager.instance.playerActions.XRILeftHand.MouseClick.canceled += LeftHandGrab;
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
        if (pickingUp)
        {
            Collider[] colliders = Physics.OverlapSphere(rightHandTransform.position, pickupRadius);
            if (colliders.Length > 0)
            {
                Rigidbody closestRB = null;
                float shortestDist = Mathf.Infinity;
                foreach (Collider col in colliders)
                {
                    Rigidbody collidedRB = col.GetComponent<Rigidbody>();
                    if (collidedRB != null)
                    {
                        float dist = Vector3.Distance(col.transform.position, rightHandTransform.position);
                        if(dist < shortestDist)
                        {
                            shortestDist = dist;
                            closestRB = collidedRB;
                        }
                    }
                }
                closestRB.isKinematic = true;
                orgRightTransform = closestRB.transform.parent;
                closestRB.transform.parent = rightHandTransform;
                objInRightHand = closestRB;
            }
        }
        else
        {
            if(objInRightHand != null)
            {
                objInRightHand.isKinematic = false;
                objInRightHand.transform.parent = orgRightTransform;
                objInRightHand = null;
            }
        }
    }
    void OnLeftPickup(bool pickingUp)
    {
        if (pickingUp)
        {
            Collider[] colliders = Physics.OverlapSphere(leftHandTransform.position, pickupRadius);
            if (colliders.Length > 0)
            {
                Rigidbody closestRB = null;
                float shortestDist = Mathf.Infinity;
                foreach (Collider col in colliders)
                {
                    Rigidbody collidedRB = col.GetComponent<Rigidbody>();
                    if (collidedRB != null)
                    {
                        float dist = Vector3.Distance(col.transform.position, leftHandTransform.position);
                        if (dist < shortestDist)
                        {
                            shortestDist = dist;
                            closestRB = collidedRB;
                        }
                    }
                }
                closestRB.isKinematic = true;
                orgLeftTransform = closestRB.transform.parent;
                closestRB.transform.parent = leftHandTransform;
                objInLeftHand = closestRB;
            }
        }
        else
        {
            if (objInLeftHand != null)
            {
                objInLeftHand.isKinematic = false;
                objInLeftHand.transform.parent = orgLeftTransform;
                objInLeftHand = null;
            }
        }
    }
}
