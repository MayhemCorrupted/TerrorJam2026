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
        float moveInput = moveAction.FindAction("Move").ReadValue<float>();
        rb.linearVelocityX = moveInput * moveSpeed;
    }
}
