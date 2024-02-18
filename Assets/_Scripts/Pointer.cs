using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(Vector3.forward * Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg);
    }
}