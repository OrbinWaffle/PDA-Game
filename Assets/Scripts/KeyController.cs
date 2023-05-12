using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyController : MonoBehaviour
{
    [Tooltip("The distance between each individal notch")]
    [SerializeField] float notchDistance = .1f;
    // The current distance from initialNotchTransform to the place we want to actually add the notch
    float currentNotchDistance = 0f;
    Stack<GameObject> notchStack = new Stack<GameObject>();
    string finalString = "";
    Stack<string> keyStack = new Stack<string>();
    [SerializeField] Transform initialNotchTransform;
    [SerializeField] GameObject initialNotch;
    [SerializeField] TextMeshProUGUI keyText;
    [SerializeField] ParticleSystem fireParticle;
    [SerializeField] ParticleSystem waterParticle;
    [SerializeField] ParticleSystem lightningParticle;
    [SerializeField] AudioClip lockSound;
    [SerializeField] AudioClip failSound;
    public static KeyController instance;

    Rigidbody RB;
    LockController currentLock;
    private AudioSource audSource;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        keyStack.Push("z");
        AddNotch(initialNotch);
        audSource = GetComponent<AudioSource>();
        RB = GetComponent<Rigidbody>();
        GameManager.instance.LoadString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivateLock()
    {
        RB.isKinematic = false;
        if (currentLock != null)
        {
            LockController LC = currentLock.gameObject.GetComponent<LockController>();
            if (LC.lockEnabled == false)
            {
                return;
            }
            // Ensure that the key matches the collided lock's symbol requirements
            bool isValid = TestTransition(LC.GetAcceptingSymbol(), LC.GetStackRequirement(), LC.GetStackReplacement());
            if (isValid || LC.GetType() == typeof(MenuLockController))
            {
                audSource.clip = lockSound;
                audSource.Play();
                LC.OnChosen();
                Transform keyPos = LC.GetKeyPos();
                transform.position = keyPos.position;
                transform.rotation = keyPos.rotation;
                RB.isKinematic = true;
            }
            else
            {
                audSource.clip = failSound;
                audSource.Play();
            }
        }
    }

    public void OnPickup()
    {
    }
    public string GetFinalString()
    {
        return finalString;
    }
    public void SetString(string newString)
    {
        finalString = newString;
        keyText.text = finalString;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check that collided object is indeed a lock
        if (other.gameObject.CompareTag("Lock"))
        {
            currentLock = other.GetComponent<LockController>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Lock"))
        {
            currentLock = null;
        }
    }
    // Given the current state of the key, process what notches to remove/add and return whether the key meets lock requirements
    bool TestTransition(string acceptingSymbol, string stackRequirement, string[] stackReplacement)
    {
        if(keyStack.Peek() != stackRequirement && stackRequirement != "v")
        {
            return false;
        }
        // If the new symbol is epsilon, we do not add anything to the final string
        // Otherwise, add symbol
        if (acceptingSymbol != "epsilon")
        {
            finalString += acceptingSymbol;
            keyText.text = finalString;
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
            UpdateParticles();
            return true;
        }
        // Adding all notches to key
        for(int i = 0; i < stackReplacement.Length; ++i)
        {
            if (stackReplacement[i] != "v")
            {
                keyStack.Push(stackReplacement[i]);
                AddNotch(GemDictionary.main.GetObject(stackReplacement[i]));
            }
            else
            {
                keyStack.Push(topOfStack);
                AddNotch(topOfStackObj);
            }
        }
        UpdateParticles();
        return true;
    }
    void UpdateParticles()
    {
        fireParticle.Stop();
        lightningParticle.Stop();
        waterParticle.Stop();
        if(keyStack.Peek() == "R"){fireParticle.Play();}
        if(keyStack.Peek() == "S"){waterParticle.Play();}
        if(keyStack.Peek() == "h"){lightningParticle.Play();}
    }

    void AddNotch(GameObject notchToAdd)
    {
        Vector3 instantiatePos = initialNotchTransform.position + initialNotchTransform.forward * currentNotchDistance;
        GameObject newNotch = Instantiate(notchToAdd, instantiatePos, initialNotchTransform.rotation, transform);
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
