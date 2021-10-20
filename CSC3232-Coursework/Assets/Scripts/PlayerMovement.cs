using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 600f;
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
    
    private void FixedUpdate()
    {
        // Get user input
        var horizontalInput = Input.GetAxis("Horizontal");
        
        // Flip player
        Flip(horizontalInput);

        // Horizontal movement
        var targetVelocity = new Vector2(horizontalInput * speed, _body.velocity.y);
        _body.velocity = Vector2.SmoothDamp(_body.velocity, targetVelocity, ref _refVelocity, movementSmoothing);

        // Vertical movement
        if (Input.GetKey(KeyCode.Space) && _grounded) Jump();

        // Set Animator parameters
        //_animator.SetBool("Walk", horizontalInput != 0);
        //_animator.SetBool("Grounded", _grounded);
    }

    // Jumping animation
    private void Jump()
    {
        _grounded = false;
        _body.AddForce(new Vector2(0f, jumpForce));
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
}