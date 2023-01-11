using UnityEngine;
using UnityEngine.AI;

internal class Flee : IState
{
    private readonly Farmer farmer;
    private AStar pathfind;
    private GridA grid;
    private readonly GameObject enemy;

    private float xFlee;
    private float zFlee;
    private const float fleeSpeed = 2.5f;

    private Vector3 lastPos = Vector3.zero;
    public float timeStuck;

    public Flee(Farmer _farmer, AStar _pathfind, GridA _grid, GameObject _enemy)
    {
        farmer = _farmer;
        pathfind = _pathfind;
        enemy = _enemy;
        grid = _grid;
    }

    public void OnEnter()
    {
        farmer.currentStateText = "Flee";
        grid.path = null;
        farmer.DropAll(); // Drop all items
        timeStuck = 0f;
        farmer.target = null;

        // Calculate flee location based off which quadrant the enemy is in
        xFlee = -(enemy.transform.position.x);
        zFlee = -(enemy.transform.position.z);
        if (20f > xFlee && xFlee > 0f)
        {
            xFlee += 30f;
        }
        if (0f > xFlee && xFlee > -20f)
        {
            xFlee -= 30f;
        }
        if (20f > zFlee && zFlee > 0f)
        {
            zFlee += 30f;
        }
        if (0f > zFlee && zFlee > -20f)
        {
            zFlee -= 30f;
        }
        //Debug.Log("flee location:" + xFlee + ", 1, " + zFlee);

        // Set flee location and flee speed
        Vector3 fleeLocation = new Vector3(xFlee, 1f, zFlee);
        pathfind.FindPath(farmer.transform.position, fleeLocation);
        farmer.speed += fleeSpeed;

    }

    public void Tick()
    {
        // Checking if the farmer is still moving (not stuck)
        if (Vector3.Distance(farmer.transform.position, lastPos) == 0f)
        {
            timeStuck += Time.deltaTime;
        }
        lastPos = farmer.transform.position;
    }

    public void OnExit()
    {
        farmer.prevStateText = "Flee";
        farmer.speed -= fleeSpeed;
        grid.path = null;
    }
}