using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : Singleton<CharacterController>
{
    private TidyOcean controls;
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    private void Awake()
    {
        controls = new TidyOcean();
        controls.Player.Enable();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 v = controls.Player.Movement.ReadValue<Vector2>();
        rb.AddRelativeForce(new Vector2(0, v.y * moveSpeed * Time.deltaTime * (v.y < 0 ? .25f : 1)) / (rb.velocity.magnitude == 0 ? 1 : rb.velocity.magnitude));
        rb.MoveRotation(rb.rotation - v.x * turnSpeed * Time.deltaTime);
        //Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //rb.AddForceAtPosition(Vector2.down * moveSpeed / 5, collision.transform.position);
        if(collision.gameObject.layer == LayerMask.NameToLayer("Damage"))
            TrashCollection.Instance.RemoveRandomTrash();
    }
}
