using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinDasherAbilityScript : MonoBehaviour
{
    public void RocketJump()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 20.0f, ForceMode2D.Impulse);
    }
}
