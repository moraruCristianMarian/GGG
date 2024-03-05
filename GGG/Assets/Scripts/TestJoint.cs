using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void JoinToCenterPiece()
    {
        GameObject centerPiece = GameObject.Find("CenterPiece");

        if (centerPiece)
        {
            Rigidbody2D centerRigidbody = centerPiece.GetComponent<Rigidbody2D>();

            if (centerRigidbody)
            {
                transform.SetParent(centerRigidbody.transform);

                FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                joint.connectedBody = centerRigidbody;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            //GameObject parent = transform.parent.gameObject;
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 20.0f, ForceMode2D.Impulse);

            Debug.Log("Jump");
        }
    }
}
