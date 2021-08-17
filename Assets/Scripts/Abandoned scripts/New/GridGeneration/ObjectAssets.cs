using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAssets : MonoBehaviour
{
    public static ObjectAssets Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    //The order of the prefabs in the array must follow the order of enum in Object
    public GameObject[] g_Tiles;
    
}
