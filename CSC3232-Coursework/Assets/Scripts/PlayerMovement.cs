using UnityEngine;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float fallForce = 100f;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    private Vector2 _refVelocity = Vector2.zero;
    private Rigidbody2D _body;
    private SpriteRenderer _renderer;
    private Animator _animator;
    private bool _grounded;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Restart level
        if (Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void FixedUpdate()
    {
        // Get user input
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        
        // Flip player
        Flip(horizontalInput);

        // Horizontal movement
        var targetVelocity = new Vector2(horizontalInput * speed, _body.velocity.y);
        _body.velocity = Vector2.SmoothDamp(_body.velocity, targetVelocity, ref _refVelocity, movementSmoothing);

        // Vertical movement
        if (verticalInput > 0 && _grounded) Jump();
        
        else if (verticalInput < 0 && !_grounded)
        {
            _body.velocity = new Vector2(_body.velocity.x, verticalInput * fallForce);
        }
        
        // Teleport player
        Teleport();
        
        // Let player jump over bridges
        Physics2D.IgnoreLayerCollision(6, 8, _body.velocity.y > 0.0f);  

        // Set Animator parameters
        //_animator.SetBool("Walk", horizontalInput != 0);
        //_animator.SetBool("Grounded", _grounded);
    }

    // Jumping animation
    private void Jump()
    {
        _grounded = false;
        _body.velocity = new Vector2(_body.velocity.x, jumpForce);
        //_animator.SetTrigger("Jump");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) _grounded = true;
    }

    // Flip player to match movement direction
    private void Flip(float movementDirection)
    {
        if (movementDirection > 0)
        {
            _renderer.flipX = false;
        }
        else if (movementDirection < 0)
        {
            _renderer.flipX = true;
        }
    }

    private void Teleport()
    {
        if (transform.position.x > 16.6f)
        {
            transform.position = new Vector3(-16.6f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -16.6f)
        {
            transform.position = new Vector3(16.6f, transform.position.y, transform.position.z);
        }
        else if (transform.position.y < -4f)
        {
            transform.position = new Vector3(transform.position.x, 8.65f, transform.position.z);
        }
    }
}