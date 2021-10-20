using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 600f;
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
        var horizontalInput = Input.GetAxis("Horizontal");
        
        // Flip player
        Flip(horizontalInput);

        // Horizontal movement
        _body.velocity = new Vector2(horizontalInput * speed, _body.velocity.y);

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