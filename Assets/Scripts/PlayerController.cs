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
    private FloorGenerator floorGenerator; // Référence au FloorGenerator
    private static bool timerStarted = false; // Pour éviter les démarrages multiples

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
        
        // Ne plus appeler PositionPlayerOnGround() car on attend la plateforme
    }
    
    void PositionPlayerOnPlatform(Vector3 platformPosition)
    {
        // Positionner le joueur au centre de la plateforme avec un offset en Y
        Vector3 playerPosition = new Vector3(platformPosition.x, platformPosition.y + 1f, platformPosition.z);
        transform.position = playerPosition;
        
        Debug.Log($"Player positioned on platform at: {playerPosition}");
    }

    void PositionPlayerOnGround()
    {
        // Cette méthode n'est plus utilisée mais gardée au cas où
        // Position par défaut plus sûre au centre du monde
        Vector3 defaultPosition = new Vector3(30f, 2f, 30f); // Centre du monde 30x30
        
        // Raycast depuis au-dessus pour trouver la surface du sol
        Vector3 rayStart = defaultPosition + Vector3.up * 10f;
        RaycastHit hit;
        
        // Raycast pour détecter tous les colliders
        if (Physics.Raycast(rayStart, Vector3.down, out hit, 20f))
        {
            // Vérifier si c'est bien un objet au sol
            if (hit.collider.gameObject.name.Contains("Ground") ||
                hit.collider.gameObject.name.Contains("Floor"))
            {
                // Place le joueur sur la surface du sol détectée avec un offset adapté au nouveau système
                Vector3 newPos = hit.point + Vector3.up * 0.0f; // Pas d'offset, directement sur le NavMesh
                transform.position = newPos;
                Debug.Log($"Player positioned on ground surface at: {newPos}");
            }
            else
            {
                // Position par défaut
                transform.position = defaultPosition;
                Debug.Log("Player positioned at default location");
            }
        }
        else
        {
            // Position de secours
            transform.position = defaultPosition;
            Debug.Log("Could not find ground surface, using default position");
        }
    }

    // Update is called once per frame
    void Update()
    {
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
                Debug.Log("Premier mouvement détecté - Timer démarré !");
            }
        }

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
        
        Debug.Log("Player movement stopped - Game Over");
    }

    void OnDestroy()
    {
        // Se désabonner de l'événement pour éviter les erreurs
        if (floorGenerator != null)
        {
            floorGenerator.OnPlatformGenerated -= PositionPlayerOnPlatform;
        }
    }
}
