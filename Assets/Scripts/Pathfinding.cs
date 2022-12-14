using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public Boss boss;
    public Player player;

    private Grid _grid;
    private GameObject _target;

	void Awake()
    {
		_grid = GetComponent<Grid>();
    }

	void Update()
    {
        _target = player.currentTarget;

        if (_target != null)
            FindPath(boss.transform.position, _target.transform.position);
    }

	void FindPath(Vector3 startPos, Vector3 targetPos)
    {
		Node startNode = _grid.GetNodeFromWorldPoint(startPos);
		Node targetNode = _grid.GetNodeFromWorldPoint(targetPos);

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0)
        {
			Node node = openSet[0];

			for (int i = 1; i < openSet.Count; i ++)
            {
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode)
            {
				RetracePath(startNode,targetNode);
				return;
			}

			foreach (Node neighbour in _grid.GetNeighbours(node))
            {
				if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);

				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode)
    {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
        {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

        path.Reverse();
		_grid.path = path;
        boss.Move(_grid);
	}

	int GetDistance(Node nodeA, Node nodeB)
    {
		int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (distanceX > distanceY)
			return 14 * distanceY + 10 * (distanceX - distanceY);

		return 14 * distanceX + 10 * (distanceY - distanceX);
	}
}