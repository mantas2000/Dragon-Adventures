using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private SpriteRenderer _renderer;
    private Animator _animator;
    private bool grounded;
    
    // Start is called before the first frame update
    void Start()
    {
        // Grab references
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // Player flipping
        if (horizontalInput > 0)
        {
            _renderer.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            _renderer.flipX = true;
        }
        
        // Horizontal movement
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Vertical movement
        if (Input.GetKey(KeyCode.Space) && grounded) Jump();

        // Set Animator parameters
        _animator.SetBool("Walk", horizontalInput != 0);
        _animator.SetBool("Grounded", grounded);
    }

    // Jumping animation
    private void Jump()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        
        body.velocity = new Vector2(body.velocity.x, speed);
        _animator.SetTrigger("Jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) grounded = true;
    }
}