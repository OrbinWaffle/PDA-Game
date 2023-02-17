using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    [Tooltip("The distance between each individal notch")]
    [SerializeField] float notchDistance = .1f;
    // The current distance from initialNotchTransform to the place we want to actually add the notch
    float currentNotchDistance = 0f;
    Stack<GameObject> notchStack = new Stack<GameObject>();
    string finalString;
    Stack<string> keyStack = new Stack<string>();
    [SerializeField] Transform initialNotchTransform;
    [SerializeField] GameObject initialNotch;
    void Start()
    {
        keyStack.Push("z");
        AddNotch(initialNotch);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check that collided object is indeed a lock
        if(collision.gameObject.CompareTag("Lock"))
        {
            LockController LC = collision.gameObject.GetComponent<LockController>();
            if(LC.lockEnabled == false)
            {
                return;
            }
            // Ensure that the key matches the collided lock's symbol requirements
            bool isValid = TestTransition(LC.GetAcceptingSymbol(), LC.GetStackRequirement(), LC.GetStackReplacement(), LC.GetNotches());
            if (isValid) { LC.OnChosen(); }
        }
    }
    // Given the current state of the key, process what notches to remove/add and return whether the key meets lock requirements
    bool TestTransition(string acceptingSymbol, string stackRequirement, string[] stackReplacement, GameObject[] notches)
    {
        if(keyStack.Peek() != stackRequirement && stackRequirement != "-")
        {
            return false;
        }
        // If the new symbol is epsilon, we do not add anything to the final string
        // Otherwise, add symbol
        if (acceptingSymbol != "epsilon")
        {
            finalString += acceptingSymbol;
        }
        string topOfStack = keyStack.Peek();
        GameObject topOfStackObj = notchStack.Peek();
        // In a real PDA, the top of the stack is replaced with something.
        // This initial pop removes the top of the stack to simulate replacement.
        keyStack.Pop();
        RemoveNotch();
        // If we want to pop stuff off the stack without adding anything
        if (stackReplacement[0] == "epsilon")
        {
            return true;
        }
        // Adding all notches to key
        for(int i = 0; i < stackReplacement.Length; ++i)
        {
            if (stackReplacement[i] != "-")
            {
                keyStack.Push(stackReplacement[i]);
                AddNotch(notches[i]);
            }
            else
            {
                keyStack.Push(topOfStack);
                AddNotch(topOfStackObj);
            }
        }
        return true;
    }

    void AddNotch(GameObject notchToAdd)
    {
        Vector3 instantiatePos = initialNotchTransform.position + transform.forward * currentNotchDistance;
        GameObject newNotch = Instantiate(notchToAdd, instantiatePos, transform.rotation, transform);
        notchStack.Push(newNotch);
        currentNotchDistance += notchDistance;
    }
    void RemoveNotch()
    {
        currentNotchDistance -= notchDistance;
        Destroy(notchStack.Peek());
        notchStack.Pop();
    }
}
