using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 12f;
    public CharacterController2D controller;
    private float _horizontalMove;
    private bool _jump;
    private bool _crouch;

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
    }

    private void FixedUpdate()
    {
        controller.Move(_horizontalMove * Time.fixedDeltaTime, _jump, _crouch);
        _jump = false;
    }
}