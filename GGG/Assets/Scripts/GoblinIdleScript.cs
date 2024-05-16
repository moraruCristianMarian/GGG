using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinIdleScript : MonoBehaviour
{
    public float FreezeY = -0.1f;
    public float RoamDist = 1.0f;
    private float _newPos = 0.0f;
    private Animator _animator;
    private bool _walking = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        StartCoroutine(IdleAround(Random.Range(0.0f,2.0f)));
    }

    IEnumerator IdleAround(float delay)
    {
        yield return new WaitForSeconds(delay);

        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;

        bool willIMove = (Random.Range(0.0f, 1.0f) > 0.4f);
        if (willIMove)
        {
            if (GetComponent<SpriteRenderer>().flipX)
                _newPos = Random.Range(-0.25f*RoamDist, transform.localPosition.x-0.05f);
            else
                _newPos = Random.Range(transform.localPosition.x+0.05f, 0.25f*RoamDist);

            _newPos = Mathf.Clamp(_newPos, -0.25f*RoamDist, 0.25f*RoamDist);

            _walking = true;
            _animator.SetBool("Walking", true);
        }

        StartCoroutine(IdleAround(Random.Range(2.0f,6.0f)));
    }

    void FixedUpdate()
    {
        float walkLimit = transform.localPosition.x - _newPos;
        if ((transform.TransformDirection(Vector3.left)).x > 0)
            walkLimit *= -1;

        if ((_newPos != 0.0f) && (Mathf.Abs(walkLimit) > 0.02f))
        {
            if (walkLimit > 0.0f)
                transform.position -= Vector3.right * 0.005f;
            else
                transform.position += Vector3.right * 0.005f;
        }
        else
        {
            if (_walking)
            {
                _walking = false;
                _animator.SetBool("Walking", false);
            }
        }

        Vector3 freezeY = transform.localPosition;
        freezeY.y = FreezeY;
        transform.localPosition = freezeY;
    }
}
