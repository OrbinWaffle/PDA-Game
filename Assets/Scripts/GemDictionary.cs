using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemDictionary : MonoBehaviour
{
    [SerializeField] string[] symbols;
    [SerializeField] GameObject[] objects;
    public Dictionary<string, GameObject> gemPairs;
    public static GemDictionary main;
    private void Awake()
    {
        main = this;
        gemPairs = new Dictionary<string, GameObject>();
        for (int i = 0; i < symbols.Length; ++i)
        {
            gemPairs.Add(symbols[i], objects[i]);
        }
    }
    public GameObject GetObject(string symbol)
    {
        return gemPairs[symbol];
    }
}
