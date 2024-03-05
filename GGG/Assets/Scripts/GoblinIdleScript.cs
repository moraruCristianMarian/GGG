using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinIdleScript : MonoBehaviour
{
    float newPos = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
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
                newPos = Random.Range(-0.25f, transform.localPosition.x-0.05f);
            else
                newPos = Random.Range(transform.localPosition.x+0.05f, 0.25f);

            newPos = Mathf.Clamp(newPos, -0.25f, 0.25f);
        }

        StartCoroutine(IdleAround(Random.Range(2.0f,6.0f)));
    }

    void FixedUpdate()
    {
        if ((newPos != 0.0f) && (Mathf.Abs(transform.localPosition.x - newPos) > 0.02f))
        {
            if (transform.localPosition.x - newPos > 0.0f)
                transform.position -= Vector3.right * 0.005f;
            else
                transform.position += Vector3.right * 0.005f;
        }

        Vector3 freezeY = transform.localPosition;
        freezeY.y = -0.1f;
        transform.localPosition = freezeY;
    }
}
