using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinDasherAbilityScript : MonoBehaviour
{
    public Quaternion DashQuaternion;

    void Start()
    {
        DashQuaternion = Quaternion.identity;
    }

    public void RocketJump(int strengthMultiplier = 10)
    {
        Vector2 dashDirection = DashQuaternion * Vector2.right;
        
        gameObject.GetComponent<Rigidbody2D>().AddForce(dashDirection * strengthMultiplier, ForceMode2D.Impulse);
    }
}
