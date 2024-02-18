using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update() => transform.rotation = Quaternion.Euler(Vector3.back * Mathf.Atan2(target.position.x - transform.position.x, target.position.y - transform.position.y) * Mathf.Rad2Deg);
}