using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrashGenerator : Singleton<TrashGenerator>
{
    [SerializeField] private float CellSize = 1f;
    [SerializeField] private float MapSize = 10f;
    [SerializeField] private TrashSO[] TrashList; ///note: should be from largest to smallest
    [SerializeField] private GameObject Prefab;

    private void Start() => ResetTrash();

    public void GenerateTrash()
    {
        UnityEngine.Random.InitState(DateTime.Now.Second + DateTime.Now.Millisecond);
        int cellCount = (int)(MapSize / CellSize);
        List<Tuple<Vector2, TrashSO>> Trash = new List<Tuple<Vector2, TrashSO>>();
        List<Cell> Cells = new List<Cell>();
        for(int x = 0; x < cellCount; x++)
            for (int y = 0; y < cellCount; y++)
                Cells.Add(new Cell(x, y, CellSize));

        foreach (TrashSO trash in TrashList)
            for(int i = 0; i < trash.TrashAmt && Cells.Where(c => !c.closed).Count() != 0; i++)
            {
                Cell cell = Cells.Where(c => !c.closed).ToArray()[UnityEngine.Random.Range(0, Cells.Where(c => !c.closed).Count())];
                Trash.Add(new Tuple<Vector2, TrashSO>(cell.PlaceInCell(), trash));
                Cells.Where(c => Mathf.Abs(c.position.x - cell.position.x) <= trash.TrashGridSize && Mathf.Abs(c.position.y - cell.position.y) <= trash.TrashGridSize).ToList().ForEach(c => c.AddOverlap(Trash[Trash.Count - 1]));
            }

        foreach(Tuple<Vector2, TrashSO> trash in Trash)
        {
            GameObject obj = Instantiate(Prefab, transform, false);
            obj.transform.position = trash.Item1;
            obj.transform.position += transform.position;
            obj.GetComponent<TrashHolder>().Trash = trash.Item2;
            obj.GetComponent<SpriteRenderer>().sprite = trash.Item2.Sprite;
            obj.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f));
            obj.AddComponent<PolygonCollider2D>();
            if (trash.Item2.Underwater)
                obj.layer = LayerMask.NameToLayer("IgnoreCollision");
        }
    }

    private class Cell
    {
        public Vector2Int position;
        private Vector2 localPosition;

        public bool closed { get; private set; } = false;
        private List<Tuple<Vector2, TrashSO>> PotentialOverlap = new List<Tuple<Vector2, TrashSO>>();
        private bool tl = false;
        private bool bl = false;
        private bool tr = false;
        private bool br = false;
        private float cellSize;

        public void AddOverlap(Tuple<Vector2, TrashSO> trash)
        {
            if (closed)
                return;
            bl = bl || Vector2.Distance(localPosition                           , trash.Item1) <= trash.Item2.TrashGridSize;
            tl = tl || Vector2.Distance(localPosition + cellSize * Vector2.up   , trash.Item1) <= trash.Item2.TrashGridSize;
            br = br || Vector2.Distance(localPosition + cellSize * Vector2.right, trash.Item1) <= trash.Item2.TrashGridSize;
            tr = tr || Vector2.Distance(localPosition + cellSize * Vector2.one  , trash.Item1) <= trash.Item2.TrashGridSize;
            if (tl && bl && tr && br)
                Close();
        }

        private void Close()
        {
            PotentialOverlap = null;
            closed = true;
        }

        public Vector2 PlaceInCell()
        {
            for(int i = 0; i < 100; i++)
            {
                Vector2 pos = localPosition + new Vector2(UnityEngine.Random.Range(0f, cellSize), UnityEngine.Random.Range(0f, cellSize));
                bool overlap = false;
                foreach(Tuple<Vector2, TrashSO> trash in PotentialOverlap)
                    if(Vector2.Distance(pos, trash.Item1) <= trash.Item2.TrashGridSize)
                    {
                        overlap = true;
                        break;
                    }
                if (!overlap)
                {
                    Close();
                    return pos;
                }
            }
            Close();
            return localPosition + Vector2.one / 2; ///return center of cell if failled 100 times
        }

        public Cell(int x, int y, float cSize) => localPosition = (Vector2)(position = new Vector2Int(x, y)) * (cellSize = cSize);
    }

    public void ResetTrash()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        GenerateTrash();
        AStarGrid.Instance.RecheckCollisionAndPaths();
    }

    public bool CheckTrashGone() => transform.childCount == 0;
}
