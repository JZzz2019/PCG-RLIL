using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IamJ : MonoBehaviour
{
    private int i = 5;
    private int x = 8;
    private int y = -64;

    private float f = 0.6543f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float together = (float)i / (float)x;
        together = y;
        float asdwa = f * x;
        Debug.Log(together);
        Debug.Log(asdwa + "asdwas");
    }
}
