using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class NewMLGrid : Agent
{
    [Header("Stopping int")]
    public int ChangeTileValue;

    [Header("NumOfTile")]
    public int KeyNum;
    public int PlayerNum;
    public int WallNum;
    public int DoorNum;
    public int GroundNum;

    [Header("Items and Base")]
    public GameObject[] items;
    public Transform parent;
    public GameObject Frame;

    [Header("Other properties")]
    public int GridX, GridY;

    [Header("Selection method")]
    public bool NarrowMethod;
    public bool TurtleMethod;
    public bool WideMethod;

    [Range(0, 1)]
    public float outline;

    [Header("Stored objects in 2D array")]
    public List<GameObject> StoredObj = new List<GameObject>();
    public GameObject[,] Grid;

    public bool requestAction;

    //ML Agent
    private Agent agent;
    void Awake()
    {
        //Grid = new int[GridX, GridY];
        Grid = new GameObject[GridX, GridY];
        agent = GetComponent<Agent>();
        //GenerateGrid();
    }

    void Start()
    {
        if (NarrowMethod == true)
        {
            //NarrowSelection();
        }
        if (TurtleMethod == true)
        {

        }
        if (WideMethod == true)
        {

        }
    }

    void GenerateGrid()
    {
        for (int x = 0; x < GridX; x++)
        {
            for (int y = 0; y < GridY; y++)
            {
                int i = Random.Range(0, items.Length);
                SelectSpawn(Grid[x, y], x, y, i);
            }
        }
    }

    void SelectSpawn(GameObject obj, int x, int y, int i)
    {
        obj = (GameObject)Instantiate(items[i], new Vector3(x, y, 0), Quaternion.identity);
        obj.transform.SetParent(parent);
        obj.transform.localScale = Vector3.one * (1 - outline);
        if (i == 0) { obj.name = "Key"; }
        if (i == 1) { obj.name = "Player"; }
        if (i == 2) { obj.name = "Wall"; }
        if (i == 3) { obj.name = "Door"; }
        if (i == 4) { obj.name = "Ground"; }
        StoredObj.Add(obj);

        foreach (GameObject o in StoredObj)
        {
            if (o.name == "Key") { KeyNum += 1; }
            if (o.name == "Player") { PlayerNum += 1; }
            if (o.name == "Wall") { WallNum += 1; }
            if (o.name == "Door") { DoorNum += 1; }
            if (o.name == "Ground") { GroundNum += 1; }
        }

    }

    //void NarrowSelection()
    //{
    //    if (parent.childCount >= (GridX * GridY))
    //    {
    //        foreach (GameObject o in StoredObj)
    //        {
    //            if (o.name == "Key") { KeyNum += 1; }
    //            if (o.name == "Player") { PlayerNum += 1; }
    //            if (o.name == "Wall") { WallNum += 1; }
    //            if (o.name == "Door") { DoorNum += 1; }
    //            if (o.name == "Ground") { GroundNum += 1; }
    //        }

    //        foreach (GameObject o in StoredObj)
    //        {
    //            Vector3 pos = o.transform.position;
    //            pos.z = -0.1f;
    //            SelectTile(pos);

    //            if (o.name == "Key" && KeyNum > 1)
    //            {
    //                KeyNum -= 1;
    //                o.SetActive(false);
    //            }
    //            if (o.name == "Player" && PlayerNum > 1)
    //            {
    //                PlayerNum -= 1;
    //                o.SetActive(false);
    //            }
    //            if (o.name == "Door" && DoorNum > 1)
    //            {
    //                DoorNum -= 1;
    //                o.SetActive(false);
    //            }
    //        }
    //    }
    //}

    void SelectTile(Vector3 pos) //Visual selection on current tile
    {
        if (Frame.activeSelf != true)
        {
            Frame.SetActive(true);
        }
        Frame.transform.position = pos;
    }

    //The state of every beginning episode


    private void FixedUpdate()
    {
       if (requestAction == true)
        {
            agent.RequestDecision();
        }
    }

    public override void OnEpisodeBegin()
    {
        //Destroy grid
        foreach (GameObject o in StoredObj)
        {
            StoredObj.Remove(o);
            Destroy(o);
        }
        //Generate grid
        GenerateGrid();
        requestAction = true;
    }
    //Observation of the environment
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(KeyNum);
        sensor.AddObservation(PlayerNum);
        sensor.AddObservation(WallNum);
        sensor.AddObservation(DoorNum);
        sensor.AddObservation(GroundNum);

        //for (int x = 0; x < Grid.Length; x++)
        //{
        //    for (int y = 0; y < Grid.Length; y++)
        //    {
        //        sensor.AddObservation(Grid[x, y]);
        //    }
        //}
        foreach (GameObject o in StoredObj)
        {
            sensor.AddObservation(o);
        }

    }

    //actions to take regarding the observation
    public override void OnActionReceived(ActionBuffers actions)
    {
        int actionToTake = actions.DiscreteActions[0];

        if (parent.childCount >= (GridX * GridY))
        {
            foreach (GameObject o in StoredObj)
            {
                Vector3 pos = o.transform.position;
                pos.z = -0.1f;
                SelectTile(pos);
                //agent.RequestDecision();
                switch (actionToTake)
                {
                    case 0: //ignore tile , Give reward for ignore the same
                        if (o.name == "Key") { agent.AddReward(0.05f); }
                        if (o.name == "Player") { agent.AddReward(0.05f); }
                        if (o.name == "Wall") { agent.AddReward(0.05f); }
                        if (o.name == "Door") { agent.AddReward(0.05f); }
                        if (o.name == "Ground") { agent.AddReward(0.05f); }
                        ChangeTileValue += 1;
                        break;
                    case 1: //change to key  
                        Destroy(o);
                        int ObjectType = CheckObjectType(o); 
                        ObjectType -= 1;
                        KeyNum += 1;
                        GameObject newObj0 = Instantiate(items[0], pos, Quaternion.identity);
                        newObj0.transform.SetParent(parent);
                        if (o.name != "Key") { agent.AddReward(0.1f); } //reward given
                        else { agent.AddReward(-0.1f); } //Negative reward
                        ChangeTileValue += 1;
                        break;
                    case 2: //change to player
                        Destroy(o);
                        int ObjectType1 = CheckObjectType(o);
                        ObjectType1 -= 1;
                        PlayerNum += 1;
                        GameObject newObj1 = Instantiate(items[1], pos, Quaternion.identity);
                        newObj1.transform.SetParent(parent);
                        if (o.name != "Player") { agent.AddReward(0.1f); }
                        else
                        {
                            //negative reward
                            agent.AddReward(-0.1f);
                        }
                        ChangeTileValue += 1;
                        break;
                    case 3: //change to wall
                        Destroy(o);
                        int ObjectType2 = CheckObjectType(o);
                        ObjectType2 -= 1;
                        WallNum += 1;
                        GameObject newObj2 = Instantiate(items[2], pos, Quaternion.identity);
                        newObj2.transform.SetParent(parent);
                        if (o.name != "Wall") { agent.AddReward(0.1f); }
                        else
                        {
                            //negative reward
                            agent.AddReward(-0.1f);
                        }
                        ChangeTileValue += 1;
                        break;
                    case 4: //change to door
                        Destroy(o);
                        int ObjectType3 = CheckObjectType(o);
                        ObjectType3 -= 1;
                        DoorNum += 1;
                        GameObject newObj3 = Instantiate(items[3], pos, Quaternion.identity);
                        newObj3.transform.SetParent(parent);
                        if (o.name != "Door") { agent.AddReward(0.1f); }
                        else
                        {
                            agent.AddReward(-0.1f);
                        }
                        ChangeTileValue += 1;
                        break;
                    case 5: //change to ground
                        Destroy(o);
                        int ObjectType4 = CheckObjectType(o);
                        ObjectType4 -= 1;
                        GroundNum += 1;
                        GameObject newObj4 = Instantiate(items[4], pos, Quaternion.identity);
                        newObj4.transform.SetParent(parent);
                        if (o.name != "Ground") { agent.AddReward(0.1f); }
                        else
                        {
                            agent.AddReward(-0.1f);
                        }
                        ChangeTileValue += 1;
                        break;
                }
            }
        }

        if (KeyNum == 1)
        {
            agent.AddReward(0.3f);
        }
        if (PlayerNum == 1)
        {
            agent.AddReward(0.3f);
        }
        if (DoorNum == 1)
        {
            agent.AddReward(0.3f);
        }

        if (ChangeTileValue > (GridX * GridY))
        {
            EndEpisode();
            ChangeTileValue = 0;
        }
    }

    int CheckObjectType(GameObject o)
    {
        if (o.name == "Key")
        {
            return KeyNum;
        }
        else if (o.name == "Player")
        {
            return PlayerNum;
        }
        
        else if (o.name == "Door")
        {
            return DoorNum;
        }
       
        else if (o.name == "Wall")
        {
            return WallNum;
        }
        
        else if (o.name == "Ground")
        {
            return GroundNum;
        }

        return 0;
    }
}
