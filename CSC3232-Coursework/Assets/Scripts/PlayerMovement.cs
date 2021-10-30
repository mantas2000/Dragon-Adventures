using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 15f;
    public CharacterController2D controller;
    private Animator _animator;
    private float _horizontalMove;
    private bool _jump;
    private bool _crouch;
    private bool _hitMove;
    private bool _crouchMove;
    private float _lastLeftTapTime;
    private float _lastRightTapTime;
    private float _holdTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _lastLeftTapTime = -Time.time;
        _lastRightTapTime = -Time.time;
        _holdTime = 0;
    }

    private void Update()
    {
        // Horizontal movement input
        _horizontalMove = Input.GetAxisRaw("Horizontal") * walkSpeed;

        // Check space bar input
        if (Input.GetKeyDown(KeyCode.Space)) _jump = true;
        
        // Check if crouch is selected
        if (Input.GetKeyDown(KeyCode.DownArrow)) _crouch = true;

        // Track for how long crouch is pressed down
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _holdTime += Time.deltaTime;

            // Check if crouching special move is available
            if (_holdTime > 1f)
            {
                _animator.Play("Player_Crouch_Move");
                
                // Activate crouching special move
                if (_jump)
                {
                    _crouchMove = true;
                    _crouch = false;
                    _holdTime = 0;
                }
            }
            
            // Disable jumping
            _jump = false;
        }
        
        // Crouching was cancelled
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            _crouch = false;
            _crouchMove = false;
            _holdTime = 0;
        }

        // Restart level
        if (Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        // Monitor left arrow clicks for hit move activation
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            if (Time.time - _lastLeftTapTime < 0.25f)
            {
                _hitMove = true;
                _animator.Play("Player_Roll");
            }

            _lastLeftTapTime = Time.time;
        }
        
        // Monitor right arrow clicks for hit move activation
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            if (Time.time - _lastRightTapTime < 0.25f)
            {
                _hitMove = true;
                _animator.Play("Player_Roll");
            }

            _lastRightTapTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(_horizontalMove * Time.fixedDeltaTime, _jump, _crouch, _hitMove, _crouchMove);
        _jump = false;
        _hitMove = false;
        _crouchMove = false;
    }
}