using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

//Description: This version focuses on iterating through a list of stored gameobjects. 
public class TileGeneration : Agent
{
    /// <summary>
    /// When the Agent requests a decision, it calls the methods once: observation-decision-action-reward;
    /// </summary>
    [Header("NumOfTile")]
    public int KeyNum;
    public int PlayerNum;
    public int WallNum;
    public int DoorNum;
    public int GroundNum;

    [Header("Items to be spawned")]
    public GameObject[] items;

    [Header("Size x and y of the grid")]
    public int GridX;
    public int GridY;

    [Header("Parent of the grid and list")]
    public GameObject parent;
    public List<GameObject> StoredObjects = new List<GameObject>();

    [Header("Number of actions in an episode")]
    public int CurrentCount = 0;
    public int InitialCount = 0;

    //ML agent and 2D array grid declared
    private Agent agent;
    private int[,] Grid;

    [Header("Selection Methods")]
    public bool NarrowMethod, WideMethod;

    [Header("Visual element")]
    public GameObject Frame;

    private void Awake()
    {
        Grid = new int[GridX, GridY];
        agent = GetComponent<Agent>();
    }

    private void Update()
    {
        if (StoredObjects.Count == GridX * GridY)
        {
            agent.RequestDecision();
        }
    }

    void GenerateGrid()
    {
        for (int x = 0; x < GridX; x++)
        {
            for (int y = 0; y < GridY; y++)
            {
                Grid[x, y] = Random.Range(0, items.Length);
                GameObject obj = null;
                Quaternion rotation = Quaternion.identity;
                Vector3 pos = new Vector3(x, y, 0);
                SpawnTile(pos, Grid[x,y], rotation, obj, 0, false);
            }
        }
    }

    void SpawnTile(Vector3 pos, int ItemValue, Quaternion rotation, GameObject obj, int objIndex, bool hasInitSpawned)
    {
        switch (ItemValue)
        {
            case 0: //Player
                obj = Instantiate(items[ItemValue], pos, rotation, parent.transform);
                CountNumAndName(ItemValue, obj);
                AddToList(obj, objIndex, hasInitSpawned);
                break;
            case 1: //Key
                obj = Instantiate(items[ItemValue], pos, rotation, parent.transform);
                CountNumAndName(ItemValue, obj);
                AddToList(obj, objIndex, hasInitSpawned);
                break;
            case 2: //Door
                obj = Instantiate(items[ItemValue], pos, rotation, parent.transform);
                CountNumAndName(ItemValue, obj);
                AddToList(obj, objIndex, hasInitSpawned);
                break;
            case 3: //Wall
                obj = Instantiate(items[ItemValue], pos, rotation, parent.transform);
                CountNumAndName(ItemValue, obj);
                AddToList(obj, objIndex, hasInitSpawned);
                break;
            case 4: //Ground
                obj = Instantiate(items[ItemValue], pos, rotation, parent.transform);
                CountNumAndName(ItemValue, obj);
                AddToList(obj, objIndex, hasInitSpawned);
                break;
        }
    }

    void AddToList(GameObject o, int indexInList, bool hasSpawned)
    {
        if (hasSpawned == false)
        {
            StoredObjects.Add(o);
        }
        else
        {
            StoredObjects.Insert(indexInList, o);
        }
    }

    void CountNumAndName(int value, GameObject o)
    {
        switch (value)
        {
            case 0:
                PlayerNum += 1;
                o.name = "Player";
                break;
            case 1:
                KeyNum += 1;
                o.name = "Key";
                break;
            case 2:
                DoorNum += 1;
                o.name = "Door";
                break;
            case 3:
                WallNum += 1;
                o.name = "Wall";
                break;
            case 4:
                GroundNum += 1;
                o.name = "Ground";
                break;
        }
    }

    public override void OnEpisodeBegin()
    {
        if (parent.transform.childCount >= GridX * GridY)
        {
            foreach (Transform child in parent.transform)
            {
                Destroy(child.gameObject);
                StoredObjects.Remove(child.gameObject);

            }
            CurrentCount = InitialCount;
            GenerateGrid();
        }
        else
        {
            CurrentCount = InitialCount;
            GenerateGrid();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(KeyNum);
        sensor.AddObservation(PlayerNum);
        sensor.AddObservation(WallNum);
        sensor.AddObservation(DoorNum);
        sensor.AddObservation(GroundNum);

        foreach (GameObject o in StoredObjects)
        {
            sensor.AddObservation(o);
        }

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int actionToTake = actions.DiscreteActions[0];

        Debug.Log(actionToTake);

        if (NarrowMethod == true)
        {
            NarrowSelection(actionToTake);
        }

        if (WideMethod == true)
        {
            WideSelection();
        }

        //Give reward for having exactly 1 key, 1 door and 1 player
        if (KeyNum == 0 && DoorNum == 0 && PlayerNum == 0)
        {
            AddReward(1f);
        }
    }

    void NarrowSelection(int actionToTake)
    {

        if (CurrentCount < GridX * GridY)
        {
            //Get current gameobject in list at a certain index
            GameObject o = StoredObjects[CurrentCount];
            Vector3 oPos = o.transform.position;
            SelectTile(oPos);

            switch (actionToTake)
            {
                case 0: //ignore tile , Give reward for ignore the same
                    if (o.name == "Key") { PositiveReward(actionToTake); }
                    if (o.name == "Player") { PositiveReward(actionToTake); }
                    if (o.name == "Wall") { PositiveReward(actionToTake); }
                    if (o.name == "Door") { PositiveReward(actionToTake); }
                    if (o.name == "Ground") { PositiveReward(actionToTake); }
                    break;
                case 1: //change to Player  
                    ChangeTile(o, actionToTake, oPos);
                    if (o.name == "Player") { NegativeReward(actionToTake); }
                    else { PositiveReward(actionToTake); }
                    break;
                case 2: //change to Key
                    ChangeTile(o, actionToTake, oPos);
                    if (o.name == "Key") { NegativeReward(actionToTake); }
                    else { PositiveReward(actionToTake); }
                    break;
                case 3: //change to Door
                    ChangeTile(o, actionToTake, oPos);
                    if (o.name == "Door") { NegativeReward(actionToTake); }
                    else { PositiveReward(actionToTake); }
                    break;
                case 4: //change to Wall
                    ChangeTile(o, actionToTake, oPos);
                    if (o.name == "Wall") { NegativeReward(actionToTake); }
                    else { PositiveReward(actionToTake); }
                    break;
                case 5: //change to Ground
                    ChangeTile(o, actionToTake, oPos);
                    if (o.name == "Ground") { NegativeReward(actionToTake); }
                    else { PositiveReward(actionToTake); }
                    break;
            }

            CurrentCount++;
        }
        else //If CurrentCount is above 25
        {
            ResetObservation();
            EndEpisode();
        }
    }

    void WideSelection()
    {

    }

    void ChangeTile(GameObject obj, int value, Vector3 objPos)
    {
        //Vector3 objPos = obj.transform.position;
        Quaternion rot = obj.transform.rotation;
        int index = StoredObjects.IndexOf(obj);
        bool hasAlrSpawned = true;
        AdjustObservation(obj);

        switch (value)
        {
            case 1: //Player
                Destroy(obj);
                StoredObjects.Remove(obj);
                SpawnTile(objPos, value - 1, rot, obj, index, hasAlrSpawned);
                break;
            case 2: //Key
                Destroy(obj);
                StoredObjects.Remove(obj);
                SpawnTile(objPos, value - 1, rot, obj, index, hasAlrSpawned);
                break;
            case 3: //Door
                Destroy(obj);
                StoredObjects.Remove(obj);
                SpawnTile(objPos, value - 1, rot, obj, index, hasAlrSpawned);
                break;
            case 4: //Wall
                Destroy(obj);
                StoredObjects.Remove(obj);
                SpawnTile(objPos, value - 1, rot, obj, index, hasAlrSpawned);
                break;
            case 5: //Ground
                Destroy(obj);
                StoredObjects.Remove(obj);
                SpawnTile(objPos, value - 1, rot, obj, index, hasAlrSpawned);
                break;
        }
    }

    void PositiveReward(int caseValue)
    {
        if (caseValue == 0)
        {
            AddReward(0.01f);
        }
        if (caseValue == 1)
        {
            AddReward(0.15f);
        }
        if (caseValue == 2)
        {
            AddReward(0.15f);
        }
        if (caseValue == 3)
        {
            AddReward(0.15f);
        }
        if (caseValue == 4)
        {
            AddReward(0.15f);
        }
        if (caseValue == 5)
        {
            AddReward(0.15f);
        }

    }

    void NegativeReward(int caseValue)
    {
        if (caseValue == 0)
        {
            AddReward(-0.2f);
        }
        if (caseValue == 1)
        {
            AddReward(-0.2f);
        }
        if (caseValue == 2)
        {
            AddReward(-0.2f);
        }
        if (caseValue == 3)
        {
            AddReward(-0.2f);
        }
        if (caseValue == 4)
        {
            AddReward(-0.2f);
        }
        if (caseValue == 5)
        {
            AddReward(-0.2f);
        }
    }

    void ResetObservation()
    {
        PlayerNum = 0;
        KeyNum = 0;
        DoorNum = 0;
        WallNum = 0;
        GroundNum = 0;
    }

    void AdjustObservation(GameObject obj)
    {
        if (obj.name == "Player")
        {
            PlayerNum -= 1;
        }
        if (obj.name == "Key")
        {
            KeyNum -= 1;
        }
        if (obj.name == "Door")
        {
            DoorNum -= 1;
        }
        if (obj.name == "Wall")
        {
            WallNum -= 1;
        }
        if (obj.name == "Ground")
        {
            GroundNum -= 1;
        }
    }

    //Visual selection on current tile
    void SelectTile(Vector3 pos)
    {
        if (Frame.activeSelf != true)
        {
            Frame.SetActive(true);
        }

        pos.z = -0.1f;
        Frame.transform.position = pos;
    }
}
