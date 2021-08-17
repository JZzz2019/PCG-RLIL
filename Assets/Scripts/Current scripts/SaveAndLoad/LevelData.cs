using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneratorClass;

public class LevelData
{
    //private string name;
    public int sizeOfGrid;
    public int GridX;
    public int GridY;
    public GameObject[] obj;

    public LevelData (TileGenerationV2 Instance)
    {
        sizeOfGrid = Instance.GridX * Instance.GridY;
        this.GridX = Instance.GridX;
        this.GridY = Instance.GridY;
        for (int i = 0; i < sizeOfGrid; i++)
        {
            obj[i] = Instance.StoredObjects[i];
        }
    }
}
