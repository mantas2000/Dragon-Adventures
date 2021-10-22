using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private float jumpForce = 800f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;

    private const float GroundedRadius = .2f;
    private Vector2 _refVelocity = Vector2.zero;
    private Rigidbody2D _body;
    private SpriteRenderer _renderer;
    private bool _grounded;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        _grounded = false;
        
        // Check if player is grounded
        var colliders = Physics2D.OverlapCircleAll(groundCheck.position, GroundedRadius, whatIsGround);
        
        foreach (var t in colliders)
        {
            if (t.gameObject != gameObject)
            {
                _grounded = true;
            }
        }
        
        // Teleport player if it enters out of bounds
        Teleport();
    }
    
    public void Move(float move, bool jump, bool crouch)
    {
        // Horizontal movement
        if (!crouch)
        {
            // Increase movement speed while mid air
            if (!_grounded)
            {
                move *= 2;
            }
            
            var targetVelocity = new Vector2(move * 10f, _body.velocity.y);
            _body.velocity = Vector2.SmoothDamp(_body.velocity, targetVelocity, ref _refVelocity, movementSmoothing);
            
            // Flip player
            Flip(move);
        }

        // Jumping logic
        if (_grounded && jump)
        {
            // Add a vertical force to the player
            _grounded = false;
            _body.AddForce(new Vector2(0f, jumpForce));
        }

        // Crouching logic
        switch (_grounded)
        {
            case true when crouch:
                // Do nothing for now
                break;
            
            case false when crouch:
                _body.AddForce(new Vector2(0f, -jumpForce));
                break;
        }
        
        // Let player jump over bridges
        Physics2D.IgnoreLayerCollision(6, 8, _body.velocity.y > 0.0f);  
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
    
    // Teleport player back when it goes out of bounds
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
