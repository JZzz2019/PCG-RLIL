using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [Header("NumOfTile")]
    public int KeyNum;
    public int PlayerNum;
    public int WallNum;
    public int DoorNum;
    public int GroundNum;

    public void CountNum(int value)
    {
        switch (value)
        {
            case 0:
                PlayerNum += 1;
                break;
            case 1:
                KeyNum += 1;
                break;
            case 2:
                DoorNum += 1;
                break;
            case 3:
                WallNum += 1;
                break;
            case 4:
                GroundNum += 1;
                break;
        }
    }
}
