using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterControler : MonoBehaviour
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

    private void Update()
    {
        Vector2 v = controls.Player.Movement.ReadValue<Vector2>();
        rb.AddRelativeForce(new Vector2(0, v.y * moveSpeed * Time.deltaTime * (v.y < 0 ? .25f : 1)) / (rb.velocity.magnitude == 0 ? 1 : rb.velocity.magnitude));
        rb.MoveRotation(rb.rotation + v.x * turnSpeed * Time.deltaTime);
    }
}
