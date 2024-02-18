using UnityEngine;

public class Boat : AStarEntity
{
    Rigidbody2D rb2d;
    private void Start() => rb2d = GetComponent<Rigidbody2D>();

    protected override void FindNewTarget() 
    {
        transform.up = rb2d.velocity;

        if (path.Count > 0 && Vector3.Distance(path[0], transform.position) < 1f)
            path.RemoveAt(0);
        if(path.Count == 0)
        {
            Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Millisecond);
            path = AStarGrid.Instance.FindPath(transform.position, BoatManager.Instance.points[Random.Range(0, BoatManager.Instance.points.Length)].position);
        }
    }
}