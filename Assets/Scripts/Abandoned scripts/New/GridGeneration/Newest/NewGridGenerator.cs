using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGridGenerator : MonoBehaviour
{
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

    [Header("Stored objects in list")]
    public List<GameObject> StoredObj = new List<GameObject>();
    public GameObject[,] Grid;

    void Awake()
    {
        //Grid = new int[GridX, GridY];
        Grid = new GameObject[GridX, GridY];
        GenerateGrid();
    }

    void Start()
    {
        if (NarrowMethod == true)
        {
            StartCoroutine(NarrowSelection());
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
                SelectSpawn(Grid[x,y], x, y, i);
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

    }

    IEnumerator NarrowSelection()
    {
        if (parent.childCount >= (GridX * GridY))
        {
            foreach (GameObject o in StoredObj)
            {
                if (o.name == "Key") { KeyNum += 1; }
                if (o.name == "Player") { PlayerNum += 1; }
                if (o.name == "Wall") { WallNum += 1; }
                if (o.name == "Door") { DoorNum += 1; }
                if (o.name == "Ground") { GroundNum += 1; }
            }

            foreach (GameObject o in StoredObj)
            {
                yield return new WaitForSeconds(0.5f);
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
        }
    }

    void SelectTile(Vector3 pos) //Visual selection on current tile
    {
        if (Frame.activeSelf != true)
        {
            Frame.SetActive(true);
        }
        Frame.transform.position = pos;
    }


}
