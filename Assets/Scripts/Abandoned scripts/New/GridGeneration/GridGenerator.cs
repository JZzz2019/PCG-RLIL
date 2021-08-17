using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//There are 3 types of tile selection method: Narrow, Turtle and Wide

/* Narrow:
 Agent selects tile systematically in a sequence from left to right, top to bottom
 */

/* Turtle:
 Agent selects tile in its current surrounding
 */

/* Wide:
 Agent selects tile freely in the grid
 */
public class GridGenerator : MonoBehaviour
{
    [Header("NumOfTile")]
    public int KeyNum;
    public int PlayerNum;
    public int WallNum;
    public int DoorNum;
    public int GroundNum;

    [Header("Items and Base")]
    public GameObject[] items;
    //public GameObject BaseTile;
    public Transform parent;
    public Transform parent2;
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
    public List<Vector3> AllPositions = new List<Vector3>();

    //public int[,] Grid;
    public GameObject[,] grid2;

    void Awake()
    {
        //Grid = new int[GridX, GridY];
        grid2 = new GameObject[GridX, GridY];
        GenerateGrid();
    }

    void Start()
    {
        //Debug.Log(Grid);
        if (NarrowMethod == true)
        {
            StartCoroutine(NarrowSelection());
            //StartCoroutine(newNarrowSelection());
        }

        if (TurtleMethod == true)
        {
            StartCoroutine(NarrowSelection());
        }

        if (WideMethod == true)
        {
            StartCoroutine(NarrowSelection());
        }
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
                CreateList(position);
                int i = Random.Range(1, 2);
                SelectAndSpawn(i, position, Quaternion.identity, parent);
                //Grid[x, y] = Random.Range(0, items.Length);
                //Grid[x, y] = (GameObject)Instantiate(items[i], new Vector3(x, y, 0), Quaternion.identity);
                grid2[x, y] = (GameObject)Instantiate(items[i], new Vector3(x, y, 0), Quaternion.identity);
                //spawnTile(x, y);
            }
        }
    }

    void spawnTile(int x, int y)
    {
        int j = Random.Range(0, items.Length);
        GameObject g = Instantiate(items[j], new Vector3(x, y, 0), Quaternion.identity);
        g.transform.SetParent(parent2);
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
            //case 2:
            //    int x = Random.Range(0, items.Length);
            //    GameObject _obj = Instantiate(_object.GetObject(_object.objectType), position, rotation);
            //    _obj.transform.localScale = Vector3.one * (1 - outline);
            //    _obj.transform.SetParent(parent);
            //    StoredObjects.Add(_obj);
            //    break;
        }
    }

    IEnumerator NarrowSelection()
    {
        if (parent.childCount >= (GridX * GridY))
        {
            foreach (GameObject o in StoredObjects)
            {
                if (o.name == "Key") { KeyNum += 1; }
                if (o.name == "Player") { PlayerNum += 1; }
                if (o.name == "Wall") { WallNum += 1; }
                if (o.name == "Door") { DoorNum += 1; }
                if (o.name == "Ground") { GroundNum += 1; }
            }

            foreach (GameObject o in StoredObjects)
            {
                yield return new WaitForSeconds(1f);
                Vector3 pos = o.transform.position;
                pos.z = -0.1f;
                SelectTile(pos);

                if (o.name == "Key" && KeyNum > 1)
                {
                    KeyNum -= 1;
                    o.SetActive(false);
                }
                if (o.name == "Player" && PlayerNum > 1)
                {
                    PlayerNum -= 1;
                    o.SetActive(false);
                }
                if (o.name == "Door" && DoorNum > 1)
                {
                    DoorNum -= 1;
                    o.SetActive(false);
                }
            }
            //for (int i = 0; i < parent.transform.childCount; i++)
            //{
            //    yield return new WaitForSeconds(1f);
            //    GameObject child = parent.transform.GetChild(i).gameObject;
            //    Vector3 pos = child.transform.position;
            //    pos.z = -0.1f;
            //    SelectTile(pos); //added child
                
            //}
        }

    }

    IEnumerator newNarrowSelection()
    {
        if (parent.childCount >= (GridX * GridY))
        {
            for (int i = 0; i < StoredObjects.Count; i++)
            {
                yield return new WaitForSeconds(1f);
                int index = StoredObjects.IndexOf(items[0]);
                GameObject child = parent.transform.GetChild(i).gameObject;
                Vector3 pos = child.transform.position;
                pos.z = -0.1f;
                SelectTile(pos);

                if (index > 1)
                {
                    StoredObjects[i].gameObject.SetActive(false);
                }

               
            }
        }
    }

    void SelectTile(Vector3 pos) //This can be used to alter tile
    {
        if (Frame.activeSelf != true)
        {
            Frame.SetActive(true);
        }

        Frame.transform.position = pos;
        //child.SetActive(false);
    }

    void CreateList(Vector3 eachPosition)
    {
        AllPositions.Add(eachPosition);

        //Debug.Log(AllPositions + " " + eachPosition);
    }

    void StoreGameObjects(GameObject obj)
    {
        StoredObjects.Add(obj);
    }

    
    List<GameObject> GetGameObjects()
    {
        foreach (GameObject obj in StoredObjects)
        {
            
        }
        return StoredObjects;
    }
    
}
