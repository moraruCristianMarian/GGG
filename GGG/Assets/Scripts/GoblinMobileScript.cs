using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMobileScript : MonoBehaviour
{
    public float Speed = 400.0f;
    private Rigidbody2D _rb;
    private bool _groundCheck = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if ((!_groundCheck) || (_rb.bodyType != RigidbodyType2D.Dynamic))
            return;

        float moveInput = 0;
        if (Input.GetKey(KeyCode.A))
            moveInput -= 1;
        if (Input.GetKey(KeyCode.D))
            moveInput += 1;

        float moveAmount = moveInput * Speed * Time.deltaTime;

        Vector2 newVelocity = new Vector2(moveAmount, _rb.velocity.y);
        _rb.velocity = newVelocity;
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
