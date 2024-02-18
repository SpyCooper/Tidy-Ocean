using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AStarEntity : MonoBehaviour
{
    protected List<Vector3> path = new List<Vector3>();
    [SerializeField] private float speed = 1f;
    private Vector3 target;
    private Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Update()
    {
        FindNewTarget();
        if (path.Count > 0)
            rb.velocity = Vector3.Lerp(transform.position, path[0], 1f) - transform.position;            
    }

    protected virtual void FindNewTarget() { return; }
}
