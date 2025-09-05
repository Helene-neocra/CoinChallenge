using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float deathYThreshold = -5f; // Seuil Y en dessous duquel le joueur meurt

    private Vector2 moveInput;
    private Rigidbody rb;
    private PlayerMove controls;
    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isJumpMiddle = false;
    private bool isFalling = false;
    private Animator animator;
    private FloorGenerator floorGenerator; // Référence au FloorGenerator
    public float groundedCast = 0.1f; // Distance du raycast pour détecter le sol
    private bool timerStarted = false; // Retirer static pour permettre réinitialisation
    private bool isDead = false; // Empêcher multiple déclenchements de mort
    SimpleAudioManager audioManager;
    void Awake()
    {
        controls = new PlayerMove();
        
        // Trouver le FloorGenerator dans la scène
        floorGenerator = FindObjectOfType<FloorGenerator>();
        if (floorGenerator != null)
        {
            floorGenerator.OnPlatformGenerated += PositionPlayerOnPlatform;
        }
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
        audioManager = FindObjectOfType<SimpleAudioManager>();

    }
    
    void PositionPlayerOnPlatform(Vector3 platformPosition)
    {
        // Positionner le joueur au centre de la plateforme avec un offset en Y
        Vector3 playerPosition = new Vector3(platformPosition.x, platformPosition.y + 1f, platformPosition.z);
        transform.position = playerPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Vérification de chute mortelle
        if (!isDead && transform.position.y < deathYThreshold)
        {
            isDead = true;
            TriggerDeath();
            return; // Sortir de Update pour éviter autres traitements
        }

        // Détection de la marche via l'input
        bool isWalking = moveInput.sqrMagnitude > 0.01f && isGrounded;

        // Démarrer le timer dès que le joueur commence à bouger
        if (!timerStarted && moveInput.sqrMagnitude > 0.01f)
        {
            timerStarted = true;
            Timer timer = FindObjectOfType<Timer>();
            if (timer != null)
            {
                timer.StartTimer();
            }
        }
        
        // Gestion des états de saut
        isJumping = !isGrounded && rb.velocity.y > 0.1f;
        isJumpMiddle = !isGrounded && rb.velocity.y <= 0.1f;
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

        bool hasHit = Physics.Raycast(transform.position, Vector3.down, out var hit, groundedCast);
        if (hasHit)
        {
            // Vérifier si c'est bien un objet au sol
            if (hit.collider.gameObject.CompareTag("Floor"))
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }
        Debug.DrawRay(transform.position, Vector3.down * groundedCast, hasHit ? Color.green : Color.red, 15f);
        
    }

    void FixedUpdate()
    {
        // Déplacement relatif à l'orientation du personnage
        Vector3 direction = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
        if (direction.sqrMagnitude > 0.01f)
        {
            rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
            // Ralentit la rotation du personnage (valeur plus faible)
            float rotationSpeed = 0.02f; // Valeur plus faible pour une rotation plus douce
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            audioManager.PlayJumpSound();
        }
    }
    
    // Méthode pour forcer l'arrêt du joueur à la fin du jeu
    public void ForceStop()
    {
        // Arrête tous les inputs
        moveInput = Vector2.zero;
        
        // Arrête le mouvement du Rigidbody
        if (rb != null)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0); // Garde seulement la gravité
        }
        
        // Force l'animation idle
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isJumpMiddle", false);
            animator.SetBool("isFalling", false);
        }
    }

    void OnDestroy()
    {
        // Se désabonner de l'événement pour éviter les erreurs
        if (floorGenerator != null)
        {
            floorGenerator.OnPlatformGenerated -= PositionPlayerOnPlatform;
        }
    }

    void TriggerDeath()
    {
        Debug.Log("Le joueur est mort par chute !");
        
        // Déclencher le game over via le GameManager
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.TriggerGameOver();
        }
        else
        {
            Debug.LogError("GameManager not found!");
        }
    }
}
