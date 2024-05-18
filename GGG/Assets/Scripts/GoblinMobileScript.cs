using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMobileScript : MonoBehaviour
{
    public float Speed = 4f;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool _groundCheck = false;
    private bool _walking = false;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if ((!_groundCheck) || (_rb.bodyType != RigidbodyType2D.Dynamic))
        {
            if (_walking)
            {
                _walking = false;
                _animator.SetBool("Walking", false);
            }
            return;
        }

        float moveInput = 0;
        if (Input.GetKey(KeyCode.A))
            moveInput -= 1;
        if (Input.GetKey(KeyCode.D))
            moveInput += 1;

        float moveAmount = moveInput * Speed;

        Vector2 newVelocity = new Vector2(moveAmount, _rb.velocity.y);
        _rb.velocity = newVelocity;

        //  Update animation
        if (moveInput != 0)
        {
            if (!_walking)
            {
                _walking = true;
                _animator.SetBool("Walking", true);
            }
            _spriteRenderer.flipX = (moveInput != 0) ? (moveInput < 0) : _spriteRenderer.flipX;
        }
        else
        {
            if (_walking)
            {
                _walking = false;
                _animator.SetBool("Walking", false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
            _groundCheck = true;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
            _groundCheck = false;
    }
}
