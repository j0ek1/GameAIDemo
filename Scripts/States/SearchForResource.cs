using System.Linq;
using UnityEngine;

internal class SearchForResource : IState
{
    private readonly Farmer farmer;
    private AStar pathfind;
    private GridA grid;
    private Generator bestGen = null;
    public int bgIndex = 0;
    public float[] genDesire = { 0f, 0f, 0f, 0f };


    public SearchForResource(Farmer _farmer, AStar _pathfind, GridA _grid)
    {
        farmer = _farmer;
        pathfind = _pathfind;
        grid = _grid;
    }
    public void Tick()
    {
        if (farmer.target == null)
        {
            farmer.target = FindBestGen();
        }
    }

    private Generator FindBestGen()
    {        
        int i = 0;
        var generators = GameObject.FindGameObjectsWithTag("Generator");
        foreach (var generator in generators) // Check all generators present in scene
        {
            // Find the path and path distance
            pathfind.FindPath(farmer.transform.position, generator.transform.position);
            float distance = pathfind.GetPathDistance(grid.path);

            if (distance > 100f) // If we just collected from this gen skip
            {
                genDesire[i] = generator.GetComponent<Generator>().howFull * (1f - (distance / 30000f)); // Desirability calculated
                if (i == 0)
                {
                    bestGen = generator.GetComponent<Generator>();
                    bgIndex = i;
                }
                if (i != 0)
                {
                    if (genDesire[i] > genDesire[i - 1]) // Find highest desirability
                    {
                        bestGen = generator.GetComponent<Generator>();
                        bgIndex = i;
                    }
                }
                grid.path = null;
                i++;
            } 
        }
        return bestGen;
    }

    public void OnEnter()
    {
        farmer.currentStateText = "Search";
        bgIndex = 0;
        grid.path = null;   
    }

    public void OnExit()
    {
        farmer.prevStateText = "Search";
        bgIndex = 0;
        grid.path = null;
    }
}