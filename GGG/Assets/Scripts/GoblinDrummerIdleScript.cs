using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinDrummerIdleScript : MonoBehaviour
{
    public Sprite SpriteFacingRight;
    public Sprite SpriteFacingLeft;

    private bool _facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LookAround(Random.Range(0.0f,2.0f)));
    }

    IEnumerator LookAround(float delay)
    {
        yield return new WaitForSeconds(delay);

        _facingRight = !_facingRight;
        GetComponent<SpriteRenderer>().sprite = _facingRight ? SpriteFacingRight : SpriteFacingLeft;
        
        StartCoroutine(LookAround(Random.Range(6.0f,15.0f)));
    }

}
