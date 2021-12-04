using System.Linq;
using Cinemachine;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private float jumpForce = 600f;
    [SerializeField] private float wallJumpForce = 1900f;
    [SerializeField] private float wallSlideSpeed = 1.25f;
    [SerializeField] private bool teleportFeature;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform leftWallCheck;
    [SerializeField] private Transform rightWallCheck;
    [SerializeField] private ParticleSystem jumpDust;
    [SerializeField] private ParticleSystem leftWallDust;
    [SerializeField] private ParticleSystem rightWallDust;
    [SerializeField] private ParticleSystem duckDust;
    [SerializeField] private CinemachineImpulseSource impulseSource;

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
    private bool _airAttack;

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
    }

    private void FixedUpdate()
    {
        // Check collisions
        GroundCheck();
        WallCheck();

        // Teleport player if it enters out of bounds
        if (teleportFeature) Teleport();
    }
    
    public void Move(float move, bool jump, bool crouch, bool dashMove, bool duckMove)
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
                
                // Check if dash move is available
                case true when dashMove:
                    FindObjectOfType<AudioManager>().Play("Attack");
                    jumpDust.Play();
                    move *= 60;
                    break;
                
                // Set player's running animations if it's moving
                case true when Mathf.Abs(move) > 0.01f && Mathf.Abs(_body.velocity.y) < 0.01f:
                    ChangeAnimationState(PLAYER_RUN);
                    break;
                
                // Set player's running animations if it's not moving
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
        if (_wallJump && (_grounded || Time.time - _lastTimeOnWall > 0.5f))
        {
            _wallJump = false;
        }

        switch (_grounded)
        {
            // Add a vertical force to the player (JUMP)
            case true when jump && !_onLeftWall && !_onRightWall:
                FindObjectOfType<AudioManager>().Play("Jump");
                jumpDust.Play();
                _grounded = false;
                _body.AddForce(new Vector2(0f, jumpForce));
                ChangeAnimationState(PLAYER_JUMP);
                break;
            
            // Add a special vertical force to the player (SUPER JUMP)
            case true when duckMove:
                FindObjectOfType<AudioManager>().Play("Attack");
                _grounded = false;
                _body.AddForce(new Vector2(0f, (float) (jumpForce * 1.5)));
                ChangeAnimationState(PLAYER_ROLL);
                break;
            
            // Attack from air
            case false when crouch:
                _airAttack = true;
                _body.AddForce(new Vector2(0f, -jumpForce));
                break;
            
            // Slide from left wall
            case false when _onLeftWall && !jump:
                leftWallDust.Play();
                _renderer.flipX = false;
                _body.velocity = new Vector2(_body.velocity.x, -wallSlideSpeed);
                if (!_wallJump) ChangeAnimationState(move > 0.01f ? PLAYER_IDLE : PLAYER_SLIDE);
                break;
            
            // Slide from right wall
            case false when _onRightWall && !jump:
                rightWallDust.Play();
                _renderer.flipX = true;
                _body.velocity = new Vector2(_body.velocity.x, -wallSlideSpeed);
                if (!_wallJump) ChangeAnimationState(move < -0.01f ? PLAYER_IDLE : PLAYER_SLIDE);
                break;
            
            // Jump from left wall
            case false when _onLeftWall && jump:
                FindObjectOfType<AudioManager>().Play("Attack");
                _body.AddForce(new Vector2(wallJumpForce, wallJumpForce * 3));
                ChangeAnimationState(PLAYER_ROLL);
                _wallJump = true;
                _lastTimeOnWall = Time.time;
                break;
            
            // Jump from right wall
            case false when _onRightWall && jump:
                FindObjectOfType<AudioManager>().Play("Attack");
                _body.AddForce(new Vector2(-wallJumpForce, wallJumpForce * 3));
                ChangeAnimationState(PLAYER_ROLL);
                _wallJump = true;
                _lastTimeOnWall = Time.time;
                break;
        }

        // Let player jump over bridges
        Physics2D.IgnoreLayerCollision(6, 8, _body.velocity.y > 0.0f);
        
        // Dodge bullets if crouching
        Physics2D.IgnoreLayerCollision(6, 13, crouch);
        
        // Disable air attack if player is on the ground
        if (_grounded && _airAttack)
        {
            // Shake camera
            impulseSource.GenerateImpulse();
            
            // Play stomp sound
            FindObjectOfType<AudioManager>().Play("Stomp");
            
            // Play dust effect
            duckDust.Play();
            
            // Disable air attack
            _airAttack = false;
        }
    }
    
    // Flip player to match movement direction
    private void Flip(float movementDirection)
    {
        if (movementDirection > 0 && _renderer.flipX)
        {
            // Play dust effect
            jumpDust.Play();
            
            // Flip player
            _renderer.flipX = false;
        }
        else if (movementDirection < 0 && !_renderer.flipX)
        {
            // Play dust effect
            jumpDust.Play();
            
            // Flip player
            _renderer.flipX = true;
        }
    }
    
    // Teleport player back when it goes out of bounds
    private void Teleport()
    {
        if (transform.position.x > 14.5f)
        {
            transform.position = new Vector3(-14.5f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -14.5f)
        {
            transform.position = new Vector3(14.5f, transform.position.y, transform.position.z);
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

    public bool AirAttack()
    {
        return _airAttack;
    }

    public bool IsGrounded()
    {
        return _grounded;
    }
}
