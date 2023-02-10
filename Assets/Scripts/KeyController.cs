using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    string finalString;
    Stack<string> keyStack = new Stack<string>();
    void Start()
    {
        keyStack.Push("z");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Lock"))
        {
            LockController LC = collision.gameObject.GetComponent<LockController>();
            TestTransition(LC.GetAcceptingSymbol(), LC.getStackRequirement(), LC.getStackReplacement());
        }
    }
    void TestTransition(string acceptingSymbol, string stackRequirement, string[] stackReplacement)
    {
        if(keyStack.Peek() != stackRequirement)
        {
            return;
        }
        Debug.Log("AYO WE VALID");
        finalString += acceptingSymbol;
        keyStack.Pop();
        if (stackReplacement[0] == "pop")
        {
            return;
        }
        foreach(string symbol in stackReplacement)
        {
            keyStack.Push(symbol);
        }
    }
}
