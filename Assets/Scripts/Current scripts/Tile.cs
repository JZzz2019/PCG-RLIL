using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region variables for original solver
    //For the agent to determine if it is traverable tile in grid
    //public bool Traversable;
    //Check if it has been visited 
    public bool Visited;
    //Confirm the Important for determining whether if it a solvable level
    public bool Confirmed;
    //Has looped?
    public bool Looped;
    //For the agent to have a condition to continously pathfind
    //Confirm if the generated level is solvable
    public bool ReachedDoor;
    public bool IsSolvable;

    //For the agent to store its current x and y position in 2d array so can be used to compare
    public int xPosition;
    public int yPosition;

    #endregion
    //for new solver
    public bool IsPlayer;
    public bool IsTarget;
    public bool Traversable;

    Color MouseOverColor = Color.blue;

    Color OriginalColor;
    new SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        OriginalColor = renderer.material.color;
    }

    private void OnMouseOver()
    {
        renderer.material.color = MouseOverColor;
    }

    private void OnMouseExit()
    {
        renderer.material.color = OriginalColor;
    }

    public void VisualTrail()
    {
        renderer.material.color = Color.green;
    }
}
