using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object
{
   public enum ObjectType
    {
        GroundTile, //0
        KeyTile,    //1
        DoorTile,   //2
        PlayerTile, //3
        WallTile    //4
    }

    public ObjectType objectType;

    public GameObject GetObject(ObjectType objectType)
    {
        switch (objectType)
        {
            default:
            case ObjectType.GroundTile:
                return ObjectAssets.Instance.g_Tiles[0];
            case ObjectType.KeyTile:
                return ObjectAssets.Instance.g_Tiles[1];
            case ObjectType.DoorTile:
                return ObjectAssets.Instance.g_Tiles[2];
            case ObjectType.PlayerTile:
                return ObjectAssets.Instance.g_Tiles[3];
            case ObjectType.WallTile:
                return ObjectAssets.Instance.g_Tiles[4];
        }
    }
}
