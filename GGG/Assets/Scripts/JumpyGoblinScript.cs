using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpyGoblinScript : MonoBehaviour
{
    public GoblinIdleScript IdleScript;
    private Rigidbody2D _rb;
    private float _anim;
    private bool _groundCheck = false;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _anim += Time.deltaTime * 9;
        if (_anim > 628)
            _anim -= 628;
        IdleScript.FreezeY = Mathf.Sin(_anim) * 0.2f + 0.1f;
    }

    public void Jump()
    {
        if (_groundCheck)
            _rb.AddForce(new Vector2(0f, 20f), ForceMode2D.Impulse);
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
