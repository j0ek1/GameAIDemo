using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
	GridA grid;

    void Awake()
    {
        grid = GetComponent<GridA>();
    }
	void Update()
	{

    }

	public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
		// Get positions as nodes
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>(); // Set of nodes to be evaluated
        HashSet<Node> closedSet = new HashSet<Node>(); // Set of nodes already evaluated
        openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			Node current = openSet[0];
			// Set current node to the node in the open set with the lowest f cost
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost < current.fCost || openSet[i].fCost == current.fCost)
				{
					if (openSet[i].hCost < current.hCost)
                    {
						current = openSet[i];
					}
				}
			}

			// Move current node from set
			openSet.Remove(current);
			closedSet.Add(current);

			// If current node equals target node, we have found path
			if (current == targetNode)
			{
				RetracePath(startNode, targetNode);
				return;
			}

			// Check all neighbours of current node
			foreach (Node neighbour in grid.GetNeighbours(current))
			{
				// If neighbour is not walkable or already evaluated, skip
				if (!neighbour.walkable || closedSet.Contains(neighbour))
				{
					continue;
				}

				// If the new path to neighbour is shorter or neighbour is not in open set
				int newCostToNeighbour = current.gCost + GetDistance(current, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					// Set new costs and parent
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = current;

					// Add neighbour to open set
					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node current = endNode;

		// Trace path backwards, from end to start through parents
		while (current != startNode)
		{
			path.Add(current);
			current = current.parent;
		}

		// Reverse order of elements to have path from start to end
		path.Reverse();

		// Assign path
		grid.path = path;
	}

	int GetDistance(Node nodeA, Node nodeB)
	{
		int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		// Calculate distance by using formula 14y - 10(x-y) OR 14x - 10(y-x) depending on x>y
		// Where 14 is diagonal distance and 10 is horizontal/vertical distance
		if (distX > distY)
        {
			return 14 * distY + 10 * (distX - distY);
		}		
		return 14 * distX + 10 * (distY - distX);
	}

	public float GetPathDistance(List<Node> path)
    {
        if (path.Count > 1 && path != null)
        {
            float totalDistance = 0f;
            Node prevNode = path[0];
            foreach (Node node in path)
            {
                totalDistance += GetDistance(prevNode, node);
            }

            return totalDistance;
        }
        return 0.1f;
    }
}
