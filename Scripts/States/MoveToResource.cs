using UnityEngine;
using UnityEngine.AI;

internal class MoveToResource : IState
{
    private readonly Farmer farmer;
    private AStar pathfind;
    private GridA grid;

    public MoveToResource(Farmer _farmer, AStar _pathfind, GridA _grid)
    {
        farmer = _farmer;
        pathfind = _pathfind;
        grid = _grid;
    }
    
    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        farmer.currentStateText = "Move To Resource";
        pathfind.FindPath(farmer.transform.position, farmer.target.transform.position);
    }

    public void OnExit()
    {
        farmer.prevStateText = "Move To Resource";
        grid.path = null;
    }
}