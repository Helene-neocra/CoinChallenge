using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Vector2 moveInput;
    private Rigidbody rb;
    private PlayerMove controls;
    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isJumpMiddle = false;
    private bool isFalling = false;
    private Animator animator;

    void Awake()
    {
        controls = new PlayerMove();
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Jump.performed += OnJumpPerformed;
    }

    void OnDisable()
    {
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.Jump.performed -= OnJumpPerformed;
        controls.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        Jump();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Détection de la marche via l'input
        bool isWalking = moveInput.sqrMagnitude > 0.01f && isGrounded;

        // Gestion des états de saut
        isJumping = !isGrounded && rb.velocity.y > 0.1f;
        isJumpMiddle = !isGrounded && Mathf.Abs(rb.velocity.y) <= 0.1f;
        isFalling = !isGrounded && rb.velocity.y < -0.1f;

        // Reset explicite si le joueur est au sol et vitesse verticale très faible
        if (isGrounded && Mathf.Abs(rb.velocity.y) < 0.05f)
        {
            isJumping = false;
            isJumpMiddle = false;
            isFalling = false;
        }

        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
            animator.SetBool("isJumping", isJumping);
            animator.SetBool("isJumpMiddle", isJumpMiddle);
            animator.SetBool("isFalling", isFalling);
        }
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
        if (direction.sqrMagnitude > 0.01f)
        {
            rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 0.25f);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void Jump()
    {
        Debug.Log($"Jump input reçu | isGrounded = {isGrounded}");
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }
}
