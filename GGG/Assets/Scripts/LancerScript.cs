using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerScript : MonoBehaviour
{
    private Animator _animator;
    public bool Marching = false;
    public bool MarchOnStart = false;
    public float MarchSpeed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.HasCustomTag("FramePiece"))
        {
            col.gameObject.GetComponent<FramePieceScript>().DestroyPiece(true);
            // Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (Marching)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * MarchSpeed, ForceMode2D.Force);
            _animator.SetBool("Walking", true);
        }
    }
}
