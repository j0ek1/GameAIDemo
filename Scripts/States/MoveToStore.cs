using UnityEngine;
using UnityEngine.AI;

internal class MoveToStore : IState
{
    private readonly Farmer farmer;
    private AStar pathfind;
    private GridA grid;

    public MoveToStore(Farmer _farmer, AStar _pathfind, GridA _grid)
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
        farmer.currentStateText = "Move to store";

        // Find path to store
        farmer.store = Object.FindObjectOfType<Store>();
        pathfind.FindPath(farmer.transform.position, farmer.store.transform.position);
    }

    public void OnExit()
    {
        farmer.prevStateText = "Move To Store";
        grid.path = null;
    }
}