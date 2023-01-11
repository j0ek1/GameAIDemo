using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] destinations;
    public Farmer farmer;
    public AStarE ePathfind;
    public GridB eGrid;
    private int index = 0;
    private float t = 0;
    private int pathIndex = 0;
    private float timeStuck = 0;
    private Vector3 lastPos = Vector3.zero;
    public LayerMask unwalkableMask;

    private void Start()
    {
        // Generate initial roam locations
        destinations[0].position = new Vector3(Random.Range(10f, 50f), 0f, Random.Range(10f, 50f));
        destinations[1].position = new Vector3(Random.Range(10f, 50f), 0f, Random.Range(-50f, -10f));
        destinations[2].position = new Vector3(Random.Range(-50f, -10f), 0f, Random.Range(-50f, -10f));
        destinations[3].position = new Vector3(Random.Range(-50f, -10f), 0f, Random.Range(10f, 50f));
        while (Physics.CheckSphere(destinations[0].position, .8f, unwalkableMask))
        {
            destinations[0].position = new Vector3(Random.Range(10f, 50f), 0f, Random.Range(10f, 50f));
            
        }
        while (Physics.CheckSphere(destinations[1].position, .8f, unwalkableMask))
        {
            destinations[1].position = new Vector3(Random.Range(10f, 50f), 0f, Random.Range(-50f, -10f));

        }
        while (Physics.CheckSphere(destinations[2].position, .8f, unwalkableMask))
        {
            destinations[2].position = new Vector3(Random.Range(-50f, -10f), 0f, Random.Range(-50f, -10f));

        }
        while (Physics.CheckSphere(destinations[3].position, .8f, unwalkableMask))
        {
            destinations[3].position = new Vector3(Random.Range(-50f, -10f), 0f, Random.Range(10f, 50f));

        }
        
        ePathfind.FindPath(transform.position, destinations[0].position);
    }

    private void Update()
    {
        if (!farmer.stopped)
        {
            t = Time.deltaTime * 6f;
            if (eGrid.path != null)
            {
                // If enemy is not at the current node
                if (eGrid.NodeFromWorldPoint(transform.position) != eGrid.path[pathIndex])
                {
                    // Move towards node
                    transform.position = Vector3.MoveTowards(transform.position, (eGrid.path[pathIndex].worldPos + Vector3.up), t);
                }
                else // If enemy is at current node, move to next node
                {
                    if (pathIndex < eGrid.path.Count - 1)
                    {
                        pathIndex++;
                    }
                    else // If we have reached the end node reset
                    {
                        pathIndex = 0;
                        eGrid.path = null;
                        var nextPos = NextPos();
                        ePathfind.FindPath(transform.position, nextPos);                        
                    }
                }
            }
            else
            {
                pathIndex = 0;
            }

            if (Vector3.Distance(transform.position, farmer.transform.position) < 15f) // If farmer is close, chase
            {
                pathIndex = 0;
                eGrid.path = null;
                ePathfind.FindPath(transform.position, farmer.transform.position);
            }
            else if (timeStuck > .5f)
            {
                timeStuck = 0;
                pathIndex = 0;
                eGrid.path = null;
                var nextPos = NextPos();
                ePathfind.FindPath(transform.position, nextPos);
            }

            // Checking if the enemy is still moving (not stuck)
            if (Vector3.Distance(transform.position, lastPos) == 0f)
            {
                timeStuck += Time.deltaTime;
            }
            lastPos = transform.position;
        }
    }

    public Vector3 NextPos() // Get next position
    {
        index++;
        if (index >= destinations.Length)
        {
            index = 0;

            // Refresh roam positions
            destinations[0].position = new Vector3(Random.Range(10f, 50f), 0f, Random.Range(10f, 50f));
            destinations[1].position = new Vector3(Random.Range(10f, 50f), 0f, Random.Range(-50f, -10f));
            destinations[2].position = new Vector3(Random.Range(-50f, -10f), 0f, Random.Range(-50f, -10f));
            destinations[3].position = new Vector3(Random.Range(-50f, -10f), 0f, Random.Range(10f, 50f));
            while (Physics.CheckSphere(destinations[0].position, .8f, unwalkableMask))
            {
                destinations[0].position = new Vector3(Random.Range(10f, 50f), 0f, Random.Range(10f, 50f));

            }
            while (Physics.CheckSphere(destinations[1].position, .8f, unwalkableMask))
            {
                destinations[1].position = new Vector3(Random.Range(10f, 50f), 0f, Random.Range(-50f, -10f));

            }
            while (Physics.CheckSphere(destinations[2].position, .8f, unwalkableMask))
            {
                destinations[2].position = new Vector3(Random.Range(-50f, -10f), 0f, Random.Range(-50f, -10f));

            }
            while (Physics.CheckSphere(destinations[3].position, .8f, unwalkableMask))
            {
                destinations[3].position = new Vector3(Random.Range(-50f, -10f), 0f, Random.Range(10f, 50f));

            }
        }
        return destinations[index].position;
    }
}