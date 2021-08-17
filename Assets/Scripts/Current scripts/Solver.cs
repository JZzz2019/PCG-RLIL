using UnityEngine;
using GeneratorClass;

public class Solver
{
    //For x and y arrays are for the original solver
    //private int[] x;
    //private int[] y;
    private bool pathExist;
    private bool[,] visited;

    private TileGenerationV2 TGV2;

    public Solver(TileGenerationV2 TGV2)
    {
        this.TGV2 = TGV2;
        visited = new bool[TGV2.GridX, TGV2.GridY];
    }

    //Use recursive approach to pathfind
    public void IsPath(GameObject[,] grid, int gridX, int gridY)
    {
        //Define of visited array to keep track of visited locations
        
        //Reset everything to false when it is used again
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                visited[x, y] = false;
            }
        }
        pathExist = false;

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                //check if it is a player and check if it hasnt been visited
                if (grid[x, y].GetComponent<Tile>().IsPlayer == true && !visited[x, y])
                {
                    if (isPath(grid, x, y, visited))
                    {
                        //if path exists
                        pathExist = true;
                        break;
                    }
                }
            }
        }
        if (pathExist)
        {
            Debug.Log("yes, there is path");
        }
        else
        {
            Debug.Log("no, there is no path");
        }
    }

    //Checking for boundries
    private bool CheckBoundaries(int x, int y, GameObject[,] grid)
    {
        if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Recursive approach - calling itself
    private bool isPath(GameObject[,] grid, int x, int y, bool[,] visited)
    {
        //Checking for boundaries
        //Checking for walls
        //Checking if it hasnt been visited
        if (CheckBoundaries(x, y, grid) && grid[x, y].GetComponent<Tile>().Traversable != false && !visited[x, y])
        {
            //Make the tile become visited
            visited[x, y] = true;

            //If a target has found, remove the target from templist
            //if templist is empty, return true
            if (grid[x, y].GetComponent<Tile>().IsTarget == true)
            {
                TGV2.TempList.Remove(grid[x, y]);
                if (TGV2.TempList.Count == 0)
                {
                    return true;
                }
            }

            //Traverse left 
            bool left = isPath(grid, x - 1, y, visited);

            if (left)
            {
                return true;
            }

            //Traverse right 
            bool right = isPath(grid, x + 1, y, visited);

            if (right)
            {
                return true;
            }

            //Traverse up
            bool up = isPath(grid, x, y + 1, visited);

            if (up)
            {
                return true;
            }

            //Traverse down
            bool down = isPath(grid, x, y - 1, visited);

            if (down)
            {
                return true;
            }
        }
        return false;
    }
    public void PerformSolver()
    {
        foreach (GameObject obj in TGV2.Grid)
        {
            switch (obj.name)
            {
                case "Player":
                    obj.GetComponent<Tile>().Traversable = true;
                    obj.GetComponent<Tile>().IsPlayer = true;
                    //GetLocation(obj);
                    break;
                case "Door":
                    obj.GetComponent<Tile>().Traversable = true;
                    obj.GetComponent<Tile>().IsTarget = true;
                    TGV2.TempList.Add(obj);
                    //GetLocation(obj);
                    break;
                case "Key":
                    obj.GetComponent<Tile>().Traversable = true;
                    obj.GetComponent<Tile>().IsTarget = true;
                    TGV2.TempList.Add(obj);
                    //GetLocation(obj);
                    break;
                case "Wall":
                    obj.GetComponent<Tile>().Traversable = false;
                    //TGV2.StoredObjects.Add(obj);
                    break;
                case "Ground":
                    obj.GetComponent<Tile>().Traversable = true;
                    //TGV2.StoredObjects.Add(obj);
                    break;
            }
        }
        //Debug.Log(TGV2.TempList + " Templist");
        //Debug.Log(TGV2.StoredObjects + " StoredObjects");

        //Define the size of x and y array by initialisng it with templist.count
        //Templist only stores important tiles:Player, Key and Door
        //x = new int[TGV2.TempList.Count];
        //y = new int[TGV2.TempList.Count];
        //int j = 0;

        //Foreach object in templist: Player,Key and Door
        //foreach (GameObject o in TGV2.TempList)
        //{
        //    if (j < TGV2.TempList.Count)
        //    {
        //        x[j] = o.GetComponent<Tile>().xPosition;
        //        y[j] = o.GetComponent<Tile>().yPosition;
        //        j++;
        //        //Store x and y positions value in two arrays at particular index for later usage to check 
        //        //if the current level is solvable 
        //    }

        //}

        //for (int index = 0; index < TGV2.TempList.Count; index++)
        //{
        //    bool IsWalkerable = false;
        //    int Direction = 1;
        //    int Counter = 0;

        //    //x and y are arrays that store the locations of the object in 2D array
        //    //Check the object at x[xPos] and y[yPos] and see if its y value in 2D array is higher than the second
        //    //object's y value; if yes, see if they are equal using while

        //    int option = 0;
        //    int ComparisonSize = TGV2.TempList.Count;

        //    while (option < TGV2.TempList.Count)
        //    {
        //        Direction = 1;
        //        Counter = 0;
        //        //if y higher than the second y value, move downwards in grid = negative direction (-Direction)
        //        if (y[index] > y[index + option])
        //        {
        //            while (y[index] - Counter != y[index + option])
        //            {
        //                IsWalkerable = CheckTraversability(index, -Direction, 0);
        //                Direction++;
        //                Counter++;
        //                if (IsWalkerable == false)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //if lower than the second y value, move upwards in grid = position direction (+Direction)
        //            while (y[index] + Counter != y[index + option])
        //            {
        //                IsWalkerable = CheckTraversability(index, Direction, 0);
        //                Direction++;
        //                Counter++;
        //                if (IsWalkerable == false)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //        Direction = 1;
        //        Counter = 0;
        //        if (x[index] > x[index + option])
        //        {
        //            //if higher than the second x value, move to left in grid = negative direction (-Direction)
        //            while (x[index] - Counter != x[index + option])
        //            {
        //                IsWalkerable = CheckTraversability(index, -Direction, 1);
        //                Direction++;
        //                Counter++;
        //                if (IsWalkerable == false)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //if lower than the second x value, move right in grid = position direction (+Direction)
        //            while (x[index] + Counter != x[index + option])
        //            {
        //                IsWalkerable = CheckTraversability(index, Direction, 1);
        //                Direction++;
        //                Counter++;
        //                if (IsWalkerable == false)
        //                {
        //                    break;
        //                }
        //            }
        //        }

        //        if (index + option >= TGV2.TempList.Count - 1)
        //        {
        //            option = (TGV2.TempList.Count - TGV2.TempList.Count) - index;
        //        }
        //        else
        //        {
        //            option++;
        //        }
        //        ComparisonSize--;
        //        //Break the while loop
        //        if (ComparisonSize <= 0)
        //        {
        //            break;
        //        }
        //    }
        //}
    }

    public bool IsSolvable()
    {
        if (pathExist == true)
        {
            return true;
        }
        else
        {
            return false;
        }
        //For the original solver

        //int ConfirmedObjects = 0;
        //foreach (GameObject obj in TGV2.TempList)
        //{
        //    if (obj.GetComponent<Tile>().Confirmed == true)
        //    {
        //        ConfirmedObjects++;
        //    }
        //}
        //if (ConfirmedObjects >= TGV2.TempList.Count) //The length of important tiles that must be there to determine a solvable level
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }
    //Check each important object to see if they can traverse to other important tiles
    //bool CheckTraversability(int i, int Dir, int caseNum)
    //{
    //    switch (caseNum)
    //    {
    //        case 0:
    //            //0 for y position
    //            //Check upwards or downwards tile on grid if it is traversable
    //            if (TGV2.Grid[x[i], y[i] + Dir].GetComponent<Tile>().Traversable == true)
    //            {
    //                //Check if it has been visited, if true, 
    //                if (TGV2.Grid[x[i], y[i] + Dir].GetComponent<Tile>().Visited == true)
    //                {
    //                    //Confirm this particular important tile for finding visited tiles which means it can be accessed by 
    //                    //other important tile, making it a solvable level
    //                    TGV2.Grid[x[i], y[i]].GetComponent<Tile>().Confirmed = true;
    //                }
    //                else
    //                {
    //                    //Make the tile.visited becomes true
    //                    TGV2.Grid[x[i], y[i] + Dir].GetComponent<Tile>().Visited = true;
    //                    TGV2.Grid[x[i], y[i] + Dir].GetComponent<Tile>().VisualTrail();
    //                }
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        case 1:
    //            //1 for x position
    //            //Check left or right tile on grid is traversable
    //            if (TGV2.Grid[x[i] + Dir, y[i]].GetComponent<Tile>().Traversable == true)
    //            {
    //                if (TGV2.Grid[x[i] + Dir, y[i]].GetComponent<Tile>().Visited == true)
    //                {
    //                    //Confirm this particular important tile for finding visited tiles which means it can be accessed by 
    //                    //other important tile
    //                    TGV2.Grid[x[i], y[i]].GetComponent<Tile>().Confirmed = true;
    //                }
    //                else
    //                {
    //                    //Make the tile.visited becomes true if hasnt been visited
    //                    TGV2.Grid[x[i] + Dir, y[i]].GetComponent<Tile>().Visited = true;
    //                    TGV2.Grid[x[i] + Dir, y[i]].GetComponent<Tile>().VisualTrail();
    //                }
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //    }
    //    return false;
    //}

    //void GetLocation(GameObject obj)
    //{
    //    for (int x = 0; x < TGV2.GridX; x++)
    //    {
    //        for (int y = 0; y < TGV2.GridY; y++)
    //        {
    //            //Get location of the object in the array by verifying their names and boolean to
    //            //see if it has been looped over already
    //            if (TGV2.Grid[x, y].name == obj.name && obj.GetComponent<Tile>().Looped == false)
    //            {
    //                obj = TGV2.Grid[x, y];
    //                obj.GetComponent<Tile>().xPosition = x;
    //                obj.GetComponent<Tile>().yPosition = y;
    //                obj.GetComponent<Tile>().Looped = true;
    //                //Store in the list
    //                TGV2.StoredObjects.Add(obj);
    //                //3 tiles, player, Key and Door should be in here
    //                TGV2.TempList.Add(obj);
    //            }
    //        }
    //    }
    //}

}
