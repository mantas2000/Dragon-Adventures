using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 12f;
    public CharacterController2D controller;
    private float _horizontalMove;
    private bool _jump;
    private bool _crouch;
    private bool _hitMove;
    private float _lastLeftTapTime;
    private float _lastRightTapTime;

    private void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * walkSpeed;

        if (Input.GetKeyDown(KeyCode.Space)) _jump = true;

        if (Input.GetButtonDown("Crouch")) _crouch = true;
        
        else if (Input.GetButtonUp("Crouch")) _crouch = false;
        
        // Restart level
        if (Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        // Monitor left arrow clicks for hit move activation
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            if (Time.time - _lastLeftTapTime < 0.5f) _hitMove = true;

            _lastLeftTapTime = Time.time;
        }
        
        // Monitor right arrow clicks for hit move activation
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            if (Time.time - _lastRightTapTime < 0.5f) _hitMove = true;

                _lastRightTapTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(_horizontalMove * Time.fixedDeltaTime, _jump, _crouch, _hitMove);
        _jump = false;
        _hitMove = false;
    }
}