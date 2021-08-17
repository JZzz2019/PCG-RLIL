using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectList
{
    private List<Object> objectList;

    public ObjectList()
    {
        objectList = new List<Object>();

        //AddObject(new Object { objectType = Object.ObjectType.DoorTile, NumOfObj = 1 });
        Debug.Log(objectList.Count);
    }

    public void AddObject(Object _object)
    {
        objectList.Add(_object);
    }
}
