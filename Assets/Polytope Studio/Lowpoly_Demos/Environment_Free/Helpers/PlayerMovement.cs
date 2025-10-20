using UnityEngine;
using UnityEngine.InputSystem;

//INUTILE

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector2 moveInput;
    private bool jumpPressed;

    private Vector3 velocity;
    private bool isGrounded;
    public Animator animator;
    // Appelé par le PlayerInput
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            jumpPressed = true;
    }

    private int comboStep = 0;
    private float lastAttackTime = 0f;
    public float comboResetTime = 1f; // temps max entre deux attaques pour garder le combo
    public void OnAttack()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
        }

        comboStep++;
        lastAttackTime = Time.time;
        Debug.Log("Combot Step : " + comboStep);

        if (comboStep == 1)
        {
            animator.SetTrigger("Attack");
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("Attack2");
        }
        else if (comboStep == 3)
        {
            animator.SetTrigger("Attack3");
            comboStep = 0;
        }

    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);

        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
