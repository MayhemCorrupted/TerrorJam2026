using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] InputActionAsset moveAction;
    [SerializeField] float moveSpeed = 5;
    [SerializeField] Animator playerAnim;
    bool isRight = true;
    float axisX;
    readonly int MoveInput = Animator.StringToHash("MoveInput");
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Movement();
        Flip();
    }
    void Movement()
    {
        axisX = moveAction.FindAction("Move").ReadValue<float>();
        playerAnim.SetInteger(MoveInput, (int)axisX);
        rb.linearVelocityX = axisX * moveSpeed;
    }
    void Flip()
    {
        if (isRight && axisX < 0 || !isRight && axisX > 0)
        {
            isRight = !isRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }
}
