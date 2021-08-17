using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAction : MonoBehaviour
{
    public int TakeAnAction(bool PrepareToReturn)
    {
        if (PrepareToReturn == true)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                return 0;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                return 1;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                return 2;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                return 3;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                return 4;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                return 5;
            }
            else
            {
                return 6;
            }
        }
        else
        {
            return 6;
        }
    }


}
