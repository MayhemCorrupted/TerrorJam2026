using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] InputActionAsset moveAction;
    [SerializeField] float moveSpeed = 5;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Movement();
    }
    void Movement()
    {
        Vector2 moveInput = moveAction.FindAction("Move").ReadValue<Vector2>();
        rb.linearVelocityX = moveInput.x * moveSpeed;
    }
}
