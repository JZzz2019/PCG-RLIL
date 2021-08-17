using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

//Description: The second version focuses on iterating through a 2D array of objects. Therefore, it has more control over specific
//locations of game objects

/// <summary>
/// When the Agent requests a decision, it calls the methods once: observation-decision-action-reward;
/// </summary>

namespace GeneratorClass
{
    public class TileGenerationV2 : Agent
    {
        #region Declared variables
        [Header("Important number Of Tiles")]
        public int KeyNum;
        public int DoorNum;
        public int PlayerNum;
        [Header("Less important number Of Tiles")]
        public int WallNum;
        public int GroundNum;

        [Header("Targeted number of objects")]
        //public int TargetPlayer;
        //public int TargetKey;
        //public int TargetDoor;
        public int TargetNum;
        public int TargetWallNum;

        [Header("Previous and Current numbers for Key,Player and Door")]
        public int PrevKeyNum;
        public int PrevDoorNum;
        public int PrevPlayerNum;
        public int CurKeyNum;
        public int CurDoorNum;
        public int CurPlayerNum;

        [Header("Items to be spawned")]
        public GameObject[] items;

        [Header("Size x and y of the grid")]
        public int GridX;
        public int GridY;

        [Header("Number of actions in an episode")]
        public int CurrentCount = 0;
        public int InitialCount = 0;

        [Header("2D Array: X and Y")]
        public int CountX = 0;
        public int CountY = 0;

        //ML agent and Enum BehaviourType
        private Agent agent;
        public BehaviorType behaviourType;

        //Grid to define positions 
        public GameObject[,] Grid;

        //Two lists and arrays to temporarily store a list of objects at the last of an episode
        //Temp list to store important tiles such player, key and door temporarily 
        //x and y arrays to store important tiles data
        public List<GameObject> StoredObjects;
        public List<GameObject> TempList;

        [Header("Selection Methods")]
        public bool NarrowMethod, WideMethod, TurtleMethod;

        [Header("Visual element")]
        public GameObject Frame;
        public GameObject SelectedFrame;

        //Enums to define state and action to control the generating process
        private enum State
        {
            WaitingForInput,
            Processing
        }
        private enum Action
        {
            Ignore,
            ToPlayer,
            ToKey,
            ToDoor,
            ToWall,
            ToGround
        }
        private State state;
        private Action action;

        //Refers to texts in canvas, give visual effects when selected
        public Text[] texts;

        public Text Episode;
        private int episodeCount;
        public Text SolvableLevel;
        private int solvableLevelCount;

        private Narrow narrow;
        private Turtle turtle;
        private Wide wide;

        private Solver solver;
        private RewardFunction rewardFunc;

        [Header("Reference to this class")]
        public TileGenerationV2 TGV2;

        private GameObject[,] level;
        private GameObject Storedlevel;

        private float xDirection = 0f;
        #endregion
        private void Awake()
        {
            //Create an instance for Narrow class
            if (NarrowMethod == true)
            {
                narrow = new Narrow(TGV2, Frame, SelectedFrame);
            }
            if (TurtleMethod == true)
            {
                turtle = new Turtle(TGV2, Frame, SelectedFrame);
            }
            if (WideMethod == true)
            {
                wide = new Wide(TGV2, Frame, SelectedFrame);
            }


            solver = new Solver(TGV2);
            rewardFunc = new RewardFunction(TGV2);

            //Initialise for the solver particularly
            StoredObjects = new List<GameObject>();
            TempList = new List<GameObject>();

            //Initialize for the grid
            Grid = new GameObject[GridX, GridY];
            agent = GetComponent<Agent>();
            behaviourType = GetComponent<BehaviorParameters>().BehaviorType;

            //If not heuristic, state is processing
            if (behaviourType == BehaviorType.HeuristicOnly)
            {
                state = State.WaitingForInput;
            }
            else
            {
                state = State.Processing;
            }
        }

        private void Update()
        {
            if (behaviourType == BehaviorType.HeuristicOnly)
            {
                int i;
                if (Input.GetKeyDown(KeyCode.Keypad0))
                {
                    i = 0;
                    Deselect(i);
                    action = Action.Ignore;
                }
                else if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    i = 1;
                    Deselect(i);
                    action = Action.ToPlayer;
                }
                else if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    i = 2;
                    Deselect(i);
                    action = Action.ToKey;
                }
                else if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    i = 3;
                    Deselect(i);
                    action = Action.ToDoor;
                }
                else if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    i = 4;
                    Deselect(i);
                    action = Action.ToWall;
                }
                else if (Input.GetKeyDown(KeyCode.Keypad5))
                {
                    i = 5;
                    Deselect(i);
                    action = Action.ToGround;
                }

                //To confirm the key pressed for heuristic
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    state = State.Processing;
                }

                if (state == State.Processing)
                {
                    if (transform.childCount == GridX * GridY)
                    {
                        agent.RequestDecision();
                    }
                }
            }

            Episode.text = "Episode: " + episodeCount;
            SolvableLevel.text = "Solvable level: " + solvableLevelCount;
        }

        private void Deselect(int SelectedAction)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                if (i == SelectedAction)
                {
                    texts[i].color = Color.blue;
                }
                else
                {
                    texts[i].color = Color.black;
                }
            }
        }
        private void GenerateGrid()
        {
            for (int x = 0; x < GridX; x++)
            {
                for (int y = 0; y < GridY; y++)
                {
                    int RandomValue = Random.Range(0, items.Length);
                    GameObject o = null;
                    SpawnTile(o, x, y, RandomValue, true);
                }
            }
            //Debug.Log("Generate grid");
        }

        private void SpawnTile(GameObject obj, int x, int y, int ItemValue, bool forInit)
        {
            obj = Instantiate(items[ItemValue], new Vector3(x, y, 0), Quaternion.identity, transform);
            Grid[x, y] = obj;

            CountNumAndName(ItemValue, obj);
            //When initialising a random grid, start request decision
            if (forInit == true && transform.childCount >= GridX * GridY)
            {
                if (state == State.Processing)
                {
                    agent.RequestDecision();
                }
            }
            //Debug.Log("I am in spawntile");

        }

        private void CountNumAndName(int value, GameObject o)
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
            //If there is object in the transform
            if (transform.childCount >= GridX * GridY)
            {
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
                StoredObjects.Clear();
                TempList.Clear();
                GenerateGrid();
            }
            else
            {
                GenerateGrid();
            }

            CurrentCount = InitialCount;
            PrevPlayerNum = 0;
            PrevKeyNum = 0;
            PrevDoorNum = 0;
            CurPlayerNum = 0;
            CurKeyNum = 0;
            CurDoorNum = 0;
            episodeCount++;

        }

        public override void CollectObservations(VectorSensor sensor)
        {
            if (state == State.Processing)
            {
                //Debug.Log("CollectObservations");
                sensor.AddObservation(PrevKeyNum); //1
                sensor.AddObservation(PrevPlayerNum); //2
                sensor.AddObservation(PrevDoorNum); //3
                sensor.AddObservation(CurKeyNum); //4
                sensor.AddObservation(CurPlayerNum); //5
                sensor.AddObservation(CurDoorNum); //6


                //The observation of each tile at each state within an episode
                if (CurrentCount <= GridX * GridY)
                {
                    for (int x = CountX; x <= CountX; x++)
                    {
                        for (int y = CountY; y <= CountY; y++)
                        {
                            sensor.AddObservation(Grid[x, y]); //7
                        }
                    }
                }

                //Add observation for the whole grid
                for (int x = 0; x < GridX; x++)
                {
                    for (int y = 0; y < GridY; y++)
                    {
                        sensor.AddObservation(Grid[x, y]); //25 + 7 = 32
                    }
                }

                //Total of 32 observation size 
            }
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            if (state == State.Processing)
            {
                //6 editting actions from 0 to 5
                int actionToTake = actions.DiscreteActions[0];
                Debug.Log(actionToTake + " actionToTake");
                if (NarrowMethod == true)
                {
                    narrow.NarrowSelection(actionToTake);
                }

                //Not implemented
                if (TurtleMethod == true)
                {
                    //For Turtle Selection
                    //Up, Down, Left, Right: Four actions
                    int turtleMovementAction = actions.DiscreteActions[1];
                    Debug.Log(turtleMovementAction + " TurtleMovement");
                    turtle.TurtleSelection();
                }

                //Not implemented
                if (WideMethod == true)
                {

                    //For Wide Selection
                    //6 size: GridX and GridY are between 0 and 5
                    int wideMovementActionX = actions.DiscreteActions[2];
                    int wideMovementActionY = actions.DiscreteActions[3];
                    Debug.Log(wideMovementActionX + " WideMovementX");
                    Debug.Log(wideMovementActionY + " WideMovementX");
                    wide.WideSelection();
                }
                float reward = 0.15f / (GridX * GridY);

                //Debug.Log(reward);

                //Stage 2 of reward function
                for (int i = 0; i < items.Length; i++)
                {
                    if (i == 0)
                    {
                        PrevKeyNum = CurKeyNum;
                        CurKeyNum = KeyNum;
                        AddReward(reward * rewardFunc.CalculateReward(PrevKeyNum, CurKeyNum));
                    }
                    if (i == 1)
                    {
                        PrevDoorNum = CurDoorNum;
                        CurDoorNum = DoorNum;
                        AddReward(reward * rewardFunc.CalculateReward(PrevDoorNum, CurDoorNum));
                    }
                    if (i == 2)
                    {
                        PrevPlayerNum = CurPlayerNum;
                        CurPlayerNum = PlayerNum;
                        AddReward(reward * rewardFunc.CalculateReward(PrevPlayerNum, CurPlayerNum));
                    }
                    if (i == 3)
                    {
                        //Reward is being multiplied by WallNum/TargetWallNum. The Highest reward can only be 
                        //achieved by having exactly the number of wall tiles
                        if (TargetWallNum > GridX * GridY)
                        {
                            Debug.LogWarning("Your targeted wall number is " + TargetWallNum + " which is higher than your " + GridX * GridY);
                        }
                        else
                        {
                            //Make the wallreward divided by grid size so wont add more to cumulative reward
                            //than the sparse reward - solvable level
                            //wallNum and TargetWallNum are ints, cast to float
                            float wallReward = 0.3f / (GridX * GridY);
                            float WallLeftToTar = (float)WallNum / (float)TargetWallNum;
                            if (WallNum > TargetWallNum)
                            {
                                //WallLeftToTar = -1;
                                AddReward(-0.05f);
                            }
                            else
                            {
                                //Debug.Log(WallLeftToTar + " WallLeftToTarget");
                                AddReward(wallReward * WallLeftToTar);
                            }
                        }
                    }
                    //Maintaining walls and grounds
                    if (i == 4)
                    {
                        float groundReward = 0.1f / (GridX * GridY);
                        float RequiredGround = (float)GroundNum / ((GridX * GridY) - (float)TargetWallNum);
                        //Debug.Log(RequiredGround);
                        if (GroundNum > ((GridX * GridY) - (float)TargetWallNum))
                        {
                            AddReward(-0.05f);
                        }
                        else
                        {
                            AddReward(groundReward * RequiredGround);
                        }
                    }
                }
                
                //Add reward for having exactly 1 key, 1 door and 1 player
                if (KeyNum == TargetNum && DoorNum == TargetNum && PlayerNum == TargetNum)
                {
                    //Stage 3
                    AddReward(0.3f);
                    solver.PerformSolver();
                    solver.IsPath(Grid, GridX, GridY);
                    if (solver.IsSolvable() == true)
                    {
                        //Stage 4
                        //Add 0.4f for having solvable level and reset to default setting and end episode
                        AddReward(0.4f);
                        ResetToDefault();
                        Debug.Log("The level is solvable");
                        solvableLevelCount++;
                        ShowLevel();
                        EndEpisode();
                        //Store the solvable level into something
                    }
                    else
                    {
                        //reward for not being able to solve
                        StoredObjects.Clear();
                        TempList.Clear();
                    }
                }

                if (behaviourType == BehaviorType.HeuristicOnly)
                {
                    state = State.WaitingForInput;
                }

                //Second condition to end the episode, if currentcount is equal to x times y for grid
                if (CurrentCount == GridX * GridY)
                {
                    ResetToDefault();
                    EndEpisode();
                }
                else
                {
                    //not equal to heuristic
                    if (behaviourType != BehaviorType.HeuristicOnly)
                    {
                        agent.RequestDecision();
                    }
                }
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            ActionSegment<int> ChosenAction = actionsOut.DiscreteActions;

            switch (action)
            {
                case Action.Ignore: //0
                    ChosenAction[0] = 0;
                    break;
                case Action.ToPlayer: //1
                    ChosenAction[0] = 1;
                    break;
                case Action.ToKey: //2
                    ChosenAction[0] = 2;
                    break;
                case Action.ToDoor: //3
                    ChosenAction[0] = 3;
                    break;
                case Action.ToWall: //4
                    ChosenAction[0] = 4;
                    break;
                case Action.ToGround: //5
                    ChosenAction[0] = 5;
                    break;
            }

        }

        public void ChangeTile(GameObject obj, int value, int x, int y)
        {
            AdjustObservation(obj);
            bool forInit = false;
            switch (value)
            {
                case 1: //Player
                    Destroy(obj);
                    SpawnTile(obj, x, y, value - 1, forInit);
                    break;
                case 2: //Key
                    Destroy(obj);
                    SpawnTile(obj, x, y, value - 1, forInit);
                    break;
                case 3: //Door
                    Destroy(obj);
                    SpawnTile(obj, x, y, value - 1, forInit);
                    break;
                case 4: //Wall
                    Destroy(obj);
                    SpawnTile(obj, x, y, value - 1, forInit);
                    break;
                case 5: //Ground
                    Destroy(obj);
                    SpawnTile(obj, x, y, value - 1, forInit);
                    break;
            }
        }

        private void ResetToDefault()
        {
            PlayerNum = 0;
            KeyNum = 0;
            DoorNum = 0;
            WallNum = 0;
            GroundNum = 0;

            CountY = 0;
            CountX = 0;
        }

        private void AdjustObservation(GameObject obj)
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

        private void ShowLevel()
        {
            level = new GameObject[GridX, GridY];
            Storedlevel = new GameObject();
            Storedlevel.name = "Generated level";
            
            for (int x = 0; x < GridX; x++)
            {
                for (int y = 0; y < GridY; y++)
                {
                    level[x, y] = Grid[x, y];
                    level[x, y].gameObject.transform.SetParent(Storedlevel.transform);
                }
            }

            xDirection = xDirection + 8f;
            Storedlevel.transform.position = new Vector3(xDirection, 8, 0);
        }
    }
}
