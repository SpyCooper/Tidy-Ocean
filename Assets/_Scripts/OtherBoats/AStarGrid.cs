using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AStarGrid : Singleton<AStarGrid>
{
    [SerializeField] private float nodeDiameter = 1f;
    [SerializeField] private float width = 10f;
    [SerializeField] private float height = 10f;
    [SerializeField] private bool showDebugGizmos = false;
    [SerializeField] private LayerMask meshObsticles;

    private ConstNode[,] grid = new ConstNode[0,0];

    public Vector3 GetTopRight() => transform.position + new Vector3(width, height, 0);

    private new void Awake()
    {
        base.Awake();
        meshObsticles = LayerMask.GetMask("Terrain");
        GenerateMap();
    }

    private class ConstNode
    {
        public bool Walkable = true;
        public Vector2Int GridPosition;
            
        public ConstNode(int x, int y) => GridPosition = new Vector2Int(x, y);
    }

    [ContextMenu("Calculate Map")]
    private void GenerateMap()
    {
        grid = new ConstNode[Mathf.RoundToInt(width / nodeDiameter), Mathf.RoundToInt(height / nodeDiameter)];
        for (int x = 0; x < grid.GetLength(0); x++)
            for (int y = 0; y < grid.GetLength(1); y++)
                grid[x, y] = new ConstNode(x, y);
        UpdateCollision();
    }

    public void RecheckCollisionAndPaths()
    {
        UpdateCollision();
        RecreatePath.Invoke(); 
    }

    private void UpdateCollision()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
            for (int y = 0; y < grid.GetLength(1); y++)
                grid[x, y].Walkable = !Physics2D.BoxCast(Position(x, y), Vector2.one * nodeDiameter / 2, 0, Vector2.zero, 1f, meshObsticles);
    }

    private Vector3 Position(int x, int y) => transform.position + Vector3.one * nodeDiameter / 2 + nodeDiameter * new Vector3(x, y, 0);

    private void OnDrawGizmos()
    {
        if (showDebugGizmos)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    Gizmos.color = grid[x, y].Walkable ? Color.green : Color.red;
                    Gizmos.DrawCube(Position(x, y), Vector3.one * nodeDiameter * .9f);
                }
        }
    }

    private class PathNode : IHeapItem<PathNode>
    {
        public ConstNode ConstantNode;
        public PathNode(ConstNode n) => ConstantNode = n;
        //A* vars
        public int GCost;
        public int HCost;
        public int FCost { get => GCost + HCost; }
        private int heapIndex;
        public int HeapIndex { get => heapIndex; set => heapIndex = value; }
        //Backtrack vars
        public PathNode Parent;
        public int CompareTo(PathNode _other)
        {
            int compare = FCost.CompareTo(_other.FCost);
            if (compare == 0)
                compare = HCost.CompareTo(_other.HCost);
            return -compare;
        }
    }

    private Vector2Int IndexFromWorldPoint(Vector3 _position)
    {
        Vector2 p = _position - transform.position - (Vector3.one * nodeDiameter / 2);
        p.x *= grid.GetLength(0) / width;
        p.y *= grid.GetLength(1) / height;
        return Vector2Int.RoundToInt(p);
    }

    public List<Vector3> FindPath(Vector3 _start, Vector3 _end)
    {
        Vector2Int startVector = IndexFromWorldPoint(_start);
        Vector2Int endVector = IndexFromWorldPoint(_end);
        PathNode[,] aStarGrid = new PathNode[grid.GetLength(0), grid.GetLength(1)];
        for (int x = 0; x < grid.GetLength(0); x++)
            for (int y = 0; y < grid.GetLength(1); y++)
                aStarGrid[x, y] = new PathNode(grid[x, y]);
        PathNode start = aStarGrid[startVector.x, startVector.y];
        PathNode end = aStarGrid[endVector.x, endVector.y];
        Heap<PathNode> openSet = new Heap<PathNode>(grid.GetLength(0) * grid.GetLength(1));
        HashSet<PathNode> closedSet = new HashSet<PathNode>();
        openSet.Add(start);
        ///While path can be found
        while (openSet.Count > 0)
        {
            PathNode currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);
            ///At End Retrace Path
            if (currentNode == end)
            {
                List<Vector3> vectorPath = new List<Vector3>();
                for (PathNode current = end; current != start; current = current.Parent)
                    vectorPath.Add(Position(current.ConstantNode.GridPosition.x, current.ConstantNode.GridPosition.y));
                vectorPath.Reverse();
                return vectorPath;
            }
            ///Get neighbours
            List<PathNode> neighbours = new List<PathNode>();
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    int checkX = currentNode.ConstantNode.GridPosition.x + x;
                    int checkY = currentNode.ConstantNode.GridPosition.y + y;

                    if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
                        neighbours.Add(aStarGrid[checkX, checkY]);
                }
            ///Update neighbours
            foreach (PathNode neighbour in neighbours)
            {
                if (!neighbour.ConstantNode.Walkable || closedSet.Contains(neighbour))
                    continue;
                int newMoveCost = currentNode.GCost + GetDistance(currentNode, neighbour);
                if (newMoveCost < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMoveCost;
                    neighbour.HCost = GetDistance(neighbour, end);
                    neighbour.Parent = currentNode;
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    else openSet.UpdateItem(neighbour);
                }
            }
        }
        return null; ///Can't create path
    }

    private int GetDistance(PathNode nodeA, PathNode nodeB)
    {
        int distX = Mathf.Abs(nodeA.ConstantNode.GridPosition.x - nodeB.ConstantNode.GridPosition.x);
        int distY = Mathf.Abs(nodeA.ConstantNode.GridPosition.y - nodeB.ConstantNode.GridPosition.y);
        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

    public UnityEvent RecreatePath = new UnityEvent();
}