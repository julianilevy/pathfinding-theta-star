using UnityEngine;

public class Node
{
    public Node parent;
    public Vector3 worldPosition;
    public bool walkable;
	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;
    public int fCost
    {
        get { return gCost + hCost; }
    }

    public Node(Vector3 worldPosition, bool walkable, int gridX, int gridY)
    {
        this.worldPosition = worldPosition;
        this.walkable = walkable;
		this.gridX = gridX;
		this.gridY = gridY;
	}
}