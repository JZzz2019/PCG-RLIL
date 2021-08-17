using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class RL_GenerateGrid : Agent
{
    [Header("NumOfTile")]
    public int KeyNum;
    public int PlayerNum;
    public int WallNum;
    public int DoorNum;
    public int GroundNum;

    public int[] array;
    public GameObject[,] objects;


    [Header("Items and Base")]
    public GameObject[] items;
    public Transform parent;
    public GameObject Frame;

    [Header("Other properties")]
    public int GridX, GridY;
    private float tilesize = 1f;

    [Header("Selection method")]
    public bool NarrowMethod;
    public bool TurtleMethod;
    public bool WideMethod;

    [Range(0, 1)]
    public float outline;

    [Header("Stored objects and positions")]
    public List<GameObject> StoredObjects = new List<GameObject>();

    //The state of every beginning episode
    public override void OnEpisodeBegin()
    {
        GenerateGrid();
    }
    //Observation of the environment
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(KeyNum);
        sensor.AddObservation(PlayerNum);
        sensor.AddObservation(WallNum);
        sensor.AddObservation(DoorNum);
        sensor.AddObservation(GroundNum);

        for (int i = 0; i < objects.Length; i++)
        {
            for (int j = 0; j < objects.Length; j++)
            {
                sensor.AddObservation(objects[i, j]);
            }
        }

    }

    //actions to take regarding the observation
    public override void OnActionReceived(ActionBuffers actions)
    {
        
    }

    void GenerateGrid()
    {
        //Create a gameobject called "Generated Grid" in the hierarchy 
        //string parentName = "Generated Grid";
        //Transform parent = new GameObject(parentName).transform;

        for (int y = 0; y < GridY; y++)
        {
            for (int x = 0; x < GridX; x++)
            {
                //yield return new WaitForSeconds(0.1f);
                Vector3 position = new Vector3(x * tilesize - (GridX - 0.5f), -y * tilesize - (GridY - 0.5f), 0);
                int i = Random.Range(1, 2);
                SelectAndSpawn(i, position, Quaternion.identity, parent);
            }
        }
    }

    void SelectAndSpawn(int TileType, Vector3 position, Quaternion rotation, Transform parent)
    {
        switch (TileType)
        {
            case 1:
                int x = Random.Range(0, items.Length); //0 = key, 1 = player, 2 = Wall, 3 = Door, 4 = Ground
                GameObject obj = Instantiate(items[x], position, rotation);
                obj.transform.localScale = Vector3.one * (1 - outline);
                obj.transform.SetParent(parent);
                if (x == 0) { obj.name = "Key"; }
                if (x == 1) { obj.name = "Player"; }
                if (x == 2) { obj.name = "Wall"; }
                if (x == 3) { obj.name = "Door"; }
                if (x == 4) { obj.name = "Ground"; }
                StoreGameObjects(obj);
                break;
        }
    }

    void StoreGameObjects(GameObject obj)
    {
        StoredObjects.Add(obj);
    }

}
