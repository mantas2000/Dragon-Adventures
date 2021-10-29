using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private float jumpForce = 600f;
    [SerializeField] private float wallJumpForce = 1900f;
    [SerializeField] private float wallSlideSpeed = 1.25f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform leftWallCheck;
    [SerializeField] private Transform rightWallCheck;
    [SerializeField] private ScoreSystem scoreSystem;

    private Rigidbody2D _body;
    private SpriteRenderer _renderer;
    private Animator _animator;
    private Vector2 _refVelocity = Vector2.zero;
    private const float GroundedRadius = .2f;
    private bool _grounded;
    private bool _onLeftWall;
    private bool _onRightWall;
    private bool _wallJump;
    private float _lastTimeOnWall;
    private string _currentState;
    private bool _isColliding;
    private int _gemsCollected;
    private int _totalGems;
    
    // Animation states
    private const string PLAYER_IDLE = "Player_Idle";
    private const string PLAYER_RUN = "Player_Run";
    private const string PLAYER_JUMP = "Player_Jump";
    private const string PLAYER_CROUCH = "Player_Crouch";
    private const string PLAYER_ROLL = "Player_Roll";
    private const string PLAYER_SLIDE = "Player_Slide";

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _totalGems = GameObject.FindGameObjectsWithTag("Gem").Length;
        scoreSystem.DrawGems(_gemsCollected, _totalGems);
    }

    private void FixedUpdate()
    {
        // Check collisions
        GroundCheck();
        WallCheck();

        // Teleport player if it enters out of bounds
        Teleport();
    }
    
    public void Move(float move, bool jump, bool crouch, bool hitMove, bool crouchMove)
    {
        // Horizontal movement
        if (!crouch)
        {
            switch (_grounded)
            {
                // Increase movement speed while mid air
                case false:
                    move *= 2;
                    break;
                
                // Check if hit move is available
                case true when hitMove:
                    move *= 60;
                    break;
                
                case true when Mathf.Abs(move) > 0.01f && Mathf.Abs(_body.velocity.y) < 0.01f:
                    ChangeAnimationState(PLAYER_RUN);
                    break;
                
                case true when Mathf.Abs(_body.velocity.x) < 0.01f && Mathf.Abs(_body.velocity.y) < 0.01f:
                    ChangeAnimationState(PLAYER_IDLE);
                    break;
            }
            
            // Move player
            var targetVelocity = new Vector2(move * 10f, _body.velocity.y);
            _body.velocity = Vector2.SmoothDamp(_body.velocity, targetVelocity, ref _refVelocity, movementSmoothing);
            
            // Flip player
            Flip(move);
        }
        
        // Disable horizontal movement while crouching
        else
        {
            _body.velocity = new Vector2(0f, _body.velocity.y);
            ChangeAnimationState(PLAYER_CROUCH);
        }
        
        // Check if player is wall jumping
        if (_wallJump && (_grounded || Time.time - _lastTimeOnWall > 1f))
        {
            _wallJump = false;
        }

        switch (_grounded)
        {
            // Add a vertical force to the player
            case true when jump && !_onLeftWall && !_onRightWall:
                _grounded = false;
                _body.AddForce(new Vector2(0f, jumpForce));
                ChangeAnimationState(PLAYER_JUMP);
                break;
            
            // Add a special vertical force to the player
            case true when crouchMove:
                _grounded = false;
                _body.AddForce(new Vector2(0f, (float) (jumpForce * 1.5)));
                ChangeAnimationState(PLAYER_ROLL);
                break;
            
            // Attack from air
            case false when crouch:
                _body.AddForce(new Vector2(0f, -jumpForce));
                break;
            
            // Slide from left wall
            case false when _onLeftWall && !jump:
                _renderer.flipX = false;
                _body.velocity = new Vector2(_body.velocity.x, -wallSlideSpeed);
                if (!_wallJump) ChangeAnimationState(move > 0.01f ? PLAYER_IDLE : PLAYER_SLIDE);
                break;
            
            // Slide from right wall
            case false when _onRightWall && !jump:
                _renderer.flipX = true;
                _body.velocity = new Vector2(_body.velocity.x, -wallSlideSpeed);
                if (!_wallJump) ChangeAnimationState(move < -0.01f ? PLAYER_IDLE : PLAYER_SLIDE);
                break;
            
            // Jump from left wall
            case false when _onLeftWall && jump:
                _body.AddForce(new Vector2(wallJumpForce, wallJumpForce * 3));
                ChangeAnimationState(PLAYER_ROLL);
                _wallJump = true;
                _lastTimeOnWall = Time.time;
                break;
            
            // Jump from right wall
            case false when _onRightWall && jump:
                _body.AddForce(new Vector2(-wallJumpForce, wallJumpForce * 3));
                ChangeAnimationState(PLAYER_ROLL);
                _wallJump = true;
                _lastTimeOnWall = Time.time;
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
    
    private void GroundCheck()
    {
        // Check if player is grounded
        var groundColliders = Physics2D.OverlapCircleAll(groundCheck.position, GroundedRadius, whatIsGround);
        
        _grounded = groundColliders.Any(t => t.gameObject != gameObject);
    }

    private void WallCheck()
    {
        // Check if player is touching walls
        var leftWallColliders = Physics2D.OverlapCircleAll(leftWallCheck.position, GroundedRadius, whatIsWall);
        var rightWallColliders = Physics2D.OverlapCircleAll(rightWallCheck.position, GroundedRadius, whatIsWall);
        
        _onLeftWall = leftWallColliders.Any(t => t.gameObject != gameObject);

        if (_onLeftWall) return;
        
        _onRightWall = rightWallColliders.Any(t => t.gameObject != gameObject);
    }

    private void ChangeAnimationState(string newState)
    {
        if (_currentState == newState) return;
        
        _animator.Play(newState);

        _currentState = newState;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isColliding) return;
        _isColliding = true;
        
        // Collision with gems
        if (other.gameObject.CompareTag("Gem"))
        {
            other.gameObject.SetActive(false);
            _gemsCollected += 1;
            
            // Update scoreboard
            scoreSystem.DrawGems(_gemsCollected, _totalGems);
        }
        
        // Reset collision trigger
        StartCoroutine(CollisionReset());
    }

    // Reset collision trigger
    private IEnumerator CollisionReset()
    {
        yield return new WaitForEndOfFrame();
        _isColliding = false;
    }
}
