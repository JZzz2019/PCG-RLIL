using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPath
{
    static void isPath(GameObject[,] grid, int gridX, int gridY)
    {
        //Define of visited array to keep track of visited locations
        bool[,] visited = new bool[gridX, gridY];

        bool pathExist = false;

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                //check if it is a player and check if it hasnt been visited
                if (grid[x,y].GetComponent<Tile>().IsPlayer == true && !visited[x, y])
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
    public static bool isSafe(int x, int y, GameObject[,] grid)
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

    public static bool isPath(GameObject[,] grid, int x, int y, bool[,] visited)
    {
        //Checking for boundries
        //Checking for walls
        //Checking if it hasnt been visited
        if (isSafe(x, y, grid) && grid[x, y].GetComponent<Tile>().Traversable != false && !visited[x, y])
        {
            //Make the tile become visited
            visited[x, y] = true;

            if (grid[x, y].GetComponent<Tile>().IsTarget == true)
            {
                return true;
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
}
